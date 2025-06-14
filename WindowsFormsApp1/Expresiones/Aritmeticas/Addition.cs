using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class Addition : BinaryExpresions
    {
        public override object value { get; set; }
        public Addition(Expresions Left, Expresions Right)
        {
            this.Left = Left;
            this.Right = Right;
        }
        public override void Execute()
        {
            Left.Execute();
            Right.Execute();
            value = Convert.ToInt32(Left.value) + Convert.ToInt32(Right.value);
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            if (Left.Type(entorno) != ExpresionsTypes.Numero || Right.Type(entorno) != ExpresionsTypes.Numero) return ExpresionsTypes.Error;
            return ExpresionsTypes.Numero;
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            bool right = Right.SemanticCheck(errors, entorno);
            bool left = Left.SemanticCheck(errors, entorno);
            if (Right.Type(entorno) != ExpresionsTypes.Numero || Left.Type(entorno) != ExpresionsTypes.Numero)
            {
                errors.Add(new Error(TypeOfError.Expected, "La adicion debe ser entre dos numeros"));
                return false;
            }
            return left && right;
        }
    }
}