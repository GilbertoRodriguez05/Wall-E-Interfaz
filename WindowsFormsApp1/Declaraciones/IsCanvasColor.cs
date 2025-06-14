using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Claims;

namespace WindowsFormsApp1
{
    class IsCanvasColor : Expresions
    {
        public List<string> DiferentsColor = new List<string> { "blue", "red", "green", "yellow", "black", "white", "orange", "purple", "transparent" };
        Expresions color;
        Expresions x;
        Expresions y;
        Canvas canvas;
        public override object value { get; set; }
        public IsCanvasColor(Expresions color, Expresions x, Expresions y, Canvas canvas)
        {
            this.color = color;
            this.x = x;
            this.y = y;
            this.canvas = canvas;
        }
        public override void Execute()
        {
            color.Execute();
            x.Execute();
            y.Execute();
            int X = Convert.ToInt32(x.value);
            int Y = Convert.ToInt32(y.value);
            string Color = (string)color.value;
            if (X >= canvas.Filas || X < 0 || Y >= canvas.Columnas || Y < 0) value = false;
            else if (canvas.Board[X + canvas.ActualX, Y + canvas.ActualY] == GetColor()) value = 1;
            else value = 0;
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            color.Execute();
            x.Execute();
            y.Execute();
            int X = Convert.ToInt32(x.value);
            int Y = Convert.ToInt32(y.value);
            string Color = (string)color.value;
            bool x1 = x.SemanticCheck(errors, entorno);
            bool y1 = y.SemanticCheck(errors, entorno);
            bool c = color.SemanticCheck(errors, entorno);
            if (x.Type(entorno) != ExpresionsTypes.Numero || y.Type(entorno) != ExpresionsTypes.Numero || color.Type(entorno) != ExpresionsTypes.Cadena)
            {
                errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo string o un tipo int"));
                return false;
            }
            else if (X + canvas.ActualX < 0 || X + canvas.ActualX >= canvas.Filas || Y + canvas.ActualY < 0 || Y + canvas.ActualY >= canvas.Columnas)
            {
                errors.Add(new Error(TypeOfError.Invalid, "La casilla tiene que estar dentro de las dimensiones del canvas"));
                return false;
            }
            else if (!DiferentsColor.Contains(Color.ToLower()))
            {
                errors.Add(new Error(TypeOfError.Invalid, "El color no es válido"));
                return false;
            }
            return x1 && y1 && c;
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            if (Convert.ToInt32(value) == 1 || Convert.ToInt32(value) == 0) return ExpresionsTypes.Numero;
            else return ExpresionsTypes.Bool;
        }
        public Colors GetColor()
        {
            color.Execute();
            string colorValue = (string)color.value;
            colorValue = colorValue.Substring(1, colorValue.Length - 2);
            switch (colorValue.ToLower())
            {
                case "red": return Colors.Red;
                case "blue": return Colors.Blue;
                case "green": return Colors.Green;
                case "yellow": return Colors.Yellow;
                case "black": return Colors.Black;
                case "white": return Colors.White;
                case "orange": return Colors.Orange;
                case "purple": return Colors.Purple;
                case "transparent": return Colors.Transparent;
                default: return Colors.White;
            }
        }
    }
}