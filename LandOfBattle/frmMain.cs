#define DEBUGGING
using LandOfBattle.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LandOfBattle
{
    public partial class frmMain : Form
    {
#if (DEBUGGING)
        int _curX = 0;
        int _curY = 0;
#endif
        bool powLevelEnabled = false;
        bool cannonFireEnabled = false;
        int cannonFireCounter = 0;

        //количество блоков стены по-горизонтали
        const int cols = 6;
        //количество блоков стены по-вертикали
        const int rows = 3;
        //количество генерируемых мишеней
        const int targets = 4;
        //расстояние до стены метрах
        const double distance = 300;
        //размеры блока в метрах
        const double blockSize = 0.5;
        //начальная скорость ядра
        const double minPow = 5;

        CCannon cannon;
        CPowLevel powLevel;
        Graphics dc;
        CWall wall;
        


        public frmMain()
        {
            InitializeComponent();
            cannon = new CCannon() { Left = 347, Top = 405 };
            powLevel = new CPowLevel() { Left = 810, Top = 15 };
            wall = new CWall(this.Width / 2 - CPartOfWall.Width * 6 / 2, 190, rows, cols, targets);
            DoubleBuffered = true;
            tmrHeartbeat.Enabled = true;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            dc = e.Graphics;

            cannon.DrawImage(dc);
            powLevel.DrawImage(dc);
            wall.Draw(dc);


#if (DEBUGGING)
            TextFormatFlags _textFlags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
            Font _font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
            TextRenderer.DrawText(dc, "X=" + _curX.ToString() + "  Y=" + _curY.ToString(), _font,
                new Rectangle(10, 15, 120, 20), SystemColors.ControlText, _textFlags);
            TextRenderer.DrawText(dc, "XAngle: " + cannon.XAngle.ToString() + "  YAngle: " + cannon.YAngle.ToString(), _font,
                new Rectangle(10, 35, 200, 20), SystemColors.ControlText, _textFlags);
            TextRenderer.DrawText(dc, "POW Level: " + powLevel.Level.ToString(), _font,
                new Rectangle(10, 55, 200, 20), SystemColors.ControlText, _textFlags);
#endif
        }

        private void frmMain_MouseMove(object sender, MouseEventArgs e)
        {
#if (DEBUGGING)
            _curX = e.X;
            _curY = e.Y;
#endif

            Refresh();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                cannon.TurnLeft();
                Refresh();
            }
            if (e.KeyCode == Keys.Right)
            {
                cannon.TurnRight();
                Refresh();
            }
            if (e.KeyCode == Keys.Up)
            {
                cannon.PullUp();
                Refresh();
            }
            if (e.KeyCode == Keys.Down)
            {
                cannon.PullDown();
                Refresh();
            }
            if (e.KeyCode == Keys.Space)
            {
                powLevelEnabled = true;
            }
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                powLevelEnabled = false;
                cannonFireEnabled = true;
                CannonFire();
                Refresh();
            }
        }

        private void tmrHeartbeat_Tick(object sender, EventArgs e)
        {
            if (powLevelEnabled)
            {
                powLevel.NextLevel();
                Refresh();
            }

            if (cannonFireEnabled)
            {
                if (cannonFireCounter >= 2)
                {
                    cannonFireEnabled = false;
                    cannon.Fire(false);
                    Refresh();
                    cannonFireCounter = 0;
                }
                cannonFireCounter++;
            }
        }

        private void CannonFire()
        {
            cannon.Fire(true);
            powLevel.Reset();
            SoundPlayer fireSound = new SoundPlayer(Resources.CannonSound);
            fireSound.Play();
        }


        private void CalculateHit()
        {
            int xAngle = cannon.XAngle;
            int yAngle = cannon.YAngle;
            int powMultiplier = powLevel.Level;

            //ускорение свободного падения
            const double g = 9.81;

            double xAngleRad = Math.PI * (double)xAngle / 180.0;
            double yAngleRad = Math.PI * (double)yAngle / 180.0;

            if (powMultiplier == 0) powMultiplier = 1;
            double Pow = powMultiplier * minPow;

            //дальность броска
            double Smax = (Math.Pow(Pow, 2) * Math.Sin(yAngleRad)) / g;

            //расстояние до стены с учетом угла горизонтального отклонения
            double c = distance / Math.Cos(xAngleRad);


            //если дальность броска больше расстояния до стены с мишенями
            if (Smax >= c)
            {
                double h;
                if (yAngle == 0)
                {
                    h = (g / (2 * Math.Pow(Pow, 2))) * Math.Pow(c, 2);
                }
                else
                {
                    h = c * Math.Tan(yAngleRad) - (g / (2 * Math.Pow(Pow, 2) * Math.Pow(Math.Cos(yAngleRad), 2))) * Math.Pow(c, 2);
                }

                if (h <= blockSize * rows)
                {
                    //снаряд попадает в стену
                }
                else
                {
                    //слишком высоко
                }
            }
            else
            {
                //снаряд недолетел
            }

        }
    }
}
