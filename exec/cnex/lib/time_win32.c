#include <iso646.h>
#include <stdlib.h>
#include <windows.h>

#include "cell.h"
#include "exec.h"
#include "number.h"
#include "stack.h"
#include "time.h"


static const ULONGLONG FILETIME_UNIX_EPOCH = 116444736000000000ULL;
static Number PERFORMANCE_FREQUENCY;

void time_initModule()
{
    LARGE_INTEGER freq;
    QueryPerformanceFrequency(&freq);
    PERFORMANCE_FREQUENCY = number_from_uint64(freq.QuadPart);
}

void time_shutdownModule()
{
    number_freeNumber(&PERFORMANCE_FREQUENCY);
}



void time_sleep(TExecutor *exec)
{
    Number seconds = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    // ToDo: If the number of seconds is greater than MAX_INT / 1000, then call Sleep() multiple times.
    if (number_is_greater(seconds, number_from_uint32(4294967))) {
        exec->rtl_raise(exec, "InvalidValueException", "", BID_ZERO);
        number_freeNumber(&seconds);
        return;
    }
    Number multiplier = number_from_uint32(1000);
    uint32_t ms = number_to_uint32(number_multiply(seconds, multiplier));
    number_freeNumber(&multiplier);
    Sleep(ms);
    number_freeNumber(&seconds);
}

void time_now(TExecutor *exec)
{
    FILETIME ft;
    GetSystemTimeAsFileTime(&ft);
    ULARGE_INTEGER ticks;
    ticks.LowPart = ft.dwLowDateTime;
    ticks.HighPart = ft.dwHighDateTime;
    push(exec->stack, cell_fromNumber(number_divide(number_from_uint64(ticks.QuadPart - FILETIME_UNIX_EPOCH), number_from_uint32(10000000))));
}

void time_tick(TExecutor *exec)
{
    LARGE_INTEGER now;
    QueryPerformanceCounter(&now);

    push(exec->stack, cell_fromNumber(number_divide(number_from_uint64(now.QuadPart), PERFORMANCE_FREQUENCY)));
}

