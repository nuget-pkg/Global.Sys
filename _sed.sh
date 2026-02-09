#! /usr/bin/env bash
#set -uvx
set -e
cd "$(dirname "$0")"
cwd=`pwd`
ts=`date "+%Y.%m%d.%H%M.%S"`
version="${ts}"

cd $cwd/Global.Sys
sed -i -e "s/<Version>.*<\/Version>/<Version>${version}<\/Version>/g" Global.Sys.csproj
cd $cwd/
echo ${version}>version.txt
