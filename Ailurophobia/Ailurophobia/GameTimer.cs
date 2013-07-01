using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ailurophobia
{
    public delegate void HealthTickHandler(object obj, ChangeHealthEventArgs e);
    public delegate void SpawnTickHandler(object obj, SpawnEventArgs e);
    public class ChangeHealthEventArgs : EventArgs
    {
        private readonly int theNumber;
        public int TheNumber
        {
            get { return theNumber; }
        }

        public ChangeHealthEventArgs(int theNumber)
        {
            this.theNumber = theNumber;
        }

    }
    public class SpawnEventArgs : EventArgs
    {
        public readonly Vector2 theVector;

        public SpawnEventArgs(Vector2 vec)
        {
            theVector = vec;
        }
    }

    class GameTimer : GameComponent
    {
        public enum TimerType { HIT_TIMER, SPAWN };
        TimerType timerType;
        Vector2 location;
        TimeSpan interval = new TimeSpan(0, 0, 1);
        TimeSpan lastTick = new TimeSpan();

        public event HealthTickHandler HTick;
        public event SpawnTickHandler STick;

        public TimeSpan Interval
        {
//            get { return interval; }
            set { interval = value; }
        }

        public GameTimer(Game game, TimerType type, int healthStep)
            : base(game)
        {
            this.timerType = type;
        }
        public GameTimer(Game game, TimerType type, Vector2 location)
            : base(game)
        {
            this.timerType = type;
            this.location = location;
        }
        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - lastTick >= interval)
            {
                ChangeHealthEventArgs he;
                SpawnEventArgs se;
                switch (timerType)
                {
                    case TimerType.HIT_TIMER:
                        he = new ChangeHealthEventArgs(0);

                        if (HTick != null)
                            HTick(this, he);
                        break;
                    case TimerType.SPAWN:
                        se = new SpawnEventArgs(location);

                        if (STick != null)
                            STick(this, se);
                        break;
                    default:
                        break;
                }

                lastTick = gameTime.TotalGameTime;
            }
        }
    }
}