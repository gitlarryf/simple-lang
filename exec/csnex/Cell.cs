using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace csnex
{
    public class Cell
    {
        public enum Type
        {
            None,
            Address,
            Array,
            Boolean,
            Bytes,
            Dictionary,
            Number,
            Object,
            Other,
            String,
        }

        public Cell Address;
        public List<Cell> Array;
        public Boolean Boolean;
        public Dictionary<String, Cell> Dictionary;
        public Number Number;
        public Object Object;
        public String String;

        public Type type { get; private set; }

        public Cell()
        {
            type = Type.None;
        }

        public Cell(Type t)
        {
            type = t;
        }

        public Cell(Cell c)
        {
            Address = c;
            type = Type.Address;
        }

        public Cell(List<Cell> a)
        {
            Array = a;
            type = Type.Array;
        }

        public Cell(Boolean b)
        {
            Boolean = b;
            type = Type.Boolean;
        }

        public Cell(Dictionary<String, Cell> d)
        {
            Dictionary = d;
            type = Type.Dictionary;
        }

        public Cell(Number d)
        {
            Number = d;
            type = Type.Number;
        }

        public Cell(Object o)
        {
            Object = o;
            type = Type.Object;
        }

        public Cell(String s)
        {
            String = s;
            type = Type.String;
        }

        public void FromCell(Cell c)
        {
            switch (c.type) {
                case Type.Address:
                    Address = c.Address;
                    break;
                case Type.Array:
                    throw new NeonNotImplementedException();
                case Type.Boolean:
                    throw new NeonNotImplementedException();
                case Type.Dictionary:
                    throw new NeonNotImplementedException();
                case Type.Number:
                    Number = c.Number;
                    break;
                case Type.String:
                    throw new NeonNotImplementedException();
                case Type.None:
                    throw new NeonNotImplementedException();
            }
            type = c.type;
        }

        public void ResetCell()
        {
            Address = null;
            Boolean = false;
            Dictionary = null;
            Number = null;
            Object = null;
            String = null;
            type = Type.None;
        }

        static public string toString(Cell c)
        {
            switch (c.type) {
                case Type.Boolean:
                    if (c.Boolean) {
                        return "TRUE";
                    }
                    return "FALSE";
                case Type.Number:
                    return c.Number.ToString();
            }
            throw new NeonNotImplementedException();
        }

        public Cell ArrayIndexForWrite(uint i)
        {
            if (type == Type.None) {
                type = Type.Array;
            }
            Debug.Assert(type == Type.Array);
            if (Array == null) {
                Array = new List<Cell>();
            }
            if (i >= Array.Count) {
                int s = Array.Count;
                for (int n = s; n < i+1; n++) {
                    Array.Insert(n, new Cell());
                }
            }
            return Array.ElementAt((int)i);
        }

        public Cell ArrayIndexForRead(uint i)
        {
            if (type == Type.None) {
                type = Type.Array;
            }
            Debug.Assert(type == Type.Array);
            return Array.ElementAt((int)i);
        }
    }
}
