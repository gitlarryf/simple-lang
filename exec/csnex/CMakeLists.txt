set(CSNEX_SOURCES
    csnex.cs
    Bytecode.cs
    Cell.cs
    Exceptions.cs
    Exec.cs
    Global.cs
    Number.cs
    Object.cs
    Opcode.cs
)

set(CSNEX_RTL
    lib/runtime.cs
    lib/sys.cs
)

add_custom_command(
    OUTPUT csnex.exe
    COMMAND ${CSC} -warn:4 -warnaserror+ -debug+ -d:DEBUG -appconfig:App.config -out:csnex.exe ${CSNEX_SOURCES} ${CSNEX_RTL}
    DEPENDS ${CSNEX_SOURCES}
)

add_custom_target(csnex_target ALL
    DEPENDS csnex.exe
)
