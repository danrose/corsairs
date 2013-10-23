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
        protected Vector2 velocity;
        protected Texture2D shipTexture;
        protected SpriteBatch spriteBatch;
        protected WorldMapScene worldMap;
        protected DateTime? leftClicked;
        protected List<Vector2> dots = new List<Vector2>();
        protected Texture2D lineTexture;
        protected bool moving;

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

            if (moving)
            {
                spriteBatch.Draw(lineTexture, new Rectangle((int)dest.X - 11, (int)dest.Y - 10, 22, 19),
                     new Rectangle(0, 0, 22, 19),
                     Color.White);
            }

            foreach (var dot in dots)
            {
                spriteBatch.Draw(lineTexture, new Rectangle((int)dot.X, (int)dot.Y, 8, 8),
                    new Rectangle(24, 24, 8, 8),
                    Color.White);
            }

            spriteBatch.Draw(shipTexture, new Rectangle((int)pos.X - 11, (int)pos.Y - 14, 23, 29),
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
            lineTexture = Game.Content.Load<Texture2D>("dots");
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
                    ResetDestination();
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

        protected virtual void ResetDestination()
        {
            // work out new velocity vector
            var direction = dest - pos; 
            var normDirection = direction;
            normDirection.Normalize();
            float speed = 0.1f;
            velocity = speed * normDirection;  

            // clear old path
            dots.Clear();

            // draw new path
            var step = direction.Multiply(1f / 10);
            for (var i = 1; i < 10; i++)
            {
                dots.Add(pos + step.Multiply(i));
            }
        }

        protected virtual void MoveToDestination(GameTime gameTime)
        {
            var direction = dest - pos;

            // finish move if within 5px
            if (direction.ToLength() < 5)
            {
                pos = dest;
                return;
            }

            var distance = velocity.Multiply((float)gameTime.ElapsedGameTime.TotalMilliseconds);
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var keyboard = Keyboard.GetState();
            var mouse = Mouse.GetState();

            moving = dest != Vector2.Zero && dest != pos;

            CheckForDestinationUpdate(mouse);

            if (moving)
            {
                MoveToDestination(gameTime);
            }
        }
    }
}
