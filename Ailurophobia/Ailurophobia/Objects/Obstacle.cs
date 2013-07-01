using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ailurophobia
{
    class Obstacle:DrawableGameComponent
    {
        protected Vector2 location;
        protected float rotation;
        protected Texture2D texture;
        protected Rectangle drawRect;
        protected SpriteBatch spriteBatch;

        public Vector2 Location
        {
            get { return location; }
//            set { location = value; }
        }

        //public float Rotation
        //{
        //    get { return rotation; }
        //    set { rotation = value; }
        //}

        //public Texture2D Texture
        //{
        //    get { return texture; }
        //}
        public Obstacle(Game game, XmlContentReader.Obstacle readIn)
            : base(game)
        {
            this.location = readIn.Location;
            //this.location.X += 400;
            //this.location.Y -= 300;
            this.rotation = readIn.Rotation;
            this.texture = readIn.Texture;
        }

        public override void Initialize()
        {
            this.drawRect = new Rectangle((int)location.X, (int)location.Y, 100, 50);
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.drawRect.Location = new Point((int)(location.X- Mouse.SLocation.X+400), (int)(location.Y-Mouse.SLocation.Y+300));
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(this.texture, this.drawRect, null, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 1f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
