using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csnex
{
    public class NeonException : System.ApplicationException
    {

        public NeonException()
        {
        }

        public NeonException(string message) : base(message)
        {
        }

        public NeonException(string message, params object[] args) : base(string.Format(message, args))
        {
        }

        public NeonException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }


    public class NeonInvalidOpcodeException : NeonException
    {

        public NeonInvalidOpcodeException()
        {
        }

        public NeonInvalidOpcodeException(string message) : base(message)
        {
        }

        public NeonInvalidOpcodeException(string message, params object[] args) : base(string.Format(message, args))
        {
        }

        public NeonInvalidOpcodeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class NeonNotImplementedException : NeonException
    {

        //public InvalidNeonOpcodeException()
        //{
        //}

        //public InvalidNeonOpcodeException(string message) : base(message)
        //{
        //}

        //public InvalidNeonOpcodeException(string message, params object[] args) : base(string.Format(message, args))
        //{
        //}

        //public InvalidNeonOpcodeException(string message, Exception innerException) : base(message, innerException)
        //{
        //}
    }

    public class NeonBytecodeException : NeonException
    {
        public NeonBytecodeException()
        {
        }

        public NeonBytecodeException(string message) : base(message)
        {
        }

        public NeonBytecodeException(string message, params object[] args) : base(string.Format(message, args))
        {
        }

        public NeonBytecodeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
