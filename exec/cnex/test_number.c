#include <assert.h>
#include <inttypes.h>
#include <limits.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "number.h"

void do_verify_eq(int line, const char *source, const char *value, const char *expected)
{
    if (strcmp(value, expected) == 0) {
        return;
    }
    fprintf(stderr, "    line: %d\n", line);
    fprintf(stderr, "  source: %s\n", source);
    fprintf(stderr, "   value: %s\n", value);
    fprintf(stderr, "expected: %s\n", expected);
    assert(value == expected);
}

#define verify_eq(value, expected) do_verify_eq(__LINE__, #value, value, expected)
#define verify_unsigned(val) do { char str[100]; snprintf(str, sizeof(str), "%llu",     number_to_uint64(number_from_string(val))); verify_eq(str, val); } while(0)
#define verify_signed(val)   do { char str[100]; snprintf(str, sizeof(str), "%"PRId64,  number_to_sint64(number_from_string(val))); verify_eq(str, val); } while(0)

int main()
{
    verify_eq(number_to_string(number_from_uint64(0)), "0");
    verify_eq(number_to_string(number_from_uint64(1)), "1");
    verify_eq(number_to_string(number_from_uint64(_UI32_MAX)), "4294967295");
    verify_eq(number_to_string(number_from_uint64((int64_t)_UI32_MAX + 1)), "4294967296");
    verify_eq(number_to_string(number_from_uint64(_I64_MAX)), "9223372036854775807");
    verify_eq(number_to_string(number_from_uint64(_UI64_MAX)), "18446744073709551615");
    
    verify_eq(number_to_string(number_from_sint64(0)), "0");
    verify_eq(number_to_string(number_from_sint64(1)), "1");
    verify_eq(number_to_string(number_from_sint64(_UI32_MAX)), "4294967295");
    verify_eq(number_to_string(number_from_sint64((int64_t)_UI32_MAX + 1)), "4294967296");
    verify_eq(number_to_string(number_from_sint64(_I64_MAX)), "9223372036854775807");

    verify_eq(number_to_string(number_from_sint64(-1)), "-1");
    verify_eq(number_to_string(number_from_sint64(-4294967295LL)), "-4294967295");
    verify_eq(number_to_string(number_from_sint64(-4294967296LL)), "-4294967296");
    verify_eq(number_to_string(number_from_sint64(-9223372036854775807LL)), "-9223372036854775807");
    verify_eq(number_to_string(number_from_sint64(_I64_MIN)), "-9223372036854775808");

    verify_unsigned("0");
    verify_unsigned("1");
    verify_unsigned("4294967295");
    verify_unsigned("4294967296");
    verify_unsigned("9223372036854775807");
    verify_unsigned("18446744073709551615");

    verify_signed("0");
    verify_signed("1");
    verify_signed("4294967295");
    verify_signed("4294967296");
    verify_signed("9223372036854775807");
    verify_signed("-1");
    verify_signed("-4294967295");
    verify_signed("-4294967296");
    verify_signed("-9223372036854775807");
    verify_signed("-9223372036854775808");
}
