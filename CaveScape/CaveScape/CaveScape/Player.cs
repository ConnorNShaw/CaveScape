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
        int numBat, numSpi;
        public List<int> holdX, holdY, holdBatX, holdBatY, holdBatVelocity, holdSpiderX, holdSpiderY, holdSpiderVelocity;
        public List<bool> dropRock;
        public int velocity;
        public int lives;
        public int speed, gravity, previous, distance, immuneCounter, pauseCounter, timer, oldTimer;
        public Rectangle playerLocat, startLocat;
        public Boolean onGround, startJump, jumping, doubleJump;
        public bool b, latch, b2, damaged, w2, jB, falling, finishedLevel, immune, immuneDamaged, paused, overLadder, oldOverLadder;
        int jTimer, jCounter;
        KeyboardState okb;
        List<Block> passableBlocks;
        List<FallingFloor> fallingBlocks;
        List<Block> ladderBlocks;

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

            holdBatVelocity = new List<int>();
            holdBatX = new List<int>();
            holdBatY = new List<int>();

            holdSpiderVelocity = new List<int>();
            holdSpiderX = new List<int>();
            holdSpiderY = new List<int>();

            velocity = 10;
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
            finishedLevel = false;

            paused = false;
            
            latch = false;
            overLadder = false;
            oldOverLadder = false;
            damaged = false;
            immune = false;
            immuneDamaged = false;
            immuneCounter = 0;
            jTimer = 0;
            jCounter = 0;
            timer = 0;
            oldTimer = 0;
            okb = Keyboard.GetState();
            passableBlocks = new List<Block>();
            fallingBlocks = new List<FallingFloor>();
            ladderBlocks = new List<Block>();
        }

        public void setBats(int numBats)
        {
            numBat = numBats;
        }

        public void setBlocks(Block[,] layout)
        {
            for(int r = 0; r < layout.GetLength(0); r++)
            {
                for(int c = 0; c < layout.GetLength(1); c++)
                {
                    //Allows player to pass through blocks directly next to ladders
                    if (layout[r, c] != null && layout[r, c].getType().Equals("ladder"))
                    {
                        passableBlocks.Add(layout[r, c + 1]);
                        passableBlocks.Add(layout[r, c - 1]);
                        ladderBlocks.Add(layout[r, c]);
                    }
                    
                    if(layout[r, c] != null && layout[r, c].getType().Equals("fall"))
                    {
                        fallingBlocks.Add((FallingFloor)layout[r, c]);
                    }
                }
            }
        }

        public void setSpiders(int numSpiders)
        {
            numSpi = numSpiders;
        }

        public void playerControls(KeyboardState ks, Block[,] layout)
        {
            List<Block> active = getActive(layout);

            timer++;

            if(timer % 60 == 0 && immune)
            {
                immuneCounter--;
            }

            if (timer % 60 == 0 && paused)
            {
                pauseCounter--;
            }

            if(pauseCounter <= 0)
            {
                paused = false;
            }


            w2 = false;
            for (int r = 0; r < layout.GetLength(0); r++)
            {
                for (int c = 0; c < layout.GetLength(1); c++)
                {
                    if (layout[r, c] != null)
                    {

                        if (layout[r, c].type.Equals("fall") && playerLocat.Intersects(layout[r, c].pos))
                        {
                            if(fallingBlocks.Contains(layout[r,c]))
                            {
                                Rectangle rect = layout[r, c].getPos();
                                layout[r, c] = new Space(rect);
                            }
                        }

                        if (layout[r, c].type.Equals("immune") && playerLocat.Intersects(layout[r, c].pos))
                        {
                            Rectangle rect = layout[r, c].getPos();
                            layout[r, c] = new Space(rect);
                            immune = true;
                            immuneCounter += 6;
                        }

                        if (layout[r, c].type.Equals("pause") && playerLocat.Intersects(layout[r, c].pos))
                        {
                            Rectangle rect = layout[r, c].getPos();
                            layout[r, c] = new Space(rect);
                            paused = true;
                            pauseCounter += 6;
                        }

                        if (layout[r, c].type.Equals("heal") && playerLocat.Intersects(layout[r, c].pos))
                        {
                            Rectangle rect = layout[r, c].getPos();
                            layout[r, c] = new Space(rect);
                            lives++;
                            speed = lives * 5;
                        }

                        if (layout[r, c].type.Equals("spider") && holdSpiderX.Count < numSpi)
                        {
                            holdSpiderX.Add(r);
                            holdSpiderY.Add(c);
                            holdSpiderVelocity.Add(5);
                        }

                        if (layout[r, c].type.Equals("spider") && playerLocat.Intersects(layout[r, c].pos) && !damaged)
                        {
                            if (!immune)
                                reduceLife();
                            else
                                reduceImmunity();
                        }

                        if (layout[r, c].type.Equals("bat") && holdBatX.Count < numBat)
                        {
                            holdBatX.Add(r);
                            holdBatY.Add(c);
                            holdBatVelocity.Add(5);
                        }

                        if (layout[r, c].type.Equals("bat") && playerLocat.Intersects(layout[r, c].pos) && !damaged)
                        {
                            if (!immune)
                                reduceLife();
                            else
                                reduceImmunity();
                        }

                        if (layout[r, c].type.Equals("lava") && playerLocat.Intersects(layout[r, c].pos) && !damaged)
                        {
                            if (!immune)
                                reduceLife();
                            else
                                reduceImmunity();
                        }

                        if (layout[r, c].type.Equals("boulder") && playerLocat.Intersects(new Rectangle(layout[r, c].pos.X, layout[r, c].pos.Y, layout[r, c].pos.Width, 10000000)))
                        {
                            dropRock.Add(true);
                            holdX.Add(r);
                            holdY.Add(c);
                        }

                        if (layout[r, c].type.Equals("spike") && playerLocat.Intersects(layout[r, c].pos) && !damaged)
                        {
                            if (!immune)
                                reduceLife();
                            else
                                reduceImmunity();
                        }

                        if (layout[r, c].type.Equals("ladder") && playerLocat.Intersects(layout[r, c].pos) && ks.IsKeyDown(Keys.Space))
                        {
                            latch = true;
                            falling = false;
                            jumping = false;
                            jCounter = 0;
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

                        //if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X, playerLocat.Y - gravity, playerLocat.Width, playerLocat.Height)) && layout[r, c].type.Equals("floor"))
                        //{
                        //    jB = true;
                        //}

                        if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X, playerLocat.Y - gravity, playerLocat.Width, playerLocat.Height)) && layout[r, c].type.Equals("impassable"))
                        {
                            jB = true;
                        }

                        if (layout[r, c].type.Equals("end") && playerLocat.Intersects(layout[r, c].pos))
                        {
                            finishedLevel = true;
                        }

                        if (w2)
                            break;
                    }
                }
            }

            //oldOverLadder = overLadder;
            

            if(!paused)
            {
                for (int k = 0; k < holdBatX.Count; k++)
                {
                    //bat moves
                    layout[holdBatX[k], holdBatY[k]].moveX(holdBatVelocity[k]);
                    for (int r = 0; r < layout.GetLength(0); r++)
                    {
                        for (int c = 0; c < layout.GetLength(1); c++)
                        {
                            //bat changes direction
                            if (layout[r, c] != null && layout[holdBatX[k], holdBatY[k]].pos.Intersects(layout[r, c].pos) && layout[r, c].type.Equals("stop"))
                            {
                                holdBatVelocity[k] *= -1;
                            }
                        }
                    }
                }

                for (int k = 0; k < holdSpiderX.Count; k++)
                {
                    //spider moves
                    layout[holdSpiderX[k], holdSpiderY[k]].moveX(holdSpiderVelocity[k]);
                    for (int r = 0; r < layout.GetLength(0); r++)
                    {
                        for (int c = 0; c < layout.GetLength(1); c++)
                        {
                            //spider goes the other direction
                            if (layout[r, c] != null && layout[holdSpiderX[k], holdSpiderY[k]].pos.Intersects(layout[r, c].pos) && layout[r, c].col.Equals(Color.SaddleBrown))
                            {
                                holdSpiderVelocity[k] *= -1;
                            }
                        }
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
                                    if (layout[r, c] != null && layout[holdX[k], holdY[k]].pos.Intersects(layout[r, c].pos) && layout[r, c].col.Equals(Color.SaddleBrown))
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
                            layout[holdX[k], holdY[k]].pos.Y += 1;
                            if (playerLocat.Intersects(layout[holdX[k], holdY[k]].pos) && !damaged && !immuneDamaged && dropRock[k])
                            {
                                if (!immune)
                                    reduceLife();
                                else
                                    reduceImmunity();
                            }
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
            
            if (jumping && jCounter <= 2)
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
                    if (layout[r, c] != null && layout[r, c].checkScreen())
                    {
                        //if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X, playerLocat.Y + gravity, playerLocat.Width, playerLocat.Height)) && layout[r, c].type.Equals("floor"))
                        if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X, playerLocat.Y + gravity, playerLocat.Width, playerLocat.Height)) && layout[r, c].type.Equals("impassable"))
                        {
                            onGround = true;
                            b5 = true;
                            jumping = false;
                            jCounter = 0;
                            falling = false;
                            break;
                        }
                        else
                        {
                            onGround = false;
                            falling = true;
                        }
                    }
                }
                if (b5)
                    break;
            }

            if ((ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.A)) && !latch)
            {
                bool a = false;
                int mHold = 0;
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null && layout[r, c].checkScreen())
                        {
                            //checks if would hit wall
                            if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X - speed, playerLocat.Y, playerLocat.Width, playerLocat.Height)) && layout[r, c].col.Equals(Color.SaddleBrown))
                            {
                                    a = true;
                                    for(int i = speed - 1; i > 0; i--)
                                    {
                                        if(!(layout[r, c].pos.Intersects(new Rectangle(playerLocat.X - i, playerLocat.Y, playerLocat.Width, playerLocat.Height))))
                                        {
                                        mHold = i;
                                        break;
                                        }
                                    }
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
                else
                {
                    for (int r = 0; r < layout.GetLength(0); r++)
                    {
                        for (int c = 0; c < layout.GetLength(1); c++)
                        {
                            if (layout[r, c] != null)
                            {
                                layout[r, c].pos.X += mHold;
                            }
                        }
                    }

                }
            }

            if ((ks.IsKeyDown(Keys.Right) || ks.IsKeyDown(Keys.D)) && !latch)
            {
                bool a = false;
                int mHold = 0;
                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null && layout[r, c].checkScreen())
                        {
                            //if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X + speed, playerLocat.Y, playerLocat.Width, playerLocat.Height)) && layout[r, c].col.Equals(Color.SaddleBrown))
                            //{
                            if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X + speed, playerLocat.Y, playerLocat.Width, playerLocat.Height)) && layout[r, c].getType().Equals("impassable"))
                            { 
                                a = true;
                                for (int i = speed - 1; i > 0; i--)
                                {
                                    if (!(layout[r, c].pos.Intersects(new Rectangle(playerLocat.X + i, playerLocat.Y, playerLocat.Width, playerLocat.Height))))
                                    {
                                        mHold = i;
                                        break;
                                    }
                                }
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
                else
                {
                    for (int r = 0; r < layout.GetLength(0); r++)
                    {
                        for (int c = 0; c < layout.GetLength(1); c++)
                        {
                            if (layout[r, c] != null)
                            {
                                layout[r, c].pos.X -= mHold;
                            }
                        }
                    }
                }
            }

            if ((ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W)) && !latch && (okb.IsKeyUp(Keys.Up) && okb.IsKeyUp(Keys.W)))
            {
                if (lives > 0)
                {
                    //preps the jump
                    onGround = false;
                    jB = false;
                    jumping = true;
                    jTimer = 0;
                    jCounter++;
                }
            }
            else if ((ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W)) && latch)
            {

                bool flag = false;

                for (int r = 0; r < layout.GetLength(0); r++)
                {
                    for (int c = 0; c < layout.GetLength(1); c++)
                    {
                        if (layout[r, c] != null)
                        {
                            if (layout[r, c].checkScreen())
                            {
                                //moving up
                                if (layout[r, c].pos.Intersects(new Rectangle(playerLocat.X, playerLocat.Y - speed, playerLocat.Width, playerLocat.Height)) && layout[r, c].getType().Equals("impassable") && !passableBlocks.Contains(layout[r, c]))
                                {
                                    flag = true;
                                }
                            }
                            if (playerLocat.Intersects(layout[r, c].pos) && layout[r, c].type.Equals("ladder"))
                            {
                                continue;
                            }
                            else
                            {
                                latch = false;
                                falling = true;
                            }
                        }
                    }
                }

                if(!flag)
                {
                    for (int r = 0; r < layout.GetLength(0); r++)
                    {
                        for (int c = 0; c < layout.GetLength(1); c++)
                        {

                            if (layout[r, c] != null)
                            {
                                layout[r, c].pos.Y += 10;
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
                            layout[r, c].pos.Y -= 5;

                            if (playerLocat.Intersects(layout[r, c].pos) && layout[r, c].type.Equals("ladder"))
                            {
                                continue;
                            }
                            else
                            {
                                latch = false;
                                falling = true;
                            }
                        }
                    }
                }
            }
            okb = ks;
        }

        public void addLife()
        {
            lives++;
            previous = speed;
        }

        public void reduceLife()
        {
            if(!immune)
            {
                lives--;
                if(lives <= 0)
                {
                    speed = 0;
                }
                previous = speed;
                holdX.Clear();
                holdY.Clear();
                dropRock.Clear();
                holdBatX.Clear();
                holdBatY.Clear();
                holdBatVelocity.Clear();
                holdSpiderX.Clear();
                holdSpiderY.Clear();
                holdSpiderVelocity.Clear();
                latch = false;
                damaged = true;
            }
        }
        public void reduceImmunity()
        {
            if(immune)
            {
                previous = speed;
                holdX.Clear();
                holdY.Clear();
                dropRock.Clear();
                holdBatX.Clear();
                holdBatY.Clear();
                immuneDamaged = true;
                if (immuneCounter <= 0)
                {
                    immune = false;
                }
            }
        }

        public Boolean isDead()
        {
            if (lives <= 0)
            {
                return true;
            }
            return false;
        }

        public List<Block> getActive(Block[,] layout)
        {
            List<Block> act = new List<Block>();
            for (int i = 0; i < layout.GetLength(0); i++)
            {
                for (int o = 0; o < layout.GetLength(1); o++)
                {
                    if (layout[i, o] != null && layout[i, o].checkScreen())
                    {
                        act.Add(layout[i, o]);
                    }
                }
            }
            return act;
        }

        public void drawLives(SpriteBatch batch, Texture2D texture)
        {
            int x = 50;
            int y = 50;
            if (lives != 0)
            {
                for (int i = 0; i < lives; i++)
                {
                    Rectangle rect = new Rectangle(x, y, 10, 10);
                    x += 50;
                    batch.Draw(texture, rect, Color.Pink);
                }
            }

            int xI = 50;
            int xI2 = 30;
            int yI = 100;

            if (immuneCounter != 0)
            {
                Rectangle rect = new Rectangle(xI, yI, immuneCounter * xI2, 10);
                batch.Draw(texture, rect, Color.Red);
            }

            if (pauseCounter != 0)
            {
                Rectangle rect = new Rectangle(xI, yI + 50, pauseCounter * xI2, 10);
                batch.Draw(texture, rect, Color.Purple);
            }
        }
    }
}
