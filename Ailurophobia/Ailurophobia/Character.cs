using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ailurophobia
{
    class Character:DrawableGameComponent
    {
        //Eryc's additions
        private int hp;                 //hit points
        private int hpmax;              //max vitality
        private int lives;              //lives
        private int kills;
        private int score;
        private bool hit;               // collision flag
        private int counter;            // counter for subsequent hits
        private Texture2D pic;


        protected SpriteBatch spriteBatch;
        protected Rectangle drawRect;

        public Rectangle DrawRect
        {
            get { return drawRect; }
        }
        #region oldLocation
        /*
        /// <summary>
        /// position relitive to the map
        /// </summary>
        protected int posX;

        public int PosX
        {
            get { return posX; }
        }

        /// <summary>
        /// position relative to the map
        /// </summary>
        protected int posY;

        public int PosY
        {
            get { return posY; }
        }
        */
        #endregion
        protected Vector2 location;

        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }

        protected Vector2 drawingLocation;

        public Vector2 DrawingLocation
        {
            get { return drawingLocation; }
            set { drawingLocation = value; }
        }


        protected Vector2 velocity;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        protected float rotation;

        public float Roatation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        protected Texture2D texture;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Character(Game game, int x, int y):base(game)
        {
            this.location.X = x;
            this.location.Y = y;
            kills = 0;
            score = 0;
            counter = 0;
        }

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            if(texture==null)
            this.texture = this.Game.Content.Load<Texture2D>("tmp");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        public void setPosition(int x, int y)
        {
            location.X = x;
            location.Y = y;
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            spriteBatch.Draw(texture, new Vector2(this.drawingLocation.X, this.drawingLocation.Y), null, Color.White,
                        rotation,new Vector2(texture.Width/2, texture.Height/2),new Vector2(1, 1),
                        SpriteEffects.None, 0);
            this.spriteBatch.End();
            Console.WriteLine("drawing location: " + drawingLocation.X + "/" + drawingLocation.Y);
            base.Draw(gameTime);
        }

        //Eryc's additions
        //Eryc's additions
        public int gethp()
        {
            return hp;
        }
        public int gethpmax()
        {
            return hpmax;
        }
        public int getlives()
        {
            return lives;
        }
        public int getkills()
        {
            return kills;
        }
        public int getscore()
        {
            return score;
        }
        public void setlives(int val)
        {
            lives = val;
        }
        public void setscore(int val)
        {
            score = val;
        }
        public void setkills(int val)
        {
            kills = val;
        }
        public void sethp(int val)
        {
            hp = val;
        }
        public void sethpmax(int val)
        {
            hp = val;
        }
        public void changekills(int val)
        {
            kills += val;
        }
        public void changescore(int val)
        {
            score += val;
        }
        public void changelives(int val)
        {
            lives += val;
        }
        public void collision(int val)
        {
            if (counter < 3)
                counter++;
            else
            {
                counter = 0;
                changehp(val);
            }

        }
        public void changehp(int val)
        {
            hp += val;
            updatestats();
        }
        public void changehpmax(int val)
        {
            hpmax += val;
        }
        public void updatestats()
        {
            if (hp <= 0)
            {
                lives--;
                hp = hpmax;
            }
            if (lives < 0)
            {
                //insert code for gameover
                lives = 20;
            }
        }
        public void setpic(Texture2D val)
        {
            pic = val;
        }
        public Texture2D getpic()
        {
            return pic;
        }
    }
}
