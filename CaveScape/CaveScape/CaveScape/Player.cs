using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CaveScape
{
    class Player
    {
        public int lives;
        public int speed, gravity;
        public Rectangle playerLocat;
        public Texture2D playerSprite;

        public Player(Rectangle r)
        {
            playerLocat = r;
            playerSprite = text;
            lives = 3;
            speed = 15;
            gravity = speed;
        }

        public void playerControls(KeyboardState ks, Block[,] layout)
        {
            //for(int r = 0; r < layout.GetLength(0); r++)
            //{
            //    for(int c = 0; c < layout.GetLength(1); c++)
            //    {
            //        if (layout[r, c].getType().Equals("floor"))
            //        {
            //            if (!playerLocat.Intersects(layout[r, c].getPos()))
            //            {
            //                playerLocat.Y += gravity;
            //            }
            //        }
            //    }
            //}
            

            if (ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.D))
            {
                playerLocat.X -= speed;
            }
            if (ks.IsKeyDown(Keys.Right) || ks.IsKeyDown(Keys.A))
            {
                playerLocat.X += speed;
            }
        }

        public void addLife()
        {
            lives++;
            speed += 5;
        }

        public void reduceLife()
        {
            lives--;
            speed -= 5;
        }

        public Boolean isDead()
        {
            if (lives <= 0)
            {
                return true;
            }
            return false;
        }
    }
}
