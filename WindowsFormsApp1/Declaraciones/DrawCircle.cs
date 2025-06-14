using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class DrawCircle : AST
    {
        List<(int x, int y)> Directions = new List<(int x, int y)>
        {
            (1, 0), (0, 1), (-1, 0), (0, -1), (1, 1), (-1, 1), (1, -1), (-1, -1)
        };
        Expresions dirX;
        Expresions dirY;
        Expresions radius;
        Canvas canvas;
        public DrawCircle(Expresions dirX, Expresions dirY, Expresions radius, Canvas canvas)
        {
            this.dirX = dirX;
            this.dirY = dirY;
            this.radius = radius;
            this.canvas = canvas;
        }
        public override void Execute()
        {
            dirX.Execute();
            dirY.Execute();
            radius.Execute();
            int x = Convert.ToInt32(dirX.value);
            int y = Convert.ToInt32(dirY.value);
            int r = Convert.ToInt32(radius.value);

            Circle(x, y, r, canvas);
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            dirX.Execute();
            dirY.Execute();
            radius.Execute();
            int x = Convert.ToInt32(dirX.value);
            int y = Convert.ToInt32(dirY.value);
            int r = Convert.ToInt32(radius.value);
            bool X = dirX.SemanticCheck(errors, entorno);
            bool Y = dirY.SemanticCheck(errors, entorno);
            bool R = radius.SemanticCheck(errors, entorno);
            if (dirX.Type(entorno) != ExpresionsTypes.Numero || dirY.Type(entorno) != ExpresionsTypes.Numero || radius.Type(entorno) != ExpresionsTypes.Numero)
            {
                errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo int"));
                return false;
            }
            else if (x * r + canvas.ActualX < 0 || x * r + canvas.ActualX >= canvas.Filas)
            {
                errors.Add(new Error(TypeOfError.Invalid, "El centro del circulo tiene que estar dentro de los limites del canvas"));
                return false;
            }
            else if (y * r + canvas.ActualY < 0 || y * r + canvas.ActualY >= canvas.Columnas)
            {
                errors.Add(new Error(TypeOfError.Invalid, "El centro del circulo tiene que estar dentro de los limites del canvas"));
                return false;
            }
            else if (!Directions.Contains((x, y)))
            {
                errors.Add(new Error(TypeOfError.Invalid, "Direccion no valida"));
            }
            return X && Y && R;
        }
        public static void Circle(int dirX, int dirY, int radius, Canvas canvas)
        {
            // Calcular el centro del círculo
            int centerX = canvas.ActualX + dirX;
            int centerY = canvas.ActualY + dirY;

            // Actualizar la posición actual del canvas al centro
            canvas.ActualX = centerX;
            canvas.ActualY = centerY;

            // Si el radio es negativo, no dibujar nada
            if (radius < 0) return;

            // Caso especial: radio 0 (dibujar un solo punto)
            if (radius == 0)
            {
                DrawBrushAt(canvas, centerX, centerY);
                return;
            }

            // Algoritmo del círculo (Midpoint circle algorithm)
            int x = 0;
            int y = radius;
            int d = 1 - radius; // Parámetro de decisión inicial

            // Dibujar los puntos iniciales
            DrawCirclePoints(canvas, centerX, centerY, x, y);

            while (x < y)
            {
                x++;
                if (d < 0)
                {
                    d += 2 * x + 1;
                }
                else
                {
                    y--;
                    d += 2 * (x - y) + 1;
                }
                DrawCirclePoints(canvas, centerX, centerY, x, y);
            }
        }

        private static void DrawCirclePoints(Canvas canvas, int centerX, int centerY, int x, int y)
        {
            // Dibujar los 8 puntos simétricos del círculo
            DrawBrushAt(canvas, centerX + x, centerY + y);
            DrawBrushAt(canvas, centerX - x, centerY + y);
            DrawBrushAt(canvas, centerX + x, centerY - y);
            DrawBrushAt(canvas, centerX - x, centerY - y);
            DrawBrushAt(canvas, centerX + y, centerY + x);
            DrawBrushAt(canvas, centerX - y, centerY + x);
            DrawBrushAt(canvas, centerX + y, centerY - x);
            DrawBrushAt(canvas, centerX - y, centerY - x);
        }

        private static void DrawBrushAt(Canvas canvas, int x, int y)
        {
            // Calcular el área del pincel (asumiendo tamaño impar)
            int halfBrush = canvas.BrushSize / 2;
            int startX = x - halfBrush;
            int endX = startX + canvas.BrushSize;
            int startY = y - halfBrush;
            int endY = startY + canvas.BrushSize;

            // Dibujar todos los píxeles del área del pincel
            for (int i = startX; i < endX; i++)
            {
                for (int j = startY; j < endY; j++)
                {
                    // Verificar que el píxel está dentro del tablero
                    if (i >= 0 && i < canvas.Width && j >= 0 && j < canvas.Height)
                    {
                        canvas.Board[j, i] = canvas.BrushColor;
                    }
                }
            }
        }
    }
}