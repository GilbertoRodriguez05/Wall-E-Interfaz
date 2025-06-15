using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class Diferent : BinaryExpresions
    {
        public override object value { get; set; }
        public int line;
        public Diferent(Expresions Right, Expresions Left, int line)
        {
            this.Right = Right;
            this.Left = Left;
            this.line = line;
        }
        public override void Execute()
        {
            Right.Execute();
            Left.Execute();
            value = Left.value == Right.value;

        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            bool right = Right.SemanticCheck(errors, entorno);
            bool left = Left.SemanticCheck(errors, entorno);
            if (Right.Type(entorno) != ExpresionsTypes.Numero || Left.Type(entorno) != ExpresionsTypes.Numero)
            {
                errors.Add(new Error(TypeOfError.Expected, "Solo se puede comparar entre dos expresiones del mismo tipo", line));
                return false;
            }
            else if (Right.Type(entorno) != ExpresionsTypes.Bool || Left.Type(entorno) != ExpresionsTypes.Bool)
            {
                errors.Add(new Error(TypeOfError.Expected, "Solo se puede comparar entre dos expresiones del mismo tipo", line));
                return false;
            }
            else if (Right.Type(entorno) != ExpresionsTypes.Cadena || Left.Type(entorno) != ExpresionsTypes.Cadena)
            {
                errors.Add(new Error(TypeOfError.Expected, "Solo se puede comparar entre dos expresiones del mismo tipo", line));
                return false;
            }
            return right && left;
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            if (Right.Type(entorno) != Left.Type(entorno))
            {
                return ExpresionsTypes.Error;
            }
            
            return ExpresionsTypes.Bool;
        }
    }
}