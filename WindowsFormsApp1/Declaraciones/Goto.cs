using System.Collections.Generic;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public class GoTo : AST
    {
        string Label;
        Expresions Condition;
        Entorno entorno;
        public GoTo(string Label, Expresions Condition, Entorno entorno)
        {
            this.Label = Label;
            this.Condition = Condition;
            this.entorno = entorno;
        }
        public override void Execute()
        {
            Condition.Execute();
            Block bloque = entorno.GetLabel(Label);
            
            if ((bool)Condition.value)
            {
                PilaCompartida.Push(true);                
                bloque.Execute();
                return;
            }
            else
            {
                PilaCompartida.Push(false);
            }
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            bool cond = Condition.SemanticCheck(errors, entorno);
            if (Condition.Type(entorno) != ExpresionsTypes.Bool)
            {
                errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo bool"));
                return false;
            }
            else if (!entorno.labels.ContainsKey(Label))
            {
                errors.Add(new Error(TypeOfError.VariableUndefined, "Label no definida"));
                return false;
            }
            return Condition.SemanticCheck(errors, entorno);
        }
    }
}