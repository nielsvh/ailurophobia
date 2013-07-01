using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Ailurophobia
{
    class MenuStateMachine : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D[] menuTextures;
        public enum textures { TITLE, CLAWS, PLAY, CATALYST, CREDITS, HIGHSCORES, OPTIONS };

        public MenuStateMachine(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            menuTextures = new Texture2D[7];
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            menuTextures[0] = this.Game.Content.Load<Texture2D>("title");
            menuTextures[1] = this.Game.Content.Load<Texture2D>("claws");
            menuTextures[2] = this.Game.Content.Load<Texture2D>("play");
            menuTextures[3] = this.Game.Content.Load<Texture2D>("catalyst");
            menuTextures[4] = this.Game.Content.Load<Texture2D>("credits");
            menuTextures[5] = this.Game.Content.Load<Texture2D>("highscores");
            menuTextures[6] = this.Game.Content.Load<Texture2D>("options");

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();
            //spriteBatch.Draw(menuTextures[(int)textures.CLAWS], new Vector2(200, 125), Color.White);
            spriteBatch.Draw(menuTextures[(int)textures.PLAY], new Vector2(305, 178), Color.White);
            spriteBatch.Draw(menuTextures[(int)textures.CATALYST], new Vector2(305, 228), Color.White);
            spriteBatch.Draw(menuTextures[(int)textures.CREDITS], new Vector2(305, 278), Color.White);
            spriteBatch.Draw(menuTextures[(int)textures.HIGHSCORES], new Vector2(305, 328), Color.White);
            spriteBatch.Draw(menuTextures[(int)textures.OPTIONS], new Vector2(305, 378), Color.White);
            spriteBatch.Draw(menuTextures[(int)textures.TITLE], new Vector2(252, 86), Color.White);
            spriteBatch.End();
        }
    }
}
