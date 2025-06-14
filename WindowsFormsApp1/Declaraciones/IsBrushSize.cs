using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class IsBrushSize : Expresions
    {
        Expresions size;
        Canvas canvas;
        public override object value { get; set; }
        public IsBrushSize(Expresions size, Canvas canvas)
        {
            this.size = size;
            this.canvas = canvas;
        }
        public override void Execute()
        {
            size.Execute();
            if (Convert.ToInt32(size.value) == canvas.BrushSize) value = 1;
            else value = 0;
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            if (size.Type(entorno) != ExpresionsTypes.Numero)
            {
                errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo int"));
                return false;
            }
            return true;
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            return ExpresionsTypes.Numero;
        }
    }
}