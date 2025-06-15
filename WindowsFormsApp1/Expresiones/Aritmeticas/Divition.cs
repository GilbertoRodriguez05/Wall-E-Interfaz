
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class Divition : BinaryExpresions
    {
        public override object value { get; set; }
        public int line;
        public Divition(Expresions Right, Expresions Left, int line)
        {
            this.Right = Right;
            this.Left = Left;
            this.line = line;
        }
        public override void Execute()
        {
            Right.Execute();
            Left.Execute();
            value = Convert.ToInt32(Right.value) / Convert.ToInt32(Left.value);
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            bool right = Right.SemanticCheck(errors, entorno);
            bool left = Left.SemanticCheck(errors, entorno);
            if (Right.Type(entorno) != ExpresionsTypes.Numero || Left.Type(entorno) != ExpresionsTypes.Numero)
            {
                errors.Add(new Error(TypeOfError.Expected, "La division solo se puede hacer entre dos numeros", line));
                return false;
            }
            else if (Convert.ToInt32(Right.value) == 0)
            {
                errors.Add(new Error(TypeOfError.Invalid, "La division por 0 no esta definida", line));
                return false;
            }
            return right && left;
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            if (Left.Type(entorno) != ExpresionsTypes.Numero || Right.Type(entorno) != ExpresionsTypes.Numero)
            {
                return ExpresionsTypes.Error;
            }
            else if (Convert.ToInt32(Right.value) == 0)
            {
                return ExpresionsTypes.Error;
            }
            else return ExpresionsTypes.Numero;
        }
    }
}