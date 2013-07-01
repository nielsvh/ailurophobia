using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using XmlContentReader;

// TODO: replace this with the type you want to write out.
using TWrite = System.String;

namespace XmlContentWriter
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class ObstacleContentWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, string value)
        {
            throw new NotImplementedException();
        }

        protected override void Write(
           ContentWriter output,
           object value)
        {
            output.Write(((Obstacle)value).Location);
            output.Write(((Obstacle)value).Rotation);
            output.Write(((Obstacle)value).TextureAsset);
            output.Write((int)((Obstacle)value).Type);
        }

        public override string GetRuntimeReader(
                TargetPlatform targetPlatform)
        {
            return typeof(ObstacleContentReader).AssemblyQualifiedName;
        }
    }
}
