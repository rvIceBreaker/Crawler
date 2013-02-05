using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SdlDotNet;
using SdlDotNet.Graphics;

using System.Drawing;

namespace Engine.World
{
    public class base_geom_entity
    {
        public Vector2 mPosition;
        public AABB mBounds;

        public Surface mTexture;
        public string mName;
        protected bool mIsSolid, mShouldDraw = true;

        public bool m_bOffsetX, m_bOffsetY;

        public int mTextureWidth, mTextureHeight;
        public int mTextureOffsetX = 0, mTextureOffsetY = 0;

        public base_geom_entity()
        {
            mBounds = AABB.Zero();
        }

        public virtual void Think()
        {
            mTextureWidth = mTexture.Width;
            mTextureHeight = mTexture.Height;

            if(m_bOffsetX)
                mTextureOffsetX = (mTextureOffsetX + 1) % mTexture.Width;

            mBounds.x = mPosition.x;
            mBounds.y = mPosition.y;
            mBounds.width = 64;
            mBounds.height = 64;
        }

        public bool VectorInGeometry(Vector2 pos)
        {
            double boundx, boundy;

            boundx = mBounds.x + mBounds.width;
            boundy = mBounds.y + mBounds.height;

            if (pos.x > mBounds.x && pos.y > mBounds.y)
            {
                if (pos.x < boundx && pos.y < boundy)
                {
                    return true;
                }
            }

            return false;
        }

        public bool InBounds(Vector2 pos, float radius)
        {
            Vector2 circleDistance = new Vector2(0, 0);

            circleDistance.x = Math.Abs(pos.x - this.mPosition.x);
            circleDistance.y = Math.Abs(pos.y - this.mPosition.y);

            Vector2 rectDimensions = new Vector2(this.mPosition.x + 1.0f, this.mPosition.y + 1.0f);

            if (circleDistance.x > (rectDimensions.x / 2 + radius)) { return false; }
            if (circleDistance.y > (rectDimensions.y / 2 + radius)) { return false; }

            if (circleDistance.x <= (rectDimensions.x / 2)) { return true; }
            if (circleDistance.y <= (rectDimensions.y / 2)) { return true; }

            double cornerDistance_sq = Math.Pow((circleDistance.x - rectDimensions.x / 2), 2) + Math.Pow((circleDistance.y - rectDimensions.y / 2), 2);

            return (cornerDistance_sq <= Math.Pow(radius, 2));
        }
    }
}
