using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class GetActualY : Expresions
    {
        Canvas canvas;
        public override object value { get; set; }

        public GetActualY(Canvas canvas)
        {
            this.canvas = canvas;
        }
        public override void Execute()
        {
            value = canvas.ActualY;
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