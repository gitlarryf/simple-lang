using System.Collections.Generic;

namespace csnex
{
    internal static partial class CodeExtensions
    {
        public static int Compare(this byte[] self, byte[] rhs)
        {
            if (self == null && rhs == null) {
                return 0;
            } else if (self != null && rhs == null) {
                if (self.Length == 0) {
                    return 0;
                }
                return 1;
            } else if (self == null && rhs != null) {
                if (rhs.Length == 0) {
                    return 0;
                }
                return -1;
            }

            int i;
            for (i = 0; i < self.Length && i < rhs.Length; i++) {
                if (self[i] > rhs[i]) {
                    return 1;
                } else if (self[i] < rhs[i]) {
                    return -1;
                }
            }
            if (i == self.Length && i == rhs.Length) {
                return 0; // The lhs and rhs are proven to be equal to one another.
            } else if (i == self.Length) {
                return -1; // rhs still has elements to process, so we must be LESS THAN the rhs.
            } else if (i == rhs.Length) {
                return 1; // lhs still has elements to process, so we must be GREATER than the rhs.
            }
            // Could this ever happen?
            return 0;
        }

        public static List<Cell> ToCellArray(this List<string> self)
        {
            List<Cell> r = new List<Cell>();
            foreach (string value in self) {
                r.Add(new Cell(value));
            }
            return r;
        }

        public static int StrSpn(this string self, int index, string pattern)
        {
            int count = index;
            string srch = self.Substring(index);
            foreach (char c in srch) {
                bool bFound = false;
                foreach (char p in pattern) {
                    if (c == p) {
                        count++;
                        bFound = true;
                        break;
                    }
                }
                if (!bFound) {
                    if (count == srch.Length) {
                        return -1;
                    }
                    break;
                }
            }
            return count;
        }
    }
}
