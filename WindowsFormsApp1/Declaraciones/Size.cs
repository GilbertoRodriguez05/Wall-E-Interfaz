
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class Sizes : AST
    {
        Expresions k;
        Canvas canvas;
        public int line;
        public Sizes(Expresions k, Canvas canvas, int line)
        {
            this.k = k;
            this.canvas = canvas;
            this.line = line;
        }
        public override void Execute()
        {
            k.Execute();
            if (Convert.ToInt32(k.value) % 2 != 0)
            {
                canvas.BrushSize = Convert.ToInt32(k.value);
            }
            else
            {
                canvas.BrushSize = Convert.ToInt32(k.value) - 1;
            }
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            k.Execute();
            if (Convert.ToInt32(k.value) < 1)
            {
                errors.Add(new Error(TypeOfError.Invalid, "El tamaño de la brocha debe ser mayor a 0", line));
            }
            else if (k.Type(entorno) != ExpresionsTypes.Numero)
            {
                errors.Add(new Error(TypeOfError.Expected, "Se esparaba un tipo int", line));
                return false;
            }
            return true;
        }
    }
}