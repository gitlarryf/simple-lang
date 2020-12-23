using System;
using System.Collections.Generic;

namespace csnex
{
    public class Bytecode
    {
        public byte[] obj { get; private set; }
        public string source_path {get; private set; }
        public byte[] source_hash;
        public uint version {get; private set; }
        public uint global_size { get; private set; }
        public List<String> strtable { get; private set; }
        public uint strtablesize { get; private set; }
        public uint strtablelen { get; private set; }
        //public List<String> strings { get; private set; }
        public uint typesize { get; private set; }
        public uint constantsize { get; private set; }
        public uint variablesize { get; private set; }
        public uint export_functionsize { get; private set; }
        public uint functionsize { get; private set; }
        public uint exceptionsize { get; private set; }
        public uint exceptionexportsize { get; private set; }
        public uint interfaceexportsize { get; private set; }
        public uint importsize { get; private set; }
        public uint classsize { get; private set; }
        public byte[] code { get; private set; }
        public uint codelen { get; private set; }

        public List<Type> export_types { get; private set; }
        public List<Constant> export_constants {get; private set; }
        public List<Variable> export_variables {get; private set; }
        public List<Function> export_functions { get; private set; }
        public List<ExceptionExport> export_exceptions { get; private set; }
        public List<Interface> export_interfaces { get; private set; }

        public List<ModuleImport> imports { get; private set; }
        public List<FunctionInfo> functions { get; private set; }
        public List<ExceptionInfo> exceptions { get; private set; }
        public List<ClassInfo> classes { get; private set; }

        public List<Cell> globals;

        public Bytecode()
        {
        }

        public static UInt32 get_vint(Byte[] pobj, uint nBuffSize, ref uint i)
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

        public List<String> GetStrTable(Byte[] obj, uint size, ref uint i)
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

        public void LoadBytecode(string a_source_path, Byte[] bytes, uint len)
        {
            source_path = a_source_path;
            obj = bytes;
            //System.Diagnostics.Debug.Assert((source_hash.ToString() != string.Empty), "Bytecode is not empty!");

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

            //source_hash = System.Text.Encoding.ASCII.GetChars(obj, (int)i, 32);
            source_hash = obj.CopyFrom(i, 32);
            i += 32;

            global_size = get_vint(obj, len, ref i);
            globals = new List<Cell>((int)global_size);

            strtablesize = get_vint(obj, len, ref i);
            strtable = GetStrTable(obj, strtablesize + i, ref i);

            typesize = get_vint(obj, len, ref i);
            export_types = new List<Type>();
            for (int f = 0; f < typesize; f++) {
                Type t = new Type();
                t.name = get_vint(obj, len, ref i);
                t.descriptor = get_vint(obj, len, ref i);
                export_types.Add(t);
            }

            constantsize = get_vint(obj, len, ref i);
            export_constants = new List<Constant>();
            for (int c = 0; c < constantsize; c++) {
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
            }

            variablesize = get_vint(obj, len, ref i);
            export_variables = new List<Variable>();
            for (int vs = 0; vs < variablesize; vs++) {
                Variable v = new Variable();
                v.name = get_vint(obj, len, ref i);
                v.type = get_vint(obj, len, ref i);
                v.index = get_vint(obj, len, ref i);
                export_variables.Add(v);
            }

            export_functionsize = get_vint(obj, len, ref i);
            export_functions = new List<Function>();
            for (int f = 0; f < export_functionsize; f++) {
                Function ef = new Function();
                ef.name = get_vint(obj, len, ref i);
                ef.descriptor = get_vint(obj, len, ref i);
                ef.index = get_vint(obj, len, ref i);
                export_functions.Add(ef);
            }

            exceptionexportsize = get_vint(obj, len, ref i);
            export_exceptions = new List<ExceptionExport>();
            while (exceptionexportsize > 0) {
                ExceptionExport e = new ExceptionExport();
                e.name = get_vint(obj, len, ref i);
                export_exceptions.Add(e);
                exceptionsize--;
            }

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
            interfaceexportsize = get_vint(obj, len, ref i);
            /*
            interfaceexportsize = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            while interfaceexportsize > 0:
            assert False, interfaceexportsize
            */
            imports = new List<ModuleImport>();
            importsize = get_vint(obj, len, ref i);
            for (int f = 0; f < importsize; f++) {
                ModuleImport imp = new ModuleImport();
                imp.name = get_vint(obj, len, ref i);
                imp.optional = get_vint(obj, len, ref i) != 0;
                if (i + 32 > obj.Length) {
                    throw new BytecodeException("unexpected end of bytecode");
                }
                imp.hash = obj.CopyFrom(i, 32);
                imports.Add(imp);
                i += 32;
            }

            functions = new List<FunctionInfo>();
            functionsize = get_vint(obj, len, ref i);
            for (int f = 0; f < functionsize; f++) {
                FunctionInfo fi = new FunctionInfo();
                fi.name = get_vint(obj, len, ref i);
                fi.nest = get_vint(obj, len, ref i);
                fi.args = get_vint(obj, len, ref i);
                fi.locals = get_vint(obj, len, ref i);
                fi.entry = get_vint(obj, len, ref i);
                functions.Add(fi);
            }

            exceptions = new List<ExceptionInfo>();
            exceptionsize = get_vint(obj, len, ref i);
            for (int e = 0; e < exceptionsize; e++) {
                ExceptionInfo ex = new ExceptionInfo();
                ex.start = get_vint(obj, len,  ref i);
                ex.end = get_vint(obj, len, ref i);
                ex.exid = get_vint(obj, len, ref i);
                ex.handler = get_vint(obj, len, ref i);
                ex.stack_depth = get_vint(obj, len, ref i);
                exceptions.Add(ex);
            }

            classes = new List<ClassInfo>();
            classsize = get_vint(obj, len, ref i);
            /*
            classsize = struct.unpack(">H", bytecode[i:i+2])[0]
            i += 2
            while classsize > 0:
            assert False, classsize
            */
            codelen = len - i;
            //code = new byte[codelen+1];
            code = obj.CopyFrom(i, (int)codelen);

            //for (int x = 0; x < codelen; x++) {
            //    code[x] = obj[i+x];
            //}

    //if (i + 32 > obj.size()) {
    //    throw BytecodeException("unexpected end of bytecode");
    //}
    //source_hash = std::string(&obj[i], &obj[i]+32);
    //i += 32;

    //global_size = get_vint(obj, i);

    //unsigned int strtablesize = get_vint(obj, i);
    //if (i+strtablesize > obj.size()) {
    //    throw BytecodeException("unexpected end of bytecode");
    //}
    //strtable = getstrtable(obj, i, strtablesize);
    //i += strtablesize;
    /* Done */
    //unsigned int typesize = get_vint(obj, i);
    //while (typesize > 0) {
    //    Type t;
    //    t.name = get_vint(obj, i);
    //    t.descriptor = get_vint(obj, i);
    //    export_types.push_back(t);
    //    typesize--;
    //}
    /* Done */
    //unsigned int constantsize = get_vint(obj, i);
    //while (constantsize > 0) {
    //    Constant c;
    //    c.name = get_vint(obj, i);
    //    c.type = get_vint(obj, i);
    //    unsigned int size = get_vint(obj, i);
    //    if (i+size > obj.size()) {
    //        throw BytecodeException("unexpected end of bytecode");
    //    }
    //    c.value = Bytes(&obj[i], &obj[i+size]);
    //    i += size;
    //    export_constants.push_back(c);
    //    constantsize--;
    //}
    /* Done */
    //unsigned int variablesize = get_vint(obj, i);
    //while (variablesize > 0) {
    //    Variable v;
    //    v.name = get_vint(obj, i);
    //    v.type = get_vint(obj, i);
    //    v.index = get_vint(obj, i);
    //    export_variables.push_back(v);
    //    variablesize--;
    //}
    /* Done */
    //unsigned int functionsize = get_vint(obj, i);
    //while (functionsize > 0) {
    //    Function f;
    //    f.name = get_vint(obj, i);
    //    f.descriptor = get_vint(obj, i);
    //    f.index = get_vint(obj, i);
    //    export_functions.push_back(f);
    //    functionsize--;
    //}
    /* Done */
    //unsigned int exceptionexportsize = get_vint(obj, i);
    //while (exceptionexportsize > 0) {
    //    ExceptionExport e;
    //    e.name = get_vint(obj, i);
    //    export_exceptions.push_back(e);
    //    exceptionexportsize--;
    //}

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

    /* Done */
    //unsigned int importsize = get_vint(obj, i);
    //while (importsize > 0) {
    //    ModuleImport imp;
    //    imp.name = get_vint(obj, i);
    //    imp.optional = get_vint(obj, i) != 0;
    //    if (i+32 > obj.size()) {
    //        throw BytecodeException("unexpected end of bytecode");
    //    }
    //    imp.hash = std::string(&obj[i], &obj[i]+32);
    //    i += 32;
    //    imports.push_back(imp);
    //    importsize--;
    //}

    /* Done */
    ///*unsigned int*/ functionsize = get_vint(obj, i);
    //while (functionsize > 0) {
    //    FunctionInfo f;
    //    f.name = get_vint(obj, i);
    //    f.nest = get_vint(obj, i);
    //    f.params = get_vint(obj, i);
    //    f.locals = get_vint(obj, i);
    //    f.entry = get_vint(obj, i);
    //    functions.push_back(f);
    //    functionsize--;
    //}

    /* Done */
    //unsigned int exceptionsize = get_vint(obj, i);
    //while (exceptionsize > 0) {
    //    ExceptionInfo e;
    //    e.start = get_vint(obj, i);
    //    e.end = get_vint(obj, i);
    //    e.excid = get_vint(obj, i);
    //    e.handler = get_vint(obj, i);
    //    e.stack_depth = get_vint(obj, i);
    //    exceptions.push_back(e);
    //    exceptionsize--;
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

    //code = Bytes(obj.begin() + i, obj.end());
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
