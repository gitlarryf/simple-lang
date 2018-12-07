#ifndef EXEC_H
#define EXEC_H
#include <stdint.h>
#include <time.h>

#include "number.h"
#include "util.h"

typedef struct tagTModule {
    char *name;
} TModule;

typedef struct tagTExceptionInfo {
    struct tagTString **info;
    struct tagTString *name;
    size_t size;
    Number code;
} ExceptionInfo;

typedef struct tagTExecutor {
    struct tagTBytecode *object;
    unsigned int ip;
    struct tagTStack *stack;
    /*struct tagTStack *callstack; */
    unsigned int callstack[300];
    int32_t callstacktop;
    int32_t param_recursion_limit;
    int32_t map_depth;
    struct tagTCell *globals;
    struct tagTFrameStack *framestack;
    int exit_code;
    BOOL enable_assert;
    BOOL debug;
    BOOL disassemble;
    void (*rtl_raise)(struct tagTExecutor *, const char *, const char *, Number);
    struct tagTExceptionInfo *exception;
    struct tagTModule *module;

    /* Debug / Diagnostic fields */
    struct {
        uint64_t total_opcodes;
        uint32_t callstack_max_height;
        clock_t time_start;
        clock_t time_end;
    } diagnostics;
} TExecutor;

int exec_loop(struct tagTExecutor *self, size_t min_callstack_depth);
int exec_run(struct tagTExecutor *self, BOOL enable_assert);

void exec_rtl_raiseException(struct tagTExecutor *self, const char *name, const char *info, Number code);

#endif
