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
        bool b;
        int jTimer;

        public Player(Rectangle r)
        {
            playerLocat = r;
            lives = 3;
            speed = 20;

            gravity = 10;

            onGround = true;
            startJump = false;
            jumping = false;
            doubleJump = false;
            previous = r;
            b = false;
            jTimer = 0;
        }

        public void playerControls(KeyboardState ks, Block[,] layout)
        {
            if (jumping && !onGround)
            {
                b = false;
                jTimer++;
                //increment timer for jump control
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (playerLocat.Intersects(layout[r, c].pos) && layout[r, c].type.Equals("floor"))
                        {
                            onGround = true;
                            jumping = false;
                            //if hitting floor, is no longer jumping and then breaks
                            b = true;
                        for (int i = 0; i < layout.GetLength(0); i++)
                        {
                            for (int o = 0; o < layout.GetLength(1); o++)
                            {
                                //moves blocks so the player is not stuck inside
                                layout[i, o].pos.Y += gravity;
                            }
                        }
                                break;
                        }
                    }
                    if (b)
                        break;
                }
            }
            if (jumping)
            {
                //creates a smoother jump by slowing at the top
                //should make sure floor is still visible when jumping
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null)
                        {
                            if (jTimer <= 15)
                                layout[r, c].pos.Y += gravity;
                            else if (jTimer > 15 && jTimer < 20)
                                layout[r, c].pos.Y += gravity / 2;
                            else if (jTimer > 20 && jTimer < 25)
                                layout[r, c].pos.Y += gravity / 2;
                            else if (jTimer >= 25)
                                layout[r, c].pos.Y -= gravity;
                        }
                    }
                }
            }
            else if (!onGround)
            {
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null)
                        {
                         //falls if not on ground
                                layout[r, c].pos.Y -= gravity;
                        }
                    }
                }
            }



           
            if (ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.D))
            {
                bool a = false;
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null)
                        {
                            //checks if would hit wall
                            if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X - speed, playerLocat.Y, playerLocat.Width, playerLocat.Height)) && layout[r, c].type.Equals("wall"))
                            {
                                a = true;
                            }
                            if (a)
                                break;
                        }
                    }
                    if (a)
                        break;
                }

                if (!a) //If it is not going to hit wall, it moves the blocks accordingly 
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
            }

            if (ks.IsKeyDown(Keys.Right) || ks.IsKeyDown(Keys.A))
            {
                bool a = false;
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null)
                        {
                            if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X + speed, playerLocat.Y, playerLocat.Width, playerLocat.Height)) && layout[r, c].type.Equals("wall"))
                            {
                                a = true;
                            }
                            if (a)
                                break;
                        }
                    }
                    if (a)
                        break;
                }

                if (!a)
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
            }
               
            

            if ((ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W)) && jumping == false)
            {
                //preps the jump
                onGround = false;
                jumping = true;
                jTimer = 0;
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

        public void drawLives(SpriteBatch batch, Texture2D texture)
        {
            int x = 50;
            int y = 50;

            for (int i = 0; i < lives; i++)
            {
                Rectangle rect = new Rectangle(x, y, 10, 10);
                x += 50;
                batch.Draw(texture, rect, Color.Pink);
            }
        }

    }
}
