#include "number.h"

#include <assert.h>
#include <stdarg.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "cell.h"


/*
 * Number / string functions
 */

char *number_to_string(Number x)
{
    static char buf[50];
    char val[50];
    bid128_to_string(val, x);
    sprintf(buf, "%g", atof(val));
    return buf;
}

/* ToDo: Implement proper number to string formatter. */
void number_toString(Number x, char *buf, size_t len)
{
    //const int PRECISION = 34; /* ToDo: Finish implementation */
    int iLeadingZeros = 0;

    assert(len != 0);

    char val[50];
    char lead[50];
    bid128_to_string(val, x);

    char *s = &val[0];

    /* Skip over the leading +, if there is one. */
    while(*s != '\0' && (*s == '+')) s++;

    char *v = s;

    while (*s != '\0' && (*s != 'E')) s++;

    char *E = s;

    if (*s == 'E') {
        /* Deal with the exponent. */
        int i;
        iLeadingZeros = abs(atoi(++s));
        for (i = 0; i < iLeadingZeros; i++) {
            lead[i] = '0';
        }
        lead[i++] = '.';
        lead[i++] = '\0';
        strcpy(buf, lead);
    }

    //const int PRECISION = 34;

    //char buf[50];
    //bid128_to_string(buf, x.get_bid());
    //std::string sbuf(buf);
    //const std::string::size_type E = sbuf.find('E');
    //if (E == std::string::npos) {
    //    // Inf or NaN
    //    return sbuf;
    //}
    //int exponent = std::stoi(sbuf.substr(E+1));
    //sbuf = sbuf.substr(0, E);
    //const std::string::size_type last_significant_digit = sbuf.find_last_not_of('0');
    //if (last_significant_digit == 0) {
    //    return "0";
    //}
    //const int trailing_zeros = static_cast<int>(sbuf.length() - (last_significant_digit + 1));
    //exponent += trailing_zeros;
    //sbuf = sbuf.substr(0, last_significant_digit + 1);
    //if (exponent != 0) {
    //    if (exponent > 0 && sbuf.length() + exponent <= PRECISION+1) {
    //        sbuf.append(exponent, '0');
    //    } else if (exponent < 0 && -exponent < static_cast<int>(sbuf.length()-1)) {
    //        sbuf = sbuf.substr(0, sbuf.length()+exponent) + "." + sbuf.substr(sbuf.length()+exponent);
    //    } else if (exponent < 0 && -exponent == static_cast<int>(sbuf.length()-1)) {
    //        sbuf = sbuf.substr(0, 1) + "0." + sbuf.substr(1);
    //    } else if (exponent < 0 && sbuf.length() - exponent <= PRECISION+2) {
    //        sbuf.insert(1, "0." + std::string(-exponent-(sbuf.length()-1), '0'));
    //    } else {
    //        exponent += static_cast<int>(sbuf.length() - 2);
    //        if (sbuf.length() >= 3) {
    //            sbuf.insert(2, 1, '.');
    //        }
    //        sbuf.push_back('e');
    //        sbuf.append(std::to_string(exponent));
    //    }
    //}
    //if (sbuf.at(0) == '+') {
    //    sbuf = sbuf.substr(1);
    //}
    //return sbuf;


    *E = '\0';
    strcat(buf, v);
}

Number number_from_string(char *s)
{
    return bid128_from_string(s);
}


/*
 * Number math functions
 */

Number number_modulo(Number x, Number y)
{
    Number m = bid128_abs(y);
    if (bid128_isSigned(x)) {
        Number q = bid128_round_integral_positive(bid128_div(bid128_abs(x), m));
        x = bid128_add(x, bid128_mul(m, q));
    }
    Number r = bid128_fmod(x, m);
    if (bid128_isSigned(y) && !bid128_isZero(r)) {
        r = bid128_sub(r, m);
    }
    return r;
}

BOOL number_is_integer(Number x)
{
    Number i = bid128_round_integral_zero(x);
    return bid128_quiet_equal(x, i) != 0;
}

/*
 * Number TO functions
 */

int32_t number_to_sint32(Number x)
{
    return bid128_to_int32_int(x);
}

uint32_t number_to_uint32(Number x)
{
    return bid128_to_uint32_int(x);
}

int64_t number_to_sint64(Number x)
{ 
    return bid128_to_int64_int(x);
}

uint64_t number_to_uint64(Number x)
{
    return bid128_to_uint64_int(x);
}


/*
 * Number FROM functions
 */

Number number_from_sint32(int32_t x)
{
    return bid128_from_int32(x);
}

Number number_from_uint32(uint32_t x)
{
    return bid128_from_uint32(x);
}

Number number_from_uint64(uint64_t x)
{
    return bid128_from_uint64(x);
}

Number number_from_sint64(int64_t x)
{
    return bid128_from_int64(x);
}


/*
 * Number comparison functions
 */

BOOL number_is_equal(Number x, Number y)
{
    return bid128_quiet_equal(x, y);
}

BOOL number_is_less(Number x, Number y)
{
    return bid128_quiet_less(x, y);
}

BOOL number_is_greater(Number x, Number y)
{
    return bid128_quiet_greater(x, y);
}


//#define __NUMBER_TESTS
#ifdef __NUMBER_TESTS
void main()
{
    assert(strcasecmp(number_to_string(number_from_string("1234.5678e5")), "123456780") == 0);
    assert(strcasecmp(number_to_string(number_from_string("1234.5678e4")), "12345678") == 0);
    assert(strcasecmp(number_to_string(number_from_string("1234.5678e3")), "1234567.8") == 0);
    assert(strcasecmp(number_to_string(number_from_string("1234.5678e2")), "123456.78") == 0);
    assert(strcasecmp(number_to_string(number_from_string("1234.5678e1")), "12345.678") == 0);
    assert(strcasecmp(number_to_string(number_from_string("1234.5678e0")), "1234.5678") == 0);
    assert(strcasecmp(number_to_string(number_from_string("1234.5678e-1")), "123.45678") == 0);
    assert(strcasecmp(number_to_string(number_from_string("1234.5678e-2")), "12.345678") == 0);
    assert(strcasecmp(number_to_string(number_from_string("1234.5678e-3")), "1.2345678") == 0);
    assert(strcasecmp(number_to_string(number_from_string("1234.5678e-4")), "0.12345678") == 0);
    assert(strcasecmp(number_to_string(number_from_string("1234.5678e-5")), "0.012345678") == 0);

    assert(strcasecmp(number_to_string(number_from_string("123.45")),      number_to_string(number_from_string("123.45"))) == 0);
    assert(strcasecmp(number_to_string(number_from_string("12.345")),      number_to_string(number_from_string("123.45"))) != 0);
    assert(strcasecmp(number_to_string(number_from_string("1234567890")),  number_to_string(number_from_string("1234567890"))) == 0);

    // +4294967290E+0
    // +18446744073709551610E+0
}
#endif
