cmake_minimum_required(VERSION 3.1)

add_executable(neonc
    src/analyzer.cpp
    src/ast.cpp
    src/bytecode.cpp
    src/compiler.cpp
    src/debuginfo.cpp
    src/disassembler.cpp
    src/format.cpp
    src/intrinsic.cpp
    src/lexer.cpp
    src/neonc.cpp
    src/number.cpp
    src/parser.cpp
    src/rtl_compile.cpp
    src/support.cpp
    src/support_compiler.cpp
    src/util.cpp

    lib/file_const_posix.cpp
)
target_include_directories(neonc PUBLIC external/IntelRDFPMathLib20U1/LIBRARY/src)
target_include_directories(neonc PUBLIC external/hash-library)
target_include_directories(neonc PUBLIC external/minijson_writer-master)
target_include_directories(neonc PUBLIC external/utf8/source)
target_include_directories(neonc PUBLIC src)
target_compile_options(neonc PUBLIC -std=c++11)
target_link_libraries(neonc -Lexternal/IntelRDFPMathLib20U1/LIBRARY)
target_link_libraries(neonc bid)
target_link_libraries(neonc -Lexternal/hash-library)
target_link_libraries(neonc hash-library)
