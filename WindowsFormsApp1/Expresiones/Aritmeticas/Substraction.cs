
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class Substraction : BinaryExpresions
    {
        public override object value { get; set; }
        public int line;
        public Substraction(Expresions left, Expresions right, int line)
        {
            this.Right = right;
            this.Left = left;
            this.line = line;
        }
        public override void Execute()
        {
            Right.Execute();
            Left.Execute();
            value = Convert.ToInt32(Left.value) - Convert.ToInt32(Right.value);
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            bool right = Right.SemanticCheck(errors, entorno);
            bool left = Left.SemanticCheck(errors, entorno);
            if (Right.Type(entorno) != ExpresionsTypes.Numero || Left.Type(entorno) != ExpresionsTypes.Numero)
            {
                errors.Add(new Error(TypeOfError.Expected, "La resta tiene que ser entre dos numeros", line));
                return false;
            }
            return left && right;
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            if (Right.Type(entorno) != ExpresionsTypes.Numero || Left.Type(entorno) != ExpresionsTypes.Numero) return ExpresionsTypes.Error;
            return ExpresionsTypes.Numero;
        }
    }
}