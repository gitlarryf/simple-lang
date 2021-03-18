namespace csnex
{
    public enum Mode
    {
        read    = 0,
        write   = 1,
    }

    public enum SeekBase
    {
        absolute = 0,
        relative = 1,
        fromEnd  = 2,
    }

    public enum Platform
    {
        posix   = 0,
        win32   = 1,
    }
}
