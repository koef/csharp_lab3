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
        public const int Brick = 0;
        public const int Target = 1;
        public const int Empty = 2;

        public int TypeOfPart
        {
            get { return partType; }
        }

        public CPartOfWall() : base(Resources.brick)
        {
            partType = Brick;
        }


        public CPartOfWall(int _partType) : base(new Bitmap(35, 35))
        {
            ChangeState(_partType);
        }

        private void ChangeState(int _partType)
        {
            switch (_partType)
            {
                case 0:
                    _bitmap = Resources.brick;
                    partType = Brick;
                    break;
                case 1:
                    _bitmap = Resources.target;
                    partType = Target;
                    break;
                case 2:
                    _bitmap = new Bitmap(35, 35);
                    partType = Empty;
                    break;
                default:
                    _bitmap = Resources.brick;
                    partType = Brick;
                    break;
            }
        }

        public void Destroy()
        {
            ChangeState(Empty);
        }
    }
}
