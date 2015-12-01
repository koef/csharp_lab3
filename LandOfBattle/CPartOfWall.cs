using LandOfBattle.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandOfBattle
{
    class CPartOfWall : CImageBase
    {
        public int partType = 0;
        public static int Brick = 1;
        public static int Target = 2;
        public static int Empty = 0;

        public int State
        {
            get { return partType; }
        }

        public static int Width = 35;
        public static int Height = 35;

        public CPartOfWall() : base(Resources.brick)
        {
            partType = Brick;
        }


        public CPartOfWall(int _partType) : base(new Bitmap(35, 35))
        {
            ChangeState(_partType);
        }

        public void ChangeState(int _partType)
        {
            if(partType != _partType)
            {
                switch (_partType)
                {
                    case 0:
                        _bitmap = new Bitmap(35, 35);
                        partType = Empty;
                        break;
                    case 1:
                        _bitmap = Resources.brick;
                        partType = Brick;
                        break;
                    case 2:
                        _bitmap = Resources.target;
                        partType = Target;
                        break;
                    default:
                        _bitmap = Resources.brick;
                        partType = Brick;
                        break;
                }
            }
        }

        //public Rectangle Rectangle
        //{
        //    get
        //    {
        //        return new Rectangle(Left, Top, Width, Height);
        //    }
        //}

        public void Destroy()
        {
            ChangeState(Empty);
        }


    }
}
