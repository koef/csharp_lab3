#define DEBUGGING
using LandOfBattle.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
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

        bool explosiveShells = false;

        //counters
        int cannonFireCounter = 0;
        int hitMessageCounter = 0;

        string hitMessage = "";

        //количество блоков стены по-горизонтали
        int cols = 6;
        //количество блоков стены по-вертикали
        int rows = 3;
        //количество генерируемых мишеней
        int targets = 4;
        //расстояние до стены метрах
        double distance = 300;
        //размеры блока в метрах
        double blockSize = 0.5;
        //начальная скорость ядра
        double minPow = 5;

        CCannon cannon;
        CPowLevel powLevel;
        Graphics dc;
        CWall wall;
        


        public frmMain()
        {
            InitializeComponent();

            //инициализируем переменные из конфигурационного файла
            cols = Int32.Parse(ConfigurationManager.AppSettings["cols"]);
            rows = Int32.Parse(ConfigurationManager.AppSettings["rows"]);
            targets = Int32.Parse(ConfigurationManager.AppSettings["targets"]);
            distance = Int32.Parse(ConfigurationManager.AppSettings["distance"]);
            blockSize = Double.Parse(ConfigurationManager.AppSettings["blockSize"], CultureInfo.InvariantCulture);
            minPow = Double.Parse(ConfigurationManager.AppSettings["minPow"]);

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

            TextFormatFlags _textFlags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
            Font _font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);

#if (DEBUGGING)
            TextRenderer.DrawText(dc, "X=" + _curX.ToString() + "  Y=" + _curY.ToString(), _font,
                new Rectangle(10, 15, 120, 20), SystemColors.ControlText, _textFlags);
            TextRenderer.DrawText(dc, "XAngle: " + cannon.XAngle.ToString() + "  YAngle: " + cannon.YAngle.ToString(), 
                _font, new Rectangle(10, 35, 200, 20), SystemColors.ControlText, _textFlags);
            TextRenderer.DrawText(dc, "POW Level: " + powLevel.Level.ToString(), _font,
                new Rectangle(10, 55, 200, 20), SystemColors.ControlText, _textFlags);
#endif
            //индикация типа снаряда
            if (explosiveShells)
            {
                TextRenderer.DrawText(dc, "Разрывные", _font, new Rectangle(720, 20, 150, 20),
                    Color.Black, _textFlags);
            }
            else
            {
                TextRenderer.DrawText(dc, "Обычные", _font, new Rectangle(720, 20, 150, 20),
                    Color.Black, _textFlags);
            }

            //сообщение о точности попадания
            TextFormatFlags hitTextFlags = TextFormatFlags.HorizontalCenter |
                TextFormatFlags.EndEllipsis;
            Font hitFont = new System.Drawing.Font("Arial", 18, FontStyle.Bold);
            TextRenderer.DrawText(dc, hitMessage, hitFont, new Rectangle(1, 102, this.Width, this.Height), 
                Color.Black, hitTextFlags);
            TextRenderer.DrawText(dc, hitMessage, hitFont, new Rectangle(0, 100, this.Width, this.Height),
                Color.White, hitTextFlags);


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
                hitMessage = "";
            }
            if (e.KeyCode == Keys.Enter)
            {
                explosiveShells = !explosiveShells;
                Refresh();
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
                else
                {
                    cannonFireCounter++;
                }
            }

            if(hitMessage != "")
            {
                if(hitMessageCounter >= 30)
                {
                    //hitMessage = "";
                    //Refresh();
                    hitMessageCounter = 0;
                }
                else
                {
                    hitMessageCounter++;
                }
            }
        }

        private void CannonFire()
        {
            cannon.Fire(true);
            CalculateHit();
            powLevel.Reset();
            SoundPlayer fireSound = new SoundPlayer(Resources.CannonSound);
            fireSound.Play();
        }


        private void CalculateHit()
        {
            //ускорение свободного падения
            const double g = 9.81;

            //сдвиг в блоках относительно левого края стены
            int shift = 0;

            double miss;

            //угол поворота пушки
            double xAngle;

            //угол наклона пушки
            double yAngle = cannon.YAngle;
            int powMultiplier = powLevel.Level;

            if (cannon.XAngle >= 0)
            {
                shift = cols / 2;
                xAngle = cannon.XAngle;
            }
            else
            {
                xAngle = Math.Abs(cannon.XAngle);
            }

            double xAngleRad = Math.PI * xAngle / 180.0;
            double yAngleRad = Math.PI * yAngle / 180.0;

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
                    //расстояние от попадания, до перпендикуляра distance
                    double b = distance * Math.Tan(xAngleRad);
                    if(b <= blockSize * cols / 2)
                    {
                        //снаряд попадает в стену
                        int col = (int)Math.Truncate(b / blockSize + shift);
                        int row = (int)Math.Truncate(h / blockSize);
                        wall.Hit(row, col, explosiveShells);
                        hitMessage = "Попадание в " + col.ToString() + " блок " + row.ToString() + "-го ряда";
                    }
                    else
                    {
                        miss = b - blockSize * cols / 2;
                        if (shift == 0)
                        {
                            //снаряд пролетел левее на miss метров
                            hitMessage = "Снаряд пролетел левее на " + Math.Round(miss, 2).ToString() + " метров";
                        } else
                        {
                            //снаряд пролетел правее на miss метров
                            hitMessage = "Снаряд пролетел правее на " + Math.Round(miss, 2).ToString() + " метров";
                        }
                    }

                }
                else
                {
                    //слишком высоко
                    miss = h - blockSize * rows;
                    hitMessage = "Снаряд пролетел выше стены на " + Math.Round(miss, 2).ToString() + " метров";
                }
            }
            else
            {
                //снаряд недолетел
                hitMessage = "Снаряд, пролетев " + Math.Round(Smax, 2).ToString() + " метров, недостиг стены";
            }

        }
    }
}
