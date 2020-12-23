cmake_minimum_required(VERSION 3.8.2)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY .)
project(csnex VERSION 0.0.1 LANGUAGES CSharp)
include(CSharpUtilities)

add_executable(csnex
    App.config
    Bytecode.cs
    Cell.cs
    Exceptions.cs
    exec.cs
    Global.cs
    Number.cs
    opcode.cs
    Program.cs
    properties/AssemblyInfo.cs
)

set_property(TARGET csnex PROPERTY VS_DOTNET_REFERENCES
    "System"
    "System.Core"
    "System.Data"
    "System.Data.DataSetExtensions"
)


#set_property(TARGET csnex PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v3.5")
#set_property(TARGET csnex PROPERTY WIN32_EXECUTABLE TRUE)
set_target_properties(csnex PROPERTIES VS_DOTNET_TARGET_FRAMEWORK_VERSION "v3.5")

if (WIN32)
    #set_property(TARGET csnex PROPERTY WIN32_EXECUTABLE TRUE)
    set_target_properties(csnex PROPERTIES WIN32_EXECUTABLE TRUE)
else (WIN32)
endif (WIN32)
