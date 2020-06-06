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

        private Image space_ship;

        private Random rnd;

        private void timer_event(Object source, EventArgs e)
        {
            // refreshes the window, forcing it to re-draw (so OnPaint gets called again)
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // call windows forms onpaint
            base.OnPaint(e);

            // our graphics object
            Graphics g = e.Graphics;

            // position of our space ship
            int x = 100;
            int y = 100;

            // size of our space ship
            int size_x = 100;
            int size_y = 100;

            // the size of our window
            Size window_client_size = ClientSize;

            // draw spaceship at random position
            x = rnd.Next(0, window_client_size.Width - size_x);
            y = rnd.Next(0, window_client_size.Height - size_y);

            // create the rectangle (position and size) for DrawImage
            RectangleF rect = new RectangleF(x, y, size_x, size_y);

            // draw our space ship
            g.DrawImage(space_ship, rect);
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

            // load our space ship image
            space_ship = Image.FromFile("ship.png");

            // create random object
            rnd = new Random();
        }
    }
}