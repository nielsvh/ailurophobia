using System;
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
    class obj
    {
        string type;            //the type of in-game object, albeit a monster, chracter or level boundary block
          //y component of hit box
//        char dir;               //direction of obj

//        string statusout;       //status collision will invoke

        Texture2D stand;
        //string status;          //present effect obj is under or action being taken
        int posX, posY;

        public obj()
        {
            
        }
        public obj(string type, int posx, int posy,
                      Texture2D stand)
        {
            this.posX = posx;
            this.posY = posy;
            this.type = type;
            //this.status = "";
            this.stand = stand;
        }
        
        public Texture2D getstand()
        {
            return stand;
        }
        public string gettype()
        {
            return type;
        }
        //public string getstatus()
        //{
        //    return status;
        //}
        public int PosX
        {
            get { return posX; }
        }
        public int PosY
        {
            get { return posY; }
        }
    }
}
