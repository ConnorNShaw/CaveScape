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

        int height;
        int width;
        SpriteBatch spriteBatch;
        

        string[,] text;

        Block[,] blocks;

        public Section(string[,] a)
        {
            height = a.GetLength(0);
            width = a.GetLength(1);
            
            blocks = new Block[height, width];

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

                            blocks[i, o] = new Spike(hold);
                            break;
                        case "R":

                            break;
                        case "w":

                            break;
                        case "l":

                            break;
                        case "P":

                            break;
                        case "B":
                            blocks[i, o] = new Bat(hold);
                            break;
                        case "S":

                            break;
                        case "h":

                            break;
                        case "t":

                            break;
                        case "i":

                            break;
                        case "|":

                            break;
                        case "/":

                            break;
                        case "-":

                            break;
                        default:

                            break;




                    }

                    hold.X += 50;

                }
                hold.Y += 50;
                hold.X = 0;
            }

        }

        public void Draw()
        {
            
            for (int i = 0; i < height; i++)
            {
                for (int o = 0; o < height; o++)
                {
                    spriteBatch.Begin();
                    
                    spriteBatch.End();

                }
            }
        }

    }
}
