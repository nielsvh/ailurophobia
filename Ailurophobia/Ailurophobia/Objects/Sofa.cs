using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ailurophobia
{
    class Sofa:Obstacle
    {
        public Sofa(Game game, XmlContentReader.Obstacle readIn)
            : base(game,readIn)
        {
        }
    }
}
