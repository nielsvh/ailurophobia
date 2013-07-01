using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ailurophobia
{
    class Projectile:DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        private Vector2 velocity;

        public Vector2 Velocity
        {
            get { return velocity * new Vector2(-1,-1); }
//            set { velocity = value; }
        }

        protected Vector2 drawingLocation;

//        public Vector2 DrawingLocation
  //      {
      //      get { return drawingLocation; }
//            set { drawingLocation = value; }
    //    }

        private int length;

        public int Length
        {
            get { return length; }
//            set { length = value; }
        }

        private int trajLength;
        public int TrajLength
        {
            get { return trajLength; }
//            set { trajLength = value; }
        }

        //private Point SLocation;

        private float rotation;

        private float tempVelocity;

        private int stepCount;

        public int StepCount
        {
            get { return stepCount; }
//            set { stepCount = value; }
        }

        private Weapon.WeaponType wepType;

        private Vector2 location;

//        private Point origin;

        private Texture2D[] text = new Texture2D[2];

        public Vector2 Location
        {
            get { return location; }
//            set { location = value; }
        }

        protected int range;
        public int Range
        {
            get { return range; }
        }

        private int damage;

        public int Damage
        {
            get { return damage; }
        }

        public Projectile(Game game, Vector2 spawn, float angle, int velocity, int dmg, Weapon.WeaponType type)
            : base(game)
        {
            location = spawn;
//            origin = new Point((int)Mouse.SLocation.X, (int)Mouse.SLocation.Y);
            tempVelocity = velocity;
            wepType = type;
            this.velocity = new Vector2((float)(tempVelocity * Math.Cos(angle)), (float)(tempVelocity * Math.Sin(angle)));
            damage = dmg;
            rotation = angle;
            MouseState ms = Microsoft.Xna.Framework.Input.Mouse.GetState();
            this.location = spawn;
            if (wepType == Weapon.WeaponType.CATNIP)
            {
                this.location = Mouse.SLocation+ new Vector2(ms.X-400, ms.Y-300) + new Vector2(-30, -30);
                Map.sModList.Add(this);
                this.range = 150;
            }
            else
                Map.sProjList.Add(this);
            length = 10;
            trajLength = 60;
        }

        public override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
        }

        protected override void LoadContent()
        {
            text[0] = this.Game.Content.Load<Texture2D>("awesome");
            text[1] = this.Game.Content.Load<Texture2D>("catnip");
            base.LoadContent();
        }
        private void Remove()
        {

            //this.Enabled = false;
            this.Visible = false;
            this.Game.Components.Remove(this);
            Map.sProjList.Remove(this);
            Map.sModList.Remove(this);
        }
        public override void Update(GameTime gameTime)
        {
            if (wepType != Weapon.WeaponType.CATNIP)
            {
                if (stepCount == trajLength)
                {
                    Remove();
                }

                location.X += (int)velocity.X;
                location.Y += (int)velocity.Y;
                stepCount++;
                if (wepType == Weapon.WeaponType.GUIDED)
                {
                    MouseState ms = Microsoft.Xna.Framework.Input.Mouse.GetState();
                    double xComp = this.location.X + 400 - Mouse.SLocation.X - ms.X;
                    double yComp = this.location.Y + 300 - Mouse.SLocation.Y - ms.Y;
                    float tmpF = (float)Math.Atan2(yComp, xComp);
                    if (tmpF != rotation)
                    {
                        this.rotation = tmpF;
                    }
                    this.velocity = new Vector2((float)(tempVelocity * Math.Cos(this.rotation)), (float)(tempVelocity * Math.Sin(this.rotation)));
                }
                this.drawingLocation = new Vector2(this.location.X + 400 - Mouse.SLocation.X, this.location.Y + 300 - Mouse.SLocation.Y);
            }
            else
            {
                if (stepCount == 500)
                {
                    Remove();
                }
                stepCount++;
                this.drawingLocation = new Vector2(this.location.X + 400 - Mouse.SLocation.X, this.location.Y + 300 - Mouse.SLocation.Y);
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (wepType != Weapon.WeaponType.CATNIP)
            {
                //Vector2 temp = new Vector2(this.location.X + origin.X - Mouse.SLocation.X, this.location.Y + origin.Y - Mouse.SLocation.Y);
                spriteBatch.Begin();
                spriteBatch.Draw(text[0], drawingLocation, null, Color.ForestGreen, rotation, Vector2.Zero, new Vector2(10, 2), SpriteEffects.None, 0);
                spriteBatch.End();
            }
            else
            {
               // Vector2 temp = new Vector2(drawingLocation, this.location.Y + origin.Y - Mouse.SLocation.Y);
                spriteBatch.Begin();
                spriteBatch.Draw(text[1], drawingLocation, null, Color.White, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
