using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ailurophobia
{
    class Cat:Character
    {
        protected const float MAX_SPEED = 2;
        protected const float MAX_TURN = .05f;
//        protected List<Delegate> flocking;
        public enum CollisionModifier { NONE, SQUIRTED, CATNIP };
        public enum ModifierDurration {SQIRTED = 60 };
        protected Vector2 modLocation;
        public Vector2 ModLocation
        {
            set { modLocation = value; }
        }
        
        protected int counter;
        protected CollisionModifier modifier;
        
        internal CollisionModifier Modifier
        {
            get { return modifier; }
            set { modifier = value; }
        }

        public Cat(Game game, int x, int y):base(game,x,y)
        {
        }

        public override void Initialize()
        {
            //flocking.Add();
            velocity.X = 3;
            velocity.Y = 3;
            modifier = CollisionModifier.NONE;
            this.counter = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            if (texture == null)
                this.texture = this.Game.Content.Load<Texture2D>("basic_cat");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (rotation>Math.PI*2)
            {
                rotation -= (float)Math.PI * 2;
            }
            if (rotation<0)
            {
                rotation += (float)Math.PI * 2;
            }
            UpdatePos();
            
            base.Update(gameTime);
        }

        internal void UpdatePos()
        {
            this.drawingLocation = new Vector2(this.location.X + 400 - Mouse.SLocation.X, this.location.Y + 300 - Mouse.SLocation.Y);
            drawRect = new Microsoft.Xna.Framework.Rectangle((int)this.drawingLocation.X, (int)this.drawingLocation.Y, texture.Width, texture.Height);
            
        }

        public Vector2 getHeading(){
            return new Vector2((float)Math.Sin(rotation * Math.PI / 18), (float)Math.Cos(rotation * Math.PI / 18));
		}

        public void TurnRight(float rot)
        {
            this.rotation += rot;
        }

        public void TurnLeft(float rot)
        {
            this.rotation -= rot;
        }

        public void Forward()
        {
            this.location += new Vector2(MAX_SPEED * (float)Math.Cos(rotation), MAX_SPEED * (float)Math.Sin(rotation));
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            spriteBatch.Draw(texture, this.drawingLocation, null, Color.White,
                        rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1),
                        SpriteEffects.None, 0);
            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
