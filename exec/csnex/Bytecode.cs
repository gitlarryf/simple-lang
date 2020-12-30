using System;
using System.Collections.Generic;

namespace csnex
{
    public class Bytecode
    {
        private byte[] obj;
        public string source_path;
        public byte[] source_hash;
        public uint version;
        public uint global_size;
        public List<String> strtable;
        public uint strtablesize;
        //private uint strtablelen;
        private uint typesize;
        private uint constantsize;
        private uint variablesize;
        private uint export_functionsize;
        private uint functionsize;
        private uint exceptionsize;
        private uint exceptionexportsize;
        private uint interfaceexportsize;
        private uint importsize;
        private uint classsize;
        public byte[] code;
        public uint codelen;

        public List<Type> export_types;
        public List<Constant> export_constants;
        public List<Variable> export_variables;
        public List<Function> export_functions;
        public List<ExceptionExport> export_exceptions;
        public List<Interface> export_interfaces;
        public List<ModuleImport> imports;
        public List<FunctionInfo> functions;
        public List<ExceptionInfo> exceptions;
        public List<ClassInfo> classes;
        public List<Cell> globals;

        public Bytecode()
        {
        }

        public static UInt32 get_vint(byte[] pobj, uint nBuffSize, ref uint i)
        {
            uint r = 0;
            while (i < nBuffSize) {
                byte x = pobj[i];
                i++;
                if ((r & ~(uint.MaxValue >> 7)) != 0) {
                    throw new BytecodeException(string.Format("Integer value exceeds maximum ({0})", int.MaxValue));
                }
                r = (r << 7) | (uint)(x & 0x7f);
                if ((x & 0x80) == 0) {
                    break;
                }
            }
            return r;
        }

        public List<String> GetStrTable(byte[] obj, uint size, ref uint i)
        {
            List<String> r = new List<string>();

            while (i < size)
            {
                uint len = get_vint(obj, size, ref i);
                String ts;
                ts = new string(System.Text.Encoding.UTF8.GetChars(obj, (int)i, (int)len));
                r.Add(ts);
                i += len;
            }
            return r;
        }

        public void LoadBytecode(string a_source_path, byte[] bytes, uint len)
        {
            source_path = a_source_path;
            obj = bytes;
            uint i = 0;

            if (i + 4 > obj.Length) {
                throw new BytecodeException("unexpected end of bytecode");
            }
            string sig = new string(System.Text.Encoding.ASCII.GetChars(obj, (int)i, 4));
            if (sig != "Ne\0n") {
                throw new BytecodeException("bytecode signature missing");
            }
            i += 4;

            version = get_vint(obj, len, ref i);
            if(version != NeonOpcode.OPCODE_VERSION) {
                throw new BytecodeException("bytecode version mismatch");
            }

            if (i + 32 > obj.Length) {
                throw new BytecodeException("unexpected end of bytecode");
            }

            source_hash = obj.CopyFrom(i, 32);
            i += 32;

            /* Globals */
            global_size = get_vint(obj, len, ref i);
            globals = new List<Cell>((int)global_size);

            /* String Table */
            strtablesize = get_vint(obj, len, ref i);
            strtable = GetStrTable(obj, strtablesize + i, ref i);

            /* Types */
            typesize = get_vint(obj, len, ref i);
            export_types = new List<Type>();
            while (typesize > 0) {
                Type t = new Type();
                t.name = get_vint(obj, len, ref i);
                t.descriptor = get_vint(obj, len, ref i);
                export_types.Add(t);
                typesize--;
            }

            /* Constants */
            constantsize = get_vint(obj, len, ref i);
            export_constants = new List<Constant>();
            while (constantsize > 0) {
                Constant cv = new Constant();
                cv.name = get_vint(obj, len, ref i);
                cv.type = get_vint(obj, len, ref i);
                uint size = get_vint(obj, len, ref i);
                if (i + size > len) {
                    throw new BytecodeException("unexpected end of bytecode");
                }
                cv.value = obj.CopyFrom(i, (int)size);
                i += size;
                export_constants.Add(cv);
                constantsize--;
            }

            /* Exported Variabes */
            variablesize = get_vint(obj, len, ref i);
            export_variables = new List<Variable>();
            while (variablesize > 0) {
                Variable v = new Variable();
                v.name = get_vint(obj, len, ref i);
                v.type = get_vint(obj, len, ref i);
                v.index = get_vint(obj, len, ref i);
                export_variables.Add(v);
                variablesize--;
            }

            /* Exported Functions */
            export_functionsize = get_vint(obj, len, ref i);
            export_functions = new List<Function>();
            while (export_functionsize > 0) {
                Function ef = new Function();
                ef.name = get_vint(obj, len, ref i);
                ef.descriptor = get_vint(obj, len, ref i);
                ef.index = get_vint(obj, len, ref i);
                export_functions.Add(ef);
                export_functionsize--;
            }

            /* Exported Exceptions */
            exceptionexportsize = get_vint(obj, len, ref i);
            export_exceptions = new List<ExceptionExport>();
            while (exceptionexportsize > 0) {
                ExceptionExport e = new ExceptionExport();
                e.name = get_vint(obj, len, ref i);
                export_exceptions.Add(e);
                exceptionsize--;
            }

            /* Exported Interfaces */
            interfaceexportsize = get_vint(obj, len, ref i);
            /*
            interfaceexportsize = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            while interfaceexportsize > 0:
            assert False, interfaceexportsize
            */

            /* Imported Modules */
            imports = new List<ModuleImport>();
            importsize = get_vint(obj, len, ref i);
            while (importsize > 0) {
                ModuleImport imp = new ModuleImport();
                imp.name = get_vint(obj, len, ref i);
                imp.optional = get_vint(obj, len, ref i) != 0;
                if (i + 32 > obj.Length) {
                    throw new BytecodeException("unexpected end of bytecode");
                }
                imp.hash = obj.CopyFrom(i, 32);
                imports.Add(imp);
                i += 32;
                importsize--;
            }

            /* Functions */
            functions = new List<FunctionInfo>();
            functionsize = get_vint(obj, len, ref i);
            while (functionsize > 0) {
                FunctionInfo fi = new FunctionInfo();
                fi.name = get_vint(obj, len, ref i);
                fi.nest = get_vint(obj, len, ref i);
                fi.args = get_vint(obj, len, ref i);
                fi.locals = get_vint(obj, len, ref i);
                fi.entry = get_vint(obj, len, ref i);
                functions.Add(fi);
                functionsize--;
            }

            /* Exception Handlers */
            exceptions = new List<ExceptionInfo>();
            exceptionsize = get_vint(obj, len, ref i);
            while (exceptionsize > 0) {
                ExceptionInfo ex = new ExceptionInfo();
                ex.start = get_vint(obj, len,  ref i);
                ex.end = get_vint(obj, len, ref i);
                ex.exid = get_vint(obj, len, ref i);
                ex.handler = get_vint(obj, len, ref i);
                ex.stack_depth = get_vint(obj, len, ref i);
                exceptions.Add(ex);
                exceptionsize--;
            }

            /* Classes */
            classes = new List<ClassInfo>();
            classsize = get_vint(obj, len, ref i);
            /*
            classsize = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            while classsize > 0:
            assert False, classsize
            */
            codelen = len - i;
            code = obj.CopyFrom(i, (int)codelen);


    //unsigned int interfaceexportsize = get_vint(obj, i);
    //while (interfaceexportsize > 0) {
    //    Interface iface;
    //    iface.name = get_vint(obj, i);
    //    unsigned int methoddescriptorsize = get_vint(obj, i);
    //    while (methoddescriptorsize > 0) {
    //        std::pair<unsigned int, unsigned int> m;
    //        m.first = get_vint(obj, i);
    //        m.second = get_vint(obj, i);
    //        iface.method_descriptors.push_back(m);
    //        methoddescriptorsize--;
    //    }
    //    export_interfaces.push_back(iface);
    //    interfaceexportsize--;
    //}


    //unsigned int classsize = get_vint(obj, i);
    //while (classsize > 0) {
    //    ClassInfo c;
    //    c.name = get_vint(obj, i);
    //    unsigned int interfacecount = get_vint(obj, i);
    //    while (interfacecount > 0) {
    //        std::vector<unsigned int> methods;
    //        unsigned int methodcount = get_vint(obj, i);
    //        while (methodcount > 0) {
    //            methods.push_back(get_vint(obj, i));
    //            methodcount--;
    //        }
    //        c.interfaces.push_back(methods);
    //        interfacecount--;
    //    }
    //    classes.push_back(c);
    //    classsize--;
    //}
        }

        public struct Type
        {
            public uint name;
            public uint descriptor;
        }

        public struct Constant
        {
            public uint name;
            public uint type;
            public byte[] value;
        }

        public struct Variable
        {
            public uint name;
            public uint type;
            public uint index;
        }

        public struct Function
        {
            public uint name;
            public uint descriptor;
            public uint index;
        }

        public struct ExceptionExport
        {
            public uint name;
            public uint descriptor;
            public uint index;
        }

        public struct Interface
        {
            public uint name;
            public List<KeyValuePair<uint, uint>> method_descriptors;
        }

        public struct ModuleImport
        {
            public uint name;
            public bool optional;
            public byte[] hash;
        }

        public struct FunctionInfo
        {
            public uint name;
            public uint nest;
            public uint args;
            public uint locals;
            public uint entry;
        }

        public struct ExceptionInfo
        {
            public uint start;
            public uint end;
            public uint exid;
            public uint handler;
            public uint stack_depth;
        }

        public struct ClassInfo
        {
            public uint name;
            public List<List<uint>> interfaces;
        }

        public struct ExportFunction
        {
            public uint name;
            public uint descriptor;
            public uint entry;
        }
    }
}
