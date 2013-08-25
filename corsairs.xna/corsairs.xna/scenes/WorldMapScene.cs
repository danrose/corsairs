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

namespace corsairs.xna.scenes
{
    public class WorldMapScene : IScene
    {
        private Random seed = new Random();
        private byte[] spritePriming = new byte[16];
        private Texture2D tileset;
        private WorldMap worldMap;

        private Dictionary<char, Color> colourMap = new Dictionary<char, Color>
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

        public string Name
        {
            get { return SceneNames.Worldmap; }
        }

        public void LoadContent(ContentManager content)
        {
            tileset = content.Load<Texture2D>("tileset");

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
            LoadOceanNaming(content);
            worldMap = Generator.GenerateMap();
            /* using (var writer = saveFileInfo.CreateText())
             {
                 var serialized = FileEncoder.Encode(locations);
                 writer.Write(serialized);
                 writer.Close();
             }
         }*/

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

        public void Update(GameTime gameTime)
        {

        }

        public void Initialise()
        {
            seed.NextBytes(spritePriming);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
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
                    spriteBatch.Draw(tileset, new Rectangle(x * 8, y * 8, 8, 8), new Rectangle((int)location.Biome.DebugSymbol * 8, spriteIndex * 8, 8, 8), Color.White);
                }
            }
        }
    }
}
