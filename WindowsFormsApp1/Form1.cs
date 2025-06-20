﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Canvas drawingCanvas {  get; set; }
        private int pixelSize = 12;

        public Form1()
        {
            InitializeComponent();
            InitializeCanvas();
            SetupPictureBox();
            TextEditor.VScroll += rtbCode_VScroll;
            TextEditor.TextChanged += rtbCode_TextChanged;
            TextEditor.Resize += rtbCode_Resize;
            pnlLineNumbers.Paint += pnlLineNumbers_Paint;
            TextEditor.Select();
        }

        private void InitializeCanvas()
        {
            int initialSize = 50;
            drawingCanvas = new Canvas(initialSize, initialSize);
            numericUpDown1.Value = initialSize;
            UpdatePictureBoxSize();
        }

        private void SetupPictureBox()
        {
            picCanvas.SizeMode = PictureBoxSizeMode.Normal;
            picCanvas.Paint += PicCanvas_Paint;
            UpdatePictureBoxSize();
        }

        private void ResizeButton_Click_1(object sender, EventArgs e)
        {
            if (numericUpDown1.Value <= 0) numericUpDown1.Value = 1;
            int newSize = (int)numericUpDown1.Value;

            drawingCanvas.Resize(newSize, newSize);
            UpdatePictureBoxSize();
            picCanvas.Invalidate();
        }

        private void DrawWallEPositionIndicator(Graphics g)
        {
            if (drawingCanvas == null) return;

            int x = drawingCanvas.ActualX;
            int y = drawingCanvas.ActualY;

            // Calcular posición en el PictureBox
            int screenX = x * pixelSize;
            int screenY = y * pixelSize;

            // Dibujar un indicador "X" roja
            using (Pen redPen = new Pen(Color.Red, 2))
            {
                // Dibujar X
                g.DrawLine(redPen, screenX + 2, screenY + 2,
                           screenX + pixelSize - 2, screenY + pixelSize - 2);
                g.DrawLine(redPen, screenX + pixelSize - 2, screenY + 2,
                           screenX + 2, screenY + pixelSize - 2);

                // Dibujar círculo alrededor
                g.DrawEllipse(redPen, screenX + 2, screenY + 2,
                              pixelSize - 4, pixelSize - 4);
            }

        }

        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (drawingCanvas == null) return;

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            for (int y = 0; y < drawingCanvas.Height; y++)
            {
                for (int x = 0; x < drawingCanvas.Width; x++)
                {
                    var rect = new Rectangle(
                        x * pixelSize,
                        y * pixelSize,
                        pixelSize,
                        pixelSize
                    );

                    Color color = ConvertCanvasColor(drawingCanvas.Board[y, x]);

                    using (var brush = new SolidBrush(color))
                    {
                        e.Graphics.FillRectangle(brush, rect);
                    }

                    e.Graphics.DrawRectangle(Pens.LightGray, rect);
                }
            }
            DrawWallEPositionIndicator(e.Graphics);
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

        private void UpdatePictureBoxSize()
        {
            if (drawingCanvas != null)
            {
                picCanvas.Size = new Size(
                    drawingCanvas.Width * pixelSize,
                    drawingCanvas.Height * pixelSize
                );
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Archivos pixel Walle (*.pw)|*.pw";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(openFileDialog1.FileName).Equals(".pw", StringComparison.OrdinalIgnoreCase))
                {
                    TextEditor.Text = File.ReadAllText(openFileDialog1.FileName);
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Archivos pixel Walle (*.pw)|*.pw";
            saveFileDialog1.DefaultExt = "pw";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                if (!Path.GetExtension(fileName).Equals(".pw", StringComparison.OrdinalIgnoreCase))
                {
                    fileName += ".pw";
                }
                File.WriteAllText(fileName, TextEditor.Text);
            }
        }

        public void executeButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = " ";
            string codigoFuente = TextEditor.Text;
            Entorno Context = new Entorno();
            List<Error> errors = new List<Error>();
            Scanner escaner = new Scanner(codigoFuente);
            List<Token> tokens = escaner.ScanTokens();
            Parser parser = new Parser(tokens, errors, Context, drawingCanvas);
            AST block = parser.Main();
            block.SemanticCheck(errors, Context);
            string s;
            if (errors.Count == 0)
            {
                block.Execute();
                PilaCompartida.Clear();
            }
            else
            {
                foreach (Error item in errors)
                {
                    s = Convert.ToString(item) + "\n";
                    richTextBox1.Text += s;
                }
            }
            UpdatePictureBoxSize();
            picCanvas.Invalidate();
        }

        private void pnlLineNumbers_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new SolidBrush(Color.DimGray))
            {
                int lineHeight = TextEditor.Font.Height;
                int firstIndex = TextEditor.GetCharIndexFromPosition(Point.Empty);
                int firstLine = TextEditor.GetLineFromCharIndex(firstIndex);
                int currentY = 0;

                for (int i = firstLine; i < TextEditor.Lines.Length; i++)
                {
                    if (currentY > pnlLineNumbers.Height) break;

                    e.Graphics.DrawString(
                        (i + 1).ToString(),
                        TextEditor.Font,
                        brush,
                        pnlLineNumbers.Width - TextRenderer.MeasureText((i + 1).ToString(), TextEditor.Font).Width - 5,
                        currentY
                    );

                    currentY += lineHeight;
                }
            }
        }

        private void rtbCode_VScroll(object sender, EventArgs e)
        {
            pnlLineNumbers.Invalidate();
        }

        private void rtbCode_TextChanged(object sender, EventArgs e)
        {
            pnlLineNumbers.Invalidate();
        }

        private void rtbCode_Resize(object sender, EventArgs e)
        {
            pnlLineNumbers.Invalidate();
        }

        private void pnlLineNumbers_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}