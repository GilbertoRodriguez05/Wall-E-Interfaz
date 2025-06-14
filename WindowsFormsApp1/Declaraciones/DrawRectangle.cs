
using System;
using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApp1
{
    class DrawRectangle : AST
    {
        List<(int x, int y)> Directions = new List<(int x, int y)>
        {
            (1, 0), (0, 1), (-1, 0), (0, -1), (1, 1), (-1, 1), (1, -1), (-1, -1)
        };
        Expresions dirX;
        Expresions dirY;
        Expresions height;
        Expresions width;
        Expresions distance;
        Canvas canvas;
        public DrawRectangle(Expresions dirX, Expresions dirY, Expresions distance, Expresions width, Expresions height, Canvas canvas)
        {
            this.dirX = dirX;
            this.dirY = dirY;
            this.distance = distance;
            this.width = width;
            this.height = height;
            this.canvas = canvas;
        }
        public override void Execute()
        {
            dirX.Execute();
            dirY.Execute();
            distance.Execute();
            width.Execute();
            height.Execute();
            int x = Convert.ToInt32(dirX.value);
            int y = Convert.ToInt32(dirY.value);
            int d = Convert.ToInt32(distance.value);
            int w = Convert.ToInt32(width.value);
            int h = Convert.ToInt32(height.value);
            Rectangle(x, y, d, w, h, canvas);
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            dirX.Execute();
            dirY.Execute();
            distance.Execute();
            int x = Convert.ToInt32(dirX.value);
            int y = Convert.ToInt32(dirY.value);
            int d = Convert.ToInt32(distance.value);
            bool X = dirX.SemanticCheck(errors, entorno);
            bool Y = dirY.SemanticCheck(errors, entorno);
            bool W = width.SemanticCheck(errors, entorno);
            bool H = height.SemanticCheck(errors, entorno);
            bool D = distance.SemanticCheck(errors, entorno);
            if (dirX.Type(entorno) != ExpresionsTypes.Numero || dirY.Type(entorno) != ExpresionsTypes.Numero || width.Type(entorno) != ExpresionsTypes.Numero || height.Type(entorno) != ExpresionsTypes.Numero || distance.Type(entorno) != ExpresionsTypes.Numero)
            {
                errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo int"));
                return false;
            }
            else if (x * d + canvas.ActualX < 0 || x * d + canvas.ActualX >= canvas.Filas)
            {
                errors.Add(new Error(TypeOfError.Invalid, "El centro del rectangulo debe estar dentro de los limites del canvas"));
                return false;
            }
            else if (y * d + canvas.ActualY < 0 || y * d + canvas.ActualY >= canvas.Columnas)
            {
                errors.Add(new Error(TypeOfError.Invalid, "El centro del rectangulo debe estar dentro de los limites del canvas"));
                return false;
            }
            return X && Y && W && H && D;
        }
        public static void Rectangle(int dirX, int dirY, int distance, int width, int height, Canvas canvas)
        {
            // Calcular el vector de dirección
            double magnitude = Math.Sqrt(dirX * dirX + dirY * dirY);
            int stepX = 0;
            int stepY = 0;

            if (magnitude > 0)
            {
                stepX = (int)Math.Round((dirX * distance) / magnitude);
                stepY = (int)Math.Round((dirY * distance) / magnitude);
            }

            // Calcular el centro del rectángulo
            int centerX = canvas.ActualX + stepX;
            int centerY = canvas.ActualY + stepY;

            // Actualizar la posición de Wall-E al centro
            canvas.ActualX = centerX;
            canvas.ActualY = centerY;

            // Validar dimensiones del rectángulo
            if (width <= 0 || height <= 0) return;

            // Calcular las coordenadas del rectángulo
            int left = centerX - width / 2;
            int top = centerY - height / 2;
            int right = left + width - 1;
            int bottom = top + height - 1;

            // Dibujar las líneas del rectángulo
            // Línea superior
            for (int x = left; x <= right; x++)
            {
                DrawBrushAt(canvas, x, top);
            }

            // Línea inferior
            for (int x = left; x <= right; x++)
            {
                DrawBrushAt(canvas, x, bottom);
            }

            // Línea izquierda (excluyendo esquinas)
            for (int y = top + 1; y < bottom; y++)
            {
                DrawBrushAt(canvas, left, y);
            }

            // Línea derecha (excluyendo esquinas)
            for (int y = top + 1; y < bottom; y++)
            {
                DrawBrushAt(canvas, right, y);
            }
        }

        // Método auxiliar para dibujar con el tamaño del pincel
        private static void DrawBrushAt(Canvas canvas, int x, int y)
        {
            int halfBrush = canvas.BrushSize / 2;
            int startX = x - halfBrush;
            int endX = startX + canvas.BrushSize;
            int startY = y - halfBrush;
            int endY = startY + canvas.BrushSize;

            for (int i = startX; i < endX; i++)
            {
                for (int j = startY; j < endY; j++)
                {
                    if (i >= 0 && i < canvas.Width && j >= 0 && j < canvas.Height)
                    {
                        canvas.Board[j, i] = canvas.BrushColor;
                    }
                }
            }
        }
    }
}