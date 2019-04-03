using System;
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
            //vGrid = 50;
            //hGrid = 50;

            this.texture = texture;

            create(a);



        }



        public void create(string[,] a)
        {

            Rectangle hold = new Rectangle(0, -50 * height + 900, 50, 50);
            for (int i = 0; i < height; i++)
            {
                for (int o = 0; o < height; o++)
                {
                    switch (a[i, o])
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
                            //new player()
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
                        default: //dance
                            blocks[i, o] = new Block(hold, Color.Transparent, "blank");
                            break;
                    }

                    hold.X += 50;

                }
                hold.Y += 50;
                hold.X = 0;
            }
        }

        //private void createBlock(int r, int c, bool movement)
        //{
        //    Rectangle rectangle = new Rectangle((c * hGrid), (r * vGrid), hGrid, vGrid);
        //    Block block = new Block(rectangle);
        //}

        public void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < height; i++)
            {
                for (int o = 0; o < height; o++)
                {
                    if(blocks[i, o] != null)
                         batch.Draw(texture, blocks[i,o].getPos(), blocks[i,o].getCol());

                }
            }
        }
    }
}
