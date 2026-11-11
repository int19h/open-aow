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
        FShowMessageEventsStore = new();
        FMapChangedEventsStore = new();
        FScannerIconsChangedEventsStore = new();
        FBackHexagonSelector = new CustomFixedVisibleHexSprite(null);
        FBackHexagonSelector.OnShow = ShowBackHexagonSelector;
        FFrontHexagonSelector = new CustomFixedVisibleHexSprite(null);
        FFrontHexagonSelector.OnShow = ShowFrontHexagonSelector;
        FHexagonSelectorVisible = false;
        FExplorationEnabled = true;
        FFogEnabled = true;
        FSeatedPlayer = 1;
        FFPS = 30;
    }

    public sbyte FCurrentPlayer;
    public sbyte FSeatedPlayer;

    internal EventStore<ShowMessageNotification> FShowMessageEventsStore;

    public event NotifyEvent<ShowMessageNotification> FShowMessageEvents
    {
        add => FShowMessageEventsStore.Add(value);
        remove => FShowMessageEventsStore.Remove(value);
    }

    public string FMessageText = "";
    public byte FMessageFrame;
    public int FFPS;
    public int FMapChangedLockCount;

    internal EventStore<MapChangedNotification> FMapChangedEventsStore;

    public event NotifyEvent<MapChangedNotification> FMapChangedEvents
    {
        add => FMapChangedEventsStore.Add(value);
        remove => FMapChangedEventsStore.Remove(value);
    }

    internal EventStore<MapChangedNotification> FScannerIconsChangedEventsStore;

    public event NotifyEvent<MapChangedNotification> FScannerIconsChangedEvents
    {
        add => FScannerIconsChangedEventsStore.Add(value);
        remove => FScannerIconsChangedEventsStore.Remove(value);
    }

    public int FScannerIconsChangedLockCount;
    public bool FFogEnabled;
    public bool FExplorationEnabled;
    public bool FShowGrid;
    public CustomFixedVisibleHexSprite FBackHexagonSelector;
    public CustomFixedVisibleHexSprite FFrontHexagonSelector;
    public sbyte FSelectorX;
    public sbyte FSelectorY;
    public bool FHexagonSelectorVisible;

    // AoWHSMap.PAS:383
    public override void Dispose()
    {
        FShowMessageEventsStore.Clear();
        FMapChangedEventsStore.Clear();
        HideHexagonSelector();
        FBackHexagonSelector?.Dispose();
        FFrontHexagonSelector?.Dispose();
        FScannerIconsChangedEventsStore.Clear();
        base.Dispose();
        FShowMessageEventsStore.Clear();
        FMapChangedEventsStore.Clear();
    }

    // AoWHSMap.PAS:396
    public override HexSpriteMapMouse CreateMouse() => new GameMapMouse();

    // AoWHSMap.PAS:440
    public override void TriggerTerrainChangedEvent(in Rect R, sbyte L)
    {
        base.TriggerTerrainChangedEvent(R, L);
        if (FMapChangedLockCount == 0)
        {
            TriggerMapChangedEvent(R, L);
        }
    }

    // AoWHSMap.PAS:810
    public override void UpdateStaticScene()
    {
        MapLevel Level;
        int MaxX;
        int MaxY;
        AoWMapField Field;
        if (FExplorationEnabled && FSeatedPlayer != -1 && !Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            ResetVisibleStaticHS();
            MaxX = ViewXhx + ViewWidthHX - 1;
            MaxY = ViewYhx + ViewHeightHX - 1;
            Level = MapContainer.GetMapLevel(ViewL);
            for (int X = ViewXhx; X <= MaxX; X++)
            {
                for (int Y = MaxY, last = ViewYhx; Y >= last; Y--)
                {
                    Field = (AoWMapField)Level.GetField(X, Y);
                    if (Field.GetExplored(FSeatedPlayer))
                    {
                        Field.CopyStaticVisibleHS(StaticVisibleHS);
                    }
                }
            }

            StaticVisibleHS.Sort();
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
        MapLevel Level;
        int MaxX;
        int MaxY;
        AoWMapField Field;
        if (FExplorationEnabled && !Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            ResetVisibleDynamicHS();
            MaxX = ViewXhx + ViewWidthHX - 1;
            MaxY = ViewYhx + ViewHeightHX - 1;
            Level = MapContainer.GetMapLevel(ViewL);
            if (FSeatedPlayer != -1)
            {
                for (int X = ViewXhx; X <= MaxX; X++)
                {
                    for (int Y = MaxY, last = ViewYhx; Y >= last; Y--)
                    {
                        Field = (AoWMapField)Level.GetField(X, Y);
                        if (Field.GetExplored(FSeatedPlayer))
                        {
                            Field.CopyDynamicVisibleHS(DynamicVisibleHS);
                        }
                    }
                }

                for (int X = 0, last = DynamicFixedVisibleHS.Count - 1; X <= last; X++)
                {
                    ((HexSprite)DynamicFixedVisibleHS.GetChild(X)).MakeVisible(DynamicVisibleHS);
                }

                DynamicVisibleHS.Sort();
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
        if (FHexagonSelectorVisible)
        {
            UpdateHexagonSelector();
        }
    }

    // AoWHSMap.PAS:759
    public virtual void ShowBackHexagonSelector(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        if (FSelectorX != -1 && FSelectorY != -1 && AoWHSMap.Current.Mouse.Capture == AoWHSMap.Current.FSelectedArmy)
        {
            HexGeometry.HXtoHP(FSelectorX, FSelectorY, ref Xhp, ref Yhp);
            Xhp = Xhp - XhpSurf - 8;
            Yhp = Yhp - YhpSurf - 32 + 2;
            AoWHSSet.Current.FGeneralImages[7].Show(Xhp, Yhp, SurfaceDesc);
        }
    }

    // AoWHSMap.PAS:772
    public virtual void ShowFrontHexagonSelector(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        if (FSelectorX != -1 && FSelectorY != -1 && AoWHSMap.Current.Mouse.Capture == AoWHSMap.Current.FSelectedArmy)
        {
            HexGeometry.HXtoHP(FSelectorX, FSelectorY, ref Xhp, ref Yhp);
            Xhp = Xhp - XhpSurf - 8;
            Yhp = Yhp - YhpSurf - 32 + 2;
            AoWHSSet.Current.FGeneralImages[6].Show(Xhp, Yhp, SurfaceDesc);
        }
    }

    // AoWHSMap.PAS:785
    public virtual void UpdateHexagonSelector()
    {
        int Xhp = default;
        int Yhp = default;
        HexSprite HS;
        if (FHexagonSelectorVisible)
        {
            FSelectorX = (sbyte)Mouse.Xhx;
            FSelectorY = (sbyte)Mouse.Yhx;
            if (FSelectorX != -1 && FSelectorY != -1)
            {
                HexGeometry.HXtoHP(FSelectorX, FSelectorY, ref Xhp, ref Yhp);
                HS = MapContainer.GetField(FSelectorX, FSelectorY, ViewL).GetMaxLevelNoneTransparentHS();
                if (HS != null)
                {
                    if (HS.GetShowPriority() > 1744830464)
                    {
                        var showPriority = HS.GetShowPriority();
                        FFrontHexagonSelector.ShowPriority = showPriority + 100;
                    }
                    else
                    {
                        FFrontHexagonSelector.ShowPriority = 1744830464 + ((Yhp + 32 + 32) << 13) + Xhp;
                    }
                }
                else
                {
                    FFrontHexagonSelector.ShowPriority = 1744830464 + ((Yhp + 32 + 32) << 13) + Xhp;
                }

                FBackHexagonSelector.ShowPriority = 1744830464 + ((Yhp + 32 - 8) << 13) + Xhp;
            }
        }
        else
        {
            FSelectorX = -1;
        }
    }

    // AoWHSMap.PAS:590
    public virtual void SetSeatedPlayer(sbyte Player)
    {
        FSeatedPlayer = Player;
    }

    // AoWHSMap.PAS:594
    public virtual void SetCurrentPlayer(sbyte Player)
    {
        FCurrentPlayer = Player;
    }

    // AoWHSMap.PAS:401
    public virtual void SetFogEnabled(bool Value)
    {
        if (Value != FFogEnabled)
        {
            FFogEnabled = Value;
            InvalidateMap();
        }
    }

    // AoWHSMap.PAS:410
    public virtual void SetExplorationEnabled(bool Value)
    {
        if (Value != FExplorationEnabled)
        {
            FExplorationEnabled = Value;
            StaticSceneInvalid = true;
            InvalidateMap();
        }
    }

    // AoWHSMap.PAS:871
    public virtual void DropHeroItem(Hero Hero, Item Item)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Hero.GetLocation(ref X, ref Y, ref L);
        Item.PlaceOnMap(this, X, Y, L);
    }

    // AoWHSMap.PAS:878
    public virtual ItemList GetHeroItemsOnGround(Hero Hero)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        ItemHexSprite Items;
        ItemList Result = null;
        Hero.GetLocation(ref X, ref Y, ref L);
        if (X != -1)
        {
            Items = (ItemHexSprite)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindOwnedChild(131697);
            if (Items != null)
            {
                Result = Items.FItems;
            }
        }

        return Result;
    }

    // AoWHSMap.PAS:578
    public virtual void ShowMessage(string Text)
    {
        var notification = new ShowMessageNotification();
        ref var Info = ref notification.Info;
        if (!GameTables.AoWEngine.FRegistry.GetDebugMode() && !IsEmpty(Text))
        {
            AudioPlayback.PlaySample("SFX\\EVENTS\\INFO1.WAV");
        }

        Info.Text = Text;
        Info.Color = 16777215;
        FShowMessageEventsStore.Raise(notification);
        FMessageText = Text;
        FMessageFrame = 128;
    }

    // AoWHSMap.PAS:566
    public virtual void ShowWarning(string Text)
    {
        var notification = new ShowMessageNotification();
        ref var Info = ref notification.Info;
        if (!GameTables.AoWEngine.FRegistry.GetDebugMode() && !IsEmpty(Text))
        {
            AudioPlayback.PlaySample("SFX\\EVENTS\\INFO2.WAV");
        }

        Info.Text = Text;
        Info.Color = 16777215;
        FShowMessageEventsStore.Raise(notification);
        FMessageText = Text;
        FMessageFrame = 160;
    }

    // AoWHSMap.PAS:420
    public void PlayCenterAnimation(sbyte X, sbyte Y, sbyte L)
    {
        ImageSequenceAnimation Animation;
        Animation = new ImageSequenceAnimation(null);
        Animation.SetImageSequence(AoWHSSet.Current.FImageSequences.GetImageSequence(11));
        PlaceHX(Animation, X, Y, L);
        AudioPlayback.PlaySample("SFX\\INT\\CENTER.WAV");
    }

    // AoWHSMap.PAS:430
    public void LockMapChangedEvent()
    {
        FMapChangedLockCount += 1;
    }

    // AoWHSMap.PAS:435
    public void UnlockMapChangedEvent()
    {
        Assert(FMapChangedLockCount != 0, "Invalid MapChanged Lock usage", "AoWHSMap.PAS", 437);
        FMapChangedLockCount -= 1;
    }

    // AoWHSMap.PAS:445
    public void MapChanged(in Rect R, int L)
    {
        if (FMapChangedLockCount == 0 && Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            TriggerTerrainChangedEvent(R, (sbyte)L);
        }
    }

    // AoWHSMap.PAS:451
    public void MapChangedRad(int X, int Y, int L, int Radius)
    {
        Rect R = default;
        MapLevel Level;
        Level = MapContainer.GetMapLevel(L);
        R.Left = X - Radius;
        if (R.Left < 0)
        {
            R.Left = 0;
        }

        R.Right = X + Radius + 1;
        if (R.Right > Level.Width)
        {
            R.Right = Level.Width;
        }

        R.Top = Y - Radius;
        if (R.Top < 0)
        {
            R.Top = 0;
        }

        R.Bottom = Y + Radius + 1;
        if (R.Bottom > Level.Height)
        {
            R.Bottom = Level.Height;
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
        ref var Info = ref notification.Info;
        Info.Bounds = R;
        Info.Level = L;
        var mapLevel = MapContainer.GetMapLevel(L);
        Info.MapLevel = mapLevel;
        FMapChangedEventsStore.Raise(notification);
    }

    // AoWHSMap.PAS:490
    public void LockScannerIconsChangedEvent()
    {
        FScannerIconsChangedLockCount += 1;
    }

    // AoWHSMap.PAS:495
    public void UnlockScannerIconsChangedEvent()
    {
        Assert(FScannerIconsChangedLockCount != 0, "Invalid ScannerIcons Lock usage", "AoWHSMap.PAS", 497);
        FScannerIconsChangedLockCount -= 1;
    }

    // AoWHSMap.PAS:500
    public void ScannerIconsChanged(in Rect R, int L)
    {
        if (FScannerIconsChangedLockCount == 0 && (Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Activated, HexSpriteMapStatusElement.Destroying) & Status) == Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Activated))
        {
            TriggerScannerIconsChangedEvent(R, L);
        }
    }

    // AoWHSMap.PAS:506
    public void ScannerIconsChangedRad(int Xhx, int Yhx, int Lhx, int Radius)
    {
        Rect R = default;
        MapLevel Level;
        Level = MapContainer.GetMapLevel(Lhx);
        R.Left = Xhx - Radius;
        if (R.Left < 0)
        {
            R.Left = 0;
        }

        R.Right = Xhx + Radius + 1;
        if (R.Right > Level.Width)
        {
            R.Right = Level.Width;
        }

        R.Top = Yhx - Radius;
        if (R.Top < 0)
        {
            R.Top = 0;
        }

        R.Bottom = Yhx + Radius + 1;
        if (R.Bottom > Level.Height)
        {
            R.Bottom = Level.Height;
        }

        ScannerIconsChanged(R, Lhx);
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
        ref var Info = ref notification.Info;
        Info.Bounds = R;
        Info.Level = L;
        var mapLevel = MapContainer.GetMapLevel(L);
        Info.MapLevel = mapLevel;
        FScannerIconsChangedEventsStore.Raise(notification);
    }

    // AoWHSMap.PAS:542
    public void UpdateShowMessage()
    {
        var notification = new ShowMessageNotification();
        ref var Info = ref notification.Info;
        if (FMessageFrame != 0)
        {
            FMessageFrame = (byte)(FMessageFrame - 1);
            if (((FMessageFrame & 16) == 0 && (FMessageFrame & 15) < 7) || ((FMessageFrame & 16) == 16 && (FMessageFrame & 15) < 11))
            {
                Info.Text = FMessageText;
                FMessageFrame = (byte)(FMessageFrame & 240);
                if ((FMessageFrame & 16) == 0)
                {
                    Info.Color = 3487029;
                }
                else
                {
                    Info.Color = 16777215;
                }

                FShowMessageEventsStore.Raise(notification);
            }

            if (FMessageFrame == 0)
            {
                Info.Text = "";
                Info.Color = 16777215;
                FShowMessageEventsStore.Raise(notification);
            }
        }
    }

    // AoWHSMap.PAS:598
    public bool FogArea(int X, int Y, int Level, int Range)
    {
        Assert(Range <= 15, "Invalid Unfog Range", "AoWHSMap.PAS", 600);
        bool Result = ((AoWMapLevel)MapContainer.GetMapLevel(Level)).FogArea(X, Y, Range);
        if (Result && FMapChangedLockCount == 0 && FFogEnabled)
        {
            MapChangedRad(X, Y, Level, Range);
        }

        return Result;
    }

    // AoWHSMap.PAS:605
    public bool UnFogArea(int X, int Y, int Level, int Range)
    {
        Assert(Range <= 15, "Invalid Unfog Range", "AoWHSMap.PAS", 607);
        bool Result = ((AoWMapLevel)MapContainer.GetMapLevel(Level)).UnFogArea(X, Y, Range);
        if (Result && FMapChangedLockCount == 0 && FFogEnabled)
        {
            MapChangedRad(X, Y, Level, Range);
        }

        return Result;
    }

    // AoWHSMap.PAS:612
    public bool ExploreArea(int X, int Y, int Level, sbyte Player, int Range)
    {
        bool Result;
        if (!Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            Result = ((AoWMapLevel)MapContainer.GetMapLevel(Level)).ExploreArea(X, Y, Player, Range);
            if (Result && FMapChangedLockCount == 0 && FFogEnabled)
            {
                MapChangedRad(X, Y, Level, Range);
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWHSMap.PAS:622
    public Set<FogExplorationChangedElement> UnFogExploreArea(int X, int Y, int Level, sbyte Player, int Range)
    {
        Set<FogExplorationChangedElement> Result = default;
        int Radius;
        // { Preserve the original assertion diagnostic, independent of the build path. }
        Radius = Range;
        if (Radius > 15)
        {
            /* TODO: Implement the inline assembly operation. */
            throw new NotImplementedException();
        }

        // { Original leaves Result undefined when a fog-only operation changes nothing. }
        if (!Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            if (Player >= 0)
            {
                Result = ((AoWMapLevel)MapContainer.GetMapLevel(Level)).UnFogExploreArea(X, Y, Player, Radius);
            }
            else
            {
                if (((AoWMapLevel)MapContainer.GetMapLevel(Level)).UnFogArea(X, Y, Radius))
                {
                    Result = Set<FogExplorationChangedElement>.Of(FogExplorationChangedElement.Fog);
                }
            }
        }
        else
        {
            if (((AoWMapLevel)MapContainer.GetMapLevel(Level)).UnFogArea(X, Y, Radius))
            {
                Result = Set<FogExplorationChangedElement>.Of(FogExplorationChangedElement.Fog);
            }
        }

        if (FMapChangedLockCount == 0 && ((Result.Contains(FogExplorationChangedElement.Fog) && FFogEnabled) || (Result.Contains(FogExplorationChangedElement.Exploration) && FExplorationEnabled && Player == FSeatedPlayer)))
        {
            MapChangedRad(X, Y, Level, Radius);
        }

        return Result;
    }

    // AoWHSMap.PAS:738
    public void ShowHexagonSelector()
    {
        if (!FHexagonSelectorVisible)
        {
            FHexagonSelectorVisible = true;
            DynamicFixedVisibleHS.AddChild(FBackHexagonSelector);
            DynamicFixedVisibleHS.AddChild(FFrontHexagonSelector);
            UpdateHexagonSelector();
        }
    }

    // AoWHSMap.PAS:748
    public void HideHexagonSelector()
    {
        if (FHexagonSelectorVisible)
        {
            DynamicVisibleHS.RemoveChild(FBackHexagonSelector);
            DynamicVisibleHS.RemoveChild(FFrontHexagonSelector);
            DynamicFixedVisibleHS.RemoveChild(FBackHexagonSelector);
            DynamicFixedVisibleHS.RemoveChild(FFrontHexagonSelector);
            FHexagonSelectorVisible = false;
        }
    }

    // AoWHSMap.PAS:867
    public void UpdateFPS(int FPS)
    {
        FFPS = FPS;
    }

    // AoWHSMap.PAS:892
    public void DrawSelectionRect(int XhpSurf, int YhpSurf, in Rect R, in LockedSurface SurfaceDesc)
    {
        int Color;
        Color = 2134015;
        if (R.Left > 0 && R.Top > 0 && XhpSurf < R.Right && YhpSurf < R.Bottom && R.Right < ViewWidth + XhpSurf && R.Bottom < ViewHeight + YhpSurf)
        {
            DrawLine(R.Left - XhpSurf, R.Top - YhpSurf, R.Right - XhpSurf, R.Top - YhpSurf, 0, Color, SurfaceDesc);
            DrawLine(R.Right - XhpSurf, R.Top - YhpSurf, R.Right - XhpSurf, R.Bottom - YhpSurf, 0, Color, SurfaceDesc);
            DrawLine(R.Right - XhpSurf, R.Bottom - YhpSurf, R.Left - XhpSurf, R.Bottom - YhpSurf, 0, Color, SurfaceDesc);
            DrawLine(R.Left - XhpSurf, R.Bottom - YhpSurf, R.Left - XhpSurf, R.Top - YhpSurf, 0, Color, SurfaceDesc);
        }
    }

    // AoWHSMap.PAS:666 advances a pointer one row. GetField retains that row order.
    public void DrawFog(in LockedSurface surface)
    {
        bool exploration = FExplorationEnabled || AoWHSMap.Current.FSeatedPlayer == -1;
        if (!FFogEnabled && !exploration) return;
        var level = MapContainer.GetMapLevel(ViewL);
        for (int x = ViewXhx; x < ViewXhx + ViewWidthHX; x++)
        {
            int xhp = 0, yhp = 0;
            HexGeometry.HXtoHP(x, ViewYhx, ref xhp, ref yhp);
            xhp -= ViewXhp + 8;
            yhp -= ViewYhp + 4;
            for (int y = ViewYhx; y < ViewYhx + ViewHeightHX; y++, yhp += 32)
            {
                var field = (AoWMapField)level.GetField(x, y);
                bool unexplored = exploration && (FSeatedPlayer == -1 || !field.GetExplored(FSeatedPlayer));
                if (unexplored) AoWHSSet.Current.FGeneralImages[3].Show(xhp, yhp, surface);
                else if (FFogEnabled && ((field.FFieldFlags >> 16) & 255) == 0)
                    AoWHSSet.Current.FGeneralImages[1].Show(xhp, yhp, surface);
            }
        }
    }

    public void DrawGrid(in LockedSurface surface)
    {
        var level = MapContainer.GetMapLevel(ViewL);
        int maxX = ViewXhx + ViewWidthHX - 1, maxY = ViewYhx + ViewHeightHX - 1;
        for (int x = ViewXhx; x <= maxX; x++)
        {
            int xhp = 0, yhp = 0;
            HexGeometry.HXtoHP(x, ViewYhx, ref xhp, ref yhp);
            xhp = xhp - ViewXhp - 8;
            yhp = yhp - ViewYhp - 4 + 4;
            for (int y = ViewYhx; y <= maxY; y++, yhp += 32)
                if (level.GetField(x, y).TerrainType != 15)
                    AoWHSSet.Current.FFlagControl.FFlags[7].Show(xhp, yhp, surface);
        }
    }

    private static void DrawLine(int X1, int Y1, int X2, int Y2, int Skip, int Color, in LockedSurface SD)
    {
        int pixelFormatID = SD.PixelFormatID;
        int num = pixelFormatID;
        if (num == 1431343888 || num == 1431409424 || num == 1448121104)
        {
            int num2 = PixelFormats.Color2PixelInfo(Color, SD.PixelFormatID, null);
            DrawLine16(X1, Y1, X2, Y2, 0, (ushort)num2, in SD);
        }
    }

    private static void DrawLine16(int X1, int Y1, int X2, int Y2, int Skip, ushort Color, in LockedSurface SD)
    {
        Blit.DrawLine16(X1, Y1, X2, Y2, Skip, Color, SD.View());
    }
}
