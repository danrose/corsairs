using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using corsairs.core.worldgen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.IO;
using corsairs.core.worldgen.topography;
using Microsoft.Xna.Framework.Input;
using corsairs.game.worldgen;
using corsairs.game;
using corsairs.xna.scenes.worldmap;

namespace corsairs.xna.scenes
{
    public class WorldMapScene : Scene
    {
        protected Random seed = new Random();
        protected byte[] spritePriming = new byte[16];
        protected Texture2D tileset;
        protected WorldMap worldMap;
        public const int SquareSize = 8;
        protected SpriteFont textFont;
        protected SpriteBatch spriteBatch;
        protected WMShip ship;
        

        public WorldMapScene(Game game)
            : base(game)
        {
            
        }

        public WorldMap Map
        {
            get { return worldMap; }
        }

        public override string Name
        {
            get { return SceneNames.Worldmap; }
        }

        public override void FirstLoad()
        {
            ship = new WMShip(Game, this);
            Game.Components.Add(ship);
        }

        public override void OnActivated()
        {
            base.OnActivated();

            var saveFile = GameState.GetSaveFile();
            var saveFileInfo = new FileInfo(saveFile);
            if (GameState.LoadExistingGame && saveFileInfo.Exists)
            {
                Console.WriteLine("Reading saved world from " + saveFile);
                using (var reader = saveFileInfo.OpenText())
                {
                    worldMap = FileEncoder.Decode(reader);
                    reader.Close();
                }
            }
            else
            {
                worldMap = Generator.GenerateMap();
                if (saveFileInfo.Exists)
                {
                    saveFileInfo.Delete();
                }
                using (var writer = saveFileInfo.CreateText())
                {
                    var serialized = FileEncoder.Encode(worldMap);
                    writer.Write(serialized);
                    writer.Close();
                }
            }

            ship.OnActivated();
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            base.OnEnabledChanged(sender, args);

            if (Enabled == false && ship != null)
            {
                ship.Enabled = false;
                ship.Visible = false;
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            tileset = Game.Content.Load<Texture2D>("tileset");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            LoadOceanNaming(Game.Content);

            textFont = Game.Content.Load<SpriteFont>("MapFont");

            sw.Stop();
            Console.WriteLine("Took " + sw.ElapsedMilliseconds + " ms.");
         
        }

        private void LoadOceanNaming(ContentManager content)
        {
            var waterAdjectives = content.Load<string[]>("waterAdjectives");
            var waterNames = content.Load<string[]>("waterNames");
            var waterNouns = content.Load<string[]>("waterNouns");
            var waterPlurals = content.Load<string[]>("waterPlurals");
            var waterPatterns = content.Load<string[]>("waterPatterns");

            OceanNamer.Initialise(waterAdjectives, waterNames, waterNouns, waterPatterns, waterPlurals);
        }

        private double Magnitude(double x, double y)
        {
            return Math.Sqrt((x * x) + (y * y));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var keyboard = Keyboard.GetState();
            var mouse = Mouse.GetState();

            if (keyboard.IsKeyDown(Keys.Escape))
            { 
                SceneManager.ChangeScene(SceneNames.MainMenu);
                return;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            seed.NextBytes(spritePriming);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            var waterFrameOffset = (gameTime.TotalGameTime.Milliseconds / 300) % 3;
            var locations = worldMap.Locations;

            for (var x = 0; x < locations.Size; x++)
            {
                for (var y = 0; y < locations.Size; y++)
                {
                    var location = locations[x, y];
                    var spriteIndex = (spritePriming[(x ^ 37 * y) % 16] + (location.IsWater ? waterFrameOffset : 0)) % 3;
                    //spriteBatch.Draw(texture, new Rectangle(x * 8, y * 8, 8, 8), location.Biome == null ? Color.Black : colourMap[location.Biome.DebugSymbol]);
                    //spriteBatch.Draw(texture, new Rectangle(x * 8, y * 8, 8, 8), new Color(location.IsWater ? 128 : 0, location.IsWater ? 128 : 0, location.IsWater ? 128 : 0));
                    //spriteBatch.Draw(texture, new Rectangle(x * 8, y * 8, 8, 8), new Color(location.Drainage, location.Drainage,location.Drainage));
                    //spriteBatch.Draw(texture, new Rectangle(x * 8, y * 8, 8, 8), new Color(location.Height, location.Height,location.Height));
                    
                    spriteBatch.Draw(tileset, 
                        new Rectangle(x * SquareSize, y * SquareSize, SquareSize, SquareSize), 
                        new Rectangle((int)location.Biome.DebugSymbol * SquareSize, spriteIndex * SquareSize, SquareSize, SquareSize),
                        Color.White);
                }
            }

            var maxDimension = locations.Size * SquareSize;
            foreach (var ocean in worldMap.Oceans)
            {
                var size = textFont.MeasureString(ocean.Name);
                var positionH = maxDimension - ((ocean.CenterH * SquareSize) - (size.Y / 2));
                var positionW = (ocean.CenterW * SquareSize) - (size.X / 2);
                spriteBatch.DrawString(textFont, ocean.Name, 
                    new Vector2(positionW > 0 ? positionW + 2 : 2, positionH > 0 ? positionH + 2 : 2), 
                    Color.Black);
                spriteBatch.DrawString(textFont, ocean.Name,
                    new Vector2(positionW > 0 ? positionW : 0, positionH > 0 ? positionH + 1 : 1),
                    Color.White);
            }

            spriteBatch.End();
        }
    }
}
