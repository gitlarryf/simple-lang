set(CMAKE_RUNTIME_OUTPUT_DIRECTORY .)

set(CSNEX_RTL
    lib/runtime.cs
)

set(CSNEX
    Bytecode.cs
    Cell.cs
    Exceptions.cs
    Extensions.cs
    exec.cs
    Global.cs
    Number.cs
    Object.cs
    opcode.cs
    csnex.cs
    properties/AssemblyInfo.cs
    ${CSNEX_RTL}
)

set(TEST_NUMBER_TO_STRING_CS
   test_number_to_string.cs
   Number.cs
)

add_custom_command(
    OUTPUT csnex.exe
    COMMAND ${CSC} -appconfig:App.config ${CSNEX}
    DEPENDS ${CSNEX}
)

#COMMAND ${CSC} -out:csnex -target:exe -appconfig:App.config ${CSNEX}

add_custom_command(
    OUTPUT test_number_to_string_cs.exe
    COMMAND ${CSC} -appconfig:App.config ${TEST_NUMBER_TO_STRING_CS}
    DEPENDS ${TEST_NUMBER_TO_STRING_CS}
)

add_custom_target(csnex_target ALL
    DEPENDS csnex.exe
)

add_custom_target(test_number_to_string_cs_target ALL
    DEPENDS test_number_to_string_cs.exe
)
add_test(
    NAME test_number_to_string_cs
    COMMAND test_number_to_string_cs.exe
)

#set_property(TARGET csnex PROPERTY VS_DOTNET_REFERENCES
#    "System"
#    "System.Core"
#    "System.Data"
#    "System.Data.DataSetExtensions"
#)

#set_target_properties(csnex PROPERTIES VS_DOTNET_TARGET_FRAMEWORK_VERSION "v3.5")
