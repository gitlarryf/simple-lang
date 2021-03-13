using System;

namespace csnex {

    public class Number
    {
        private Decimal val;

#region Constructors
        public Number()
        {
            val = 0;
        }

        public Number(byte n)
        {
            val = n;
        }

        public Number(Decimal n)
        {
            val = n;
        }

        public Number(Int32 n)
        {
            val = n;
        }

        public Number(Number n)
        {
            val = 0;
            if (n != null) {
                val = n.val;
            }
        }

        public Number(double d)
        {
            val = (Decimal)d;
        }

#endregion

        public static Number FromString(string str)
        {
            Decimal result;
            int i = 0;
            int skip = 0;
            if (str[i] == '+') {
                i++;
                skip = i;
            } else if (str[i] == '-') {
                i++;
            }

            int next = str.StrSpn(i, "0123456789");
            // Number must start with a digit.
            if (next == i) {
                return new Number(null);
            }
            // If all digits, then do a large integer conversion.
            if (next == -1) {
                try {
                    if (Decimal.TryParse(str.ToString(), out result)) {
                        return new Number(result);
                    }
                    //return System.Numberics.BigInteger(str.SubString(skip), 10);
                } catch {
                    return new Number(0);
                }
            }
            // Check exact allowed formats because bid128_from_string
            // is a bit too permissive.
            if (str[next] == '.') {
                i = next + 1;
                if (i >= str.Length) {
                    return new Number(null);
                }
                next = str.StrSpn(i, "0123456789");
                // Must have one or more digits after decimal point.
                if (next == i) {
                    return new Number(null);
                }
            }
            // Check for exponential notation.
            if (next != -1 && (str[next] == 'e' || str[next] == 'E')) {
                i = next + 1;
                // e may be followed by a + or -
                if (i < str.Length && (str[i] == '+' || str[i] == '-')) {
                    i++;
                }
                if (i >= str.Length) {
                    return new Number(null);
                }
                // Must have one or more digits in exponent.
                next = str.StrSpn(i, "0123456789");
                if (next == i) {
                    return new Number(null);
                }
            }
            // Reject if there is any trailing junk.
            if (next != str.Length) {
                return new Number(null);
            }
            if (Decimal.TryParse(str.Substring(skip), out result)) {
                return new Number(result);
            }
            return new Number(null);

            //return new Number(Decimal.Parse(str, System.Globalization.NumberStyles.Float));
            /*
            int i = 0;
            int npos = str.Length;

            // Skip any sign notations that may exist.
            if (str[i] == '-' || str[i] == '+') {
                i++;
            }

            // First, our number must start with a digit.
            int next = i + str.StrSpan(1, "0123456789");
            if (next == i) {
                return new Number(Double.PositiveInfinity);
            }

            // If we're dealing strictly with an integer, then we pass it off to gmp.
            // ToDo: implement libgmp!

            // Next, check for allowed formats because bid128_from_string() is a bit too permissive.
            if (next != npos && str[next] == '.') {
                i = next + 1;
                if (i >= npos) {
                    return new Number(Double.PositiveInfinity);
                }
                next = i + str.StrSpan(i, "0123456789");
                // Must have digits after a decimal point, otherwise fail.
                if (next == i) {
                    return new Number(Double.PositiveInfinity);
                }
            }
            // Check for exponential notation.
            if (next != npos && (str[next] == 'e' || str[next] == 'E')) {
                i = next + 1;
                // Exponent can be followed by a + or -...
                if (i < npos && (str[i] == '+' || str[i] == '-')) {
                    i++;
                }
                if (i >= npos) {
                    return new Number(Double.PositiveInfinity);
                }
                // ...that must have one or more digits in the exponent.
                next = i + str.StrSpan(i, "0123456789");
                if (next == i) {
                    return new Number(Double.PositiveInfinity);
                }
            }
            // Reject if we have any trailing non-numerical junk.
            if (next != npos) {
                return new Number(Double.PositiveInfinity);
            }
            // Note: bid128_from_string() takes a char*, not a const char*.
            double val = double.NaN;
            if (!Double.TryParse(str, System.Globalization.NumberStyles.Any, null, out val)) {
                return new Number(Double.PositiveInfinity);
            }
            return new Number(val);
                //Decimal.Parse(str, System.Globalization.NumberStyles.Float));
                */
        }

#region Static Arithmetic Functions
        public static Number Add(Number x, Number y)
        {
            return new Number(x.val + y.val);
        }

        public static Number Subtract(Number x, Number y)
        {
            return new Number(x.val - y.val);
        }

        public static Number Multiply(Number x, Number y)
        {
            return new Number(x.val * y.val);
        }

        public static Number Divide(Number x, Number y)
        {
            return new Number(x.val / y.val);
        }

        public static Number Pow(Number x, Number y)
        {
            if (y.IsInteger() && !y.IsNegative()) {
                UInt32 iy = number_to_uint32(y);
                Number r = new Number(1);
                while (iy != 0) {
                    if ((iy & 1) == 1) {
                        r = Multiply(r, x);
                    }
                    x = Multiply(x, x);
                    iy >>= 1;
                }
                return r;
            }
            return new Number((Decimal)(Decimal.ToInt64(x.val) ^ Decimal.ToInt64(y.val)));
        }

        public static Number Modulo(Number x, Number y)
        {
            return new Number(Decimal.Remainder(x.val, y.val));
        }

        public static Number Negate(Number x)
        {
            return new Number(Decimal.Negate(x.val));
        }

        public static Number Abs(Number x)
        {
            return new Number(Math.Abs(x.val));
        }

        public static Number Sign(Number x)
        {
            // Force clear the sign bit on x.
            // ToDo: implement this correctly.
            return new Number(Math.Sign(x.val));
        }

        public static Number Ceil(Number x)
        {
            return new Number(Math.Ceiling(x.val));
        }

        public static Number Floor(Number x)
        {
            return new Number(Math.Floor(x.val));
        }

        public static Number Trunc(Number x)
        {
            return new Number(Math.Truncate(x.val));
        }

        public static Number Exp(Number x)
        {
            return new Number(Math.Exp((double)x.val));
        }

        public static Number Log(Number x)
        {
            return new Number(Math.Log((double)x.val));
        }

        public static Number Sqrt(Number x)
        {
            return new Number(Math.Sqrt((double)x.val));
        }

        public static Number Acos(Number x)
        {
            return new Number(Math.Acos((double)x.val));
        }

        public static Number Asin(Number x)
        {
            return new Number(Math.Asin((double)x.val));
        }

        public static Number Atan(Number x)
        {
            return new Number(Math.Atan((double)x.val));
        }

        public static Number Cos(Number x)
        {
            return new Number(Math.Cos((double)x.val));
        }

        public static Number Sin(Number x)
        {
            return new Number(Math.Sin((double)x.val));
        }

        public static Number Tan(Number x)
        {
            return new Number(Math.Tan((double)x.val));
        }

        public static Number Acosh(Number x)
        {
            return new Number(Double.NaN);
            //return new Number(Math.Acsin((double)x.val));
        }

        //public static Number asinh(Number x)
        //{
        //    return bid128_asinh(x);
        //}

        //public static Number atanh(Number x)
        //{
        //    return bid128_atanh(x);
        //}

        public static Number Atan2(Number y, Number x)
        {
            return new Number(Math.Atan2((double)y.val, (double)x.val));
        }

        public static Number Cbrt(Number x)
        {
            return new Number(Double.NaN);
            //return bid128_cbrt(x);
        }

        public static Number Cosh(Number x)
        {
            return new Number(Math.Cosh((double)x.val));
        }

        public static Number Erf(Number x)
        {
            return new Number(Double.NaN);
            //return bid128_erf(x);
        }

        //public static Number erfc(Number x)
        //{
        //    return bid128_erfc(x);
        //}

        public static Number Exp2(Number x)
        {
            return new Number(Double.NaN);
            //return bid128_exp2(x);
        }

        //public static Number expm1(Number x)
        //{
        //    return bid128_expm1(x);
        //}

        //public static Number frexp(Number x, int *exp)
        //{
        //    return bid128_frexp(x, exp);
        //}

        //public static Number hypot(Number x, Number y)
        //{
        //    return bid128_hypot(x, y);
        //}

        public static Number Ldexp(Number x, int exp)
        {
            return new Number(Double.NaN);
            //return Math.bid128_ldexp(x, exp);
        }

        //public static Number lgamma(Number x)
        //{
        //    return bid128_lgamma(x);
        //}

        public static Number Log10(Number x)
        {
            return new Number(Math.Log10((double)x.val));
        }

        //public static Number log1p(Number x)
        //{
        //    return bid128_log1p(x);
        //}

        public static Number Log2(Number x)
        {
            return new Number(Math.Log((double)x.val));
        }

        public static Number NearbyInt(Number x)
        {
            return new Number(Math.Round(x.val));
        }

        //public static Number sinh(Number x)
        //{
        //    return bid128_sinh(x);
        //}

        public static Number Tanh(Number x)
        {
            return new Number(Math.Tanh((double)x.val));
        }

        //public static Number tgamma(Number x)
        //{
        //    return bid128_tgamma(x);
        //}
#endregion
#region Static "Is" Functions
        public static bool IsGreaterThan(Number x, Number y)
        {
            return Decimal.Compare(x.val, y.val) > 0;
        }

        public static bool IsLessThan(Number x, Number y)
        {
            return Decimal.Compare(x.val, y.val) < 0;
        }

        public static bool IsLessOrEqual(Number x, Number y)
        {
            return Decimal.Compare(x.val, y.val) <= 0;
        }

        public static bool IsGreaterOrEqual(Number x, Number y)
        {
            return Decimal.Compare(x.val, y.val) >= 0;
        }

        public static bool IsEqual(Number x, Number y)
        {
            return Decimal.Equals(x.val, y.val);
        }

        public static bool IsOdd(Number x)
        {
            return Decimal.Remainder(x.val, 2) == 0;
        }
#endregion
#region "Is" Tests
        public bool IsInteger()
        {
            Decimal i = Decimal.Truncate(val);
            return Decimal.Compare(val, i) == 0;
        }

        public bool IsNegative()
        {
            return Math.Sign(val) == -1;
        }

        public bool IsZero()
        {
            return val == Decimal.Zero;
        }
#endregion
#region Static Number Conversions
        public static UInt32 number_to_uint32(Number n)
        {
            return Decimal.ToUInt32(n.val);
        }

        public static Int32 number_to_int32(Number n)
        {
            return Decimal.ToInt32(n.val);
        }

        public static Int64 number_to_int64(Number n)
        {
            return Decimal.ToInt64(n.val);
        }
#endregion
#region Overrides
        public override string ToString()
        {
            return val.ToString();
        }
#endregion
        public static string number_to_string(Number x)
        {
            const int PRECISION = 34;

            char[] buf = new char[50];
            string sbuf = "";
            try {
                sbuf = x.val.ToString();
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
}
