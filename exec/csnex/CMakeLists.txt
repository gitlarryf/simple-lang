set(CSNEX_SOURCES
    csnex.cs
    Bytecode.cs
    Cell.cs
    Exceptions.cs
    Exec.cs
    Global.cs
    Object.cs
    Opcode.cs
)

add_custom_command(
    OUTPUT csnex.exe
    COMMAND ${CSC} -warn:4 -warnaserror+ -appconfig:App.config ${CSNEX_SOURCES}
    DEPENDS ${CSNEX_SOURCES}
)

add_custom_target(csnex_target ALL
    DEPENDS csnex.exe
)
