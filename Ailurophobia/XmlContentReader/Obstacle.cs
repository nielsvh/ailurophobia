using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace XmlContentReader
{
    public class Obstacle
    {
        public enum objtype { SOFA, SPAWN };
        objtype type;
        Vector2 location;
        float rotation;

        string textureAsset;
        Texture2D texture;

        public objtype Type
        {
            get { return type; }
            set { type = value; }
        }

        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public string TextureAsset
        {
            get { return textureAsset; }
            set { textureAsset = value; }
        }

        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return texture; }
        }

        public void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>(textureAsset);
        }
    }
}