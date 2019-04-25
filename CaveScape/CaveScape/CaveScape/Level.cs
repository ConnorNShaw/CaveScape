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

    class Level
    {
        List<Section> levelSections;
        int tracker;
        bool finished;

        public Level(List<Section> levelSections)
        {
            this.levelSections = levelSections;
            //shuffleSections();
            Console.WriteLine(levelSections.ToString());
            tracker = 0;
            finished = false;
        }

        public void moveToNextSection()
        {
            tracker++;
            if(tracker > levelSections.Count)
            {
                finished = true;
            }
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
            if(!finished)
                levelSections[tracker].Draw(batch);
        }

        public void drawLevel(SpriteBatch batch, Player player)
        {
            if(!finished)
                 levelSections[tracker].drawSection(batch);//, player);
        }

        private void shuffleSections()
        {
            List<Section> randomList = new List<Section>();

            Random random = new Random();
            int randomIndex = 0;
            while(levelSections.Count > 0)
            {
                randomIndex = random.Next(0, levelSections.Count); //Choose a random object in the list
                randomList.Add(levelSections[randomIndex]); //add it to the new, random list
                levelSections.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            for(int i = 0; i < randomList.Count; i++)
            {
                levelSections.Add(randomList[i]);
            }
        }
    }
}
