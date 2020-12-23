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
        public Boolean Bool;
        public Dictionary<String, Cell> Dictionary;
        public Number Number;
        public String String;

        public Types type { get; private set; }

        public Cell(Cell c)
        {
            Address = c;
            type = Types.Address;
        }

        public Cell()
        {
            type = Types.None;
        }

        public Cell(List<Cell> a)
        {
            Array = a;
            type = Types.Array;
        }

        public Cell(Boolean b)
        {
            Bool = b;
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

        public Cell(String s)
        {
            String = s;
            type = Types.String;
        }

        
        //public Cell Address
        //{
        //    get {
        //        return _Address;
        //    }
        //    set {
        //        _Address = value;
        //    }
        //}

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
            Address = new Cell();
            Array = new List<Cell>();
            Bool = false;
            Dictionary = new Dictionary<string, Cell>();
            Number = new Number();
            String = "";
            type = Types.None;
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
                case Types.Number:
                    return c.Number.ToString();
            }
            return "null";
        }
    }
}
