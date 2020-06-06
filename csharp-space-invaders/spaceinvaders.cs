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

        // frame counter for enemy movement
        private int framecounter;

        // bool for enemy movement left/right
        private bool enemy_moving_right = true;

        // image of our space ship
        private Image space_ship;

        // image of our enemy
        private Image enemy_ship;

        // position of our space ship
        private int pos_x, pos_y;

        // size of our space ship
        private int ship_size_x, ship_size_y;

        // position of our enemy
        private int [] pos_x_enemy = new int [50], pos_y_enemy = new int [50];

        // size of our enemy
        private int enemy_size_x, enemy_size_y;

        // should our space ship move left/right?
        private bool move_left = false, move_right = false;

        // gets called every time our timer fires
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
                pos_x -= ClientSize.Width / 80;

                if (pos_x < 0) pos_x = 0;
            }
            else if (move_right)
            {
                pos_x += ClientSize.Width / 80;

                if (pos_x > ClientSize.Width - ship_size_x) pos_x = ClientSize.Width - ship_size_x;
            }


            //move our enemies

            framecounter++;
            if (framecounter ==29)
            {
                
                bool moved = false;
                framecounter = 0;
                if (enemy_moving_right == true && pos_x_enemy[pos_x_enemy.Length -1] + ClientSize.Width / 20 + enemy_size_x >= ClientSize.Width) //wenn 
                {
                    enemy_moving_right = false;
                    for (int i = 0; i < pos_x_enemy.Length; i++)
                    {
                        pos_y_enemy[i] += enemy_size_y;

                    }
                    moved = true;
                }
                else if (enemy_moving_right == false && pos_x_enemy[0] - ClientSize.Width / 20 <= 0)
                {
                    for (int i = 0; i < pos_x_enemy.Length; i++)
                    {
                        pos_y_enemy[i] += enemy_size_y;
                        enemy_moving_right = true;
                    }
                    moved = true;
                }
              if (moved == false)
                {
                    int offset = ClientSize.Width / 20;
                    if (enemy_moving_right == false)
                    { offset = -offset; }
                    for (int i = 0; i < pos_x_enemy.Length; i++)
                    {
                        pos_x_enemy[i] += offset;
                    }

                }
              if (pos_y_enemy[pos_y_enemy.Length-1] > ClientSize.Height - 30)
                {
                    MessageBox.Show("You lose!");
                    Environment.Exit(0); }
            }

            



            // create the rectangle (position and size) for DrawImage
            RectangleF rect = new RectangleF(pos_x, pos_y, ship_size_x, ship_size_y);

            // draw our space ship
            g.DrawImage(space_ship, rect);

            RectangleF[] enemy = new RectangleF[pos_x_enemy.Length];

                int zaehler = 0;
            foreach ( RectangleF f in enemy)
            {
                enemy[zaehler] = new RectangleF(pos_x_enemy[zaehler], pos_y_enemy[zaehler], enemy_size_x, enemy_size_y);
                g.DrawImage(enemy_ship, enemy[zaehler]);
                zaehler++;

            }
        }

        public spaceinvaders()
        {
            InitializeComponent();


            framecounter = 0;

            // load our space ship image
            space_ship = Image.FromFile("./../../../img/ship.png");

            // the size of our window
            Size window_client_size = ClientSize;

            // size of our space ship
            ship_size_x = 75;
            ship_size_y = 75;

            // position of our space ship
            pos_x = (ClientSize.Width - ship_size_x) / 2;
            pos_y = (ClientSize.Height - ship_size_y) - ClientSize.Height / 50;

            // load our enemy
            enemy_ship = Image.FromFile("./../../../img/enemy.png");

            // size of the enemy
            enemy_size_x = 30;
            enemy_size_y = 30;

            //position of the enemy
            int x = 0;
            int j = 0;
            for (int i = 0; i<pos_x_enemy.Length;i++)
            {
                if (i%10 == 0)
                {
                    x++;
                    j = 0;

                }
                else { j++; }

                pos_x_enemy[i] = (ClientSize.Width / 20) * (j+1)+10*(j+1);
                pos_y_enemy[i] = ClientSize.Height / 50 + x*30;
                

            }

            // add a message filter so we can capture key presses (and releases)
            Application.AddMessageFilter(this);

            // create the timer that keeps refreshing our window
            int target_fps = 60; // how many FPS our window should have

            refresh_timer = new System.Windows.Forms.Timer();
            refresh_timer.Interval = (int)(1000.0 / ((double)target_fps)); // turn FPS to millisecond interval
            refresh_timer.Tick += new System.EventHandler(timer_event);
            refresh_timer.Start();
        }

        public bool PreFilterMessage(ref Message m)
        {
            // keycodes for button down/up
            const int WM_KEYDOWN = 0x100;
            const int WM_KEYUP = 0x101;

            // check if we hit a button or released one
            if (m.Msg != WM_KEYDOWN && m.Msg != WM_KEYUP) return false;

            // get the keycode
            KeyEventArgs e = new KeyEventArgs(((Keys)((int)((long)m.WParam))) | ModifierKeys);

            Keys keycode = e.KeyCode;

            // check if we pressed A/D/Left/Right
            if (keycode == Keys.A || keycode == Keys.D || keycode == Keys.Left || keycode == Keys.Right)
            {
                bool down = (m.Msg == WM_KEYDOWN);
                bool left = (keycode == Keys.A || keycode == Keys.Left);

                if (left)
                {
                    move_left = down;
                }
                else
                {
                    move_right = down;
                }
            }

            return false;
        }
    }
}