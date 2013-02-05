using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using SdlDotNet;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Graphics.Primitives;

using Engine.Game;

namespace Engine.Components.Renderer
{
    public class Viewport
    {
        Viewpoint mViewpoint;

        public Rectangle mViewportBounds;
        public Rectangle mStretchBounds;

        bool mRender3d = true, mRender2d = true, mRenderSprites = true, mUseOld3d = true;
        bool mDebugGeomZBuffer = false, mDebugSpriteZBuffer = false;

        public Surface mViewportSurface;

        public string mName;

        public Angle2D Plane = new Angle2D(0, 0.5f);

        double[] mGeometryZBuffer, mSpriteZBuffer;

        SdlDotNet.Graphics.Font debugFont = new SdlDotNet.Graphics.Font("LSANS.TTF", 12);

        public bool mStretchX, mStretchY;
        
        #region Constructors
        public Viewport(Viewpoint vp, int x, int y, int width, int height, string name) : this(vp, new Rectangle(x, y, width, height), name) { }
        public Viewport(Viewpoint vp, Rectangle screenSpace, string name)
        {
            mViewpoint = vp;

            mViewportBounds = screenSpace;

            mName = name;

            mViewportSurface = new Surface(mViewportBounds);

            Plane.y = (float)((double)(mViewportBounds.Width / mViewportBounds.Height) / 2);

            mGeometryZBuffer = new double[mViewportBounds.Width];
            mSpriteZBuffer = new double[mViewportBounds.Width];
        }
        #endregion

        public void Render()
        {
            mViewpoint.UpdatePositions();

            mViewportSurface.Fill(Color.Gray);

            if (mRender3d)
                RayCast();//RayCast();

            if (mRenderSprites)
                SpriteCast();
            
            if(mRender2d)
                Draw2d();            

            mViewportSurface.Blit(debugFont.Render("Stamina: " + (this.GetViewpoint().mBoundEnt as base_player).stamina.ToString(), Color.White), new Point(0, mViewportBounds.Height - 15));
        }

        /// <summary>
        /// Old raycast code; Demanding, non-optimized
        /// </summary>
        public void RayCast()
        {
            World.base_geom_entity lastGeom = null;
            int texX = 0;
            int lastTexX = 0;

            int sliceWidth = 1;

            Point
                pDrawStart = Point.Empty,// = new Point(x, drawStart),
                pDrawEnd = Point.Empty;// = new Point(x, drawEnd);

            for (int x = 0; x < mViewportBounds.Width; x += sliceWidth)
            {
                World.base_geom_entity geom = null;
            
                //calculate ray position and direction 
                double cameraX = 2 * x / (double)mViewportBounds.Width - 1; //x-coordinate in camera space
                double rayPosX = mViewpoint.mPos.x;
                double rayPosY = mViewpoint.mPos.y;
                double rayDirX = mViewpoint.mAngles.x + Plane.x * cameraX;
                double rayDirY = mViewpoint.mAngles.y + Plane.y * cameraX;

                //which box of the map we're in  
                double mapX = (int)rayPosX;
                double mapY = (int)rayPosY;

                //length of ray from current position to next x or y-side
                double sideDistX;
                double sideDistY;

                //length of ray from one x or y-side to next x or y-side
                double deltaDistX = Math.Sqrt(1 + (rayDirY * rayDirY) / (rayDirX * rayDirX));
                double deltaDistY = Math.Sqrt(1 + (rayDirX * rayDirX) / (rayDirY * rayDirY));
                double perpWallDist;
                int uWallDist;

                //what direction to step in x or y-direction (either +1 or -1)
                int stepX;
                int stepY;

                bool hit = false; //was there a wall hit?
                bool side = false; //was a NS or a EW wall hit?

                //calculate step and initial sideDist
                if (rayDirX < 0)
                {
                    stepX = -1;
                    sideDistX = (rayPosX - mapX) * deltaDistX;
                }
                else
                {
                    stepX = 1;
                    sideDistX = (mapX + 1.0 - rayPosX) * deltaDistX;
                }

                if (rayDirY < 0)
                {
                    stepY = -1;
                    sideDistY = (rayPosY - mapY) * deltaDistY;
                }
                else
                {
                    stepY = 1;
                    sideDistY = (mapY + 1.0 - rayPosY) * deltaDistY;
                }

                Vector2 mapVec = new Vector2((float)mapX, (float)mapY);
                int mPassCount = 0;

                //perform DDA
                while (!hit)
                {
                    //jump to next map square, OR in x-direction, OR in y-direction
                    if (sideDistX < sideDistY)
                    {
                        sideDistX += deltaDistX;
                        mapX += stepX;
                        side = false;
                    }
                    else if(sideDistX > sideDistY)
                    {
                        sideDistY += deltaDistY;
                        mapY += stepY;
                        side = true;
                    }                   

                    if (mPassCount > 50)
                        hit = true;
                    
                    mPassCount++;

                    mapVec = new Vector2((float)mapX, (float)mapY);

                    if (Engine.mWorld.mGeometry.Exists(i => i.mPosition == mapVec))
                    {
                        if ((geom = Engine.mWorld.mGeometry.Find(i => i.mPosition == mapVec.Floored())) != null)
                        {
                            hit = true;
                        }
                    }                  
                }

                if (geom != null)
                {

                    //Calculate distance projected on camera direction (oblique distance will give fisheye effect!)
                    if (!side)
                        perpWallDist = Math.Abs((mapX - rayPosX + (1 - stepX) / 2.0) / rayDirX);
                    else
                        perpWallDist = Math.Abs((mapY - rayPosY + (1 - stepY) / 2.0) / rayDirY);

                    //Near-Z cut
                    if (perpWallDist <= 0.10)
                        return;

                    double uX = mapX - rayPosX;
                    double uY = mapY - rayPosY;

                    uWallDist = (int)((uX * uX) + (uY * uY));

                    mGeometryZBuffer[x] = perpWallDist; //(distX * distX) + (distY * distY);

                    //Calculate height of line to draw on screen
                    int lineHeight = (int)Math.Abs(mViewportBounds.Height / perpWallDist);

                    //calculate lowest and highest pixel to fill in current stripe
                    int drawStart = -lineHeight / 2 + mViewportBounds.Height / 2, drawEnd = lineHeight / 2 + mViewportBounds.Height / 2;

                    //calculate value of wallX
                    double wallX; //where exactly the wall was hit
                    if (side)
                        wallX = rayPosX + ((mapY - rayPosY + (1 - stepY) / 2.0) / rayDirY) * rayDirX;
                    else
                        wallX = rayPosY + ((mapX - rayPosX + (1 - stepX) / 2.0) / rayDirX) * rayDirY;
                    wallX -= Math.Floor(wallX);

                    //x coordinate on the texture
                    texX = (int)geom.mTexture.Width - ((int)(wallX * (double)geom.mTexture.Width)) - 1;

                    //Fixes horizontal flipping on negative sides
                    if (!side && rayDirX > 0 || side && rayDirY < 0)
                    {
                        texX = (int)geom.mTexture.Width - texX - 1;
                    }

                    pDrawStart = new Point(x, drawStart);
                    pDrawEnd = new Point(x, drawEnd);

                    //TODO: Get rid of these surface calls
                    //TODO: Move to buffer system
                    //UPDATE: This now takes into account the geometry's texture offset [(texX + geom.mTextureOffsetX) % geom.mTexture.Width]
                    Surface geomSlice = geom.mTexture.CreateStretchedSurface(new Rectangle((texX + geom.mTextureOffsetX) % geom.mTexture.Width, 0, 1, geom.mTexture.Height), new Rectangle(x, 0, 1, lineHeight));
                    Surface darkener = new Surface(geomSlice.Width, geomSlice.Height);

                    darkener.Alpha = (byte)math.Clamp(Math.Pow(perpWallDist, 5), 200, 0);
                    darkener.AlphaBlending = true;

                    if (darkener.Alpha > 0)
                        geomSlice.Blit(darkener);

                    mViewportSurface.Blit(geomSlice, pDrawStart);
                    sliceWidth = geomSlice.Width;
                }
            }
        }

        /// <summary>
        /// New raycast code; Optimized
        /// </summary>
        public void RayCast2()
        {
            //Get viewport's FOV, determine fov's start and end angles relative to the viewpoint's angles
            //Interpolate between the start and end angles, casting a ray each interval
            int fovDegrees = 75;

            Angle2D minAng = this.mViewpoint.mAngles; minAng.Add((float)-(fovDegrees / 2));
            Angle2D maxAng = this.mViewpoint.mAngles; maxAng.Add((float)(fovDegrees / 2));

            //Column Buffer loop
            for (int x = 0; x < mViewportBounds.Width; x++)
            {
                float scalar = x / mViewportBounds.Width;
                Angle2D ang = new Angle2D(0) { x = minAng.x + (minAng.x - maxAng.x) * scalar, y = minAng.y + (minAng.y - maxAng.y) * scalar };

                //bool geomDone = false;

                Vector2 startPos = mViewpoint.mPos;
                Vector2 endPos = startPos + ang.ToVector2() * 100;

                Vector2 curPos = startPos;

                World.base_geom_entity geom = null;

                for (float sc = 0.0f; sc < 1; sc += 0.01f)
                {
                    curPos = startPos + ((startPos - endPos) * sc);

                    geom = Engine.mWorld.mGeometry.Find(i => i.mBounds.intersectsRadius(curPos, 0.01f));

                    if (geom != null)
                    {
                        int dist = (int)(startPos.DistTo(curPos));

                        if (dist <= 0)
                            break;

                        int drawHeight = (int)(this.mViewportBounds.Height / dist);

                        Point pDrawStart = new Point(x, (this.mViewportBounds.Height / 2) - (drawHeight / 2));
                        Point pDrawEnd = new Point(x, (this.mViewportBounds.Height / 2) + (drawHeight / 2));

                        mViewportSurface.Draw(new Line(pDrawStart, pDrawEnd), Color.Red);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        private void RangeSetZBuffer(int start, int end, double value)
        {
            for (int i = start; i < end; i++)
            {
                mGeometryZBuffer[i] = value;
            }
        }

        private void SetZBuffer(int x, double value)
        {
            mGeometryZBuffer[x] = value;
        }

        public void DrawSlice(Point scrPos, Rectangle Src, Rectangle Dest, double Distance, World.base_geom_entity ent)
        {
            if (Distance > 0.25)
            {
                Surface slice = ent.mTexture.CreateStretchedSurface(Src, Dest);
                mViewportSurface.Blit(slice,scrPos);
            }
        }

        public void SpriteCast()
        {
            double dirX = mViewpoint.mAngles.x, dirY = mViewpoint.mAngles.y;
            double posX = mViewpoint.mPos.x, posY = mViewpoint.mPos.y;

            Dictionary<Game.base_drawable, double> nearbyEnts = new Dictionary<global::Engine.Game.base_drawable,double>();

            foreach (Game.base_drawable e in Engine.mWorld.mEntList)
            {
                if (nearbyEnts.ContainsKey(e))
                    return;

                if (!e.isVisible)
                    return;

                double dist;
                double xsqr = mViewpoint.mPos.x - e.mPosition.x;
                double ysqr = mViewpoint.mPos.y - e.mPosition.y;

                dist = ((xsqr * xsqr) + (ysqr * ysqr));

                if (e == Engine.mWorld.GetLocalPlayer()) { if (Engine.mRenderer.mShouldDrawLocalPlayer) { nearbyEnts.Add(e, dist); } }
                else
                {
                    if(dist > 0.5)
                        nearbyEnts.Add(e, dist);
                }
            }

            //Loop through each sprite, furthest to closest; compute their screen-coordinates; draw them if they appear on the screen
            foreach (KeyValuePair<Game.base_drawable, double> e in nearbyEnts.OrderByDescending(key => key.Value))
            {               
                double spriteX = e.Key.mPosition.x - posX;
                double spriteY = e.Key.mPosition.y - posY;
                              
                double invDet = 1.0 / (Plane.x * dirY - dirX * Plane.y); //required for correct matrix multiplication
          
                double transformX = invDet * (dirY * spriteX - dirX * spriteY);
                double transformY = invDet * (-Plane.y * spriteX + Plane.x * spriteY); //this is actually the depth inside the screen, that what Z is in 3D                           
                int spriteScreenX = (int)((mViewportBounds.Width / 2) * (1 + transformX / transformY));

                //int spriteHeight = Math.Abs((int)(mViewportBounds.Height / transformY)); //using "transformY" instead of the real distance prevents fisheye
                
                //int drawEndY = spriteHeight / 2 + mViewportBounds.Height / 2;
                //if(drawEndY >= mViewportBounds.Height) drawEndY = mViewportBounds.Height - 1;

                int spriteWidth = Math.Abs((int)(mViewportBounds.Height / (transformY)) + 1);
                int drawStartX = -spriteWidth / 2 + spriteScreenX;
                int drawEndX = spriteWidth / 2 + spriteScreenX;
                //if (drawEndX >= mViewportBounds.Width) drawEndX = mViewportBounds.Width - 1;
                
                int drawStartY = (mViewportBounds.Height / 2) - (spriteWidth / 2);
                //if(drawStartY < 0) drawStartY = 0;

                if (transformY > 0 && ((drawStartX + spriteWidth) > 0) && (drawStartX < mViewportBounds.Width))
                {
                    Surface sprite = e.Key.mTexture.Surface.CreateStretchedSurface( new Rectangle(0,0,64,64), new Rectangle(drawStartX, drawStartY, spriteWidth, spriteWidth) );
                    sprite.TransparentColor = Color.Magenta;
                    sprite.Transparent = true;

                    for (int x = 0; x < sprite.Width; x++)
                    {
                        if (((drawStartX + x) < 0) || ((drawStartX + x) > (mViewportBounds.Width - 1)))
                            continue;
                        else
                        {
                            if (x > mViewportBounds.Width - 1)
                                continue;

                            //Console.WriteLine(mGeometryZBuffer[x] + " " + transformY);

                            if (mGeometryZBuffer[drawStartX + x] > transformY)
                            {
                                mViewportSurface.Blit(sprite, new Rectangle(drawStartX + x, drawStartY, 1, sprite.Height), new Rectangle(x, 0, 1, sprite.Height));
                            }
                        }
                    }
                }
            }

        }

        //Called after raycast
        public void Draw2d()
        {
            mViewportSurface.Blit(debugFont.Render("TEST UI", Color.White, true), new Point(mViewportSurface.Width / 2, mViewportSurface.Height / 2));
        }

        public Viewpoint GetViewpoint()
        {
            return mViewpoint;
        }
    }
}
