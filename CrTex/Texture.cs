using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using SdlDotNet;
using SdlDotNet.Graphics;

using System.Drawing;

namespace CrTex
{
    public enum texture_flags
    {
        TEXTURE_FLIP_X = 0x01,
        TEXTURE_FLIP_Y = 0x02,
        TEXTURE_IS_IN_ANIMATION = 0x04,
    }

    public struct tflag
    {
        uint value;

        public tflag(uint val)
        {
            value = val;
        }

        public static bool operator &(tflag flag, uint test)
        {
            return ((flag.value & test) == test);
        }
    }
    
    class Texture 
    {
        string name;            //The name of the texture, as it will be used in the engine

        int width, height;      //Width, Height of texture

        tflag t_flags;          //Bitwise flagspace for this texture
                                //Using positive values, no need for negatives
        Surface surface;        //Surface for this texture

        string animation_name;

        public Texture(Bitmap pic, uint flags)
        {
            surface = new Surface(pic);

            t_flags = new tflag(flags);
        }
    }

    class FrameSet
    {
        string name;            //The name of the texture, as it will be used in the engine

        private Dictionary<int, Frame> surface_collection;        //Surface for this texture
                
        int current_frame;

        public FrameSet()
        {
            current_frame = 0;

            surface_collection = new Dictionary<int, Frame>();
        }

        public void AddFrame(Bitmap image)
        {
            surface_collection.Add(surface_collection.Keys.Count, new Frame(image, 0));
        }

        public void RemoveFrame(int index)
        {
            if (!surface_collection.Keys.Contains<int>(index))
                return;

            surface_collection.Remove(index);            
        }

        public Frame GetFrame(int index)
        {
            return surface_collection[index];
        }

        public void SetFrame(int index, Frame frame)
        {
            surface_collection[index] = frame;
        }
    }

    class Frame
    {
        Surface surface;
        int delay;

        public Frame(Bitmap surf, int d)
        {
            surface = new Surface(surf);
            delay = d;
        }

        public int GetDelay()
        {
            return delay;
        }

        public void SetDelay(int d)
        {
            delay = d;
        }

        public Surface GetSurface()
        {
            return surface;
        }

        public void SetSurface(Surface surf)
        {
            surface = surf;
        }
    }
}
