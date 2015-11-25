using LandOfBattle.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandOfBattle
{
    class CCannon : CImageBase
    {
        private static Bitmap bitmapCannon = Resources.cannon1;
        public int XAngle;
        public int YAngle;

        public CCannon() : base(Resources.cannon1)
        {
            XAngle = 0;
            YAngle = 0;
        }

        public void TurnRight()
        {
            if (XAngle < 20)
            {
                XAngle++;
                Rotate(XAngle);
            }
        }

        public void TurnLeft()
        {
            if (XAngle > -20)
            {
                XAngle--;
                Rotate(XAngle);
            }
        }

        private void Rotate(int _xangle)
        {
            using (Graphics gfx = Graphics.FromImage(_bitmap))
            {
                gfx.TranslateTransform((float)bitmapCannon.Width / 2, (float)bitmapCannon.Height);
                gfx.RotateTransform(_xangle);
                gfx.TranslateTransform(-(float)bitmapCannon.Width / 2, -(float)bitmapCannon.Height);
                gfx.Clear(Color.Transparent);
                gfx.DrawImage(bitmapCannon, 0, 0);
            }
        }

        public void PullDown()
        {
            if (YAngle < 18) {
                YAngle++;
                Pull(YAngle);
            }
        }

        public void PullUp()
        {
            if (YAngle > -18)
            {
                YAngle--;
                Pull(YAngle);
            }
        }

        private void Pull(int _yangle)
        {
            Point[] destinationPoints = {
                    new Point(0, YAngle),   // destination for upper-left point of original
                    new Point(_bitmap.Width, YAngle),  // destination for upper-right point of original
                    new Point(0, _bitmap.Height)};  // destination for lower-left point of original
            using (Graphics gfx = Graphics.FromImage(_bitmap))
            {
                gfx.Clear(Color.Transparent);
                gfx.DrawImage(bitmapCannon, destinationPoints);
            }
        }

    }
}
