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

        //начальные координаты стены
        private const int beginX = 180;
        private const int beginY = 210;

        //количество блоков по-вертикали и горизонтали в стене
        private const int rows = 3;
        private const int columns = 6;

        //количество мишеней, которые будут генерироваться в стене
        private const int targetsNumber = 4;

        //расстояние до стены метрах
        private const double distance = 300;
        //размеры блока в метрах
        private const double blockSize = 0.5;
        //ускорение свободного падения
        private const double g = 9.81;
        //начальная скорость ядра
        private const double minPow = 5;


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

        public int Hit(int xAngle, int yAngle, int Pow)
        {
            double xAngleRad = Math.PI * (double)xAngle / 180.0;
            double yAngleRad = Math.PI * (double)yAngle / 180.0;

            if (Pow == 0) Pow = 1;

            double Smax = (Math.Pow(minPow * Pow, 2) * Math.Sin(yAngleRad))/g;



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
