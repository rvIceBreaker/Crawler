using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;

namespace Engine.UI
{
    class r_ProgressBar : r_baseUI
    {
        Point Position;
        Rectangle progressBar;

        int stepAmount;

        public r_ProgressBar(int x, int y, int width, int height) : base()
        {
            drawBounds = new Rectangle(x, y, width, height);
            drawSurf = new Surface(drawBounds);
        }

        public void Step()
        {
            progressBar.Width += stepAmount;
        }
    }
}
