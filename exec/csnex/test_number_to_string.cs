using System;
using System.Diagnostics;

namespace test_number_to_string_cs
{
    class test_number_to_string_cs
    {
        static void Main(string[] args)
        {
            Debug.Assert(Number.number_to_string(new Number(Decimal.One)) == "1");
            Debug.Assert(Number.number_to_string(new Number(Decimal.MinusOne)) == "-1");
            Debug.Assert(Number.number_to_string(new Number(Decimal.MaxValue)) == "79228162514264337593543950335");
            Debug.Assert(Number.number_to_string(new Number(Decimal.MinValue)) == "-79228162514264337593543950335");

            Debug.Assert(Number.number_to_string(Number.FromString("1234.5678e5")) == "123456780");
            Debug.Assert(Number.number_to_string(Number.FromString("1234.5678e4")) == "12345678");
            Debug.Assert(Number.number_to_string(Number.FromString("1234.5678e3")) == "1234567.8");
            Debug.Assert(Number.number_to_string(Number.FromString("1234.5678e2")) == "123456.78");
            Debug.Assert(Number.number_to_string(Number.FromString("1234.5678e1")) == "12345.678");
            Debug.Assert(Number.number_to_string(Number.FromString("1234.5678e0")) == "1234.5678");
            Debug.Assert(Number.number_to_string(Number.FromString("1234.5678e-1")) == "123.45678");
            Debug.Assert(Number.number_to_string(Number.FromString("1234.5678e-2")) == "12.345678");
            Debug.Assert(Number.number_to_string(Number.FromString("1234.5678e-3")) == "1.2345678");
            Debug.Assert(Number.number_to_string(Number.FromString("1234.5678e-4")) == "0.12345678");
            Debug.Assert(Number.number_to_string(Number.FromString("1234.5678e-5")) == "0.012345678");
            Debug.Assert(Number.number_to_string(Number.FromString("-12345678e-10")) == "-0.0012345678");
            Debug.Assert(Number.number_to_string(Number.FromString("-12345678e-26")) == "-0.00000000000000000012345678");
            Debug.Assert(Number.number_to_string(Number.FromString("+12345678e-26")) == "0.00000000000000000012345678");
        }
    }
}
