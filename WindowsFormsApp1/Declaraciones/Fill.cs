using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApp1
{
    class Fill : AST
    {
        Canvas canvas;
        public Fill(Canvas canvas)
        {
            this.canvas = canvas;
        }
        public override void Execute()
        {
            Filling(canvas);
        }
        public override bool SemanticCheck(List<Error> errors, Entorno entorno)
        {
            return true;
        }
        public void Filling(Canvas canvas)
        {
            // Obtener la posición actual y verificar que esté dentro de los límites
            int startX = canvas.ActualX;
            int startY = canvas.ActualY;

            if (startX < 0 || startX >= canvas.Width || startY < 0 || startY >= canvas.Height)
                return;

            // Obtener el color objetivo (el color en la posición actual)
            Colors targetColor = canvas.Board[startY, startX];

            // Si el color objetivo es igual al color de la brocha, no hay nada que hacer
            if (targetColor == canvas.BrushColor)
                return;

            // Crear una cola para BFS y una matriz para rastrear píxeles visitados
            Queue<(int x, int y)> queue = new Queue<(int, int)>();
            bool[,] visited = new bool[canvas.Height, canvas.Width];

            // Inicializar con la posición actual
            queue.Enqueue((startX, startY));
            visited[startY, startX] = true;

            // Direcciones para los 4 vecinos (arriba, abajo, izquierda, derecha)
            (int dx, int dy)[] directions = new[]
            {
                (0, -1), (0, 1), (-1, 0), (1, 0)
            };

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                // Cambiar el color del píxel actual
                canvas.Board[y, x] = canvas.BrushColor;

                // Explorar todos los vecinos
                foreach (var dir in directions)
                {
                    int newX = x + dir.dx;
                    int newY = y + dir.dy;

                    // Verificar si el vecino está dentro de los límites
                    if (newX < 0 || newX >= canvas.Width || newY < 0 || newY >= canvas.Height)
                        continue;

                    // Verificar si el píxel ya fue visitado
                    if (visited[newY, newX])
                        continue;

                    // Verificar si el píxel tiene el color objetivo
                    if (canvas.Board[newY, newX] == targetColor)
                    {
                        visited[newY, newX] = true;
                        queue.Enqueue((newX, newY));
                    }
                }
            }
        }
    }
}