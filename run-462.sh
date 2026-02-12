#! /usr/bin/env bash
set -uvx
set -e
cd "$(dirname "$0")"
cwd=`pwd`
ts=`date "+%Y.%m%d.%H%M.%S"`
cd Global.Sys.Demo
dotnet run --project Global.Sys.Demo.csproj --framework net462 "$@"
