using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Ailurophobia
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static bool running = true;
        private enum textureIndex { TMP=0, BOUNDARIES = 1, TITLE = 2, CLAWS = 3, PLAY = 4, CATALYST = 5, CREDITS = 6, HIGHSCORES = 7, OPTIONS = 8 };
        private enum StateIndex { MENUSTATE = 0, GAMESTATE = 1, PAUSESTATE = 2 };
        GraphicsDeviceManager graphics;
//        SpriteBatch spriteBatch;
//        Texture2D[] textures;
        StateIndex state;
        Map gameMap;
        MenuStateMachine gameMenu;
        MouseState prevStateMouse;
        KeyboardState prevStateKeyB;
        bool togglePause = false; //Switch for Pause Screen
        bool stateChanged = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.gameMap = new Map(this);
            //textures = new Texture2D[20];
            gameMenu = new MenuStateMachine(this);
            IsMouseVisible = true;
            this.Components.Add(gameMap);

            this.Components.Add(gameMenu);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            this.state = StateIndex.MENUSTATE;
            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!running)
            {
                foreach (GameComponent comp in this.Components)
                {
                    comp.Enabled = false;
                }
            }
            MouseState ms = Microsoft.Xna.Framework.Input.Mouse.GetState();
            KeyboardState ks = Keyboard.GetState();

            if (gameMenu.Enabled == true && 305 < ms.X && ms.X < 495 && 178 < ms.Y && ms.Y < 222)
            {
                if (ms.LeftButton == ButtonState.Released && prevStateMouse.LeftButton == ButtonState.Pressed)
                {
                    stateChanged = true;
                    state = StateIndex.GAMESTATE;
                }
            }
            if (gameMap.Enabled == false && ks.IsKeyUp(Keys.P) && prevStateKeyB.IsKeyDown(Keys.P) && togglePause == true) //unpause
            {
                stateChanged = true;
                togglePause = false;
                state = StateIndex.GAMESTATE;
            }

            if (gameMap.Enabled == true && ks.IsKeyUp(Keys.P) && prevStateKeyB.IsKeyDown(Keys.P) && togglePause == false) //pause
            {
                stateChanged = true;
                togglePause = true;
                state = StateIndex.PAUSESTATE;

            }
            prevStateKeyB = ks;
            prevStateMouse = ms;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (stateChanged)
            {
                switch (state)
                {
                    case StateIndex.MENUSTATE:
                        foreach (GameComponent Component in this.Components)
                        {
                            if (!(Component is MenuStateMachine))
                            {
                                Component.Enabled = false;

                                if (Component is DrawableGameComponent)
                                {
                                    (Component as DrawableGameComponent).Visible = false;
                                }
                            }
                        }
                        stateChanged = false;
                        break;
                    case StateIndex.GAMESTATE:
                        foreach (GameComponent Component in this.Components)
                        {
                            if (!(Component is MenuStateMachine))
                            {
                                Component.Enabled = true;

                                if (Component is DrawableGameComponent)
                                {
                                    (Component as DrawableGameComponent).Visible = true;
                                }
                            }
                            gameMenu.Visible = false;
                            gameMenu.Enabled = false;
                            IsMouseVisible = false;
                        }
                        stateChanged = false;
                        break;
                    case StateIndex.PAUSESTATE:
                        foreach (GameComponent Component in this.Components)
                        {
                            if (!(Component is MenuStateMachine)||!(Component is hud))
                            {
                                Component.Enabled = false;
                            }
                            else
                            {
                                Component.Enabled = true;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            /*this.graphics.PreferredBackBufferHeight = 1050;
            this.graphics.PreferredBackBufferWidth = 1200;
            this.graphics.ApplyChanges();*/

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (state == StateIndex.MENUSTATE)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }
            else
            {
                GraphicsDevice.Clear(Color.Gray);
            }

            base.Draw(gameTime);
           
        }
    }
}
