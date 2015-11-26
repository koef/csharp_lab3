using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandOfBattle
{
    class CPowLevel : CImageBase
    {
        public int Level = 0;
        //private Bitmap bmpPowLevel;
        public CPowLevel():base(new Bitmap(18, 100))
        {
            SetLevel(78);
        }

        private void SetLevel(int percents)
        {
            using (Graphics gfx = Graphics.FromImage(_bitmap))
            {
                Pen penBorder = new Pen(Color.Black);
                SolidBrush brushLevel = new SolidBrush(Color.Red);
                gfx.DrawRectangle(penBorder, 0, 0, _bitmap.Width - 1, _bitmap.Height - 1);
                int minX = 1;
                int minY = 1;
                int maxWidth = _bitmap.Width - 2;
                int maxHeigth = _bitmap.Height - 1;
                if (percents > 0 && percents < 100) 
                {
                    int val = (int)(maxHeigth / 100.0 * percents);
                    gfx.FillRectangle(brushLevel, minX, maxHeigth - val, maxWidth, val);
                }
                else if (percents >= 100)
                {
                    gfx.FillRectangle(brushLevel, minX, minY, maxWidth, maxHeigth - 1);
                }

            }
        }

        public void NextLevel()
        {
            SetLevel(100);
        }
    }
}
