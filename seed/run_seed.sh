#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
SEED_FILE="${SCRIPT_DIR}/seed_data.cypher"

echo "=========================================="
echo "EquityGraph CognoDB Seeder"
echo "=========================================="

# Check if cypher-shell is installed
if ! command -v cypher-shell &> /dev/null; then
    echo "Error: 'cypher-shell' command not found."
    echo ""
    echo "To install cypher-shell:"
    echo "  - Via Neo4j Desktop / Cypher Shell CLI: https://neo4j.com/deployment-center/"
    echo "  - Via Homebrew (macOS/Linux): brew install cypher-shell"
    echo "  - Via apt (Debian/Ubuntu): sudo apt-get install cypher-shell"
    echo "  - Via Chocolatey (Windows): choco install cypher-shell"
    echo ""
    exit 1
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

if [ ! -f "${SEED_FILE}" ]; then
    echo "Error: Seed file not found at ${SEED_FILE}"
    exit 1
fi

echo "Connecting to CognoDB at: ${COGNODB_URI}"
echo "Applying seed data from: ${SEED_FILE}"

cypher-shell \
    -a "${COGNODB_URI}" \
    -u "${COGNODB_USERNAME}" \
    -p "${COGNODB_PASSWORD}" \
    -f "${SEED_FILE}"

if [ $? -eq 0 ]; then
    echo "=========================================="
    echo "Seed data applied successfully!"
    echo "=========================================="
else
    echo "=========================================="
    echo "Failed to apply seed data."
    echo "=========================================="
    exit 1
fi
