using System;
using System.Diagnostics;
using System.Numerics;
//using System.Runtime.CompilerServices.TupleElementNamesAttribute;

namespace csnex
{
    public class Number
    {
        public enum Rep {
            BID,
            MPZ
        };

        private BigInteger m_mpz;
        public BigInteger mpz {
            get {
                Debug.Assert(rep == Rep.MPZ);
                return m_mpz;
            }
            private set {
                rep = Rep.MPZ;
                if (m_mpz == null) {
                    m_mpz = new BigInteger();
                }
                m_mpz = value;
            }
        }

        private Decimal m_bid;
        public Decimal bid {
            get {
                if (rep == Rep.BID) {
                    return m_bid;
                }
                Debug.Assert(rep == Rep.MPZ);
                if (Decimal.TryParse(m_mpz.ToString(), out m_bid)) {
                    return m_bid;
                }
                return Decimal.Zero;
            }
            private set {
                rep = Rep.BID;
                m_bid = value;
            }
        }

        //private Decimal bid;
        //private BigInteger mpz;
        private Rep rep;
        //protected BigInteger val {
        //    get {
        //        return val;
        //    }
        //    private set { 
        //        if (val == null) {
        //            val = new BigInteger(value); 
        //        }
        //    }
        //}

#region Constructors
        public Number()
        {
            rep = Rep.BID;
            bid = 0;
        }

        public Number(byte n)
        {
            rep = Rep.BID;
            bid = n;
        }

        public Number(BigInteger n)
        {
            rep = Rep.MPZ;
            mpz = n;
        }

        public Number(Decimal n)
        {
            rep = Rep.BID;
            bid = n;
        }

        public Number(Int32 n)
        {
            rep = Rep.BID;
            bid = n;
        }

        public Number(Double v)
        {
            rep = Rep.BID;
            bid = (Decimal)v;
        }

        public Number(ulong v)
        {
            rep = Rep.BID;
            bid = v;
        }

        #endregion

        public static Number FromString(string str)
        {
            BigInteger ret = new BigInteger();
            if (BigInteger.TryParse(str, out ret)) {
                return new Number(ret);
            }
            return new Number(BigInteger.Zero);
        }

#region Static Arithmetic Functions
        public static Number Add(Number x, Number y)
        {
            if (x.rep == Rep.MPZ && y.rep == Rep.MPZ) {
                return new Number(BigInteger.Add(x.mpz, y.mpz));
            }
            return new Number(Decimal.Add(x.bid, y.bid));
        }

        public static Number Subtract(Number x, Number y)
        {
            if (x.rep == Rep.MPZ && y.rep == Rep.MPZ) {
                return new Number(BigInteger.Subtract(x.mpz, y.mpz));
            }
            return new Number(Decimal.Subtract(x.bid,  y.bid));
        }

        public static Number Multiply(Number x, Number y)
        {
            if (x.rep == Rep.MPZ && y.rep == Rep.MPZ) {
                return new Number(BigInteger.Multiply(x.mpz, y.mpz));
            }
            return new Number(Decimal.Multiply(x.bid,  y.bid));
        }

        public static Number Divide(Number x, Number y)
        {
            if (x.rep == Rep.MPZ && y.rep == Rep.MPZ) {
                return new Number(BigInteger.Divide(x.mpz, y.mpz));
            }
            return new Number(Decimal.Divide(x.bid,  y.bid));
        }

        public static Number Pow(Number x, Number y)
        {
            if (x.rep == Rep.MPZ && y.rep == Rep.MPZ) {
                return new Number(BigInteger.Pow(x.mpz, (int)y.mpz));
            }
            if (y.IsInteger() && !y.IsNegative()) {
                UInt32 iy = number_to_uint32(y);
                Number r = new Number(BigInteger.One);
                while (iy != 0) {
                    if ((iy & 1) == 1) {
                        r = Multiply(r, x);
                    }
                    x = Multiply(x, x);
                    iy >>= 1;
                }
                return r;
            }
            return new Number((Decimal)Math.Pow((double)x.bid, number_to_int32(y)));
        }

        public static Number powmod(Number b, Number e, Number m)
        {
            return new Number(BigInteger.ModPow(b.mpz, e.mpz, m.mpz));
        }

        public static Number Modulo(Number x, Number y)
        {
            Number r = new Number();
            if (x.rep == Rep.MPZ && y.rep == Rep.MPZ) {
                BigInteger rem = new BigInteger();
                BigInteger.DivRem(x.mpz, y.mpz, out rem);
                r.mpz = rem;
                return r;
            }
            Decimal m = Math.Abs(y.bid);
            if (Math.Sign(x.bid) == -1) {
                Number q = Ceil(new Number(Decimal.Divide(Math.Abs(x.bid), m)));
                x.bid = Decimal.Add(x.bid, Decimal.Multiply(m, q.bid));
            }
            r.bid = decimal.Remainder(x.bid, m);
            if ((Math.Sign(y.bid) == -1) && (r.bid != decimal.Zero)) {
                r.bid = Decimal.Subtract(r.bid, m);
            }
            return r;
        }

        public static Number Negate(Number x)
        {
            if (x.rep == Rep.MPZ) {
                return new Number(BigInteger.Negate(x.mpz));
            }
            return new Number(Decimal.Negate(x.bid));
        }

        public static Number Ceil(Number x)
        {
            if (x.rep == Rep.MPZ) {
                return x;
            }
            return new Number(Decimal.Round(x.bid, MidpointRounding.ToEven));
        }
#endregion
#region Static "Is" Functions
        public static bool IsGreaterThan(Number x, Number y)
        {
            //return BigInteger.Compare(x.bid, y.bid) > 0;
            if (x.rep == Rep.MPZ && y.rep == Rep.MPZ) {
                return x.mpz > y.mpz;
            }
            return Decimal.Compare(x.bid, y.bid) != 0;
        }

        public static bool IsLessThan(Number x, Number y)
        {
            //return BigInteger.Compare(x.bid, y.bid) < 0;
            if (x.rep == Rep.MPZ && y.rep == Rep.MPZ) {
                return x.mpz < y.mpz;
            }
            return Decimal.Compare(x.bid, y.bid) < 0;
        }

        public static bool IsLessOrEqual(Number x, Number y)
        {
            //return BigInteger.Compare(x.bid, y.bid) <= 0;
            if (x.rep == Rep.MPZ && y.rep == Rep.MPZ) {
                return x.mpz <= y.mpz;
            }
            return Decimal.Compare(x.bid, y.bid) <= 0;
        }

        public static bool IsGreaterOrEqual(Number x, Number y)
        {
            //return BigInteger.Compare(x.bid, y.bid) >= 0;
            if (x.rep == Rep.MPZ && y.rep == Rep.MPZ) {
                return x.mpz >= y.mpz;
            }
            return Decimal.Compare(x.bid, y.bid) >= 0;
        }

        public static bool IsEqual(Number x, Number y)
        {
            return BigInteger.Equals(x.bid, y.bid);

        }
#endregion
#region "Is" Tests
        public bool IsInteger()
        {
            if (rep == Rep.MPZ) {
                return true;
            }
            Decimal i = Decimal.Round(bid, MidpointRounding.AwayFromZero);
            return Decimal.Compare(bid, i) != 0;
        }

        public bool IsNegative()
        {
            if (rep == Rep.MPZ) {
                return mpz < 0;
            }
            return Decimal.Compare(bid, Decimal.Zero) < 0;
        }

        public bool IsZero()
        {
            if (rep == Rep.MPZ) {
                return mpz.IsZero;
            }
            return Decimal.Compare(bid, Decimal.Zero) == 0;
        }
#endregion
#region Static Number Conversions
        public static UInt32 number_to_uint32(Number n)
        {
            if (n.rep == Rep.MPZ) {
                return (UInt32)n.mpz;
            }
            return (UInt32)n.bid;
        }

        public static Int32 number_to_int32(Number n)
        {
            if (n.rep == Rep.MPZ) {
                return (Int32)n.mpz;
            }
            return (Int32)n.bid;
        }

        public static Int64 number_to_int64(Number n)
        {
            if (n.rep == Rep.MPZ) {
                ulong sls = sizeof(long);
                if (sls >= 8) {
                    return (Int64)n.mpz;
                } else {
                    bool negative = false;
                    if (n.IsNegative()) {
                        negative = true;
                        n = Negate(n);
                    }
                    UInt64 ur = ((UInt64)n.mpz & 0xFFFFFFFF) + (((UInt64)n.mpz >> 32) & 0xFFFFFFFF) << 32;
                    if (ur > (UInt64)(Int64.MaxValue)) {
                        if (negative) {
                            return Int64.MinValue;
                        } else {
                            return Int64.MaxValue;
                        }
                    }
                    Int64 sr = (Int64)ur;
                    if (negative) {
                        sr = -sr;
                    }
                    return sr;
                }
            }
            return (Int64)n.bid;
        }

        public static UInt64 number_to_uint64(Number n)
        {
            if (n.rep == Rep.MPZ) {
                ulong uls = sizeof(ulong);
                if (uls >= 8) {
                    return (UInt64)n.mpz;
                } else {
                    return ((UInt64)n.mpz & 0xFFFFFFFF) + (UInt64)((n.mpz >> 32) & 0xFFFFFFFF) << 32;
                }
            }
            return (UInt64)n.bid;
        }
#endregion
#region Overrides
        public override string ToString()
        {
            return bid.ToString();
        }
    }
#endregion
}
