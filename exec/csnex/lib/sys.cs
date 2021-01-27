using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csnex
{
    public partial class rtl
    {
        public static Executor exec;

        public rtl(Executor e)
        {
            exec = e;
        }

        public class sys
        {
            private Executor exec;
            public Cell args { get; private set; }

            public sys(Executor exe)
            {
                exec = exe;
                List<Cell> arr = new List<Cell>();
                for (int x = 1; x < Environment.GetCommandLineArgs().Length - 1; x++) {
                    arr.Add(new Cell(Environment.GetCommandLineArgs()[x]));
                }
                args = new Cell(arr);
            }

            public void exit()
            {
                Number x = exec.stack.Pop().Number;

                if (!x.IsInteger()) {
                    exec.RaiseLiteral("InvalidValueException", new Cell(string.Format("{0} {1}", "sys.exit invalid parameter:", x.ToString())));
                    return;
                }
                int r = Number.number_to_int32(x);
                if (r < 0 || r > 255) {
                    exec.RaiseLiteral("InvalidValueException", new Cell(string.Format("{0} {1}", "sys.exit invalid parameter:", x.ToString())));
                    return;
                }
                Environment.Exit(r);
            }
        }
    }
}
