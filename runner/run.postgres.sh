#!/usr/bin/env bash
set -euo pipefail
SCRIPT_ROOT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

docker run --name some-postgres -p 5432:5432 -v ~/VMData/some-postgres:/var/lib/postgresql/data -e POSTGRES_PASSWORD=mysecretpassword -d postgres -c 'shared_buffers=512MB' -c 'wal_buffers=64MB' -c 'maintenance_work_mem=256MB' -c 'max_connections=200'