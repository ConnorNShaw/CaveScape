using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveScape
{

    class Level
    {
        List<Section> levelSections;

        public Level(List<Section> levelSections)
        {
            this.levelSections = levelSections;
        }

    }
}
