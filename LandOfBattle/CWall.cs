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

        //количество блоков по-вертикали и горизонтали в стене
        private int rows;
        private int columns;

        //количество мишеней, которые будут генерироваться в стене
        private int targetsNumber;

        private int _targetRemains = 0;

        public int TargetRemains
        {
            get
            {

                return _targetRemains;
            }
        }

        public CWall(int beginX, int beginY, int _rows = 3, int _columns = 6, int _targetsNumber = 4)
        {
            //количество блоков по-вертикали не может быть меньше 2 и больше 5
            if (rows >= 2 && _rows <= 5) rows = _rows;
            else rows = 4;

            //количество блоков по-горизонтали не может быть меньше 2 и больше 8
            //так же оно должно быть обязательно четным, т.к. на этом завязана логика CalculateHit
            if (_columns >= 2 && _columns <= 8 && _columns % 2 == 0) columns = _columns;
            else columns = 8;

            //если в конструкторе количество мишеней задано больше, чем количество блоков,
            //то количество мишеней равно количеству блоков в стене
            if (_targetsNumber > columns * rows) targetsNumber = columns * rows;
            else targetsNumber = _targetsNumber;

            arrWall = new CPartOfWall[rows, columns];

            Point[] targetPoint;
            targetPoint = new Point[targetsNumber];

            int curX = beginX;
            int curY = beginY;

            Random rnd = new Random();
            for (int tc = 0; tc < targetsNumber; tc++)
            {
                targetPoint[tc] = new Point(rnd.Next(columns), rnd.Next(rows));
            }

            for (int r = rows - 1; r >= 0; r--)
            {
                curX = beginX;
                for (int c = 0; c < columns; c++)
                {
                    int partState = CPartOfWall.Brick;
                    foreach (Point pnt in targetPoint)
                    {
                        if (pnt.X == c && pnt.Y == r)
                        {
                            if(partState != CPartOfWall.Target)
                            {
                                partState = CPartOfWall.Target;
                                _targetRemains += 1;
                            }
                        }
                    }
                    arrWall[r, c] = new CPartOfWall(partState);

                    arrWall[r, c].Left = curX;
                    arrWall[r, c].Top = curY;

                    curX += CPartOfWall.Width;
                }
                curY += CPartOfWall.Height;
            }
        }

        public void Draw(Graphics gfx)
        {
            for (int r = rows - 1; r >= 0; r--)
            {
                for (int c = 0; c < columns; c++)
                {
                    arrWall[r, c].DrawImage(gfx);
                }
            }
        }

        public int Hit(int row, int column, bool isExploding)
        {
            int bState = arrWall[row, column].State;
            if (bState != CPartOfWall.Empty)
            {
                if (isExploding)
                {
                    DestroyBlock(row, column); //собственно блок
                    DestroyBlock(row + 1, column); //блок выше
                    DestroyBlock(row - 1, column); //ниже
                    DestroyBlock(row, column + 1); //правее
                    DestroyBlock(row, column - 1); //левее
                }
                else
                {
                    DestroyBlock(row, column);
                }

            }
            return bState;
        }

        private void DestroyBlock(int row, int column)
        {
            if(row < rows && row >= 0 && column < columns && column >= 0)
            {
                //если блок в верхнем ряду, ничего не сдвигается
                if (row == rows - 1)
                {
                    arrWall[row, column].Destroy();
                }
                else
                {

                    for(int r = row; r < rows; r++)
                    {
                        if (r + 1 < rows) arrWall[r, column].ChangeState(arrWall[r + 1, column].State);
                        else
                        {
                            arrWall[r, column].Destroy();
                        }
                    }

                    //если блок в нижнем ряду и представляет собой мишень, то смещаем повторно 
                    if (row == 0)
                    {
                        while(arrWall[row, column].State == CPartOfWall.Target)
                        {
                            for (int r = row; r < rows; r++)
                            {
                                if (r + 1 < rows) arrWall[r, column].ChangeState(arrWall[r + 1, column].State);
                                else
                                {
                                    arrWall[r, column].Destroy();
                                }
                            }
                        }
                    }
                }
            }
            _targetRemains = 0;
            foreach (CPartOfWall part in arrWall)
            {
                if (part.State == CPartOfWall.Target) _targetRemains += 1;
            }
        }
    }
}
