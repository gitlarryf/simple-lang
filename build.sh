#! /bin/sh

rm -R CMakeFiles
rm CMakeCache.txt
cmake -DCMAKE_TOOLCHAIN_FILE=external/toolchain.cmake
/opt/timesys/datm3/toolchain/bin/cross_make $*
