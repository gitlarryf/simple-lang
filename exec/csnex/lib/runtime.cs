using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csnex
{
    public partial class rtl
    {
        public class runtime
        {
            private Executor exec;
            public runtime(Executor ex)
            {
                exec = ex;
            }

            public void assertionsEnabled()
            {
                exec.stack.Push(new Cell(exec.enable_assert));
            }

            public void executorName()
            {
                exec.stack.Push(new Cell("csnex"));
            }

            public void setRecursionLimit()
            {
                exec.ParamRecursionLimit = Number.number_to_int32(exec.stack.Pop().Number);
            }
        }
    }
}
