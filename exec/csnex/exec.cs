using System;

// The fields here must match the declaration of
// ExceptionInfo in ast.cpp.
namespace csnex {

    public class ExceptionInfo
    {
        public Decimal Code;
        public String Info;

        public ExceptionInfo()
        {
            Info = "";
            Code = 0;
        }

        public ExceptionInfo(string info, Decimal code)
        {
            Info = info;
            Code = code;
        }

        public ExceptionInfo(string info, UInt32 code)
        {
            Info = info;
            Code = code;
        }
    };
}
