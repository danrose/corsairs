using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace corsairs.xna.scenes.worldmap
{
    public class WMShip : DrawableGameComponent
    {
        protected Vector2 pos;
        protected Vector2 dest;
        protected Texture2D shipTexture;
        protected SpriteBatch spriteBatch;

        public WMShip(Game game)
            : base(game)
        {
            DrawOrder = 9999;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.Draw(gameTime);

            spriteBatch.Draw(shipTexture, new Rectangle((int)pos.X, (int)pos.Y, 23, 29),
               new Rectangle(0, 0, 23, 29),
               Color.White);

            spriteBatch.End();
        }

        public virtual void OnActivated()
        {
            Enabled = true;
            dest = pos = new Vector2(256, 256);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            shipTexture = Game.Content.Load<Texture2D>("ship");
        }

        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var keyboard = Keyboard.GetState();
            var mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                dest = new Vector2(mouse.X, mouse.Y);
            }

            if (dest != Vector2.Zero && dest != pos)
            {
                var direction = dest - pos;

                if (direction.ToLength() < 5)
                {
                    pos = dest;
                    return;
                }

                direction.Normalize();
                float speed = 0.1f;

                var distance = direction.Multiply((float)(speed * gameTime.ElapsedGameTime.TotalMilliseconds));
                pos += distance;
            }
        }
    }
}
