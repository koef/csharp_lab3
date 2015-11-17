using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LandOfBattle
{
    public partial class frmMain : Form
    {
        Cannon cn;
        public frmMain()
        {
            InitializeComponent();
        }

        public class Cannon
        {
            int Height;
            int bottomWidth;
            int topWidth;
            int deltaWidth;

            PaintEventArgs pe;

            int leftBottomPointX;
            int leftBottomPointY;
            int rightBottomPointX;
            int rightBottomPointY;
            int leftTopPointX;
            int leftTopPointY;
            int rightTopPointX;
            int rightTopPointY;

            public Cannon(int fieldWidth, int fieldHeight, int X, int Y, PaintEventArgs paintEvent)
            {
                Height = fieldHeight / 7;
                deltaWidth = 5;
                bottomWidth = fieldWidth / 24;
                topWidth = bottomWidth - deltaWidth * 2;
                leftBottomPointX = fieldWidth / 2 - bottomWidth / 2 + X;
                leftBottomPointY = fieldHeight + Y;
                rightBottomPointX = leftBottomPointX + bottomWidth;
                rightBottomPointY = fieldHeight + Y;
                leftTopPointX = leftBottomPointX + deltaWidth;
                leftTopPointY = leftBottomPointY - Height;
                rightTopPointX = leftTopPointX + topWidth;
                rightTopPointY = leftTopPointY;

                pe = paintEvent;
            }

            public void PullUp()
            {
                deltaWidth -= 1;
                leftTopPointY += 5;
                rightTopPointY += 5;
                this.Draw();
            }

            public void PullDown()
            {
                deltaWidth += 1;
                leftTopPointY -= 5;
                rightTopPointY -= 5;
                this.Draw();
            }

            public void Draw()
            {
                Graphics g = pe.Graphics;

                GraphicsPath pathCannon = new GraphicsPath();
                pathCannon.AddLine(rightBottomPointX, rightBottomPointY, leftBottomPointX, leftBottomPointY);
                pathCannon.AddLine(leftBottomPointX, leftBottomPointY, leftTopPointX, leftTopPointY);
                pathCannon.AddLine(leftTopPointX, leftTopPointY, rightTopPointX, rightTopPointY);
                pathCannon.AddLine(rightTopPointX, rightTopPointY, rightBottomPointX, rightBottomPointY);

                //Pen pn = new Pen(Color.Brown);
                //g.DrawPath(pn, pathCannon);
                SolidBrush brushCannon = new SolidBrush(Color.Brown);
                g.FillPath(brushCannon, pathCannon);
            }
        }
        //private void frmMain_Paint(object sender, PaintEventArgs e)
        //{
        //    int fieldWidth = this.Width - 30;
        //    int fieldHeight = this.Height - 45;
        //    cn = new Cannon(fieldWidth, fieldHeight, 10, 10, e);
        //    cn.Draw();
        //}

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            base.OnPaint(e);
        }

        private void tmrHeartbeat_Tick(object sender, EventArgs e)
        {
            
        }
    }
}
