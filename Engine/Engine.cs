using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine.Components;
using Engine.World;
using Engine.Game;

using SdlDotNet.Core;
using Box2DX.Collision;

namespace Engine
{
    public class Engine
    {
        bool IsInitialized = false;

        public static vRenderer mRenderer;
        public static Input mInput;
        public static Network mNetwork;
        public static Script mScript;

        public static cWorld mWorld;

        public static bool mIsAlive;

        public static double mFrameTime = 0;
        public static double mCurrentTime = 0;

        public Engine()
        {
            mRenderer = new vRenderer();
            mInput = new Input();
            mNetwork = new Network();
            mScript = new Script();

            mWorld = new cWorld();

            bool other = false;

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    World_Geom geom = new World_Geom(new Vector2(x*64, y*64), "tex_bookshelf.bmp");

                    if (!other)
                    {
                        //geom = new World_Geom(new Vector2(x, y), "tex2.bmp");

                        e_pillar door = new e_pillar(new Vector2(x+0.5f, y+0.5f), "tex_3.bmp");
                        mWorld.AddEntity(door);
                    }

                    mWorld.AddGeometry(geom);
                    other = !other;
                }
                other = !other;
            }           
            
        }

        public void Initialize()
        {
            if (IsInitialized)
                return;
            
            IsInitialized = true;
            mIsAlive = true;

            mScript.InitScript();

            Events.Fps = 2000;
            Events.Tick += new EventHandler<TickEventArgs>(Tick);
            Events.Quit += new EventHandler<QuitEventArgs>(Quit);
            Events.Run();           
        }

        public static void C_MSG(string msg, ENGINE_CONST type)
        {
            string prefix = "";

            switch (type)
            {
                case ENGINE_CONST.C_ERROR:
                    prefix = "ERROR: ";
                    break;
                case ENGINE_CONST.C_WARNING:
                    prefix = "WARNING: ";
                    break;
                case ENGINE_CONST.C_MSG:
                    prefix = "";
                    break;
            }

            Console.WriteLine(prefix + msg);
        }

        public void Tick(object sender, TickEventArgs e)
        {
            mFrameTime = e.SecondsElapsed;
            mCurrentTime += mFrameTime;

            mInput.Frame();
            mWorld.Frame();
            mRenderer.DrawScene();
        }

        public void Quit(object sender, QuitEventArgs e)
        {
            mIsAlive = false;

            Events.QuitApplication();
        }
    }
}
