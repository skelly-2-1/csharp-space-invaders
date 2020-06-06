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
    public partial class spaceinvaders : Form, IMessageFilter
    {

        // the timer that forces our window to refresh
        private static System.Windows.Forms.Timer refresh_timer;
        private Image space_ship;
        private Random rnd;
        private int pos_x, pos_y;
        private int ship_size_x, ship_size_y;
        private bool move_left = false, move_right = false;

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

            // move our ship
            if (move_left)
            {
                pos_x -= 5;

                if (pos_x < 0) pos_x = 0;
            }
            else if (move_right)
            {
                pos_x += 5;

                if (pos_x > ClientSize.Width - ship_size_x) pos_x = ClientSize.Width - ship_size_x;
            }

            // create the rectangle (position and size) for DrawImage
            RectangleF rect = new RectangleF(pos_x, pos_y, ship_size_x, ship_size_y);

            // draw our space ship
            g.DrawImage(space_ship, rect);
        }

        public spaceinvaders()
        {
            InitializeComponent();

            // create the timer that keeps refreshing our window
            int target_fps = 144; // how many FPS our window should have

            refresh_timer = new System.Windows.Forms.Timer();
            refresh_timer.Interval = (int)(1000.0 / ((double)target_fps)); // turn FPS to millisecond interval
            refresh_timer.Tick += new System.EventHandler(timer_event);
            refresh_timer.Start();

            // load our space ship image
            space_ship = Image.FromFile("./../../../img/ship.png");

            // create random object
            rnd = new Random();

            // the size of our window
            Size window_client_size = ClientSize;

            // size of our space ship
            ship_size_x = 100;
            ship_size_y = 100;

            // position of our space ship
            pos_x = (ClientSize.Width - ship_size_x) / 2;
            pos_y = (ClientSize.Height - ship_size_y) - ClientSize.Height / 50;

            // add a message filter so we can capture key presses (and releases)
            Application.AddMessageFilter(this);
        }

        public bool PreFilterMessage(ref Message m)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_KEYUP = 0x101;

            if (m.Msg != WM_KEYDOWN && m.Msg != WM_KEYUP) return false;

            KeyEventArgs e = new KeyEventArgs(((Keys)((int)((long)m.WParam))) | ModifierKeys);

            Keys keycode = e.KeyCode;

            if (keycode == Keys.A || keycode == Keys.D || keycode == Keys.Left || keycode == Keys.Right)
            {
                bool down = (m.Msg == WM_KEYDOWN);
                bool left = (keycode == Keys.A || keycode == Keys.Left);

                if (down)
                {
                    if (left)
                    {
                        move_left = true;
                    }
                    else
                    {
                        move_right = true;
                    }
                }
                else
                {
                    if (left)
                    {
                        move_left = false;
                    }
                    else
                    {
                        move_right = false;
                    }
                }
            }

            return false;
        }
    }
}