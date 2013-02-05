using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SdlDotNet;
using SdlDotNet.Graphics;

namespace Engine.World
{
    class Dynamic_World_Geom : base_geom_entity
    {
        Vector2 DestPos;

        public Dynamic_World_Geom(Vector2 pos, string texture)
            : base()
        {
            mTexture = new Surface(new System.Drawing.Bitmap(texture));

            mPosition = pos;
            //DestPos = pos + new Vector2(2, 2);
        }

        public void Think()
        {
            //if(mPosition.y < DestPos.y)
            //    mPosition.y += 0.1;
            //if (mPosition.x < DestPos.x)
            //    mPosition.x += 0.1;

            //Console.WriteLine("Ran think on dynamic geometry entity");
        }
    }
}
