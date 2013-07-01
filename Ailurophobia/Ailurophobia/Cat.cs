using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ailurophobia
{
    class Cat:Character
    {
        private static Vector2 centroid = Vector2.Zero;
        private static List<Cat> flock = new List<Cat>();
        private static Vector2 averageHeading = Vector2.Zero;
        private const int MAX_SPEED = 2;
        private List<Delegate> flocking;

        private delegate void FlockingFunction();

        public Cat(Game game, int x, int y, int mouseX, int mouseY):base(game,x,y)
        {
            flock.Add(this);
        }

        public override void Initialize()
        {
            flocking = new List<Delegate>();
            //flocking.Add();
            drawRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 10, 10);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            UpdatePos();
            setFlockVars();
            base.Update(gameTime);
        }

        public void setFlockVars()
        {
            /*averageHeading = new Point( );
			for (var i:int=0;i<flock.length;i++)
			{	
				var temp:Point = flock[i].getVelocity( );
				temp.normalize(1);	
				averageHeading.offset(temp.x, temp.y);
			}
			centroid = new Point( );

		for (var j:int=0;j<flock.length;j++)
			{		
				centroid.offset(flock[j].x, flock[j].y);
			}
			centroid = new Point(centroid.x/flock.length, centroid.y/flock.length);	*/

            averageHeading = new Vector2();
            for (int i = 0; i < flock.Count; i++)
            {
                Vector2 tmp = flock[i].Velocity;
                tmp.Normalize();
            }
            centroid = Vector2.Zero;

            for (int j = 0; j < flock.Count; j++)
            {
                
            }
            centroid = new Vector2(centroid.X / flock.Count, centroid.Y / flock.Count);
        }

        internal void UpdatePos()
        {
            drawRect.X = 400 + (int)Location.X;// +(int)Mouse.sLocation.X;
            drawRect.Y = 300 + (int)Location.Y;//+ (int)Mouse.sLocation.Y;
            drawingLocation.X = drawRect.X;
            drawingLocation.Y = drawRect.Y;
        }

        public void Terminator()
        {
            int dirX = (int)Mouse.sLocation.X - (int)Location.X;
            int dirY = (int)Mouse.sLocation.Y - (int)Location.Y;

            double d = Math.Sqrt(dirX * dirX + dirY * dirY);

            dirX = (int) (MAX_SPEED * dirX / d);
            dirY = (int) (MAX_SPEED * dirY / d);

            location.X += dirX;
            location.Y += dirY;
        }
    }
}
