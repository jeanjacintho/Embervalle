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

        private readonly GameSessionController session = new(GameSessionState.MainMenu);
        private KeyboardState previousKeyboardState;
        private GamePadState previousGamePadState;
        private MouseState previousMouseState;

        private SpriteBatch spriteBatch = null!;
        private SpriteFont font = null!;
        private Texture2D pixel = null!;

        private readonly PlayerBody player = new();
        private static readonly Color PlayerColor = new(34, 100, 68);

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

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/Hud");
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        protected override void UnloadContent()
        {
            pixel.Dispose();
            spriteBatch.Dispose();
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
                keyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape);
            bool backJustPressed =
                gamePadState.Buttons.Back == ButtonState.Pressed
                && previousGamePadState.Buttons.Back == ButtonState.Released;

            bool click =
                mouseState.LeftButton == ButtonState.Pressed
                && previousMouseState.LeftButton == ButtonState.Released;

            int vw = GraphicsDevice.Viewport.Width;
            int vh = GraphicsDevice.Viewport.Height;
            Point mousePoint = mouseState.Position;

            GameSessionState stateAtStart = session.State;
            bool suppressPauseToggleFromEsc = false;

            if (click)
            {
                if (stateAtStart == GameSessionState.MainMenu)
                {
                    switch (MenuScreens.HitTestMainMenu(vw, vh, mousePoint))
                    {
                        case MainMenuHit.NewGame:
                            PlayerWASDMovement.SpawnCentered(player, vw, vh);
                            session.SetState(GameSessionState.InGame);
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
                            session.SetState(GameSessionState.InGame);
                            suppressPauseToggleFromEsc = true;
                            break;
                        case PauseMenuHit.ExitToMainMenu:
                            session.SetState(GameSessionState.MainMenu);
                            break;
                    }
                }
            }

            if (!suppressPauseToggleFromEsc)
            {
                if (IsDesktop && escapeJustPressed)
                {
                    session.TogglePause();
                }
                else if (IsMobile && backJustPressed)
                {
                    session.TogglePause();
                }
            }

            previousKeyboardState = keyboardState;
            previousGamePadState = gamePadState;
            previousMouseState = mouseState;

            if (session.State == GameSessionState.InGame)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                PlayerWASDMovement.Tick(player, keyboardState, dt, vw, vh);
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

            switch (session.State)
            {
                case GameSessionState.MainMenu:
                    GraphicsDevice.Clear(new Color(25, 32, 48));
                    spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    MenuScreens.DrawMainMenu(spriteBatch, font, pixel, vw, vh, mouse);
                    spriteBatch.End();
                    break;

                case GameSessionState.InGame:
                    GraphicsDevice.Clear(Color.MonoGameOrange);
                    spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    DrawPlayer();
                    spriteBatch.End();
                    break;

                case GameSessionState.Paused:
                    GraphicsDevice.Clear(Color.Lerp(Color.MonoGameOrange, Color.Black, 0.45f));
                    spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    DrawPlayer();
                    MenuScreens.DrawPauseMenu(spriteBatch, font, pixel, vw, vh, mouse);
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }

        private void DrawPlayer()
        {
            var rect = new Rectangle(
                (int)MathF.Round(player.Position.X),
                (int)MathF.Round(player.Position.Y),
                player.Size,
                player.Size);
            spriteBatch.Draw(pixel, rect, PlayerColor);
        }
    }
}