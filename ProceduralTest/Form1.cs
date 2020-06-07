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

namespace ProceduralTest
{
    public partial class Form1 : Form
    {
        PictureBox picture = new PictureBox();
        public Form1()
        {
            InitializeComponent();
            picture.Size = new Size(1000, 1000);

            Bitmap noise = new Bitmap(1000, 1000);
            Perlin2D perlin = new Perlin2D();
            for (float x = 0; x < 1000; x++)
            {
                for (float y = 0; y < 1000; y++)
                {
                    Vector2[] grads = new Vector2[12];
                    float n = perlin.OctaveNoise(new Vector2(x / 1000, y / 1000), 5, 2) * 255;

                    Vector2 unit = new Vector2(
                        (int)Math.Floor(x) & 255,
                        (int)Math.Floor(y) & 255);

                    noise.SetPixel((int)x, (int)y, Color.FromArgb((int)n, (int)n, (int)n));
                }
            }

            picture.Image = noise;

            this.Controls.Add(picture);
        }
    }
}
