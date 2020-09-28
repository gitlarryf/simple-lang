using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csnex
{
    public struct CommandLineOptions
    {
        public Boolean ExecutorDebugStats;
        public Boolean ExecutorDisassembly;
        public Boolean EnableAssertions;
        public string Filename;
        public string ExecutableName;
    }

    //public class Module
    //{
    //    class NumberTable<B, D>
    //    {
    //        public NumberTable()
    //        {
    //        }

    //        public B b { get; set; }
    //        public D d { get; set; }
    //    }

    //    public Module(string name, Bytecode obj, Executor executor)
    //    {

    //    }

    //    public string name { get; set; }
    //    public Bytecode obj { get; private set; }
    //    List<Cell> globals;
    //    List<int> rtl_call_tokens;
    //    List<NumberTable<bool, Decimal>> number_table;
    //}

    public class Diagnostics
    {
        public DateTime time_start;
        public DateTime time_end;
        public UInt64 total_opcodes;
        public UInt64 opstack_max;
        public UInt64 callstack_max;
        public UInt64 frameset_max;
    }

    public class Executor
    {
        public Executor(CommandLineOptions ops)
        {
            options = ops;
        }

        public Bytecode bytecode { get; set; }
        private Stack<Cell> stack { get; set; }
        private Stack<int> callstack { get; set; }
        public int param_recursion_limit { get; set; }
        public List<Cell> frame_stack { get; set; }
        public List<Cell> globals { get; set; }
        public bool enable_assert { get; set; }
        //public Module module { get; set; }
        public Diagnostics diagnostics { get; set; }
        private CommandLineOptions options { get; set; }


        public void run(bool EnableAssertions)
        {

        }

        void ENTER()
        {
            throw new NotImplementedException("ENTER");
        }

        void LEAVE()
        {
            throw new NotImplementedException("LEAVE");
        }

        void PUSHB()
        {
            throw new NotImplementedException("PUSHB");
        }

        void PUSHN()
        {
            throw new NotImplementedException("PUSHN");
        }

        void PUSHS()
        {
            throw new NotImplementedException("PUSHS");
        }

        void PUSHT()
        {
            throw new NotImplementedException("PUSHT");
        }

        void PUSHPG()
        {
            throw new NotImplementedException("LEAVE");
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

        void LOADB()
        {
            throw new NotImplementedException("LOADB");
        }

        void LOADN()
        {
            throw new NotImplementedException("LOADN");
        }

        void LOADS()
        {
            throw new NotImplementedException("LOADS");
        }

        void LOADT()
        {
            throw new NotImplementedException("LOADT");
        }

        void LOADA()
        {
            throw new NotImplementedException("LOADA");
        }

        void LOADD()
        {
            throw new NotImplementedException("LOADD");
        }

        void LOADP()
        {
            throw new NotImplementedException("LOADP");
        }

        void STOREB()
        {
            throw new NotImplementedException("STOREB");
        }

        void STOREN()
        {
            throw new NotImplementedException("STOREN");
        }

        void STORES()
        {
            throw new NotImplementedException("STORES");
        }

        void STORET()
        {
            throw new NotImplementedException("STORET");
        }

        void STOREA()
        {
            throw new NotImplementedException("STOREA");
        }

        void STORED()
        {
            throw new NotImplementedException("STORED");
        }

        void STOREP()
        {
            throw new NotImplementedException("STOREP");
        }

        void NEGN()
        {
            throw new NotImplementedException("NEGN");
        }

        void ADDN()
        {
            throw new NotImplementedException("ADDN");
        }

        void SUBN()
        {
            throw new NotImplementedException("SUBN");
        }

        void MULN()
        {
            throw new NotImplementedException("MULN");
        }

        void DIVN()
        {
            throw new NotImplementedException("DIVN");
        }

        void MODN()
        {
            throw new NotImplementedException("MODN");
        }

        void EXPN()
        {
            throw new NotImplementedException("EXPN");
        }

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
            throw new NotImplementedException("EQN");
        }

        void NEN()
        {
            throw new NotImplementedException("NEN");
        }

        void LTN()
        {
            throw new NotImplementedException("LTN");
        }

        void GTN()
        {
            throw new NotImplementedException("GTN");
        }

        void LEN()
        {
            throw new NotImplementedException("LEN");
        }

        void GEN()
        {
            throw new NotImplementedException("GEN");
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
            throw new NotImplementedException("GTS");
        }

        void LES()
        {
            throw new NotImplementedException("LES");
        }

        void GES()
        {
            throw new NotImplementedException("GES");
        }

        void EQT()
        {
            throw new NotImplementedException("EQT");
        }

        void NET()
        {
            throw new NotImplementedException("NET");
        }

        void LTT()
        {
            throw new NotImplementedException("LTT");
        }

        void GTT()
        {
            throw new NotImplementedException("GTT");
        }

        void LET()
        {
            throw new NotImplementedException("LET");
        }

        void GET()
        {
            throw new NotImplementedException("GET");
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
            throw new NotImplementedException("NOTB");
        }

        void INDEXAR()
        {
            throw new NotImplementedException("INDEXAR");
        }

        void INDEXAW()
        {
            throw new NotImplementedException("INDEXAW");
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

        void INA()
        {
            throw new NotImplementedException("INA");
        }

        void IND()
        {
            throw new NotImplementedException("IND");
        }

        void CALLP()
        {
            throw new NotImplementedException("CALLP");
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

        void JUMP()
        {
            throw new NotImplementedException("JUMP");
        }

        void JF()
        {
            throw new NotImplementedException("JF");
        }

        void JT()
        {
            throw new NotImplementedException("JT");
        }

        void JFCHAIN()
        {
            throw new NotImplementedException("JFCHAIN");
        }

        void DUP()
        {
            throw new NotImplementedException("DUP");
        }

        void DUPX1()
        {
            throw new NotImplementedException("DUPX1");
        }

        void DROP()
        {
            throw new NotImplementedException("DROP");
        }

        void RET()
        {
            throw new NotImplementedException("RET");
        }

        void CALLE()
        {
            throw new NotImplementedException("CALLE");
        }

        void CONSA()
        {
            throw new NotImplementedException("CONSA");
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
            throw new NotImplementedException("RESETC");
        }

        void PUSHPEG()
        {
            throw new NotImplementedException("PUSHPEG");
        }

        void JUMPTBL()
        {
            throw new NotImplementedException("JUMPTBL");
        }

        void CALLX()
        {
            throw new NotImplementedException("CALLX");
        }

        void SWAP()
        {
            throw new NotImplementedException("SWAP");
        }

        void DROPN()
        {
            throw new NotImplementedException("DROPN");
        }

        void PUSHM()
        {
            throw new NotImplementedException("PUSHM");
        }

        void CALLV()
        {
            throw new NotImplementedException("CALLV");
        }

        void PUSHCI()
        {
            throw new NotImplementedException("PUSHCI");
        }


        private int ip { get; set; }

        private void Loop()
        {
            while (ip < bytecode.Module.codelen)
            {
                switch (bytecode.code[ip])
                {
                    case (byte)Opcode.ENTER: ENTER(); break;
                    case (byte)Opcode.LEAVE: LEAVE(); break;
                    case (byte)Opcode.PUSHB: PUSHB(); break;
                    case (byte)Opcode.PUSHN: PUSHN(); break;
                    case (byte)Opcode.PUSHS: PUSHS(); break;
                    case (byte)Opcode.PUSHT: PUSHT(); break;
                    case (byte)Opcode.PUSHPG: PUSHPG(); break;
                    case (byte)Opcode.PUSHPPG: PUSHPPG(); break;
                    case (byte)Opcode.PUSHPMG: PUSHPMG(); break;
                    case (byte)Opcode.PUSHPL: PUSHPL(); break;
                    case (byte)Opcode.PUSHPOL: PUSHPOL(); break;
                    case (byte)Opcode.PUSHI: PUSHI(); break;
                    case (byte)Opcode.LOADB: LOADB(); break;
                    case (byte)Opcode.LOADN: LOADN(); break;
                    case (byte)Opcode.LOADS: LOADS(); break;
                    case (byte)Opcode.LOADT: LOADT(); break;
                    case (byte)Opcode.LOADA: LOADA(); break;
                    case (byte)Opcode.LOADD: LOADD(); break;
                    case (byte)Opcode.LOADP: LOADP(); break;
                    case (byte)Opcode.STOREB: STOREB(); break;
                    case (byte)Opcode.STOREN: STOREN(); break;
                    case (byte)Opcode.STORES: STORES(); break;
                    case (byte)Opcode.STORET: STORET(); break;
                    case (byte)Opcode.STOREA: STOREA(); break;
                    case (byte)Opcode.STORED: STORED(); break;
                    case (byte)Opcode.STOREP: STOREP(); break;
                    case (byte)Opcode.NEGN: NEGN(); break;
                    case (byte)Opcode.ADDN: ADDN(); break;
                    case (byte)Opcode.SUBN: SUBN(); break;
                    case (byte)Opcode.MULN: MULN(); break;
                    case (byte)Opcode.DIVN: DIVN(); break;
                    case (byte)Opcode.MODN: MODN(); break;
                    case (byte)Opcode.EXPN: EXPN(); break;
                    case (byte)Opcode.EQB: EQB(); break;
                    case (byte)Opcode.NEB: NEB(); break;
                    case (byte)Opcode.EQN: EQN(); break;
                    case (byte)Opcode.NEN: NEN(); break;
                    case (byte)Opcode.LTN: LTN(); break;
                    case (byte)Opcode.GTN: GTN(); break;
                    case (byte)Opcode.LEN: LEN(); break;
                    case (byte)Opcode.GEN: GEN(); break;
                    case (byte)Opcode.EQS: EQS(); break;
                    case (byte)Opcode.NES: NES(); break;
                    case (byte)Opcode.LTS: LTS(); break;
                    case (byte)Opcode.GTS: GTS(); break;
                    case (byte)Opcode.LES: LES(); break;
                    case (byte)Opcode.GES: GES(); break;
                    case (byte)Opcode.EQT: EQT(); break;
                    case (byte)Opcode.NET: NET(); break;
                    case (byte)Opcode.LTT: LTT(); break;
                    case (byte)Opcode.GTT: GTT(); break;
                    case (byte)Opcode.LET: LET(); break;
                    case (byte)Opcode.GET: GET(); break;
                    case (byte)Opcode.EQA: EQA(); break;
                    case (byte)Opcode.NEA: NEA(); break;
                    case (byte)Opcode.EQD: EQD(); break;
                    case (byte)Opcode.NED: NED(); break;
                    case (byte)Opcode.EQP: EQP(); break;
                    case (byte)Opcode.NEP: NEP(); break;
                    case (byte)Opcode.ANDB: ANDB(); break;
                    case (byte)Opcode.ORB: ORB(); break;
                    case (byte)Opcode.NOTB: NOTB(); break;
                    case (byte)Opcode.INDEXAR: INDEXAR(); break;
                    case (byte)Opcode.INDEXAW: INDEXAW(); break;
                    case (byte)Opcode.INDEXAV: INDEXAV(); break;
                    case (byte)Opcode.INDEXAN: INDEXAN(); break;
                    case (byte)Opcode.INDEXDR: INDEXDR(); break;
                    case (byte)Opcode.INDEXDW: INDEXDW(); break;
                    case (byte)Opcode.INDEXDV: INDEXDV(); break;
                    case (byte)Opcode.INA: INA(); break;
                    case (byte)Opcode.IND: IND(); break;
                    case (byte)Opcode.CALLP: CALLP(); break;
                    case (byte)Opcode.CALLF: CALLF(); break;
                    case (byte)Opcode.CALLMF: CALLMF(); break;
                    case (byte)Opcode.CALLI: CALLI(); break;
                    case (byte)Opcode.JUMP: JUMP(); break;
                    case (byte)Opcode.JF: JF(); break;
                    case (byte)Opcode.JT: JT(); break;
                    case (byte)Opcode.JFCHAIN: JFCHAIN(); break;
                    case (byte)Opcode.DUP: DUP(); break;
                    case (byte)Opcode.DUPX1: DUPX1(); break;
                    case (byte)Opcode.DROP: DROP(); break;
                    case (byte)Opcode.RET: RET(); break;
                    case (byte)Opcode.CALLE: CALLE(); break;
                    case (byte)Opcode.CONSA: CONSA(); break;
                    case (byte)Opcode.CONSD: CONSD(); break;
                    case (byte)Opcode.EXCEPT: EXCEPT(); break;
                    case (byte)Opcode.ALLOC: ALLOC(); break;
                    case (byte)Opcode.PUSHNIL: PUSHNIL(); break;
                    case (byte)Opcode.JNASSERT: JNASSERT(); break;
                    case (byte)Opcode.RESETC: RESETC(); break;
                    case (byte)Opcode.PUSHPEG: PUSHPEG(); break;
                    case (byte)Opcode.JUMPTBL: JUMPTBL(); break;
                    case (byte)Opcode.CALLX: CALLX(); break;
                    case (byte)Opcode.SWAP: SWAP(); break;
                    case (byte)Opcode.DROPN: DROPN(); break;
                    case (byte)Opcode.PUSHM: PUSHM(); break;
                    case (byte)Opcode.CALLV: CALLV(); break;
                    case (byte)Opcode.PUSHCI: PUSHCI(); break;
                    default:
                        throw new NeonInvalidOpcodeException(string.Format("Invalid opcode ({0}) in bytecode file.", bytecode.code[ip]));
                }
            }
        }
    }

    static class Program
    {
        private static CommandLineOptions gOptions;

        private static void ShowUsage()
        {
            Console.Error.Write("Usage:\n\n");
            Console.Error.Write("   {0} [options] program.neonx\n", gOptions.ExecutableName);
            Console.Error.Write("\n Where [options] is one or more of the following:\n");
            Console.Error.Write("     -d       Display executor debug stats.\n");
            Console.Error.Write("     -D       Display executor disassembly during run.\n");
            Console.Error.Write("     -h       Display this help screen.\n");
            Console.Error.Write("     -n       No Assertions\n");
        }

        private static Boolean ParseOptions(string[] args)
        {
            Boolean Retval = false;
            for (int nIndex = 0; nIndex < args.Length; nIndex++)
            {
                if (args[nIndex][0] == '-')
                {
                    if (args[nIndex][1] == 'h' || args[nIndex][1] == '?' || ((args[nIndex][1] == '-' && args[nIndex][2] != '\0') && (args[nIndex][2] == 'h')))
                    {
                        ShowUsage();
                        Environment.Exit(1);
                    }
                    else if (args[nIndex][1] == 'D')
                    {
                        gOptions.ExecutorDisassembly = true;
                    }
                    else if (args[nIndex][1] == 'd')
                    {
                        gOptions.ExecutorDebugStats = true;
                    }
                    else if (args[nIndex][1] == 'n')
                    {
                        gOptions.EnableAssertions = false;
                    }
                    else
                    {
                        Console.Error.WriteLine(string.Format("Unknown option %s\n", args[nIndex]));
                        return false;
                    }
                }
                else
                {
                    Retval = true;
                    gOptions.Filename = args[nIndex];
                }
            }
            if (gOptions.Filename.Length == 0)
            {
                Console.Error.WriteLine("You must provide a Neon binary file to execute.\n");
                Retval = false;
            }
            return Retval;
        }

        static string GetApplicationName()
        {
            return System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        }

        static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                gOptions.ExecutableName = GetApplicationName();
                if (!ParseOptions(args))
                {
                    return 3;
                }

                System.IO.FileStream fs;
                try
                {
                    fs = new System.IO.FileStream(gOptions.Filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    Console.Error.Write("Could not open Neon executable: {0}\nError: {1}.\n", gOptions.Filename, ex.Message);
                    return 2;
                }


                long nSize = fs.Length;
                Executor exec = new Executor(gOptions);
                Byte[] code = new Byte[nSize];
                fs.Read(code, 0, (int)nSize);
                exec.bytecode = new Bytecode();
                exec.bytecode.LoadBytecode(code, (int)nSize);

                exec.diagnostics.time_start = DateTime.Now;
                exec.run(gOptions.EnableAssertions);
                exec.diagnostics.time_end = DateTime.Now;

                //    if (gOptions.ExecutorDebugStats)
                //    {
                //        Console.Error.Write("\n*** Neon CS Executor Statistics ***\n----------------------------------\n");
                //        Console.Error.Write("Total Opcode Executed : {0}\n", exec.diagnostics.total_opcodes);
                //        Console.Error.Write("Max Opstack Height     : {0}\n", exec.stack.max + 1);
                //        Console.Error.Write("Opstack Height         : {0}\n", exec.stack.size());
                //        Console.Error.Write("Max Callstack Height   : %" PRIu32 "\n"
                //        Console.Error.Write("CallStack Height       : %" PRId32 "\n"
                //        Console.Error.Write("Global Size            : %" PRIu32 "\n"
                //        Console.Error.Write("Max Framesets          : %d\n"
                //        Console.Error.Write("Execution Time         : %fms\n",

                //                        exec->stack->top,
                //                        exec->diagnostics.callstack_max_height + 1,
                //                        exec->callstacktop,
                //                        exec->object->global_size,
                //                        exec->framestack->max,
                //                        (((float)exec->diagnostics.time_end - exec->diagnostics.time_start) / CLOCKS_PER_SEC) * 1000
                //        );
                //    }
                //exec_freeExecutor(exec);
                //bytecode_freeBytecode(pModule);

                //free(bytecode);
            }
            return 0;
        }
    }
}
