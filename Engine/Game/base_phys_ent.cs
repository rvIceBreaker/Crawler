using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Box2DX;
using Box2DX.Common;
using Box2DX.Collision;
using Engine.World;

namespace Engine.Game
{
    public class base_phys_ent : base_drawable
    {
        public float mCollRadius = 0.20f;
        Vector2 mVelocity;
        public Vector2 mLastPreVelocity;
        float mDrag = 0.5f;

        public base_phys_ent() : base()
        {
            mVelocity = Vector2.Zero;
        }

        public override void Think()
        {
            CheckCollide();

            //mPosition += mVelocity;

            mVelocity = mVelocity * mDrag;

            base.Think();
        }

        public player AsPlayer(base_phys_ent e)
        {
            return (player)e;
        }

        private void CheckCollide()
        {
            mLastPreVelocity = mVelocity;

            List<base_geom_entity> geom = Engine.mWorld.mGeometry.FindAll(i => i.mPosition.DistTo(mPosition) < 5);

            foreach (base_geom_entity g in geom)
            {
                if (g.mBounds.intersectsRadius(mPosition + new Vector2(mLastPreVelocity.x, 0), mCollRadius))
                {
                    mLastPreVelocity.x = 0;
                }

                if (g.mBounds.intersectsRadius(mPosition + new Vector2(0, mLastPreVelocity.y), mCollRadius))
                {
                    mLastPreVelocity.y = 0;
                }
            }

            mPosition += mLastPreVelocity;
        }

        public void SetVelocity(float x, float y) { SetVelocity(new Vector2(x, y)); }

        public void SetVelocity(Vector2 velocity)
        {
            mVelocity = velocity;
        }

        public void AddVelocity(float x, float y) { AddVelocity(new Vector2(x, y)); }

        public void AddVelocity(Vector2 velocity)
        {
            mVelocity += velocity;
        }
    }
}
