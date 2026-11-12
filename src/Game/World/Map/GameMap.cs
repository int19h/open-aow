// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Graphics;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

public partial class GameMap : HSMap
{

    public GameMap(ECustomNode owner) : base(owner)
    {
        _showMessageEventsStore = new();
        _mapChangedEventsStore = new();
        _scannerIconsChangedEventsStore = new();
        _backHexSelector = new CustomFixedVisibleHexSprite(null);
        _backHexSelector.OnShow = ShowBackHexSelector;
        _frontHexSelector = new CustomFixedVisibleHexSprite(null);
        _frontHexSelector.OnShow = ShowFrontHexSelector;
        _hexSelectorVisible = false;
        ExplorationEnabled = true;
        FogEnabled = true;
        SeatedPlayer = 1;
        FPS = 30;
    }

    public sbyte CurrentPlayer;

    public sbyte SeatedPlayer { get; set; }

    private EventStore<ShowMessageNotification> _showMessageEventsStore;

    public event NotifyEvent<ShowMessageNotification> FShowMessageEvents
    {
        add => _showMessageEventsStore.Add(value);
        remove => _showMessageEventsStore.Remove(value);
    }

    private string _messageText = "";
    private byte _messageFrame;

    public int FPS { get; private set; }
    public int MapChangedLockCount { get; private set; }

    private EventStore<MapChangedNotification> _mapChangedEventsStore;

    public event NotifyEvent<MapChangedNotification> FMapChangedEvents
    {
        add => _mapChangedEventsStore.Add(value);
        remove => _mapChangedEventsStore.Remove(value);
    }

    private EventStore<MapChangedNotification> _scannerIconsChangedEventsStore;

    public event NotifyEvent<MapChangedNotification> FScannerIconsChangedEvents
    {
        add => _scannerIconsChangedEventsStore.Add(value);
        remove => _scannerIconsChangedEventsStore.Remove(value);
    }

    private int _scannerIconsChangedLockCount;
    public bool FogEnabled;
    public bool ExplorationEnabled;

    public bool ShowGrid { get; set; }

    private CustomFixedVisibleHexSprite _backHexSelector;
    private CustomFixedVisibleHexSprite _frontHexSelector;
    private sbyte _selectorX;
    private sbyte _selectorY;
    private bool _hexSelectorVisible;

    // AoWHSMap.PAS:383
    public override void Dispose()
    {
        _showMessageEventsStore.Clear();
        _mapChangedEventsStore.Clear();
        HideHexSelector();
        _backHexSelector?.Dispose();
        _frontHexSelector?.Dispose();
        _scannerIconsChangedEventsStore.Clear();
        base.Dispose();
        _showMessageEventsStore.Clear();
        _mapChangedEventsStore.Clear();
    }

    // AoWHSMap.PAS:396
    public override HexSpriteMapMouse CreateMouse() => new GameMapMouse();

    // AoWHSMap.PAS:440
    public override void TriggerTerrainChangedEvent(in Rect R, sbyte L)
    {
        base.TriggerTerrainChangedEvent(R, L);
        if (MapChangedLockCount == 0)
        {
            TriggerMapChangedEvent(R, L);
        }
    }

    // AoWHSMap.PAS:810
    public override void UpdateStaticScene()
    {
        MapLevel level;
        int maxX;
        int maxY;
        AoWMapField field;
        if (ExplorationEnabled && SeatedPlayer != -1 && !Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            ResetVisibleStaticHexSprite();
            maxX = ViewHexX + ViewWidthHex - 1;
            maxY = ViewHexY + ViewHeightHex - 1;
            level = MapContainer.GetMapLevel(ViewL);
            for (int X = ViewHexX; X <= maxX; X++)
            {
                for (int Y = maxY, last = ViewHexY; Y >= last; Y--)
                {
                    field = (AoWMapField)level.GetField(X, Y);
                    if (field.GetExplored(SeatedPlayer))
                    {
                        field.CopyStaticVisibleHexSprite(StaticVisibleHexSprite);
                    }
                }
            }

            StaticVisibleHexSprite.Sort();
            StaticSceneInvalid = false;
        }
        else
        {
            base.UpdateStaticScene();
        }
    }

    // AoWHSMap.PAS:833
    public override void UpdateDynamicScene()
    {
        MapLevel level;
        int maxX;
        int maxY;
        AoWMapField field;
        if (ExplorationEnabled && !Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            ResetVisibleDynamicHexSprite();
            maxX = ViewHexX + ViewWidthHex - 1;
            maxY = ViewHexY + ViewHeightHex - 1;
            level = MapContainer.GetMapLevel(ViewL);
            if (SeatedPlayer != -1)
            {
                for (int X = ViewHexX; X <= maxX; X++)
                {
                    for (int Y = maxY, last = ViewHexY; Y >= last; Y--)
                    {
                        field = (AoWMapField)level.GetField(X, Y);
                        if (field.GetExplored(SeatedPlayer))
                        {
                            field.CopyDynamicVisibleHexSprite(DynamicVisibleHexSprite);
                        }
                    }
                }

                for (int X = 0, last = DynamicFixedVisibleHexSprite.Count - 1; X <= last; X++)
                {
                    DynamicFixedVisibleHexSprite[X].MakeVisible(DynamicVisibleHexSprite);
                }

                DynamicVisibleHexSprite.Sort();
            }

            DynamicSceneInvalid = false;
        }
        else
        {
            base.UpdateDynamicScene();
        }
    }

    // AoWHSMap.PAS:861
    public override void NewFrame()
    {
        base.NewFrame();
        UpdateShowMessage();
        if (_hexSelectorVisible)
        {
            UpdateHexSelector();
        }
    }

    // AoWHSMap.PAS:759
    public virtual void ShowBackHexSelector(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        if (_selectorX != -1 && _selectorY != -1 && AoWHSMap.Current.Mouse.Capture == AoWHSMap.Current.SelectedArmy)
        {
            HexGeometry.HexToPixel(_selectorX, _selectorY, ref pixelX, ref pixelY);
            pixelX = pixelX - pixelXSurface - 8;
            pixelY = pixelY - pixelYSurface - 32 + 2;
            AoWHSSet.Current.GeneralImages[7].Show(pixelX, pixelY, lockedSurface);
        }
    }

    // AoWHSMap.PAS:772
    public virtual void ShowFrontHexSelector(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        if (_selectorX != -1 && _selectorY != -1 && AoWHSMap.Current.Mouse.Capture == AoWHSMap.Current.SelectedArmy)
        {
            HexGeometry.HexToPixel(_selectorX, _selectorY, ref pixelX, ref pixelY);
            pixelX = pixelX - pixelXSurface - 8;
            pixelY = pixelY - pixelYSurface - 32 + 2;
            AoWHSSet.Current.GeneralImages[6].Show(pixelX, pixelY, lockedSurface);
        }
    }

    // AoWHSMap.PAS:785
    public virtual void UpdateHexSelector()
    {
        int pixelX = default;
        int pixelY = default;
        HexSprite hexSprite;
        if (_hexSelectorVisible)
        {
            _selectorX = (sbyte)Mouse.HexX;
            _selectorY = (sbyte)Mouse.HexY;
            if (_selectorX != -1 && _selectorY != -1)
            {
                HexGeometry.HexToPixel(_selectorX, _selectorY, ref pixelX, ref pixelY);
                hexSprite = MapContainer.GetField(_selectorX, _selectorY, ViewL).GetMaxLevelNoneTransparentHexSprite();
                if (hexSprite != null)
                {
                    if (hexSprite.GetShowPriority() > 1744830464)
                    {
                        var showPriority = hexSprite.GetShowPriority();
                        _frontHexSelector.ShowPriority = showPriority + 100;
                    }
                    else
                    {
                        _frontHexSelector.ShowPriority = 1744830464 + ((pixelY + 32 + 32) << 13) + pixelX;
                    }
                }
                else
                {
                    _frontHexSelector.ShowPriority = 1744830464 + ((pixelY + 32 + 32) << 13) + pixelX;
                }

                _backHexSelector.ShowPriority = 1744830464 + ((pixelY + 32 - 8) << 13) + pixelX;
            }
        }
        else
        {
            _selectorX = -1;
        }
    }

    // AoWHSMap.PAS:590
    public virtual void SetSeatedPlayer(sbyte player)
    {
        SeatedPlayer = player;
    }

    // AoWHSMap.PAS:594
    public virtual void SetCurrentPlayer(sbyte player)
    {
        CurrentPlayer = player;
    }

    // AoWHSMap.PAS:401
    public virtual void SetFogEnabled(bool value)
    {
        if (value != FogEnabled)
        {
            FogEnabled = value;
            InvalidateMap();
        }
    }

    // AoWHSMap.PAS:410
    public virtual void SetExplorationEnabled(bool value)
    {
        if (value != ExplorationEnabled)
        {
            ExplorationEnabled = value;
            StaticSceneInvalid = true;
            InvalidateMap();
        }
    }

    // AoWHSMap.PAS:871
    public virtual void DropHeroItem(Hero hero, Item item)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        hero.GetLocation(ref X, ref Y, ref L);
        item.PlaceOnMap(this, X, Y, L);
    }

    // AoWHSMap.PAS:878
    public virtual ItemList GetHeroItemsOnGround(Hero hero)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        ItemHexSprite items;
        ItemList result = null;
        hero.GetLocation(ref X, ref Y, ref L);
        if (X != -1)
        {
            items = (ItemHexSprite)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindOwnedChild(131697);
            if (items != null)
            {
                result = items.Items;
            }
        }

        return result;
    }

    // AoWHSMap.PAS:578
    public virtual void ShowMessage(string text)
    {
        var notification = new ShowMessageNotification();
        ref var info = ref notification.Info;
        if (!GameTables.GameEngine.Registry.GetDebugMode() && !IsEmpty(text))
        {
            AudioPlayback.PlaySample("SFX\\EVENTS\\INFO1.WAV");
        }

        info.Text = text;
        info.Color = 16777215;
        _showMessageEventsStore.Raise(notification);
        _messageText = text;
        _messageFrame = 128;
    }

    // AoWHSMap.PAS:566
    public virtual void ShowWarning(string text)
    {
        var notification = new ShowMessageNotification();
        ref var info = ref notification.Info;
        if (!GameTables.GameEngine.Registry.GetDebugMode() && !IsEmpty(text))
        {
            AudioPlayback.PlaySample("SFX\\EVENTS\\INFO2.WAV");
        }

        info.Text = text;
        info.Color = 16777215;
        _showMessageEventsStore.Raise(notification);
        _messageText = text;
        _messageFrame = 160;
    }

    // AoWHSMap.PAS:420
    public void PlayCenterAnimation(sbyte X, sbyte Y, sbyte L)
    {
        ImageSequenceAnimation animation;
        animation = new ImageSequenceAnimation(null);
        animation.SetImageSequence(AoWHSSet.Current.ImageSequences.GetImageSequence(11));
        PlaceHex(animation, X, Y, L);
        AudioPlayback.PlaySample("SFX\\INT\\CENTER.WAV");
    }

    // AoWHSMap.PAS:430
    public void LockMapChangedEvent()
    {
        MapChangedLockCount += 1;
    }

    // AoWHSMap.PAS:435
    public void UnlockMapChangedEvent()
    {
        Assert(MapChangedLockCount != 0, "Invalid MapChanged Lock usage", "AoWHSMap.PAS", 437);
        MapChangedLockCount -= 1;
    }

    // AoWHSMap.PAS:445
    public void MapChanged(in Rect R, int L)
    {
        if (MapChangedLockCount == 0 && Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            TriggerTerrainChangedEvent(R, (sbyte)L);
        }
    }

    // AoWHSMap.PAS:451
    public void MapChangedRadius(int X, int Y, int L, int radius)
    {
        Rect R = default;
        MapLevel level;
        level = MapContainer.GetMapLevel(L);
        R.Left = X - radius;
        if (R.Left < 0)
        {
            R.Left = 0;
        }

        R.Right = X + radius + 1;
        if (R.Right > level.Width)
        {
            R.Right = level.Width;
        }

        R.Top = Y - radius;
        if (R.Top < 0)
        {
            R.Top = 0;
        }

        R.Bottom = Y + radius + 1;
        if (R.Bottom > level.Height)
        {
            R.Bottom = level.Height;
        }

        TerrainChanged(R, (sbyte)L);
    }

    // AoWHSMap.PAS:468
    public void InvalidateMap()
    {
        Rect R = default;
        R.Left = 0;
        R.Right = MapContainer.Width;
        R.Top = 0;
        R.Bottom = MapContainer.Height;
        for (int I = 0, last = MapContainer.LevelCount - 1; I <= last; I++)
        {
            MapChanged(R, I);
        }
    }

    // AoWHSMap.PAS:480
    public void TriggerMapChangedEvent(in Rect R, int L)
    {
        var notification = new MapChangedNotification();
        ref var info = ref notification.Info;
        info.Bounds = R;
        info.Level = L;
        var mapLevel = MapContainer.GetMapLevel(L);
        info.MapLevel = mapLevel;
        _mapChangedEventsStore.Raise(notification);
    }

    // AoWHSMap.PAS:490
    public void LockScannerIconsChangedEvent()
    {
        _scannerIconsChangedLockCount += 1;
    }

    // AoWHSMap.PAS:495
    public void UnlockScannerIconsChangedEvent()
    {
        Assert(_scannerIconsChangedLockCount != 0, "Invalid ScannerIcons Lock usage", "AoWHSMap.PAS", 497);
        _scannerIconsChangedLockCount -= 1;
    }

    // AoWHSMap.PAS:500
    public void ScannerIconsChanged(in Rect R, int L)
    {
        if (_scannerIconsChangedLockCount == 0 && (Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Activated, HexSpriteMapStatusElement.Destroying) & Status) == Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Activated))
        {
            TriggerScannerIconsChangedEvent(R, L);
        }
    }

    // AoWHSMap.PAS:506
    public void ScannerIconsChangedRadius(int hexX, int hexY, int hexLevel, int radius)
    {
        Rect R = default;
        MapLevel level;
        level = MapContainer.GetMapLevel(hexLevel);
        R.Left = hexX - radius;
        if (R.Left < 0)
        {
            R.Left = 0;
        }

        R.Right = hexX + radius + 1;
        if (R.Right > level.Width)
        {
            R.Right = level.Width;
        }

        R.Top = hexY - radius;
        if (R.Top < 0)
        {
            R.Top = 0;
        }

        R.Bottom = hexY + radius + 1;
        if (R.Bottom > level.Height)
        {
            R.Bottom = level.Height;
        }

        ScannerIconsChanged(R, hexLevel);
    }

    // AoWHSMap.PAS:522
    public void InvalidateScannerIcons()
    {
        Rect R = default;
        R.Left = 0;
        R.Right = MapContainer.Width;
        R.Top = 0;
        R.Bottom = MapContainer.Height;
        for (int I = 0, last = MapContainer.LevelCount - 1; I <= last; I++)
        {
            ScannerIconsChanged(R, I);
        }
    }

    // AoWHSMap.PAS:533
    public void TriggerScannerIconsChangedEvent(in Rect R, int L)
    {
        var notification = new MapChangedNotification();
        ref var info = ref notification.Info;
        info.Bounds = R;
        info.Level = L;
        var mapLevel = MapContainer.GetMapLevel(L);
        info.MapLevel = mapLevel;
        _scannerIconsChangedEventsStore.Raise(notification);
    }

    // AoWHSMap.PAS:542
    public void UpdateShowMessage()
    {
        var notification = new ShowMessageNotification();
        ref var info = ref notification.Info;
        if (_messageFrame != 0)
        {
            _messageFrame = (byte)(_messageFrame - 1);
            if (((_messageFrame & 16) == 0 && (_messageFrame & 15) < 7) || ((_messageFrame & 16) == 16 && (_messageFrame & 15) < 11))
            {
                info.Text = _messageText;
                _messageFrame = (byte)(_messageFrame & 240);
                if ((_messageFrame & 16) == 0)
                {
                    info.Color = 3487029;
                }
                else
                {
                    info.Color = 16777215;
                }

                _showMessageEventsStore.Raise(notification);
            }

            if (_messageFrame == 0)
            {
                info.Text = "";
                info.Color = 16777215;
                _showMessageEventsStore.Raise(notification);
            }
        }
    }

    // AoWHSMap.PAS:598
    public bool FogArea(int X, int Y, int level, int range)
    {
        Assert(range <= 15, "Invalid Unfog Range", "AoWHSMap.PAS", 600);
        bool result = ((AoWMapLevel)MapContainer.GetMapLevel(level)).FogArea(X, Y, range);
        if (result && MapChangedLockCount == 0 && FogEnabled)
        {
            MapChangedRadius(X, Y, level, range);
        }

        return result;
    }

    // AoWHSMap.PAS:605
    public bool UnFogArea(int X, int Y, int level, int range)
    {
        Assert(range <= 15, "Invalid Unfog Range", "AoWHSMap.PAS", 607);
        bool result = ((AoWMapLevel)MapContainer.GetMapLevel(level)).UnFogArea(X, Y, range);
        if (result && MapChangedLockCount == 0 && FogEnabled)
        {
            MapChangedRadius(X, Y, level, range);
        }

        return result;
    }

    // AoWHSMap.PAS:612
    public bool ExploreArea(int X, int Y, int level, sbyte player, int range)
    {
        bool result;
        if (!Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            result = ((AoWMapLevel)MapContainer.GetMapLevel(level)).ExploreArea(X, Y, player, range);
            if (result && MapChangedLockCount == 0 && FogEnabled)
            {
                MapChangedRadius(X, Y, level, range);
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWHSMap.PAS:622
    public Set<FogExplorationChangedElement> UnFogExploreArea(int X, int Y, int level, sbyte player, int range)
    {
        Set<FogExplorationChangedElement> result = default;
        int radius;
        // { Preserve the original assertion diagnostic, independent of the build path. }
        radius = range;
        if (radius > 15)
        {
            /* TODO: Implement the inline assembly operation. */
            throw new NotImplementedException();
        }

        // { Original leaves Result undefined when a fog-only operation changes nothing. }
        if (!Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            if (player >= 0)
            {
                result = ((AoWMapLevel)MapContainer.GetMapLevel(level)).UnFogExploreArea(X, Y, player, radius);
            }
            else
            {
                if (((AoWMapLevel)MapContainer.GetMapLevel(level)).UnFogArea(X, Y, radius))
                {
                    result = Set<FogExplorationChangedElement>.Of(FogExplorationChangedElement.Fog);
                }
            }
        }
        else
        {
            if (((AoWMapLevel)MapContainer.GetMapLevel(level)).UnFogArea(X, Y, radius))
            {
                result = Set<FogExplorationChangedElement>.Of(FogExplorationChangedElement.Fog);
            }
        }

        if (MapChangedLockCount == 0 && ((result.Contains(FogExplorationChangedElement.Fog) && FogEnabled) || (result.Contains(FogExplorationChangedElement.Exploration) && ExplorationEnabled && player == SeatedPlayer)))
        {
            MapChangedRadius(X, Y, level, radius);
        }

        return result;
    }

    // AoWHSMap.PAS:738
    public void ShowHexSelector()
    {
        if (!_hexSelectorVisible)
        {
            _hexSelectorVisible = true;
            DynamicFixedVisibleHexSprite.AddChild(_backHexSelector);
            DynamicFixedVisibleHexSprite.AddChild(_frontHexSelector);
            UpdateHexSelector();
        }
    }

    // AoWHSMap.PAS:748
    public void HideHexSelector()
    {
        if (_hexSelectorVisible)
        {
            DynamicVisibleHexSprite.RemoveChild(_backHexSelector);
            DynamicVisibleHexSprite.RemoveChild(_frontHexSelector);
            DynamicFixedVisibleHexSprite.RemoveChild(_backHexSelector);
            DynamicFixedVisibleHexSprite.RemoveChild(_frontHexSelector);
            _hexSelectorVisible = false;
        }
    }

    // AoWHSMap.PAS:867
    public void UpdateFPS(int fPS)
    {
        FPS = fPS;
    }

    // AoWHSMap.PAS:892
    public void DrawSelectionRectangle(int pixelXSurface, int pixelYSurface, in Rect R, in LockedSurface lockedSurface)
    {
        int color;
        color = 2134015;
        if (R.Left > 0 && R.Top > 0 && pixelXSurface < R.Right && pixelYSurface < R.Bottom && R.Right < ViewWidth + pixelXSurface && R.Bottom < ViewHeight + pixelYSurface)
        {
            DrawLine(R.Left - pixelXSurface, R.Top - pixelYSurface, R.Right - pixelXSurface, R.Top - pixelYSurface, 0, color, lockedSurface);
            DrawLine(R.Right - pixelXSurface, R.Top - pixelYSurface, R.Right - pixelXSurface, R.Bottom - pixelYSurface, 0, color, lockedSurface);
            DrawLine(R.Right - pixelXSurface, R.Bottom - pixelYSurface, R.Left - pixelXSurface, R.Bottom - pixelYSurface, 0, color, lockedSurface);
            DrawLine(R.Left - pixelXSurface, R.Bottom - pixelYSurface, R.Left - pixelXSurface, R.Top - pixelYSurface, 0, color, lockedSurface);
        }
    }

    // AoWHSMap.PAS:666 advances a pointer one row. GetField retains that row order.
    public void DrawFog(in LockedSurface surface)
    {
        bool exploration = ExplorationEnabled || AoWHSMap.Current.SeatedPlayer == -1;
        if (!FogEnabled && !exploration) return;
        var level = MapContainer.GetMapLevel(ViewL);
        for (int x = ViewHexX; x < ViewHexX + ViewWidthHex; x++)
        {
            int pixelX = 0, pixelY = 0;
            HexGeometry.HexToPixel(x, ViewHexY, ref pixelX, ref pixelY);
            pixelX -= ViewPixelX + 8;
            pixelY -= ViewPixelY + 4;
            for (int y = ViewHexY; y < ViewHexY + ViewHeightHex; y++, pixelY += 32)
            {
                var field = (AoWMapField)level.GetField(x, y);
                bool unexplored = exploration && (SeatedPlayer == -1 || !field.GetExplored(SeatedPlayer));
                if (unexplored) AoWHSSet.Current.GeneralImages[3].Show(pixelX, pixelY, surface);
                else if (FogEnabled && ((field.FieldFlags >> 16) & 255) == 0)
                    AoWHSSet.Current.GeneralImages[1].Show(pixelX, pixelY, surface);
            }
        }
    }

    public void DrawGrid(in LockedSurface surface)
    {
        var level = MapContainer.GetMapLevel(ViewL);
        int maxX = ViewHexX + ViewWidthHex - 1, maxY = ViewHexY + ViewHeightHex - 1;
        for (int x = ViewHexX; x <= maxX; x++)
        {
            int pixelX = 0, pixelY = 0;
            HexGeometry.HexToPixel(x, ViewHexY, ref pixelX, ref pixelY);
            pixelX = pixelX - ViewPixelX - 8;
            pixelY = pixelY - ViewPixelY - 4 + 4;
            for (int y = ViewHexY; y <= maxY; y++, pixelY += 32)
                if (level.GetField(x, y).TerrainType != 15)
                    AoWHSSet.Current.FlagControl.Flags[7].Show(pixelX, pixelY, surface);
        }
    }

    private static void DrawLine(int x1, int y1, int x2, int y2, int skip, int color, in LockedSurface lockedSurface)
    {
        int pixelFormatId = lockedSurface.PixelFormatId;
        int number = pixelFormatId;
        if (number == 1431343888 || number == 1431409424 || number == 1448121104)
        {
            int number2 = PixelFormats.Color2PixelInfo(color, lockedSurface.PixelFormatId, null);
            DrawLine16(x1, y1, x2, y2, 0, (ushort)number2, in lockedSurface);
        }
    }

    private static void DrawLine16(int x1, int y1, int x2, int y2, int skip, ushort color, in LockedSurface lockedSurface)
    {
        Blit.DrawLine16(x1, y1, x2, y2, skip, color, lockedSurface.View());
    }
}
