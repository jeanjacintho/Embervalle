using System;
using System.Collections.Generic;
using System.Globalization;
using Embervalle.Core.Gameplay;
using Embervalle.Core.Localization;
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

        private readonly GameSessionController _session = new(GameSessionState.InGame);
        private KeyboardState _previousKeyboardState;
        private GamePadState _previousGamePadState;

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

            bool escapeJustPressed =
                keyboardState.IsKeyDown(Keys.Escape) && _previousKeyboardState.IsKeyUp(Keys.Escape);
            bool backJustPressed =
                gamePadState.Buttons.Back == ButtonState.Pressed
                && _previousGamePadState.Buttons.Back == ButtonState.Released;

            if (IsDesktop && escapeJustPressed)
            {
                _session.TogglePause();
            }
            else if (IsMobile && backJustPressed)
            {
                _session.TogglePause();
            }

            // Atalho de desenvolvimento: fechar o jogo no desktop (ESC agora só pausa).
            if (IsDesktop
                && keyboardState.IsKeyDown(Keys.LeftControl)
                && keyboardState.IsKeyDown(Keys.Q)
                && _previousKeyboardState.IsKeyUp(Keys.Q))
            {
                Exit();
            }

            _previousKeyboardState = keyboardState;
            _previousGamePadState = gamePadState;

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
            // Feedback visual rápido: pausado fica mais escuro (além do log no console).
            Color clear = _session.State == GameSessionState.Paused
                ? Color.Lerp(Color.MonoGameOrange, Color.Black, 0.4f)
                : Color.MonoGameOrange;
            GraphicsDevice.Clear(clear);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}