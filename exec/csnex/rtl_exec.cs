using System;
using System.Collections.Generic;

namespace csnex {

    public class RtlException : System.Exception
    {
        public string name;
        public string info;
        public UInt32 code;

        public RtlException(ExceptionName name, string info, UInt32 code = 0)
        {
            this.name = name.name;
            this.info = info;
            this.code = code;
        }
    }
}



//    public class rtl
//    {
//        static Dictionary<string, uint> FunctionNames;
//        static Dictionary<string, uint> VariableNames;

//        void rtl_exec_init(int argc, string[] argv)
//        {
//            //extern void rtl_sys_init(int, string[]);
//            rtl_sys_init(argc, argv);
//            FunctionNames.Clear();
//            VariableNames.Clear();

//            uint i = 0;
//            for (auto f: BuiltinFunctions) {
//                FunctionNames[f.name] = i;
//                i++;
//            }

//            i = 0;
//            for (auto v: BuiltinVariables) {
//                VariableNames[v.name] = i;
//                i++;
//            }
//        }

//        void rtl_call(opstack<Cell> &stack, const std::string &name, size_t &token)
//        {
//            if (token == SIZE_MAX) {
//                auto f = FunctionNames.find(name);
//                if (f == FunctionNames.end()) {
//                    fprintf(stderr, "neon: function not found: %s\n", name.c_str());
//                    abort();
//                }
//                token = f->second;
//            }
//            auto &fn = BuiltinFunctions[token];
//            fn.thunk(stack, fn.func);
//        }

//        Cell *rtl_variable(const std::string &name)
//        {
//            return BuiltinVariables[VariableNames[name]].value;
//        }

//                }
//                void rtl_call(opstack<Cell> &stack, const std::string &name, size_t &token);
//        Cell *rtl_variable(const std::string &name);
//}
