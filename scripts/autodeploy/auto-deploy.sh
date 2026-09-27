#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_DIR="${AUTODEPLOY_REPO_DIR:-$(cd "$SCRIPT_DIR/../.." && pwd)}"
LOCK_FILE="/tmp/filistin-autodeploy.lock"
HEALTH_URL="${AUTODEPLOY_HEALTH_URL:-http://127.0.0.1:80/health/ready}"
HEALTH_TIMEOUT="${AUTODEPLOY_HEALTH_TIMEOUT:-180}"
BRANCH="main"
TELEGRAM_BOT_TOKEN="${TELEGRAM_BOT_TOKEN:-}"
TELEGRAM_CHAT_ID="${TELEGRAM_CHAT_ID:-}"
STATE_DIR="/tmp"

log() { echo "[$(date '+%Y-%m-%d %H:%M:%S')] $*"; }

html_escape() {
  local s="$1"
  s="${s//&/&amp;}"
  s="${s//</&lt;}"
  s="${s//>/&gt;}"
  printf '%s' "$s"
}

notify() {
  [ -n "$TELEGRAM_BOT_TOKEN" ] && [ -n "$TELEGRAM_CHAT_ID" ] || return 0
  curl -s --max-time 10 -o /dev/null \
    --data-urlencode "chat_id=$TELEGRAM_CHAT_ID" \
    --data-urlencode "parse_mode=HTML" \
    --data-urlencode "text=$1" \
    "https://api.telegram.org/bot${TELEGRAM_BOT_TOKEN}/sendMessage" || true
}

notify_state() {
  local key="$1"
  shift
  if [ ! -f "$STATE_DIR/filistin-autodeploy.$key" ]; then
    notify "$@"
    touch "$STATE_DIR/filistin-autodeploy.$key"
  fi
}

exec 9>"$LOCK_FILE"
if ! flock -n 9; then
  log "Another deploy instance is running, skipping this tick."
  exit 0
fi

cd "$REPO_DIR"

export GIT_TERMINAL_PROMPT=0
if ! git fetch --quiet origin "$BRANCH"; then
  log "FETCH FAILED (network or git credentials). Will retry on next tick."
  notify_state "fetchfail" "⚠️ <b>GitHub'a ulaşılamıyor</b>
git fetch başarısız — yeni deploy yapılamıyor.
Bağlantı normale dönünce otomatik tekrar denenecek."
  exit 1
fi

if [ -f "$STATE_DIR/filistin-autodeploy.fetchfail" ]; then
  rm -f "$STATE_DIR/filistin-autodeploy.fetchfail"
  notify "✅ <b>GitHub bağlantısı normale döndü</b>"
fi

LOCAL_SHA="$(git rev-parse HEAD)"
REMOTE_SHA="$(git rev-parse "origin/$BRANCH")"

if [ "$LOCAL_SHA" = "$REMOTE_SHA" ]; then
  exit 0
fi

log "New commit detected: ${LOCAL_SHA:0:7} -> ${REMOTE_SHA:0:7}"
log "Repository: $REPO_DIR"

if ! git pull --ff-only origin "$BRANCH"; then
  log "PULL FAILED: local branch diverged from origin/$BRANCH. Manual intervention required."
  notify_state "diverged" "⚠️ <b>Pull başarısız — yerel commit var</b>
Sunucuda yerel commit bulundu, deploy durduruldu.
Manuel müdahale gerekli: git log --oneline -3"
  exit 1
fi

rm -f "$STATE_DIR/filistin-autodeploy.diverged"

PULL_SUBJECT="$(git log -1 --pretty=%s)"
PULL_SHORT="$(git rev-parse --short HEAD)"
DEPLOY_START="$(date +%s)"

log "Pulled $PULL_SHORT: $PULL_SUBJECT"

if ! command -v docker >/dev/null 2>&1; then
  log "ERROR: docker not found in PATH."
  notify "❌ <b>Deploy başarısız</b>
Sebep: docker PATH'te bulunamadı"
  exit 1
fi

notify "🚀 <b>Deploy başladı</b>
Commit: <code>$(html_escape "$PULL_SHORT")</code> — $(html_escape "$PULL_SUBJECT")
Health check: $(html_escape "$HEALTH_URL")"

deploy() {
  log "Building Docker image..."
  docker compose build web || return 1
  log "Starting container..."
  docker compose up -d web || return 1
}

wait_healthy() {
  local waited=0
  local code="000"
  while [ "$waited" -lt "$HEALTH_TIMEOUT" ]; do
    code="$(curl -s -o /dev/null -w '%{http_code}' --max-time 5 "$HEALTH_URL")" || code="000"
    if [ "$code" = "200" ]; then
      log "Health check OK (200) after ${waited}s."
      return 0
    fi
    sleep 3
    waited=$((waited + 3))
  done
  log "Health check FAILED: no 200 from $HEALTH_URL within ${HEALTH_TIMEOUT}s (last code: $code)."
  return 1
}

FAIL_REASON=""
if ! deploy; then
  FAIL_REASON="build/start failed"
elif ! wait_healthy; then
  FAIL_REASON="health check failed"
fi

if [ -z "$FAIL_REASON" ]; then
  ELAPSED=$(( $(date +%s) - DEPLOY_START ))
  log "DEPLOY SUCCESS: $(git rev-parse --short HEAD) is live."
  notify "✅ <b>Site ayakta</b>
Commit: <code>$(html_escape "$PULL_SHORT")</code>
Toplam süre: <b>${ELAPSED}s</b>
Health check: 200 OK"
  exit 0
fi

log "DEPLOY FAILED ($FAIL_REASON) - rolling back to ${LOCAL_SHA:0:7}..."
notify "❌ <b>Deploy başarısız</b>
Commit: <code>$(html_escape "$PULL_SHORT")</code>
Sebep: $(html_escape "$FAIL_REASON")
Eski sürüme rollback yapılıyor..."

git reset --hard "$LOCAL_SHA"
if deploy && wait_healthy; then
  log "ROLLBACK SUCCESS: previous version restored. Fix the new commit and push again."
  notify "↩️ <b>Rollback başarılı</b>
Eski sürüm <code>$(html_escape "${LOCAL_SHA:0:7}")</code> yayında.
Yeni commit düzeltilip tekrar push edilmeli."
else
  log "ROLLBACK ALSO FAILED - manual intervention required!"
  notify "🚨 <b>Rollback de başarısız — manuel müdahale gerekli!</b>
Sunucuya bağlan ve logları incele:
journalctl -u filistin-autodeploy.service -n 100"
fi
exit 1
