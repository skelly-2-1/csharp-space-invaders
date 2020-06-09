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

        //test
        private int j = 0;

        //hit counter
        private int h = 0;

        // frame counter for enemy movement
        private int framecounter;

        // bool for enemy movement left/right
        private bool enemy_moving_right = true;

        // image of our space ship
        private Image space_ship;

        // image of our enemy
        private Image enemy_ship;

        //image of the laser bullet
        private Image laser_bullet;

        // position of our space ship
        private int pos_x, pos_y;

        // size of our space ship
        private int ship_size_x, ship_size_y;

        // position of our enemy
        private int[] pos_x_enemy = new int[50], pos_y_enemy = new int[50];

        // size of our enemies
        private int enemy_size_x, enemy_size_y;

        // if the enemy is dead
        private bool[] dead = new bool[50];

        //position of the bullet
        private int pos_x_bullet, pos_y_bullet;

        //Size of the bullet
        private int laser_bullet_size_x, laser_bullet_size_y;

        // should our space ship move left/right?
        private bool move_left = false, move_right = false;

        //tells if we shoot
        private bool shoot = false;

        // tells if the bullet is still on the way
        private bool bullettravel = false;

        //tells if we won
        private bool win = false;

        // gets called every time our timer fires
        private void timer_event(Object source, EventArgs e)
        {
            // refreshes the window, forcing it to re-draw (so OnPaint gets called again)
            if ( win == false) Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // call windows forms onpaint
            base.OnPaint(e);

            // our graphics object
            Graphics g = e.Graphics;

            //checks if we won
            for (int i = 0; i < pos_x_enemy.Length; i++)
            {
                if (dead[i] == true)
                { h++; }
                if (h == 50)
                { MessageBox.Show("you win!"); win = true; break; }
                
            }
            h = 0;

            //checks if an enemy was hit
            for (int i = 0; i < pos_x_enemy.Length; i++)
            {
                if (pos_x_bullet >= pos_x_enemy[i] && pos_x_bullet <= (pos_x_enemy[i] + ClientSize.Width / 17) &&
                    pos_y_bullet >= pos_y_enemy[i] && pos_y_bullet <= (pos_y_enemy[i] + ClientSize.Height / 17
                    )&& dead[i] == false)
                {
                    dead[i] = true;
                    j = 0;
                        shoot = false;
                    bullettravel = false;
                    pos_x_bullet = 0;
                    pos_y_bullet = 0;
                    break;

                }


            }


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

            // move our enemies every 30th frame
            framecounter++;

            if (framecounter == 29)
            {
                // reset the frame counter
                framecounter = 0;

                // to keep track if our enemies already moved down, we created this variable.
                // its purpose is to stop left/right movement if enemies already moved down.
                bool moved = false;

                // check if the next right/left movement would put our enemies outside the screen
                // and if so, reverse the movement, move enemies downwards and indicate that our
                // enemies already moved downwards
                if ((enemy_moving_right && pos_x_enemy[pos_x_enemy.Length - 1] + ClientSize.Width / 20 + enemy_size_x >= ClientSize.Width) ||
                    (!enemy_moving_right && pos_x_enemy[0] - ClientSize.Width / 20 <= 0))
                {
                    // reverse movement
                    enemy_moving_right = !enemy_moving_right;

                    // move all enemies downwards
                    for (int i = 0; i < pos_x_enemy.Length; i++) pos_y_enemy[i] += enemy_size_y;

                    // indicate that our enemies already moved downwards
                    moved = true;
                }

                // if our enemies didn't get moved downwards already, move them to the left/right
                if (!moved)
                {
                    // the offset for movement (default for right-side movement)
                    int offset = ClientSize.Width / 20;

                    // enemies should be moving left, negate the offset
                    if (!enemy_moving_right) offset = -offset;

                    // add the offset to all enemies' X axis
                    for (int i = 0; i < pos_x_enemy.Length; i++) pos_x_enemy[i] += offset;
                }
                // also check if the enemies hit our ship, in which case we lose
                else if (moved && pos_y_enemy[pos_y_enemy.Length - 1] > ClientSize.Height - enemy_size_y)
                {
                    MessageBox.Show("You lose!");
                    Environment.Exit(0);
                }
            }

            // create the ships rectangle (position and size) for DrawImage
            RectangleF rect = new RectangleF(pos_x, pos_y, ship_size_x, ship_size_y);

            // draw our space ship
            g.DrawImage(space_ship, rect);

            // draw the enemies
            for (int i = 0; i < pos_x_enemy.Length; i++)
            {
                if (dead[i] == false)
                { g.DrawImage(enemy_ship, new RectangleF(pos_x_enemy[i], pos_y_enemy[i], enemy_size_x, enemy_size_y)); }
                else { }
                    }

            //draws the bullet
            if (shoot == true)
            {
                if( j == 0)
                        {
                    // startposition of the bullet
                    pos_x_bullet = pos_x + ship_size_x / 2 - 2;
                    pos_y_bullet = pos_y - 2;
                    j = 1;
                }

                RectangleF rect2 = new RectangleF(pos_x_bullet, pos_y_bullet, laser_bullet_size_x, laser_bullet_size_y);
                g.DrawImage(laser_bullet, rect2);
                
                bullettravel = true;
                pos_y_bullet -= 25;

                if (pos_y_bullet <= 0)
                { shoot = false;
                    bullettravel = false;
                    j = 0;
                }

            }
        }

        public spaceinvaders()
        {
            InitializeComponent();

            // framecounter should start at 0 (didn't draw a frame yet)
            framecounter = 0;



            // load our space ship image
            space_ship = Image.FromFile("./../../../img/ship.png");

            // size of our space ship
            ship_size_x = (int)((double)ClientSize.Height / 8.1);
            ship_size_y = (int)((double)ClientSize.Height / 8.1);

            // position of our space ship
            pos_x = (ClientSize.Width - ship_size_x) / 2;
            pos_y = (ClientSize.Height - ship_size_y) - ClientSize.Height / 50;

            // load the laser bullet image
            laser_bullet = Image.FromFile("./../../../img/laser.png");

            // size of the laser bullet
            laser_bullet_size_x = 5;
            laser_bullet_size_y = 10;



            // load our enemy
            enemy_ship = Image.FromFile("./../../../img/enemy.png");

            // size of the enemy
            enemy_size_x = ClientSize.Width / 17;
            enemy_size_y = ClientSize.Height / 17;

            // set up the position of our enemies
            int x = 0; // keeps track of how many 10's we hit (we only want 10 enemies in a single row)
            int j = 0; // goes from 0-9 instead of 0-pos_x_enemy.Length, so that we can scale the X axis properly

            for (int i = 0; i < pos_x_enemy.Length; i++)
            {
                // check if i is a multiplier of 10
                if (i % 10 == 0)
                {
                    // increase x by one (if i isn't 0)
                    if (i > 0) x++;

                    // set j to 0 (since it should only go from 0-9)
                    j = 0;
                }
                else
                {
                    // i is not a multiplier of 10, simply increase j by one
                    j++;
                }

                pos_x_enemy[i] = (ClientSize.Width / 20) * (j + 1) + (ClientSize.Width / 50) * (j + 1);
                pos_y_enemy[i] = ClientSize.Height / 50 + x * enemy_size_y;
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
            if (keycode == Keys.A || keycode == Keys.D || keycode == Keys.Left || keycode == Keys.Right || keycode == Keys.Space)
            {
                bool down = (m.Msg == WM_KEYDOWN);
                bool left = (keycode == Keys.A || keycode == Keys.Left);
                bool shit = (keycode == Keys.Space);

                if (shit)
                { shoot = true; }
                
                else if (left)
                {
                    move_left = down;

                    if (down && move_right) move_right = false;
                }
                else if (!left)
                {
                    move_right = down;

                    if (down && move_left) move_left = false;
                }
            }

            return false;
        }
    }
}