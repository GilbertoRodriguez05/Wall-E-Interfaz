
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    class DrawLine : AST
    {
        List<(int x, int y)> Directions = new List<(int x, int y)>
        {
            (1, 0), (0, 1), (-1, 0), (0, -1), (1, 1), (-1, 1), (1, -1), (-1, -1)
        };
        Expresions dirX;
        Expresions dirY;
        Expresions distance;
        Canvas canvas;
        int line;
        public DrawLine(Expresions dirX, Expresions dirY, Expresions distance, Canvas canvas, int line)
        {
            this.dirX = dirX;
            this.dirY = dirY;
            this.distance = distance;
            this.canvas = canvas;
            this.line = line;
        }
        public override void Execute()
        {
            dirX.Execute();
            dirY.Execute();
            distance.Execute();
            
            int x = Convert.ToInt32(dirX.value);
            int y = Convert.ToInt32(dirY.value);
            int dist = Convert.ToInt32(distance.value);
            canvas.Board[canvas.ActualY, canvas.ActualX] = canvas.BrushColor;
            if (canvas.BrushSize > 1)
            {
                FillDirection(canvas, Directions);
            }
            for (int i = 1; i < dist; i++)
            {
                canvas.Board[canvas.ActualY + y, canvas.ActualX + x] = canvas.BrushColor;
                canvas.ActualX += x;
                canvas.ActualY += y;
                if (canvas.BrushSize > 1)
                {
                    FillDirection(canvas, Directions);
                }
            }
            canvas.ActualX += x;
            canvas.ActualY += y;
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            dirX.Execute();
            dirY.Execute();
            distance.Execute();
            int x1 = Convert.ToInt32(dirX.value);
            int y1 = Convert.ToInt32(dirY.value);
            int dist1 = Convert.ToInt32(distance.value);
            bool x = dirX.SemanticCheck(errors, entorno);
            bool y = dirY.SemanticCheck(errors, entorno);
            bool dist = distance.SemanticCheck(errors, entorno);
            if (dirX.Type(entorno) != ExpresionsTypes.Numero || dirY.Type(entorno) != ExpresionsTypes.Numero || distance.Type(entorno) != ExpresionsTypes.Numero)
            {
                errors.Add(new Error(TypeOfError.Expected, "Se esperaba un tipo int", line));
                return false;
            }
            if (canvas.ActualX + x1 * dist1 < 0 || canvas.ActualX + x1 * dist1 > canvas.Filas)
            {
                errors.Add(new Error(TypeOfError.Invalid, "La distancia se sale de los limites del canvas", line));
                return false;
            }
            if (canvas.ActualY + y1 * dist1 < 0 || canvas.ActualY + y1 * dist1 > canvas.Columnas)
            {
                errors.Add(new Error(TypeOfError.Invalid, "La distancia se sale de los limites del canvas", line));
                return false;
            }
            if (!Directions.Contains((x1, y1)))
            {
                errors.Add(new Error(TypeOfError.Invalid, "Direccion no valida", line));
            }
            return x && y && dist;
        }
        public void FillDirection(Canvas canvas, List<(int x, int y)> Directions)
        {
            for (int k = 0; k < Directions.Count; k++)
            {
                int tempX = canvas.ActualY;
                int tempY = canvas.ActualX;
                for (int j = 0; j < canvas.BrushSize / 2; j++)
                {
                    if (tempX + Directions[k].x < 0 || tempX + Directions[k].x >= canvas.Filas) break;
                    if (tempY + Directions[k].y < 0 || tempY + Directions[k].y >= canvas.Columnas) break;
                    canvas.Board[tempX + Directions[k].x, tempY + Directions[k].y] = canvas.BrushColor;
                    tempX += Directions[k].x;
                    tempY += Directions[k].y;
                }
            }
        }
    }
}