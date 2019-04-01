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
        public int speed;
        public Rectangle playerLocat;
        public Texture2D playerSprite;
        public Boolean onGround;
        public Boolean startJump;
        public Boolean jumping;
        public Boolean doubleJump;
        public Rectangle previous;

        public Player(Rectangle r, Texture2D text)
        {
            playerLocat = r;
            playerSprite = text;
            lives = 3;
            speed = 15;

            onGround = true;
            startJump = false;
            jumping = false;
            doubleJump = false;
            previous = r;
        }

        public void playerControls(KeyboardState ks)
        {
            if (onGround == true)
            {
                previous = playerLocat;
            }
            if (ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.D))
            {
                playerLocat.X -= speed;
            }
            if (ks.IsKeyDown(Keys.Right) || ks.IsKeyDown(Keys.A))
            {
                playerLocat.X += speed;
            }
            if ((ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W)) && jumping == false)
            {
                onGround = false;
            }
            if (onGround == false)
            {
                playerLocat.Y -= speed;
                if (playerLocat.Y > previous.Y - 150)
                {
                    jumping = true;
                }
                else
                {
                    playerLocat.Y -= speed;
                }
                if (doubleJump == false && playerLocat.Y > previous.Y - 250 && (ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W)))
                {
                    doubleJump = true;
                }
                if (doubleJump == true)
                {
                    playerLocat.Y -= speed;
                }
                else
                {
                    doubleJump = true;
                    playerLocat.Y += speed;
                    if (playerLocat.Y >= previous.Y)
                    {
                        playerLocat.Y += 0;
                        onGround = true;
                        jumping = false;
                        doubleJump = false;
                    }
                }
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
