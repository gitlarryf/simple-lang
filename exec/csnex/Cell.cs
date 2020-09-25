using System;
using System.Collections.Generic;
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

        public Cell _Address;
        public List<Cell> _Array;
        public Boolean _Bool;
        public Dictionary<String, Cell> _Dictionary;
        public Decimal _Number;
        public String _String;
        
        public Types type { get; private set; }

        public Cell(Cell c)
        {
            switch (c.type) {
                case Types.Address:
                    this.Address = c.Address;
                    break;
                case Types.Array:
                    throw new NeonNotImplementedException();
                case Types.Boolean:
                    throw new NeonNotImplementedException();
                case Types.Dictionary:
                    throw new NeonNotImplementedException();
                case Types.Number:
                    throw new NeonNotImplementedException();
                case Types.String:
                    throw new NeonNotImplementedException();
                case Types.None:
                    throw new NeonNotImplementedException();
            }
            this.type = c.type;
        }

        public Cell(List<Cell> a)
        {
            this._Array = a;
            type = Types.Array;
        }

        public Cell(Boolean b)
        {
            this._Bool = b;
            type = Types.Boolean;
        }

        public Cell(Dictionary<String, Cell> d)
        {
            this._Dictionary = d;
            type = Types.Dictionary;
        }

        public Cell(Decimal d)
        {
            this._Number = d;
            type = Types.Number;
        }

        public Cell(String s)
        {
            this._String = s;
            type = Types.String;
        }

        
        public Cell Address
        {
            get {
                return this._Address;
            }
            set {
                this._Address = value;
            }
        }
    }
}
