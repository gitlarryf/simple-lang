using System.Collections.Generic;

namespace csnex {

    public abstract class Object
    {
        ~Object()
        {
        }

        public virtual bool getBoolean(ref bool b)
        {
            return false;
        }

        public virtual bool getNumber(ref Number n)
        {
            return false;
        }

        public virtual bool getString(ref string s)
        {
            return false;
        }

        public virtual bool getBytes(ref List<byte> b)
        {
            return false;
        }

        public virtual bool getArray(ref List<Object> a)
        {
            return false;
        }

        public virtual bool getDictionary(ref Dictionary<string, Object> o)
        {
            return false;
        }

        public virtual bool invokeMethod(string name, List<Object> args, ref Object result)
        {
            return false;
        }

        public virtual bool setProperty(Object a, ref Object b)
        {
            return false;
        }

        public virtual bool subscript(Object a, ref Object b)
        {
            return false;
        }

        public virtual string toLiteralString()
        {
            return toString();
        }

        public abstract string toString();
    }

    public class ObjectBoolean: Object
    {
        public ObjectBoolean(bool b)
        {
            val = b;
        }

        public ObjectBoolean(ObjectBoolean o)
        {
            val = o.getBoolean(ref val);
        }

        public override bool getBoolean(ref bool r)
        {
            r = val;
            return true;
        }

        public override string toString()
        {
            return val ? "TRUE" : "FALSE";
        }

        private bool val;
    };

    public class ObjectNumber: Object
    {
        public ObjectNumber(Number n)
        {
            this.n = n;
        }

        public override bool getNumber(ref Number r)
        {
            r = n;
            return true;
        }

        public override string toString()
        {
            return n.ToString();
        }

        private Number n;
    };

    public class ObjectString: Object
    {
        public ObjectString(string s)
        {
            this.s = s;
        }

        public override bool getString(ref string r)
        {
            r = s;
            return true;
        }

        // TODO: Use quoting function to quote the value properly.
        public override string toLiteralString()
        {
            return "\"" + s + "\"";
        }

        public override string toString()
        {
            return s;
        }

        private string s;
    };

    public class ObjectBytes: Object
    {
        public ObjectBytes(List<byte> b)
        {
            this.b = b;
        }

        public override bool getBytes(ref List<byte> r)
        {
            r = b;
            return true;
        }

        public override string toString()
        {
            string r = "HEXBYTES \"";
            bool first = true;
            foreach (char x in b) {
                if (first) {
                    first = false;
                } else {
                    r += ' ';
                }
                r += string.Format("{0:x2}", x);
            }
            r += "\"";
            return r;
        }
        private List<byte> b;
    }

    public abstract class ObjectArray : Object
    {
        public ObjectArray(List<Object> a)
        {
            this.a = a;
        }

        public override bool getArray(ref List<Object> r)
        {
            r = a;
            return true;
        }

        public abstract override bool subscript(Object index, ref Object r);
        public abstract override string toString();

        private List<Object> a;
    }

    public abstract class ObjectDictionary : Object
    {
        public ObjectDictionary(Dictionary<string, List<Object>> d)
        {
            this.d = d;
        }

        public virtual bool getDictionary(ref Dictionary<string, List<Object>> r)
        {
            r = d;
            return true;
        }

        public abstract override bool subscript(Object index, ref Object r);
        public abstract override string toString();

        private Dictionary<string, List<Object>> d;
    }
}
