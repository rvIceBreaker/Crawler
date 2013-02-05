using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;

namespace Engine.UI
{
    class r_baseUI
    {
        public Surface drawSurf;
        public Rectangle drawBounds;

        protected bool shouldDraw;
        protected bool shouldUpdate;
        protected bool wantsVis;
        protected bool isVis;
        protected bool hasFocus;

        public float drawAlpha;
        public int uiIndex;

        public virtual void Update()
        {

        }

        public Surface GetDrawSurface()
        {
            return drawSurf;
        }

        private void CheckVis()
        {
            switch (wantsVis)
            {
                case true:
                    if (isVis && wantsVis && !hasFocus) { shouldUpdate = false; shouldDraw = true; return; }

                    if (isVis && wantsVis) { shouldUpdate = true; shouldDraw = true; return; } //We should be faded in, just make sure we're drawing and updating

                    shouldDraw = true;
                    shouldUpdate = false;

                    drawAlpha += 0.1f; //Bump our alpha

                    if (drawAlpha >= 1) //Alpha is on a 0 - 1 basis. Not 0 - 255!!
                    {
                        drawAlpha = 1;
                        isVis = true;
                    }

                    break; //Should be it, should fade and take input when fully faded in

                case false:
                    if (!isVis && !wantsVis) { shouldUpdate = false; shouldDraw = false; return; } //Should be faded out, make sure we're not taking input or drawing

                    shouldDraw = true;
                    shouldUpdate = false;

                    drawAlpha -= 0.1f;

                    if (drawAlpha <= 0)
                    {
                        drawAlpha = 0;
                        isVis = false;
                    }

                    break;
            }
        }

        public void Show() //Fades in the panel
        {
            wantsVis = true;
        }

        public void Hide() //Fades out the panel
        {
            wantsVis = false;
        }

        public void ShowImmediate() //Shows the panel, with no fading
        {
            drawAlpha = 255;
            wantsVis = true;
            isVis = true;
        }

        public void HideImmediate() //Hides the panel, with no fading
        {
            drawAlpha = 0;
            wantsVis = false;
            isVis = false;
        }
    }
}
