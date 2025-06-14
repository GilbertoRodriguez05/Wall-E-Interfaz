using System.Collections.Generic;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public class Entorno
    {
        public Dictionary<string, object> Value = new Dictionary<string, object>();
        private Dictionary<string, ExpresionsTypes> Type = new Dictionary<string, ExpresionsTypes>();
        public Dictionary<string, Block> labels = new Dictionary<string, Block>();
        public Entorno()
        {

        }
        public object Execute(string name)
        {
            if (Value.ContainsKey(name)) return Value[name];
            else return null;
        }
        public void SetValue(string name, object value)
        {
            if (Value.ContainsKey(name)) Value[name] = value;
            else Value.Add(name, value);
        }
        public ExpresionsTypes GetType(string name)
        {
            if (Type.ContainsKey(name)) return Type[name];
            else return ExpresionsTypes.Error;
        }
        public void SetType(string name, ExpresionsTypes type)
        {
            if (Type.ContainsKey(name)) Type[name] = type;
            else Type.Add(name, type);
        }
        public void SetLabel(string name, Block label)
        {
            labels.Add(name, label);
        }

        public Block GetLabel(string name)
        {
            return labels[name];
        }
    }
}