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
    public static class SceneManager
    {
        private static List<IScene> scenes = new List<IScene>();
        private static IScene currentScene;

        public static void RegisterScenes(params IScene[] toRegister)
        {
            scenes.AddRange(toRegister);
        }

        public static void Initialise(Game game)
        {
            foreach (var scene in scenes)
            {
                scene.Initialise(game);
            }
        }

        public static void LoadContent(ContentManager content)
        {
            foreach (var scene in scenes)
            {
                scene.LoadContent(content);
            }

            currentScene = scenes[0];
        }

        public static void Update(GameTime gameTime, KeyboardState keyboard)
        {
            currentScene.Update(gameTime, keyboard);
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            currentScene.Draw(gameTime, spriteBatch);
        }

        private static IScene GetSceneByName(string sceneName)
        {
            return scenes.FirstOrDefault(x => x.Name == sceneName);
        }

        public static void ChangeScene(string sceneName)
        {
            if (currentScene != null)
            {
                currentScene.OnHide();
            }

            var scene = GetSceneByName(sceneName);
            if (scene == null)
            {
                throw new Exception("Scene " + sceneName + " not found.");
            }

            currentScene = scene;
            scene.OnShow();
        }
    }
}
