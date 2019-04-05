﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;


namespace CaveScape
{
    class Player
    {
        public int lives;
        public int speed, gravity;
        public Rectangle playerLocat;
        public Boolean onGround, startJump, jumping, doubleJump;
        public Rectangle previous;

        public Player(Rectangle r)
        {
            playerLocat = r;
            lives = 3;
            speed = 20;

            gravity = 1;

            onGround = true;
            startJump = false;
            jumping = false;
            doubleJump = false;
            previous = r;
        }

        public void playerControls(KeyboardState ks, Block[,] layout)
        {
            if (jumping)
            {
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c].type.Equals("floor"))
                        {
                            if (playerLocat.Intersects(layout[r, c].getPos()))
                            {
                                onGround = true;
                                jumping = false;
                            }
                            onGround = false;
                            jumping = true;
                        }
                    }
                }
            }
            if (!onGround)
            {
                playerLocat.Y += gravity;
            }
            if (ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.D))
            {
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null)
                        {
                            layout[r, c].pos.X += speed;
                        }
                    }
                }
            }
            if (ks.IsKeyDown(Keys.Right) || ks.IsKeyDown(Keys.A))
            {
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null)
                        {
                            layout[r, c].pos.X -= speed;
                        }
                    }
                }
            }
            if ((ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W)) && jumping == false)
            {
                playerLocat.Y -= 100;
                onGround = false;
                jumping = true;
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
