using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine.Game;
using Engine.World;

using Box2DX;
using Box2DX.Collision;
using Box2DX.Dynamics;
using Box2DX.Common;

namespace Engine.World
{
    public class cWorld
    {
        public List<Base_Entity> mEntList;
        public List<base_geom_entity> mGeometry;

        public static Box2DX.Dynamics.World mPhysWorld;

        public List<Level> mLevels;

        public cWorld()
        {
            mGeometry = new List<base_geom_entity>();
            mLevels = new List<Level>();
            mEntList = new List<Base_Entity>();

            Initialize();
        }

        public void LoadMap()
        {

        }

        public void Initialize()
        {
            player localPlayer = new player(new Vector2(2.5f, 2.5f));
            localPlayer.SetAsLocalPlayer();
            mEntList.Add(localPlayer);
        }

        public void Frame()
        {
            foreach (base_geom_entity e in mGeometry)
            {
                e.Think();
            }

            foreach (Base_Entity e in mEntList)
            {
                e.Think();
            }
        }

        public void AddEntity(Game.Base_Entity entity)
        {
            mEntList.Add(entity);
        }


        public player GetLocalPlayer()
        {
            return (player)mEntList[0];
        }

        public bool AddGeometry(base_geom_entity g)
        {
            try
            {
                if (mGeometry.Exists(i => i.mPosition == g.mPosition))
                {
                    string msg = "Attempted to add geometry in filled vector!";

                    msg += " " + g.mName + "; " + g.mPosition.ToString();

                    Engine.C_MSG(msg, ENGINE_CONST.C_ERROR);
                    return false;
                }

                mGeometry.Add(g); return true;
            }
            catch { return false; }
        }
    }
}
