using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace 랜덤_도형_생성_윈도우
{
    public partial class Form1 : Form
    {
        ArrayList ar;
        public Form1()
        {
            InitializeComponent();
            ar = new ArrayList();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Random Random = new Random();
            if (e.Button == MouseButtons.Left)
            {
                CMyData c = new CMyData();
                c.Shape = (int)Random.Next(2);
                c.Size = (int)Random.Next(50, 200);
                c.Point = new Point(e.X, e.Y);
                c.pColor = Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));
                c.bColor = Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));
                ar.Add(c);

            }
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach(CMyData c in ar)
            {
                SolidBrush brc = new SolidBrush(c.bColor);
                Pen p = new Pen(c.pColor);
                if (c.Shape == 1)
                {
                    Graphics G = e.Graphics;
                    //DrawEllipse(Pen_obj, X, Y, width, height)
                    G.DrawEllipse(p, c.Point.X, c.Point.Y, c.Size, c.Size);
                    //FillEllipse(Brush_obj, x, y, width, height)
                    G.FillEllipse(brc, c.Point.X, c.Point.Y, c.Size, c.Size);
                }
            }
        }
    }

    public class CMyData
    {
        public Color pColor { get; set; }
        public Color bColor { get; set; }
        public Point Point { get; set; }
        public int Size { get; set; }
        public int Shape { get; set; }
    }
}
