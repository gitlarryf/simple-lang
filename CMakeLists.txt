cmake_minimum_required(VERSION 3.1)
project("neon")

if (CMAKE_HOST_SYSTEM_NAME STREQUAL posix)
    message(WARNING "Running on a POSIX system.")
    set(TARGET_SYSTEM_POSIX TRUE)
elseif (CMAKE_HOST_SYSTEM_NAME STREQUAL "linux")
    message(WARNING "Running on a LINUXsystem.")
    set(TARGET_SYSTEM_UNIX  TRUE)
elseif (CMAKE_HOST_SYSTEM_NAME STREQUAL "nt")
    message(WARNING "Running on a WIN32 system.")
    set(TARGET_SYSTEM_WIN32 TRUE)
elseif (CMAKE_HOST_SYSTEM_NAME STREQUAL "darwin")
    message(WARNING "Running on a DARWIN system.")
    set(TARGET_SYSTEM_OSX   TRUE)
endif ()

option(MINIMAL      "Build the minimal components for the Neon VM."     OFF)
option(RELEASE      "Build in release mode, instead of debug."          OFF)
option(COVERAGE     "Enable Code coverage."                             OFF)
option(USE_CURSES   "Use the Curses library."                           ON)
option(USE_SDL      "Use the SDL library."                              ON)
option(USE_SODIUM   "Use the Sodium library."                           ON)
option(USE_BZ2      "Use the BZip v2 library."                          ON)
option(USE_LZMA     "Use the LZMA library."                             ON)
option(USE_CURL     "Use the cURL library."                             ON)
option(USE_PCRE     "Use the PCRE library."                             ON)
option(USE_SQLITE   "Use the SQLite library."                           ON)

if (MINIMAL)
    set(USE_CURSES  OFF)
    set(USE_SODIUM  OFF)
    set(USE_CURL    OFF)
    set(USE_PCRE    OFF)
endif (MINIMAL)

if (TARGET_SYSTEM_UNIX)
    set(CMAKE_CXX_FLAGS     "${CMAKE_CXX_FLAGS} -Wall -std=c++0x -Wextra -Weffc++ -Werror")
    if (RELEASE)
        set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -O3")
    else ()
        set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -g")
    endif (RELEASE)
elseif (TARGET_SYSTEM_WIN32)
    set(CMAKE_CXX_FLAGS     "${CMAKE_CXX_FLAGS} /EHsc /W4 /WX /FS")
    if (RELEASE)
        set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /MTd /Zi /Od")
        set(CMAKE_LINK_FLAGS "${CMAKE_CXX_FLAGS} /DEBUG")
    else ()
        set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /Ox /MT")
    endif (RELEASE)
endif ()

if (COVERAGE)
    set(CMAKE_CXX_FLAGS         "${CMAKE_CXX_FLAGS} --coverage -g -O0")
    set(CMAKE_EXE_LINKER_FLAGS  "${CMAKE_EXE_LINKER_FLAGS} --coverage -g -O0")
endif ()

include_directories(external/IntelRDFPMathLib20U1/LIBRARY/src
                    external/hash-library
                    external/lib/libffi-3.2.1/include
                    external/minijson_writer-master
                    external/minizip11
                    external/libsodium-1.0.5/src/libsodium/include
                    external/sqlite-amalgamation-3080803
                    external/utfcpp-master/source
                    external/zlib-1.2.8
                    src
)

link_directories(external/lib/
                 external/bzip2-1.0.6/
                 external/hash-library/
                 external/curl-7.41.0/lib/.libs/
                 external/IntelRDFPMathLib20U1/LIBRARY/
                 external/libsodium-1.0.5/src/libsodium/.libs
                 external/pcre2-10.10/.libs/
                 external/libressl-2.2.4/ssl/.libs/
                 external/libressl-2.2.4/tls/.libs/
                 external/libressl-2.2.4/crypto/.libs/
                 external/xz-5.2.1/src/liblzma/.libs/
                 external/easysid-version-1.0/
                 external/SDL2-2.0.3/build/.libs/
                 external/libffi-3.2.1/i686-pc-linux-gnu/.libs/
)

if (USE_CURESES)
    set(rtl_const   "${rtl_const} lib/curses_const.cpp")
    set(rtl_cpp     "${rtl_cpp}   lib/curses.cpp")
endif ()
if (USE_SDL)
    set(rtl_const   "${rtl_const} lib/sdl_const.cpp")
    set(rtl_cpp     "${rtl_cpp}   lib/sdl.cpp")
endif ()
if (USE_SODIUM)
    set(rtl_const   "${rtl_const} lib/sodium_const.cpp")
    set(rtl_cpp     "${rtl_cpp}   lib/sodium.cpp")
endif ()
if (TARGET_SYSTEM_UNIX)
    set(rtl_const   "${rtl_const} lib/file_const_posix.cpp")
elseif (TARGET_SYSTEM_WIN32)
    set(rtl_const   "${rtl_const} lib/file_const_win32.cpp")
endif ()
if (USE_BZ2 AND USE_LZMA)
    set(rtl_cpp     "${rtl_cpp}   lib/compress.cpp")
endif ()
if (USE_CURL)
    set(rtl_cpp     "${rtl_cpp}   lib/http.cpp")
endif (USE_CURL)
if (USE_POSIX)    
    set(rtl_cpp     "${rtl_cpp}   lib/posix.cpp
                    lib/runtime.cpp
                    lib/file_posix.cpp
                    lib/mmap_posix.cpp
                    lib/os_posix.cpp
                    lib/process_posix.cpp
                    lib/time_posix.cpp")
endif (USE_POSIX)
if (USE_PCRE)
    set(rtl_cpp     "${rtl_cpp}   lib/regex.cpp")
endif (USE_PCRE)
if (USE_SQLITE)
    set(rtl_cpp     "${rtl_cpp}   lib/sqlite.cpp")
endif (USE_SQLITE)
if (TARGET_SYSTEM_DARWIN)
    set(rtl_cpp     "${rtl_cpp}   lib/time_darwin.cpp")
endif (TARGET_SYSTEM_DARWIN)
if (TARGET_SYSTEM_LINUX)
    set(rtl_cpp     "${rtl_cpp}   lib/time_linux.cpp")
endif (TARGET_SYSTEM_LINUX)
if (TARGET_SYSTEM_WIN32)
    set(rtl_cpp     "${rtl_cpp}   lib/file_win32.cpp"
                    "lib/mmap_win32.cpp"
                    "lib/os_win32.cpp"
                    "lib/process_win32.cpp"
                    "lib/time_win32.cpp")
endif (TARGET_SYSTEM_WIN32)

message(WARNING ${rtl_const})
message(WARNING ${rtl_cpp})

add_executable(neonx
    src/bundle.cpp
    src/bytecode.cpp
    src/cell.cpp
    src/exec.cpp
    src/format.cpp
    src/httpserver.cpp
    src/intrinsic.cpp
    src/neonx.cpp
    src/number.cpp
    src/rtl_exec.cpp
    src/support.cpp
    ${rtl_const}
    ${rtl_cpp}
)

target_link_libraries(neonx libbid.a
                            libbz2.a
                            bz2.a
                            libcurl.so
                            libcurses.so
                            libeasysid.so
                            libffi.so
                            libhash-library.a
                            liblzma.a
                            libpcre2-8.a
                            libSDL2.so
                            libsodium.a
)                        

