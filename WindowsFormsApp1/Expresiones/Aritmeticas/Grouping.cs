
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class Grouping : Expresions
    {
        public override object value { get; set; }
        Expresions expresions;
        public int line;
        public Grouping(Expresions expresions, int line)
        {
            this.expresions = expresions;
            this.line = line;
        }
        public override void Execute()
        {
            expresions.Execute();
            value = expresions.value;
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            bool group = expresions.SemanticCheck(errors, entorno);
            if (expresions.Type(entorno) == ExpresionsTypes.Numero) return group;
            else if (expresions.Type(entorno) == ExpresionsTypes.Cadena) return group;
            else if (expresions.Type(entorno) == ExpresionsTypes.Bool) return group;
            else
            {
                errors.Add(new Error(TypeOfError.Invalid, "Expresion invalida", line));
                return false;
            }
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            if (expresions.Type(entorno) == ExpresionsTypes.Numero) return ExpresionsTypes.Numero;
            else if (expresions.Type(entorno) == ExpresionsTypes.Cadena) return ExpresionsTypes.Cadena;
            else if (expresions.Type(entorno) == ExpresionsTypes.Bool) return ExpresionsTypes.Bool;
            else return ExpresionsTypes.Error;
        }
    }
}