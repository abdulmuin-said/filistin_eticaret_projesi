#!/usr/bin/env bash
set -euo pipefail

if [ "$(id -u)" -ne 0 ]; then
  echo "ERROR: Run this script with sudo: sudo bash scripts/autodeploy/install.sh" >&2
  exit 1
fi

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_DIR="$(cd "$SCRIPT_DIR/../.." && pwd)"
OWNER_USER="$(stat -c '%U' "$REPO_DIR")"

echo "Repository: $REPO_DIR"
echo "Owner user: $OWNER_USER"

sed -i 's/\r$//' "$SCRIPT_DIR/auto-deploy.sh"
chmod +x "$SCRIPT_DIR/auto-deploy.sh"

sed -e "s|__REPO_DIR__|$REPO_DIR|g" \
    -e "s|__USER__|$OWNER_USER|g" \
    "$SCRIPT_DIR/filistin-autodeploy.service" \
    > /etc/systemd/system/filistin-autodeploy.service

install -m 644 "$SCRIPT_DIR/filistin-autodeploy.timer" /etc/systemd/system/filistin-autodeploy.timer

systemctl daemon-reload
systemctl enable --now filistin-autodeploy.timer

echo
echo "Timer status:"
systemctl list-timers filistin-autodeploy.timer --no-pager
echo
echo "Running first deploy check..."
if systemctl start filistin-autodeploy.service; then
  echo "First check completed."
else
  echo "First check FAILED. Inspect logs: journalctl -u filistin-autodeploy.service -n 50"
fi
echo
echo "Live logs: journalctl -u filistin-autodeploy.service -f"
