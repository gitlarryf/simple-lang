using System;
using System.Diagnostics;

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

    static class csnex
    {
        private static CommandLineOptions gOptions;

        private static void ShowUsage()
        {
            Console.Error.Write("Usage:\n\n");
            Console.Error.Write("   {0} [options] program.neonx\n", gOptions.ExecutableName);
            Console.Error.Write("\n Where [options] is one or more of the following:\n");
            Console.Error.Write("     -d       Display executor debug stats.\n");
            Console.Error.Write("     -t       Trace execution disassembly during run.\n");
            Console.Error.Write("     -h       Display this help screen.\n");
            Console.Error.Write("     -n       No Assertions\n");
        }

        private static Boolean ParseOptions(string[] args)
        {
            Boolean Retval = false;
            for (int nIndex = 0; nIndex < args.Length; nIndex++) {
                if (args[nIndex][0] == '-') {
                    if (args[nIndex][1] == 'h' || args[nIndex][1] == '?' || ((args[nIndex][1] == '-' && args[nIndex][2] != '\0') && (args[nIndex][2] == 'h'))) {
                        ShowUsage();
                        Environment.Exit(1);
                    } else if (args[nIndex][1] == 't') {
                        gOptions.ExecutorDisassembly = true;
                    } else if(args[nIndex][1] == 'd') {
                        gOptions.ExecutorDebugStats = true;
                    } else if(args[nIndex][1] == 'n') {
                        gOptions.EnableAssertions = false;
                    } else {
                        Console.Error.WriteLine(string.Format("Unknown option {0}\n", args[nIndex]));
                        return false;
                    }
                } else {
                    Retval = true;
                    gOptions.Filename = args[nIndex];
                }
            }
            if(gOptions.Filename.Length == 0) {
                Console.Error.WriteLine("You must provide a Neon binary file to execute.\n");
                Retval = false;
            }
            return Retval;
        }

        static string GetApplicationName()
        {
            return Process.GetCurrentProcess().ProcessName;
        }

        static int Main(string[] args)
        {
            int retval = 0;
            Stopwatch sw = new Stopwatch();
            Console.WriteLine("{0,02:x2}", 0xFFF);
            gOptions.ExecutableName = GetApplicationName();
            if(!ParseOptions(args)) {
                return 3;
            }

            System.IO.FileStream fs;
            try {
                fs = new System.IO.FileStream(gOptions.Filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            } catch(Exception ex) {
                Console.Error.Write("Could not open Neon executable: {0}\nError: {1} - {2}.\n", gOptions.Filename, ex.HResult & 0xffff, ex.Message);
                return 2;
            }

            long nSize = fs.Length;
            Executor exec = new Executor(gOptions);
            Byte[] code = new Byte[nSize];
            fs.Read(code, 0, (int)nSize);
            fs.Close();

            exec.bytecode = new Bytecode();
            exec.bytecode.LoadBytecode(gOptions.Filename, code, (uint)nSize); // ToDo: Fix this to be 64 bit, or correct size for program ABI

            exec.diagnostics.timer.Start();
            retval = exec.run(gOptions.EnableAssertions);
            exec.diagnostics.timer.Stop();

            if(gOptions.ExecutorDebugStats) {
                Console.Error.Write("\n*** Neon CS Executor Statistics ***\n----------------------------------\n");
                Console.Error.Write("Total Opcodes Executed : {0}\n", exec.diagnostics.total_opcodes);
                Console.Error.Write("Max Opstack Height     : {0}\n", exec.diagnostics.opstack_max);
                Console.Error.Write("Opstack Height         : {0}\n", exec.stack.Count);
                // ToDo: Implement these later...
                //Console.Error.Write("Max Callstack Height   : {0}\n", exec.?);
                //Console.Error.Write("CallStack Height       : {0}\n", exec.?);
                //Console.Error.Write("Global Size            : {0}\n", exec.?);
                //Console.Error.Write("Max Framesets          : {0}\n", exec.?);
                Console.Error.Write("Execution Time         : {0}ms\n", exec.diagnostics.timer.ElapsedMilliseconds);
            }
            return retval;
        }
    }
}
