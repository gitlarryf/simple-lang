using System;
using System.Numerics;

public class Number
{
    public enum Rep {
        MPZ,
        BID
    };

    private Decimal bid;
    //private BigInteger mpz;

    //private System.Numerics.BigInteger gmp;
#region Constructors
    public Number()
    {
        bid = 0;
    }

    public Number(char n)
    {
        bid = n;
    }

    public Number(UInt16 n)
    {
        bid = n;
    }

    public Number(UInt32 n)
    {
        bid = n;
    }

    public Number(UInt64 n)
    {
        bid = n;
    }

    public Number(int n)
    {
        bid = n;
    }

    public Number(long n)
    {
        bid = n;
    }

    public Number(Decimal n)
    {
        bid = n;
    }

    public Number(Number n)
    {
        bid = n.bid;
    }
#endregion

    public static Number FromString(string str)
    {
        return new Number(Decimal.Parse(str, System.Globalization.NumberStyles.Float));
    }

    public override string ToString()
    {
        return bid.ToString();
    }

    public static explicit operator Int64(Number n)
    {
        return Decimal.ToInt64(n.bid);
    }

    public static explicit operator UInt32(Number n)
    {
        return Decimal.ToUInt32(n.bid);
    }

    public static explicit operator UInt64(Number n)
    {
        return Decimal.ToUInt64(n.bid);
    }

    #region Static Arithmetic Functions
    public static Number Add(Number x, Number y)
    {
        return new Number(x.bid + y.bid);
    }

    public static Number Subtract(Number x, Number y)
    {
        return new Number(x.bid - y.bid);
    }

    public static Number Multiply(Number x, Number y)
    {
        return new Number(x.bid * y.bid);
    }

    public static Number Divide(Number x, Number y)
    {
        return new Number(x.bid / y.bid);
    }

    public static Number Pow(Number x, Number y)
    {
        //Decimal z = new Decimal(Math.Exp(Decimal.ToDouble(y.bid)));
        //return new Number(z);
        //return new Number(Decimal.ToUInt64(x.bid) ^ Decimal.ToUInt64(y.bid));
        if (y.IsInteger() && !y.IsNegative()) {
            UInt32 iy = (UInt32)y;
            Number r = new Number((UInt32)1);
            while (iy != 0) {
                if ((iy & 1) == 1) {
                    r = Multiply(r, x);
                }
                x = Multiply(x, x);
                iy >>= 1;
            }
            return r;
        }
        //return bid128_pow(x.get_bid(), y.get_bid());
        return new Number(Decimal.ToInt64(x.bid) ^ Decimal.ToInt64(y.bid));
    }

    public static Number Modulo(Number x, Number y)
    {
        return new Number(Decimal.Remainder(x.bid, y.bid));
    }

    public static Number Negate(Number x)
    {
        return new Number(Decimal.Negate(x.bid));
    }
    #endregion

    public bool IsZero()
    {
        return bid == Decimal.Zero;
    }

    public bool IsInteger()
    {
        Decimal i = Decimal.Round(bid, MidpointRounding.AwayFromZero);
        return Decimal.Compare(bid, i) == 0;
    }

    public bool IsNegative()
    {
        return Math.Sign(bid) == -1;
    }

#region Static "Is" functions
    static public bool IsGreaterThan(Number x, Number y)
    {
        return Decimal.Compare(x.bid, y.bid) > 0;
    }

    static public bool IsLessThan(Number x, Number y)
    {
        return Decimal.Compare(x.bid, y.bid) < 0;
    }

    static public bool IsEqual(Number x, Number y)
    {
        return Decimal.Equals(x.bid, y.bid);
    }

    static public bool IsGreaterThanOrEqual(Number x, Number y)
    {
        return Decimal.Compare(x.bid, y.bid) >= 0;
    }

    static public bool IsLessThanOrEqual(Number x, Number y)
    {
        return Decimal.Compare(x.bid, y.bid) <= 0;
    }
#endregion

    public static string number_to_string(Number x)
    {
        const int PRECISION = 34;

        char[] buf = new char[50];
        string sbuf = "";
        try {
            sbuf = x.bid.ToString();
        } catch (Exception) {
            // Inf or NaN
            return sbuf;
        }

        int E = -1;
        E = sbuf.IndexOf("E");
        if (E == -1) {
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
                sbuf.Insert(1, "0." + new string('0', -exponent - (sbuf.Length-1)));
            } else {
                exponent += sbuf.Length - 2;
                if (sbuf.Length >= 3) {
                    sbuf.Insert(2, ".");
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
