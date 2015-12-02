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

        private int _XAngle, _YAngle;

        public double XAngle
        {
            get { return _XAngle / 1.0; }
        }

        public double YAngle
        {
            get { return _YAngle / 2.0; }
        }

        public CCannon() : base(Resources.cannon)
        {
            _XAngle = 0;
            _YAngle = 1;
        }

        public void TurnRight()
        {
            if (_XAngle < 20)
            {
                _XAngle++;
                Rotate(_XAngle, _YAngle, false);
            }
        }

        public void TurnLeft()
        {
            if (_XAngle > -20)
            {
                _XAngle--;
                Rotate(_XAngle, _YAngle, false);
            }
        }

        public void PullDown()
        {
            if (_YAngle > 1) {
                _YAngle--;
                Rotate(_XAngle, _YAngle, false);
            }
        }

        public void PullUp()
        {
            if (_YAngle < 20)
            {
                _YAngle++;
                Rotate(_XAngle, _YAngle, false);
            }
        }

        private void Rotate(int _xangle, int _yangle, bool _isShooting)
        {
            Point[] destinationPoints = {
                    new Point(0, -_YAngle),   // destination for upper-left point of original
                    new Point(_bitmap.Width, -_YAngle),  // destination for upper-right point of original
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
            Rotate(_XAngle, _YAngle, isShooting);
        }
    }
}
