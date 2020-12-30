namespace csnex {
    public class runtime
    {
        private static Executor exec;

        public static void SetExecutor(Executor e)
        {
            exec = e;
        }

        public static bool assertionsEnabled()
        {
            exec.stack.Push(new Cell(exec.enable_assert));
            return true;
        }

        public static bool executorName()
        {
            exec.stack.Push(new Cell("csnex"));
            return true;
        }
    }
}
