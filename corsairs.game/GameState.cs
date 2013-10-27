using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using corsairs.core.worldgen;
using System.IO;

namespace corsairs.game
{
    public static class GameState
    {
        public static string GetSaveFile()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "save.foo");
        }
        public static bool LoadExistingGame;
        public static bool NewGame;
        public static WorldMap WorldMap;
        public static int Width = 1024;
        public static int MapWidth = 768;
        public static int Col1Width = Width - MapWidth;
    }
}
