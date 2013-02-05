using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine.Game;

namespace Engine.Components.Renderer
{
    //Local Camera Class
    public class Viewpoint
    {
        public Base_Entity mBoundEnt;
        bool mBoundToEnt = false;

        ENT_TYPE mBoundType;

        public Vector2 mPos;
        public Angle2D mAngles;

        public Viewpoint()
        {
            mBoundEnt = null;
            mBoundToEnt = false;

            mPos = new Vector2(2.5f, 2.5f);
            mAngles = new Angle2D(-1, 0);
        }

        public Viewpoint(Base_Entity boundEnt)
        {
            BindToEntity(boundEnt);

            mPos = boundEnt.mPosition;
            mAngles = boundEnt.mAngles;
        }
        
        public void BindToEntity(Base_Entity ent)
        {
            if (ent == null)
                return;
            
            mBoundEnt = ent;
            mBoundToEnt = true;

            if (ent.GetType() == typeof(player))
                mBoundType = ENT_TYPE.ENT_PLAYER;
            else
                mBoundType = ENT_TYPE.ENT_GENERIC;
        }

        public void UpdatePositions()
        {
            if (!mBoundToEnt) return;

            //if(mPos != mBoundEnt.mPosition)
                mPos = mBoundEnt.mPosition;

            //if(mAngles != mBoundEnt.mAngles)
                mAngles = mBoundEnt.mAngles;

            //double angDiff = mAngles.GetRadians() - mBoundEnt.mAngles.GetRadians();
        }
    }
}
