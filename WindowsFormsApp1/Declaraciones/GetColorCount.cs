using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Drawing;

namespace WindowsFormsApp1
{
    class GetColorCount : Expresions
    {
        public List<string> DiferentsColor = new List<string> { "blue", "red", "green", "yellow", "black", "white", "orange", "purple", "transparent" };
        Expresions x1;
        Expresions x2;
        Expresions y1;
        Expresions y2;
        Expresions color;
        Canvas canvas;
        int line;
        public override object value { get; set; }

        public GetColorCount(Expresions color, Expresions x1, Expresions y1, Expresions x2, Expresions y2, Canvas canvas, int line)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.color = color;
            this.canvas = canvas;
            this.line = line;
        }
        public override void Execute()
        {
            x1.Execute();
            x2.Execute();
            y1.Execute();
            y2.Execute();
            color.Execute();
            int X1 = Convert.ToInt32(x1.value);
            int X2 = Convert.ToInt32(x2.value);
            int Y1 = Convert.ToInt32(y1.value);
            int Y2 = Convert.ToInt32(y2.value);
            string colorValue = (string)color.value;
            if (X1 < 0 || X1 >= canvas.Filas || Y1 < 0 || Y1 >= canvas.Columnas) value = 0;
            else if (X2 < 0 || X2 >= canvas.Filas || Y2 < 0 || Y2 >= canvas.Columnas) value = 0;
            else value = CheckRectangle(X1, Y1, X2, Y2, GetColor(), canvas);
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            x1.Execute();
            x2.Execute();
            y1.Execute();
            y2.Execute();
            color.Execute();
            bool X1 = x1.SemanticCheck(errors, entorno);
            bool X2 = x2.SemanticCheck(errors, entorno);
            bool Y1 = y1.SemanticCheck(errors, entorno);
            bool Y2 = y2.SemanticCheck(errors, entorno);
            bool colorValue = color.SemanticCheck(errors, entorno);
            if (x1.Type(entorno) != ExpresionsTypes.Numero || y1.Type(entorno) != ExpresionsTypes.Numero || y1.Type(entorno) != ExpresionsTypes.Numero || y2.Type(entorno) != ExpresionsTypes.Numero)
            {
                errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo int", line));
                return false;
            }
            else if (color.Type(entorno) != ExpresionsTypes.Cadena)
            {
                errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo string", line));
                return false;
            }
            else if (!DiferentsColor.Contains((string)color.value))
            {
                errors.Add(new Error(TypeOfError.Invalid, "Color no valido", line));
                return false;
            }
            return X1 && X2 && Y1 && Y2 && colorValue;
        }
        public override ExpresionsTypes Type(Entorno entorno)
        {
            return ExpresionsTypes.Numero;
        }

        public int CheckRectangle(int x1, int y1, int x2, int y2, Colors colors, Canvas canvas)
        {
            int MinX = Math.Min(x1, x2);
            int MaxX = Math.Max(x1, x2);
            int MinY = Math.Min(y1, y2);
            int MaxY = Math.Max(y1, y2);
            int Count = 0;
            for (int i = MinX; i <= MaxX; i++)
            {
                for (int j = MinY; j <= MaxY; j++)
                {
                    if (canvas.Board[i, j] == colors) Count++;
                }
            }
            return Count;
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