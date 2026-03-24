#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "Starting Order API on http://localhost:5100..."
dotnet run --project "$SCRIPT_DIR/CompileAndSip.OrderApi" &
API_PID=$!
sleep 3

echo "Starting Kiosk TUI..."
dotnet run --project "$SCRIPT_DIR/CompileAndSip.KioskTui" &
KIOSK_PID=$!

echo "Starting Kitchen Display..."
dotnet run --project "$SCRIPT_DIR/CompileAndSip.KitchenDisplay" &
KDS_PID=$!

echo ""
echo "All apps started. Press Ctrl+C to stop."
trap "kill $API_PID $KIOSK_PID $KDS_PID 2>/dev/null" EXIT
wait
