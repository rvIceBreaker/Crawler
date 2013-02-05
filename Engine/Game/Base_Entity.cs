using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Game
{
    public class Base_Entity
    {
        public Vector2 mPosition;
        public Angle2D mAngles;

        public ENT_TYPE mEntityType;

        public bool mBoundToViewpoint = false;
        public Components.Renderer.Viewport mBoundViewport;

        public Base_Entity() { }

        public virtual void Think() { }

        public virtual void OnUse() { }
        public virtual void OnTouch() { }
    }
}
