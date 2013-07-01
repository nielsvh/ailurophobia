using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Author: Eryc
    /// Edit: Niels
    /// By changing the component to a drawable game component, it gets an update, draw etc.
    /// </summary>
    class hud:DrawableGameComponent
    {
        private Texture2D[] inplay = new Texture2D[5]; //holds the sprites for the in-play interface 1)sidebar
        private List<obj> sidebar = new List<obj>();//holds the sidebar objects
        private List<obj> wframe = new List<obj>();//holds weapon frame sprites
        private List<obj> pframe = new List<obj>();//holds power up frame sprites
        private List<obj> health = new List<obj>();//holds the stages of health        
        private bool timeout;
        private SpriteFont lifefont;
//        private GameTime gametime;
        private double sec = 0;
        private double secmax = 0;
        private int min = 0;
        private int minmax;
        private Texture2D blip;
        private SpriteBatch spriteBatch;
        //private int preswep=-1;
        private int prespow=-1;
        private Texture2D minipmap;
        private List<Texture2D> wep=new List<Texture2D>();
        private List<Texture2D> pow = new List<Texture2D>();

        public hud(Game game, int mins, int secs)
            : base(game)
        {
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            if (mins == -1)
                minmax = 2;
            else minmax = mins;
            if (secs == -1)
                secmax = 30;
            else secmax = secs;
            min = minmax;
            sec = secmax;
            //readin(this.Game.Content);
//            gametime = new GameTime();
//            timeout = false;            
        }
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {


            //insert code for reading in files

            //for text

            blip = this.Game.Content.Load<Texture2D>("tmp");

            #region PURPLEBOX
            lifefont = this.Game.Content.Load<SpriteFont>("background/lifetext");

            char[] delim = { '#' };


            inplay[0] = this.Game.Content.Load<Texture2D>("background/sidebar");
            inplay[4] = this.Game.Content.Load<Texture2D>("background/x");
            #endregion 

            #region READING IN FILE (INTERFACE)
            // the contnet folder is located in the same directory as the executable.
            // Make sure to edit the properties of anything in content to be coppied always.
            // Niels
            StreamReader file = new StreamReader(@".\content\levels\sidebar.txt");
            string input;
            //int[] range = { 0, 0 };

            while ((input = file.ReadLine()) != null)
            {
                string[] line = input.Split(delim);

                if(line[1]=="wepslot")
                    wframe.Add(new obj(line[1], int.Parse(line[2]), int.Parse(line[3]), this.Game.Content.Load<Texture2D>(line[0])));
                else if(line[1]=="powslot")
                    pframe.Add(new obj(line[1], int.Parse(line[2]), int.Parse(line[3]), this.Game.Content.Load<Texture2D>(line[0])));
                else sidebar.Add(new obj(line[1], int.Parse(line[2]), int.Parse(line[3]), this.Game.Content.Load<Texture2D>(line[0])));

            }
            #endregion

            #region READING IN FILE (HEALTH)

            file = new StreamReader(@".\content\levels\health.txt");

            while ((input = file.ReadLine()) != null)
            {
                string[] line = input.Split(delim);

                health.Add(new obj("health bar", int.Parse(line[1]), int.Parse(line[2]), this.Game.Content.Load<Texture2D>(line[0])));

            }

            #endregion

        
            base.LoadContent();
        }
        
        public void setminimap(Texture2D val)
        {
            minipmap = val;
        }
        public void checktime()
        {
            if (min == 0 && sec <= 0)
            {
                timeout = true;
                min = minmax;
                sec = secmax;
            }
            if (sec <= 0)
            {
                sec = 60;
                min--;
            }
            
        }
        public bool timestatus()
        {
            if (timeout)
            {
                timeout = !timeout;
                return !timeout;
            }
            else return timeout;
        }

        public override void Update(GameTime gameTime)
        {
            updatetime();
            base.Update(gameTime);
        }

        public void updatetime()
        {
            //Console.WriteLine("time lost: " + (int)(.02 * Math.Ceiling((double) gametime.ElapsedRealTime.Milliseconds)));
            sec -= .02;    //(int)(.02 * Math.Ceiling(gametime.ElapsedRealTime.TotalSeconds));
            checktime();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            int i = new int();
            int state = new int();
            List<Character> character = Map.sCharList;
            Rectangle back = Map.sBackGroundRect;
            spriteBatch.Begin();
            #region INTERFACE

            for (i = 0; i < sidebar.Count; i++)
            {
                if ((i == (sidebar.Count - 1)) && sec < 30 && min < 1)
                {
                    spriteBatch.Draw(sidebar[i].getstand(), new Rectangle(sidebar[i].PosX, sidebar[i].PosY, sidebar[i].getstand().Width, sidebar[i].getstand().Height), null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
                else if (sidebar[i].gettype() == "map")
                {
                    if (minipmap != null)
                        spriteBatch.Draw(minipmap, new Rectangle(sidebar[i].PosX, sidebar[i].PosY, sidebar[i].getstand().Width, sidebar[i].getstand().Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    spriteBatch.Draw(sidebar[i].getstand(), new Rectangle(sidebar[i].PosX, sidebar[i].PosY, sidebar[i].getstand().Width, sidebar[i].getstand().Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
                else spriteBatch.Draw(sidebar[i].getstand(), new Rectangle(sidebar[i].PosX, sidebar[i].PosY, sidebar[i].getstand().Width, sidebar[i].getstand().Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }


            //draws the weapon sprites. only the present weapon is drawn full size.
            for (i = 0; i < wep.Count; i++)
            {
                 spriteBatch.Draw(wep[i], new Rectangle(wframe[i].PosX, wframe[i].PosY, wframe[i].getstand().Width, wframe[i].getstand().Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
            
            //draws the powerup sprites. only the present weapon is drawn full size.
            for (i = 0; i < pow.Count; i++)
            {
                if (prespow == i)
                    spriteBatch.Draw(pow[i], new Rectangle(pframe[i].PosX, pframe[i].PosY,pframe[i].getstand().Width, pframe[i].getstand().Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                else spriteBatch.Draw(pow[i], new Rectangle(pframe[i].PosX, pframe[i].PosY, pframe[i].getstand().Width / 2, pframe[i].getstand().Height / 2), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }

            //draw the weapon and powerup frames. These get drawn regardless, but they'll be empty
            for (i = 0; i < wframe.Count; i++)
            {
                if (((Mouse)character[0]).getweapon().CurrentWeapon == (Weapon.WeaponType)i)
                    spriteBatch.Draw(wframe[i].getstand(), new Rectangle(wframe[i].PosX, wframe[i].PosY, wframe[i].getstand().Width, wframe[i].getstand().Height), null, Color.Coral, 0, Vector2.Zero, SpriteEffects.None, 0);
                else spriteBatch.Draw(wframe[i].getstand(), new Rectangle(wframe[i].PosX, wframe[i].PosY, wframe[i].getstand().Width , wframe[i].getstand().Height ), null, Color.Blue, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
            for (i = 0; i < pframe.Count; i++)
            {
                if (prespow == i)
                    spriteBatch.Draw(pframe[i].getstand(), new Rectangle(pframe[i].PosX, pframe[i].PosY, pframe[i].getstand().Width, pframe[i].getstand().Height), null, Color.Coral, 0, Vector2.Zero, SpriteEffects.None, 0);
                else spriteBatch.Draw(pframe[i].getstand(), new Rectangle(pframe[i].PosX, pframe[i].PosY, pframe[i].getstand().Width / 2, pframe[i].getstand().Height / 2), null, Color.Coral, 0, Vector2.Zero, SpriteEffects.None, 0);
            }

            //Console.WriteLine(" hp max: " + character[0].gethpmax() + " hp: " + character[0].gethp());
            state = (int)Math.Floor((character[0].HpMax  * 1.0) / (character[0].HP));
            try
            {
            state = character[0].HP; // health.Count - state;
            spriteBatch.Draw(health[state].getstand(),
                new Rectangle(health[state].PosX, health[state].PosY, health[state].getstand().Width, health[state].getstand().Height), Color.White);


            }
            catch (Exception)
            {
            }
            //text
            spriteBatch.DrawString(lifefont, "X " + character[0].Lives/*getlives()*/, new Vector2(85, 50), Color.Black);
            spriteBatch.DrawString(lifefont, "X " + character[0].Kills/*getkills()*/, new Vector2(80, 215), Color.Black);
            spriteBatch.DrawString(lifefont, "SCORE: " + character[0].Score/*getscore()*/, new Vector2(4, 275), Color.Yellow);
            if (min > 0 || sec > 30)
                spriteBatch.DrawString(lifefont, " " + min + ":" + Math.Truncate(sec), new Vector2(460, 15), Color.White);
            else spriteBatch.DrawString(lifefont, " " + min + ":" + Math.Truncate(sec), new Vector2(460, 15), Color.Red);


            //mouse cursor
            if (character[0].HP/*gethp()*/ > (2 * character[0].HpMax/*gethpmax()*/ / 3))
                spriteBatch.Draw(inplay[4], new Rectangle(Microsoft.Xna.Framework.Input.Mouse.GetState().X - 20, Microsoft.Xna.Framework.Input.Mouse.GetState().Y - 20, inplay[4].Width, inplay[4].Height), Color.Green);
            else if (character[0].HP/*gethp()*/ > (character[0].HpMax/*gethpmax()*/ / 3))
                spriteBatch.Draw(inplay[4], new Rectangle(Microsoft.Xna.Framework.Input.Mouse.GetState().X - 20, Microsoft.Xna.Framework.Input.Mouse.GetState().Y - 20, inplay[4].Width, inplay[4].Height), Color.Yellow);
            else spriteBatch.Draw(inplay[4], new Rectangle(Microsoft.Xna.Framework.Input.Mouse.GetState().X - 20, Microsoft.Xna.Framework.Input.Mouse.GetState().Y - 20, inplay[4].Width, inplay[4].Height), Color.Red);

           
            #endregion

            #region trackers
            double scalew = back.Width / 145;
            double scaleh = back.Height / 97;
            spriteBatch.Draw(blip, new Rectangle((int)Math.Ceiling((character[0].Location.X / scalew) + 4), (int)Math.Ceiling((character[0].Location.Y / scaleh) + 475), 4, 4), Color.Green);
            for (i = 1; i < character.Count; i++)
            {
                spriteBatch.Draw(blip, new Rectangle((int)Math.Ceiling(((character[i].Location.X)  / scalew)+4 ), (int)Math.Ceiling(((character[i].Location.Y)  / scaleh)+475 ), 4, 4), Color.Red);
            }
            //Console.WriteLine("X: " + character[1].Location.X + " Y: " + character[1].Location.Y+" scale: "+scalew+"/"+scaleh);
            #endregion 
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public void addwep(Texture2D val)
        {
            wep.Add(val);
        }
        public void addpow(Texture2D val)
        {
            pow.Add(val);
        }



    }
}
