using System.IO;
using System.Runtime.InteropServices;

namespace csnex.rtl
{
    internal sealed class NativeFunctions
    {
        [DllImport("libc.so", EntryPoint="symlink")]
        internal static extern int CreateSymlink(string target, string newlink);
        [DllImport("libc.so")]
        internal static extern string strerror(int err);
    }
    
    public partial class file
    {
        private enum Errors {
            ENOENT        =   2,      /* No such file or directory */
            EACCES        =  13,      /* Permission denied */
            EEXIST        =  17,      /* File exists */
        }

        private void handle_error(int error,  string path)
        {
            switch ((Errors)error) {
                case Errors.EACCES: Exec.Raise("FileException.PermissionDenied", path);      break;
                case Errors.EEXIST: Exec.Raise("FileException.DirectoryExists", path);       break;
                case Errors.ENOENT: Exec.Raise("FileException.PathNotFound", path);          break;
                default: {
                    string err = string.Format("{0}: {1}", path, NativeFunctions.strerror(error));
                    Exec.Raise("FileException", err);
                    break;
                }
            }
        }

        // This might not be necessary.  It should be handled by the framework on the given OS.
        //public void _CONSTANT_Separator()
        //{
        //    Exec.stack.Push(new Cell(Path.PathSeparator.ToString()));
        //}

        public void symlink()
        {
            Exec.stack.Pop(); // bool targetIsDirectory
            string newlink = Exec.stack.Pop().String;
            string target = Exec.stack.Pop().String;

            int r = NativeFunctions.CreateSymlink(newlink, target);
            if (r != 0) {
                handle_error(r, newlink);
            }
        }
    }
}
