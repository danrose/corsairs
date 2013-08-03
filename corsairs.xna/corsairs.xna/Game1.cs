using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using corsairs.core.worldgen;
using System.Diagnostics;
using System.IO;

namespace corsairs.xna
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture;
        ArrayMap<Location> locations;
        Random seed = new Random();
        byte[] spritePriming = new byte[16];

        Dictionary<char, Color> colourMap = new Dictionary<char, Color>
        {
            {'b', Color.Cornsilk},
            {'#', Color.DarkBlue},
            {'~', Color.Navy},
            {'.', Color.Blue},
            {'g', Color.LightGreen},
            {'h', Color.Red},
            {'m', Color.DarkSeaGreen},
            {'M', Color.White},
            {'f', Color.DarkGreen},
            {'i', Color.White},
            {'G', Color.Green},
            {'p', Color.LightGoldenrodYellow},
            {'r', Color.Gray},
            {'s', Color.Brown},
            {'t', Color.GhostWhite}
        };

        Texture2D tileset;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            //graphics.PreferredBackBufferHeight = 1024;
            //graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 512;
            graphics.PreferredBackBufferWidth = 512;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            seed.NextBytes(spritePriming);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var saveFile = Path.Combine(Directory.GetCurrentDirectory(), "save.foo");
            /* var saveFileInfo = new FileInfo(saveFile);
             if (saveFileInfo.Exists)
             {
                 Console.WriteLine("Reading saved world from " + saveFile);
                 using (var reader = saveFileInfo.OpenText())
                 {
                     locations = FileEncoder.Decode(reader);
                     reader.Close();
                 }
             }
             else
             {*/
            Console.WriteLine("Writing new world to " + saveFile);
            locations = Generator.GenerateMap();
            /* using (var writer = saveFileInfo.CreateText())
             {
                 var serialized = FileEncoder.Encode(locations);
                 writer.Write(serialized);
                 writer.Close();
             }
         }*/

            sw.Stop();
            Console.WriteLine("Took " + sw.ElapsedMilliseconds + " ms.");
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });

            tileset = Content.Load<Texture2D>("tileset");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            for (var x = 0; x < locations.Size; x++)
            {
                for (var y = 0; y < locations.Size; y++)
                {
                    var location = locations[x, y];
                    var spriteIndex = spritePriming[(x ^ 37 * y) % 16] % 3;
                    //spriteBatch.Draw(texture, new Rectangle(x * 8, y * 8, 8, 8), location.Biome == null ? Color.Black : colourMap[location.Biome.DebugSymbol]);
                    //spriteBatch.Draw(texture, new Rectangle(x * 8, y * 8, 8, 8), new Color(location.IsWater ? 128 : 0, location.IsWater ? 128 : 0, location.IsWater ? 128 : 0));
                    //spriteBatch.Draw(texture, new Rectangle(x * 8, y * 8, 8, 8), new Color(location.Drainage, location.Drainage,location.Drainage));
                    //spriteBatch.Draw(texture, new Rectangle(x * 8, y * 8, 8, 8), new Color(location.Height, location.Height,location.Height));
                    spriteBatch.Draw(tileset, new Rectangle(x * 8, y * 8, 8, 8), new Rectangle((int)location.Biome.DebugSymbol * 8, spriteIndex * 8, 8, 8), Color.White);
                }
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
