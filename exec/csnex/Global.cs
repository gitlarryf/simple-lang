using System;

namespace csnex {


    public class Global
    {
        private Executor Exec;

        public Global(Executor exec)
        {
            Exec = exec;
        }

        public bool dispatch(String name) 
        {
            switch (name) {
                case "print":
                    print();
                    return true;
                case "str":
                    str();
                    return true;
                default:
                    break;
            }
            switch (name) {
                case "string__concat":
                    string_concat();
                    return true;
            }
            return false;
        }

        public void print()
        {
            Cell str = Exec.stack.Pop();
            System.Console.Out.WriteLine(str.String);
        }

        public void str()
        {
            Number v = Exec.stack.Pop().Number;
            Exec.stack.Push(new Cell(v.ToString()));
        }

        public void string_concat()
        {
            Cell b = Exec.stack.Pop();
            Cell a = Exec.stack.Pop();

            String r = new string(a.String.ToCharArray());
            r += b.String;
            Exec.stack.Push(new Cell(r));
        }

        public void number_toString()
        {
            Cell s = Exec.stack.Pop();
            Exec.stack.Push(new Cell(Cell.toString(s)));
        }


    }
}
