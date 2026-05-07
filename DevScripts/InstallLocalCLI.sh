#!/usr/bin/env bash

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

cd "$SCRIPT_DIR" || exit 1
cd ..

dotnet tool uninstall -g GUnit.CLI
dotnet pack -c Release
dotnet tool install --global GUnit.CLI --add-source ./GUnit.CLI/bin/Release
