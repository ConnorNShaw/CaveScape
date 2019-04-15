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
        public List<int> holdX, holdY;
        public List<bool> dropRock;
        public int lives;
        public int speed, gravity, previous;
        public Rectangle playerLocat, startLocat;
        public Boolean onGround, startJump, jumping, doubleJump;
        public bool b, latch, b2, damaged, w2, jB, falling;
        int jTimer;

        public Player(Rectangle r)
        {
            playerLocat = r;
            startLocat = playerLocat;
            lives = 3;
            speed = 20;
            previous = speed;

            holdX = new List<int>();
            holdY = new List<int>();
            dropRock = new List<bool>();

            gravity = 10;
            falling = false;
            onGround = true;
            startJump = false;
            jumping = false;
            doubleJump = false;
            b = false;
            b2 = false;
            jB = false;
            w2 = false;

            latch = false;
            damaged = false;
            jTimer = 0;
        }

        public void playerControls(KeyboardState ks, Block[,] layout)
        {
            w2 = false;
            for (int r = 0; r < layout.GetLength(0); r++)
            {
                for (int c = 0; c < layout.GetLength(1); c++)
                {
                    if (layout[r, c].type.Equals("lava") && playerLocat.Intersects(layout[r, c].pos) && !damaged)
                    {
                        reduceLife();
                    }

                    if (layout[r, c].type.Equals("boulder") && playerLocat.Intersects(new Rectangle(layout[r, c].pos.X, layout[r, c].pos.Y, layout[r, c].pos.Width, 10000000)))
                    {
                        dropRock.Add(true);
                        holdX.Add(r);
                        holdY.Add(c);
                    }

                    if (layout[r, c].type.Equals("spike") && playerLocat.Intersects(layout[r, c].pos) && !damaged)
                    {
                        reduceLife();
                    }

                    if (layout[r, c].type.Equals("ladder") && playerLocat.Intersects(layout[r, c].pos) && ks.IsKeyDown(Keys.Space))
                    {
                        latch = true;
                        falling = false;
                        jumping = false;
                        b2 = !b2;
                        onGround = false;
                    }
                    else if (ks.IsKeyUp(Keys.Space))
                    {
                        //(playerLocat.Y + playerLocat.Height > layout[r, c].pos.Y && playerLocat.Y < layout[r, c].pos.Y)
                        //(playerLocat.Intersects(layout[r, c].pos) && layout[r, c].col.Equals(Color.Transparent))
                        latch = false;
                        falling = true;
                    }

                    if (layout[r, c].type.Equals("water") && playerLocat.Intersects(layout[r, c].pos))
                    {
                        w2 = true;
                        break;
                    }

                    if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X, playerLocat.Y - gravity, playerLocat.Width, playerLocat.Height)) && layout[r, c].type.Equals("floor"))
                    {
                        jB = true;
                    }
                    if (w2)
                        break;
                }
            }

            for (int k = 0; k < dropRock.Count; k++)
            {
                if (dropRock[k])
                {
                    bool a = false;
                    for (int r = 0; r < layout.GetLength(0); r++)
                    {
                        for (int c = 0; c < layout.GetLength(1); c++)
                        {
                            if (layout[r, c] != null)
                            {
                                //checks if boulder reaches the floor and stops the rock from falling through the floor
                                if (layout[holdX[k], holdY[k]].pos.Intersects(layout[r, c].pos) && layout[r, c].col.Equals(Color.SaddleBrown))
                                {
                                    a = true;
                                    dropRock[k] = false;
                                }
                            }
                            if (a)
                                break;
                        }
                        if (a)
                            break;
                    }
                    if (!a) //rock falls 
                    {
                        layout[holdX[k], holdY[k]].pos.Y += gravity/2;
                        if (playerLocat.Intersects(layout[holdX[k], holdY[k]].pos) && !damaged && dropRock[k])
                        {
                            reduceLife();
                        }
                    }
                }
            }

            if (w2)
                speed = 5;
            else
                speed = previous;

            if (!onGround && !latch)
                falling = true;
            else
                falling = false;

            if (jumping)
            {
                falling = false;
                //creates a smoother jump by slowing at the top
                //should make sure floor is still visible when jumping
                jTimer++;
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null)
                        {
                            if (jTimer <= 22)
                            {
                                if (!jB)
                                    layout[r, c].pos.Y += gravity;
                            }
                            else
                                falling = true;
                        }
                    }
                }
            }

            if (falling)
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

            bool b5 = false;
            for (int r = 0; r < layout.GetLength(0); r++)
            {
                for (int c = 0; c < layout.GetLength(1); c++)
                {
                    if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X, playerLocat.Y + gravity, playerLocat.Width, playerLocat.Height)) && layout[r, c].type.Equals("floor"))
                    {
                        onGround = true;
                        b5 = true;
                        jumping = false;
                        falling = false;
                        break;
                    }
                    else
                    {
                        onGround = false;
                        falling = true;
                    }
                }
                if (b5)
                    break;
            }

            if ((ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.A)) && !latch)
            {
                bool a = false;
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null)
                        {
                            //checks if would hit wall
                            if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X - speed, playerLocat.Y, playerLocat.Width, playerLocat.Height)) && layout[r, c].col.Equals(Color.SaddleBrown))
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

            if ((ks.IsKeyDown(Keys.Right) || ks.IsKeyDown(Keys.D)) && !latch)
            {
                bool a = false;
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null)
                        {
                            if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X + speed, playerLocat.Y, playerLocat.Width, playerLocat.Height)) && layout[r, c].col.Equals(Color.SaddleBrown))
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

            if ((ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W)) && jumping == false && !latch)
            {
                //preps the jump
                onGround = false;
                jB = false;
                jumping = true;
                jTimer = 0;
            }
            else if ((ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W)) && latch)
            {
                bool a = false;
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null)
                        {
                            if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X, playerLocat.Y - speed, playerLocat.Width, playerLocat.Height)) && layout[r, c].col.Equals(Color.SaddleBrown))
                            {
                                a = true;
                            }
                            if (a)
                            {
                                break;
                            }
                        }
                    }
                    if (a)
                        break;
                }
                //note, fix the wall spacing thing
                if (!a)
                {
                    for (int r = 0; r < layout.GetLength(0); r++)
                    {
                        for (int c = 0; c < layout.GetLength(1); c++)
                        {
                            if (layout[r, c] != null)
                            {
                                //moving up
                                layout[r, c].pos.Y += speed;
                            }
                        }
                    }
                }
            }
            else if ((ks.IsKeyDown(Keys.Down) || ks.IsKeyDown(Keys.S)) && latch)
            {
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null)
                        {
                            //moving down
                            layout[r, c].pos.Y -= speed;
                        }
                        if ((playerLocat.Intersects(layout[r, c].pos) && layout[r, c].col.Equals(Color.Transparent)))
                        {
                            latch = false;
                            falling = true;
                        }
                    }
                }
            }
        }

        public void addLife()
        {
            lives++;
            speed += 5;
            previous = speed;
        }

        public void reduceLife()
        {
            lives--;
            speed -= 5;
            previous = speed;
            holdX.Clear();
            holdY.Clear();
            dropRock.Clear();
            damaged = true;
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
