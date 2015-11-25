using LandOfBattle.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandOfBattle
{
    class CCannon : CImageBase
    {
        //private int X;
        //private int Y;
        public CCannon() : base(Resources.cannon) { }

        public void TurnLeft()
        {
            using (Graphics gfx = Graphics.FromImage(_bitmap))
            {
                gfx.TranslateTransform((float)_bitmap.Width / 2, (float)_bitmap.Height / 2);
                gfx.RotateTransform(10);
            }
        }
    }
}
