using System;
using System.Collections.Generic;
using System.Globalization;
using Embervalle.Core.Gameplay;
using Embervalle.Core.Localization;
using Embervalle.Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Embervalle.Core
{
    /// <summary>
    /// The main class for the game, responsible for managing game components, settings, 
    /// and platform-specific configurations.
    /// </summary>
    public class EmbervalleGame : Game
    {
        // Resources for drawing.
        private GraphicsDeviceManager graphicsDeviceManager;

        private readonly GameSessionController _session = new(GameSessionState.MainMenu);
        private KeyboardState _previousKeyboardState;
        private GamePadState _previousGamePadState;
        private MouseState _previousMouseState;

        private SpriteBatch _spriteBatch = null!;
        private SpriteFont _font = null!;
        private Texture2D _pixel = null!;

        /// <summary>
        /// Indicates if the game is running on a mobile platform.
        /// </summary>
        public readonly static bool IsMobile = OperatingSystem.IsAndroid() || OperatingSystem.IsIOS();

        /// <summary>
        /// Indicates if the game is running on a desktop platform.
        /// </summary>
        public readonly static bool IsDesktop =
            OperatingSystem.IsMacOS() || OperatingSystem.IsLinux() || OperatingSystem.IsWindows();

        /// <summary>
        /// Initializes a new instance of the game. Configures platform-specific settings, 
        /// initializes services like settings and leaderboard managers, and sets up the 
        /// screen manager for screen transitions.
        /// </summary>
        public EmbervalleGame()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Share GraphicsDeviceManager as a service.
            Services.AddService(typeof(GraphicsDeviceManager), graphicsDeviceManager);

            Content.RootDirectory = "Content";

            // Configure screen orientations.
            graphicsDeviceManager.SupportedOrientations =
                DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the game, including setting up localization and adding the 
        /// initial screens to the ScreenManager.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Load supported languages and set the default language.
            List<CultureInfo> cultures = LocalizationManager.GetSupportedCultures();
            var languages = new List<CultureInfo>();
            for (int i = 0; i < cultures.Count; i++)
            {
                languages.Add(cultures[i]);
            }

            // TODO You should load this from a settings file or similar,
            // based on what the user or operating system selected.
            var selectedLanguage = LocalizationManager.DEFAULT_CULTURE_CODE;
            LocalizationManager.SetCulture(selectedLanguage);
        }

        /// <summary>
        /// Loads game content, such as textures and particle systems.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Fonts/Hud");
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        protected override void UnloadContent()
        {
            _pixel.Dispose();
            _spriteBatch.Dispose();
            base.UnloadContent();
        }

        /// <summary>
        /// Updates the game's logic, called once per frame.
        /// </summary>
        /// <param name="gameTime">
        /// Provides a snapshot of timing values used for game updates.
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            MouseState mouseState = Mouse.GetState();

            bool escapeJustPressed =
                keyboardState.IsKeyDown(Keys.Escape) && _previousKeyboardState.IsKeyUp(Keys.Escape);
            bool backJustPressed =
                gamePadState.Buttons.Back == ButtonState.Pressed
                && _previousGamePadState.Buttons.Back == ButtonState.Released;

            bool click =
                mouseState.LeftButton == ButtonState.Pressed
                && _previousMouseState.LeftButton == ButtonState.Released;

            int vw = GraphicsDevice.Viewport.Width;
            int vh = GraphicsDevice.Viewport.Height;
            Point mousePoint = mouseState.Position;

            GameSessionState stateAtStart = _session.State;
            bool suppressPauseToggleFromEsc = false;

            if (click)
            {
                if (stateAtStart == GameSessionState.MainMenu)
                {
                    switch (MenuScreens.HitTestMainMenu(vw, vh, mousePoint))
                    {
                        case MainMenuHit.NewGame:
                            _session.SetState(GameSessionState.InGame);
                            suppressPauseToggleFromEsc = true;
                            break;
                        case MainMenuHit.Exit:
                            Exit();
                            break;
                    }
                }
                else if (stateAtStart == GameSessionState.Paused)
                {
                    switch (MenuScreens.HitTestPauseMenu(vw, vh, mousePoint))
                    {
                        case PauseMenuHit.Continue:
                            _session.SetState(GameSessionState.InGame);
                            suppressPauseToggleFromEsc = true;
                            break;
                        case PauseMenuHit.ExitToMainMenu:
                            _session.SetState(GameSessionState.MainMenu);
                            break;
                    }
                }
            }

            if (!suppressPauseToggleFromEsc)
            {
                if (IsDesktop && escapeJustPressed)
                {
                    _session.TogglePause();
                }
                else if (IsMobile && backJustPressed)
                {
                    _session.TogglePause();
                }
            }

            _previousKeyboardState = keyboardState;
            _previousGamePadState = gamePadState;
            _previousMouseState = mouseState;

            if (_session.State == GameSessionState.InGame)
            {
                // TODO: simulação, input do jogador, sistemas — só roda com o jogo "em curso".
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game's graphics, called once per frame.
        /// </summary>
        /// <param name="gameTime">
        /// Provides a snapshot of timing values used for rendering.
        /// </param>
        protected override void Draw(GameTime gameTime)
        {
            int vw = GraphicsDevice.Viewport.Width;
            int vh = GraphicsDevice.Viewport.Height;
            Point mouse = Mouse.GetState().Position;

            switch (_session.State)
            {
                case GameSessionState.MainMenu:
                    GraphicsDevice.Clear(new Color(25, 32, 48));
                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    MenuScreens.DrawMainMenu(_spriteBatch, _font, _pixel, vw, vh, mouse);
                    _spriteBatch.End();
                    break;

                case GameSessionState.InGame:
                    GraphicsDevice.Clear(Color.MonoGameOrange);
                    break;

                case GameSessionState.Paused:
                    GraphicsDevice.Clear(Color.Lerp(Color.MonoGameOrange, Color.Black, 0.45f));
                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    MenuScreens.DrawPauseMenu(_spriteBatch, _font, _pixel, vw, vh, mouse);
                    _spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}