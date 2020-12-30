using System;

internal static partial class CodeExtensions
{
    public static byte[] CopyFrom(this byte[] source, uint start, int len)
    {
        byte[] dest = new byte[len];

        for (int x = 0; x < len; x++) {
            dest[x] = source[start + x];
        }
        return dest;
    }
}
