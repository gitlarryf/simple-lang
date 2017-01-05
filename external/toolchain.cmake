include (CMakeForceCompiler)
# this one is important
SET(CMAKE_SYSTEM_NAME           Generic)
SET(CMAKE_SYSTEM_PROCESSOR      arm5l)
#this one not so much
SET(CMAKE_SYSTEM_VERSION        3)


# specify the cross compiler
SET(CMAKE_C_COMPILER            /opt/timesys/datm3/toolchain/bin/armv5l-timesys-linux-gnueabi-gcc)
SET(CMAKE_CXX_COMPILER          /opt/timesys/datm3/toolchain/bin/armv5l-timesys-linux-gnueabi-g++)
#SET(CMAKE_LINKER                /opt/timesys/datm3/toolchain/bin/armv5l-timesys-linux-gnueabi-ld)
#SET(CMAKE_CXX_LINK_EXECUTABLE   /opt/timesys/datm3/toolchain/bin/armv5l-timesys-linux-gnueabi-ld)
#SET(CMAKE_CXX_LINK_EXECUTABLE  "$<CMAKE_LINKER> $<FLAGS> $<CMAKE_CXX_LINK_FLAGS> $<LINK_FLAGS> $<OBJECTS> -o $<TARGET> $<LINK_LIBRARIES>")
add_definitions(-Dnullptr=NULL -Dalignof=sizeof -Doverride=)
CMAKE_FORCE_C_COMPILER(${CMAKE_C_COMPILER}         TimesysdATM3)
CMAKE_FORCE_CXX_COMPILER(${CMAKE_CXX_COMPILER}     TimesysdATM3)

# where is the target environment
SET(CMAKE_FIND_ROOT_PATH        /opt/timesys/datm3/toolchain/)

# search for programs in the build host directories
SET(CMAKE_FIND_ROOT_PATH_MODE_PROGRAM NEVER)
# for libraries and headers in the target directories
SET(CMAKE_FIND_ROOT_PATH_MODE_LIBRARY ONLY)
SET(CMAKE_FIND_ROOT_PATH_MODE_INCLUDE ONLY)
