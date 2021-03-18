using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csnex.rtl
{
    internal sealed class ProcessObject: Object
    {
        public ProcessObject(Process p)
        {
            handle = p;
        }

        public override string toString()
        {
            return string.Format("<PROCESS {0}>", handle.Handle);
        }

        public Process handle;
    }


    public class os
    {
        private readonly  Executor Exec;
        public os(Executor exe)
        {
            Exec = exe;
        }

        private Process check_process(Object pp)
        {
            ProcessObject po = (ProcessObject)pp;
            if (po == null || po.handle == null) {
                throw new NeonRuntimeException("OsException.InvalidProcess", "");
            }
            return po.handle;
        }

        public void getenv()
        {
            string name = Exec.stack.Pop().String;

            string r = Environment.GetEnvironmentVariable(name);

            if (r == null || r.Length == 0) {
                Exec.stack.Push(new Cell(""));
                return;
            }
            Exec.stack.Push(new Cell(r));
        }

        public void system()
        {
            string cmd = Exec.stack.Pop().String;

            Exec.stack.Push(new Cell(new Number(Process.Start(cmd).Id)));
        }

        public void chdir()
        {
            string path = Exec.stack.Pop().String;

            try {
                Environment.CurrentDirectory = path;
            } catch {
                throw new NeonRuntimeException("OsException.PathNotFound", path);
            }
        }

        public void getcwd()
        {
            string buf = Environment.CurrentDirectory;
            Exec.stack.Push(new Cell(buf));
        }

        public void kill()
        {
            Object process = Exec.stack.Pop().Object;
            Process p = check_process(process);

            p.Close();
            p = null;
        }

        public void platform()
        {
            Platform os = 0;
            switch (Environment.OSVersion.Platform) {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                    os = Platform.win32;
                    break;
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                    os = Platform.posix;
                    break;
                default:
                    throw new NeonRuntimeException("OsException.UnknownPlatform", Environment.OSVersion.Platform.ToString());
            }
            Exec.stack.Push(new Cell(new Number((int)os)));
        }

        public void spawn()
        {
            string cmd = Exec.stack.Pop().String;

            ProcessStartInfo psi = new ProcessStartInfo(cmd);
            psi.CreateNoWindow = true;
            psi.ErrorDialog = false;
            psi.UseShellExecute = true;
            try {
                ProcessObject po = new ProcessObject(Process.Start(psi));
                Exec.stack.Push(new Cell(po));
            } catch {
                throw new NeonRuntimeException("OsException.PathNotFound", cmd);
            }
        }

        public void wait()
        {
            Object process = Exec.stack.Pop().Object;
            int r;
            {
                Process p = check_process(process);
                p.WaitForExit();
                r = p.ExitCode;
                p.Close();
            }
            Exec.stack.Push(new Cell(new Number(r)));
        }
    }
}
