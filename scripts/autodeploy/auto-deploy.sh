#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_DIR="${AUTODEPLOY_REPO_DIR:-$(cd "$SCRIPT_DIR/../.." && pwd)}"
LOCK_FILE="/tmp/filistin-autodeploy.lock"
HEALTH_URL="${AUTODEPLOY_HEALTH_URL:-http://127.0.0.1:80/health/ready}"
HEALTH_TIMEOUT="${AUTODEPLOY_HEALTH_TIMEOUT:-180}"
BRANCH="main"

log() { echo "[$(date '+%Y-%m-%d %H:%M:%S')] $*"; }

exec 9>"$LOCK_FILE"
if ! flock -n 9; then
  log "Another deploy instance is running, skipping this tick."
  exit 0
fi

cd "$REPO_DIR"

if ! command -v docker >/dev/null 2>&1; then
  log "ERROR: docker not found in PATH."
  exit 1
fi

export GIT_TERMINAL_PROMPT=0
if ! git fetch --quiet origin "$BRANCH"; then
  log "FETCH FAILED (network or git credentials). Will retry on next tick."
  exit 1
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
  exit 1
fi

log "Pulled $(git rev-parse --short HEAD): $(git log -1 --pretty=%s)"

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
  log "DEPLOY SUCCESS: $(git rev-parse --short HEAD) is live."
  exit 0
fi

log "DEPLOY FAILED ($FAIL_REASON) - rolling back to ${LOCAL_SHA:0:7}..."
git reset --hard "$LOCAL_SHA"
if deploy && wait_healthy; then
  log "ROLLBACK SUCCESS: previous version restored. Fix the new commit and push again."
else
  log "ROLLBACK ALSO FAILED - manual intervention required!"
fi
exit 1
