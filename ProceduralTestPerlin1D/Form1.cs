using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using SheepInDevelopment.Procedural.Perlin;
using System.Diagnostics;

namespace ProceduralTestPerlin1D
{
    public partial class Form1 : Form
    {
        private PictureBox picture = new PictureBox();
        private List<Point> coordinates = new List<Point>();

        private int resolution = 100;
        private int xStretch = 2;
        private int yOffset = Screen.PrimaryScreen.WorkingArea.Height / 2;
        public Form1()
        {
            InitializeComponent();
            picture.Dock = DockStyle.Fill;
            picture.BackColor = Color.White;

            Perlin1D perlin = new Perlin1D();

            for (float x = 0; x < 20; x += (1f/resolution))
            {
                float n = perlin.OctaveNoise(x, 5, 2);

                coordinates.Add(new Point((int)(x * resolution * xStretch), (int)(n * resolution) + yOffset));
            }

            picture.Paint += new PaintEventHandler(this.picture_Paint);

            this.Controls.Add(picture);
        }

        private void picture_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Pen pen = new Pen(Color.Blue);
            Pen graphPen = new Pen(Color.DimGray);

            Debug.WriteLine(coordinates.Count);

            int topY = Screen.PrimaryScreen.WorkingArea.Height;
            int midY = topY / 2;
            //lines and stuff
            g.DrawLine(graphPen, 0, midY, Screen.PrimaryScreen.WorkingArea.Width, midY);
            for(int x = 0; x < Screen.PrimaryScreen.WorkingArea.Width; x+=1*resolution*xStretch)
            {
                g.DrawLine(graphPen, x, 0, x, topY);
            }

            for(var i = 0; i < coordinates.Count; i++)
            {
                if (i + 1 >= coordinates.Count) break;
                //Debug.WriteLine(coordinates[i].X + " " + coordinates[i].Y);
                g.DrawLine(pen, coordinates[i], coordinates[i + 1]);
            }
        }

    }
}
