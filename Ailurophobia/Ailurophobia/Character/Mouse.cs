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
        private static Vector2 sLocation;

        public static Vector2 SLocation
        {
            get { return Mouse.sLocation; }
        }
        private MouseState oldState;
        private KeyboardState oldKS;

        private Weapon weapon;       
        private int wheel = 0;
        private Weapon.WeaponType wep;
        

        // add another weapon for mouse to use
        // need a bool that gets turned on upon pickup
        // make the weapon instance an array


        public Mouse(Game game, int x, int y):base(game,x,y)
        {
        }
        public Weapon getweapon()
        {
            return weapon;
        }

        public override void Initialize()
        {
            drawRect = new Microsoft.Xna.Framework.Rectangle(400, 300, 10, 10);
            this.drawingLocation = new Vector2(400f, 300f);
            weapon = new Weapon(this.Game);
            this.Game.Components.Add(weapon);
            velocity.X = 3;
            velocity.Y = 3;
            this.spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            this.timer = new GameTimer(this.Game, GameTimer.TimerType.HIT_TIMER, 0);
            this.timer.Interval = new TimeSpan(50000000);
            this.Game.Components.Add(timer);
            this.timer.HTick += new HealthTickHandler(timer_HTick);
            this.Enabled = false;
            this.attackable = true;
            base.Initialize();
        }

        void timer_HTick(object obj, ChangeHealthEventArgs e)
        {
            if (!this.attackable)
            {
                this.attackable = true;
                timer.Enabled = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState ms = Microsoft.Xna.Framework.Input.Mouse.GetState();
            // if keys are pressed, update the character positions.
            if (ms.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                weapon.FireWeapon(this.location, this.rotation);
            }
            if (keyState.GetPressedKeys().Length > 0)
            {
                // if >0 move down
                if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && location.Y < 1800)
                {
                    this.location.Y += (int)velocity.Y;
                }
                // if <1800 move up
                if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && location.Y > 0)
                {
                    this.location.Y -= (int)velocity.Y;
                }
                // if >0 move right
                if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && location.X < 2400)
                {
                    this.location.X += (int)velocity.X;
                }
                // if <2400 move leftx
                if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && location.X > 0)
                {
                    this.location.X -= (int)velocity.X;
                }

                if (Microsoft.Xna.Framework.Input.Mouse.GetState().ScrollWheelValue < wheel)
                {
                    if ((int)weapon.CurrentWeapon == 0)
                        weapon.SetWeapon(Weapon.WeaponType.CATNIP);
                    else weapon.SetWeapon(weapon.CurrentWeapon - 1);
                    //wep = weapon.CurrentWeapon;
                    wheel = Microsoft.Xna.Framework.Input.Mouse.GetState().ScrollWheelValue;
                }
                if (Microsoft.Xna.Framework.Input.Mouse.GetState().ScrollWheelValue > wheel)
                {
                    if ((int)weapon.CurrentWeapon == 3)
                        weapon.SetWeapon(0);
                    else weapon.SetWeapon(weapon.CurrentWeapon + 1);
                    //wep = weapon.CurrentWeapon;
                    wheel = Microsoft.Xna.Framework.Input.Mouse.GetState().ScrollWheelValue;
                }
                if (Microsoft.Xna.Framework.Input.Mouse.GetState().RightButton.Equals(ButtonState.Pressed))
                {
                    wep = weapon.CurrentWeapon;
                    weapon.SetWeapon(Weapon.WeaponType.CATNIP);
                    weapon.FireWeapon(this.location, this.rotation);
                    weapon.SetWeapon(wep);
                }
                else if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D4) && oldKS.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.D4))
                {
                    weapon.SetWeapon(Weapon.WeaponType.CATNIP);
                }
                if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D1) && oldKS.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.D1))
                {
                    weapon.SetWeapon(Weapon.WeaponType.BASIC);
                    
                }
                if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D2) && oldKS.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.D2))
                {
                    weapon.SetWeapon(Weapon.WeaponType.SHOTGUN);
                }
                if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D3) && oldKS.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.D3))
                {
                    weapon.SetWeapon(Weapon.WeaponType.GUIDED);
                }
                
            }
            oldState = ms;
            oldKS = keyState;
            sLocation = location;

            MouseState tmp = Microsoft.Xna.Framework.Input.Mouse.GetState();
            float tmpF = (float)Math.Atan2(300-tmp.Y, 400-tmp.X);
            if (tmpF!=rotation)
            {
                this.rotation = tmpF;
                //this.drawingLocation.X = (float)((drawingLocation.X) * Math.Cos(rotation) - (drawingLocation.Y) * Math.Sin(rotation));
                //this.drawingLocation.Y = (float)((drawingLocation.Y) * Math.Cos(rotation) + (drawingLocation.X) * Math.Sin(rotation));
            }

            // IF MOUSE ENCOUNTERS A CATNIP PICKUP (put this wherever collisions are being done)
            // Delete the pickup object
            // weapon.CurrentWeapon = 3;

            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            if (texture == null) 
            this.texture = this.Game.Content.Load<Texture2D>("background/mouse");
            base.LoadContent();
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
