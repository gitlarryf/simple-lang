#ifndef NUMBER_H
#define NUMBER_H

#include <limits.h>
#include <stdlib.h>
#include <stdint.h>

#define DECIMAL_GLOBAL_ROUNDING 1
#define DECIMAL_GLOBAL_EXCEPTION_FLAGS 1
//#define NUMBER_FREE_NUMBERS 1

#include "bid_conf.h"
#ifdef _MSC_VER
#pragma warning(push, 0)
#endif
#include "bid_functions.h"
#include <gmp.h>
#ifdef _MSC_VER
#pragma warning(pop)
#endif

//#undef min
//#undef max

#include "util.h"

typedef enum {
    NNI,    // Number Not Initialized; number has not been initialized at all; and is therefore UNSAFE!
    MPZ,    // Multi-precision Integer
    BID     // Binary encoded Decimal number
} Rep;

typedef struct Number {
    BID_UINT128 bid;
    mpz_t  mpz;
    Rep rep;
} Number;


//#define BID_ZERO        bid128_from_uint32(0)
static const Number BID_ZERO        = { { 0x0000000000000000, 0x3040000000000000 }, {0}, BID };
static const Number BID_MIN_INT32   = { { 0x0000000080000000, 0xb040000000000000 }, 0, BID };
static const Number BID_MAX_INT32   = { { 0x000000007fffffff, 0x3040000000000000 }, 0, BID };
static const Number BID_MIN_UINT32  = { { 0x0000000000000000, 0x3040000000000000 }, 0, BID };
static const Number BID_MAX_UINT32  = { { 0x00000000ffffffff, 0x3040000000000000 }, 0, BID };

static const Number BID_MIN_INT64   = { { 0x8000000000000000, 0xb040000000000000 }, 0, BID };
static const Number BID_MAX_INT64   = { { 0x7fffffffffffffff, 0x3040000000000000 }, 0, BID };
static const Number BID_MIN_UINT64  = { { 0x0000000000000000, 0x3040000000000000 }, 0, BID };
static const Number BID_MAX_UINT64  = { { 0xffffffffffffffff, 0x3040000000000000 }, 0, BID };
static const Number MPZ_ZERO        = { { 0x0000000000000000, 0x0000000000000000 }, { 0, 0, 0} , MPZ };

//#define NUMBER_ZERO     BID_ZERO

//static const Number BID_MIN_INT32 = { { bid128_from_int32(INT_MIN) }}

//#define OLD_BID_MIN_INT32   bid128_from_int32(INT_MIN)
//#define OLD_BID_MAX_INT32   bid128_from_int32(INT_MAX)
//#define OLD_BID_MIN_UINT32  bid128_from_uint32(0)
//#define OLD_BID_MAX_UINT32  bid128_from_uint32(UINT_MAX)
//
//#define OLD_BID_MIN_INT64   bid128_from_int64(LLONG_MIN)
//#define OLD_BID_MAX_INT64   bid128_from_int64(LLONG_MAX)
//#define OLD_BID_MIN_UINT64  bid128_from_uint64(0)
//#define OLD_BID_MAX_UINT64  bid128_from_uint64(ULLONG_MAX)

//#define BID_MIN_INT32   { { 0x0000000080000000, 0xb040000000000000 }, NULL, BID };
//#define BID_MAX_INT32   { { 0x000000007fffffff, 0x3040000000000000 }, NULL, BID };
//#define BID_MIN_UINT32  { { 0x0000000000000000, 0x3040000000000000 }, NULL, BID };
//#define BID_MAX_UINT32  { { 0x00000000ffffffff, 0x3040000000000000 }, NULL, BID };
//
//#define BID_MIN_INT64   { { 0x8000000000000000, 0xb040000000000000 }, NULL, BID };
//#define BID_MAX_INT64   { { 0x7fffffffffffffff, 0x3040000000000000 }, NULL, BID };
//#define BID_MIN_UINT64  { { 0x0000000000000000, 0x3040000000000000 }, NULL, BID };
//#define BID_MAX_UINT64  { { 0xffffffffffffffff, 0x3040000000000000 }, NULL, BID };

//void number_copyNumber(Number *dest, const Number *src);
void number_freeNumber(Number *n);
#ifdef NUMBER_FREE_NUMBERS
Number number_fromNumber(Number *n);
#else
Number number_fromNumber(const Number *n);
#endif
//Number number_fromNumber(const Number *src);

void number_toString(Number x, char *buf, size_t len);
char *number_to_string(Number x);

Number number_from_string(char *s);

Number number_from_bid(BID_UINT128 n);

int32_t number_to_sint32(Number x);
uint32_t number_to_uint32(Number x);
int64_t number_to_sint64(Number x);
uint64_t number_to_uint64(Number x);
float number_to_float(Number x);
double number_to_double(Number x);

Number number_add(Number x, Number y);
Number number_subtract(Number x, Number y);
Number number_multiply(Number x, Number y);
Number number_divide(Number x, Number y);
Number number_modulo(Number x, Number y);
Number number_multiply(Number x, Number y);
Number number_negate(Number x);
Number number_abs(Number x);
Number number_sign(Number x);
Number number_ceil(Number x);
Number number_floor(Number x);
Number number_trunc(Number x);
Number number_exp(Number x);
Number number_log(Number x);
Number number_sqrt(Number x);
Number number_acos(Number x);
Number number_asin(Number x);
Number number_atan(Number x);
Number number_cos(Number x);
Number number_sin(Number x);
Number number_tan(Number x);
Number number_acosh(Number x);
Number number_asinh(Number x);
Number number_atanh(Number x);
Number number_atan2(Number y, Number x);
Number number_cbrt(Number x);
Number number_cosh(Number x);
Number number_erf(Number x);
Number number_erfc(Number x);
Number number_exp2(Number x);
Number number_expm1(Number x);
Number number_frexp(Number x, int *exp);
Number number_hypot(Number x, Number y);
Number number_ldexp(Number x, int exp);
Number number_lgamma(Number x);
Number number_log10(Number x);
Number number_log1p(Number x);
Number number_log2(Number x);
Number number_nearbyint(Number x);
Number number_pow(Number x, Number y);
Number number_powmod(Number b, Number e, Number m);
Number number_sinh(Number x);
Number number_tanh(Number x);
Number number_tgamma(Number x);

BOOL number_is_greater(Number x, Number y);
BOOL number_is_integer(Number x);
BOOL number_is_nan(Number x);

Number number_from_uint8(uint8_t x);
Number number_from_sint8(int8_t x);
//Number number_from_uint16(uint16_t x);
//Number number_from_sint16(int16_t x);
Number number_from_uint32(uint32_t x);
Number number_from_sint32(int32_t x);
Number number_from_uint64(uint64_t x);
Number number_from_sint64(int64_t x);
Number number_from_float(float x);
Number number_from_double(double x);

BOOL number_is_zero(Number x);
BOOL number_is_negative(Number x);
BOOL number_is_equal(Number x, Number y);
BOOL number_is_not_equal(Number x, Number y);
BOOL number_is_less(Number x, Number y);
BOOL number_is_greater(Number x, Number y);
BOOL number_is_less_equal(Number x, Number y);
BOOL number_is_greater_equal(Number x, Number y);
//BOOL number_is_integer(Number x);
BOOL number_is_odd(Number x);
//BOOL number_is_nan(Number x);

#endif
