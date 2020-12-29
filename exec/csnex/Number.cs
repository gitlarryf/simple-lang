using System;
//using System.Numerics;

public class Number {

    private Decimal number;
    //private System.Numerics.BigInteger gmp;
    #region Constructors
    public Number()
    {
        number = 0;
    }

    public Number(char n)
    {
        number = n;
    }

    public Number(UInt16 n)
    {
        number = n;
    }

    public Number(UInt32 n)
    {
        number = n;
    }

    public Number(UInt64 n)
    {
        number = n;
    }

    public Number(int n)
    {
        number = n;
    }

    public Number(long n)
    {
        number = n;
    }

    public Number(Decimal n)
    {
        number = n;
    }

    public Number(Number n)
    {
        number = n.number;
    }
    #endregion

    static public Number FromString(string str)
    {
        return new Number(Decimal.Parse(str));
    }

    public override string ToString()
    {
        return number.ToString();
    }

    public static explicit operator Int64(Number n)
    {
        return Decimal.ToInt64(n.number);
    }

    public static explicit operator UInt32(Number n)
    {
        return Decimal.ToUInt32(n.number);
    }

    public static explicit operator UInt64(Number n)
    {
        return Decimal.ToUInt64(n.number);
    }

    #region Static Arithmetic Functions
    public static Number Add(Number x, Number y)
    {
        return new Number(x.number + y.number);
    }

    public static Number Subtract(Number x, Number y)
    {
        return new Number(x.number - y.number);
    }

    public static Number Multiply(Number x, Number y)
    {
        return new Number(x.number * y.number);
    }

    public static Number Divide(Number x, Number y)
    {
        return new Number(x.number / y.number);
    }

    public static Number Powof(Number x, Number y)
    {
        Decimal z = new Decimal(Math.Exp(Decimal.ToDouble(y.number)));
        return new Number(z);
        //return new Number(Decimal.ToUInt64(x.number) ^ Decimal.ToUInt64(y.number));
    }

    public static Number Modulo(Number x, Number y)
    {
        return new Number(Decimal.Remainder(x.number, y.number));
        //long rem;
        //Math.DivRem(Decimal.ToInt64(a.number), Decimal.ToInt64(b.number), out rem);
        //return new Number(rem);
    }

    public static Number Negate(Number x)
    {
        return new Number(Decimal.Negate(x.number));
    }
    #endregion

    public bool IsZero()
    {
        return number == Decimal.Zero;
    }

    public bool IsInteger()
    {
        Decimal i = Decimal.Round(number, MidpointRounding.AwayFromZero);
        return Decimal.Compare(number, i) == 0;
    }

    public bool IsNegative()
    {
        return Math.Sign(number) == -1;
    }

    #region Static "Is" functions
    static public bool IsGreaterThan(Number x, Number y)
    {
        return Decimal.Compare(x.number, y.number) > 0;
    }

    static public bool IsLessThan(Number x, Number y)
    {
        return Decimal.Compare(x.number, y.number) < 0;
    }

    static public bool IsEqual(Number x, Number y)
    {
        return Decimal.Equals(x.number, y.number);
    }

    static public bool IsGreaterThanOrEqual(Number x, Number y)
    {
        return Decimal.Compare(x.number, y.number) >= 0;
    }

    static public bool IsLessThanOrEqual(Number x, Number y)
    {
        return Decimal.Compare(x.number, y.number) <= 0;
    }
    #endregion

    public string number_to_string(Number x)
    {
        const int PRECISION = 34;

        char[] buf = new char[50];

        string sbuf = x.number.ToString();
        int E = -1;
        //try {
            E = sbuf.IndexOf("E");
        //} catch (ArgumentNullException) {
        //    return sbuf;
        //}

        if (E == -1) {
            // Inf or NaN
            return sbuf;
        }
        int exponent = (int)Decimal.Parse(sbuf.Substring(E+1));
        sbuf = sbuf.Substring(0, E);
        char[] num = {'1','2','3','4','5','6','7','8','9'};
        int last_significant_digit = sbuf.LastIndexOfAny(num);
        if (last_significant_digit == 0) {
            return "0";
        }
        int trailing_zeros = (sbuf.Length - (last_significant_digit + 1));
        exponent += trailing_zeros;
        sbuf = sbuf.Substring(0, last_significant_digit + 1);
        if (exponent != 0) {
            if (exponent > 0 && sbuf.Length + exponent <= PRECISION+1) {
                sbuf.Insert(exponent, "0");
            } else if (exponent < 0 && -exponent < sbuf.Length-1) {
                sbuf = sbuf.Substring(0, sbuf.Length+exponent) + "." + sbuf.Substring(sbuf.Length+exponent);
            } else if (exponent < 0 && -exponent == sbuf.Length-1) {
                sbuf = sbuf.Substring(0, 1) + "0." + sbuf.Substring(1);
            } else if (exponent < 0 && sbuf.Length - exponent <= PRECISION+2) {
                //sbuf.Insert(1, "0." + new string(-exponent - (sbuf.Length-1), "0"));
            } else {
                exponent += sbuf.Length - 2;
                if (sbuf.Length >= 3) {
                    //sbuf.Insert(2, 1, ".");
                }
                sbuf += "e";
                sbuf += exponent.ToString();
            }
        }
        if (sbuf[0] == '+') {
            sbuf = sbuf.Substring(1);
        }
        return sbuf;
    }
}
