using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace corsairs.xna
{
    /// <summary>
    /// Represents a top-level mode of the game
    /// </summary>
    public interface IScene
    {
        string Name { get; }
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Initialise();
        void LoadContent(ContentManager content);
    }
}
