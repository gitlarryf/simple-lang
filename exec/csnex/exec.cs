using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace csnex
{
    // The fields here must match the declaration of
    // ExceptionInfo in ast.cpp.
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
    }

    public class Diagnostics
    {
        public Diagnostics()
        {
            timer = new Stopwatch();
            opstack_max = 0;
            total_opcodes = 0;
        }

        public Stopwatch timer;
        public UInt64 total_opcodes;
        public int opstack_max;
        // ToDo: We have no callstack or frames yet.
        //public UInt64 callstack_max;
        //public UInt64 frameset_max;
    }

    public class Executor
    {
        public Executor(CommandLineOptions ops)
        {
            stack = new Stack<Cell>();
            callstack = new Stack<uint>();
            frame_stack = new List<Cell>();
            globals = new List<Cell>();
            diagnostics = new Diagnostics();
            global = new Global(this);
            options = ops;
        }

        private int exit_code { get; set; }
        public Bytecode bytecode { get; set; }
        public Stack<Cell> stack { get; set; }
        private Stack<uint> callstack { get; set; }
        public uint param_recursion_limit { get; set; }
        public List<Cell> frame_stack { get; set; }
        public List<Cell> globals { get; set; }
        public bool enable_assert { get; set; }
        //public Module module { get; set; }
        public Diagnostics diagnostics { get; set; }
        private CommandLineOptions options { get; set; }
        private uint ip;
        private Global global;

        void raise_literal(string exception, ExceptionInfo info)
        {
            // The fields here must match the declaration of
            // ExceptionType in ast.cpp.
            //Cell exceptionvar;
            //exceptionvar.array_index_for_write(0) = Cell(exception);
            //exceptionvar.array_index_for_write(1) = Cell(info.info);
            //exceptionvar.array_index_for_write(2) = Cell(info.code);
            //exceptionvar.array_index_for_write(3) = Cell(number_from_uint32(static_cast<uint32_t>(ip)));

            //auto tmodule = module;
            //auto tip = ip;
            //size_t sp = callstack.size();
            //for (;;) {
            //    for (auto e = tmodule->object.exceptions.begin(); e != tmodule->object.exceptions.end(); ++e) {
            //        if (tip >= e->start && tip < e->end) {
            //            const std::string handler = tmodule->object.strtable[e->excid];
            //            if (exception.str() == handler
            //             || (exception.length() > handler.length() && exception.substr(0, handler.length()) == handler && exception.at(handler.length()) == '.')) {
            //                module = tmodule;
            //                ip = e->handler;
            //                while (stack.depth() > (frames.empty() ? 0 : frames.back().opstack_depth) + e->stack_depth) {
            //                    stack.pop();
            //                }
            //                callstack.resize(sp);
            //                stack.push(exceptionvar);
            //                return;
            //            }
            //        }
            //    }
            //    if (sp == 0) {
            //        break;
            //    }
            //    sp -= 1;
            //    if (not frames.empty()) {
            //        frames.pop_back();
            //    }
            //    tmodule = callstack[sp].first;
            //    tip = callstack[sp].second;
            //}

            //fprintf(stderr, "Unhandled exception %s (%s) (code %d)\n", exception.c_str(), info.info.c_str(), number_to_sint32(info.code));
            //while (ip < module->object.code.size()) {
            //    if (module->debug != nullptr) {
            //        auto line = module->debug->line_numbers.end();
            //        auto p = ip;
            //        for (;;) {
            //            line = module->debug->line_numbers.find(p);
            //            if (line != module->debug->line_numbers.end()) {
            //                break;
            //            }
            //            if (p == 0) {
            //                fprintf(stderr, "No matching debug information found.\n");
            //                break;
            //            }
            //            p--;
            //        }
            //        if (line != module->debug->line_numbers.end()) {
            //            fprintf(stderr, "  Stack frame #%lu: file %s line %d address %lu\n", static_cast<unsigned long>(callstack.size()), module->debug->source_path.c_str(), line->second, static_cast<unsigned long>(ip));
            //            fprintf(stderr, "    %s\n", module->debug->source_lines.at(line->second).c_str());
            //        } else {
            //            fprintf(stderr, "  Stack frame #%lu: file %s address %lu (line number not found)\n", static_cast<unsigned long>(callstack.size()), module->debug->source_path.c_str(), static_cast<unsigned long>(ip));
            //        }
            //    } else {
            //        fprintf(stderr, "  Stack frame #%lu: file %s address %lu (no debug info available)\n", static_cast<unsigned long>(callstack.size()), source_path.c_str(), static_cast<unsigned long>(ip));
            //    }
            //    if (callstack.empty()) {
            //        break;
            //    }
            //    module = callstack.back().first;
            //    ip = callstack.back().second;
            //    callstack.pop_back();
            //}
            // Setting exit_code here will cause exec_loop to terminate and return this exit code.
            exit_code = 1;
        }

        void raise(ExceptionName exception, ExceptionInfo info)
        {
            raise_literal(exception.name, info);
        }

        void raise(RtlException x)
        {
            //raise_literal(x.Name, new ExceptionInfo(x.Info, x.Code));
        }


        public int run(bool EnableAssertions)
        {
            ip = 0;
            //invoke(module, 0);

            // This sets up the call stack in such a way as to initialize
            // each module in the order determined in init_order, followed
            // by running the code in the main module.
            //foreach(var module in init_order) {
            //    invoke(modules[modules], 0);
            //}
            for (int g = 0; g < bytecode.global_size; g++) {
                bytecode.globals.Add(new Cell(Cell.Type.Address));
            }

            int r = Loop();

            if (r == 0) {
                Debug.Assert(stack.Count == 0);
            }
            return r;
        }

#region Opcode Handlers
#region PUSHx Opcodes
        void PUSHB()
        {
            bool val = bytecode.code[ip+1] != 0;
            ip += 2;
            stack.Push(new Cell(val));
        }

        void PUSHN()
        {
            ip++;
            uint val = Bytecode.get_vint(bytecode.code, (uint)bytecode.code.Length, ref ip);
            stack.Push(new Cell(Number.FromString(bytecode.strtable[(int)val])));
        }

        void PUSHS()
        {
            ip++;
            uint val = Bytecode.get_vint(bytecode.code, (uint)bytecode.code.Length, ref ip);
            stack.Push(new Cell(bytecode.strtable[(int)val]));
        }

        void PUSHY()
        {
            throw new NotImplementedException("PUSHY");
        }

        void PUSHPG()
        {
            ip++;
            uint addr = Bytecode.get_vint(bytecode.code, (uint)bytecode.code.Length, ref ip);
            Debug.Assert(addr < bytecode.global_size);
            stack.Push(bytecode.globals[(int)addr]);
        }

        void PUSHPPG()
        {
            throw new NotImplementedException("PUSHPPG");
        }

        void PUSHPMG()
        {
            throw new NotImplementedException("PUSHPMG");
        }

        void PUSHPL()
        {
            throw new NotImplementedException("PUSHPL");
        }

        void PUSHPOL()
        {
            throw new NotImplementedException("PUSHPOL");
        }

        void PUSHI()
        {
            throw new NotImplementedException("PUSHI");
        }
#endregion
#region LOADx Opcodes
        void LOADB()
        {
            ip++;
            Cell addr = stack.Pop().Address;
            stack.Push(new Cell(addr.Boolean));
        }

        void LOADN()
        {
            ip++;
            Cell addr = stack.Pop().Address;
            stack.Push(new Cell(addr.Number));
        }

        void LOADS()
        {
            ip++;
            Cell addr = stack.Pop().Address;
            stack.Push(new Cell(addr.String));
        }

        void LOADY()
        {
            throw new NotImplementedException("LOADY");
        }

        void LOADA()
        {
            ip++;
            Cell addr = stack.Pop().Address;
            stack.Push(new Cell(addr.Array));
        }

        void LOADD()
        {
            throw new NotImplementedException("LOADD");
        }

        void LOADP()
        {
            throw new NotImplementedException("LOADP");
        }

        void LOADJ()
        {
            throw new NotImplementedException("LOADJ");
        }

        void LOADV()
        {
            throw new NotImplementedException("LOADV");
        }
#endregion
#region STOREx Opcodes
        void STOREB()
        {
            ip++;
            Cell addr = stack.Pop();
            addr.Address = stack.Pop();
        }

        void STOREN()
        {
            ip++;
            Cell addr = stack.Pop();
            addr.Address = stack.Pop();
        }

        void STORES()
        {
            ip++;
            Cell addr = stack.Pop();
            addr.Address = stack.Pop();
        }

        void STOREY()
        {
            throw new NotImplementedException("STOREY");
        }

        void STOREA()
        {
            ip++;
            Cell addr = stack.Pop();
            addr.Address = stack.Pop();
        }

        void STORED()
        {
            throw new NotImplementedException("STORED");
        }

        void STOREP()
        {
            throw new NotImplementedException("STOREP");
        }

        void STOREJ()
        {
            throw new NotImplementedException("STOREJ");
        }

        void STOREV()
        {
            throw new NotImplementedException("STOREV");
        }
        #endregion
#region Arithmetic Opcodes
        void NEGN()
        {
            ip++;
            Number x = stack.Pop().Number;
            stack.Push(new Cell(Number.Negate(x)));
        }

        void ADDN()
        {
            ip++;
            Number b = stack.Pop().Number;
            Number a = stack.Pop().Number;
            stack.Push(new Cell(Number.Add(a, b)));
        }

        void SUBN()
        {
            ip++;
            Number b = stack.Pop().Number;
            Number a = stack.Pop().Number;
            stack.Push(new Cell(Number.Subtract(a, b)));
        }

        void MULN()
        {
            ip++;
            Number b = stack.Pop().Number;
            Number a = stack.Pop().Number;
            stack.Push(new Cell(Number.Multiply(a, b)));
        }

        void DIVN()
        {
            ip++;
            Number b = stack.Pop().Number;
            Number a = stack.Pop().Number;
            if (b.IsZero()) {
                throw new NeonDivideByZeroException("DivideByZeroException", "");
            }
            stack.Push(new Cell(Number.Divide(a, b)));
        }

        void MODN()
        {
            ip++;
            Number b = stack.Pop().Number;
            Number a = stack.Pop().Number;
            if (b.IsZero()) {
                throw new NeonDivideByZeroException("DivideByZeroException", "");
            }
            stack.Push(new Cell(Number.Modulo(a, b)));
        }

        void EXPN()
        {
            ip++;
            Number b = stack.Pop().Number;
            Number a = stack.Pop().Number;
            if (b.IsZero()) {
                throw new NeonDivideByZeroException("DivideByZeroException", "");
            }
            stack.Push(new Cell(Number.Pow(a, b)));
        }
        #endregion
#region Comparison Opcodes
        void EQB()
        {
            throw new NotImplementedException("EQB");
        }

        void NEB()
        {
            throw new NotImplementedException("NEB");
        }

        void EQN()
        {
            ip++;
            Number b = stack.Pop().Number;
            Number a = stack.Pop().Number;
            stack.Push(new Cell(Number.IsEqual(a, b)));
        }

        void NEN()
        {
            ip++;
            Number b = stack.Pop().Number;
            Number a = stack.Pop().Number;
            stack.Push(new Cell(!Number.IsEqual(a, b)));
        }

        void LTN()
        {
            ip++;
            Number b = stack.Pop().Number;
            Number a = stack.Pop().Number;
            stack.Push(new Cell(Number.IsLessThan(a, b)));
        }

        void GTN()
        {
            ip++;
            Number b = stack.Pop().Number;
            Number a = stack.Pop().Number;
            stack.Push(new Cell(Number.IsGreaterThan(a, b)));
        }

        void LEN()
        {
            ip++;
            Number b = stack.Pop().Number;
            Number a = stack.Pop().Number;
            stack.Push(new Cell(Number.IsLessThanOrEqual(a, b)));
        }

        void GEN()
        {
            ip++;
            Number b = stack.Pop().Number;
            Number a = stack.Pop().Number;
            stack.Push(new Cell(Number.IsGreaterThanOrEqual(a, b)));
        }

        void EQS()
        {
            throw new NotImplementedException("EQS");
        }

        void NES()
        {
            throw new NotImplementedException("NES");
        }

        void LTS()
        {
            throw new NotImplementedException("LTS");
        }

        void GTS()
        {
            ip++;
            Cell b = stack.Pop();
            Cell a = stack.Pop();
            stack.Push(new Cell(string.Compare(a.String, b.String) > 0));
        }

        void LES()
        {
            throw new NotImplementedException("LES");
        }

        void GES()
        {
            throw new NotImplementedException("GES");
        }

        void EQY()
        {
            throw new NotImplementedException("EQY");
        }

        void NEY()
        {
            throw new NotImplementedException("NEY");
        }

        void LTY()
        {
            throw new NotImplementedException("LTY");
        }

        void GTY()
        {
            throw new NotImplementedException("GTY");
        }

        void LEY()
        {
            throw new NotImplementedException("LEY");
        }

        void GEY()
        {
            throw new NotImplementedException("GEY");
        }

        void EQA()
        {
            throw new NotImplementedException("EQA");
        }

        void NEA()
        {
            throw new NotImplementedException("NEA");
        }

        void EQD()
        {
            throw new NotImplementedException("EQD");
        }

        void NED()
        {
            throw new NotImplementedException("NED");
        }

        void EQP()
        {
            throw new NotImplementedException("EQP");
        }

        void NEP()
        {
            throw new NotImplementedException("NEP");
        }

        void EQV()
        {
            throw new NotImplementedException("EQV");
        }

        void NEV()
        {
            throw new NotImplementedException("NEV");
        }
        #endregion
#region Logic Opcodes
        void ANDB()
        {
            throw new NotImplementedException("ANDB");
        }

        void ORB()
        {
            throw new NotImplementedException("ORB");
        }

        void NOTB()
        {
            ip++;
            bool x = stack.Pop().Boolean;
            stack.Push(new Cell(!x));
        }
        #endregion
#region Index Opcodes
        void INDEXAR()
        {
            ip++;
            Number index = stack.Pop().Number;
            Cell addr = stack.Pop().Address;
            if (!index.IsInteger()) {
                throw new NeonArrayIndexException(index.ToString());
            }
            Int64 i = (Int64)index;
            if (i < 0) {
                throw new NeonArrayIndexException(((Int64)index).ToString());
            }
            UInt64 j = (UInt64)i;

            if (j >= (uint)addr.Array.Count) {
                throw new NeonArrayIndexException(((UInt64)index).ToString());
            }
            stack.Push(addr.ArrayIndexForRead((uint)j));
        }

        void INDEXAW()
        {
            ip++;
            Number index = stack.Pop().Number;
            Cell addr = stack.Pop().Address;

            if (!index.IsInteger()) {
                throw new NeonArrayIndexException(index.ToString());
            }
            Int64 i = (Int64)index;
            if (i < 0) {
                throw new NeonArrayIndexException(((Int64)index).ToString());
            }
            UInt64 j = (UInt64)i;
            stack.Push(addr.ArrayIndexForWrite((uint)j));
        }

        void INDEXAV()
        {
            throw new NotImplementedException("INDEXAV");
        }

        void INDEXAN()
        {
            throw new NotImplementedException("INDEXAN");
        }

        void INDEXDR()
        {
            throw new NotImplementedException("INDEXDR");
        }

        void INDEXDW()
        {
            throw new NotImplementedException("INDEXDW");
        }

        void INDEXDV()
        {
            throw new NotImplementedException("INDEXDV");
        }
        #endregion
#region INx Opcdes
        void INA()
        {
            throw new NotImplementedException("INA");
        }

        void IND()
        {
            throw new NotImplementedException("IND");
        }
#endregion
#region CALLx Opcodes
        void CALLP()
        {
            uint start_ip = ip;
            ip++;
            UInt32 val = Bytecode.get_vint(bytecode.code, bytecode.codelen, ref ip);
            string func = bytecode.strtable[(int)val];
            global.dispatch(func);
        }

        void CALLF()
        {
            throw new NotImplementedException("CALLF");
        }

        void CALLMF()
        {
            throw new NotImplementedException("CALLMF");
        }

        void CALLI()
        {
            throw new NotImplementedException("CALLI");
        }
        #endregion
#region JUMP Opcodes
        void JUMP()
        {
            ip++;
            uint target = Bytecode.get_vint(bytecode.code, bytecode.codelen, ref ip);
            ip = target;
        }

        void JF()
        {
            ip++;
            UInt32 target = Bytecode.get_vint(bytecode.code, bytecode.codelen, ref ip);
            bool a = stack.Pop().Boolean;
            if (!a) {
                ip = target;
            }
        }

        void JT()
        {
            ip++;
            UInt32 target = Bytecode.get_vint(bytecode.code, bytecode.codelen, ref ip);
            bool a = stack.Pop().Boolean;
            if (a) {
                ip = target;
            }
        }

        void JFCHAIN()
        {
            throw new NotImplementedException("JFCHAIN");
        }
#endregion
#region Stack Handler Opcodes
        void DUP()
        {
            ip++;
            Cell top = stack.Peek();
            stack.Push(top);
        }

        void DUPX1()
        {
            throw new NotImplementedException("DUPX1");
        }

        void DROP()
        {
            ip++;
            stack.Pop();
        }

        void RET()
        {
            ip++;
            // ToDo: Implement Call stack
            //ip = callstack.Pop();
        }
#endregion
        void CALLE()
        {
            throw new NotImplementedException("CALLE");
        }

        void CONSA()
        {
            ip++;
            int val = (int)Bytecode.get_vint(bytecode.code, bytecode.codelen, ref ip);
            List<Cell> a = new List<Cell>(val);
            while (val > 0) {
                a[a.Count - val] = stack.Pop();
                val--;
            }
            stack.Push(new Cell(a));
        }

        void CONSD()
        {
            throw new NotImplementedException("CONSD");
        }

        void EXCEPT()
        {
            throw new NotImplementedException("EXCEPT");
        }

        void ALLOC()
        {
            throw new NotImplementedException("ALLOC");
        }

        void PUSHNIL()
        {
            throw new NotImplementedException("PUSHNIL");
        }

        void JNASSERT()
        {
            throw new NotImplementedException("JNASSERT");
        }

        void RESETC()
        {
            ip++;
            Cell addr = stack.Pop();
            addr.ResetCell();
        }

        void PUSHPEG()
        {
            throw new NotImplementedException("PUSHPEG");
        }

        void JUMPTBL()
        {
            ip++;
            UInt32 val = Bytecode.get_vint(bytecode.code, bytecode.codelen, ref ip);
            Number n = stack.Pop().Number;
            if (n.IsInteger() && !n.IsNegative()) {
                UInt32 i = (UInt32)n;
                if(i < val) {
                    ip += 6 * i;
                } else {
                    ip += 6 * val;
                }
            } else {
                ip += 6 * val;
            }
        }

        void CALLX()
        {
            throw new NotImplementedException("CALLX");
        }

        void SWAP()
        {
            ip++;
            Cell a = stack.Pop();
            Cell b = stack.Pop();
            stack.Push(a);
            stack.Push(b);
        }

        void DROPN()
        {
            throw new NotImplementedException("DROPN");
        }

        void PUSHFP()
        {
            throw new NotImplementedException("PUSHFP");
        }

        void CALLV()
        {
            throw new NotImplementedException("CALLV");
        }

        void PUSHCI()
        {
            throw new NotImplementedException("PUSHCI");
        }
        #endregion

        public void Invoke()
        {
            // ToDo: Implement Invoke Method before calling Loop().
        }

        private int Loop()
        {
            while (ip < bytecode.codelen && exit_code == 0) {
                // ToDo: Implement Trace Output
                diagnostics.total_opcodes++;
                if (stack.Count > diagnostics.opstack_max) {
                    diagnostics.opstack_max = stack.Count;
                }
                switch (bytecode.code[ip]) {
                    case (byte)Opcode.PUSHB: PUSHB(); break;                // push boolean immediate
                    case (byte)Opcode.PUSHN: PUSHN(); break;                // push number immediate
                    case (byte)Opcode.PUSHS: PUSHS(); break;                // push string immediate
                    case (byte)Opcode.PUSHY: PUSHY(); break;                // push bytes immediate
                    case (byte)Opcode.PUSHPG: PUSHPG(); break;              // push pointer to global
                    case (byte)Opcode.PUSHPPG: PUSHPPG(); break;            // push pointer to predefined global
                    case (byte)Opcode.PUSHPMG: PUSHPMG(); break;            // push pointer to module global
                    case (byte)Opcode.PUSHPL: PUSHPL(); break;              // push pointer to local
                    case (byte)Opcode.PUSHPOL: PUSHPOL(); break;            // push pointer to outer local
                    case (byte)Opcode.PUSHI: PUSHI(); break;                // push 32-bit integer immediate
                    case (byte)Opcode.LOADB: LOADB(); break;                // load boolean
                    case (byte)Opcode.LOADN: LOADN(); break;                // load number
                    case (byte)Opcode.LOADS: LOADS(); break;                // load string
                    case (byte)Opcode.LOADY: LOADY(); break;                // load bytes
                    case (byte)Opcode.LOADA: LOADA(); break;                // load array
                    case (byte)Opcode.LOADD: LOADD(); break;                // load dictionary
                    case (byte)Opcode.LOADP: LOADP(); break;                // load pointer
                    case (byte)Opcode.LOADJ: LOADJ(); break;                // load object
                    case (byte)Opcode.LOADV: LOADV(); break;                // load voidptr
                    case (byte)Opcode.STOREB: STOREB(); break;              // store boolean
                    case (byte)Opcode.STOREN: STOREN(); break;              // store number
                    case (byte)Opcode.STORES: STORES(); break;              // store string
                    case (byte)Opcode.STOREY: STOREY(); break;              // store bytes
                    case (byte)Opcode.STOREA: STOREA(); break;              // store array
                    case (byte)Opcode.STORED: STORED(); break;              // store dictionary
                    case (byte)Opcode.STOREP: STOREP(); break;              // store pointer
                    case (byte)Opcode.STOREJ: STOREJ(); break;              // store object
                    case (byte)Opcode.STOREV: STOREV(); break;              // store voidptr
                    case (byte)Opcode.NEGN: NEGN(); break;                  // negate number
                    case (byte)Opcode.ADDN: ADDN(); break;                  // add number
                    case (byte)Opcode.SUBN: SUBN(); break;                  // subtract number
                    case (byte)Opcode.MULN: MULN(); break;                  // multiply number
                    case (byte)Opcode.DIVN: DIVN(); break;                  // divide number
                    case (byte)Opcode.MODN: MODN(); break;                  // modulo number
                    case (byte)Opcode.EXPN: EXPN(); break;                  // exponentiate number
                    case (byte)Opcode.EQB: EQB(); break;                    // compare equal boolean
                    case (byte)Opcode.NEB: NEB(); break;                    // compare unequal boolean
                    case (byte)Opcode.EQN: EQN(); break;                    // compare equal number
                    case (byte)Opcode.NEN: NEN(); break;                    // compare unequal number
                    case (byte)Opcode.LTN: LTN(); break;                    // compare less number
                    case (byte)Opcode.GTN: GTN(); break;                    // compare greater number
                    case (byte)Opcode.LEN: LEN(); break;                    // compare less equal number
                    case (byte)Opcode.GEN: GEN(); break;                    // compare greater equal number
                    case (byte)Opcode.EQS: EQS(); break;                    // compare equal string
                    case (byte)Opcode.NES: NES(); break;                    // compare unequal string
                    case (byte)Opcode.LTS: LTS(); break;                    // compare less string
                    case (byte)Opcode.GTS: GTS(); break;                    // compare greater string
                    case (byte)Opcode.LES: LES(); break;                    // compare less equal string
                    case (byte)Opcode.GES: GES(); break;                    // compare greater equal string
                    case (byte)Opcode.EQY: EQY(); break;                    // compare equal bytes
                    case (byte)Opcode.NEY: NEY(); break;                    // compare unequal bytes
                    case (byte)Opcode.LTY: LTY(); break;                    // compare less bytes
                    case (byte)Opcode.GTY: GTY(); break;                    // compare greater bytes
                    case (byte)Opcode.LEY: LEY(); break;                    // compare less equal bytes
                    case (byte)Opcode.GEY: GEY(); break;                    // compare greater equal bytes
                    case (byte)Opcode.EQA: EQA(); break;                    // compare equal array
                    case (byte)Opcode.NEA: NEA(); break;                    // compare unequal array
                    case (byte)Opcode.EQD: EQD(); break;                    // compare equal dictionary
                    case (byte)Opcode.NED: NED(); break;                    // compare unequal dictionary
                    case (byte)Opcode.EQP: EQP(); break;                    // compare equal pointer
                    case (byte)Opcode.NEP: NEP(); break;                    // compare unequal pointer
                    case (byte)Opcode.EQV: EQV(); break;                    // compare equal voidptr
                    case (byte)Opcode.NEV: NEV(); break;                    // compare unequal voidptr
                    case (byte)Opcode.ANDB: ANDB(); break;                  // and boolean
                    case (byte)Opcode.ORB: ORB(); break;                    // or boolean
                    case (byte)Opcode.NOTB: NOTB(); break;                  // not boolean
                    case (byte)Opcode.INDEXAR: INDEXAR(); break;            // index array for read
                    case (byte)Opcode.INDEXAW: INDEXAW(); break;            // index array for write
                    case (byte)Opcode.INDEXAV: INDEXAV(); break;            // index array value
                    case (byte)Opcode.INDEXAN: INDEXAN(); break;            // index array value, no exception
                    case (byte)Opcode.INDEXDR: INDEXDR(); break;            // index dictionary for read
                    case (byte)Opcode.INDEXDW: INDEXDW(); break;            // index dictionary for write
                    case (byte)Opcode.INDEXDV: INDEXDV(); break;            // index dictionary value
                    case (byte)Opcode.INA: INA(); break;                    // in array
                    case (byte)Opcode.IND: IND(); break;                    // in dictionary
                    case (byte)Opcode.CALLP: CALLP(); break;                // call predefined
                    case (byte)Opcode.CALLF: CALLF(); break;                // call function
                    case (byte)Opcode.CALLMF: CALLMF(); break;              // call module function
                    case (byte)Opcode.CALLI: CALLI(); break;                // call indirect
                    case (byte)Opcode.JUMP: JUMP(); break;                  // unconditional jump
                    case (byte)Opcode.JF: JF(); break;                      // jump if false
                    case (byte)Opcode.JT: JT(); break;                      // jump if true
                    case (byte)Opcode.DUP: DUP(); break;                    // duplicate
                    case (byte)Opcode.DUPX1: DUPX1(); break;                // duplicate under second value
                    case (byte)Opcode.DROP: DROP(); break;                  // drop
                    case (byte)Opcode.RET: RET(); break;                    // return
                    case (byte)Opcode.CONSA: CONSA(); break;                // construct array
                    case (byte)Opcode.CONSD: CONSD(); break;                // construct dictionary
                    case (byte)Opcode.EXCEPT: EXCEPT(); break;              // throw exception
                    case (byte)Opcode.ALLOC: ALLOC(); break;                // allocate record
                    case (byte)Opcode.PUSHNIL: PUSHNIL(); break;            // push nil pointer
                    case (byte)Opcode.RESETC: RESETC(); break;              // reset cell
                    case (byte)Opcode.PUSHPEG: PUSHPEG(); break;            // push pointer to external global
                    case (byte)Opcode.JUMPTBL: JUMPTBL(); break;            // jump table
                    case (byte)Opcode.CALLX: CALLX(); break;                // call extension
                    case (byte)Opcode.SWAP: SWAP(); break;                  // swap two top stack elements
                    case (byte)Opcode.DROPN: DROPN(); break;                // drop element n
                    case (byte)Opcode.PUSHFP: PUSHFP(); break;              // push function pointer
                    case (byte)Opcode.CALLV: CALLV(); break;                // call virtual
                    case (byte)Opcode.PUSHCI: PUSHCI(); break;              // push class info
                    default:
                        throw new NeonInvalidOpcodeException(string.Format("Invalid opcode ({0}) in bytecode file.", bytecode.code[ip]));
                }
            }
            return exit_code;
        }
    }
}
