#!/usr/bin/env bash
set -euo pipefail
SCRIPT_ROOT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

sudo dotnet run -c Release --project "${SCRIPT_ROOT}/../src"