
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class ExpresionEvaluator : AST
    {
        public Expresions expresion { get; set; }
        public int line;
        public ExpresionEvaluator(Expresions expresion, int line)
        {
            this.expresion = expresion;
            this.line = line;
        }
        public override void Execute()
        {
            expresion.Execute();
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            bool valid = expresion.SemanticCheck(errors, entorno);
            if (expresion.Type(entorno) != ExpresionsTypes.Funcion)
            {
                errors.Add(new Error(TypeOfError.Invalid, "Solo se permiten funciones", line));
                return false;
            }
            return valid;
        }
    }
}