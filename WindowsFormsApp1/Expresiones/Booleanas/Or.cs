
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class Or : BinaryExpresions
    {
        public override object value { get; set; }
        public Or(Expresions Right, Expresions Left)
        {
            this.Right = Right;
            this.Left = Left;
        }
        public override void Execute()
        {
            Right.Execute();
            Left.Execute();
            value = (bool)Right.value || (bool)Left.value;
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            bool right = Right.SemanticCheck(errors, entorno);
            bool left = Left.SemanticCheck(errors, entorno);
            if (Right.Type(entorno) != ExpresionsTypes.Bool || Left.Type(entorno) != ExpresionsTypes.Bool)
            {
                errors.Add(new Error(TypeOfError.Expected, "Solo se puede aplicar a expresiones booleanas"));
                return false;
            }
            return right && left;
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            if (Right.Type(entorno) != ExpresionsTypes.Bool || Left.Type(entorno) != ExpresionsTypes.Bool)
            {
                return ExpresionsTypes.Error;
            }
            return ExpresionsTypes.Bool;
        }
    }
}