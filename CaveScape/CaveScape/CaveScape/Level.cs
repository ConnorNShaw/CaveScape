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

    class Level
    {
        List<Section> levelSections;
        int tracker;

        public Level(List<Section> levelSections)
        {
            this.levelSections = levelSections;
        }

        public void moveToNextSection()
        {
            tracker++;
        }

        public bool levelFinished()
        {
            if(tracker > levelSections.Count)
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch batch)
        {
            levelSections[tracker].Draw(batch);
        }

    }
}
