#!/usr/bin/env bash
set -euo pipefail

SCRIPT_ROOT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

echo "!!WARNING!! This script for pipeline test running and checking code coverage for netstandard and netcoreapp targets"

dotnet test -c Release /p:CollectCoverage=true /p:Threshold=75 \
/p:Exclude="[NUnit3.*]*" \
"${SCRIPT_ROOT}/../test/Flurl.Test/"