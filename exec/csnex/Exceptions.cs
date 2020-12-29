using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csnex
{
    public class ExceptionName {
        public ExceptionName(string n) {
            name = n;
        }
        public string name;
    };

    public class Exceptions {
    }

    public class NeonException : System.ApplicationException {

        public NeonException() {
        }

        public NeonException(string message) : base(message) {
        }

        public NeonException(string message, params object[] args) : base(string.Format(message, args)) {
        }

        public NeonException(string message, System.Exception innerException) : base(message, innerException) {
        }
    }


    public class NeonInvalidOpcodeException : NeonException {

        public NeonInvalidOpcodeException() {
        }

        public NeonInvalidOpcodeException(string message) : base(message) {
        }

        public NeonInvalidOpcodeException(string message, params object[] args) : base(string.Format(message, args)) {
        }

        public NeonInvalidOpcodeException(string message, Exception innerException) : base(message, innerException) {
        }
    }

    public class NeonArrayIndexException : NeonException {

        public NeonArrayIndexException() {
        }

        public NeonArrayIndexException(string message) : base(message) {
        }

        public NeonArrayIndexException(string message, params object[] args) : base(string.Format(message, args)) {
        }

        public NeonArrayIndexException(string message, Exception innerException) : base(message, innerException) {
        }
    }

    public class NeonNotImplementedException : NeonException {

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

    public class BytecodeException : NeonException {
        public BytecodeException() {
        }

        public BytecodeException(string message) : base(message) {
        }

        public BytecodeException(string message, params object[] args) : base(string.Format(message, args)) {
        }

        public BytecodeException(string message, Exception innerException) : base(message, innerException) {
        }
    }

    public class NeonDivideByZeroException : NeonException {
        public NeonDivideByZeroException() {
        }

        public NeonDivideByZeroException(string message) : base(message) {
        }

        public NeonDivideByZeroException(string message, params object[] args) : base(string.Format(message, args)) {
        }

        public NeonDivideByZeroException(string message, Exception innerException) : base(message, innerException) {
        }
    }

    public class RtlException : NeonException {
        public RtlException() {
        }

        public RtlException(string message) : base(message) {
        }

        public RtlException(string message, params object[] args) : base(string.Format(message, args)) {
        }

        public RtlException(string message, Exception innerException) : base(message, innerException) {
        }
    }

        //    namespace rtl {
        //        namespace ne_datetime { static public class ExceptionName { static Exception_TodoException = new Exceptions.ExceptionName("test"); } }
        //    };
        //    namespace ne_file { static ExceptionName Exception_FileException = { "FileException" }; }
        //    namespace ne_file { static ExceptionName Exception_FileException_DirectoryExists = { "FileException.DirectoryExists" }; }
        //    namespace ne_file { static ExceptionName Exception_FileException_Exists = { "FileException.Exists" }; }
        //    namespace ne_file { static ExceptionName Exception_FileException_Open = { "FileException.Open" }; }
        //    namespace ne_file { static ExceptionName Exception_FileException_PathNotFound = { "FileException.PathNotFound" }; }
        //    namespace ne_file { static ExceptionName Exception_FileException_PermissionDenied = { "FileException.PermissionDenied" }; }
        //    namespace ne_file { static ExceptionName Exception_FileException_Write = { "FileException.Write" }; }
        //    namespace ne_global { static public class ExceptionName { static ExceptionName Exception_ArrayIndexException = new Exceptions.ExceptionName("ArrayIndexException"); } }
        //};
        //namespace ne_global { static ExceptionName Exception_AssertFailedException = { "AssertFailedException" }; }
        //namespace ne_global { static ExceptionName Exception_ByteOutOfRangeException = { "ByteOutOfRangeException" }; }
        //namespace ne_global { static ExceptionName Exception_BytesIndexException = { "BytesIndexException" }; }
        //namespace ne_global { static ExceptionName Exception_DictionaryIndexException = { "DictionaryIndexException" }; }
        //namespace ne_global { static ExceptionName Exception_DivideByZeroException = { "DivideByZeroException" }; }
        //namespace ne_global { static ExceptionName Exception_DynamicConversionException = { "DynamicConversionException" }; }
        //namespace ne_global { static ExceptionName Exception_EndOfFileException = { "EndOfFileException" }; }
        //namespace ne_global { static ExceptionName Exception_FormatException = { "FormatException" }; }
        //namespace ne_global { static ExceptionName Exception_InvalidFunctionException = { "InvalidFunctionException" }; }
        //namespace ne_global { static ExceptionName Exception_InvalidValueException = { "InvalidValueException" }; }
        //namespace ne_global { static ExceptionName Exception_FunctionNotFoundException = { "FunctionNotFoundException" }; }
        //namespace ne_global { static ExceptionName Exception_LibraryNotFoundException = { "LibraryNotFoundException" }; }
        //namespace ne_global { static ExceptionName Exception_ObjectSubscriptException = { "ObjectSubscriptException" }; }
        //namespace ne_global { static ExceptionName Exception_SqlException = { "SqlException" }; }
        //namespace ne_global { static ExceptionName Exception_StackOverflowException = { "StackOverflowException" }; }
        //namespace ne_global { static ExceptionName Exception_StringIndexException = { "StringIndexException" }; }
        //namespace ne_global { static ExceptionName Exception_Utf8DecodingException = { "Utf8DecodingException" }; }
        //namespace ne_global { static ExceptionName Exception_ValueRangeException = { "ValueRangeException" }; }
        //namespace ne_io { static ExceptionName Exception_IoException = { "IoException" }; }
        //namespace ne_io { static ExceptionName Exception_IoException_InvalidFile = { "IoException.InvalidFile" }; }
        //namespace ne_io { static ExceptionName Exception_IoException_Open = { "IoException.Open" }; }
        //namespace ne_io { static ExceptionName Exception_IoException_Write = { "IoException.Write" }; }
        //namespace ne_mmap { static ExceptionName Exception_MmapException = { "MmapException" }; }
        //namespace ne_mmap { static ExceptionName Exception_MmapException_InvalidFile = { "MmapException.InvalidFile" }; }
        //namespace ne_mmap { static ExceptionName Exception_OpenFileException = { "OpenFileException" }; }
        //namespace ne_net { static ExceptionName Exception_SocketException = { "SocketException" }; }
        //namespace ne_os { static ExceptionName Exception_OsException = { "OsException" }; }
        //namespace ne_os { static ExceptionName Exception_OsException_InvalidProcess = { "OsException.InvalidProcess" }; }
        //namespace ne_os { static ExceptionName Exception_OsException_PathNotFound = { "OsException.PathNotFound" }; }
        //namespace ne_os { static ExceptionName Exception_OsException_Spawn = { "OsException.Spawn" }; }
        //namespace ne_process { static ExceptionName Exception_ProcessException = { "ProcessException" }; }
        //namespace ne_random { static ExceptionName Exception_RandomException = { "RandomException" }; }
        //namespace ne_runtime { static ExceptionName Exception_NativeObjectException = { "NativeObjectException" }; }
        //namespace ne_sqlite { static ExceptionName Exception_SqliteException = { "SqliteException" }; }
        //namespace ne_sqlite { static ExceptionName Exception_SqliteException_InvalidDatabase = { "SqliteException.InvalidDatabase" }; }
        //namespace ne_sqlite { static ExceptionName Exception_SqliteException_ParameterName = { "SqliteException.ParameterName" }; }
        //namespace ne_textio { static ExceptionName Exception_TextioException = { "TextioException" }; }
        //namespace ne_textio { static ExceptionName Exception_TextioException_InvalidFile = { "TextioException.InvalidFile" }; }
        //namespace ne_textio { static ExceptionName Exception_TextioException_Open = { "TextioException.Open" }; }
        //namespace ne_textio { static ExceptionName Exception_TextioException_Write = { "TextioException.Write" }; }

        //public class ExceptionNames {
        //    public List<ExceptionNames>()

        //        public static struct Exceptions.ExceptionName[] ExceptionNames = {
        //            rtl.ne_global.Exception_ArrayIndexException,
        //            rtl.ne_global.Exception_AssertFailedException,
        //            rtl.ne_global::Exception_ByteOutOfRangeException,
        //            rtl.ne_global::Exception_BytesIndexException,
        //            rtl.ne_global::Exception_DictionaryIndexException,
        //            rtl.ne_global::Exception_DivideByZeroException,
        //            ne_global::Exception_DynamicConversionException,
        //            ne_global::Exception_EndOfFileException,
        //            ne_global::Exception_FormatException,
        //            ne_global::Exception_InvalidFunctionException,
        //            ne_global::Exception_InvalidValueException,
        //            ne_global::Exception_FunctionNotFoundException,
        //            ne_global::Exception_LibraryNotFoundException,
        //            ne_global::Exception_ObjectSubscriptException,
        //            ne_global::Exception_SqlException,
        //            ne_global::Exception_StackOverflowException,
        //            ne_global::Exception_StringIndexException,
        //            ne_global::Exception_Utf8DecodingException,
        //            ne_global::Exception_ValueRangeException,
        //};
}
