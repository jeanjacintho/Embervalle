using System;
using System.Collections.Generic;
using System.Globalization;
using Embervalle.Core.Assets;
using Embervalle.Core.Characters;
using Embervalle.Core.Combat;
using Embervalle.Core.Gameplay;
using Embervalle.Core.Input;
using Embervalle.Core.Inventory;
using Embervalle.Core.Localization;
using Embervalle.Core.Sprites;
using Embervalle.Core.UI;
using Embervalle.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Embervalle.Core
{
    
    
    public class EmbervalleGame : Game
    {
        
        private GraphicsDeviceManager graphicsDeviceManager;

        private readonly GameSessionController session = new(GameSessionState.MainMenu);
        private readonly InputManager inputManager = new();
        private readonly Camera2D worldCamera = new();
        private readonly CombatSession combat = new();

        private PlayerInventory playerInventory = new();

        private ToolbarSlots toolbar = new();

        private int selectedToolbarSlotIndex;

        private bool backpackOpen;

        private (IContainer container, int index)? transferSource;
        private KeyboardState previousKeyboardState;
        private GamePadState previousGamePadState;
        private MouseState previousMouseState;

        private SpriteBatch spriteBatch = null!;
        private SpriteFont font = null!;
        private Texture2D pixel = null!;

        private Texture2D? itemIconAtlasTexture;

        private AssetManager assetManager = null!;
        private readonly PlayerBody player = new();
        private CompositeCharacterComponent? playerComposite;
        private SpriteComponent? playerSprite;
        private PlayerSpriteAnimationController playerAnim = null!;
        private WorldSpriteRenderer worldRenderer = null!;

        private CompositeCharacterRenderer compositeRenderer = null!;

        
        public readonly static bool IsMobile = OperatingSystem.IsAndroid() || OperatingSystem.IsIOS();

        
        public readonly static bool IsDesktop =
            OperatingSystem.IsMacOS() || OperatingSystem.IsLinux() || OperatingSystem.IsWindows();

        
        public EmbervalleGame()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            
            Services.AddService(typeof(GraphicsDeviceManager), graphicsDeviceManager);

            Content.RootDirectory = "Content";

            
            graphicsDeviceManager.SupportedOrientations =
                DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

            IsMouseVisible = true;
        }

        
        protected override void Initialize()
        {
            base.Initialize();

            
            List<CultureInfo> cultures = LocalizationManager.GetSupportedCultures();
            var languages = new List<CultureInfo>();
            for (int i = 0; i < cultures.Count; i++)
            {
                languages.Add(cultures[i]);
            }

            
            var selectedLanguage = LocalizationManager.DEFAULT_CULTURE_CODE;
            LocalizationManager.SetCulture(selectedLanguage);
        }

        
        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/Hud");
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            itemIconAtlasTexture = ItemIconAtlasBuilder.Build(GraphicsDevice, pixel, spriteBatch);
            EmbervalleSheets.LoadItemIcons(
                new SpriteSheet(
                    itemIconAtlasTexture,
                    ItemIconAtlasBuilder.CellSize,
                    ItemIconAtlasBuilder.CellSize));

            assetManager = new AssetManager(Content);
            EmbervalleSheets.Load(assetManager);
            EmbervalleSheets.LoadWeaponIcons(
                assetManager.LoadSheet("Sprites/Items/weapons",WeaponVisualConstants.AtlasFrameSize,WeaponVisualConstants.AtlasFrameSize));

            ItemDatabase.RegisterCoreItems();

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
            itemIconAtlasTexture?.Dispose();
            itemIconAtlasTexture = null;
            pixel.Dispose();
            spriteBatch.Dispose();
            base.UnloadContent();
        }

        
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            MouseState mouseState = Mouse.GetState();

            
            KeyboardState prevKeyboardState = previousKeyboardState;
            GamePadState prevGamePadState = previousGamePadState;
            MouseState prevMouseState = previousMouseState;

            bool escapeJustPressed =
                keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape);
            bool iJustPressed =
                keyboardState.IsKeyDown(Keys.I) && prevKeyboardState.IsKeyUp(Keys.I);
            bool rightClick =
                mouseState.RightButton == ButtonState.Pressed
                && prevMouseState.RightButton == ButtonState.Released;
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
                            playerInventory = new PlayerInventory();
                            toolbar = new ToolbarSlots();
                            NewGameInventoryBootstrap.Apply(playerInventory, toolbar);
                            selectedToolbarSlotIndex = 0;
                            backpackOpen = false;
                            transferSource = null;
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
                    if (session.State == GameSessionState.InGame && backpackOpen)
                    {
                        backpackOpen = false;
                        transferSource = null;
                    }
                    else
                    {
                        if (session.State == GameSessionState.InGame)
                        {
                            backpackOpen = false;
                            transferSource = null;
                        }

                        session.TogglePause();
                    }
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

                if (iJustPressed)
                {
                    backpackOpen = !backpackOpen;
                    if (!backpackOpen)
                    {
                        transferSource = null;
                    }
                }

                if (backpackOpen && rightClick)
                {
                    transferSource = null;
                }

                if (backpackOpen && click)
                {
                    TryHandleBackpackTransfer(mousePoint, vw, vh);
                }

                if (!backpackOpen)
                {
                    if (inputManager.SelectQuickSlot0JustPressed)
                    {
                        selectedToolbarSlotIndex = 0;
                    }

                    if (inputManager.SelectQuickSlot1JustPressed)
                    {
                        selectedToolbarSlotIndex = 1;
                    }

                    
                    if (!combat.IsPlayerMovementLocked)
                    {
                        PlayerWASDMovement.Tick(player, keyboardState, dt, vw, vh);
                    }

                    PlayerCardinalFacing combatFacing = playerAnim.GetCombatFacing(player.LastVelocity);
                    combat.Update(
                        player,
                        worldCamera,
                        inputManager,
                        dt,
                        toolbar,
                        selectedToolbarSlotIndex,
                        combatFacing,
                        vw,
                        vh);

                    var viewRect = new Rectangle(0, 0, vw, vh);
                    if (CompositeSpritePerformance.ShouldUpdateAnimation(viewRect, player.FeetPosition))
                    {
                        playerAnim.Update(
                            dt,
                            player.LastVelocity,
                            attacking: combat.IsAttackAnimationActive,
                            usingTool: false,
                            attackFaceDirection: combat.GetAttackSpriteFaceDirection());
                    }
                }
            }

            
            previousKeyboardState = keyboardState;
            previousGamePadState = gamePadState;
            previousMouseState = mouseState;

            base.Update(gameTime);
        }

        
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
                    DrawToolbarSlots(vh);
                    if (backpackOpen)
                    {
                        DrawBackpackPanel(vw, vh);
                    }

                    DrawGameplayHud(vw, vh);
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
                    DrawToolbarSlots(vh);
                    if (backpackOpen)
                    {
                        DrawBackpackPanel(vw, vh);
                    }

                    DrawGameplayHud(vw, vh);
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
            float baseDepth = worldRenderer.GetLayerDepth(player.FeetPosition.Y);
            if (playerComposite != null && compositeRenderer != null)
            {
                compositeRenderer.Draw(spriteBatch, player.FeetPosition, playerComposite, baseDepth);
            }
            else if (playerSprite != null)
            {
                worldRenderer.DrawEntity(spriteBatch, playerSprite, player.FeetPosition);
            }

            if (combat.ShouldDrawMeleeWeaponOverlay)
            {
                MeleeSwingVisualRenderer.Draw(
                    spriteBatch,
                    EmbervalleSheets.WeaponIcons,
                    combat.MeleeSwingWeaponIconFrame,
                    player.FeetPosition,
                    combat.MeleeSwingFacing,
                    combat.MeleeWeaponOverlayDrawFrame,
                    baseDepth,
                    Color.White);
            }
        }

        private void DrawCombatDebug(int viewportWidth, int viewportHeight)
        {
            Vector2 from = player.FeetPosition;
            Vector2 to = combat.RangedAim.AimWorldPosition;
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
                string state = e.IsHostileAi ? $" {e.HostileState}" : string.Empty;
                string label = $"HP {e.Health.Current}/{e.Health.Max}{state}";
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

        }

        private void DrawGameplayHud(int viewportWidth, int viewportHeight)
        {
            string hud =
                $"HP {player.Health:0}/{player.MaxHealth:0}  Mana {combat.Mana.Current:0}/{combat.Mana.Max:0}  X/C toolbar  LMB use item  I bag  Q spell  Esc";
            spriteBatch.DrawString(font, hud, new Vector2(8, viewportHeight - 52), Color.White * 0.9f);
        }

        private void DrawToolbarSlots(int viewportHeight)
        {
            for (int i = 0; i < ToolbarSlots.SlotCountValue; i++)
            {
                Rectangle r = BackpackScreenLayout.QuickSlotRect(viewportHeight, i);
                bool pickHl = transferSource.HasValue
                    && ReferenceEquals(transferSource.Value.container, toolbar)
                    && transferSource.Value.index == i;
                bool active = !backpackOpen && i == selectedToolbarSlotIndex;
                Color fill = pickHl
                    ? Color.Lerp(Color.Cyan, Color.Black, 0.35f)
                    : active
                        ? Color.Lerp(Color.Gold, Color.Black, 0.45f)
                        : Color.Lerp(Color.Black, Color.White, 0.55f);
                spriteBatch.Draw(pixel, r, fill * 0.65f);

                ItemSlot slot = toolbar.GetSlot(i);
                if (!slot.IsEmpty)
                {
                    DrawItemSpriteInSlot(r, slot.Item);
                    string id = TrimId(slot.Item!.ItemId);
                    spriteBatch.DrawString(
                        font,
                        id,
                        new Vector2(r.X + 2, r.Y + r.Height - 14),
                        Color.White * 0.88f,
                        0f,
                        Vector2.Zero,
                        0.32f,
                        SpriteEffects.None,
                        0f);
                }
            }

            Rectangle r0 = BackpackScreenLayout.QuickSlotRect(viewportHeight, 0);
            Rectangle r1 = BackpackScreenLayout.QuickSlotRect(viewportHeight, 1);
            spriteBatch.DrawString(font, "X", new Vector2(r0.X + 14, r0.Y - 16), Color.White * 0.9f);
            spriteBatch.DrawString(font, "C", new Vector2(r1.X + 14, r1.Y - 16), Color.White * 0.9f);
        }

        private void DrawBackpackPanel(int viewportWidth, int viewportHeight)
        {
            spriteBatch.Draw(
                pixel,
                new Rectangle(0, 0, viewportWidth, viewportHeight),
                Color.Black * 0.5f);

            spriteBatch.DrawString(font, "Backpack (I close, RMB cancel pick)", new Vector2(12, 10), Color.White * 0.92f);

            for (int i = 0; i < PlayerInventory.MainSlots; i++)
            {
                Rectangle cell = BackpackScreenLayout.GridCellRect(viewportWidth, viewportHeight, i);
                bool hl = transferSource.HasValue
                    && ReferenceEquals(transferSource.Value.container, playerInventory)
                    && transferSource.Value.index == i;
                spriteBatch.Draw(
                    pixel,
                    cell,
                    (hl ? Color.Lerp(Color.Cyan, Color.Black, 0.2f) : Color.Lerp(Color.White, Color.Black, 0.6f))
                    * 0.35f);

                ItemSlot sl = playerInventory.GetSlot(i);
                if (!sl.IsEmpty)
                {
                    DrawItemSpriteInSlot(cell, sl.Item);
                    spriteBatch.DrawString(
                        font,
                        TrimId(sl.Item!.ItemId),
                        new Vector2(cell.X + 2, cell.Y + cell.Height - 14),
                        Color.White * 0.88f,
                        0f,
                        Vector2.Zero,
                        0.32f,
                        SpriteEffects.None,
                        0f);
                }
            }
        }

        private void DrawItemSpriteInSlot(Rectangle slotRect, ItemInstance? stack)
        {
            if (stack == null || !ItemDatabase.TryGet(stack.ItemId, out ItemData? def) || def is null)
            {
                return;
            }

            if (def.IconAtlasFrameIndex < 0)
            {
                return;
            }

            SpriteSheet icons = def.Category == ItemCategory.Weapon
                ? EmbervalleSheets.WeaponIcons
                : EmbervalleSheets.ItemIcons;
            Rectangle src = icons.GetFrame(def.IconAtlasFrameIndex);
            float maxW = slotRect.Width - 4;
            float maxH = slotRect.Height * 0.55f;
            float s = MathF.Min(maxW / src.Width, maxH / src.Height);
            float x = slotRect.X + (slotRect.Width - src.Width * s) * 0.5f;
            float y = slotRect.Y + 3f;
            spriteBatch.Draw(
                icons.Texture,
                new Vector2(x, y),
                src,
                Color.White,
                0f,
                Vector2.Zero,
                s,
                SpriteEffects.None,
                0f);
        }

        private void TryHandleBackpackTransfer(Point mouse, int viewportWidth, int viewportHeight)
        {
            int? gridHit = BackpackScreenLayout.HitTestGrid(viewportWidth, viewportHeight, mouse);
            int? quickHit = BackpackScreenLayout.HitTestQuickSlot(viewportHeight, mouse);
            if (gridHit == null && quickHit == null)
            {
                return;
            }

            IContainer targetContainer = gridHit != null ? playerInventory : toolbar;
            int targetIndex = gridHit ?? quickHit!.Value;

            if (transferSource == null)
            {
                transferSource = (targetContainer, targetIndex);
                return;
            }

            (IContainer container, int index) src = transferSource.Value;
            if (ReferenceEquals(src.container, targetContainer) && src.index == targetIndex)
            {
                transferSource = null;
                return;
            }

            TransferSystem.Transfer(src.container, src.index, targetContainer, targetIndex, -1);
            playerInventory.UpdateWeight();
            transferSource = null;
        }

        private static string TrimId(string id) =>
            id.Length <= 14 ? id : id.Substring(0, 14);
    }
}