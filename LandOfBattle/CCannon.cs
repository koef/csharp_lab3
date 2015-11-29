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
        private static Bitmap bitmapCannon = Resources.cannon;
        private static Bitmap bitmapCannonFire = Resources.cannon_fire;
        public int XAngle, YAngle;

        public CCannon() : base(Resources.cannon)
        {
            XAngle = 0;
            YAngle = 0;
        }

        public void TurnRight()
        {
            if (XAngle < 20)
            {
                XAngle++;
                Rotate(XAngle, YAngle, false);
            }
        }

        public void TurnLeft()
        {
            if (XAngle > -20)
            {
                XAngle--;
                Rotate(XAngle, YAngle, false);
            }
        }

        public void PullDown()
        {
            if (YAngle < 18) {
                YAngle++;
                Rotate(XAngle, YAngle, false);
            }
        }

        public void PullUp()
        {
            if (YAngle > -18)
            {
                YAngle--;
                Rotate(XAngle, YAngle, false);
            }
        }

        private void Rotate(int _xangle, int _yangle, bool _isShooting)
        {
            Point[] destinationPoints = {
                    new Point(0, YAngle),   // destination for upper-left point of original
                    new Point(_bitmap.Width, YAngle),  // destination for upper-right point of original
                    new Point(0, _bitmap.Height)};  // destination for lower-left point of original
            using (Graphics gfx = Graphics.FromImage(_bitmap))
            {
                gfx.TranslateTransform((float)bitmapCannon.Width / 2, (float)bitmapCannon.Height);
                gfx.RotateTransform(_xangle);
                gfx.TranslateTransform(-(float)bitmapCannon.Width / 2, -(float)bitmapCannon.Height);
                gfx.Clear(Color.Transparent);
                if (_isShooting) {
                    gfx.DrawImage(bitmapCannonFire, destinationPoints);
                } else {
                    gfx.DrawImage(bitmapCannon, destinationPoints);
                }
            }
        }

        public void Fire(bool isShooting)
        {
            Rotate(XAngle, YAngle, isShooting);
        }
    }
}
