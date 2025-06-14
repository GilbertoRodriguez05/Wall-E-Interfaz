using System.Collections.Generic;
using System.Security.Cryptography;

namespace WindowsFormsApp1
{
    class GetCanvasSize : Expresions
    {
        Canvas canvas;
        public override object value { get; set; }
        public GetCanvasSize(Canvas canvas)
        {
            this.canvas = canvas;
        }
        public override void Execute()
        {
            value = canvas.Filas;
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