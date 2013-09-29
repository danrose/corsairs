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
        private static List<Scene> scenes = new List<Scene>();
        private static Scene currentScene;

        public static void RegisterScenes(params Scene[] toRegister)
        {
            foreach (var scene in toRegister)
            {
                scene.Visible = scene.Enabled = false;
                scene.Game.Components.Add(scene);
            }

            scenes.AddRange(toRegister);
        }

        private static Scene GetSceneByName(string sceneName)
        {
            return scenes.FirstOrDefault(x => x.Name == sceneName);
        }

        public static void Init()
        {
            ChangeScene(scenes[0].Name);
        }

        public static void ChangeScene(string sceneName)
        {
            if (currentScene != null)
            {
                currentScene.Visible = currentScene.Enabled = false;
            }

            var scene = GetSceneByName(sceneName);
            if (scene == null)
            {
                throw new Exception("Scene " + sceneName + " not found.");
            }

            currentScene = scene;
            scene.OnActivated();
            scene.Visible = scene.Enabled = true;
        }
    }
}
