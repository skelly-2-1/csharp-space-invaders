using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csharp_space_invaders
{
    public partial class spaceinvaders : Form
    {
        
        // the timer that forces our window to refresh
        private static System.Windows.Forms.Timer refresh_timer;

        private void timer_event(Object source, EventArgs e)
        {
            // refreshes the window, forcing it to re-draw (so OnPaint gets called again)
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            using (Pen selPen = new Pen(Color.Blue))
            {
                using (Brush brush = new SolidBrush(Color.Blue)) g.FillRectangle(brush, 10, 10, 50, 50);
            }
        }

        public spaceinvaders()
        {
            InitializeComponent();

            // create the timer that keeps refreshing our window
            int target_fps = 60; // how many FPS our window should have

            refresh_timer = new System.Windows.Forms.Timer();
            refresh_timer.Interval = (int)(1000.0 / ((double)target_fps)); // turn FPS to millisecond interval
            refresh_timer.Tick += new System.EventHandler(timer_event);
            refresh_timer.Start();
        }
    }
}
