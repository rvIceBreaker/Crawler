using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SdlDotNet.Graphics;

using System.Drawing;
using Engine.World;

namespace Engine.Components.Renderer
{
    class GeomSlice
    {
        public base_geom_entity mGeom;
        public Surface mSurface;
        public Point pDrawStart;
        public int targetWidth;

        public GeomSlice(base_geom_entity geom, Surface srf, Point start)
        {
            mGeom = geom; mSurface = srf; pDrawStart = start;
        }
    }
}
