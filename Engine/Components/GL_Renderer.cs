using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Components
{
    class GLTextureAtlas
    {

    }

    class GL_Renderer
    {
        GLTextureAtlas textureAtlas;
        public static int w = 800, h = 600;

        //List<Viewport> mViewports;

        public GL_Renderer()
        {
            textureAtlas = new GLTextureAtlas();


        }

    }
}
