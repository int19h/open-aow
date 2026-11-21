#nullable disable
// Translated from HSEngine.pas and AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Movement;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.World.Map;

// the map. FMouse, FMapContainer, FEngine, FSelection, FStaticVisibleHS, FDynamicVisibleHS (the
// sprites in the view port), FStaticFixedVisibleHS, FDynamicFixedVisibleHS (sprites shown
// wherever the view is), FStaticSceneInvalid, FDynamicSceneInvalid, FVersion,
// _hssFileName, _status, _frame, TerrainChanged, OnCenterView, OnChangeView,
// OnViewLevel, OnProgress, _terrainChangedLockCounter, FrameStarted,
// ViewPortChanged, MapDestroying, HexSpriteChanged, _fileName, _viewPixelX, _viewPixelY,
// FViewL, FViewWidth, FViewHeight (the view port in pixels), FViewXhx, FViewYhx, FViewWidthHX,
// FViewHeightHX (the hexagons it covers). Persisted (ReadWrite): 2 FHSSFileName (rwString, not
// in text streams), 3 FVersion (rwByte of a Shortint copy), 4 FViewXhp, 5 FViewYhp, 6 FViewL
// (rwInteger). FVersion is a guessed name: nothing but ReadWrite touches it. The original's FTimer
// (the Delphi timer node, child slot 5) is gone (issue 185 PE-3-i): no file stores it, nothing
// raised a timer event, and the stored children keep their original slots through ChildSlots
public partial class GameMap : OwningNode
{
    public new const int ClassId = 66_048;

    public GameMap(CustomNode owner) : base(owner)
    {
        Mouse = CreateMouse();
        Mouse.HexSpriteMap = this;
        Selection = new HexSpriteSelection(this);
        Selection.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        StaticVisibleHexSprite = new VisibilityList(this);
        StaticVisibleHexSprite.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        DynamicVisibleHexSprite = new VisibilityList(this);
        DynamicVisibleHexSprite.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        StaticFixedVisibleHexSprite = new VisibilityList(this);
        StaticFixedVisibleHexSprite.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        DynamicFixedVisibleHexSprite = new VisibilityList(this);
        DynamicFixedVisibleHexSprite.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        HexSpriteChangedEventsStore = new();
        TerrainChangedEventsStore = new();
        ViewPortChangedEventsStore = new();
        NewFrameEventsStore = new();
        DestroyEventsStore = new();

        _showMessageEventsStore = new();
        _mapChangedEventsStore = new();
        _scannerIconsChangedEventsStore = new();
        _backHexSelector = new CustomFixedVisibleHexSprite(null) { Holder = CreationHolder() };
        _backHexSelector.SetShowHandler(ShowBackHexSelector);
        _frontHexSelector = new CustomFixedVisibleHexSprite(null) { Holder = CreationHolder() };
        _frontHexSelector.SetShowHandler(ShowFrontHexSelector);
        _hexSelectorVisible = false;
        ExplorationEnabled = true;
        FogEnabled = true;
        SeatedPlayer = 1;
        FPS = 30;
    }

    private string _hssFileName = "";

    internal EventStore<TerrainChangedNotification> TerrainChangedEventsStore { get; }

    internal event NotifyEvent<TerrainChangedNotification> TerrainChanged
    {
        add => TerrainChangedEventsStore.Add(value);
        remove => TerrainChangedEventsStore.Remove(value);
    }

    private Action<int, int, int> _centerView;

    internal void SetCenterViewHandler(Action<int, int, int> handler) => _centerView = handler;

    private Action<int, int, int> _changeView;

    internal void SetChangeViewHandler(Action<int, int, int> handler) => _changeView = handler;

    private Action<int> _viewLevel;

    internal void SetViewLevelHandler(Action<int> handler) => _viewLevel = handler;

    private ProgressEvent _progress;

    protected void RaiseProgress(EngineObject sender, string message, int progression) => _progress?.Invoke(sender, message, progression);

    protected bool HasProgressHandlers => _progress is not null;

    internal void SetProgressHandler(ProgressEvent handler) => _progress = handler;

    private int _terrainChangedLockCounter;

    internal EventStore<GameMap> NewFrameEventsStore { get; }

    internal event NotifyEvent<GameMap> FrameStarted
    {
        add => NewFrameEventsStore.Add(value);
        remove => NewFrameEventsStore.Remove(value);
    }

    internal EventStore<GameMap> ViewPortChangedEventsStore { get; }

    internal event NotifyEvent<GameMap> ViewPortChanged
    {
        add => ViewPortChangedEventsStore.Add(value);
        remove => ViewPortChangedEventsStore.Remove(value);
    }

    internal EventStore<GameMap> DestroyEventsStore { get; }

    internal event NotifyEvent<GameMap> MapDestroying
    {
        add => DestroyEventsStore.Add(value);
        remove => DestroyEventsStore.Remove(value);
    }

    internal EventStore<HexSprite> HexSpriteChangedEventsStore { get; }

    internal event NotifyEvent<HexSprite> HexSpriteChanged
    {
        add => HexSpriteChangedEventsStore.Add(value);
        remove => HexSpriteChangedEventsStore.Remove(value);
    }

    // AoWHSMap.PAS:383 around HSMAP.PAS:37.
    public override void Dispose()
    {
        _showMessageEventsStore.Clear();
        _mapChangedEventsStore.Clear();
        HideHexSelector();
        _backHexSelector?.Dispose();
        _frontHexSelector?.Dispose();
        _scannerIconsChangedEventsStore.Clear();
        LockTerrainChangedEvent();
        Status += Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Destroying);
        Selection.OnSelectStore.Clear();
        Selection.OnDeselectStore.Clear();
        Selection.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        Selection.UnselectAll();
        DestroyEventsStore.Raise(this);
        HexSpriteChangedEventsStore.Clear();
        StaticVisibleHexSprite.QuickClear();
        DynamicVisibleHexSprite.QuickClear();
        StaticFixedVisibleHexSprite.QuickClear();
        DynamicFixedVisibleHexSprite.QuickClear();
        EngineMessage.InitializeMessage(out EngineMessage message, messageId: 536936471, sender: this);
        SendMessage(ref message, sender: this);
        NewFrameEventsStore.Clear();
        base.Dispose();
        HexSpriteChangedEventsStore.Clear();
        TerrainChangedEventsStore.Clear();
        ViewPortChangedEventsStore.Clear();
        NewFrameEventsStore.Clear();
        Mouse?.Dispose();
        DestroyEventsStore.Clear();
        UnlockTerrainChangedEvent();

        _showMessageEventsStore.Clear();
        _mapChangedEventsStore.Clear();
    }

    // HSMAP.PAS:76
    public override Set<ControlStyleElement> GetControlStyle() => base.GetControlStyle() + Set<ControlStyleElement>.Of(EngineServices.Reserved2);

    // HSMAP.PAS:240
    // The hex sprite set of a full load reports its progress to the load's handler, as the original
    // ReadWrite passes Stream.ProgressHandler. Everything else follows the original ReadWrite.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        if (persister.Loading && persister.Preview)
        {
            Status += Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.PreviewMode);
        }

        // The original leaves the file name out of a text stream. Under the translate stream's mode
        // (read, text only) it still initializes the hexagon sprite set of the name the map holds.
        bool translating = persister.Translating();
        if (!translating)
        {
            _hssFileName = persister.String("HssFileName", value: _hssFileName);
        }

        if (!persister.Preview && (persister.Loading || translating) && !IsEmpty(_hssFileName))
        {
            GameEngine engine = persister.Engine();
            engine.InitializeHexSpriteSet(engine.StartupDirectory + _hssFileName, progress: persister.Progress());
        }

        Version = persister.UnsignedByte(name: "Version", value: (sbyte)Version);
        ViewPixelX = persister.Int32("ViewPixelX", value: ViewPixelX);
        ViewPixelY = persister.Int32("ViewPixelY", value: ViewPixelY);
        ViewMapLevel = persister.Int32("ViewMapLevel", value: ViewMapLevel);
    }

    // HSMAP.PAS:261
    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (!Status.Contains(HexSpriteMapStatusElement.PreviewMode) && persister.Loading)
        {
            MapContainer.Loaded();
        }
    }

    // The original's node children are the selection, the four visibility lists and the timer at
    // slots 0 to 5, so the map container a map creates stores at slot 6. The timer is gone, and
    // every child after the visibility lists stores one slot past its index, so the document's
    // slot and the original image's id stay as they were and the live list holds no placeholder.
    private const int RetiredTimerSlot = 5;

    public override void PersistChildren(IPersister persister) => persister.Children("Children", children: new ChildSlots(this));

    // The child collection of a map: NodeChildren with the slot table above.
    private sealed class ChildSlots(GameMap map) : IChildCollection
    {
        private readonly NodeChildren _children = new(map);

        public int Count => _children.Count;

        public IPersistable Get(int index) => _children.Get(index);

        public bool IsStored(int index) => _children.IsStored(index);

        public int Slot(int index) => index < RetiredTimerSlot ? index : index + 1;

        public bool KeepIndex => _children.KeepIndex;

        public void Attach(IPersistable child) => _children.Attach(child);

        public void Place(int slot, IPersistable child) => _children.Place(slot > RetiredTimerSlot ? slot - 1 : slot, child);
    }

    // HSMAP.PAS:81
    public override void Clear()
    {
        StaticVisibleHexSprite.QuickClear();
        DynamicVisibleHexSprite.QuickClear();
        Selection.QuickClear();
        base.Clear();
        MapContainer = null;
        ClearViewPort();
    }

    // HSMAP.PAS:189
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        if (message.MessageId == 536870920)
        {
            Engine = message.Payload.As<GameEngine>();
        }

        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            Mouse.Reset();
        }
    }

    // HSMAP.PAS makes the engine mouse. AoWHSMap.PAS:396 makes the game mouse.
    public virtual GameMapMouse CreateMouse() => new();

    // HSMAP.PAS:236
    public virtual void NotifyProgress(in MapProgress progress)
    {
    }

    // HSMAP.PAS:269
    public virtual void InitializeNewMap()
    {
    }

    // HSMAP.PAS:141, then AoWHSMap.PAS:440.
    public virtual void TriggerTerrainChangedEvent(in Rect R, sbyte L)
    {
        TerrainChangedNotification notification = new();
        TerrainChangedNotification.TerrainChangedInfo info = notification.Info;
        info.Rectangle = R;
        info.Level = L;
        MapLevel mapLevel = MapContainer.GetMapLevel(L);
        info.MapLevel = mapLevel;
        notification.Info = info;
        TerrainChangedEventsStore.Raise(notification);

        if (MapChangedLockCount == 0)
        {
            TriggerMapChangedEvent(R, L);
        }
    }

    // HSMAP.PAS:705
    public virtual void DrawStaticHexSprite(ref readonly LockedSurface lockedSurface)
    {
        if (StaticSceneInvalid)
        {
            throw new InvalidOperationException("Static scene is invalid, call UpdateStaticScene!");
        }

        for (int i = StaticVisibleHexSprite.StoredCount - 1; i >= 0; i--)
        {
            StaticVisibleHexSprite[i].Show(ViewPixelX, ViewPixelY, lockedSurface);
        }
    }

    // HSMAP.PAS:725
    public virtual void DrawDynamicHexSprite(ref readonly LockedSurface lockedSurface)
    {
        if (DynamicSceneInvalid)
        {
            throw new InvalidOperationException("Dynamic scene is invalid, call UpdateDynamicScene!");
        }

        for (int i = DynamicVisibleHexSprite.StoredCount - 1; i >= 0; i--)
        {
            DynamicVisibleHexSprite[i].Show(ViewPixelX, ViewPixelY, lockedSurface);
        }
    }

    // HSMAP.PAS:715
    public virtual void DrawStaticExtras(ref readonly LockedSurface lockedSurface)
    {
    }

    // HSMAP.PAS:735
    public virtual void DrawDynamicExtras(ref readonly LockedSurface lockedSurface)
    {
    }

    // HSMAP.PAS:273
    public virtual void FillLevelWithResource(int L, MapObjectResource resource)
    {
        for (int y = 0, last = MapContainer.Height - 1; y <= last; y++)
        {
            for (int x = 0, lastX = MapContainer.Width - 1; x <= lastX; x++)
            {
                _ = resource.PlaceResourceUser(this, (sbyte)x, (sbyte)y, (sbyte)L);
            }
        }
    }

    // HSMAP.PAS:96
    public virtual bool AddMapLevel() => MapContainer.AddLevel(65_794);

    // HSMAP.PAS:101
    public virtual void Activate()
    {
        if (Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            throw new InvalidOperationException("HSMap already activated");
        }

        Status += Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Activated);
    }

    // HSMAP.PAS:108
    public virtual void Deactivate()
    {
        if (!Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            throw new InvalidOperationException("HSMap wasn't activated");
        }

        Status -= Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Activated);
    }

    // HSMAP.PAS:128
    // { Tests OnCenterView instead of OnViewLevel (original slip). }
    public virtual void ViewLevel(int hexLevel)
    {
        if (_centerView is not null && hexLevel < MapContainer.LevelCount)
        {
            _viewLevel(hexLevel);
        }
    }

    // HSMAP.PAS:115
    public virtual void CenterViewTo(int hexX, int hexY, int hexLevel) => _centerView?.Invoke(hexX, hexY, hexLevel);

    // HSMAP.PAS:121
    public virtual void ChangeViewTo(int pixelX, int pixelY, int lHitPoints) => _changeView?.Invoke(pixelX, pixelY, lHitPoints);

    // HSMAP.PAS:579
    public virtual void GetViewCenter(out int hexX, out int hexY, out int hexLevel)
    {
        HexGeometry.PixelToHex(ViewPixelX + (ViewWidth >>> 1), vertical: ViewPixelY + (ViewHeight >>> 1), out hexX, out hexY);
        hexLevel = ViewMapLevel;
    }

    // HSMAP.PAS:134
    public virtual void MakeVisible(int hexX, int hexY, int hexLevel, int radius)
    {
        if (ViewMapLevel != hexLevel || hexX - radius < ViewHexX || hexX + radius >= ViewHexX + ViewWidthHex || hexY - radius < ViewHexY || hexY + radius >= ViewHexY + ViewHeightHex)
        {
            CenterViewTo(hexX, hexY, hexLevel);
        }
    }

    // HSMAP.PAS:543
    public virtual bool CreateNewMap(NewHexSpriteMapSettings settings)
    {
        Clear();
        try
        {
            SetHssFileName(settings.HssFileName);
            Engine.InitializeHexSpriteSet(settings.HssFileName, progress: null);
            MapContainer = Engine.CreateEngineObject<MapContainer>(settings.MapContainerClassId, owner: this);
            Engine.ActivateEngineObject(MapContainer);
            MapContainer.New(settings);
            InitializeNewMap();
            Activate();
        }
        catch
        {
            Clear();
            throw;
        }

        return true;
    }

    // HSMAP.PAS:284
    // { ChangeTerrain, ChangeTerrainEx and ClearTerrain clip the right edge of the changed rectangle
    //   into its Left (original slip). }
    public virtual int ChangeTerrain(EngineObject mapObject, int X, int Y, int L, int radius, int terrainType)
    {
        Rect r = default;
        LockTerrainChangedEvent();
        SByteList list = new();
        MapLevel mapLevel = MapContainer.GetMapLevel(L);
        int count = 0;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
        MapField mapField;
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                mapField = mapLevel.GetField(walk.X, walk.Y);
                list[walk.HexNumber] = mapField.TerrainType;
                mapField.TerrainType = (sbyte)terrainType;
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, radius: 0);
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                mapField = mapLevel.GetField(walk.X, walk.Y);
                if (!mapField.CanChangeTerrain(mapObject))
                {
                    mapField.TerrainType = list[walk.HexNumber];
                    list[walk.HexNumber] = -127;
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, radius: 0);
        while (walk.Radius <= radius)
        {
            if (list[walk.HexNumber] != -127 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                mapLevel.GetField(walk.X, walk.Y).TerrainType = (sbyte)terrainType;
                mapLevel.GetField(walk.X, walk.Y).TerrainChanged();
                count += 1;
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        UnlockTerrainChangedEvent();
        list?.Dispose();
        if (count != 0)
        {
            r.Left = X - radius;
            if (r.Left < 0)
            {
                r.Left = 0;
            }

            r.Right = X + radius + 1;
            if (r.Right > mapLevel.Width)
            {
                r.Left = mapLevel.Width;
            }

            r.Top = Y - radius;
            if (r.Top < 0)
            {
                r.Top = 0;
            }

            r.Bottom = Y + radius + 1;
            if (r.Bottom > mapLevel.Height)
            {
                r.Bottom = mapLevel.Height;
            }

            NotifyTerrainChanged(r, L: (sbyte)L);
        }

        return count;
    }

    // HSMAP.PAS:357
    public virtual int ChangeTerrain(EngineObject mapObject, int X, int Y, int L, int radius, ChangeTerrainProc changeTerrainProc, TerrainChangedProc terrainChangedProc)
    {
        Rect r = default;
        if (changeTerrainProc is not null)
        {
            LockTerrainChangedEvent();
            SByteList orgList = new();
            SByteList newList = new();
            BitList changed = new();
            MapLevel mapLevel = MapContainer.GetMapLevel(L);
            int count = 0;
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
            MapField mapField;
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    mapField = mapLevel.GetField(walk.X, walk.Y);
                    sbyte terrainType = mapField.TerrainType;
                    sbyte overlayType = mapField.OverlayType;
                    orgList[walk.HexNumber * 2] = terrainType;
                    orgList[(walk.HexNumber * 2) + 1] = overlayType;
                    changeTerrainProc(mapField, ref terrainType, overlayType);
                    mapField.TerrainType = terrainType;
                    mapField.OverlayType = overlayType;
                    newList[walk.HexNumber * 2] = terrainType;
                    newList[(walk.HexNumber * 2) + 1] = overlayType;
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, radius: 0);
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    mapField = mapLevel.GetField(walk.X, walk.Y);
                    if (!mapField.CanChangeTerrain(mapObject))
                    {
                        mapField.TerrainType = orgList[walk.HexNumber * 2];
                        mapField.OverlayType = orgList[(walk.HexNumber * 2) + 1];
                        changed[walk.HexNumber] = false;
                    }
                    else
                    {
                        changed[walk.HexNumber] = true;
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, radius: 0);
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    if (changed[walk.HexNumber])
                    {
                        sbyte item = newList[walk.HexNumber * 2];
                        mapLevel.GetField(walk.X, walk.Y).TerrainType = item;
                        sbyte newListItem = newList[(walk.HexNumber * 2) + 1];
                        mapLevel.GetField(walk.X, walk.Y).OverlayType = newListItem;
                        mapLevel.GetField(walk.X, walk.Y).TerrainChanged();
                        count += 1;
                    }

                    terrainChangedProc?.Invoke(originalOverlayType: orgList[(walk.HexNumber * 2) + 1], originalTerrainType: orgList[walk.HexNumber * 2], mapField: mapLevel.GetField(walk.X, walk.Y));
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            UnlockTerrainChangedEvent();
            changed?.Dispose();
            orgList?.Dispose();
            newList?.Dispose();
            if (count != 0)
            {
                r.Left = X - radius;
                if (r.Left < 0)
                {
                    r.Left = 0;
                }

                r.Right = X + radius + 1;
                if (r.Right > mapLevel.Width)
                {
                    r.Left = mapLevel.Width;
                }

                r.Top = Y - radius;
                if (r.Top < 0)
                {
                    r.Top = 0;
                }

                r.Bottom = Y + radius + 1;
                if (r.Bottom > mapLevel.Height)
                {
                    r.Bottom = mapLevel.Height;
                }

                NotifyTerrainChanged(r, L: (sbyte)L);
            }

            return count;
        }
        else
        {
            return 0;
        }
    }

    // AoWHSMap.PAS:810 shows the explored fields only.
    public virtual void UpdateStaticScene()
    {
        if (ExplorationEnabled && SeatedPlayer != -1 && !Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            ResetVisibleStaticHexSprite();
            int maxX = ViewHexX + ViewWidthHex - 1;
            int maxY = ViewHexY + ViewHeightHex - 1;
            MapLevel level = MapContainer.GetMapLevel(ViewMapLevel);
            for (int x = ViewHexX; x <= maxX; x++)
            {
                for (int y = maxY, last = ViewHexY; y >= last; y--)
                {
                    MapField field = level.GetField(x, y);
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
            UpdateWholeStaticScene();
        }
    }

    // HSMAP.PAS:669 shows every field of the view, explored or not.
    private void UpdateWholeStaticScene()
    {
        ResetVisibleStaticHexSprite();
        int maxX = ViewHexX + ViewWidthHex - 1;
        int maxY = ViewHexY + ViewHeightHex - 1;
        MapLevel mapLevel = MapContainer.GetMapLevel(ViewMapLevel);
        for (int x = ViewHexX; x <= maxX; x++)
        {
            for (int y = maxY, last = ViewHexY; y >= last; y--)
            {
                mapLevel.GetField(x, y).CopyStaticVisibleHexSprite(StaticVisibleHexSprite);
            }
        }

        for (int x = 0, last = StaticFixedVisibleHexSprite.StoredCount - 1; x <= last; x++)
        {
            StaticFixedVisibleHexSprite[x].MakeVisible(StaticVisibleHexSprite);
        }

        StaticVisibleHexSprite.Sort();
        StaticSceneInvalid = false;
    }

    // AoWHSMap.PAS:833 shows the explored fields only.
    public virtual void UpdateDynamicScene()
    {
        if (ExplorationEnabled && !Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            ResetVisibleDynamicHexSprite();
            int maxX = ViewHexX + ViewWidthHex - 1;
            int maxY = ViewHexY + ViewHeightHex - 1;
            MapLevel level = MapContainer.GetMapLevel(ViewMapLevel);
            if (SeatedPlayer != -1)
            {
                for (int x = ViewHexX; x <= maxX; x++)
                {
                    for (int y = maxY, last = ViewHexY; y >= last; y--)
                    {
                        MapField field = level.GetField(x, y);
                        if (field.GetExplored(SeatedPlayer))
                        {
                            field.CopyDynamicVisibleHexSprite(DynamicVisibleHexSprite);
                        }
                    }
                }

                for (int x = 0, last = DynamicFixedVisibleHexSprite.Count - 1; x <= last; x++)
                {
                    DynamicFixedVisibleHexSprite[x].MakeVisible(DynamicVisibleHexSprite);
                }

                DynamicVisibleHexSprite.Sort();
            }

            DynamicSceneInvalid = false;
        }
        else
        {
            UpdateWholeDynamicScene();
        }
    }

    // HSMAP.PAS:687 shows every field of the view, explored or not.
    private void UpdateWholeDynamicScene()
    {
        ResetVisibleDynamicHexSprite();
        int maxX = ViewHexX + ViewWidthHex - 1;
        int maxY = ViewHexY + ViewHeightHex - 1;
        MapLevel mapLevel = MapContainer.GetMapLevel(ViewMapLevel);
        for (int x = ViewHexX; x <= maxX; x++)
        {
            for (int y = maxY, last = ViewHexY; y >= last; y--)
            {
                mapLevel.GetField(x, y).CopyDynamicVisibleHexSprite(DynamicVisibleHexSprite);
            }
        }

        for (int x = 0, last = DynamicFixedVisibleHexSprite.StoredCount - 1; x <= last; x++)
        {
            DynamicFixedVisibleHexSprite[x].MakeVisible(DynamicVisibleHexSprite);
        }

        DynamicVisibleHexSprite.Sort();
        DynamicSceneInvalid = false;
    }

    // HSMAP.PAS:719
    public virtual void DrawStaticScene(ref readonly LockedSurface lockedSurface)
    {
        DrawStaticHexSprite(in lockedSurface);
        DrawStaticExtras(in lockedSurface);
    }

    // HSMAP.PAS:739
    public virtual void DrawDynamicScene(ref LockedSurface lockedSurface)
    {
        DrawDynamicHexSprite(in lockedSurface);
        DrawDynamicExtras(in lockedSurface);
    }

    // HSMAP.PAS:752, then AoWHSMap.PAS:861.
    public virtual void NewFrame()
    {
        Frame += 1;
        if (Frame > 1_048_575)
        {
            Frame = 0;
        }

        NewFrameEventsStore.Raise(this);

        UpdateShowMessage();
        if (_hexSelectorVisible)
        {
            UpdateHexSelector();
        }
    }

    // HSMAP.PAS:91
    public void RemoveMapLevel(int index) => MapContainer.RemoveLevel(index);

    // HSMAP.PAS:151
    public void NotifyTerrainChanged(in Rect R, sbyte L)
    {
        if (_terrainChangedLockCounter == 0)
        {
            TriggerTerrainChangedEvent(R, L);
        }
    }

    // HSMAP.PAS:157
    public void TerrainChangedRadius(sbyte X, sbyte Y, sbyte L, int radius)
    {
        Rect r = default;
        MapLevel mapLevel = MapContainer.GetMapLevel(L);
        r.Left = X - radius;
        if (r.Left < 0)
        {
            r.Left = 0;
        }

        r.Right = X + radius + 1;
        if (r.Right > mapLevel.Width)
        {
            r.Right = mapLevel.Width;
        }

        r.Top = Y - radius;
        if (r.Top < 0)
        {
            r.Top = 0;
        }

        r.Bottom = Y + radius + 1;
        if (r.Bottom > mapLevel.Height)
        {
            r.Bottom = mapLevel.Height;
        }

        NotifyTerrainChanged(r, L);
    }

    // HSMAP.PAS:178
    public void LockTerrainChangedEvent() => _terrainChangedLockCounter += 1;

    // HSMAP.PAS:183
    public void UnlockTerrainChangedEvent()
    {
        if (_terrainChangedLockCounter == 0)
        {
            Assert(false, message: "Invalid TerrainChangedLockCounter", file: "D:\\GEngine\\HSENGINE\\inc\\HSMAP.PAS", line: 5989);
        }

        _terrainChangedLockCounter -= 1;
    }

    // HSMAP.PAS:198
    public void SetModified() => Status += Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Modified);

    // HSMAP.PAS:203
    public void ResetModified() => Status -= Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Modified);

    // HSMAP.PAS:208
    public int MaxPixelX
    {
        get
        {
            HexGeometry.HexToPixel(MapContainer.Width - 1, MapContainer.Height - 1, out int x, out _);

            x += 40;
            return x;
        }
    }

    // HSMAP.PAS:218
    public int MaxPixelY
    {
        get
        {
            int y;
            if (MapContainer.Height > 1)
            {
                HexGeometry.HexToPixel(1, MapContainer.Height - 1, out _, out y);
            }
            else
            {
                HexGeometry.HexToPixel(0, MapContainer.Height - 1, out _, out y);
            }

            y += 32;
            return y;
        }
    }

    // HSMAP.PAS:231
    public void SetHssFileName(string fileName)
    {
        string expandFileName = ExpandFileName(fileName);
        _hssFileName = RtlHost.Files.ExtractRelativePath(Engine.StartupDirectory, destination: expandFileName);
    }

    // HSMAP.PAS:459
    public void ClearTerrain(int X, int Y, int L, int radius, int terrainType)
    {
        MapFieldMessage message = default;
        Rect r = default;
        LockTerrainChangedEvent();
        MapLevel mapLevel = MapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, X, Y, radius: 0);
        message.MessageId = 16;
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                mapLevel.GetField(walk.X, walk.Y).SendMapFieldMessage(ref message);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        if (terrainType != -1)
        {
            _ = ChangeTerrain(null, X, Y, L, radius, terrainType);
        }

        UnlockTerrainChangedEvent();
        r.Left = X - radius;
        if (r.Left < 0)
        {
            r.Left = 0;
        }

        r.Right = X + radius + 1;
        if (r.Right > mapLevel.Width)
        {
            r.Left = mapLevel.Width;
        }

        r.Top = Y - radius;
        if (r.Top < 0)
        {
            r.Top = 0;
        }

        r.Bottom = Y + radius + 1;
        if (r.Bottom > mapLevel.Height)
        {
            r.Bottom = mapLevel.Height;
        }

        NotifyTerrainChanged(r, L: (sbyte)L);
    }

    // HSMAP.PAS:561
    public bool PlaceHex(HexSprite hexSprite, int hexX, int hexY, int L)
    {
        HexGeometry.HexToPixel(hexX, hexY, out int pixelX, out int pixelY);
        return hexSprite.MainPlace(MapContainer, pixelX, pixelY, L);
    }

    // HSMAP.PAS:569
    public bool Place(HexSprite hexSprite, int pixelX, int pixelY, int L) => hexSprite.MainPlace(MapContainer, pixelX, pixelY, L);

    // HSMAP.PAS:574
    public void Remove(HexSprite hexSprite) => hexSprite.MainRemove();

    // HSMAP.PAS:585
    public bool SetViewPort(int pixelX, int pixelY, int lHitPoints, int width, int height)
    {
        if (pixelX != ViewPixelX || pixelY != ViewPixelY || lHitPoints != ViewMapLevel || width != ViewWidth || height != ViewHeight)
        {
            ClearViewPort();
            ViewMapLevel = lHitPoints;
            ViewPixelX = pixelX;
            ViewWidth = width;
            ViewPixelY = pixelY;
            ViewHeight = height;
            HexGeometry.PixelToHex(ViewPixelX - 8, vertical: ViewPixelY - 16, out int x1, out int y1);
            HexGeometry.PixelToHex(ViewPixelX + width - 1 + 8, vertical: ViewPixelY + height - 1 + 16, out int x2, out int y2);
            if (x2 >= MapContainer.Width)
            {
                x2 = MapContainer.Width - 1;
            }

            if (y2 >= MapContainer.Height)
            {
                y2 = MapContainer.Height - 1;
            }

            if (x1 < 0)
            {
                x1 = 0;
            }

            if (y1 < 0)
            {
                y1 = 0;
            }

            ViewWidthHex = x2 - x1 + 1;
            ViewHeightHex = y2 - y1 + 1;
            ViewHexX = x1;
            ViewHexY = y1;
            ViewPortChangedEventsStore.Raise(this);
        }
        else
        {
            return false;
        }

        return true;
    }

    // HSMAP.PAS:619
    public void ClearViewPort()
    {
        ResetVisibleStaticHexSprite();
        ResetVisibleDynamicHexSprite();
        StaticSceneInvalid = true;
        DynamicSceneInvalid = true;
        ViewPixelX = -1;
        ViewPixelY = -1;
        ViewMapLevel = -1;
        ViewWidth = 0;
        ViewHeight = 0;
    }

    // HSMAP.PAS:633
    // { Viewing accepts one row below the view port, InvalidateStaticScene does not. }
    public bool Viewing(int hexX, int hexY, int hexLevel) => hexX >= ViewHexX && hexX < ViewHexX + ViewWidthHex && hexY >= ViewHexY && hexY <= ViewHexY + ViewHeightHex && hexLevel == ViewMapLevel;

    // HSMAP.PAS:640
    // { The message names ResetVisibleDynamicHS (original slip). }
    public void ResetVisibleStaticHexSprite()
    {
        try
        {
            for (int i = StaticVisibleHexSprite.StoredCount - 1; i >= 0; i--)
            {
                StaticVisibleHexSprite[i].Visible = false;
            }

            StaticVisibleHexSprite.QuickClear();
            StaticVisibleHexSprite.Sorted = false;
        }
        catch
        {
            throw new InvalidOperationException("Exception during ResetVisibleDynamicHS");
        }
    }

    // HSMAP.PAS:654
    public void ResetVisibleDynamicHexSprite()
    {
        for (int i = DynamicVisibleHexSprite.StoredCount - 1; i >= 0; i--)
        {
            try
            {
                DynamicVisibleHexSprite[i].Visible = false;
            }
            catch
            {
                throw new InvalidOperationException($"Exception during ResetVisibleDynamicHS ({DynamicVisibleHexSprite.Items[i].Child.GetType().Name})");
            }
        }

        DynamicVisibleHexSprite.QuickClear();
        DynamicVisibleHexSprite.Sorted = false;
    }

    // HSMAP.PAS:745
    public void InvalidateStaticScene(int hexX, int hexY, int hexLevel) => StaticSceneInvalid = StaticSceneInvalid || (hexX >= ViewHexX && hexX < ViewHexX + ViewWidthHex && hexY >= ViewHexY && hexY < ViewHexY + ViewHeightHex && hexLevel == ViewMapLevel);

    public GameMapMouse Mouse { get; }
    public MapContainer MapContainer { get; internal set; }
    public GameEngine Engine { get; private set; }
    public HexSpriteSelection Selection { get; }
    public VisibilityList StaticVisibleHexSprite { get; }
    public VisibilityList DynamicVisibleHexSprite { get; }
    public VisibilityList StaticFixedVisibleHexSprite { get; }
    public VisibilityList DynamicFixedVisibleHexSprite { get; }
    public bool StaticSceneInvalid { get; set; }
    public bool DynamicSceneInvalid { get; protected set; }
    public int Version { get; private set; }
    public string HssFileName { get => _hssFileName; set => SetHssFileName(value); }
    public Set<HexSpriteMapStatusElement> Status { get; set; }
    public int Frame { get; private set; }

    public event NotifyEvent<TerrainChangedNotification> TerrainChangedEvents
    {
        add => TerrainChangedEventsStore.Add(value);
        remove => TerrainChangedEventsStore.Remove(value);
    }

    public event NotifyEvent<GameMap> NewFrameEvents
    {
        add => NewFrameEventsStore.Add(value);
        remove => NewFrameEventsStore.Remove(value);
    }

    public event NotifyEvent<GameMap> ViewPortChangedEvents
    {
        add => ViewPortChangedEventsStore.Add(value);
        remove => ViewPortChangedEventsStore.Remove(value);
    }

    public event NotifyEvent<GameMap> DestroyEvents
    {
        add => DestroyEventsStore.Add(value);
        remove => DestroyEventsStore.Remove(value);
    }

    public event NotifyEvent<HexSprite> HexSpriteChangedEvents
    {
        add => HexSpriteChangedEventsStore.Add(value);
        remove => HexSpriteChangedEventsStore.Remove(value);
    }

    public string FileName { get; set; } = "";
    public int ViewPixelX { get; protected set; }
    public int ViewPixelY { get; protected set; }
    public int ViewMapLevel { get; protected set; }
    public int ViewWidth { get; private set; }
    public int ViewHeight { get; private set; }
    public int ViewHexX { get; private set; }
    public int ViewHexY { get; private set; }
    public int ViewWidthHex { get; private set; }
    public int ViewHeightHex { get; private set; }

    public sbyte CurrentPlayer { get; set; }

    public sbyte SeatedPlayer { get; set; }

    private readonly EventStore<ShowMessageNotification> _showMessageEventsStore;

    public event NotifyEvent<ShowMessageNotification> ShowingMessage
    {
        add => _showMessageEventsStore.Add(value);
        remove => _showMessageEventsStore.Remove(value);
    }

    private string _messageText = "";
    private byte _messageFrame;

    public int FPS { get; private set; }
    public int MapChangedLockCount { get; private set; }

    private readonly EventStore<MapChangedNotification> _mapChangedEventsStore;

    public event NotifyEvent<MapChangedNotification> MapChanged
    {
        add => _mapChangedEventsStore.Add(value);
        remove => _mapChangedEventsStore.Remove(value);
    }

    private readonly EventStore<MapChangedNotification> _scannerIconsChangedEventsStore;

    public event NotifyEvent<MapChangedNotification> ScannerIconsChanged
    {
        add => _scannerIconsChangedEventsStore.Add(value);
        remove => _scannerIconsChangedEventsStore.Remove(value);
    }

    private int _scannerIconsChangedLockCount;

    public bool FogEnabled { get; protected set; }
    public bool ExplorationEnabled { get; set; }

    public bool ShowGrid { get; set; }

    private readonly CustomFixedVisibleHexSprite _backHexSelector;
    private readonly CustomFixedVisibleHexSprite _frontHexSelector;
    private sbyte _selectorX;
    private sbyte _selectorY;
    private bool _hexSelectorVisible;

    // AoWHSMap.PAS:759
    public virtual void ShowBackHexSelector(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (_selectorX != -1 && _selectorY != -1 && StrategicMap.Current.Mouse.Capture == StrategicMap.Current.SelectedArmy)
        {
            HexGeometry.HexToPixel(_selectorX, _selectorY, out int pixelX, out int pixelY);
            pixelX = pixelX - pixelXSurface - 8;
            pixelY = pixelY - pixelYSurface - 32 + 2;
            GamePack.Current.GeneralImages[7].Show(pixelX, pixelY, lockedSurface);
        }
    }

    // AoWHSMap.PAS:772
    public virtual void ShowFrontHexSelector(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (_selectorX != -1 && _selectorY != -1 && StrategicMap.Current.Mouse.Capture == StrategicMap.Current.SelectedArmy)
        {
            HexGeometry.HexToPixel(_selectorX, _selectorY, out int pixelX, out int pixelY);
            pixelX = pixelX - pixelXSurface - 8;
            pixelY = pixelY - pixelYSurface - 32 + 2;
            GamePack.Current.GeneralImages[6].Show(pixelX, pixelY, lockedSurface);
        }
    }

    // AoWHSMap.PAS:785
    public virtual void UpdateHexSelector()
    {
        if (_hexSelectorVisible)
        {
            _selectorX = (sbyte)Mouse.HexX;
            _selectorY = (sbyte)Mouse.HexY;
            if (_selectorX != -1 && _selectorY != -1)
            {
                HexGeometry.HexToPixel(_selectorX, _selectorY, out int pixelX, out int pixelY);
                HexSprite hexSprite = MapContainer.GetField(_selectorX, _selectorY, ViewMapLevel).GetMaxLevelNoneTransparentHexSprite();
                if (hexSprite is not null)
                {
                    if (hexSprite.GetShowPriority() > 1_744_830_464)
                    {
                        int showPriority = hexSprite.GetShowPriority();
                        _frontHexSelector.ShowPriority = showPriority + 100;
                    }
                    else
                    {
                        _frontHexSelector.ShowPriority = 1_744_830_464 + ((pixelY + 32 + 32) << 13) + pixelX;
                    }
                }
                else
                {
                    _frontHexSelector.ShowPriority = 1_744_830_464 + ((pixelY + 32 + 32) << 13) + pixelX;
                }

                _backHexSelector.ShowPriority = 1_744_830_464 + ((pixelY + 32 - 8) << 13) + pixelX;
            }
        }
        else
        {
            _selectorX = -1;
        }
    }

    // AoWHSMap.PAS:590
    public virtual void SetSeatedPlayer(sbyte player) => SeatedPlayer = player;

    // AoWHSMap.PAS:594
    public virtual void SetCurrentPlayer(sbyte player) => CurrentPlayer = player;

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
        MapCoordinate coordinate30 = hero.GetLocation();
        sbyte x = coordinate30.X;
        sbyte y = coordinate30.Y;
        sbyte l = coordinate30.MapLevel;
        _ = item.PlaceOnMap(this, x, y, l);
    }

    // AoWHSMap.PAS:878
    public virtual ItemList GetHeroItemsOnGround(Hero hero)
    {
        MapCoordinate coordinate31 = hero.GetLocation();
        sbyte x = coordinate31.X;
        sbyte y = coordinate31.Y;
        sbyte l = coordinate31.MapLevel;
        if (x != -1)
        {
            ItemHexSprite items = StrategicMap.Current.MapContainer.GetField(x, y, l).FindOwnedChild<ItemHexSprite>(ItemHexSprite.ClassId);
            if (items is not null)
            {
                return items.Items;
            }
        }

        return null;
    }

    // AoWHSMap.PAS:578
    public virtual void ShowMessage(string text)
    {
        ShowMessageNotification notification = new();
        ShowMessageInfo info = notification.Info;
        if (!GetEngine().Registry.GetDebugMode() && !IsEmpty(text))
        {
            _ = AudioPlayback.PlaySample("SFX\\EVENTS\\INFO1.WAV");
        }

        info.Text = text;
        info.Color = 0xFF_FF_FF;
        notification.Info = info;
        _showMessageEventsStore.Raise(notification);
        _messageText = text;
        _messageFrame = 128;
    }

    // AoWHSMap.PAS:566
    public virtual void ShowWarning(string text)
    {
        ShowMessageNotification notification = new();
        ShowMessageInfo info = notification.Info;
        if (!GetEngine().Registry.GetDebugMode() && !IsEmpty(text))
        {
            _ = AudioPlayback.PlaySample("SFX\\EVENTS\\INFO2.WAV");
        }

        info.Text = text;
        info.Color = 0xFF_FF_FF;
        notification.Info = info;
        _showMessageEventsStore.Raise(notification);
        _messageText = text;
        _messageFrame = 160;
    }

    // AoWHSMap.PAS:420
    public void PlayCenterAnimation(sbyte X, sbyte Y, sbyte L)
    {
        ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
        animation.SetImageSequence(GamePack.Current.ImageSequences.GetImageSequence(11));
        _ = PlaceHex(animation, X, Y, L);
        _ = AudioPlayback.PlaySample("SFX\\INT\\CENTER.WAV");
    }

    // AoWHSMap.PAS:430
    public void LockMapChangedEvent() => MapChangedLockCount += 1;

    // AoWHSMap.PAS:435
    public void UnlockMapChangedEvent()
    {
        Assert(MapChangedLockCount != 0, message: "Invalid MapChanged Lock usage", file: "AoWHSMap.PAS", line: 437);
        MapChangedLockCount -= 1;
    }

    // AoWHSMap.PAS:445
    public void NotifyMapChanged(in Rect R, int L)
    {
        if (MapChangedLockCount == 0 && Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            TriggerTerrainChangedEvent(R, L: (sbyte)L);
        }
    }

    // AoWHSMap.PAS:451
    public void MapChangedRadius(int X, int Y, int L, int radius)
    {
        Rect r = default;
        MapLevel level = MapContainer.GetMapLevel(L);
        r.Left = X - radius;
        if (r.Left < 0)
        {
            r.Left = 0;
        }

        r.Right = X + radius + 1;
        if (r.Right > level.Width)
        {
            r.Right = level.Width;
        }

        r.Top = Y - radius;
        if (r.Top < 0)
        {
            r.Top = 0;
        }

        r.Bottom = Y + radius + 1;
        if (r.Bottom > level.Height)
        {
            r.Bottom = level.Height;
        }

        NotifyTerrainChanged(r, L: (sbyte)L);
    }

    // AoWHSMap.PAS:468
    public void InvalidateMap()
    {
        Rect r = default;
        r.Left = 0;
        r.Right = MapContainer.Width;
        r.Top = 0;
        r.Bottom = MapContainer.Height;
        for (int i = 0, last = MapContainer.LevelCount - 1; i <= last; i++)
        {
            NotifyMapChanged(r, L: i);
        }
    }

    // AoWHSMap.PAS:480
    public void TriggerMapChangedEvent(in Rect R, int L)
    {
        MapChangedNotification notification = new();
        MapChangedInfo info = notification.Info;
        info.Bounds = R;
        info.Level = L;
        MapLevel mapLevel = MapContainer.GetMapLevel(L);
        info.MapLevel = mapLevel;
        notification.Info = info;
        _mapChangedEventsStore.Raise(notification);
    }

    // AoWHSMap.PAS:490
    public void LockScannerIconsChangedEvent() => _scannerIconsChangedLockCount += 1;

    // AoWHSMap.PAS:495
    public void UnlockScannerIconsChangedEvent()
    {
        Assert(_scannerIconsChangedLockCount != 0, message: "Invalid ScannerIcons Lock usage", file: "AoWHSMap.PAS", line: 497);
        _scannerIconsChangedLockCount -= 1;
    }

    // AoWHSMap.PAS:500
    public void NotifyScannerIconsChanged(in Rect R, int L)
    {
        if (_scannerIconsChangedLockCount == 0 && (Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Activated, HexSpriteMapStatusElement.Destroying) & Status) == Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Activated))
        {
            TriggerScannerIconsChangedEvent(R, L);
        }
    }

    // AoWHSMap.PAS:506
    public void ScannerIconsChangedRadius(int hexX, int hexY, int hexLevel, int radius)
    {
        Rect r = default;
        MapLevel level = MapContainer.GetMapLevel(hexLevel);
        r.Left = hexX - radius;
        if (r.Left < 0)
        {
            r.Left = 0;
        }

        r.Right = hexX + radius + 1;
        if (r.Right > level.Width)
        {
            r.Right = level.Width;
        }

        r.Top = hexY - radius;
        if (r.Top < 0)
        {
            r.Top = 0;
        }

        r.Bottom = hexY + radius + 1;
        if (r.Bottom > level.Height)
        {
            r.Bottom = level.Height;
        }

        NotifyScannerIconsChanged(r, L: hexLevel);
    }

    // AoWHSMap.PAS:522
    public void InvalidateScannerIcons()
    {
        Rect r = default;
        r.Left = 0;
        r.Right = MapContainer.Width;
        r.Top = 0;
        r.Bottom = MapContainer.Height;
        for (int i = 0, last = MapContainer.LevelCount - 1; i <= last; i++)
        {
            NotifyScannerIconsChanged(r, L: i);
        }
    }

    // AoWHSMap.PAS:533
    public void TriggerScannerIconsChangedEvent(in Rect R, int L)
    {
        MapChangedNotification notification = new();
        MapChangedInfo info = notification.Info;
        info.Bounds = R;
        info.Level = L;
        MapLevel mapLevel = MapContainer.GetMapLevel(L);
        info.MapLevel = mapLevel;
        notification.Info = info;
        _scannerIconsChangedEventsStore.Raise(notification);
    }

    // AoWHSMap.PAS:542
    public void UpdateShowMessage()
    {
        ShowMessageNotification notification = new();
        ShowMessageInfo info = notification.Info;
        if (_messageFrame != 0)
        {
            _messageFrame = (byte)(_messageFrame - 1);
            if (((_messageFrame & 0b0001_0000) == 0 && (_messageFrame & 0xF) < 7) || ((_messageFrame & 0b0001_0000) == 0b0001_0000 && (_messageFrame & 0xF) < 11))
            {
                info.Text = _messageText;
                _messageFrame = (byte)(_messageFrame & 0xF0);
                if ((_messageFrame & 0b0001_0000) == 0)
                {
                    info.Color = 0x35_35_35;

                    notification.Info = info;
                }
                else
                {
                    info.Color = 0xFF_FF_FF;

                    notification.Info = info;
                }

                _showMessageEventsStore.Raise(notification);
            }

            if (_messageFrame == 0)
            {
                info.Text = "";
                info.Color = 0xFF_FF_FF;
                notification.Info = info;
                _showMessageEventsStore.Raise(notification);
            }
        }
    }

    // AoWHSMap.PAS:598
    public bool FogArea(int X, int Y, int level, int range)
    {
        Assert(range <= 15, message: "Invalid Unfog Range", file: "AoWHSMap.PAS", line: 600);
        bool result = MapContainer.GetMapLevel(level).FogArea(X, Y, radius: range);
        if (result && MapChangedLockCount == 0 && FogEnabled)
        {
            MapChangedRadius(X, Y, L: level, radius: range);
        }

        return result;
    }

    // AoWHSMap.PAS:605
    public bool UnFogArea(int X, int Y, int level, int range)
    {
        Assert(range <= 15, message: "Invalid Unfog Range", file: "AoWHSMap.PAS", line: 607);
        bool result = MapContainer.GetMapLevel(level).UnFogArea(X, Y, radius: range);
        if (result && MapChangedLockCount == 0 && FogEnabled)
        {
            MapChangedRadius(X, Y, L: level, radius: range);
        }

        return result;
    }

    // AoWHSMap.PAS:612
    public bool ExploreArea(int X, int Y, int level, sbyte player, int range)
    {
        bool result;
        if (!Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            result = MapContainer.GetMapLevel(level).ExploreArea(X, Y, player, radius: range);
            if (result && MapChangedLockCount == 0 && FogEnabled)
            {
                MapChangedRadius(X, Y, L: level, radius: range);
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // AoWHSMap.PAS:622
    public Set<FogExplorationChangedElement> UnFogExploreArea(int X, int Y, int level, sbyte player, int range)
    {
        Set<FogExplorationChangedElement> result = default;
        // { Preserve the original assertion diagnostic, independent of the build path. }
        int radius = range;
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
                result = MapContainer.GetMapLevel(level).UnFogExploreArea(X, Y, player, radius);
            }
            else
            {
                if (MapContainer.GetMapLevel(level).UnFogArea(X, Y, radius))
                {
                    result = Set<FogExplorationChangedElement>.Of(FogExplorationChangedElement.Fog);
                }
            }
        }
        else
        {
            if (MapContainer.GetMapLevel(level).UnFogArea(X, Y, radius))
            {
                result = Set<FogExplorationChangedElement>.Of(FogExplorationChangedElement.Fog);
            }
        }

        if (MapChangedLockCount == 0 && ((result.Contains(FogExplorationChangedElement.Fog) && FogEnabled) || (result.Contains(FogExplorationChangedElement.Exploration) && ExplorationEnabled && player == SeatedPlayer)))
        {
            MapChangedRadius(X, Y, L: level, radius);
        }

        return result;
    }

    // AoWHSMap.PAS:738
    public void ShowHexSelector()
    {
        if (!_hexSelectorVisible)
        {
            _hexSelectorVisible = true;
            _ = DynamicFixedVisibleHexSprite.AddChild(_backHexSelector);
            _ = DynamicFixedVisibleHexSprite.AddChild(_frontHexSelector);
            UpdateHexSelector();
        }
    }

    // AoWHSMap.PAS:748
    public void HideHexSelector()
    {
        if (_hexSelectorVisible)
        {
            _ = DynamicVisibleHexSprite.RemoveChild(_backHexSelector);
            _ = DynamicVisibleHexSprite.RemoveChild(_frontHexSelector);
            _ = DynamicFixedVisibleHexSprite.RemoveChild(_backHexSelector);
            _ = DynamicFixedVisibleHexSprite.RemoveChild(_frontHexSelector);
            _hexSelectorVisible = false;
        }
    }

    // AoWHSMap.PAS:867
    public void UpdateFPS(int fPS) => FPS = fPS;

    // AoWHSMap.PAS:892
    public void DrawSelectionRectangle(int pixelXSurface, int pixelYSurface, in Rect R, in LockedSurface lockedSurface)
    {
        if (R.Left > 0 && R.Top > 0 && pixelXSurface < R.Right && pixelYSurface < R.Bottom && R.Right < ViewWidth + pixelXSurface && R.Bottom < ViewHeight + pixelYSurface)
        {
            int color = 2_134_015;
            DrawLine(R.Left - pixelXSurface, R.Top - pixelYSurface, R.Right - pixelXSurface, R.Top - pixelYSurface, color, lockedSurface);
            DrawLine(R.Right - pixelXSurface, R.Top - pixelYSurface, R.Right - pixelXSurface, R.Bottom - pixelYSurface, color, lockedSurface);
            DrawLine(R.Right - pixelXSurface, R.Bottom - pixelYSurface, R.Left - pixelXSurface, R.Bottom - pixelYSurface, color, lockedSurface);
            DrawLine(R.Left - pixelXSurface, R.Bottom - pixelYSurface, R.Left - pixelXSurface, R.Top - pixelYSurface, color, lockedSurface);
        }
    }

    // AoWHSMap.PAS:666 advances a pointer one row. GetField retains that row order.
    public void DrawFog(in LockedSurface surface)
    {
        bool exploration = ExplorationEnabled || StrategicMap.Current.SeatedPlayer == -1;
        if (!FogEnabled && !exploration)
        {
            return;
        }

        MapLevel level = MapContainer.GetMapLevel(ViewMapLevel);
        for (int x = ViewHexX; x < ViewHexX + ViewWidthHex; x++)
        {
            HexGeometry.HexToPixel(x, ViewHexY, out int pixelX, out int pixelY);
            pixelX -= ViewPixelX + 8;
            pixelY -= ViewPixelY + 4;
            for (int y = ViewHexY; y < ViewHexY + ViewHeightHex; y++, pixelY += 32)
            {
                MapField field = level.GetField(x, y);
                bool unexplored = exploration && (SeatedPlayer == -1 || !field.GetExplored(SeatedPlayer));
                if (unexplored)
                {
                    GamePack.Current.GeneralImages[3].Show(pixelX, pixelY, lockedSurface: surface);
                }
                else if (FogEnabled && field.FogCount == 0)
                {
                    GamePack.Current.GeneralImages[1].Show(pixelX, pixelY, lockedSurface: surface);
                }
            }
        }
    }

    public void DrawGrid(in LockedSurface surface)
    {
        MapLevel level = MapContainer.GetMapLevel(ViewMapLevel);
        int maxX = ViewHexX + ViewWidthHex - 1, maxY = ViewHexY + ViewHeightHex - 1;
        for (int x = ViewHexX; x <= maxX; x++)
        {
            HexGeometry.HexToPixel(x, ViewHexY, out int pixelX, out int pixelY);
            pixelX = pixelX - ViewPixelX - 8;
            pixelY = pixelY - ViewPixelY - 4 + 4;
            for (int y = ViewHexY; y <= maxY; y++, pixelY += 32)
            {
                if (level.GetField(x, y).TerrainType != 15)
                {
                    GamePack.Current.FlagControl.Flags[7].Show(pixelX, pixelY, lockedSurface: surface);
                }
            }
        }
    }

    private static void DrawLine(int x1, int y1, int x2, int y2, int color, in LockedSurface lockedSurface)
    {
        PixelFormat pixelFormatId = lockedSurface.PixelFormatId;
        if (pixelFormatId is PixelFormat.Rgb555 or PixelFormat.Argb1555 or PixelFormat.Rgb565)
        {
            int number2 = PixelFormats.Color2PixelInfo(color, lockedSurface.PixelFormatId, palette: null);
            DrawLine16(x1, y1, x2, y2, skip: 0, color: (ushort)number2, in lockedSurface);
        }
    }

    private static void DrawLine16(int x1, int y1, int x2, int y2, int skip, ushort color, in LockedSurface lockedSurface) => Blit.DrawLine16(x1, y1, x2, y2, skip, color, destination: lockedSurface.View());
}
