#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "=========================================="
echo "EquityGraph CognoDB Seeder"
echo "=========================================="

# Load .env if present in repo root
if [ -f "${SCRIPT_DIR}/../.env" ]; then
    set -a
    . "${SCRIPT_DIR}/../.env"
    set +a
fi

# Verify required environment variables
if [ -z "${COGNODB_URI}" ] || [ -z "${COGNODB_USERNAME}" ] || [ -z "${COGNODB_PASSWORD}" ]; then
    echo "Error: Required environment variables are missing."
    echo "Please ensure the following environment variables are set:"
    echo "  COGNODB_URI      (e.g., bolt+s://db-509458f8.databases.cognodb.com)"
    echo "  COGNODB_USERNAME (e.g., cognodb)"
    echo "  COGNODB_PASSWORD"
    echo ""
    echo "Example usage:"
    echo "  export COGNODB_URI=\"bolt+s://db-509458f8.databases.cognodb.com\""
    echo "  export COGNODB_USERNAME=\"your_username\""
    echo "  export COGNODB_PASSWORD=\"your_password\""
    echo "  ./seed/run_seed.sh"
    exit 1
fi

dotnet run --project "$(dirname "$0")/SeedRunner"
