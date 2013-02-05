using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SdlDotNet;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using System.Drawing;
using System.Drawing.Drawing2D;
using Engine;
using Engine.Components.Renderer;
using SdlDotNet.Core;

namespace Engine.Components
{
    public class vRenderer
    {
        Surface screenBuffer;
        List<Surface> textureAtlas;

        public static int w = 800, h = 600;

        List<Viewport> mViewports;

        public bool mShouldDrawLocalPlayer = false;

        SdlDotNet.Graphics.Font debugFont = new SdlDotNet.Graphics.Font("LSANS.TTF", 12);

        Surface msurf;

        TimeSpan lastFrametime;
        DateTime lastDateTime;

        public vRenderer()
        {
            Video.WindowCaption = "Game";
            screenBuffer = Video.SetVideoMode(w, h);

            mViewports = new List<Viewport>();

            InitRenderer();
        }

        public int WindowWidth()
        {
            return w;
        }

        public int WindowHeight()
        {
            return h;
        }

        public void DrawScene()
        {
            RenderViewport(0);

            Game.player localply = Engine.mWorld.GetLocalPlayer();

            foreach (World.base_geom_entity i in Engine.mWorld.mGeometry)
            {
                screenBuffer.Blit( i.mTexture.CreateStretchedSurface(new Rectangle(0,0, 64, 64), new Rectangle(0, 0, 10, 10)), new Point((int)(i.mPosition.x * 10), (int)(i.mPosition.y * 10)) );
            }

            foreach (Game.base_phys_ent i in Engine.mWorld.mEntList)
            {
                screenBuffer.Draw(new Circle(new Point((int)(i.mPosition.x * 10), (int)(i.mPosition.y * 10)), (short)(i.mCollRadius * 10)), Color.Red);

                Point start, end, fovStart, fovEnd;

                if (i.mAngles != null)
                {
                    start = new Point((int)((i.mPosition.x * 10)), (int)((i.mPosition.y * 10)));
                    end = new Point((int)(start.X + (i.mAngles.x * 20)), (int)(start.Y + (i.mAngles.y * 20)));

                    screenBuffer.Draw(new Line(start, end), Color.Red);

                    Angle2D angle = i.mAngles.Right();

                    fovStart = new Point((int)((i.mPosition.x * 10)), (int)((i.mPosition.y * 10)));
                    fovEnd = new Point((int)(fovStart.X + (angle.x * 20)), (int)(fovStart.Y + (angle.y * 20)));

                    screenBuffer.Draw(new Line(fovStart, fovEnd), Color.Blue);
                }

            }

            screenBuffer.Update();

            lastDateTime = DateTime.Now;

            Video.WindowCaption = "Game - FPS: " + (1000 / (lastDateTime.Millisecond - lastFrametime.Milliseconds));

            lastFrametime = new TimeSpan(lastDateTime.Ticks);
        }

        #region LoadExternalTextures
        public void LoadExternalTextures()
        {
            string[] textureFiles = null;
            try { textureFiles = Directory.GetFiles(Environment.CurrentDirectory + "\\textures\\", "*.tex", SearchOption.AllDirectories); }
            catch { }

            if (textureFiles == null)
                return;

            textureAtlas = new List<Surface>();

            for (int i = 0; i < textureFiles.Length; i++)
            {
                textureAtlas.Add(new Surface(new Bitmap(textureFiles[i])));
            }
        }
        #endregion

        public void InitRenderer()
        {
            LoadExternalTextures();

            Viewport p1_vp = new Viewport(new Viewpoint(), new Rectangle(0, 0, w/2, h/2), "viewport1");
            p1_vp.mStretchY = true; p1_vp.mStretchX = true;

            mViewports.Add(p1_vp);
        }

        public void RenderViewport(int vIndex)
        {
            if (vIndex == -1)
                return;

            Viewport vPort = mViewports[vIndex];

            vPort.Render();

            if (vPort.mStretchX && vPort.mViewportBounds.Width < screenBuffer.Width)
            {
                vPort.mStretchBounds.Width = screenBuffer.Width;
            }

            if (vPort.mStretchY && vPort.mViewportBounds.Height < screenBuffer.Height)
            {
                vPort.mStretchBounds.Height = screenBuffer.Height;
            }

            if (vPort.mViewportBounds.Width != screenBuffer.Width || vPort.mViewportBounds.Height != screenBuffer.Height)
            {
                screenBuffer.Blit(vPort.mViewportSurface.CreateStretchedSurface(vPort.mViewportBounds, vPort.mStretchBounds), vPort.mViewportBounds);
            }
            else { screenBuffer.Blit(vPort.mViewportSurface, vPort.mStretchBounds); }
            
        }

        public Viewport FindViewportByName(string name)
        {
            return mViewports.Find( i => i.mName == name );
        }

        public Viewport FindViewportByIndex(int index)
        {
            try { return mViewports.ElementAt<Viewport>(index); }
            catch { Engine.C_MSG("Could not find viewport at index " + index, ENGINE_CONST.C_ERROR); return null; }
        }

        public int ViewportIndexByName(string name)
        {
            if (mViewports.IndexOf(FindViewportByName(name)) >= 0)
                return mViewports.IndexOf(FindViewportByName(name));
            else
                Engine.C_MSG("Viewport does not exist: '" + name + "'", ENGINE_CONST.C_ERROR); return -1;
        }
    }
}
