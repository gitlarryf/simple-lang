cmake_minimum_required(VERSION 3.1)
#project("neon")

string(TOLOWER CMAKE_HOST_SYSTEM_NAME _type)
if (_type STREQUAL posix)

endif()

if (CMAKE_HOST_SYSTEM_NAME STREQUAL Posix)
    message(ERROR "Running on a POSIX system.")
    set(TARGET_SYSTEM_POSIX TRUE)
elseif (_type STREQUAL "linux")
    message(ERROR "Running on a LINUXsystem.")
    set(TARGET_SYSTEM_UNIX  TRUE)
elseif (CMAKE_HOST_SYSTEM_NAME STREQUAL "nt")
    message(ERROR "Running on a WIN32 system.")
    set(TARGET_SYSTEM_WIN32 TRUE)
elseif (CMAKE_HOST_SYSTEM_NAME STREQUAL "Darwin")
    message(ERROR "Running on a DARWIN system.")
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
    set(CMAKE_CXX_FLAGS     "${CMAKE_CXX_FLAGS} -Wall -Wextra -Weffc++ -Werror")
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

if (CMAKE_CROSSCOMPILING)
    set(CMAKE_CXX_FLAGS     "${CMAKE_CXX_FLAGS} -std=gnu++0x")
else ()
    set(CMAKE_CXX_FLAGS     "${CMAKE_CXX_FLAGS} -std=c++0x")
endif ()


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
)

link_directories(external/lib/
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
)

add_executable(neonx
     lib/binary.cpp
     lib/compress.cpp
     lib/curses.cpp
     lib/curses.cpp
     lib/curses_const.cpp
     lib/datetime.cpp
     lib/debugger.cpp
     lib/file.cpp
     lib/file_const_posix.cpp
     lib/file_posix.cpp
     lib/global.cpp
     lib/hash.cpp
     lib/http.cpp
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
#     lib/sdl.cpp
#     lib/sdl_const.cpp
#     lib/sodium.cpp
#     lib/sodium_const.cpp
#     lib/sqlite.cpp  
)

message($<JOIN:$<TARGET_LINKER_FILE:neonx>, -I>)

#target_link_libraries(neonx
#                         libbz2.so.1.0.6
#                         libbid.a
#                         libbz2.so
#                         libffi.so
#                         liblzma.a
#)
target_link_libraries(neonx rt
                            dl
                            bid
                            bz2
                            lzma
                            libcrypto.a
                            libssl.a
                            z
#                            bz2
                            libcurl.a
                            curses
#                            libeasysid.so
                            ffi
                            hash-library
                            lzma
                            pcre2-8
                            minizip
#                            libSDL2.so
#                            libsodium.a
)                        

