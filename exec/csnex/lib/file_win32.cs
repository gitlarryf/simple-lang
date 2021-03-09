using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace csnex.lib
{
    internal sealed class NativeMethods
    {
        [DllImport("kernel32.dll", ThrowOnUnmappableChar=true, CharSet=CharSet.Unicode)]
        internal static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);
    }

    public partial class file
    {
        public void symlink()
        {
            bool targetIsDirectory = Exec.stack.Pop().Boolean;
            string newlink = Exec.stack.Pop().String;
            string target = Exec.stack.Pop().String;

            try {
                NativeMethods.CreateSymbolicLink(newlink, target, targetIsDirectory ? 1 : 0);
            } catch (Exception ex) {
                handle_error(ex, newlink);
            }
        }
    }
}
