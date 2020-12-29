using System;

namespace csnex {


    public class Global
    {
        private Executor Exec;

        public Global(Executor exec)
        {
            Exec = exec;
            runtime.SetExecutor(Exec);
        }

        public bool dispatch(String name)
        {
            // ToDo: Isn't there a better way to do this?  System.Reflection maybe?
            // Globals
            switch (name) {
                case "print":
                    print();
                    return true;
                case "str":
                    str();
                    return true;

                // Boolean
                case "boolean__toString":
                    boolean__toString();
                    return true;

                // Number
                case "number__toString":
                    number__toString();
                    return true;

                // Object
                case "object__makeString":
                    object__makeString();
                    return true;

                // String
                case "string__concat":
                    string__concat();
                    return true;
            }

            // Modules
            // Runtime
            switch (name) {
                case "runtime$assertionsEnabled":
                    return runtime.assertionsEnabled();
                case "runtime$executorName":
                    return runtime.executorName();
            }
            throw new RtlException("global_callFunction(): \"{0}\" - invalid or unsupported predefined function call.", name);
        }

        public void print()
        {
            Object o = Exec.stack.Pop().Object;
            if (o == null) {
                System.Console.Out.WriteLine("NIL");
                return;
            }
            System.Console.Out.WriteLine(o.toString());
        }

        public void str()
        {
            Number v = Exec.stack.Pop().Number;
            Exec.stack.Push(new Cell(v.ToString()));
        }

        public void boolean__toString()
        {
            string s = Cell.toString(Exec.stack.Pop());
            Exec.stack.Push(new Cell(s));
        }

        public void object__makeString()
        {
            Cell o = Exec.stack.Pop();
            Exec.stack.Push(new Cell(new ObjectString(o.String)));
        }

        public void string__concat()
        {
            Cell b = Exec.stack.Pop();
            Cell a = Exec.stack.Pop();

            Exec.stack.Push(new Cell(a.String + b.String));
        }

        public void number__toString()
        {
            Cell s = Exec.stack.Pop();
            Exec.stack.Push(new Cell(Cell.toString(s)));
        }
    }
}
