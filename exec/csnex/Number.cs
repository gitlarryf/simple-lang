using System;
using System.Numerics;

public class Number {

    private Decimal number;
    private System.Numerics.BigInteger gmp;

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

    static public Number FromString(string str)
    {
        return new Number(Decimal.Parse(str));
    }

    public override string ToString()
    {
        return number.ToString();
    }


    static public Number Add(Number x, Number y)
    {
        return new Number(x.number + y.number);
    }

    static public Number Subtract(Number x, Number y)
    {
        return new Number(x.number - y.number);
    }

    static public Number Multiply(Number x, Number y)
    {
        return new Number(x.number * y.number);
    }

    static public Number Divide(Number x, Number y)
    {
        return new Number(x.number / y.number);
    }

    static public Number Powof(Number x, Number y)
    {
        Decimal z = new Decimal(Math.Exp(Decimal.ToDouble(y.number)));
        return new Number(z);
        //return new Number(Decimal.ToUInt64(x.number) ^ Decimal.ToUInt64(y.number));
    }

    static public Number Modulo(Number x, Number y)
    {
        return new Number(Decimal.Remainder(x.number, y.number));
        //long rem;
        //Math.DivRem(Decimal.ToInt64(a.number), Decimal.ToInt64(b.number), out rem);
        //return new Number(rem);
    }

    static public Number Negate(Number x)
    {
        return new Number(Decimal.Negate(x.number));
    }


    public bool IsZero()
    {
        return number == Decimal.Zero;
    }

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
}
