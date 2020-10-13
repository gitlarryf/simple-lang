#include "math.h"

#include <iso646.h>

#include "cell.h"
#include "exec.h"
#include "number.h"
#include "stack.h"


void math_abs(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);
    
    Cell *r = cell_newCellType(cNumber);
    r->number = number_abs(x);
    push(exec->stack, r);
    number_freeNumber(&x);
}

void math_acos(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_acos(x)));
    number_freeNumber(&x);
}

void math_acosh(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_acosh(x)));
    number_freeNumber(&x);
}

void math_asin(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_asin(x)));
    number_freeNumber(&x);
}

void math_asinh(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_asinh(x)));
    number_freeNumber(&x);
}

void math_atan(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_atan(x)));
    number_freeNumber(&x);
}

void math_atanh(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_atanh(x)));
    number_freeNumber(&x);
}

void math_atan2(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);
    Number y = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_atan2(y, x)));
    number_freeNumber(&x);
    number_freeNumber(&y);
}

void math_cbrt(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_cbrt(x)));
    number_freeNumber(&x);
}

void math_ceil(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_ceil(x)));
    number_freeNumber(&x);
}

void math_cos(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_cos(x)));
    number_freeNumber(&x);
}

void math_cosh(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_cosh(x)));
    number_freeNumber(&x);
}

void math_erf(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_erf(x)));
    number_freeNumber(&x);
}

void math_erfc(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_erfc(x)));
    number_freeNumber(&x);
}

void math_exp(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_exp(x)));
    number_freeNumber(&x);
}

void math_exp2(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_exp2(x)));
    number_freeNumber(&x);
}

void math_expm1(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_expm1(x)));
    number_freeNumber(&x);
}

void math_floor(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_floor(x)));
    number_freeNumber(&x);
}

void math_frexp(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    int iexp;
    Number r = number_frexp(x, &iexp);

    push(exec->stack, cell_fromNumber(r));
    push(exec->stack, cell_fromNumber(number_from_sint32(iexp)));
    number_freeNumber(&x);
    number_freeNumber(&r);
}

void math_hypot(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);
    Number y = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_hypot(y, x)));
    number_freeNumber(&x);
    number_freeNumber(&y);
}

void math_intdiv(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);
    Number y = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_trunc(number_divide(x, y))));
    number_freeNumber(&x);
    number_freeNumber(&y);
}

void math_ldexp(TExecutor *exec)
{
    Number exp = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    if (!number_is_integer(exp)) {
        // TODO: more specific exception?
        exec->rtl_raise(exec, "ValueRangeException", number_to_string(exp), BID_ZERO);
        number_freeNumber(&exp);
        number_freeNumber(&x);
        return;
    }

    push(exec->stack, cell_fromNumber(number_ldexp(x, number_to_sint32(exp))));
    number_freeNumber(&exp);
    number_freeNumber(&x);
}

void math_lgamma(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_lgamma(x)));
    number_freeNumber(&x);
}

void math_log(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_log(x)));
    number_freeNumber(&x);
}

void math_log10(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_log10(x)));
    number_freeNumber(&x);
}

void math_log1p(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_log1p(x)));
    number_freeNumber(&x);
}

void math_log2(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_log2(x)));
    number_freeNumber(&x);
}

void math_max(TExecutor *exec)
{
    Number b = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);
    Number a = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    if (number_is_greater(a, b)) {
        push(exec->stack, cell_fromNumber(a));
    } else {
        push(exec->stack, cell_fromNumber(b));
    }
    number_freeNumber(&b);
    number_freeNumber(&a);
}

void math_min(TExecutor *exec)
{
    Number b = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);
    Number a = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    if (number_is_greater(a, b)) {
        push(exec->stack, cell_fromNumber(b));
    } else {
        push(exec->stack, cell_fromNumber(a));
    }
    number_freeNumber(&b);
    number_freeNumber(&a);
}

void math_nearbyint(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_nearbyint(x)));
    number_freeNumber(&x);
}

void math_odd(TExecutor *exec)
{
    Number n = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    if (!number_is_integer(n)) {
        exec->rtl_raise(exec, "ValueRangeException", "odd() requires integer", BID_ZERO);
        number_freeNumber(&n);
        return;
    }

    push(exec->stack, cell_fromBoolean(number_is_odd(n)));
    number_freeNumber(&n);
}

void math_powmod(TExecutor *exec)
{
    Number m = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);
    Number e = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);
    Number b = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    if (!number_is_integer(b)) {
        exec->rtl_raise(exec, "ValueRangeException", number_to_string(b), BID_ZERO);
        return;
    }
    if (!number_is_integer(e)) {
        exec->rtl_raise(exec, "ValueRangeException", number_to_string(e), BID_ZERO);
        return;
    }
    if (!number_is_integer(m)) {
        exec->rtl_raise(exec, "ValueRangeException", number_to_string(m), BID_ZERO);
        return;
    }
    Cell *r = cell_newCellType(cNumber);
    r->number = number_powmod(b, e, m);
    
    push(exec->stack, r);
    number_freeNumber(&m);
    number_freeNumber(&e);
    number_freeNumber(&b);
}

void math_round(TExecutor *exec)
{
    Number value = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);
    Number places = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    Number scale = number_from_uint32(1);
    for (int i = number_to_sint32(places); i > 0; i--) {
        scale = number_multiply(scale, number_from_uint32(10));
    }
    push(exec->stack, cell_fromNumber(number_divide(number_nearbyint(number_multiply(value, scale)), scale)));
    number_freeNumber(&value);
    number_freeNumber(&places);
}

void math_sign(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    Cell *r = cell_newCellType(cNumber);
    r->number = number_sign(x);

    push(exec->stack, r);
    number_freeNumber(&x);
}

void math_sin(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_sin(x)));
    number_freeNumber(&x);
}

void math_sinh(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_sinh(x)));
    number_freeNumber(&x);
}

void math_sqrt(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    if (number_is_negative(x)) {
        exec->rtl_raise(exec, "ValueRangeException", number_to_string(x), BID_ZERO);
        number_freeNumber(&x);
        return;
    }

    push(exec->stack, cell_fromNumber(number_sqrt(x)));
    number_freeNumber(&x);
}

void math_tan(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_tan(x)));
    number_freeNumber(&x);
}

void math_tanh(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_tanh(x)));
    number_freeNumber(&x);
}

void math_tgamma(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_tgamma(x)));
    number_freeNumber(&x);
}

void math_trunc(TExecutor *exec)
{
    Number x = number_fromNumber(&top(exec->stack)->number); pop(exec->stack);

    push(exec->stack, cell_fromNumber(number_trunc(x)));
    number_freeNumber(&x);
}
