using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SdlDotNet.Input;

namespace Engine.Components
{
    public class Input
    {
        public int mInputString, mLastString;

        public float mousex;

        public bool mCaptureMouse = true;

        public Input()
        {
            Console.WriteLine("Init Input...");
        }

        public void Frame()
        {
            //Input works by setting bitstrings for the base_player class to use
            //The bitstring is an integer, and each key, when pressed, sets each bit appropriately to on or off.
            //The bit values are found in the INPUT_BITS enum.

            mLastString = mInputString;
            mInputString = 0;

            if (Keyboard.IsKeyPressed(Key.W))
                mInputString += (int)INPUT_BITS.IN_UP;

            if (Keyboard.IsKeyPressed(Key.S))
                mInputString += (int)INPUT_BITS.IN_DOWN;

            if (Keyboard.IsKeyPressed(Key.A))
                mInputString += (int)INPUT_BITS.IN_LEFT;

            if (Keyboard.IsKeyPressed(Key.D))
                mInputString += (int)INPUT_BITS.IN_RIGHT;

            if (Mouse.IsButtonPressed(MouseButton.PrimaryButton))
                mInputString += (int)INPUT_BITS.IN_ATTACK1;

            if (Mouse.IsButtonPressed(MouseButton.SecondaryButton))
                mInputString += (int)INPUT_BITS.IN_ATTACK2;

            if (Keyboard.IsKeyPressed(Key.E))
                mInputString += (int)INPUT_BITS.IN_USE;

            if (Keyboard.IsKeyPressed(Key.I))
                mInputString += (int)INPUT_BITS.IN_INVENTORY;

            if (Keyboard.IsKeyPressed(Key.C))
                mInputString += (int)INPUT_BITS.IN_CHARSHEET;

            if (Keyboard.IsKeyPressed(Key.Tab))
                mInputString += (int)INPUT_BITS.IN_MOUSE;

            if (Keyboard.IsKeyPressed(Key.LeftShift) || Keyboard.IsKeyPressed(Key.RightShift))
                mInputString += (int)INPUT_BITS.IN_SPRINT;

            if (mCaptureMouse)
            {
                mousex = Mouse.MousePosition.X;

                //Renderer.Viewport view = Engine.mRenderer.FindViewportByIndex(0);

                int screenwidth = Engine.mRenderer.WindowWidth(), screenheight = Engine.mRenderer.WindowHeight();
                Mouse.MousePosition = new System.Drawing.Point(screenwidth / 2, screenheight / 2);
                //Subtract half of the screen space, to make it a negative/positive, multiply by 0.001 for sensativity, and negate for rotation correction
                mousex = (float)((mousex - (screenwidth / 2)) * 0.001);
            }
            //Console.WriteLine(Convert.ToString(mInputString, 2)+" "+mInputString);
        }
    }
}
