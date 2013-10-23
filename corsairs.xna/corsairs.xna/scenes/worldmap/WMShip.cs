using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using corsairs.core.worldgen;

namespace corsairs.xna.scenes.worldmap
{
    public class WMShip : DrawableGameComponent
    {
        protected Vector2 pos;
        protected Vector2 dest;
        protected Texture2D shipTexture;
        protected SpriteBatch spriteBatch;
        protected WorldMapScene worldMap;
        protected DateTime? leftClicked;

        protected static TimeSpan ClickThreshold = new TimeSpan(0, 0, 0, 0, 100);

        public WMShip(Game game, WorldMapScene worldMap)
            : base(game)
        {
            this.worldMap = worldMap;
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

        /// <summary>
        /// Move the ship to a suitable (i.e. water) start square
        /// </summary>
        public virtual void MoveToStart()
        {
            int x, y;
            bool water = false;
            var seed = new Random();
            Enabled = false;
            do
            {
                var index = seed.Next(worldMap.Map.Locations.Count);
                var loc = worldMap.Map.Locations[index];
                water = loc.IsWater;
                x = loc.X * WorldMapScene.SquareSize;
                y = loc.Y * WorldMapScene.SquareSize;
            } while (!water);

            dest = pos = new Vector2(x, y);
            Enabled = true;
        }

        public virtual void OnActivated()
        {
            Enabled = true;
            Visible = true;
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

        protected virtual void CheckForDestinationUpdate(MouseState mouse)
        {
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                // if left button is clicked then check to see if it's been held longer than 100ms
                // if so then change the destination
                var now = DateTime.Now;
                if (now - leftClicked > ClickThreshold)
                {
                    dest = new Vector2(mouse.X, mouse.Y);
                    leftClicked = null;
                }
                else if (leftClicked == null)
                {
                    // if not previously clicked then capture the baseline
                    leftClicked = DateTime.Now;
                }
            }
            else if (leftClicked != null)
            {
                // no longer clicked so reset the click time
                leftClicked = null;
            }
        }

        protected virtual void MoveToDestination(GameTime gameTime)
        {
            if (dest != Vector2.Zero && dest != pos)
            {
                var direction = dest - pos;

                // finish move if within 5px
                if (direction.ToLength() < 5)
                {
                    pos = dest;
                    return;
                }

                direction.Normalize();
                float speed = 0.1f;

                var distance = direction.Multiply((float)(speed * gameTime.ElapsedGameTime.TotalMilliseconds));
                var destination = pos + distance;
                var destSquare = worldMap.Map.Locations[(int)destination.X / WorldMapScene.SquareSize, (int)destination.Y / WorldMapScene.SquareSize];

                if (destSquare.IsWater)
                {
                    pos += distance;
                }
                else
                {
                    // hit land - stop!
                    dest = pos;
                    return;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var keyboard = Keyboard.GetState();
            var mouse = Mouse.GetState();

            CheckForDestinationUpdate(mouse);
            MoveToDestination(gameTime);   
        }
    }
}
