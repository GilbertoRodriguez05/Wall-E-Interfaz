
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;

namespace WindowsFormsApp1
{
    class Spawn : AST
    {
        public Expresions initialX;
        public Expresions initialY;
        public Canvas canvas;
        public int line;
        public Spawn(Expresions x, Expresions y, Canvas canvas, int line)
        {
            initialX = x;
            initialY = y;
            this.canvas = canvas;
            this.line = line;
        }
        public override void Execute()
        {
            canvas.InicializarCanvas(Colors.Transparent);
            initialX.Execute();
            initialY.Execute();
            canvas.ActualX = Convert.ToInt32(initialX.value);
            canvas.ActualY = Convert.ToInt32(initialY.value);
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            bool x = initialX.SemanticCheck(errors, entorno);
            bool y = initialY.SemanticCheck(errors, entorno);
            if (initialX.Type(entorno) != ExpresionsTypes.Numero || initialY.Type(entorno) != ExpresionsTypes.Numero)
            {
                errors.Add(new Error(TypeOfError.Expected, "Se esperaban parametros de tipo entero", line));
                return false;
            }
            if (Convert.ToInt32(initialX.value) < 0 || Convert.ToInt32(initialX.value) >= canvas.Filas)
            {
                errors.Add(new Error(TypeOfError.Invalid, "La Posicion inicial debe estar dentro del rango del canvas", line));
                return false;
            }
            if (Convert.ToInt32(initialY.value) < 0 || Convert.ToInt32(initialY.value) >= canvas.Filas)
            {
                errors.Add(new Error(TypeOfError.Invalid, "La Posicion inicial debe estar dentro del rango del canvas", line));
                return false;
            }
            return x && y;

        }
    }
}