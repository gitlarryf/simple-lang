using System;
using System.Collections.Generic;

namespace csnex
{
    public class Bytecode
    {
        public Bytecode Module { get; private set; }
        public List<Bytecode> Modules { get; private set; }
        public byte[] source_hash { get; private set; }
        public int global_size { get; private set; }
        public int strtablesize { get; private set; }
        public int strtablelen { get; private set; }
        public List<String> strings { get; private set; }
        public int typesize { get; private set; }
        public int constantsize { get; private set; }
        public int variablesize { get; private set; }
        public int export_functionsize { get; private set; }
        public int functionsize { get; private set; }
        public int exceptionsize { get; private set; }
        public int exceptionexportsize { get; private set; }
        public int interfaceexportsize { get; private set; }
        public int importsize { get; private set; }
        public int classsize { get; private set; }
        public byte[] code { get; private set; }
        public int codelen { get; private set; }

        public List<Type> export_types { get; private set; }
        public List<ExportFunction> export_functions { get; private set; }
        public List<Function> functions { get; private set; }
        public List<Import> imports { get; private set; }
        public List<Exception> exceptions { get; private set; }


        public Bytecode()
        {
        }

        private int VInt(Byte[] pobj, int nBuffSize, ref int i)
        {
            int r = 0;
            while (i < nBuffSize) {
                char x = System.Text.Encoding.ASCII.GetChars(pobj)[i];
                i++;
                if ((r & ~(int.MaxValue >> 7)) != 0) {
                    throw new NeonBytecodeException(string.Format("Integer value exceeds maximum ({0})", int.MaxValue));
                }
                r = (r << 7) | (x & 0x7f);
                if ((x & 0x80) == 0) {
                    break;
                }
            }
            return r;
        }

        public List<String> GetStrTable(Byte[] obj, int size, ref int i)
        {
            List<String> r = new List<string>();

            while (i < size)
            {
                int len = VInt(obj, size, ref i);
                String ts;
                ts = new string(System.Text.Encoding.UTF8.GetChars(obj, i, len));
                r.Add(ts);
                i += len;
            }
            return r;
        }

        public void LoadBytecode(Byte[] code, int len)
        {
            int i = 0;

            System.Diagnostics.Debug.Assert((Module.source_hash.ToString() != string.Empty), "Bytecode is not empty!");

            foreach (byte h in code) {
                Module.source_hash[i++] = h;
                if (i > 32) {
                    break;
                }
            }

            Module.global_size = VInt(code, len, ref i);

            Module.strtablesize = VInt(code, len, ref i);
            Module.strings = GetStrTable(code, Module.strtablesize + i, ref i);

            Module.typesize = VInt(code, len, ref i);
            /*
            typesize = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            self.export_types = []
            while typesize > 0:
            t = Type()
            t.name = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            t.descriptor = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            self.export_types.append(t)
            typesize -= 1
            */
            Module.constantsize = VInt(code, len, ref i);
            /*
            constantsize = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            self.export_constants = []
            while constantsize > 0:
            c = Constant()
            c.name = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            c.type = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            size = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            c.value = bytecode[i:i+size]
            i += size
            self.export_constants.append(c)
            constantsize -= 1;
            */
            Module.variablesize = VInt(code, len, ref i);
            /*
            variablesize = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            self.export_variables = []
            while variablesize > 0:
            v = Variable()
            v.name = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            v.type = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            v.index = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            self.export_variables.append(v)
            variablesize -= 1
            */
            Module.export_functionsize = VInt(code, len, ref i);

            for (int f = 0; f < Module.export_functionsize; f++) {
                ExportFunction ef = new ExportFunction();
                ef.name = VInt(code, len, ref i);
                ef.descriptor = VInt(code, len, ref i);
                ef.entry = VInt(code, len, ref i);
                Module.export_functions.Add(ef);
            }
            Module.exceptionexportsize = VInt(code, len, ref i);
            /*
            exceptionexportsize = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            self.export_exceptions = []
            while exceptionexportsize > 0:
            e = ExceptionExport()
            e.name = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            self.export_exceptions.append(e)
            exceptionexportsize -= 1
            */
            Module.interfaceexportsize = VInt(code, len, ref i);
            /*
            interfaceexportsize = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            while interfaceexportsize > 0:
            assert False, interfaceexportsize
            */
            Module.importsize = VInt(code, len, ref i);
            for (int f = 0; f < Module.importsize; f++) {
                Import ip = new Import();
                ip.name = VInt(code, len, ref i);
                for (int x = i; x < i + 32; i++) {
                    ip.hash[x - i] = (char)code[x];
                }
                i += 32;
            }

            Module.functionsize = VInt(code, len, ref i);
            for (int f = 0; f < Module.functionsize; f++) {
                Function fc = new Function();
                fc.name = VInt(code, len, ref i);
                fc.entry = VInt(code, len, ref i);
                Module.functions.Add(fc);
            }

            Module.exceptionsize = VInt(code, len, ref i);
            for (int e = 0; e < Module.exceptionsize; e++) {
                Exception ex = new Exception();
                ex.start = VInt(code, len,  ref i);
                ex.end = VInt(code, len, ref i);
                ex.exid = VInt(code, len, ref i);
                ex.handler = VInt(code, len, ref i);
                Module.exceptions.Add(ex);
            }

            Module.classsize = VInt(code, len, ref i);
            /*
            classsize = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            while classsize > 0:
            assert False, classsize
            */
            code.CopyTo(Module.code, i);
            Module.codelen = len - i;
        }
    }

    public struct Type
    {
        public int name;
        public int descriptor;
    }

    public struct Function
    {
        public int name;
        public int entry;
    }

    public struct ExportFunction
    {
        public int name;
        public int descriptor;
        public int entry;
    }

    public struct Import
    {
        public int name;
        public char[] hash;
    }

    public struct Exception
    {
        public int start;
        public int end;
        public int exid;
        public int handler;
    }

    public class TBytecode
    {
        public byte[] source_hash;
        public int global_size;
        public int strtablesize;
        public int strtablelen;
        public List<String> strings;
        public int typesize;
        public int constantsize;
        public int variablesize;
        public int export_functionsize;
        public int functionsize;
        public int exceptionsize;
        public int exceptionexportsize;
        public int interfaceexportsize;
        public int importsize;
        public int classsize;
        public byte[] code;
        public int codelen;

        public List<Type> export_types;
        public List<ExportFunction> export_functions;
        public List<Function> functions;
        public List<Import> imports;
        public List<Exception> exceptions;
    }
}
