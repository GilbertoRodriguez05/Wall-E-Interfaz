using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class Cadena : Expresions
    {
        public override object value { get; set; }
        public string SubCadena;
        public Cadena(string value)
        {
            this.value = value;
        }
        public override void Execute()
        {
            if (value is string Value)
            {
                SubCadena = Value.Substring(1, Value.Length - 2);
            }
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            return true;
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            return ExpresionsTypes.Cadena;
        }
    }
}