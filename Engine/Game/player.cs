using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine.World;

namespace Engine.Game
{
    public class player : base_player
    {

        Dictionary<Vector2, base_item> inventory;

        public player(Vector2 pos) : base()
        {
            mPosition = pos;
            mAngles = new Angle2D(90);
        }

        public override void Think()
        {
            if (mIsLocalPlayer)
            {
                if ((Engine.mInput.mInputString & (int)INPUT_BITS.IN_ATTACK2) == (int)INPUT_BITS.IN_ATTACK2)
                {
                    if (!((Engine.mInput.mLastString & (int)INPUT_BITS.IN_ATTACK2) == (int)INPUT_BITS.IN_ATTACK2))
                    {
                        RemoveGeometry();
                    }
                }

                if ((Engine.mInput.mInputString & (int)INPUT_BITS.IN_ATTACK1) == (int)INPUT_BITS.IN_ATTACK1)
                {
                    if (!((Engine.mInput.mLastString & (int)INPUT_BITS.IN_ATTACK1) == (int)INPUT_BITS.IN_ATTACK1))
                    {
                        CreateGeometry();
                    }
                }
            }

            base.Think();
        }

        public void RemoveGeometry()
        {
            bool hit = false;
            int count = 0;

            Vector2 castPos = mPosition + mAngles * 0.2f; ;
            base_geom_entity geom = null;

            while (!hit)
            {
                if (Engine.mWorld.mGeometry.Exists(i => i.VectorInGeometry(castPos)))
                {
                    hit = true;
                    geom = Engine.mWorld.mGeometry.Find(i => i.VectorInGeometry(castPos));
                }
                else if (count > 100)
                {
                    hit = true;
                }

                count++;

                castPos += mAngles * 0.1f;
            }

            if (geom == null)
                return;

            try { Engine.mWorld.mGeometry.Remove(geom); }
            catch { }
        }

        public void CreateGeometry()
        {
            if(!Engine.mWorld.mGeometry.Exists(i => i.mPosition == mPosition))
            {
                Engine.mWorld.mGeometry.Add(new World_Geom(new Vector2((int)mPosition.x, (int)mPosition.y), "tex2.bmp"));
            }
        }
    }
}
