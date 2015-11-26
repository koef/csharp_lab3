#define DEBUGGING
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
#if (DEBUGGING)
        int _curX = 0;
        int _curY = 0;
#endif
        CCannon _cannon;
        CPowLevel _powLevel;
        Graphics dc;

        public frmMain()
        {
            InitializeComponent();
            _cannon = new CCannon() { Left = 347, Top = 400 };
            _powLevel = new CPowLevel() { Left = 810, Top = 15 };
            DoubleBuffered = true;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            dc = e.Graphics;

            _cannon.DrawImage(dc);
            _powLevel.DrawImage(dc);


#if (DEBUGGING)
            TextFormatFlags _textFlags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
            Font _font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
            TextRenderer.DrawText(dc, "X=" + _curX.ToString() + "  Y=" + _curY.ToString(), _font,
                new Rectangle(10, 15, 120, 20), SystemColors.ControlText, _textFlags);
            TextRenderer.DrawText(dc, "XAngle:" + _cannon.XAngle.ToString() + "  YAngle: " + _cannon.YAngle.ToString(), _font,
                new Rectangle(10, 35, 200, 20), SystemColors.ControlText, _textFlags);

#endif
        }

        private void tmrHeartbeat_Tick(object sender, EventArgs e)
        {
            
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
                _cannon.TurnLeft();
                Refresh();
            }
            if (e.KeyCode == Keys.Right)
            {
                _cannon.TurnRight();
                Refresh();
            }
            if (e.KeyCode == Keys.Up)
            {
                _cannon.PullUp();
                Refresh();
            }
            if (e.KeyCode == Keys.Down)
            {
                _cannon.PullDown();
                Refresh();
            }
            if (e.KeyCode == Keys.Space)
            {
                _cannon.Fire(true);
                Refresh();
            }
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                _cannon.Fire(false);
                Refresh();
            }
        }
    }
}
