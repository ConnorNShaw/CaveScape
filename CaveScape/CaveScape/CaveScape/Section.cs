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
            for (int i = 0; i < height; i++)
            {
                for (int o = 0; o < height; o++)
                {

                }
            }

        }




    }
}
