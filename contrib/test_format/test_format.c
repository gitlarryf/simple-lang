#ifdef __MS_HEAP_DBG
#define _CRTDBG_MAP_ALLOC
#include <stdlib.h>
#include <crtdbg.h>
#endif

#include <assert.h>
#include <errno.h>
#include <inttypes.h>
#include <stdarg.h>
#include <stdint.h>
#include <stdio.h>
#include <string.h>
#ifndef __MS_HEAP_DBG
#include <stdlib.h>
#endif

#include "format.h"
#include "nstring.h"
#include "util.h"

void test(Spec *spec, const char*pszValue, const char *pszExpected)
{
    TString *a = string_createCString(pszValue);
    TString *f = formatString(NULL, a, spec);
    char *cmp = string_asCString(f);
    string_freeString(f);

    assert(strcmp(cmp, pszExpected) == 0);
    free(cmp);
}

int main()
{
#ifdef __MS_HEAP_DBG
    /* ToDo: Remove this!  This is only for debugging. */
    /* gOptions.ExecutorDebugStats = TRUE; */
    _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
    _CrtSetBreakAlloc(420);
#endif
    Spec *spec = malloc(sizeof(Spec));
    TString *fmt = string_createCString("5");
    assert(parse(fmt, spec));
    test(spec, "",          "     " );
    test(spec, "a",         "a    " );
    test(spec, "ab",        "ab   " );
    test(spec, "abc",       "abc  " );
    test(spec, "abcd",      "abcd " );
    test(spec, "abcde",     "abcde" );
    test(spec, "abcdef",    "abcdef");
    string_freeString(fmt);

    fmt = string_createCString(">5");
    assert(parse(fmt, spec));
    test(spec, "",          "     " );
    test(spec, "a",         "    a" );
    test(spec, "ab",        "   ab" );
    test(spec, "abc",       "  abc" );
    test(spec, "abcd",      " abcd" );
    test(spec, "abcde",     "abcde" );
    test(spec, "abcdef",    "abcdef");
    string_freeString(fmt);

    fmt = string_createCString("^5");
    assert(parse(fmt, spec));
    test(spec, "",          "     " );
    test(spec, "a",         "  a  " );
    test(spec, "ab",        " ab  " );
    test(spec, "abc",       " abc " );
    test(spec, "abcd",      "abcd " );
    test(spec, "abcde",     "abcde" );
    test(spec, "abcdef",    "abcdef");
    string_freeString(fmt);

    fmt = string_createCString("5.3");
    assert(parse(fmt, spec));
    test(spec, "",          "     ");
    test(spec, "a",         "a    ");
    test(spec, "ab",        "ab   ");
    test(spec, "abc",       "abc  ");
    test(spec, "abcd",      "abc  ");
    test(spec, "abcde",     "abc  ");
    test(spec, "abcdef",    "abc  ");
    string_freeString(fmt);

    fmt = string_createCString("4d");
    assert(parse(fmt, spec));
    test(spec, "0",         "   0" );
    test(spec, "1",         "   1" );
    test(spec, "12",        "  12" );
    test(spec, "123",       " 123" );
    test(spec, "1234",      "1234" );
    test(spec, "12345",     "12345");
    string_freeString(fmt);

    fmt = string_createCString("04d");
    assert(parse(fmt, spec));
    test(spec, "0",         "0000" );
    test(spec, "1",         "0001" );
    test(spec, "12",        "0012" );
    test(spec, "123",       "0123" );
    test(spec, "1234",      "1234" );
    test(spec, "12345",     "12345");
    string_freeString(fmt);

    fmt = string_createCString("4x");
    assert(parse(fmt, spec));
    test(spec, "0",         "   0" );
    test(spec, "1",         "   1" );
    test(spec, "12",        "   c" );
    test(spec, "123",       "  7b" );
    test(spec, "1234",      " 4d2" );
    test(spec, "12345",     "3039" );
    test(spec, "123456",    "1e240");
    string_freeString(fmt);

    fmt = string_createCString("04x");
    assert(parse(fmt, spec));
    test(spec, "0",         "0000" );
    test(spec, "1",         "0001" );
    test(spec, "12",        "000c" );
    test(spec, "123",       "007b" );
    test(spec, "1234",      "04d2" );
    test(spec, "12345",     "3039" );
    test(spec, "123456",    "1e240");
    string_freeString(fmt);

    free(spec);
    // TODO: Many more format specs.
}
