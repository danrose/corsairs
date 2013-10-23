using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.IO;
using corsairs.game;

namespace corsairs.xna.scenes
{
    public abstract class BaseMenuScene : Scene
    {
        protected SpriteFont textFont;
        protected SpriteBatch spriteBatch;
        protected List<MenuOption> options;
        protected int maxTextWidth;
        protected int maxTextHeight;
        protected Texture2D menuBackground;
        protected int currentlySelected;
        protected TimeSpan lastRespondedToDown;
        protected TimeSpan lastRespondedToUp;

        protected int centerAligningOffset;
        protected int elementHeight;
        protected int textStartPos;

        protected const int hPadding = 60;
        protected const int vPadding = 2;
        protected const int spacing = 5;
        protected const int keyboardThrottle = 200;

        protected void SetCurrentIndex(int index)
        {
            currentlySelected = index;
            for (var i = 0; i < options.Count; i++)
            {
                options[i].Selected = i == index;
            }
        }

        protected BaseMenuScene(Game game)
            : base(game)
        {
        }

        public abstract string MenuTitle { get; }
        public abstract void RegisterMenuItems();

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var keyboard = Keyboard.GetState();
            var mouse = Mouse.GetState();

            if (keyboard.IsKeyDown(Keys.Up))
            {
                if (gameTime.TotalGameTime - lastRespondedToDown > TimeSpan.FromMilliseconds(keyboardThrottle))
                {
                    lastRespondedToDown = gameTime.TotalGameTime;
                    DecrementOption();
               }
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                if (gameTime.TotalGameTime - lastRespondedToUp > TimeSpan.FromMilliseconds(keyboardThrottle))
                {
                    lastRespondedToUp = gameTime.TotalGameTime;
                    IncrementOption();
                }
            }
            if (keyboard.IsKeyDown(Keys.Enter))
            {
                ExecuteAction(options[currentlySelected]);
            }
 
            for(var i = 0; i < options.Count; i++)
            {
                if (options[i].IsWithinBounds(mouse.X, mouse.Y))
                {
                    if (currentlySelected != i)
                    {
                        SetCurrentIndex(i);
                    }
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        ExecuteAction(options[currentlySelected]);
                    }
                    break;
                }
            }
        }

        public void AddMenuOption(string text, Action onExecute)
        {
            if (options == null)
            {
                options = new List<MenuOption>();
            }
            options.Add(new MenuOption(text, onExecute));
        }

        protected virtual void ExecuteAction(MenuOption action)
        {
            var matchedOption = options.FirstOrDefault(x => x.Text == action.Text);
            if (matchedOption != null)
            {
                matchedOption.OnExecute();
            }
        }

        protected void IncrementOption()
        {
            SetCurrentIndex((currentlySelected + 1) % options.Count);
        }

        protected void DecrementOption()
        {
            if (currentlySelected == 0)
            {
                SetCurrentIndex(currentlySelected = options.Count - 1);
            }
            else
            {
                SetCurrentIndex(currentlySelected - 1);
            }
        }

        protected void DrawAppName()
        {
            var midWidth = spriteBatch.GraphicsDevice.Viewport.Width / 2;
            var text = MenuTitle;
            var textWidth = textFont.MeasureString(text).X;

            spriteBatch.DrawString(textFont, text,
                    new Vector2(
                        midWidth - textWidth / 2,
                        5),
                    Color.White);
        }

        protected void RecalculateState()
        {
            var midWidth = Game.GraphicsDevice.Viewport.Width / 2;
            var midHeight = Game.GraphicsDevice.Viewport.Height / 2;
            textStartPos = midWidth - (maxTextWidth / 2);
            elementHeight = spacing + maxTextHeight + vPadding * 2;
            centerAligningOffset = midHeight - (options.Count * elementHeight / 2);
            Console.WriteLine("recalc " + midHeight + " " + options.Count + " " + elementHeight);

            for (var i = 0; i < options.Count; i++)
            {
                options[i].Bounds = new Rectangle(
                    textStartPos - hPadding,
                    centerAligningOffset + (i * elementHeight),
                    maxTextWidth + hPadding * 2,
                    maxTextHeight + vPadding * 2);
            }
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
                     
            for(var i = 0; i < options.Count; i++)
            {
                var currentOption = options[i];
                spriteBatch.Draw(menuBackground, currentOption.Bounds,
                    currentOption.Selected ? Color.Red : Color.CornflowerBlue);

                spriteBatch.DrawString(textFont, currentOption.Text,
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

            options = new List<MenuOption>();
            RegisterMenuItems();

            foreach (var option in options)
            {
                var size = textFont.MeasureString(option.Text);
                if (size.X > maxTextWidth)
                {
                    maxTextWidth = (int)size.X;
                }

                if (size.Y > maxTextHeight)
                {
                    maxTextHeight = (int)size.Y;
                }
            }

            SetCurrentIndex(0);
            RecalculateState();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            textFont = Game.Content.Load<SpriteFont>("MenuFont"); 
        }

        public class MenuOption
        {
            public MenuOption(string text, Action onExecute)
            {
                this.Text = text;
                this.OnExecute = onExecute;
            }

            public string Text { get; set; }
            public Rectangle Bounds { get; set; }
            public bool Selected { get; set; }
            public Action OnExecute { get; set; }
            public bool IsWithinBounds(int x, int y)
            {
                return x >= Bounds.Left && x <= Bounds.Right && y >= Bounds.Top && y <= Bounds.Bottom;
            }
        }
    }

    public class MainMenuScene : BaseMenuScene
    {
        public MainMenuScene(Game game)
            : base(game)
        {
        }

        public override string Name
        {
            get { return SceneNames.MainMenu; }
        }

        public override string MenuTitle
        {
            get { return "Welcome to Corsairs - choose an option"; }
        }

        public override void RegisterMenuItems()
        {
            AddMenuOption("Start New Game", () =>
                {
                    GameState.NewGame = true;
                    GameState.LoadExistingGame = false;
                    SceneManager.ChangeScene(SceneNames.Worldmap);
                });

            if (File.Exists(GameState.GetSaveFile()))
            {
                AddMenuOption("Load Game", () =>
                {
                    GameState.LoadExistingGame = true;
                    SceneManager.ChangeScene(SceneNames.Worldmap);
                });
            }
            AddMenuOption("Quit", () => Game.Exit());
        }
    }
}
