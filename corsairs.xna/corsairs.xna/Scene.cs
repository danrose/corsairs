using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace corsairs.xna
{
    /// <summary>
    /// Represents a top-level mode of the game
    /// </summary>
    public abstract class Scene : DrawableGameComponent
    {
        protected Scene(Game game) : base(game)
        {
        }

        public abstract string Name { get; }

        public virtual void OnActivated()
        {
        }
    }
}
