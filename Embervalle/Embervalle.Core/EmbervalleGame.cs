using System;
using System.Collections.Generic;
using System.Globalization;
using Embervalle.Core.Assets;
using Embervalle.Core.Characters;
using Embervalle.Core.Combat;
using Embervalle.Core.Gameplay;
using Embervalle.Core.Input;
using Embervalle.Core.Localization;
using Embervalle.Core.Sprites;
using Embervalle.Core.UI;
using Embervalle.Core.World;
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
        private readonly InputManager inputManager = new();
        private readonly Camera2D worldCamera = new();
        private readonly CombatSession combat = new();
        private KeyboardState previousKeyboardState;
        private GamePadState previousGamePadState;
        private MouseState previousMouseState;

        private SpriteBatch spriteBatch = null!;
        private SpriteFont font = null!;
        private Texture2D pixel = null!;

        private AssetManager assetManager = null!;
        private readonly PlayerBody player = new();
        private CompositeCharacterComponent? playerComposite;
        private SpriteComponent? playerSprite;
        private PlayerSpriteAnimationController playerAnim = null!;
        private WorldSpriteRenderer worldRenderer = null!;

        private CompositeCharacterRenderer compositeRenderer = null!;

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

            assetManager = new AssetManager(Content);
            EmbervalleSheets.Load(assetManager);

            worldRenderer = new WorldSpriteRenderer(pixel, GraphicsDevice.Viewport.Height);

            compositeRenderer = new CompositeCharacterRenderer(pixel);

            CharacterDefinition playerDef = GameCharacterBootstrap.PlayerDefinition;
            ILocomotionAnimationTarget playerLocomotion;
            if (playerDef.VisualKind == CharacterVisualKind.SingleSpriteSheet)
            {
                playerSprite = SpriteCharacterFactory.CreateSingleSprite(playerDef, assetManager);
                playerLocomotion = new SingleSpriteLocomotionAdapter(playerSprite);
            }
            else
            {
                playerComposite = CompositeCharacterFactory.FromDefinition(
                    playerDef,
                    assetManager,
                    EmbervalleSheets.Player);
                playerLocomotion = playerComposite;
            }

            playerAnim = new PlayerSpriteAnimationController(playerLocomotion);
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

            // Snapshot dos estados anteriores para edge detection neste frame.
            KeyboardState prevKeyboardState = previousKeyboardState;
            GamePadState prevGamePadState = previousGamePadState;
            MouseState prevMouseState = previousMouseState;

            bool escapeJustPressed =
                keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape);
            bool backJustPressed =
                gamePadState.Buttons.Back == ButtonState.Pressed
                && prevGamePadState.Buttons.Back == ButtonState.Released;

            bool click =
                mouseState.LeftButton == ButtonState.Pressed
                && prevMouseState.LeftButton == ButtonState.Released;

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
                            combat.ResetDemoTargets(vw, vh);
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

            if (session.State == GameSessionState.InGame)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                inputManager.Update(
                    keyboardState,
                    prevKeyboardState,
                    mouseState,
                    prevMouseState);
                PlayerWASDMovement.Tick(player, keyboardState, dt, vw, vh);
                combat.Update(player, worldCamera, inputManager, dt);

                var viewRect = new Rectangle(0, 0, vw, vh);
                if (CompositeSpritePerformance.ShouldUpdateAnimation(viewRect, player.FeetPosition))
                {
                    playerAnim.Update(
                        dt,
                        player.LastVelocity,
                        attacking: combat.IsAttackAnimationActive,
                        usingTool: false);
                }
            }

            // Atualiza os estados anteriores apenas ao final do frame.
            previousKeyboardState = keyboardState;
            previousGamePadState = gamePadState;
            previousMouseState = mouseState;

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
                    worldRenderer.SetMapHeight(vh);
                    spriteBatch.Begin(
                        SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp);
                    DrawPlayerCharacter();
                    DrawCombatDebug(vw, vh);
                    spriteBatch.End();
                    break;

                case GameSessionState.Paused:
                    GraphicsDevice.Clear(Color.Lerp(Color.MonoGameOrange, Color.Black, 0.45f));
                    worldRenderer.SetMapHeight(vh);
                    spriteBatch.Begin(
                        SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp);
                    DrawPlayerCharacter();
                    DrawCombatDebug(vw, vh);
                    spriteBatch.End();
                    spriteBatch.Begin(
                        SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp);
                    MenuScreens.DrawPauseMenu(spriteBatch, font, pixel, vw, vh, mouse);
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }

        private void DrawPlayerCharacter()
        {
            if (playerComposite != null && compositeRenderer != null)
            {
                float baseDepth = worldRenderer.GetLayerDepth(player.FeetPosition.Y);
                compositeRenderer.Draw(spriteBatch, player.FeetPosition, playerComposite, baseDepth);
            }
            else if (playerSprite != null)
            {
                worldRenderer.DrawEntity(spriteBatch, playerSprite, player.FeetPosition);
            }
        }

        private void DrawCombatDebug(int viewportWidth, int viewportHeight)
        {
            Vector2 from = player.FeetPosition;
            Vector2 to = combat.Aim.AimWorldPosition;
            Vector2 delta = to - from;
            float len = delta.Length();
            if (len > 1f)
            {
                float angle = MathF.Atan2(delta.Y, delta.X);
                spriteBatch.Draw(
                    pixel,
                    position: from,
                    sourceRectangle: null,
                    color: Color.White * 0.45f,
                    rotation: angle,
                    origin: Vector2.Zero,
                    scale: new Vector2(len, 2f),
                    effects: SpriteEffects.None,
                    layerDepth: 0f);
            }

            for (int i = 0; i < combat.Enemies.Count; i++)
            {
                CombatEnemy e = combat.Enemies[i];
                Microsoft.Xna.Framework.Rectangle r = e.Hitbox.GetRect(e.FeetPosition);
                spriteBatch.Draw(pixel, r, Color.Lerp(Color.Magenta, Color.Black, 0.35f) * 0.4f);
                string label = $"HP {e.Health.Current}/{e.Health.Max}";
                spriteBatch.DrawString(font, label, new Vector2(r.X, r.Y - 18), Color.White * 0.85f);
            }

            ReadOnlySpan<ProjectileState> projectiles = combat.Projectiles.AllSlots;
            for (int i = 0; i < projectiles.Length; i++)
            {
                ProjectileState p = projectiles[i];
                if (!p.IsActive)
                {
                    continue;
                }

                spriteBatch.Draw(
                    pixel,
                    new Rectangle((int)p.Position.X - 2, (int)p.Position.Y - 2, 4, 4),
                    p.Kind == ProjectileKind.Spell ? Color.Orange : Color.SandyBrown);
            }

            string hud =
                $"Mana {combat.Mana.Current:0}/{combat.Mana.Max:0} | Arma: {combat.Equipment.MainHand.Id} | Q = magia";
            spriteBatch.DrawString(font, hud, new Vector2(8, viewportHeight - 28), Color.White * 0.9f);
        }
    }
}