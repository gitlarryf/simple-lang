cmake_minimum_required(VERSION 3.1)
cmake_policy(SET CMP0005 NEW)

project("neon")

set(INSTALL_BIN_DIR "${CMAKE_INSTALL_PREFIX}/bin"       CACHE PATH "Installation directory for executables")
set(INSTALL_LIB_DIR "${CMAKE_INSTALL_PREFIX}/libs"      CACHE PATH "Installation directory for libraries")
set(INSTALL_MAN_DIR "${CMAKE_INSTALL_PREFIX}/docs"      CACHE PATH "Installation directory for documentation")

string(TOLOWER CMAKE_HOST_SYSTEM_NAME _host)
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
endif ()

if(MSVC)
    set(CMAKE_DEBUG_POSTFIX "d")
    add_definitions(-D_CRT_SECURE_NO_DEPRECATE)
    add_definitions(-D_CRT_NONSTDC_NO_DEPRECATE)
    include_directories(${CMAKE_CURRENT_SOURCE_DIR})
endif()

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

if (USE_CURSES)
    list(APPEND RTL_CONST lib/curses_const.cpp)
endif (USE_CURSES)
if (USE_SDL)
    list(APPEND RTL_CONST lib/sdk_const.cpp)
endif (USE_SDL)
if (USE_SODIUM)
    list(APPEND RTL_CONST lib/sodium_const.cpp)
endif (USE_SODIUM)

if (TARGET_SYSTEM_POSIX)
    list(APPEND RTL_CONST lib/file_const_posix.cpp)
elseif (TARGET_SYSTEM_WIN32)
    list(APPEND RTL_CONST lib/file_const_win32.cpp)
else ()
    message(ERROR "Unsupported platform: " os.name)
endif ()

set(RTL_CONST
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

if (USE_BZ2 AND USE_LZMA)
    list(APPEND RTL_CONST lib/compress.cpp)
endif ()
if (USE_CURSES)
    list(APPEND RTL_CONST lib/curses)
endif ()
if (USE_CURL)
    list(APPEND RTL_CONST lib/http.cpp)
endif ()
if (USE_POSIX)
    list(APPEND RTL_CONST lib/posix.cpp)
endif ()
if (USE_PCRE)
    list(APPEND RTL_CONST lib/regex.cpp)
endif ()
if (USE_SDL)
    list(APPEND RTL_CONST lib/sdl.cpp)
endif ()
if (USE_SODIUM)
    list(APPEND RTL_CONST lib/sodium.cpp)
endif ()
if (USE_SQLITE)
    list(APPEND RTL_CONST lib/sqlite.cpp)
endif ()

#rtl_neon = squeeze([
#    "lib/binary.neon",
#    "lib/compress.neon" if use_bz2 and use_lzma else None,
#    "lib/curses.neon" if use_curses else None,
#    "lib/datetime.neon",
#    "lib/debugger.neon",
#    "lib/file.neon",
#    "lib/global.neon",
#    "lib/hash.neon",
#    "lib/http.neon" if use_curl else None,
#    "lib/io.neon",
#    "lib/math.neon",
#    "lib/mmap.neon",
#    "lib/net.neon",
#    "lib/os.neon",
#    "lib/posix.neon" if use_posix else None,
#    "lib/process.neon",
#    "lib/random.neon",
#    "lib/runtime.neon",
#    "lib/regex.neon" if use_pcre else None,
 #   "lib/sdl.neon" if use_sdl else None,
  #  "lib/sodium.neon" if use_sodium else None,
   # "lib/sqlite.neon" if use_sqlite else None,
#    "lib/string.neon",
#    "lib/sys.neon",
#    "lib/time.neon",
#])


#if os.name == "posix":
#    rtl_cpp.extend([
##        "lib/file_posix.cpp",
 #       "lib/mmap_posix.cpp",
#        "lib/os_posix.cpp",
#        "lib/process_posix.cpp",
#        "lib/time_posix.cpp",
#    ])
#    if sys.platform.startswith("darwin"):
#        rtl_cpp.extend([
#            "lib/time_darwin.cpp",
#        ])
#    elif sys.platform.startswith("linux"):
#        rtl_cpp.extend([
#            "lib/time_linux.cpp",
#        ])
#    else:
#        print >>sys.stderr, "Unsupported platform:", sys.platform
#        sys.exit(1)
#    rtl_platform = "src/rtl_posix.cpp"
#elif os.name == "nt":
#    rtl_cpp.extend([
#        "lib/file_win32.cpp",
#        "lib/mmap_win32.cpp",
#        "lib/os_win32.cpp",
#        "lib/process_win32.cpp",
#        "lib/time_win32.cpp",
#    ])
#    rtl_platform = "src/rtl_win32.cpp"
#else:
#    print "Unsupported platform:", os.name
#    sys.exit(1)

#env.Command(["src/thunks.inc", "src/functions_compile.inc", "src/functions_exec.inc", "src/enums.inc", "src/exceptions.inc", "src/constants_compile.inc"], [rtl_neon, "scripts/make_thunks.py"], sys.executable + " scripts/make_thunks.py " + " ".join(rtl_neon))

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
     #lib/http.cpp
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
                            libbz2.a
                            minizip
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

