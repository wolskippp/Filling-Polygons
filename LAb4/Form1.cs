using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace LAb4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            _bt = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            pictureBox1.Image = _bt;
            exmapleBitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            allIndices = new List<int[]>();
            currPolygon = new List<Point>();
            AllVertices = new List<List<Point>>();

            using (Graphics g = Graphics.FromImage(_bt))
            {
                g.FillRectangle(Brushes.White, 0, 0, _bt.Width, _bt.Height);
            }

            using (Graphics g = Graphics.FromImage(exmapleBitmap))
            {
                g.FillRectangle(Brushes.White, 0, 0, _bt.Width, _bt.Height);
            }


            LoadPolygon(new[] {100, 200, 200, 100}, new[] {10, 10, 100, 10});

            LoadPolygon(new[] {100, 120, 160, 100}, new[] {300, 300, 350, 350});

            LoadPolygon(new[] {500, 560, 490, 450}, new[] {430, 430, 410, 510});

            LoadPolygon(new[] {230, 300, 310, 210}, new[] {45, 130, 100, 100});

        }

        private readonly Color _primaryColor = Color.Black;
        private readonly Color _secondaryColor = Color.Green;
        private Bitmap _bt;
        private readonly Bitmap exmapleBitmap;
        private bool lineFlag = false;
        private Point lastPoint;
        private Point firstPoint;
        private List<int[]> allIndices;

        private int[] currIndices;
        private List<Point> currPolygon;
        private List<List<Point>> AllVertices;



        private void LoadPolygon(int[] xCor, int[] yCor)
        {
            for (int i = 1; i < xCor.Length; i++)
            {
                LineDraw.DdaLine(xCor[i - 1], yCor[i - 1], xCor[i], yCor[i], exmapleBitmap, pictureBox1.Width,
                    pictureBox1.Height, 2,
                    _primaryColor);
            }

            LineDraw.DdaLine(xCor[xCor.Length - 1], yCor[yCor.Length - 1], xCor[0], yCor[0], exmapleBitmap,
                pictureBox1.Width, pictureBox1.Height, 2,
                _primaryColor);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && lineFlag == false)
            {
                _bt.SetPixel(e.X, e.Y, _primaryColor);
                lineFlag = true;
                firstPoint = new Point(e.X, e.Y);
                lastPoint = new Point(e.X, e.Y);
                currPolygon.Add(new Point(e.X, e.Y));


            }
            else if (e.Button == MouseButtons.Left)
            {
                _bt = LineDraw.DdaLine(lastPoint.X, lastPoint.Y, e.X, e.Y, _bt, pictureBox1.Width, pictureBox1.Height, 2,
                    _primaryColor);
                lastPoint = new Point(e.X, e.Y);
                currPolygon.Add(new Point(e.X, e.Y));
            }
            else if (e.Button == MouseButtons.Right)
            {



                if (radioButton1.Checked)
                    FloodFill4(e.X, e.Y, _bt, _bt.GetPixel(e.X, e.Y), _secondaryColor);
                else
                {
                    FloodFill8(e.X, e.Y, _bt, _bt.GetPixel(e.X, e.Y), _secondaryColor);
                }
            }

            pictureBox1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            _bt = LineDraw.DdaLine(lastPoint.X, lastPoint.Y, firstPoint.X, firstPoint.Y, _bt, pictureBox1.Width,
                pictureBox1.Height, 2,
                _primaryColor);
            lineFlag = false;

            List<Point> tempList = currPolygon.OrderBy(x => x.Y).ToList();
            int i = 0;
            currIndices = new int[tempList.Count];
            foreach (Point currPoint in tempList)
            {
                currIndices[i++] = currPolygon.FindIndex(a => a == currPoint);
            }


            allIndices.Add(currIndices);
            AllVertices.Add(new List<Point>(currPolygon));
            currPolygon.Clear();

            pictureBox1.Invalidate();
        }


        private void FloodFill8(int x, int y, Bitmap bt, Color old, Color newone)
        {
            var floodFillColor = new Stack<Point>();

          

            floodFillColor.Push(new Point(x, y));

            while (floodFillColor.Count != 0)
            {
                Point currPoint = floodFillColor.Pop();

                if (clump(currPoint.X, currPoint.Y, pictureBox1.Width, pictureBox1.Height))
                {
                    if (bt.GetPixel(currPoint.X, currPoint.Y) == old)
                    {
                        bt.SetPixel(currPoint.X, currPoint.Y, newone);


                     
                        floodFillColor.Push(new Point(currPoint.X - 1, currPoint.Y-1));
                        floodFillColor.Push(new Point(currPoint.X -1, currPoint.Y));
                        floodFillColor.Push(new Point(currPoint.X -1, currPoint.Y + 1));
                        floodFillColor.Push(new Point(currPoint.X , currPoint.Y -1));
                        floodFillColor.Push(new Point(currPoint.X , currPoint.Y +1));
                        floodFillColor.Push(new Point(currPoint.X +1, currPoint.Y - 1));
                        floodFillColor.Push(new Point(currPoint.X +1, currPoint.Y ));
                        floodFillColor.Push(new Point(currPoint.X + 1, currPoint.Y + 1));
                    }

                }
            }

            pictureBox1.Invalidate();
        }
        
        private void FloodFill4(int x, int y, Bitmap bt, Color old, Color newone)
        {
            var floodFillColor = new Stack<Point>();

           

            floodFillColor.Push(new Point(x,y));

            while (floodFillColor.Count != 0)
            {
                Point currPoint = floodFillColor.Pop();

                if (clump(currPoint.X, currPoint.Y, pictureBox1.Width, pictureBox1.Height))
                {
                    if (bt.GetPixel(currPoint.X, currPoint.Y) == old)
                    {
                        bt.SetPixel(currPoint.X,currPoint.Y, newone);

                        floodFillColor.Push(new Point(currPoint.X -1, currPoint.Y));
                        floodFillColor.Push(new Point(currPoint.X , currPoint.Y -1));
                        floodFillColor.Push(new Point(currPoint.X , currPoint.Y +1));
                        floodFillColor.Push(new Point(currPoint.X +1, currPoint.Y));
                    }
                  
                }
            }

            pictureBox1.Invalidate();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Checked = !radioButton1.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = !radioButton2.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(_bt))
            {
                g.FillRectangle(Brushes.White, 0, 0, _bt.Width, _bt.Height);
            }
            AllVertices.Clear();
            currPolygon.Clear();
            lineFlag = false;
            lastPoint = new Point(0, 0);
            firstPoint = new Point(0, 0);
            pictureBox1.Invalidate();
        }

        private static bool clump(int x, int y, int maxWidth, int maxHeight)
        {
            return x < maxWidth && x >= 0 && y < maxHeight && y >= 0;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _bt = (Bitmap) exmapleBitmap.Clone();

            pictureBox1.Image = _bt;
        }


        public  void  ScanlineAlgorithm( Bitmap bt, Color secondaryColor)
        {
            var polygon = new List<Point>(AllVertices[AllVertices.Count - 1]);
            AllVertices.RemoveAt(AllVertices.Count - 1);
            
            var yMin = int.MaxValue;
            var yMax = int.MinValue;
            var aet = new List<Edge>();
            foreach (var polygonPoint in polygon)
            {
                if (yMin > polygonPoint.Y)
                    yMin = polygonPoint.Y;
                if (yMax < polygonPoint.Y)
                    yMax = polygonPoint.Y;
            }
            var edgeTable = new List<List<Edge>>();
            for (int i = 0; i <= yMax - yMin + 1; ++i)
            {
                edgeTable.Add(null);
            }
            Point nextPoint = polygon[polygon.Count - 1];
            for (int i = 0; i < polygon.Count; ++i)
            {
                Point currentPoint = polygon[i];
                if (nextPoint.Y < currentPoint.Y)
                {
                    Point tempPoint = currentPoint;
                    currentPoint = nextPoint;
                    nextPoint = tempPoint;
                }
                if (edgeTable[currentPoint.Y - yMin] == null)
                    edgeTable[currentPoint.Y - yMin] = new List<Edge>();
                edgeTable[currentPoint.Y - yMin].Add(new Edge(currentPoint, nextPoint));
                nextPoint = new Point(polygon[i].X, polygon[i].Y);
            }
            for (var y = yMin; y <= yMax; ++y)
            {
                if (edgeTable[y - yMin] != null)
                    aet.AddRange(edgeTable[y - yMin]);
                aet.RemoveAll(x => x.YMax == y);
                aet.Sort((x1, x2) => x1.X.CompareTo(x2.X));
                for (int i = 0; i < aet.Count; i += 2)
                {
                    for (int px = (int)aet[i].X; px <= aet[i + 1].X; ++px)
                    {
                        bt.SetPixel(px, y, secondaryColor);
                    }
                }
                for (int i = 0; i < aet.Count; ++i)
                {
                    var newEdge = aet[i];
                    newEdge.X += newEdge.M;
                    aet[i] = newEdge;
                }
            }
            pictureBox1.Invalidate();
        }

     
        private class Edge
        {
            public readonly int YMax;
            public readonly float M;
            public float X;
            public Edge(Point beginningPoint, Point endingPoint)
            {
                if (endingPoint.Y < beginningPoint.Y)
                {
                    var temp = beginningPoint;
                    beginningPoint = endingPoint;
                    endingPoint = temp;
                }
                YMax = endingPoint.Y;
                X = beginningPoint.X;
                M = (beginningPoint.X - endingPoint.X) / (float)(beginningPoint.Y - endingPoint.Y);
            }
        }



        private void ScanLineClick(object sender, EventArgs e)
        {
            if (AllVertices.Count == 0)
                return;
            ScanlineAlgorithm(_bt, _secondaryColor);
        }
    }
}
