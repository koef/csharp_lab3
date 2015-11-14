﻿using System;
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
        public frmMain()
        {
            InitializeComponent();
        }
        public class BattleField
        {
            int Height;
            int Width;
            int fieldTopCornerX;
            int fieldTopCornerY;
            PaintEventArgs pe;


            public BattleField(int fieldWidth, int fieldHeight, int X, int Y, PaintEventArgs paintEvent)
            {
                Width = fieldWidth;
                Height = fieldHeight;
                fieldTopCornerX = X;
                fieldTopCornerY = Y;
                pe = paintEvent;
            }

            public void SetDimensions(int fieldWidth, int fieldHeight)
            {
                Width = fieldWidth;
                Height = fieldHeight;
                this.Draw();
            }

            public void Draw()
            {
                int Border = 1;

                int skyTopCornerX = fieldTopCornerX + Border;
                int skyTopCornerY = fieldTopCornerY + Border;
                int skyHeight = Height / 3;
                int skyWidth = Width - 1;
                int grassTopCornerX = fieldTopCornerX + Border;
                int grassTopCornerY = skyTopCornerY + skyHeight;
                int grassHeight = Height - skyHeight - Border;
                int grassWidth = Width - 1;

                Graphics g = pe.Graphics;
                //Border
                Pen pnBorder = new Pen(Color.Black);
                Rectangle rectBorder = new Rectangle(fieldTopCornerX, fieldTopCornerY, Width, Height);
                g.DrawRectangle(pnBorder, rectBorder);

                //Sky
                Rectangle rectSky = new Rectangle(skyTopCornerX, skyTopCornerY, skyWidth, skyHeight);
                SolidBrush brushSky = new SolidBrush(Color.Blue);
                g.FillRectangle(brushSky, rectSky);

                //Grass
                Rectangle rectGrass = new Rectangle(grassTopCornerX, grassTopCornerY, grassWidth, grassHeight);
                SolidBrush brushGrass = new SolidBrush(Color.Green);
                g.FillRectangle(brushGrass, rectGrass);
            }

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
                Height = fieldHeight / 8;
                deltaWidth = 3;
                bottomWidth = fieldWidth / 10;
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

            public void Draw()
            {
                Graphics g = pe.Graphics;
                GraphicsPath pathCannon = new GraphicsPath(new Point[] {
                    new Point(leftBottomPointX, leftBottomPointY), new Point(leftTopPointX, leftTopPointY),
                    new Point(rightTopPointX, rightTopPointY), new Point(rightBottomPointX, rightBottomPointY) },
                    new byte[] {
                        (byte)PathPointType.Start,
                        (byte)PathPointType.Line,
                        (byte)PathPointType.Bezier,
                        (byte)PathPointType.Line,
                });
                SolidBrush brushCannon = new SolidBrush(Color.Brown);
                g.FillPath(brushCannon, pathCannon);
            }
        }
        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            int fieldWidth = this.Width - 30;
            int fieldHeight = this.Height - 45;
            //BattleField bf = new BattleField(fieldWidth, fieldHeight, 10, 10, e);
            //bf.Draw();
            Cannon cn = new Cannon(fieldWidth, fieldHeight, 10, 10, e);
            cn.Draw();
        }
    }
}
