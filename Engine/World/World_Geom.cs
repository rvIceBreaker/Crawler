using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SdlDotNet;
using SdlDotNet.Graphics;

namespace Engine.World
{
    public class World_Geom : base_geom_entity
    {
        public World_Geom(Vector2 pos, string texture)
        {
            mPosition = pos;
            mTexture = new Surface(new System.Drawing.Bitmap(texture));
            mName = texture;
        }

        //public World_Geom(Vector2 pos, string texture) : this(pos, texture) { }

        public override void Think()
        {

            base.Think();
        }

        public Vector2 GetTexDim()
        {
            return new Vector2(mTextureWidth, mTextureHeight);
        }
    }
}
