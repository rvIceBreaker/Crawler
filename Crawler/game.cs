using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

using Engine;

namespace Project1
{
    class game
    {
        /*static NativeWindow wnd;

        static GraphicsContext gp;*/

        static void Main()
        {
            //Stopwatch frameWatch = new Stopwatch();
            //frameWatch.Start();

            //Engine Init
            Engine.Engine engine = new Engine.Engine();
            engine.Initialize();
            /*//Setup window
            wnd = new NativeWindow(800, 600, "Game_GL", GameWindowFlags.Default, GraphicsMode.Default, DisplayDevice.Default);
            wnd.Visible = true;
            //Graphics context
            gp = new GraphicsContext(GraphicsMode.Default, wnd.WindowInfo);
            gp.MakeCurrent(wnd.WindowInfo);
            (gp as IGraphicsContextInternal).LoadAll();

            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.5f, 0.5f, 0.5f, 0);

            frameWatch.Start();

            //Frame
            while (wnd.Exists)
            {
                wnd.ProcessEvents();

                if (!Engine.Engine.mIsAlive)
                    wnd.Close();

                //Engine.Engine.mStartTime
                double seconds = frameWatch.Elapsed.Seconds;

                //engine.Frame(seconds);

                //Can't swap buffers if our context is invalid (no window handle)
                if(wnd.Exists)
                    gp.SwapBuffers();

                frameWatch.Reset();
                frameWatch.Start();
            }*/
        }
    }
}
