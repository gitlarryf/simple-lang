using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csnex.rtl
{
    public class math
    {
        private Executor Exec;
        public math(Executor exe)
        {
            Exec = exe;
        }

        public void abs()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Abs(x)));
        }

        public void acos()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Acos(x)));
        }

        public void acosh()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Abs(x)));
        }

        public void asin()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Asin(x)));
        }

        public void asinh()
        {
            //Number x = Exec.stack.Pop().Number;

            //Exec.stack.Push(new Cell(Number.Asinh(x)));
        }

        public void atan()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Atan(x)));
        }

        public void atanh()
        {
            //Number x = Exec.stack.Pop().Number;

            //Exec.stack.Push(new Cell(Number.Atanh(x)));
        }

        public void atan2()
        {
            Number x = Exec.stack.Pop().Number;
            Number y = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Atan2(y, x)));
        }

        public void cbrt()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Cbrt(x)));
        }

        public void ceil()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Ceil(x)));
        }

        public void cos()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Cos(x)));
        }

        public void cosh()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Cosh(x)));
        }

        public void erf()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Erf(x)));
        }

        public void erfc()
        {
            //Number x = Exec.stack.Pop().Number;

            //Exec.stack.Push(new Cell(Number.Etfc(x)));
        }

        public void exp()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Exp(x)));
        }

        public void exp2()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Exp2(x)));
        }

        public void expm1()
        {
            //Number x = Exec.stack.Pop().Number;

            //Exec.stack.Push(new Cell(Number.Expm1(x)));
        }

        public void floor()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Floor(x)));
        }

        public void frexp()
        {
            //Number x = Exec.stack.Pop().Number;

            //int iexp;
            //Number r = Number.Frexp(x, out iexp);

            //Exec.stack.Push(new Cell(r));
            //Exec.stack.Push(new Cell(new Number(iexp)));
        }

        public void hypot()
        {
            //Number x = Exec.stack.Pop().Number;
            //Number y = Exec.stack.Pop().Number;

            //Exec.stack.Push(new Cell(Number.Hypot(y, x)));
        }

        public void intdiv()
        {
            Number y = Exec.stack.Pop().Number;
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Trunc(Number.Divide(x, y))));
        }

        public void ldexp()
        {
            Number exp = Exec.stack.Pop().Number;
            Number x = Exec.stack.Pop().Number;

            if (!exp.IsInteger()) {
                Exec.Raise("ValueRangeException", exp.ToString());
                return;
            }
            Exec.stack.Push(new Cell(Number.Ldexp(x, Number.number_to_int32(exp))));
        }

        public void lgamma()
        {
            //Number x = Exec.stack.Pop().Number;

            //Exec.stack.Push(new Cell(Number.LGamma(x)));
        }

        public void log()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Log(x)));
        }

        public void log10()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Log10(x)));
        }

        public void log1p()
        {
            //Number x = Exec.stack.Pop().Number;

            //Exec.stack.Push(new Cell(Number.Log1p(x)));
        }

        public void log2()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Log2(x)));
        }

        public void max()
        {
            Number b = Exec.stack.Pop().Number;
            Number a = Exec.stack.Pop().Number;

            if (Number.IsGreaterThan(a, b)) {
                Exec.stack.Push(new Cell(a));
            } else {
                Exec.stack.Push(new Cell(b));
            }
        }

        public void min()
        {
            Number b = Exec.stack.Pop().Number;
            Number a = Exec.stack.Pop().Number;

            if (Number.IsGreaterThan(a, b)) {
                Exec.stack.Push(new Cell(b));
            } else {
                Exec.stack.Push(new Cell(a));
            }
        }

        public void nearbyint()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.NearbyInt(x)));
        }

        public void odd()
        {
            Number n = Exec.stack.Pop().Number;

            if (!n.IsInteger()) {
                Exec.Raise("ValueRangeException", "odd() requres integer");
                return;
            }

            Exec.stack.Push(new Cell(Number.IsOdd(Number.NearbyInt(n))));
        }

        public void round()
        {
            Number value = Exec.stack.Pop().Number;
            Number places = Exec.stack.Pop().Number;

            Number scale = new Number(Decimal.One);
            Number ten = new Number(10);
            for (int i = Number.number_to_int32(places); i > 0; i--) {
                scale = Number.Multiply(scale, ten);
            }
            Exec.stack.Push(new Cell(Number.Divide(Number.NearbyInt(Number.Multiply(value, scale)), scale)));
        }

        public void sign()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Sign(x)));
        }

        public void sin()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Sin(x)));
        }

        public void sinh()
        {
            //Number x = Exec.stack.Pop().Number;

            //Exec.stack.Push(new Cell(Number.Sinh(x)));
        }

        public void sqrt()
        {
            //Number x = top(exec->stack)->number; pop(exec->stack);

            //if (number_is_negative(x)) {
            //    exec->rtl_raise(exec, "ValueRangeException", number_to_string(x));
            //    return;
            //}

            //push(exec->stack, cell_fromNumber(number_sqrt(x)));
        }

        public void tan()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Tan(x)));
        }

        public void tanh()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Tanh(x)));
        }

        public void tgamma()
        {
            //Number x = Exec.stack.Pop().Number;

            //Exec.stack.Push(new Cell(Number.Tgamma(x)));
        }

        public void trunc()
        {
            Number x = Exec.stack.Pop().Number;

            Exec.stack.Push(new Cell(Number.Trunc(x)));
        }

    }
}
