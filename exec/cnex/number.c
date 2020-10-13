#include "number.h"

#include <assert.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "cell.h"

#define INIT_NUMBER { { 0 }, 0, NNI }

/*
 * Number / string functions
 */

char *number_to_string(Number x)
{
    static char buf[500];
    if (x.rep == BID) {
        number_toString(x, buf, sizeof(buf));
        return buf;
    } else {
        mpz_get_str(buf, 10, x.mpz);
    }
    return buf;
}

void number_toString(Number x, char *buf, size_t len)
{
    memset(buf, 0x00, len);
    const int PRECISION = 34;

    assert(len != 0);

    char val[50] = { 0 };
    bid128_to_string(val, x.bid);

    char *v, *s = v = &val[1];

    // Calculate the length of the string, locating the exponent value at the same time.
    int slen = 0;
    while (*s != '\0' && (*s != 'E')) {
        s++;
        slen++;
    }

    // Store the position of the exponent marker.
    char *E = s;

    // If we didn't find an exponent marker, then just return the buffer that it is.
    if (*s != 'E') {
        // Number is possibly +Nan, or +Inf.
        strncpy(buf, val, len);
        buf[len-1] = '\0';
        return;
    }

    // Get the actual exponent, by walking past the eponent marker, then converting to decimal value.
    int exponent = atoi(++s);

    // Place a NULL char at the exponent marker location, effectively terminating the string there.
    *E = '\0';

    // Now, we setup a (p)ointer to be one byte BEFORE the NULL we just added to the string, this
    // effectively places us at the end of the numerical string.
    char *p = E - 1;

    // We're going to start walking from right to left, looking for the last significant digit; the last digit in the string that isn't zero.
    while ((p >= v) && (*p == '0')) p--;

    // Once found, we can note the position of it in the string.  Since we know that v is a pointer to the start of the string, and p is the last digit found.
    const int last_significant_digit = (int)(p - v);

    if (last_significant_digit == -1) {
        buf[0] = '0';
        buf[1] = '\0';
        return;
    }

    // Once we know the last siginificant digit, we can calculate how many trailing zero existed.
    const int trailing_zeros = (int)slen - last_significant_digit-1;
    // Since we're going to chop of some trailing zeros, we need to update the length of our string.
    slen -= trailing_zeros;
    exponent += trailing_zeros;
    char mantissa[50] = { 0 };
    char r[50] = { 0 };
    // Copy the mantissa out to the last significant digit.
    memcpy(mantissa, v, last_significant_digit+1);
    // Guarantee the string is null terminated, despite the buffer starting off NULL.
    mantissa[last_significant_digit+1] = '\0';

    if (exponent != 0) {
        if (exponent > 0 && slen + exponent <= PRECISION) {
            // We have an exponent value we need to deal with, but the length combined with the exponent
            // is less than our desired max precision, so we move the mantissa value into (r)esult, the length
            // of the significant mantissa, adding zeros as necessary.
            memcpy(r, mantissa, last_significant_digit+1);
            for (int i = last_significant_digit+1; i < ((last_significant_digit+1) + exponent); i++, slen++) {
                r[i] = '0';
            }
        } else if (exponent < 0 && -exponent < slen) {
            // Our exponent is negative, and is less than the length of the mantissa, so we're
            // going to insert the decimal into slen+exponent location.
            memcpy(r, mantissa, slen+exponent);
            r[slen+exponent] = '.';
            memcpy(&r[slen+exponent+1], &mantissa[slen+exponent], slen - (slen+exponent));
        } else if (exponent < 0 && -exponent == slen) {
            // Our exponent is negative, and is the length of the mantissa, so we're going to start
            // the number with "0.", and then add the rest of the mantissa on the right side of the decimal.
            memcpy(r, "0.", 2);
            memcpy(&r[2], mantissa, slen+=2);
        } else if (exponent < 0 && slen - exponent <= PRECISION + 1) {
            // Our exponent is negative, and the mantissa length, minus the exponent less or equal to our max PRECISION
            // then we need to insert zero's to the right of the decimal.
            int count = 2;
            // Start out by setting [r]esult to "0."
            memcpy(r, "0.", count);
            // Now, we copy the necessary number of zeros after the decimal point...
            for (int i = 0; i < -exponent-slen;i++, count++) {
                r[count] = '0';
            }
            // Then copy the remainder of the mantissa to our [r]esult.
            memcpy(&r[count], mantissa, slen);
            // Ensure that [r]esult is null terminated.  I'm not 100% convinced this is necessary.
            slen += count;
            r[slen] = '\0';
        } else {
            // Our exponent is positive, therefore we need to add the decimal point on the right of the number.
            exponent += last_significant_digit;
            if (last_significant_digit >= 1) {
                // Our mantissa is at least 2 digits long, so copy the first digit to [r]esult
                r[0] = mantissa[0];
                // add the decimal point
                r[1] = '.';
                // copy the remainder of the mantissa to [r]esult
                memcpy(&r[2], &mantissa[1], last_significant_digit);
                slen++;
            } else {
                // The mantissa is only one significant digit, so copy that to the output, then apply the exponent value.
                memcpy(r, mantissa, last_significant_digit+1);
            }
            // Now, create the actual exponent value.
            char exp[36];
            int explen = sprintf(exp, "e%d", exponent);
            memcpy(&r[slen], exp, explen+1); // Include the terminating NULL char given to us by sprintf()
            slen += explen;
        }
    } else {
        // Exponent is zero, or just copy the mantissa, up to the last significant digit.
        memcpy(r, mantissa, last_significant_digit+1); // Include terminating NULL char.
    }
    if (val[0] == '-') {
        // The number is a negative number, so include the sign on the end of it.
        memcpy(&buf[1], r, slen+1); // Be sure to include the terminating NULL char.
        buf[0] = '-';
        return;
    }
    memcpy(buf, r, slen+1); // Be sure to include the terminating NULL char.
}

Number number_from_string(char *s)
{
    Number r = { { 0 }, 0, BID };

    char *p = s;
    size_t len = strlen(s);
    size_t i = 0;
    if (s[i] == '-') {
        i++;
        p++;
    }
    if (strspn(p, "0123456789") == len - i) {
        if (mpz_init_set_str(r.mpz, s, 10) == 0) {
            r.rep = MPZ;
        } else {
            r.rep = BID;
            r.bid = bid128_nan(NULL);
        }
        return r;
    }
    r.rep = BID;
    r.bid = bid128_from_string(s);
    return r;
}

Number number_newNumber(Rep representation)
{
    Number r = { { 0 }, 0, NNI };

    if (representation == MPZ) {
        r.rep = MPZ;
        mpz_init(r.mpz);
    } else if (representation == BID) {
        r.rep = BID;
        // BID is already initialized to 0.
    }
    return r;
}

Number number_from_bid(BID_UINT128 n)
{
    Number r = INIT_NUMBER;
    r.bid = n;
    r.rep = BID;
    return r;
}

Number number_from_mpz(mpz_t n)
{
    Number r = { { 0 }, 0, MPZ };
    mpz_init_set(r.mpz, n);
    // Should we really clear this?  This should probably be the callers responsibility.
    mpz_clear(n);
    return r;
}

Number number_fromNumber(const Number *n)
{
    Number r = INIT_NUMBER;

    r.rep = n->rep;
    if (n->rep == MPZ) {
        mpz_init_set(r.mpz, n->mpz);
    } else {
        r.bid = n->bid;
    }
    return r;
}

BID_UINT128 bid_from_number(Number n)
{
    if (n.rep == BID) {
        return n.bid;
    }
    assert(n.rep == MPZ);
    char buf[500];
    mpz_get_str(buf, 10, n.mpz);
    //mpz_clear(n.mpz);
    return bid128_from_string(buf);
}

//void number_copyNumber(Number *dest, const Number *src)
//{
//    dest->rep = src->rep;
//    if (dest->rep == MPZ) {
//        mpz_init_set(dest->mpz, src->mpz);
//    } else {
//        dest->bid = src->bid;
//    }
//}

void number_freeNumber(Number *n)
{
    if (n->rep == MPZ) {
        mpz_clear(n->mpz);
    }
}


/*
 * Number math functions
 */

Number number_add(Number x, Number y)
{
    Number res = INIT_NUMBER;

    if (x.rep == MPZ && y.rep == MPZ) {
        res.rep = MPZ;
        mpz_init(res.mpz);
        mpz_add(res.mpz, x.mpz, y.mpz);
        return res;
    }
    res.rep = BID;
    res.bid = bid128_add(bid_from_number(x), bid_from_number(y));
    return res;
}

Number number_subtract(Number x, Number y)
{
    Number res = INIT_NUMBER;

    if (x.rep == MPZ && y.rep == MPZ) {
        res.rep = MPZ;
        mpz_init(res.mpz);
        mpz_sub(res.mpz, x.mpz, y.mpz);
        return res;
    }
    res.rep = BID;
    res.bid = bid128_sub(bid_from_number(x), bid_from_number(y));
    return res;
}

Number number_divide(Number x, Number y)
{
    Number res = INIT_NUMBER;

    if (x.rep == MPZ && y.rep == MPZ) {
        res.rep = MPZ;
        mpz_init(res.mpz);
        if (mpz_divisible_p(x.mpz, y.mpz)) {
            mpz_tdiv_q(res.mpz, x.mpz, y.mpz);
            return res;
        }
        mpz_clear(res.mpz);
    }
    res.rep = BID;
    res.bid = bid128_div(bid_from_number(x), bid_from_number(y));
    return res;
}

Number number_modulo(Number x, Number y)
{
    Number m;
    if (x.rep == MPZ && y.rep == MPZ) {
        m.rep = MPZ;
        mpz_init(m.mpz);
        mpz_fdiv_r(m.mpz, x.mpz, y.mpz);
        return m;
    }

    m.bid = bid128_abs(y.bid);
    if (bid128_isSigned(x.bid)) {
        Number q = number_from_bid(bid128_round_integral_positive(bid128_div(bid128_abs(x.bid), m.bid)));
        x.bid = bid128_add(x.bid, bid128_mul(m.bid, q.bid));
    }
    Number r = number_from_bid(bid128_fmod(x.bid, m.bid));
    if (bid128_isSigned(y.bid) && !bid128_isZero(r.bid)) {
        r.bid = bid128_sub(r.bid, m.bid);
    }
    return r;
}

Number number_multiply(Number x, Number y)
{
    Number res = INIT_NUMBER;

    if (x.rep == MPZ && y.rep == MPZ) {
        res.rep = MPZ;
        mpz_init(res.mpz);
        mpz_mul(res.mpz, x.mpz, y.mpz);
        return res;
    }
    res.rep = BID;
    res.bid = bid128_mul(bid_from_number(x), bid_from_number(y));
    return res;
}

Number number_negate(Number x)
{
    if (x.rep == MPZ) {
        mpz_neg(x.mpz, x.mpz);
        return x;
    }
    return number_from_bid(bid128_negate(x.bid));
}

Number number_pow(Number x, Number y)
{
    if (x.rep == MPZ && y.rep == MPZ) {
        Number r = INIT_NUMBER;
        mpz_init(r.mpz);
        r.rep = MPZ;
        mpz_pow_ui(r.mpz, x.mpz, mpz_get_ui(y.mpz));
        return r;
    }

    if (number_is_integer(y) && !number_is_negative(y)) {
        uint32_t iy = number_to_uint32(y);
        Number r = number_from_uint32(1);
        while (iy != 0) {
            if (iy & 1) {
                r = number_multiply(r, x);
            }
            x = number_multiply(x, x);
            iy >>= 1;
        }
        return r;
    }
    return number_from_bid(bid128_pow(x.bid, y.bid));
}

Number number_powmod(Number b, Number e, Number m)
{
    Number r = INIT_NUMBER;

    mpz_powm(r.mpz, b.mpz, e.mpz, m.mpz);
    r.rep = MPZ;

    return r;
}

Number number_abs(Number x)
{
    Number res = INIT_NUMBER;

    if (x.rep == MPZ) {
        res.rep = MPZ;
        mpz_init(res.mpz);
        mpz_abs(res.mpz, x.mpz);
        return res;
    }
    res.rep = BID;
    res.bid = bid128_abs(bid_from_number(x));
    return res;
}

Number number_sign(Number x)
{
    Number res = INIT_NUMBER;

    if (x.rep == MPZ) {
        res.rep = MPZ;
        mpz_init_set_si(res.mpz, mpz_sgn(x.mpz));
        return res;
    }
    res.rep = BID;
    res.bid = bid128_copySign(bid128_from_uint32(1), bid_from_number(x));
    return res;
}

Number number_ceil(Number x)
{
    Number res = INIT_NUMBER;

    if (x.rep == MPZ) {
        return x;
    }
    res.rep = BID;
    res.bid = bid128_round_integral_positive(bid_from_number(x));
    return res;
}

Number number_floor(Number x)
{
    Number res = INIT_NUMBER;

    if (x.rep == MPZ) {
        return x;
    }
    res.rep = BID;
    res.bid = bid128_round_integral_negative(bid_from_number(x));
    return res;
}

Number number_trunc(Number x)
{
    Number res = INIT_NUMBER;

    if (x.rep == MPZ) {
        return x;
    }
    res.rep = BID;
    res.bid = bid128_round_integral_zero(bid_from_number(x));
    return res;
}

Number number_exp(Number x)
{
    return number_from_bid(bid128_exp(bid_from_number(x)));
}

Number number_log(Number x)
{
    return number_from_bid(bid128_log(bid_from_number(x)));
}

Number number_sqrt(Number x)
{
    // ToDo: mpz
    return number_from_bid(bid128_sqrt(bid_from_number(x)));
}

Number number_acos(Number x)
{
    return number_from_bid(bid128_acos(bid_from_number(x)));
}

Number number_asin(Number x)
{
    return number_from_bid(bid128_asin(bid_from_number(x)));
}

Number number_atan(Number x)
{
    return number_from_bid(bid128_atan(bid_from_number(x)));
}

Number number_cos(Number x)
{
    return number_from_bid(bid128_cos(bid_from_number(x)));
}

Number number_sin(Number x)
{
    return number_from_bid(bid128_sin(bid_from_number(x)));
}

Number number_tan(Number x)
{
    return number_from_bid(bid128_tan(bid_from_number(x)));
}

Number number_acosh(Number x)
{
    return number_from_bid(bid128_acosh(bid_from_number(x)));
}

Number number_asinh(Number x)
{
    return number_from_bid(bid128_asinh(bid_from_number(x)));
}

Number number_atanh(Number x)
{
    return number_from_bid(bid128_atanh(bid_from_number(x)));
}

Number number_atan2(Number y, Number x)
{
    return number_from_bid(bid128_atan2(bid_from_number(y), bid_from_number(x)));
}

Number number_cbrt(Number x)
{
    // ToDo: mpz
    return number_from_bid(bid128_cbrt(bid_from_number(x)));
}

Number number_cosh(Number x)
{
    return number_from_bid(bid128_cosh(bid_from_number(x)));
}

Number number_erf(Number x)
{
    return number_from_bid(bid128_erf(bid_from_number(x)));
}

Number number_erfc(Number x)
{
    return number_from_bid(bid128_erfc(bid_from_number(x)));
}

Number number_exp2(Number x)
{
    // ToDo: mpz
    return number_from_bid(bid128_exp2(bid_from_number(x)));
}

Number number_expm1(Number x)
{
    return number_from_bid(bid128_expm1(bid_from_number(x)));
}

Number number_frexp(Number x, int *exp)
{
    return number_from_bid(bid128_frexp(bid_from_number(x), exp));
}

Number number_hypot(Number x, Number y)
{
    return number_from_bid(bid128_hypot(bid_from_number(x), bid_from_number(y)));
}

Number number_ldexp(Number x, int exp)
{
    return number_from_bid(bid128_ldexp(bid_from_number(x), exp));
}

Number number_lgamma(Number x)
{
    return number_from_bid(bid128_lgamma(bid_from_number(x)));
}

Number number_log10(Number x)
{
    return number_from_bid(bid128_log10(bid_from_number(x)));
}

Number number_log1p(Number x)
{
    return number_from_bid(bid128_log1p(bid_from_number(x)));
}

Number number_log2(Number x)
{
    return number_from_bid(bid128_log2(bid_from_number(x)));
}

Number number_nearbyint(Number x)
{
    if (x.rep == MPZ) {
        return x;
    }
    return number_from_bid(bid128_nearbyint(bid_from_number(x)));
}

Number number_sinh(Number x)
{
    return number_from_bid(bid128_sinh(bid_from_number(x)));
}

Number number_tanh(Number x)
{
    return number_from_bid(bid128_tanh(bid_from_number(x)));
}

Number number_tgamma(Number x)
{
    return number_from_bid(bid128_tgamma(bid_from_number(x)));
}


/*
 * Number Test functions
 */

BOOL number_is_integer(Number x)
{
    if (x.rep == MPZ) {
        return TRUE;
    }
    BID_UINT128 i = bid128_round_integral_zero(x.bid);
    return bid128_quiet_equal(x.bid, i) != 0;
}

BOOL number_is_nan(Number x)
{
    if (x.rep == MPZ) {
        return FALSE;
    }
    return bid128_isNaN(x.bid) != 0;
}

BOOL number_is_negative(Number x)
{
    if (x.rep == MPZ) {
        return mpz_sgn(x.mpz) == -1;
    }
    return bid128_isSigned(x.bid) != 0;
}

BOOL number_is_zero(Number x)
{
    if (x.rep == MPZ) {
        return mpz_cmp_si(x.mpz, 0) == 0;
    }
    return bid128_isZero(bid_from_number(x)) == 0;
}

/*
 * Number TO functions
 */

int32_t number_to_sint32(Number x)
{
    if (x.rep == MPZ) {
        return (int32_t)mpz_get_si(x.mpz);
    }
    return bid128_to_int32_int(x.bid);
}

uint32_t number_to_uint32(Number x)
{
    if (x.rep == MPZ) {
        return (uint32_t)mpz_get_ui(x.mpz);
    }
    return bid128_to_uint32_int(x.bid);
}

int64_t number_to_sint64(Number x)
{ 
    if (x.rep == MPZ) {
        size_t sls = sizeof(signed long);
        if (sls >= 8) {
            return mpz_get_si(x.mpz);
        } else {
            BOOL negative = FALSE;
            if (number_is_negative(x)) {
                negative = TRUE;
                x = number_negate(x);
            }
            mpz_t ms;
            mpz_init_set(ms, x.mpz);
            mpz_fdiv_q_2exp(ms, x.mpz, 32);
            uint64_t ur = (mpz_get_ui(x.mpz) & 0xFFFFFFFF) | (((uint64_t)mpz_get_ui(ms) & 0xFFFFFFFF) << 32);
            mpz_clear(ms);
            if (ur > LLONG_MAX) {
                if (negative) {
                    return LLONG_MIN;
                } else {
                    return LLONG_MAX;
                }
            }
            int64_t sr = (int64_t)ur;
            if (negative) {
                sr = -sr;
            }
            return sr;
        }
    }
    return bid128_to_int64_int(x.bid);
}

uint64_t number_to_uint64(Number x)
{
    if (x.rep == MPZ) {
        size_t uls = sizeof(unsigned long);
        if (uls >= 8) {
            return mpz_get_ui(x.mpz);
        } else {
            uint64_t lsw = mpz_get_ui(x.mpz) & 0xFFFFFFFF;
            mpz_t ms;
            mpz_init_set(ms, x.mpz);
            mpz_fdiv_q_2exp(ms, x.mpz, 32);
            uint32_t msw = mpz_get_ui(ms);
            mpz_clear(ms);
            return lsw | (((uint64_t)msw & 0xFFFFFFFF) << 32);
        }
    }
    return bid128_to_uint64_int(x.bid);
}

float number_to_float(Number x)
{
    if (x.rep == MPZ) {
        return (float)mpz_get_d(x.mpz);
    }
    return bid128_to_binary32(x.bid);
}

double number_to_double(Number x)
{
    if (x.rep == MPZ) {
        return mpz_get_d(x.mpz);
    }
    return bid128_to_binary64(x.bid);
}


/*
 * Number FROM functions
 */

Number number_from_sint8(int8_t x)
{
    //Number r = INIT_NUMBER;
    //r.rep = MPZ;
    //mpz_init_set_si(r.mpz, x);
    //return r;
    return number_from_bid(bid128_from_int32(x));
}

Number number_from_uint8(uint8_t x)
{
    //Number r = INIT_NUMBER;
    //r.rep = MPZ;
    //mpz_init_set_ui(r.mpz, x);
    //return r;
    return number_from_bid(bid128_from_uint32(x));
}

Number number_from_sint32(int32_t x)
{
    //Number r = INIT_NUMBER;
    //r.rep = MPZ;
    //mpz_init_set_si(r.mpz, x);
    //return r;
    return number_from_bid(bid128_from_int32(x));
}

Number number_from_uint32(uint32_t x)
{
    //Number r = INIT_NUMBER;
    //r.rep = MPZ;
    //mpz_init_set_ui(r.mpz, x);
    //return r;
    return number_from_bid(bid128_from_uint32(x));
}

Number number_from_uint64(uint64_t x)
{
    //Number r = INIT_NUMBER;
    //mpz_t hi;
    //mpz_t lo;

    //mpz_init_set_ui(hi, ((x >> 32) & 0xFFFFFFFF));
    //mpz_mul_2exp(hi, hi, 32);
    //mpz_init_set_ui(lo, (uint32_t)(x & 0xFFFFFFFF));
    //mpz_add(r.mpz, lo, hi);
    //mpz_clear(lo);
    //mpz_clear(hi);
    //r.rep = MPZ;

    //return r;
    return number_from_bid(bid128_from_uint64(x));
}

Number number_from_sint64(int64_t x)
{
    //Number r = INIT_NUMBER;
    //mpz_t hi;
    //mpz_t lo;

    //if (x >= 0) {
    //    mpz_init_set_ui(hi, (uint32_t)(x >> 32));
    //    mpz_mul_2exp(hi, hi, 32);
    //    mpz_init_set_ui(lo, (uint32_t)(x & 0xFFFFFFFF));
    //    mpz_add(r.mpz, lo, hi);
    //} else if (x == _I64_MIN) {
    //    mpz_init_set_ui(hi, 0x80000000);
    //    mpz_mul_2exp(hi, hi, 32);
    //    mpz_init_set_ui(lo, (int32_t)(x & 0xFFFFFFFF));
    //    mpz_add(r.mpz, lo, hi);
    //    mpz_neg(r.mpz, r.mpz);
    //} else {
    //    mpz_init_set_ui(hi, (uint32_t)(-x >> 32));
    //    mpz_mul_2exp(hi, hi, 32);
    //    mpz_init_set_ui(lo, (uint32_t)(-x & 0xFFFFFFFF));
    //    mpz_add(r.mpz, lo, hi);
    //    mpz_neg(r.mpz, r.mpz);
    //}

    //r.rep = MPZ;
    //mpz_clear(lo);
    //mpz_clear(hi);
    //return r;
    return number_from_bid(bid128_from_int64(x));
}

Number number_from_float(float x)
{
    return number_from_bid(binary32_to_bid128(x));
}

Number number_from_double(double x)
{
    return number_from_bid(binary64_to_bid128(x));
}


/*
 * Number comparison functions
 */

BOOL number_is_equal(Number x, Number y)
{
    if (x.rep == MPZ && y.rep == MPZ) {
        return mpz_cmp(x.mpz, y.mpz) == 0;
    }
    return bid128_quiet_equal(bid_from_number(x), bid_from_number(y));
}

BOOL number_is_not_equal(Number x, Number y)
{
    if (x.rep == MPZ && y.rep == MPZ) {
        return mpz_cmp(x.mpz, y.mpz) != 0;
    }
    return bid128_quiet_equal(bid_from_number(x), bid_from_number(y)) != 0;
}

BOOL number_is_less(Number x, Number y)
{
    if (x.rep == MPZ && y.rep == MPZ) {
        return mpz_cmp(x.mpz, y.mpz) < 0;
    }
    return bid128_quiet_less(bid_from_number(x), bid_from_number(y));
}

BOOL number_is_greater(Number x, Number y)
{
    if (x.rep == MPZ && y.rep == MPZ) {
        return mpz_cmp(x.mpz, y.mpz) > 0;
    }
    return bid128_quiet_greater(bid_from_number(x), bid_from_number(y));
}

BOOL number_is_less_equal(Number x, Number y)
{
    if (x.rep == MPZ && y.rep == MPZ) {
        return mpz_cmp(x.mpz, y.mpz) <= 0;
    }
    return bid128_quiet_less_equal(bid_from_number(x), bid_from_number(y)) != 0;
}

BOOL number_is_greater_equal(Number x, Number y)
{
    if (x.rep == MPZ && y.rep == MPZ) {
        return mpz_cmp(x.mpz, y.mpz) >= 0;
    }
    return bid128_quiet_greater_equal(bid_from_number(x), bid_from_number(y)) != 0;
}

BOOL number_is_odd(Number x)
{
    if (x.rep == MPZ) {
        return  mpz_odd_p(x.mpz);
    }
    return !bid128_isZero(bid128_fmod(bid_from_number(x), bid128_from_uint32(2)));
}
