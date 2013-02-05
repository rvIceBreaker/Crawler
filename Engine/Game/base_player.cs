using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine.Components;

using SdlDotNet.Input;
using SdlDotNet.Graphics.Sprites;

namespace Engine.Game
{
    public class base_player : base_phys_ent
    {
        public bool mIsLocalPlayer;
        public int mLocalPlayerIndex; //For same-system multiplayer games
        internal float mMoveSpeed, mWalkSpeed = 0.025f, mRunSpeed = 0.05f;
        public bool mTakeMouseInput = true;

        public int health = 100, stamina = 100, ac = 0;
        int maxStamina = 100;
        double weight, maxWeight;

        double lastSprintTime;

        public base_player() : base()
        {
            mTexFrames = new AnimationCollection();
            mTexFrames.Add("tex3.bmp", new System.Drawing.Size(64, 64));
            //mTexFrames.Add("tex2.bmp", new System.Drawing.Size(64, 64));

            mTexture = new AnimatedSprite("player", mTexFrames);
        }

        public override void Think()
        {
            mTexture.Frame++;

            if ((Engine.mInput.mInputString & (int)INPUT_BITS.IN_SPRINT) == (int)INPUT_BITS.IN_SPRINT && (stamina > 0))
            {
                mMoveSpeed = mRunSpeed;

                stamina--;
            }
            else
            {
                if ((Engine.mInput.mLastString & (int)INPUT_BITS.IN_SPRINT) == (int)INPUT_BITS.IN_SPRINT)
                {
                    lastSprintTime = Engine.mCurrentTime;
                }

                if ((Engine.mCurrentTime - lastSprintTime) > 5 && (stamina < maxStamina))
                {
                    stamina += 1;
                }

                mMoveSpeed = mWalkSpeed;
            }

            if (mIsLocalPlayer)
            {
                Movement();

                if ((Engine.mInput.mInputString & (int)INPUT_BITS.IN_MOUSE) == (int)INPUT_BITS.IN_MOUSE)
                {
                    if (!((Engine.mInput.mLastString & (int)INPUT_BITS.IN_MOUSE) == (int)INPUT_BITS.IN_MOUSE))
                    {
                        mTakeMouseInput = !mTakeMouseInput;
                        Engine.mInput.mCaptureMouse = !Engine.mInput.mCaptureMouse;
                    }
                }

                if (mTakeMouseInput)
                {
                    mAngles.Add(Engine.mInput.mousex);

                    if (mBoundToViewpoint)
                    {
                        mBoundViewport.Plane.Add(Engine.mInput.mousex);
                    }
                }
            }

            base.Think();
        }

        public void Movement()
        {
            if ((Engine.mInput.mInputString & (int)INPUT_BITS.IN_UP) == (int)INPUT_BITS.IN_UP)
            {
                AddVelocity((mAngles.ToVector2() * mMoveSpeed));
            }
            
            if ((Engine.mInput.mInputString & (int)INPUT_BITS.IN_DOWN) == (int)INPUT_BITS.IN_DOWN)
            {
                AddVelocity( (mAngles.ToVector2() * mMoveSpeed) * -1);
            }

            if ((Engine.mInput.mInputString & (int)INPUT_BITS.IN_LEFT) == (int)INPUT_BITS.IN_LEFT)
            {
                //Angle2D angle = mAngles;

                AddVelocity((mAngles.Right().ToVector2() * mMoveSpeed) * -1);
            }

            if ((Engine.mInput.mInputString & (int)INPUT_BITS.IN_RIGHT) == (int)INPUT_BITS.IN_RIGHT)
            {
                //Angle2D angle = mAngles;

                AddVelocity( (mAngles.Right().ToVector2() * mMoveSpeed) );
            }

            base.Think();
        }

        internal void SetAsLocalPlayer()
        {
            mBoundViewport =  Engine.mRenderer.FindViewportByName("viewport1");
            mBoundViewport.GetViewpoint().BindToEntity(this);
            mBoundToViewpoint = true;
            mIsLocalPlayer = true;
        }

    }
}
