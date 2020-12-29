#set(CMAKE_RUNTIME_OUTPUT_DIRECTORY .)

# set(CSNEX_TARGET csnex)
# if (WIN32)
    # set(CSNEX_TARGET csnex.exe)
# endif (WIN32)

set(CSNEX_CORE
    App.config
    Bytecode.cs
    Cell.cs
    Exceptions.cs
    Extensions.cs
    exec.cs
    Global.cs
    Number.cs
    opcode.cs
    Program.cs
    properties/AssemblyInfo.cs
)

add_custom_command(
    OUTPUT csnex
    COMMAND ${CSC} /out:csnex /target:exe ${CSNEX_CORE}
    DEPENDS ${CSNEX_CORE}
)

add_custom_target(csnex ALL
    DEPENDS csnex
)

#set_property(TARGET csnex PROPERTY VS_DOTNET_REFERENCES
#    "System"
#    "System.Core"
##    "System.Data"
#    "System.Data.DataSetExtensions"
#)

#set_target_properties(csnex PROPERTIES VS_DOTNET_TARGET_FRAMEWORK_VERSION "v3.5")

if (WIN32)
    #set_target_properties(csnex PROPERTIES WIN32_EXECUTABLE TRUE)
else (WIN32)
endif (WIN32)
