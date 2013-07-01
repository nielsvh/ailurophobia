using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ailurophobia
{
    class Mouse: Character
    {
        public static Vector2 sLocation;
        private KeyboardState oldState;
        private MouseState oldMState;
        private Weapon weapon;

        public Mouse(Game game, int x, int y):base(game, x,y)
        {
        }

        public override void Initialize()
        {
            drawRect = new Microsoft.Xna.Framework.Rectangle(400, 300, 10, 10);
            this.drawingLocation = new Vector2(400f, 300f);
            weapon = new Weapon(this.Game, 0, Color.Blue, this.Game.Content.Load<Texture2D>("tmp"));
            this.Game.Components.Add(weapon);
            velocity.X = 3;
            velocity.Y = 3;
            base.Initialize();
        }

        public void ChangeWeapon(int wepID, Color color, Texture2D texture)
        {
            this.weapon = new Weapon(this.Game, wepID, color, texture);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState msState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            // if keys are pressed, update the character positions.
            if (keyState.GetPressedKeys().Length > 0)
            {
                if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && location.Y < 1800)
                {
                    this.location.Y -= (int)velocity.Y;
                    if (location.Y < 0)
                        location.Y = 5;
                }
                if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && location.Y > 0)
                {
                    this.location.Y += (int)velocity.Y;
                    if (location.Y > 1800)
                        location.Y = 1795;
                    
                }
                if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && location.X < 2400)
                {
                    this.location.X -= (int)velocity.X;
                    if (location.X < 0)
                        location.X = 5;
                }
                if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && location.X > 0)
                {
                    this.location.X += (int)velocity.X;
                    if (location.X > 2400)
                        location.X = 2395;
                    
                }
            }
            // if left mouse button pressed, fire the current weapon
            if ((msState.LeftButton == ButtonState.Pressed) && (oldMState.LeftButton == ButtonState.Released))
            {
                Point mouseLoc = new Point(msState.X, msState.Y);
                weapon.FireWeapon(this.Location, mouseLoc);
            }
            //System.Console.WriteLine("X: {0}; Y: {1}", this.location.X, this.location.Y);
            oldState = keyState;
            oldMState = msState;
            sLocation = location;

            MouseState tmp = Microsoft.Xna.Framework.Input.Mouse.GetState();
            float tmpF = (float)Math.Atan2(300-tmp.Y, 400-tmp.X);
            if (tmpF!=rotation)
            {
                this.rotation = tmpF;
                //this.drawingLocation.X = (float)((drawingLocation.X) * Math.Cos(rotation) - (drawingLocation.Y) * Math.Sin(rotation));
                //this.drawingLocation.Y = (float)((drawingLocation.Y) * Math.Cos(rotation) + (drawingLocation.X) * Math.Sin(rotation));
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
