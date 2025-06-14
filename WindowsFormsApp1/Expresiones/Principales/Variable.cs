using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    class Variable : Expresions
    {
        public string var;
        public Entorno entorno { get; set; }
        public override object value { get; set; }
        public ExpresionsTypes type { get; set; }
        public Variable(string var, Entorno entorno)
        {
            this.var = var;
            this.entorno = entorno; 
        }
        public override void Execute()
        {
            this.value = entorno.Execute(var);
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            if (entorno.GetType(var) == ExpresionsTypes.Error)
            {
                errors.Add(new Error(TypeOfError.VariableUndefined, "La variable no esta definida"));
                type = ExpresionsTypes.Error;
                return false;
            }
            type = entorno.GetType(var);
            return true;
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            if (entorno.GetType(var) == ExpresionsTypes.Error) return ExpresionsTypes.Error;
            else return entorno.GetType(var);
        }

        public override string ToString()
        {
            return string.Format("{0}", var);
        }
    }
}