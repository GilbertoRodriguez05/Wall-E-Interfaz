
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace WindowsFormsApp1
{
    internal class Colores : AST
    {
        public List<string> DiferentsColor = new List<string> { "blue", "red", "green", "yellow", "black", "white", "orange", "purple", "transparent" };
        public Expresions color;
        public Canvas canvas;
        public Colores(Expresions color, Canvas canvas)
        {
            this.color = color;
            this.canvas = canvas;
        }
        public override void Execute()
        {
            color.Execute();
            string colorValue = (string)color.value;
            colorValue = colorValue.Substring(1, colorValue.Length - 2);
            
            switch (colorValue.ToLower())
            {
                case "red": canvas.BrushColor = Colors.Red; break;
                case "blue": canvas.BrushColor = Colors.Blue; break;
                case "green": canvas.BrushColor = Colors.Green; break;
                case "yellow": canvas.BrushColor = Colors.Yellow; break;
                case "black": canvas.BrushColor = Colors.Black; break;
                case "white": canvas.BrushColor = Colors.White; break;
                case "orange": canvas.BrushColor = Colors.Orange; break;
                case "purple": canvas.BrushColor = Colors.Purple; break;
                case "transparent": canvas.BrushColor = Colors.Transparent; break;
                default: break;
            }
            ConvertCanvasColor(canvas.BrushColor);
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            color.Execute();
            string colorValue = (string)color.value;
            colorValue = colorValue.Substring(1, colorValue.Length - 2);
            if (color.Type(entorno) != ExpresionsTypes.Cadena)
            {
                errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo string"));
                return false;
            }
            if (!DiferentsColor.Contains(colorValue.ToLower()))
            {
                errors.Add(new Error(TypeOfError.Invalid, "Color no definido"));
                return false;
            }
            return true;
        }
        public Color ConvertCanvasColor(Colors canvasColor)
        {
            switch (canvasColor)
            {
                case Colors.Blue: return Color.Blue;
                case Colors.Red: return Color.Red;
                case Colors.Green: return Color.Green;
                case Colors.Yellow: return Color.Yellow;
                case Colors.Black: return Color.Black;
                case Colors.White: return Color.White;
                case Colors.Purple: return Color.Purple;
                case Colors.Orange: return Color.Orange;
                case Colors.Transparent: return Color.Transparent;
                default: return Color.White;
            }
        }
    }
}