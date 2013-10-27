using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using corsairs.core.worldgen;
using corsairs.game;

namespace corsairs.xna.scenes.worldmap
{
    public class WMShip : DrawableGameComponent
    {
        protected WorldMapScene worldMap;

        // position and waypoints
        protected List<Vector2> waypoints = new List<Vector2>();
        protected int currentWaypoint;
        protected List<Vector2> dots = new List<Vector2>();
        protected Vector2 pos;
        protected Vector2 velocity;
        protected Vector2 startOfPath;

        // graphics
        protected Texture2D shipTexture;
        protected SpriteBatch spriteBatch;
        protected Texture2D lineTexture;

        // user input
        protected DateTime? leftClicked;  
        protected static TimeSpan ClickThreshold = new TimeSpan(0, 0, 0, 0, 100);

        public WMShip(Game game, WorldMapScene worldMap)
            : base(game)
        {
            this.worldMap = worldMap;
            DrawOrder = 9999;
        }

        public virtual void OnActivated()
        {
            if (GameState.NewGame)
            {
                GameState.NewGame = false;
                MoveToStart();
            }

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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var keyboard = Keyboard.GetState();
            var mouse = Mouse.GetState();

            if (mouse.X >= GameState.Col1Width && mouse.X <= GameState.Width && mouse.Y >= 0 && mouse.Y <= GameState.MapWidth)
            {
                CheckForDestinationUpdate(mouse, keyboard);
            }

            if (waypoints.Any())
            {
                MoveToDestination(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.Draw(gameTime);

            foreach (var dot in dots)
            {
                spriteBatch.Draw(lineTexture, new Rectangle((int)dot.X, (int)dot.Y, 8, 8),
                    new Rectangle(24, 24, 8, 8),
                    Color.White);
            }

            if (waypoints.Any())
            {
                var finalDestination = waypoints.Last();
                spriteBatch.Draw(lineTexture, new Rectangle((int)finalDestination.X - 11, (int)finalDestination.Y - 10, 22, 19),
                     new Rectangle(0, 0, 22, 19),
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
            bool water = false;
            var seed = new Random();
            Enabled = false;
            do
            {
                var index = seed.Next(worldMap.Map.Locations.Count);
                var loc = worldMap.Map.Locations[index];
                water = loc.IsWater;
                pos = WorldMapScene.GetPixelFromCoord(loc.X, loc.Y);
            } while (!water);

            waypoints.Clear();
            dots.Clear();
            Enabled = true;
        }

        protected virtual void CheckForDestinationUpdate(MouseState mouse, KeyboardState keyboard)
        {
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                // if left button is clicked then check to see if it's been held longer than 100ms
                // if so then change the destination
                var now = DateTime.Now;
                if (now - leftClicked > ClickThreshold)
                {
                    // shift enqueues waypoints
                    if (!keyboard.IsKeyDown(Keys.LeftShift | Keys.RightShift))
                    {
                        waypoints.Clear();
                        currentWaypoint = 0;
                    }

                    if (waypoints.Count == 0)
                    {
                        // this enables the current path segment to be drawn rather than always coming from the ship
                        startOfPath = pos;
                    }

                    waypoints.Add(new Vector2(mouse.X, mouse.Y));
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

        /// <summary>
        /// Creates a series of points between two vectors
        /// </summary>
        protected virtual Vector2[] DrawPath(Vector2 from, Vector2 to)
        {
            var direction = to - from;
            var normDirection = direction;
            normDirection.Normalize();

            // draw new path
            var step = direction.Multiply(1f / 6);
            var ret = new Vector2[6];
            for (var i = 1; i < 7; i++)
            {
                ret[i - 1] = from + step.Multiply(i);
            }

            return ret;
        }

        /// <summary>
        /// Reset our velocity vector based on speed and the normalised
        /// direction vector to the first destination in the waypoint
        /// queue
        /// </summary>
        protected virtual void RecalculateVelocity()
        {
            if (waypoints.Count == 0)
            {
                return;
            }

            float speed = 0.1f;
            var direction = waypoints[currentWaypoint] - pos;
            var normDirection = direction;
            normDirection.Normalize();
            velocity = speed * normDirection;
        }

        /// <summary>
        /// Redraw our destination and pathing dots
        /// </summary>
        protected virtual void ResetDestination()
        {
            dots.Clear();
            if (waypoints.Count == 0)
            {
                return;
            }

            RecalculateVelocity();

            Vector2 prev = startOfPath;
            foreach (var waypoint in waypoints)
            {
                dots.AddRange(DrawPath(prev, waypoint));
                prev = dots[dots.Count - 1];
            }
        }

        protected virtual void CollideWithLand()
        {
            waypoints.Clear();
            ResetDestination();
        }

        /// <summary>
        /// Increment movement towards our destination, if we have one
        /// </summary>
        protected virtual void MoveToDestination(GameTime gameTime)
        {
            var dest = waypoints[currentWaypoint];
            var direction = dest - pos;

            // finish move if within 5px
            if (direction.ToLength() < 5)
            {
                pos = dest;
                if (currentWaypoint == waypoints.Count - 1)
                {
                    // if the last waypoint then empty the list and reset
                    waypoints.Clear();
                    currentWaypoint = 0;
                }
                else
                {
                    currentWaypoint++;
                }
                // recalc velocity so we move in the direction of the new waypoint
                RecalculateVelocity();
                return;
            }

            var distance = velocity.Multiply((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            var destination = pos + distance;

            // convert from pixel location to map square
            var destIndex = WorldMapScene.GetCoordFromPixel(destination);
            var destSquare = worldMap.Map.Locations[(int)destIndex.X, (int)destIndex.Y];

            if (destSquare.IsWater)
            {
                pos += distance;
            }
            else
            {
                CollideWithLand();
                return;
            }
        }
    }
}
