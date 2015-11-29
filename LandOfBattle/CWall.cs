using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandOfBattle
{
    class CWall
    {
        public CPartOfWall[,] arrWall;

        private int rows = 3;
        private int columns = 6;
        private int beginX = 180;
        private int beginY = 210;
        private int targetsNumber = 4;


        public CWall()
        {
            arrWall = new CPartOfWall[rows, columns];

            Point[] targetPoint;
            targetPoint = new Point[targetsNumber];

            int curX = beginX;
            int curY = beginY;

            Random rnd = new Random();
            for (int tc = 0; tc < targetsNumber; tc++)
            {
                targetPoint[tc] = new Point(rnd.Next(columns - 1), rnd.Next(rows - 1));
            }

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    int partType = CPartOfWall.Brick;
                    foreach (Point pnt in targetPoint)
                    {
                        if (pnt.X == c && pnt.Y == r) partType = CPartOfWall.Target;
                    }
                    arrWall[r, c] = new CPartOfWall(partType);

                    arrWall[r, c].Left = curX;
                    arrWall[r, c].Top = curY;

                    curX += CPartOfWall.Width;
                }
                curY += CPartOfWall.Height;
            }
        }

        public void Draw(Graphics gfx)
        {
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    arrWall[r, c].DrawImage(gfx);
                }
            }
        }

        public int Hit(int x, int y)
        {
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    Rectangle shell = new Rectangle(x, y, 10, 10);
                    if(arrWall[r, c].Rectangle.Contains(shell))
                    {
                        if(arrWall[r, c].TypeOfPart != CPartOfWall.Empty)
                        {
                            arrWall[r, c].Destroy();
                            return arrWall[r, c].TypeOfPart;
                        }
                    }
                }
            }
            return 0;
        }
    }
}
