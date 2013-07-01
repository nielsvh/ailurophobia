using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace XmlContentReader
{
    public class ObstacleContentReader:ContentTypeReader<Obstacle>
    {
        protected override Obstacle Read(ContentReader input, Obstacle existingInstance)
        {
            Obstacle sprite = new Obstacle();

            sprite.Location = input.ReadVector2();
            sprite.Rotation = input.ReadSingle();
            sprite.TextureAsset = input.ReadString();
            sprite.Type = (Obstacle.objtype)input.ReadInt32();

            sprite.Load(input.ContentManager);
            
            return sprite;
        }
    }
}
