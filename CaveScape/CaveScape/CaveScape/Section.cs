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
        int height;
        int width;

        public Section(string[,] layout, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.layout = layout;
        }

        public Section(int h, int w)
        {
            height = h;
            width = w;
        }







    }
}
