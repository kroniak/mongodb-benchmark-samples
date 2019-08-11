#!/usr/bin/env bash
set -euo pipefail
SCRIPT_ROOT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

docker run --name some-mongo -p 27017:27017 -v ~/VMData/some-mongo/:/data/db -d mongo