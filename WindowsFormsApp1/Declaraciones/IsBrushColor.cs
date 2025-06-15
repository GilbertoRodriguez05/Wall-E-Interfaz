using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace WindowsFormsApp1
{
    class IsBrushColor : Expresions
    {
        public List<string> DiferentsColor = new List<string> { "blue", "red", "green", "yellow", "black", "white", "orange", "purple", "transparent" };
        Expresions color;
        Canvas canvas;
        int line;
        public override object value { get; set; }
        public IsBrushColor(Expresions color, Canvas canvas, int line)
        {
            this.color = color;
            this.canvas = canvas;
            this.line = line;
        }
        public override void Execute()
        {
            color.Execute();
            string colorValue = (string)color.value;

            if (GetColor() == canvas.BrushColor) value = 1;
            else value = 0;
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            color.Execute();
            string colorValue = (string)color.value;
            colorValue = colorValue.Substring(1, colorValue.Length - 2);
            if (color.Type(entorno) != ExpresionsTypes.Cadena)
            {
                errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo string", line));
                return false;
            }
            else if (!DiferentsColor.Contains(colorValue.ToLower()))
            {
                errors.Add(new Error(TypeOfError.Invalid, "Color no definido", line));
                return false;
            }
            return true;
        }

        public override ExpresionsTypes Type(Entorno entorno)
        {
            return ExpresionsTypes.Numero;
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