set(CMAKE_RUNTIME_OUTPUT_DIRECTORY .)

add_executable(cnex
    array.c
    bytecode.c
    cell.c
    cnex.c
    dictionary.c
    framestack.c
    global.c
    nstring.c
    number.c
    stack.c
    util.c
)
if ("${CMAKE_CXX_COMPILER_ID}" STREQUAL "GNU" OR "${CMAKE_CXX_COMPILER_ID}" STREQUAL "Clang")
    set_target_properties(cnex PROPERTIES COMPILE_FLAGS "-std=c99 -Wall -Werror")
endif ()
target_link_libraries(cnex
    bid
)