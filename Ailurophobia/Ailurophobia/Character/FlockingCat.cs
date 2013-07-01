using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ailurophobia
{
    class FlockingCat:Cat
    {

        private static Vector2 centroid = Vector2.Zero;
        private static List<FlockingCat> flock = new List<FlockingCat>();
        private enum FlockVars { SEEK_MOUSE, AVOID, COHESION, DIRECTION, AVOID_EDGES };
        private List<int> chances;
        int chance;
        private static Vector2 averageHeading = Vector2.Zero;


        public FlockingCat(Game game, int x, int y)
            : base(game, x, y)
        {
            flock.Add(this);
        }

        public override void Initialize()
        {
            velocity.X = 3;
            velocity.Y = 3;

            chances = new List<int>();
            chances.Add(15); //seek mouse
            chances.Add(5); //avoid
            chances.Add(5); //cohesion
            chances.Add(5); //direction
            chances.Add(5); //avoid edges

            for (int i = 0; i < chances.Count; i++)
            {
                chance += chances[i];
            }

            base.Initialize();
        }

        public Vector2 Seek(Vector2 targetlocation)
        {
            Vector2 seekVector = targetlocation - this.location;
            if (seekVector.Length() == 0)
                seekVector = Seek(Mouse.SLocation);
            else
                seekVector.Normalize();
			return seekVector;
        }

        protected void Steer(Vector2 desiredVelocity)
        {
			Vector2 forwardVector;
			Vector2 rightVector;
			float dotForward;
			float dotRight;

			forwardVector=Polar(1,rotation);
			rightVector=new Vector2(- forwardVector.Y,forwardVector.X);
			dotForward=Dot(desiredVelocity,forwardVector);
			dotRight=Dot(desiredVelocity,rightVector);
			//check sign of shadow of DV on forward vector to see if goal is in front or behind
			//if behind turn max
			if (dotForward<0) {
				if (dotRight>0) {
					// if projection of shadow of DV on right vector is positive turn right
					TurnRight(MAX_TURN);
				} else {
					TurnLeft(MAX_TURN);
				}

				//else turn percent right or left
			} else {
                TurnRight(MAX_TURN * dotRight);
			}

		}

        private static float Dot(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        private static Vector2 Polar(float length, float rotation)
        {
            return new Vector2(length * (float)Math.Cos(rotation), length * (float)Math.Sin(rotation));
        }

        private static void setFlockVars()
        {

            averageHeading = new Vector2();
            for (int i = 0; i < flock.Count; i++)
            {
                Vector2 tmp = flock[i].velocity;
                tmp.Normalize();
                averageHeading += tmp;
            }
            centroid = Vector2.Zero;

            for (int j = 0; j < flock.Count; j++)
            {
                centroid += flock[j].Location;
            }
            centroid = new Vector2(centroid.X / flock.Count, centroid.Y / flock.Count);
        }

        public Vector2 cohesion() {
			List<FlockingCat> tempArr=flock;
			Vector2 centPoint=new Vector2(0,0);
			for (int i = 0; i<tempArr.Count; i++) {
				centPoint+=new Vector2(tempArr[i].Location.X,tempArr[i].Location.Y);
			}
			centPoint.X=centPoint.X/tempArr.Count;
			centPoint.Y=centPoint.Y/tempArr.Count;
			return Seek(centPoint);
		}

		// make the flock not collide with itsself
		// check for closest flock mates and flee
		public Vector2 separation(){
			float dist = 10000;
			int turtNum = 0;
			for(int i = 0;i< flock.Count; i++)
			{
				if(Vector2.Distance(this.location, flock[i].Location)<dist&& Vector2.Distance(location, flock[i].Location)!=0)
				{
					dist = Vector2.Distance(location, flock[i].Location);
					turtNum = i;
				}
			}
			return flee(flock[turtNum].Location);
		}
		
		public Vector2 avoidEdges()
		{
			if(location.X+(100*getHeading().X)<=0)
			{
				return Seek(new Vector2(Map.sBackGroundRect.Center.X, Map.sBackGroundRect.Center.Y));
			}
            if (location.X + (100 * getHeading().X) >= Map.sBackGroundRect.Width)
			{
                return Seek(new Vector2(Map.sBackGroundRect.Center.X, Map.sBackGroundRect.Center.Y));
			}
            if (location.Y+ (100 * getHeading().Y) <= 0)
			{
                return Seek(new Vector2(Map.sBackGroundRect.Center.X, Map.sBackGroundRect.Center.Y));
			}
            if (location.Y + (100 * getHeading().Y) >= Map.sBackGroundRect.Height)
			{
                return Seek(new Vector2(Map.sBackGroundRect.Center.X, Map.sBackGroundRect.Center.Y));
			}
            return Vector2.Zero;
		}

        protected Vector2 flee(Vector2 targetPosition)
        {
            Vector2 seekVector = location - targetPosition;
            if (seekVector.Length() == 0)
                seekVector = Seek(Mouse.SLocation);
            else
                seekVector.Normalize();
            return seekVector;
        }

        public override void Update(GameTime gameTime)
        {
            setFlockVars();
            this.roulett();
            this.Forward();
            base.Update(gameTime);
        }

        private void roulett()
        {
            switch (this.Modifier)
            {
                case CollisionModifier.NONE:
                    Random rand = new Random();
                    int turn = rand.Next(chance);
                    int step = 0;
                    for (int i = 0; i < chances.Count; i++)
                    {
                        step += chances[i];
                        if (turn <= step)
                        {
                            switch ((FlockVars)i)
                            {
                                case FlockVars.SEEK_MOUSE:
                                    Steer(Seek(Mouse.SLocation));
                                    break;
                                case FlockVars.AVOID:
                                    Steer(separation());
                                    break;
                                case FlockVars.COHESION:
                                    Steer(cohesion());
                                    break;
                                case FlockVars.DIRECTION:
                                    Steer(Seek(averageHeading));
                                    break;
                                case FlockVars.AVOID_EDGES:
                                    Steer(avoidEdges());
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }
                    }
                    break;
                case CollisionModifier.SQUIRTED:
                    if (this.counter<=(int)ModifierDurration.SQIRTED)
                    {
                        Steer(flee(Mouse.SLocation));
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                        modifier = CollisionModifier.NONE;
                    }
                    break;
                case CollisionModifier.CATNIP:
                    Steer(Seek(modLocation+new Vector2(30,30)));
                    modifier = CollisionModifier.NONE;
                    break;
                default:
                    break;
            }
            
        }
    }
}
