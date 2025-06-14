using System;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class Canvas
    {
            
            public Colors[,] Board;
            public int ActualX { get; set; }
            public int ActualY { get; set; }
            public int BrushSize = 1;
            public Colors BrushColor { get; set; }

            public int Width { get; private set; }
            public int Height { get; private set; }

            public Canvas() : this(25, 25) { }

            public Canvas(int width, int height)
            {
                Width = width;
                Height = height;
                Board = new Colors[height, width];
                ActualX = 0;
                ActualY = 0;
                BrushColor = Colors.Black; // Valor por defecto
                InicializarCanvas(Colors.White);
            }

            public int Filas => Height;
            public int Columnas => Width;

            public void InicializarCanvas(Colors color)
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        Board[i, j] = color;
                    }
                }
            }

            public void Resize(int newWidth, int newHeight)
            {
                var newBoard = new Colors[newHeight, newWidth];

                // Inicializar con blanco
                for (int i = 0; i < newHeight; i++)
                    for (int j = 0; j < newWidth; j++)
                        newBoard[i, j] = Colors.White;

                // Copiar contenido existente
                int copyWidth = Math.Min(Width, newWidth);
                int copyHeight = Math.Min(Height, newHeight);

                for (int i = 0; i < copyHeight; i++)
                    for (int j = 0; j < copyWidth; j++)
                        newBoard[i, j] = Board[i, j];

                Board = newBoard;
                Width = newWidth;
                Height = newHeight;

                // Ajustar posición si es necesario
                ActualX = Math.Min(ActualX, newWidth - 1);
                ActualY = Math.Min(ActualY, newHeight - 1);
            }
        }
    }

    public enum Colors
    {
        Blue, Red, Green, Yellow, Black, White, Purple, Orange, Transparent
    }


   