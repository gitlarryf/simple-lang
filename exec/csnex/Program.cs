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
                    case (byte)Opcodes.ENTER: ENTER(); break;
                    case (byte)Opcodes.LEAVE: LEAVE(); break;
                    case (byte)Opcodes.PUSHB: PUSHB(); break;
                    case (byte)Opcodes.PUSHN: PUSHN(); break;
                    case (byte)Opcodes.PUSHS: PUSHS(); break;
                    case (byte)Opcodes.PUSHT: PUSHT(); break;
                    case (byte)Opcodes.PUSHPG: PUSHPG(); break;
                    case (byte)Opcodes.PUSHPPG: PUSHPPG(); break;
                    case (byte)Opcodes.PUSHPMG: PUSHPMG(); break;
                    case (byte)Opcodes.PUSHPL: PUSHPL(); break;
                    case (byte)Opcodes.PUSHPOL: PUSHPOL(); break;
                    case (byte)Opcodes.PUSHI: PUSHI(); break;
                    case (byte)Opcodes.LOADB: LOADB(); break;
                    case (byte)Opcodes.LOADN: LOADN(); break;
                    case (byte)Opcodes.LOADS: LOADS(); break;
                    case (byte)Opcodes.LOADT: LOADT(); break;
                    case (byte)Opcodes.LOADA: LOADA(); break;
                    case (byte)Opcodes.LOADD: LOADD(); break;
                    case (byte)Opcodes.LOADP: LOADP(); break;
                    case (byte)Opcodes.STOREB: STOREB(); break;
                    case (byte)Opcodes.STOREN: STOREN(); break;
                    case (byte)Opcodes.STORES: STORES(); break;
                    case (byte)Opcodes.STORET: STORET(); break;
                    case (byte)Opcodes.STOREA: STOREA(); break;
                    case (byte)Opcodes.STORED: STORED(); break;
                    case (byte)Opcodes.STOREP: STOREP(); break;
                    case (byte)Opcodes.NEGN: NEGN(); break;
                    case (byte)Opcodes.ADDN: ADDN(); break;
                    case (byte)Opcodes.SUBN: SUBN(); break;
                    case (byte)Opcodes.MULN: MULN(); break;
                    case (byte)Opcodes.DIVN: DIVN(); break;
                    case (byte)Opcodes.MODN: MODN(); break;
                    case (byte)Opcodes.EXPN: EXPN(); break;
                    case (byte)Opcodes.EQB: EQB(); break;
                    case (byte)Opcodes.NEB: NEB(); break;
                    case (byte)Opcodes.EQN: EQN(); break;
                    case (byte)Opcodes.NEN: NEN(); break;
                    case (byte)Opcodes.LTN: LTN(); break;
                    case (byte)Opcodes.GTN: GTN(); break;
                    case (byte)Opcodes.LEN: LEN(); break;
                    case (byte)Opcodes.GEN: GEN(); break;
                    case (byte)Opcodes.EQS: EQS(); break;
                    case (byte)Opcodes.NES: NES(); break;
                    case (byte)Opcodes.LTS: LTS(); break;
                    case (byte)Opcodes.GTS: GTS(); break;
                    case (byte)Opcodes.LES: LES(); break;
                    case (byte)Opcodes.GES: GES(); break;
                    case (byte)Opcodes.EQT: EQT(); break;
                    case (byte)Opcodes.NET: NET(); break;
                    case (byte)Opcodes.LTT: LTT(); break;
                    case (byte)Opcodes.GTT: GTT(); break;
                    case (byte)Opcodes.LET: LET(); break;
                    case (byte)Opcodes.GET: GET(); break;
                    case (byte)Opcodes.EQA: EQA(); break;
                    case (byte)Opcodes.NEA: NEA(); break;
                    case (byte)Opcodes.EQD: EQD(); break;
                    case (byte)Opcodes.NED: NED(); break;
                    case (byte)Opcodes.EQP: EQP(); break;
                    case (byte)Opcodes.NEP: NEP(); break;
                    case (byte)Opcodes.ANDB: ANDB(); break;
                    case (byte)Opcodes.ORB: ORB(); break;
                    case (byte)Opcodes.NOTB: NOTB(); break;
                    case (byte)Opcodes.INDEXAR: INDEXAR(); break;
                    case (byte)Opcodes.INDEXAW: INDEXAW(); break;
                    case (byte)Opcodes.INDEXAV: INDEXAV(); break;
                    case (byte)Opcodes.INDEXAN: INDEXAN(); break;
                    case (byte)Opcodes.INDEXDR: INDEXDR(); break;
                    case (byte)Opcodes.INDEXDW: INDEXDW(); break;
                    case (byte)Opcodes.INDEXDV: INDEXDV(); break;
                    case (byte)Opcodes.INA: INA(); break;
                    case (byte)Opcodes.IND: IND(); break;
                    case (byte)Opcodes.CALLP: CALLP(); break;
                    case (byte)Opcodes.CALLF: CALLF(); break;
                    case (byte)Opcodes.CALLMF: CALLMF(); break;
                    case (byte)Opcodes.CALLI: CALLI(); break;
                    case (byte)Opcodes.JUMP: JUMP(); break;
                    case (byte)Opcodes.JF: JF(); break;
                    case (byte)Opcodes.JT: JT(); break;
                    case (byte)Opcodes.JFCHAIN: JFCHAIN(); break;
                    case (byte)Opcodes.DUP: DUP(); break;
                    case (byte)Opcodes.DUPX1: DUPX1(); break;
                    case (byte)Opcodes.DROP: DROP(); break;
                    case (byte)Opcodes.RET: RET(); break;
                    case (byte)Opcodes.CALLE: CALLE(); break;
                    case (byte)Opcodes.CONSA: CONSA(); break;
                    case (byte)Opcodes.CONSD: CONSD(); break;
                    case (byte)Opcodes.EXCEPT: EXCEPT(); break;
                    case (byte)Opcodes.ALLOC: ALLOC(); break;
                    case (byte)Opcodes.PUSHNIL: PUSHNIL(); break;
                    case (byte)Opcodes.JNASSERT: JNASSERT(); break;
                    case (byte)Opcodes.RESETC: RESETC(); break;
                    case (byte)Opcodes.PUSHPEG: PUSHPEG(); break;
                    case (byte)Opcodes.JUMPTBL: JUMPTBL(); break;
                    case (byte)Opcodes.CALLX: CALLX(); break;
                    case (byte)Opcodes.SWAP: SWAP(); break;
                    case (byte)Opcodes.DROPN: DROPN(); break;
                    case (byte)Opcodes.PUSHM: PUSHM(); break;
                    case (byte)Opcodes.CALLV: CALLV(); break;
                    case (byte)Opcodes.PUSHCI: PUSHCI(); break;
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
                //        Console.Error.Write("Total Opcodes Executed : {0}\n", exec.diagnostics.total_opcodes);
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
