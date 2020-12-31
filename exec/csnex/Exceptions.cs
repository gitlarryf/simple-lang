using System;

namespace csnex
{
    [Serializable()]
    public class NeonException: ApplicationException
    {
        public NeonException() {
        }

        public NeonException(string message) : base(message) {
        }

        public NeonException(string message, params object[] args) : base(string.Format(message, args)) {
        }

        public NeonException(string message, System.Exception innerException) : base(message, innerException) {
        }
    }

    [Serializable]
    public class NeonInvalidOpcodeException: NeonException
    {
        public NeonInvalidOpcodeException() {
        }

        public NeonInvalidOpcodeException(string message) : base(message) {
        }
    }

    [Serializable]
    public class BytecodeException: NeonException 
    {
        public BytecodeException() {
        }

        public BytecodeException(string message) : base(message) {
        }
    }

    [Serializable]
    public class NeonNotImplementedException: NeonException
    {
        public NeonNotImplementedException() {
        }

        public NeonNotImplementedException(string message) : base(message) {
        }
    }
}
