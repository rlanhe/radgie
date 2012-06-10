using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AsteroidsStorm.GameComponents.GUI
{
    class Bar: Image
    {
        public int Target
        {
            get
            {
                return mTarget;
            }
            set
            {
                mTarget = value;
            }
        }
        private int mTarget;
        private int mCurrentValue;

        public int Value
        {
            set
            {
                Target = mCurrentValue = value;
                float xPos = 0.0f;
                if (Absolute)
                {
                    xPos = LayaoutUtil.GetPositionInX(this);
                }
                else
                {
                    xPos = LayaoutUtil.GetPositionPercentInX(this);
                }

                Width = (int)((mCurrentValue / 100.0f) * mMax);

                if (Absolute)
                {
                    LayaoutUtil.SetPositionInX(this, (int)xPos);
                }
                else
                {
                    LayaoutUtil.SetPositionPercentInX(this, xPos);
                }
            }
            get
            {
                return mCurrentValue;
            }
        }

        public bool Absolute { get; set; }
        
        private int mMax;
        
        public void Update(GameTime time)
        {
            int step = 2;
            int diff = Math.Abs(Math.Abs(mTarget)-Math.Abs(mCurrentValue));
            if (diff >= step)
            {
                float xPos = 0.0f;
                if (Absolute)
                {
                    xPos = LayaoutUtil.GetPositionInX(this);
                }
                else
                {
                    xPos = LayaoutUtil.GetPositionPercentInX(this);
                }

                if (mTarget > mCurrentValue)
                {
                    mCurrentValue += step;
                }
                else
                {
                    mCurrentValue -= step;
                }

                Width = (int)((mCurrentValue / 100.0f) * mMax);

                if (Absolute)
                {
                    LayaoutUtil.SetPositionInX(this, (int)xPos);
                }
                else
                {
                    LayaoutUtil.SetPositionPercentInX(this, xPos);
                }
            }
        }

        public Bar(string id, int width, int height)
            : base(id, width, height)
        {
            mMax = width;
            mCurrentValue = Target = 0;

            Width = (mCurrentValue / 100) * mMax;
            LayaoutUtil.SetPositionPercentInX(this, 0.0f);
        }
    }
}
