using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using System.Drawing;

namespace Engine.Game
{
    class e_pillar : base_phys_ent
    {
        public e_pillar(Vector2 pos, string tex)
            : base()
        {
            mPosition = pos;

            mTexFrames = new AnimationCollection();
            mTexFrames.Add(tex, new Size(64, 64));
            mTexture = new AnimatedSprite("pillar", mTexFrames);
        }

    }
}
