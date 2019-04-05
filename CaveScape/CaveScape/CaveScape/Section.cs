﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CaveScape
{
    class Section
    {

        string[,] layout, a;
        int width, height, x, y, vGrid, hGrid;
        bool created, hasBeenReset;

        string[,] text;

        Block[,] blocks, startBlocks;
        Player player;
        Texture2D texture;

        public Section(string[,] a, int sectionWidth, int sectionHeight, Texture2D texture)
        {
            height = a.GetLength(0);
            width = a.GetLength(1);

            hasBeenReset = false;
            
            blocks = new Block[height, width];
            x = 0;
            y = 0;
            //vGrid = 50;
            //hGrid = 50;
            player = new Player(new Rectangle(500, 100, 50, 50));
            this.texture = texture;

            this.a = a;

            create(a, hasBeenReset);

        }

        public void create(string[,] a, bool hasBeenReset)
        {
            //starts beyond the screen to make the bottom the focus
            Rectangle hold = new Rectangle(0, -50 * height + 700, 50, 50);
            for (int i = 0; i < height; i++)
            {
                for (int o = 0; o < width; o++)
                {
                    switch (a[i, o]) //polymorphically adds object to block array to retain individual properties, based on character given
                    {
                        case "s":
                            blocks[i, o] = new Spike(hold);
                            break;
                        case "R":
                            blocks[i, o] = new Boulder(hold);
                            break;
                        case "w":
                            blocks[i, o] = new Water(hold);
                            break;
                        case "l":
                            blocks[i, o] = new Lava(hold);
                            break;
                        case "P":
                            if(!hasBeenReset)
                            {
                                player = new Player(hold);
                            }
                            player.playerLocat = hold;
                            blocks[i, o] = new Space(hold);
                            break;
                        case "B":
                            blocks[i, o] = new Bat(hold);
                            break;
                        case "S":
                            blocks[i, o] = new Spider(hold);
                            break;
                        case "h":
                            blocks[i, o] = new HealShroom(hold);
                            break;
                        case "t":
                            blocks[i, o] = new TimeOrb(hold);
                            break;
                        case "i":
                            blocks[i, o] = new Immune(hold);
                            break;
                        case "|":
                            blocks[i, o] = new Wall(hold);
                            break;
                        case "/":
                            blocks[i, o] = new Ladder(hold);
                            break;
                        case "-":
                            blocks[i, o] = new Floor(hold);
                            break;
                        case " ":
                            blocks[i, o] = new Space(hold);
                            break;
                    }

                    hold.X += 50;
                    //adjust the setting rectangle
                }
                hold.Y += 50;
                hold.X = 0;
            }
            startBlocks = (Block[,])blocks.Clone();
        }

        //private void createBlock(int r, int c, bool movement)
        //{
        //    Rectangle rectangle = new Rectangle((c * hGrid), (r * vGrid), hGrid, vGrid);
        //    Block block = new Block(rectangle);
        //}

        public void drawSection(SpriteBatch batch)//, Player player)
        {
            KeyboardState ks = Keyboard.GetState();

            if(player.damaged && !player.isDead())
            {
                hasBeenReset = true;
                create(a, hasBeenReset);
                player.damaged = false;
            }

            for (int i = 0; i < height; i++)
            {
                for (int o = 0; o < width; o++)
                {
                    if(blocks[i, o] != null)
                         batch.Draw(texture, blocks[i,o].pos, blocks[i,o].getCol());
                    //draws each block with its properties
                }
            }
            
            player.playerControls(ks, blocks);
            batch.Draw(texture, player.playerLocat, Color.White);
            player.drawLives(batch, texture);

        }

        public void Draw(SpriteBatch batch)
        {

        }

    }
}
