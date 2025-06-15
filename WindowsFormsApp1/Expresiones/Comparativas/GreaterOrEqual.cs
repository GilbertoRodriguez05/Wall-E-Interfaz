using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class GreaterOrEqual : BinaryExpresions
    {
        public override object value { get; set; }
        public int line;
        public GreaterOrEqual(Expresions Right, Expresions Left, int line)
        {
            this.Right = Right;
            this.Left = Left;
            this.line = line;
        }
        public override void Execute()
        {
            Right.Execute();
            Left.Execute();
            value = Convert.ToInt32(Left.value) >= Convert.ToInt32(Right.value);
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            bool right = Right.SemanticCheck(errors, entorno);
            bool left = Left.SemanticCheck(errors, entorno);
            if (Right.Type(entorno) != ExpresionsTypes.Numero || Left.Type(entorno) != ExpresionsTypes.Numero)
            {
                errors.Add(new Error(TypeOfError.Expected, "Solo se puede comparar entre dos numeros", line));
                return false;
            }
            return right && left;
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            if (Right.Type(entorno) != ExpresionsTypes.Numero || Left.Type(entorno) != ExpresionsTypes.Numero)
            {
                return ExpresionsTypes.Error;
            }
            return ExpresionsTypes.Bool;
        }
    }
}