using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class GetActualX : Expresions
    {
        Canvas canvas;
        public override object value { get; set; }

        public GetActualX(Canvas canvas)
        {
            this.canvas = canvas;
        }
        public override void Execute()
        {
            value = canvas.ActualX;
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            return true;
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            return ExpresionsTypes.Numero;
        }
    }
}