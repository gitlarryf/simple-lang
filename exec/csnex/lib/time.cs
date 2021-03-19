using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace csnex.rtl
{
    public class time
    {
        private readonly Executor Exec;
        const ulong FILETIME_UNIX_EPOCH = 116444736000000000UL;

        public time(Executor exe)
        {
            Exec = exe;
        }

        public void sleep()
        {
            Number seconds = Exec.stack.Pop().Number;

            // ToDo: If the number of seconds is greater than MAX_INT / 1000, then call Sleep() multiple times.
            if (Number.IsGreaterThan(seconds, new Number(4294967))) {
                Exec.Raise("InvalidValueException", "");
                return;
            }
            int ms = Number.number_to_int32(Number.Multiply(seconds, new Number(1000)));
            Thread.Sleep(ms);
        }

        public void now()
        {
            DateTime ft = DateTime.Now;
            ulong ticks = (ulong)ft.ToFileTime();
            Exec.stack.Push(new Cell(Number.Divide(new Number(ticks - FILETIME_UNIX_EPOCH), new Number(10000000))));
        }

        public void tick()
        {
            DateTime now = DateTime.Now;

            Exec.stack.Push(new Cell(new Number(now.Ticks)));
        }
    }
}
