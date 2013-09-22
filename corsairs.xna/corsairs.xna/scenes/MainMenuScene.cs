using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace corsairs.xna.scenes
{
    public class MainMenuScene : Scene
    {
        private SpriteFont textFont;
        private SpriteBatch spriteBatch;
        private List<string> options;
        private int maxTextWidth;
        private int maxTextHeight;
        private Texture2D menuBackground;
        private int currentlySelected;
        private TimeSpan lastRespondedToDown;
        private TimeSpan lastRespondedToUp;

        private const int hPadding = 60;
        private const int vPadding = 2;
        private const int spacing = 5;
        private const int keyboardThrottle = 200;


        public MainMenuScene(Game game)
            : base(game)
        {
        }

        public override string Name
        {
            get { return SceneNames.MainMenu; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Down))
            {
                if (gameTime.TotalGameTime - lastRespondedToDown > TimeSpan.FromMilliseconds(keyboardThrottle))
                {
                    lastRespondedToDown = gameTime.TotalGameTime;
                    DecrementOption();
               }
            }
            if (keyboard.IsKeyDown(Keys.Up))
            {
                if (gameTime.TotalGameTime - lastRespondedToUp > TimeSpan.FromMilliseconds(keyboardThrottle))
                {
                    lastRespondedToUp = gameTime.TotalGameTime;
                    IncrementOption();
                }
            }
            if (keyboard.IsKeyDown(Keys.Enter))
            {
                var command = options[currentlySelected];
                if (command == "Quit")
                {
                    Game.Exit();
                    return;
                }
                if (command == "Start New Game")
                {
                    SceneManager.ChangeScene(SceneNames.Worldmap);
                    return;
                }
            }
        }

        private void IncrementOption()
        {
            currentlySelected = (currentlySelected + 1) % options.Count;
        }

        private void DecrementOption()
        {
            if (currentlySelected == 0)
            {
                currentlySelected = options.Count - 1;
            }
            else
            {
                currentlySelected--;
            }
        }

        private void DrawAppName()
        {
            var midWidth = spriteBatch.GraphicsDevice.Viewport.Width / 2;
            var text = "Welcome to Corsairs - choose an option";
            var textWidth = textFont.MeasureString(text).X;

            spriteBatch.DrawString(textFont, text,
                    new Vector2(
                        midWidth - textWidth / 2,
                        5),
                    Color.White);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            base.Draw(gameTime);
            if (menuBackground == null)
            {
                menuBackground = new Texture2D(Game.GraphicsDevice, 1, 1);
                menuBackground.SetData(new[] { Color.White });
            }

            Game.GraphicsDevice.Clear(Color.Navy);

            DrawAppName();

            var midWidth = Game.GraphicsDevice.Viewport.Width / 2;
            var midHeight = Game.GraphicsDevice.Viewport.Height / 2;
            var textStartPos = midWidth - (maxTextWidth / 2);
            var optionCount = options.Count;
            var elementHeight = spacing + maxTextHeight + vPadding * 2;
            var centerAligningOffset = midHeight - (optionCount * elementHeight / 2);

            for(var i = 0; i < optionCount; i++)
            {
                spriteBatch.Draw(menuBackground, new Rectangle(
                    textStartPos - hPadding,
                    centerAligningOffset + (i * elementHeight), 
                    maxTextWidth + hPadding * 2,
                    maxTextHeight + vPadding * 2), 
                    i == currentlySelected ? Color.Red : Color.CornflowerBlue);

                spriteBatch.DrawString(textFont, options[i],
                    new Vector2(
                        textStartPos,
                        centerAligningOffset + vPadding + i * elementHeight),
                    Color.NavajoWhite);
            }

            spriteBatch.End();
        }

        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            options = new List<string>();
            options.Add("Start New Game");
            options.Add("Quit");

            foreach (var option in options)
            {
                var size = textFont.MeasureString(option);
                if (size.X > maxTextWidth)
                {
                    maxTextWidth = (int)size.X;
                }

                if (size.Y > maxTextHeight)
                {
                    maxTextHeight = (int)size.Y;
                }
            }

            currentlySelected = 0;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            textFont = Game.Content.Load<SpriteFont>("MenuFont"); 
        }
    }
}
