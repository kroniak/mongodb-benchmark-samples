#!/usr/bin/env bash
set -euo pipefail
SCRIPT_ROOT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

docker run --name some-postgres -p 5432:5432 -v ~/VMData/some-postgres:/var/lib/postgresql/data -e POSTGRES_PASSWORD=mysecretpassword -d postgres