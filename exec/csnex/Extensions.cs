using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Reflection;

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
