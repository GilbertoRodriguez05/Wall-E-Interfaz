using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    class Equal : BinaryExpresions
    {
        public override object value { get; set; }
        public Equal(Expresions Right, Expresions Left)
        {
            this.Right = Right;
            this.Left = Left;
        }
        public override void Execute()
        {
            Right.Execute();
            Left.Execute();
            value = Right.value == Left.value;

        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            bool right = Right.SemanticCheck(errors, entorno);
            bool left = Left.SemanticCheck(errors, entorno);
            if (Right.Type(entorno) != Left.Type(entorno))
            {
                errors.Add(new Error(TypeOfError.Expected, "Solo se puede comparar entre dos expresiones del mismo tipo"));
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