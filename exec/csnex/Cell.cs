using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace csnex
{
    public class Cell
    {
        public enum Types
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

        public Types type { get; private set; }

        public Cell()
        {
            type = Types.None;
        }

        public Cell(Types t)
        {
            type = t;
        }

        public Cell(Cell c)
        {
            Address = c;
            type = Types.Address;
        }

        public Cell(List<Cell> a)
        {
            Array = a;
            type = Types.Array;
        }

        public Cell(Boolean b)
        {
            Boolean = b;
            type = Types.Boolean;
        }

        public Cell(Dictionary<String, Cell> d)
        {
            Dictionary = d;
            type = Types.Dictionary;
        }

        public Cell(Number d)
        {
            Number = d;
            type = Types.Number;
        }

        public Cell(Object o)
        {
            Object = o;
            type = Types.Object;
        }

        public Cell(String s)
        {
            String = s;
            type = Types.String;
        }

        public void FromCell(Cell c)
        {
            switch (c.type) {
                case Types.Address:
                    Address = c.Address;
                    break;
                case Types.Array:
                    throw new NeonNotImplementedException();
                case Types.Boolean:
                    throw new NeonNotImplementedException();
                case Types.Dictionary:
                    throw new NeonNotImplementedException();
                case Types.Number:
                    Number = c.Number;
                    break;
                case Types.String:
                    throw new NeonNotImplementedException();
                case Types.None:
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
            type = Types.None;
            //if (Address != null) {
            //    Address = new Cell();
            //}
            //Array = new List<Cell>();
            //Bool = false;
            //Dictionary = new Dictionary<string, Cell>();
            //Number = new Number();
            //String = "";
            //switch (type) {
            //    case Types.Address:
            //        Address = null;
            //        break;
            //    case Types.Array:
            //        _Array = new List<Cell>();
            //        break;
            //    case Types.Boolean:
            //        _Bool = false;
            //        break;
            //    case Types.Dictionary:
            //        _Dictionary = new Dictionary<string, Cell>();
            //        break;
            //    case Types.Number:
            //        Number = 0;
            //        break;
            //    case Types.String:
            //        String = "";
            //        break;
            //    case Types.None:
            //        //Debug.Assert(this.type != Types.None, "Cell type is Cell::Types::None");
            //        break;
            //}
        }

        static public string toString(Cell c)
        {
            switch (c.type) {
                case Types.Boolean:
                    if (c.Boolean) {
                        return "TRUE";
                    }
                    return "FALSE";
                case Types.Number:
                    return c.Number.ToString();
                default:
                    return "null";
            }
            throw new NeonNotImplementedException();
        }

        public Cell ArrayIndexForWrite(uint i)
        {
            if (type == Types.None) {
                type = Types.Array;
            }
            Debug.Assert(type == Types.Array);
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
            if (type == Types.None) {
                type = Types.Array;
            }
            Debug.Assert(type == Types.Array);
            return Array.ElementAt((int)i);
        }

    }
}
