using System;
using System.Collections.Generic;
using System.Text;

namespace SheepInDevelopment.Procedural.Perlin
{
    public class Perlin1D
    {
        public Perlin1D()
        {
            for (var i = 0; i < 256; i++)
            {
                p[256 + i] = p[i] = permutation[i];
            }
        }

        private float Fade(float t)
        {
            //I hate this but it's the most efficient way to compute this. (i think)
            //Basically this can be simplified down to 6t^5 - 15t^4 + 10t^3
            //This is essentially a "smooth" version of linear interpolation. Lerping has a very unnatural look to it as the line is completely straight
            //from start to finish. This is much smoother and thus, looks more natural.
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private float Lerp(float t, float a, float b)
        {
            return a + t * (b - a); //why does this work: research
        }

        private float Grad(int hash, float x)
        {
            //negative x if the 2^0 bit is 1
            //negative y if the 2^1 bit is 1
            return ((hash & 1) == 0 ? x : -x);
        }

        public float OctaveNoise(float x, int octaves, float persistence)
        {
            float average = 0;
            float freq = 1;
            float ampl = 1;
            float max = 0;
            for (int i = 0; i < octaves; i++)
            {
                average += Noise(x * freq) * ampl;

                max += ampl;

                ampl *= persistence;

                freq *= 2;
            }

            return average / max;
        }

        public float Noise(float x)
        {
            //Imagine that the noise is in a predefined graph. This graph contains a grid of size 1, such that all of the grid's points fall on integer values.
            //This code below determines the "unit" which contains the point we have been given.
            //Example: P(3.7, 29.35) would fall in unit vector U(3, 29)
            float unit = (int)Math.Floor(x);

            //Now that we know which unit the point is in, all we need is the relative position of the point to its unit. Basically, all we want is the decimals.
            //Example: P(3.7,29.35) would become P(0.7,0.35)
            x -= unit;

            float fade = Fade(x);

            int A, B; //So this hashing function is basically equivalent to:
            A = p[(int)unit]; //p[X]+Y
            B = p[(int)unit + 1]; //p[X+1] + Y
            //The whole point of the hash function is to supply a predictable pseudo-random number based on a coordinate input.

            float[] grads = new float[12];
            grads[0] = Grad(p[A], x);
            grads[1] = Grad(p[B], x + 1);

            return (Lerp(
                fade, //t
                grads[0],
                grads[1]
            ));
        }

        protected int[] p = new int[512];
        protected static int[] permutation = { 151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180};
    }
}
