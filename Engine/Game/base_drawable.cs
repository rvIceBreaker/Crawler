using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SdlDotNet;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;

namespace Engine.Game
{
    public class base_drawable : Base_Entity
    {
        public bool isVisible = true;

        public AnimatedSprite mTexture;
        public AnimationCollection mTexFrames;

        public base_drawable()
            : base()
        {           
        }

        public override void Think()
        {
            if(mTexture != null)
                mTexture.Update((SdlDotNet.Core.TickEventArgs)null);

            base.Think();
        }

        public override void OnUse()
        {
                        
            base.OnUse();
        }
    }
}
