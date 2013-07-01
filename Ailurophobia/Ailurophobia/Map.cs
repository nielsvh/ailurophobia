using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using XmlContentReader;

namespace Ailurophobia
{
    class Map:DrawableGameComponent
    {
        public static List<Character> sCharList;
        public static Rectangle sBackGroundRect;
        public static HashSet<Projectile> sProjList = new HashSet<Projectile>();
        public static HashSet<Projectile> sModList = new HashSet<Projectile>();
        List<Character> charList;
        List<Obstacle> obstacles;
        
        Texture2D[] textures;
        Rectangle backGroundRect;
        SpriteBatch spriteBatch;
        hud HUD;
        GameTimer timer;

        public Map(Game game):base(game)
        {
        }

        public override void Initialize()
        {
            Random rand = new Random();
            timer = new GameTimer(this.Game, GameTimer.TimerType.HIT_TIMER, 0);

            HUD = new hud(this.Game, -1, -1);

            charList = new List<Character>();
            obstacles = new List<Obstacle>();
            this.textures = new Texture2D[2];
            this.backGroundRect = new Rectangle();
            
            charList.Add(new Mouse(this.Game, 1200, 900));
            charList[0].HpMax = 2;// sethpmax(3);
            charList[0].Lives = 2;// setlives(2);
            charList[0].HP = 2;// sethp(3);
            for (int i = 0; i < 3; i++)
            {
                //charList.Add(new FlockingCat(this.Game,1100,800));
                charList.Add(new FlockingCat(this.Game, rand.Next(0,2400),rand.Next(0,1800)));
            }
            
            for (int i = 0; i < charList.Count; i++)
            {
                this.Game.Components.Add(charList[i]);
            }
            this.Game.Components.Add(HUD);
            /*timer.Interval = new TimeSpan(10000000);
            timer.HTick += new HealthTickHandler(charList[0].changehp);
            this.Game.Components.Add(timer);*/
            base.Initialize();
            
        }

        public void spawnCats(object o, SpawnEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                FlockingCat tmp = new FlockingCat(this.Game, (int)e.theVector.X, (int)e.theVector.Y);
                this.charList.Add(tmp);
                this.Game.Components.Add(tmp);
                tmp = null;
            }
        }

        protected override void LoadContent()
        {

            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            LoadTextures();
            LoadObstacles();
            for (int i = 0; i < obstacles.Count; i++)
            {
                this.Game.Components.Add(obstacles[i]);
                if (obstacles[i] is SpawnPoint)
                {
                    ((SpawnPoint)obstacles[i]).spawnTimer.STick += new SpawnTickHandler(spawnCats);
                }
            }            
            this.backGroundRect.Width = 2400;
            this.backGroundRect.Height = 1800;
            base.LoadContent();
        }

        public void LoadObstacles()
        {
            List<XmlContentReader.Obstacle> tmp = this.Game.Content.Load<List<XmlContentReader.Obstacle>>(@"Test");
            for (int i = 0; i < tmp.Count; i++)
            {
                tmp[i].Load(this.Game.Content);
                switch (tmp[i].Type)
                {
                    case XmlContentReader.Obstacle.objtype.SOFA:
                        obstacles.Add(new Sofa(this.Game, tmp[i]));
                        break;
                    case XmlContentReader.Obstacle.objtype.SPAWN:
                        obstacles.Add(new SpawnPoint(this.Game, tmp[i]));
                        
                        break;
                    default:
                        break;
                }
            }
        }

        public void LoadTextures()
        {
            this.textures[0] = this.Game.Content.Load<Texture2D>("Boundries");
            HUD.setminimap(this.Game.Content.Load<Texture2D>("Boundries"));
            HUD.addwep(this.Game.Content.Load<Texture2D>("background/gun"));
            HUD.addwep(this.Game.Content.Load<Texture2D>("background/gun3"));
            HUD.addwep(this.Game.Content.Load<Texture2D>("background/missile"));
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Projectile proj in sProjList)
            {
                if (!proj.Enabled && proj.TrajLength>proj.StepCount)
                {
                    proj.Enabled = true;
                    proj.Visible = true;
                }
            }
            this.backGroundRect.X = 400-(int)Mouse.SLocation.X;
            this.backGroundRect.Y = 300-(int)Mouse.SLocation.Y;
            base.Update(gameTime);
            sCharList = charList;
            sBackGroundRect = backGroundRect;
            this.CheckCollision();
           
            

        }

        public void CheckCollision()
        {
            List<Projectile> toRemove = new List<Projectile>();
            for(int i = 1; i< charList.Count; i++)
            {
                foreach(Projectile proj in sProjList)
                {
                    Vector2 vectTo = new Vector2(charList[i].Location.X, charList[i].Location.Y);
                    int radius = charList[i].DrawRect.Height/2;
                    //float dist;
                    //Vector2.Distance(sProjList[j].Location,new Vector2(charList[i].DrawRect.Center.X, charList[i].DrawRect.Center.Y), dist);
                    if(Distance(proj.Location/*-new Vector2(400,300)*/, vectTo) < radius + proj.Length)
                    {
                        Vector2 normal = new Vector2(-proj.Velocity.Y, proj.Velocity.X);
                        vectTo.Normalize();
                        normal.Normalize();
                        if(Math.Abs(Project(Dot(vectTo, normal),proj.Velocity)) < radius + proj.Length)
                        {
                            toRemove.Add(proj);
                            Console.WriteLine("COLLIDE");
                            // Deal with it!
                            ((Cat)charList[i]).Modifier = Cat.CollisionModifier.SQUIRTED;
                            break;
                        }
                    }
                }
            }
            foreach (Projectile modifier in Map.sModList)
            {
                foreach (Character kit in charList)
                {
                        if (kit is Cat)
                        {
                            if (Distance(kit.Location, modifier.Location)<modifier.Range)
                            {
                                ((FlockingCat)kit).Modifier = Cat.CollisionModifier.CATNIP;
                                ((Cat)kit).ModLocation = modifier.Location;
                                //break;
                            }
                            else
                            {
                                (kit as FlockingCat).Modifier = Cat.CollisionModifier.NONE;
                                (kit as Cat).ModLocation = Vector2.Zero;
                            }
                        }
                    }
                }
            foreach (Projectile proj in toRemove)
            {
                sProjList.Remove(proj);
                this.Game.Components.Remove(proj);
            }
            for (int i = 1; i < charList.Count; i++)
            {
                if (charList[0].Attackable)
                {
                    if (charList[0].DrawRect.Intersects(charList[i].DrawRect))
                    {
                        charList[0].HP -= 1;
                        break;
                    }
                }
            }


            if (Game.Components.Contains(HUD))
                Game.Components.Remove(HUD);
            if(!Game.Components.Contains(HUD))
                this.Game.Components.Add(HUD);
        }

        private static float Project(float p, Vector2 vector2)
        {
            vector2.X *= p;
            vector2.Y *= p;
            return vector2.Length();
        }

        private static float Distance(Vector2 start, Vector2 end)
        {
            return (float) Math.Sqrt(Math.Pow(-start.X + end.X, 2) + Math.Pow(-start.Y + end.Y, 2));
        }

        private static float Dot(Vector2 start, Vector2 end)
        {
            return start.X * end.X + start.Y * end.Y;
        }

        public override void Draw(GameTime gameTime)
        {
            
            //Rectangle temp = backGroundRect;
            //temp.X += 400;
            //temp.Y += 300;
            spriteBatch.Begin();
            spriteBatch.Draw(textures[0], backGroundRect,null, Color.Bisque,0,Vector2.Zero,SpriteEffects.None,1f); //Background draw statement            
            spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
