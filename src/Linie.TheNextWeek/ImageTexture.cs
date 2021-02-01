namespace Linie.TheNextWeek
{
    using Bitmap = System.Drawing.Bitmap;

    public class ImageTexture : ITexture
    {
        private object locker = new object();

        private readonly Color[,] pixels;

        private readonly int width;

        private readonly int height;

        public ImageTexture(string filename)
        {
            using(var bmp = new Bitmap(filename))
            {
                this.width = bmp.Width;
                this.height = bmp.Height;
                this.pixels = new Color[bmp.Width, bmp.Height];
                var colorScale = 1.0 / 255.0;
                for(var i = 0; i < this.width; i++)
                {
                    for(var j = 0; j < this.height; j++)
                    {
                        var pixel = bmp.GetPixel(i, j);
                        this.pixels[i, j] = new Color(
                            colorScale * pixel.R,
                            colorScale * pixel.G,
                            colorScale * pixel.B);
                    }
                }
            }
        }

        public Color GetColor(double u, double v, in Point3 point)
        {
            u = Utils.Clamp(u, 0, 1);
            v = 1.0 - Utils.Clamp(v, 0, 1);

            var i = (int)(u * this.width);
            var j = (int)(v * this.height);

            if (i >= this.width)
            {
                i = this.width - 1;
            }

            if (j >= this.height)
            {
                j = this.height - 1;
            }

            return this.pixels[i, j];
        }
    }
}