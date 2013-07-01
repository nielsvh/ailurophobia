using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ailurophobia
{
    class Character : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        protected Rectangle drawRect;

        //Eryc's additions
        private int hp;                 //hit points
        private int hpmax;              //max vitality
        private int lives;              //lives
        private int kills;
        private int score;
        private bool hit;               // collision flag
        private int counter;            // counter for subsequent hits

        protected bool attackable;

        public bool Attackable
        {
            get { return attackable; }
        }
        protected GameTimer timer;
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
//            set { location = value; }
        }

        protected Vector2 drawingLocation;


        protected Vector2 velocity;

        protected float rotation;

        protected Texture2D texture;

        public Character(Game game, int x, int y)
            : base(game)
        {
            this.location = new Vector2(x, y);
            this.drawingLocation = new Vector2(-100, -100);
        }

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            //rotation = (float)Math.PI / 2f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            if (texture == null)
                this.texture = this.Game.Content.Load<Texture2D>("tmp");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            spriteBatch.Draw(texture, this.drawingLocation, null, Color.White,
                        rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1),
                        SpriteEffects.None, 0f);
            this.spriteBatch.End();
            base.Draw(gameTime);
        }

        //Eryc's additions
        //Eryc's additions
        // FIXED!
        public int HP
        {
            get { return hp; }
            set
            {
                hp = value;
                if (value < 0)
                {
                    attackable = false;
                    this.timer.Enabled = true;
                }
                if (hp < 0)
                {
                    lives--;
                    if (lives >= 0)
                        hp = hpmax;
                }
                if (hp > hpmax)
                {
                    hp = hpmax;
                    lives++;
                }
                if (lives==-1)
                {
                    Game1.running = false;
                }
            }
        }
        public int HpMax
        {
            get { return hpmax; }
            set { hpmax = value; }
        }
        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }
        public int Kills
        {
            get { return kills; }
//            set { kills = value; }
        }
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public void changehp(object o, ChangeHealthEventArgs e)
        {
            HP += e.TheNumber;
        }
    }
}