
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class Multiplication : BinaryExpresions
    {
        public override object value { get; set; }
        public int line;
        public Multiplication(Expresions Left, Expresions Right, int line)
        {
            this.Left = Left;
            this.Right = Right;
            this.line = line;
        }
        public override void Execute()
        {
            Right.Execute();
            Left.Execute();
            value = Convert.ToInt32(Right.value) * Convert.ToInt32(Left.value);
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            bool right = Right.SemanticCheck(errors, entorno);
            bool left = Left.SemanticCheck(errors, entorno);
            if (Right.Type(entorno) != ExpresionsTypes.Numero || Left.Type(entorno) != ExpresionsTypes.Numero)
            {
                errors.Add(new Error(TypeOfError.Expected, "La multiplicacion debe ser entre dos numero", line));
                return false;
            }
            return right && left;
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            if (Right.Type(entorno) != ExpresionsTypes.Numero || Left.Type(entorno) != ExpresionsTypes.Numero) return ExpresionsTypes.Error;
            return ExpresionsTypes.Numero;
        }
    }
}