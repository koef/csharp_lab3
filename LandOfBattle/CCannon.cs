using LandOfBattle.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandOfBattle
{
    class CCannon : CImageBase
    {
        //private int X;
        //private int Y;
        private static Bitmap bitmapCannon = Resources.cannon1;
        public int angle;

        public CCannon() : base(Resources.cannon1)
        {
            angle = 0;
        }

        public void TurnRight()
        {
            
            if (angle <= 20)
            {
                angle++;
                using (Graphics gfx = Graphics.FromImage(_bitmap))
                {
                    gfx.TranslateTransform((float)bitmapCannon.Width / 2, (float)bitmapCannon.Height / 2);
                    gfx.RotateTransform(angle);
                    gfx.TranslateTransform(-(float)bitmapCannon.Width / 2, -(float)bitmapCannon.Height / 2);
                    gfx.Clear(Color.Transparent);
                    gfx.DrawImage(bitmapCannon, 0, 0);
                }
            }
        }

        public void TurnLeft()
        {
            
            if (angle >= -20)
            {
                angle--;
                using (Graphics gfx = Graphics.FromImage(_bitmap))
                {
                    gfx.TranslateTransform((float)bitmapCannon.Width / 2, (float)bitmapCannon.Height / 2);
                    gfx.RotateTransform(angle);
                    gfx.TranslateTransform(-(float)bitmapCannon.Width / 2, -(float)bitmapCannon.Height / 2);
                    gfx.Clear(Color.Transparent);
                    gfx.DrawImage(bitmapCannon, 0, 0);
                }
            }
        }
    }
}
