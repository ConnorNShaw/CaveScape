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

        string[,] layout;
        int width, height, x, y, vGrid, hGrid;
        bool created;

        string[,] text;

        Block[,] blocks;

        Texture2D texture;

        public Section(string[,] a, int sectionWidth, int sectionHeight, Texture2D texture)
        {
            height = a.GetLength(0);
            width = a.GetLength(1);
            
            blocks = new Block[height, width];
            x = 0;
            y = 0;
            vGrid = 50;
            hGrid = 50;

            this.texture = texture;

            create(a);



        }



        public void create(string[,] a)
        {

            Rectangle hold = new Rectangle(0, 0, 50, 50);
            for (int i = 0; i < height; i++)
            {
                for (int o = 0; o < height; o++)
                {
                    switch (a[i, o])
                    {
                        case "s":

                            blocks[i, o] = new Spike(hold, Color.Red);
                            break;
                        case "b":
                            createBlock(i, o, false, Color.White);
                            break;
                        case "w":
                            createBlock(i, o, false, Color.White);
                            break;
                        case "l":
                            createBlock(i, o, false, Color.White);
                            break;
                        case "P":
                            createBlock(i, o, false, Color.White);
                            break;
                        case "B":
                            blocks[i, o] = new Bat(hold, Color.Black);
                            break;
                        case "S":
                            createBlock(i, o, false, Color.White);
                            break;
                        case "h":
                            createBlock(i, o, false, Color.White);
                            break;
                        case "t":
                            createBlock(i, o, false, Color.White);
                            break;
                        case "i":
                            createBlock(i, o, false, Color.White);
                            break;
                        case "|":
                            createBlock(i, o, false, Color.White);
                            break;
                        case "/":
                            createBlock(i, o, false, Color.White);
                            break;
                        case "-":
                            createBlock(i, o, false, Color.White);
                            break;
                        default:

                            createBlock(i, o, false, Color.Transparent);
                            //blocks[i, o] = new Block();
                            break;
                    }

                    hold.X += 50;

                }
                hold.Y += 50;
                hold.X = 0;
            }
        }

        private void createBlock(int r, int c, bool movement, Color color)
        {
            Rectangle rectangle = new Rectangle((c * hGrid), (r * vGrid), hGrid, vGrid);
            Block block = new Block(rectangle, color);
            blocks[r, c] = block;
        }

        public void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < height; i++)
            {
                for (int o = 0; o < height; o++)
                {
                    batch.Draw(texture, blocks[i,o].getPos(), blocks[i,o].getColor());
                }
            }
        }
    }
}
