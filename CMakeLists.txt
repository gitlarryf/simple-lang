cmake_minimum_required(VERSION 3.1)
cmake_policy(SET CMP0005 NEW)

project("neon")

set(INSTALL_BIN_DIR "${CMAKE_INSTALL_PREFIX}/bin"       CACHE PATH "Installation directory for executables")
set(INSTALL_LIB_DIR "${CMAKE_INSTALL_PREFIX}/libs"      CACHE PATH "Installation directory for libraries")
set(INSTALL_MAN_DIR "${CMAKE_INSTALL_PREFIX}/docs"      CACHE PATH "Installation directory for documentation")

string(TOLOWER CMAKE_HOST_SYSTEM_NAME _host)
message(STATUS "We are running on a ${_host} OS.")
if (_host STREQUAL Posix)
    message(STATUS "Running on a POSIX system.")
    set(TARGET_SYSTEM_POSIX TRUE)
elseif (_type STREQUAL "linux")
    message(STATUS "Running on a LINUXsystem.")
    set(TARGET_SYSTEM_UNIX  TRUE)
elseif (_host STREQUAL "nt")
    message(STATUS "Running on a WIN32 system.")
    set(TARGET_SYSTEM_WIN32 TRUE)
elseif (_host STREQUAL "Darwin")
    message(STATUS "Running on a DARWIN system.")
    set(TARGET_SYSTEM_OSX   TRUE)
else ()
#    message(FATAL_ERROR "Unknown or unsupported host operating system.")
endif ()

if(MSVC)
    set(CMAKE_DEBUG_POSTFIX "d")
    add_definitions(-D_CRT_SECURE_NO_DEPRECATE)
    add_definitions(-D_CRT_NONSTDC_NO_DEPRECATE)
    include_directories(${CMAKE_CURRENT_SOURCE_DIR})
endif()

if (NOT Win32)
    # external packages
    FIND_PACKAGE(curses)
endif ()

option(MINIMAL      "Build the minimal components for the Neon VM."     OFF)
option(RELEASE      "Build in release mode, instead of debug."          OFF)
option(COVERAGE     "Enable Code coverage."                             OFF)
option(USE_CURSES   "Use the Curses library."                           ON)
option(USE_SDL      "Use the SDL library."                              OFF)
option(USE_SODIUM   "Use the Sodium library."                           OFF)
option(USE_BZ2      "Use the BZip v2 library."                          ON)
option(USE_LZMA     "Use the LZMA library."                             ON)
option(USE_CURL     "Use the cURL library."                             ON)
option(USE_PCRE     "Use the PCRE library."                             ON)
option(USE_SQLITE   "Use the SQLite library."                           OFF)

if (MINIMAL)
    set(USE_CURSES  OFF)
    set(USE_SODIUM  OFF)
    set(USE_CURL    OFF)
    set(USE_PCRE    OFF)
endif (MINIMAL)

# Use the following syntax for inclusion of certain files.
#set(srcs archive.cpp zip.cpp)
#if (LZMA_FOUND)
#  list(APPEND srcs lzma.cpp)
#endif()
#add_library(archive SHARED ${srcs})

if (TARGET_SYSTEM_UNIX)
    set(CMAKE_CXX_FLAGS         "${CMAKE_CXX_FLAGS} -Wall -Wextra -Weffc++ -Werror")
    set(CMAKE_EXE_LINKER_FLAGS  "${CMAKE_EXE_LINKER_FLAGS} -static-libgcc -static-libstdc++")
    if (RELEASE)
        set(CMAKE_CXX_FLAGS     "${CMAKE_CXX_FLAGS} -O3")
    else ()
        set(CMAKE_CXX_FLAGS     "${CMAKE_CXX_FLAGS} -g")
    endif (RELEASE)
elseif (TARGET_SYSTEM_WIN32)
    set(CMAKE_CXX_FLAGS         "${CMAKE_CXX_FLAGS} /EHsc /W4 /WX /FS")
    if (RELEASE)
        set(CMAKE_CXX_FLAGS     "${CMAKE_CXX_FLAGS} /MTd /Zi /Od")
        set(CMAKE_LINK_FLAGS    "${CMAKE_CXX_FLAGS} /DEBUG")
    else ()
        set(CMAKE_CXX_FLAGS     "${CMAKE_CXX_FLAGS} /Ox /MT")
    endif (RELEASE)
endif ()
if (COVERAGE)
    set(CMAKE_CXX_FLAGS         "${CMAKE_CXX_FLAGS} --coverage -g -O0")
    set(CMAKE_EXE_LINKER_FLAGS  "${CMAKE_EXE_LINKER_FLAGS} --coverage -g -O0")
endif ()
if (CMAKE_CROSSCOMPILING)
    set(CMAKE_CXX_FLAGS         "${CMAKE_CXX_FLAGS} -std=gnu++0x")
else ()
    set(CMAKE_CXX_FLAGS         "${CMAKE_CXX_FLAGS} -std=c++0x")
endif ()

## Start the build process...
set(RTL_CPP
    lib/binary.cpp
    lib/datetime.cpp
    lib/debugger.cpp
    lib/global.cpp
    lib/file.cpp
    lib/hash.cpp
    lib/io.cpp
    lib/math.cpp
    lib/net.cpp
    lib/os.cpp
    lib/random.cpp
    lib/runtime.cpp
    lib/string.cpp
    lib/sys.cpp
    lib/time.cpp
)

set(RTL_NEON 
    "lib/binary.neon \
    lib/datetime.neon   \
    lib/debugger.neon   \
    lib/file.neon   \
    lib/global.neon \
    lib/hash.neon   \
    lib/io.neon \
    lib/math.neon   \
    lib/mmap.neon   \
    lib/net.neon    \
    lib/os.neon \
    lib/process.neon    \
    lib/random.neon \
    lib/runtime.neon    \
    lib/string.neon \
    lib/sys.neon    \
    lib/time.neon")
if (USE_CURSES)
    list(APPEND RTL_CONST   lib/curses_const.cpp)
endif (USE_CURSES)
if (USE_SDL)
    list(APPEND RTL_CONST   lib/sdk_const.cpp)
endif (USE_SDL)
if (USE_SODIUM)
    list(APPEND RTL_CONST   lib/sodium_const.cpp)
endif (USE_SODIUM)

if (TARGET_SYSTEM_POSIX)
    list(APPEND RTL_CPP     lib/file_const_posix.cpp
                            lib/file_posix.cpp
                            lib/mmap_posix.cpp
                            lib/os_posix.cpp
                            lib/process_posix.cpp
                            lib/time_posix.cpp)
    set(RTL_PLATFORM        src/rtl_posix.cpp)
elseif (TARGET_SYSTEM_WIN32)
    set(RTL_PLATFORM        src/rtl_win32.cpp)
    list(APPEND RTL_CONST   lib/file_const_win32.cpp)
    list(APPEND RTL_CPP     lib/file_win32.cpp
                            lib/mmap_win32.cpp
                            lib/os_win32.cpp
                            lib/process_win32.cpp
                            lib/time_win32.cpp)
elseif (TARGET_SYSTEM_UNIX)
    list(APPEND RTL_CPP     lib/time_linux.cpp)
elseif (TARGET_SYSTEM_OSX)
    list(APPEND RTL_CPP     lib/time_darwin.cpp)
else ()
    message(ERROR "Unsupported platform: " os.name)
endif ()



if (USE_BZ2 AND USE_LZMA)
    list(APPEND RTL_CPP     lib/compress.cpp)
    set(RTL_NEON   "${RTL_NEON} lib/compress.neon")
endif ()
if (USE_CURSES)
    list(APPEND RTL_CPP     lib/curses.cpp)
    set(RTL_NEON   "${RTL_NEON} lib/curses.neon")
endif ()
if (USE_CURL)
    list(APPEND RTL_CPP     lib/http.cpp)
    set(RTL_NEON    "${RTL_NEON}    lib/http.neon")
endif ()
if (USE_POSIX)
    list(APPEND RTL_CPP     lib/posix.cpp)
    set(RTL_NEON    "$RTL_NEON      ib/posix.neon")
endif ()
if (USE_PCRE)
    list(APPEND RTL_CPP     lib/regex.cpp)
    set(RTL_NEON    "${RTL_NEON}    lib/regex.neon")
endif ()
if (USE_SDL)
    list(APPEND RTL_CPP     lib/sdl.cpp)
    set(RTL_NEON    "${RTL_NEON}    lib/sdl.neon")
endif ()
if (USE_SODIUM)
    list(APPEND RTL_CPP     lib/sodium.cpp)
    set(RTL_NEON    "${RTL_NEON}    lib/sodium.neon")
endif ()
if (USE_SQLITE)
    list(APPEND RTL_CPP     lib/sqlite.cpp)
    set(RTL_NEON    "${RTL_NEON}    lib/sqlite.neon")
endif ()

set(PYTHON python)

message(STATUS "Building thunks...")
add_custom_command(
    OUTPUT  src/thunks.inc
            src/functions_compile.inc
            src/functions_exec.inc
            src/enums.inc
            src/exceptions.inc
            src/constants_compile.inc
    #COMMAND env LC_ALL=C "${NROFF}" ${NROFF_MANOPT}
    #        "${CMAKE_CURRENT_SOURCE_DIR}/src" |
    #        "${PYTHON}" "${CMAKE_CURRENT_SOURCE_DIR} scripts/make_thunks.py ${RTL_NEON}"
    COMMAND "${PYTHON} scripts/make_thunks.py ${RTL_NEON}"
    DEPENDS
        "${CMAKE_CURRENT_SOURCE_DIR}/thunks.inc"
)

#Static start
set(CMAKE_FIND_LIBRARY_SUFFIXES ".a")
#Static Libs

include_directories(external/IntelRDFPMathLib20U1/LIBRARY/src
                    external/bzip2-1.0.6
                    external/curl-7.41.0/include
                    external/hash-library
                    external/lib/libffi-3.2.1/include
                    external/minijson_writer-master
                    external/minizip11
                    external/libsodium-1.0.5/src/libsodium/include
                    external/pcre2-10.10/src
                    external/sqlite-amalgamation-3080803
                    external/utfcpp-master/source
                    external/xz-5.2.1/src/liblzma/api
                    external/zlib-1.2.8
                    src
                    /opt/timesys/datm3/toolchain/include
)

link_directories(external/lib/
                 external/libffi-3.2.1/armv5l-timesys-linux-gnueabi/.libs/
                 external/bzip2-1.0.6/
                 external/hash-library/
                 external/curl-7.41.0/lib/.libs/
                 external/IntelRDFPMathLib20U1/LIBRARY/
                 external/libsodium-1.0.5/src/libsodium/.libs
                 external/pcre2-10.10/.libs/
                 external/minizip11/
                 external/libressl-2.2.4/ssl/.libs/
#                 external/libressl-2.2.4/crypto/.libs/
                 external/xz-5.2.1/src/liblzma/.libs/
                 external/easysid-version-1.0/
                 external/SDL2-2.0.3/build/.libs/
                /opt/timesys/datm3/toolchain/lib
)

add_executable(neonx
     lib/binary.cpp
     lib/compress.cpp
     lib/curses.cpp
     lib/curses_const.cpp
     lib/datetime.cpp
     lib/debugger.cpp
     lib/file.cpp
     lib/file_const_posix.cpp
     lib/file_posix.cpp
     lib/global.cpp
     lib/hash.cpp
     lib/io.cpp
     lib/math.cpp
     lib/mmap_posix.cpp
     lib/net.cpp
     lib/os.cpp
     lib/os_posix.cpp
     lib/posix.cpp
     lib/process_posix.cpp
     lib/random.cpp
     lib/regex.cpp
     lib/regex.cpp
     lib/runtime.cpp
     lib/string.cpp
     lib/sys.cpp
     lib/time.cpp
     lib/time_linux.cpp
     lib/time_posix.cpp
     src/bundle.cpp
     src/bytecode.cpp
     src/cell.cpp
     src/exec.cpp
     src/format.cpp
     src/httpserver.cpp
     src/intrinsic.cpp
     src/neonx.cpp
     src/number.cpp
#     src/rtl_compile.cpp
     src/rtl_exec.cpp
     src/rtl_posix.cpp
     src/support.cpp
     src/stdutl.cpp
)

add_executable(neondis
     src/bytecode.cpp
     src/debuginfo.cpp
     src/disassembler.cpp
     src/neondis.cpp
     src/number.cpp
     src/lexer.cpp
     src/util.cpp
     src/stdutl.cpp
)

add_executable(neonbind
    src/bytecode.cpp
    src/neonbind.cpp
    src/support.cpp
)

set_target_properties(neonx PROPERTIES LINK_SEARCH_START_STATIC 1)

target_link_libraries(neonx rt
                            dl
                            bid
                            libbz2.a
                            minizip
                            lzma
                            libcrypto.a
                            libssl.a
                            z
                            libcurl.a
                            curses
                            ncurses
                            ncursesw
#                            libeasysid.so
                            ffi
                            hash-library
                            lzma
                            pcre2-8
#                            libSDL2.so
#                            libsodium.a
)

target_link_libraries(neondis   rt
                                dl
                                bid
                                ffi
                                hash-library
)

target_link_libraries(neonbind  rt
                                dl
                                bid
                                ffi
                                z
                                minizip
)



set_target_properties(neonx PROPERTIES LINK_SEARCH_END_STATIC 1)

