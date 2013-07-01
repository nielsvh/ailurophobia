using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ailurophobia
{
    class SpawnPoint:Obstacle
    {
        public GameTimer spawnTimer;
        public SpawnPoint(Game game, XmlContentReader.Obstacle readIn)
            : base(game,readIn)
        {
            spawnTimer = new GameTimer(this.Game, GameTimer.TimerType.SPAWN, this.Location);
        }

        public override void Initialize()
        {
            spawnTimer.Interval = new TimeSpan(100000000);
            this.Game.Components.Add(spawnTimer);
            base.Initialize();
        }
    }
}
