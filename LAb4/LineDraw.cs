using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAb4
{
    class LineDraw
    {
        public static Bitmap DdaLine(int xa, int ya, int xb, int yb, Bitmap bt, int maxWidth, int maxHeight, int brushSize, Color CustomColor)
        {
            var dx = xb - xa;
            var dy = yb - ya;
            brushSize--;

            var steps = Math.Abs(Math.Abs(dx) > Math.Abs(dy) ? dx : dy);

            var inc_x = (float)dx / (float)steps;
            var inc_y = (float)dy / (float)steps;


            float x = xa;
            float y = ya;

            for (int i = -brushSize; i <= brushSize; i++)
            {
                for (int j = -brushSize; j <= brushSize; j++)
                {
                    if (clump((int)x + i, (int)y + j, maxWidth, maxHeight))
                        bt.SetPixel((int)x + i, (int)y + j, CustomColor);

                }
            }


            for (var xd = 0; xd < steps; xd++)
            {
                x += inc_x;
                y += inc_y;

                for (int i = -brushSize; i <= brushSize; i++)
                {
                    for (int j = -brushSize; j <= brushSize; j++)
                    {

                        if (clump((int)x + i, (int)y + j, maxWidth, maxHeight))
                            bt.SetPixel((int)x + i, (int)y + j, CustomColor);
                    }
                }
            }



            return bt;
        }

        private static bool clump(int x, int y, int maxWidth, int maxHeight)
        {
            return x < maxWidth && x >= 0 && y < maxHeight && y >= 0;
        }
    }
}
