// Translated from HSEngine.pas and AoWE.pas.
using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Movement;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

// the map. FMouse, FMapContainer, FEngine, FSelection, FStaticVisibleHS, FDynamicVisibleHS (the
// sprites in the view port), FStaticFixedVisibleHS, FDynamicFixedVisibleHS (sprites shown
// wherever the view is), FTimer, FStaticSceneInvalid, FDynamicSceneInvalid, FVersion,
// _hssFileName, _status, _frame, TerrainChanged, OnCenterView, OnChangeView,
// OnViewLevel, OnProgress, _terrainChangedLockCounter, FrameStarted,
// ViewPortChanged, MapDestroying, _hexSpriteChangedEvents, _fileName, _viewPixelX, _viewPixelY,
// FViewL, FViewWidth, FViewHeight (the view port in pixels), FViewXhx, FViewYhx, FViewWidthHX,
// FViewHeightHX (the hexagons it covers). Persisted (ReadWrite): 2 FHSSFileName (rwString, not
// in text streams), 3 FVersion (rwByte of a Shortint copy), 4 FViewXhp, 5 FViewYhp, 6 FViewL
// (rwInteger). FVersion is a guessed name: nothing but ReadWrite touches it
public partial class GameMap : ENode
{
    public new const int ClassId = 66048;

    public GameMap(ECustomNode owner) : base(owner)
    {
        _mouse = CreateMouse();
        _mouse.HexSpriteMap = this;
        _selection = new HexSpriteSelection(this);
        _selection.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        _staticVisibleHexSprite = new VisibilityList(this);
        _staticVisibleHexSprite.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        _dynamicVisibleHexSprite = new VisibilityList(this);
        _dynamicVisibleHexSprite.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        _staticFixedVisibleHexSprite = new VisibilityList(this);
        _staticFixedVisibleHexSprite.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        _dynamicFixedVisibleHexSprite = new VisibilityList(this);
        _dynamicFixedVisibleHexSprite.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        _timer = new ETimerNode(this);
        HexSpriteChangedEventsStore = new();
        TerrainChangedEventsStore = new();
        ViewPortChangedEventsStore = new();
        NewFrameEventsStore = new();
        DestroyEventsStore = new();

        _showMessageEventsStore = new();
        _mapChangedEventsStore = new();
        _scannerIconsChangedEventsStore = new();
        _backHexSelector = new CustomFixedVisibleHexSprite(null) { Holder = CreationHolder() };
        _backHexSelector.OnShow += ShowBackHexSelector;
        _frontHexSelector = new CustomFixedVisibleHexSprite(null) { Holder = CreationHolder() };
        _frontHexSelector.OnShow += ShowFrontHexSelector;
        _hexSelectorVisible = false;
        ExplorationEnabled = true;
        FogEnabled = true;
        SeatedPlayer = 1;
        FPS = 30;
    }

    private readonly HexSpriteMapMouse _mouse;
    private MapContainer _mapContainer;
    private GameEngine _engine;
    private readonly HexSpriteSelection _selection;
    private readonly VisibilityList _staticVisibleHexSprite;
    private readonly VisibilityList _dynamicVisibleHexSprite;
    private readonly VisibilityList _staticFixedVisibleHexSprite;
    private readonly VisibilityList _dynamicFixedVisibleHexSprite;
    private readonly ETimerNode _timer;
    private bool _staticSceneInvalid;
    private bool _dynamicSceneInvalid;
    private int _version;
    private string _hssFileName = "";
    private Set<HexSpriteMapStatusElement> _status;
    private int _frame;

    internal EventStore<TerrainChangedNotification> TerrainChangedEventsStore { get; }

    internal event NotifyEvent<TerrainChangedNotification> TerrainChanged
    {
        add => TerrainChangedEventsStore.Add(value);
        remove => TerrainChangedEventsStore.Remove(value);
    }

    public event CenterViewEvent OnCenterView;

    internal void SetCenterViewHandler(CenterViewEvent handler) => OnCenterView = handler;

    internal void ClearCenterViewHandler() => OnCenterView = null;

    public event ChangeViewEvent OnChangeView;

    internal void SetChangeViewHandler(ChangeViewEvent handler) => OnChangeView = handler;

    public event ViewLevelEvent OnViewLevel;

    internal void SetViewLevelHandler(ViewLevelEvent handler) => OnViewLevel = handler;

    internal void ClearViewLevelHandler() => OnViewLevel = null;

    public event Aow.Engine.ProgressEvent OnProgress;

    protected void RaiseProgress(EngineObject sender, string message, int progression)
    {
        if (OnProgress != null)
        {
            OnProgress(sender, message, progression);
        }
    }

    protected bool HasProgressHandlers => OnProgress != null;

    internal void SetProgressHandler(Aow.Engine.ProgressEvent handler) => OnProgress = handler;

    internal void ClearProgressHandler() => OnProgress = null;

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

    internal event NotifyEvent<HexSprite> _hexSpriteChangedEvents
    {
        add => HexSpriteChangedEventsStore.Add(value);
        remove => HexSpriteChangedEventsStore.Remove(value);
    }

    private string _fileName = "";
    protected internal int _viewPixelX;
    protected internal int _viewPixelY;
    protected internal int _viewMapLevel;
    private int _viewWidth;
    private int _viewHeight;
    private int _viewHexX;
    private int _viewHexY;
    private int _viewWidthHex;
    private int _viewHeightHex;

    // AoWHSMap.PAS:383 around HSMAP.PAS:37.
    public override void Dispose()
    {
        _showMessageEventsStore.Clear();
        _mapChangedEventsStore.Clear();
        HideHexSelector();
        _backHexSelector?.Dispose();
        _frontHexSelector?.Dispose();
        _scannerIconsChangedEventsStore.Clear();
        EngineMessage message = default;
        LockTerrainChangedEvent();
        _status = _status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Destroying);
        _selection.OnSelectStore.Clear();
        _selection.OnDeselectStore.Clear();
        _selection.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        _selection.UnselectAll();
        DestroyEventsStore.Raise(this);
        HexSpriteChangedEventsStore.Clear();
        _staticVisibleHexSprite.QuickClear();
        _dynamicVisibleHexSprite.QuickClear();
        _staticFixedVisibleHexSprite.QuickClear();
        _dynamicFixedVisibleHexSprite.QuickClear();
        EngineMessage.InitializeMessage(out message, 536936471, this);
        SendMessage(ref message, this);
        NewFrameEventsStore.Clear();
        base.Dispose();
        HexSpriteChangedEventsStore.Clear();
        TerrainChangedEventsStore.Clear();
        ViewPortChangedEventsStore.Clear();
        NewFrameEventsStore.Clear();
        _mouse?.Dispose();
        DestroyEventsStore.Clear();
        UnlockTerrainChangedEvent();

        _showMessageEventsStore.Clear();
        _mapChangedEventsStore.Clear();
    }

    // HSMAP.PAS:76
    public override Set<ControlStyleElement> GetControlStyle() => base.GetControlStyle() + Set<ControlStyleElement>.Of(EngineServices.Reserved2);

    // HSMAP.PAS:240
    public override void ReadWrite(StorageStream stream)
    {
        sbyte B = default;
        base.ReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Partial)) == Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Partial))
        {
            _status = _status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.PreviewMode);
        }

        if (!stream.StorageMode().Contains(EngineServices.Text))
        {
            stream.String(ref _hssFileName, 2);
        }

        if (!stream.StorageMode().Contains(EngineServices.Partial) && stream.StorageMode().Contains(StorageModeElement.Read) && !IsEmpty(_hssFileName))
        {
            ((GameEngine)stream.Engine).InitializeHexSpriteSet(stream.Engine.StartupDirectory + _hssFileName, stream.ProgressHandler);
        }

        B = (sbyte)_version;
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref B), 3);
        _version = B;
        stream.Int32(ref _viewPixelX, 4);
        stream.Int32(ref _viewPixelY, 5);
        stream.Int32(ref _viewMapLevel, 6);
    }

    // HSMAP.PAS:261
    public override void MainReadWrite(StorageStream stream)
    {
        base.MainReadWrite(stream);
        if (!_status.Contains(HexSpriteMapStatusElement.PreviewMode) && (stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Text, StorageModeElement.SetupCompare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            _mapContainer.Loaded();
        }
    }

    // HSMAP.PAS:81
    public override void Clear()
    {
        _staticVisibleHexSprite.QuickClear();
        _dynamicVisibleHexSprite.QuickClear();
        _selection.QuickClear();
        base.Clear();
        _mapContainer = null;
        ClearViewPort();
    }

    // HSMAP.PAS:189
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        if (message.MessageId == 536870920)
        {
            _engine = message.Payload.As<GameEngine>();
        }

        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            _mouse.Reset();
        }
    }

    // HSMAP.PAS makes the engine mouse. AoWHSMap.PAS:396 makes the game mouse.
    public virtual HexSpriteMapMouse CreateMouse() => new GameMapMouse();

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
        var notification = new TerrainChangedNotification();
        ref TerrainChangedNotification.TerrainChangedInfo info = ref notification.Info;
        info.Rectangle = R;
        info.Level = L;
        var mapLevel = _mapContainer.GetMapLevel(L);
        info.MapLevel = mapLevel;
        TerrainChangedEventsStore.Raise(notification);

        if (MapChangedLockCount == 0)
        {
            TriggerMapChangedEvent(R, L);
        }
    }

    // HSMAP.PAS:705
    public virtual void DrawStaticHexSprite(ref LockedSurface lockedSurface)
    {
        if (_staticSceneInvalid)
        {
            throw new InvalidOperationException("Static scene is invalid, call UpdateStaticScene!");
        }

        for (int I = _staticVisibleHexSprite._count - 1; I >= 0; I--)
        {
            _staticVisibleHexSprite[I].Show(_viewPixelX, _viewPixelY, lockedSurface);
        }
    }

    // HSMAP.PAS:725
    public virtual void DrawDynamicHexSprite(ref LockedSurface lockedSurface)
    {
        if (_dynamicSceneInvalid)
        {
            throw new InvalidOperationException("Dynamic scene is invalid, call UpdateDynamicScene!");
        }

        for (int I = _dynamicVisibleHexSprite._count - 1; I >= 0; I--)
        {
            _dynamicVisibleHexSprite[I].Show(_viewPixelX, _viewPixelY, lockedSurface);
        }
    }

    // HSMAP.PAS:715
    public virtual void DrawStaticExtras(ref LockedSurface lockedSurface)
    {
    }

    // HSMAP.PAS:735
    public virtual void DrawDynamicExtras(ref LockedSurface lockedSurface)
    {
    }

    // HSMAP.PAS:273
    public virtual void FillLevelWithResource(int L, MapObjectResource resource)
    {
        for (int Y = 0, last = _mapContainer.Height - 1; Y <= last; Y++)
        {
            for (int X = 0, lastX = _mapContainer.Width - 1; X <= lastX; X++)
            {
                resource.PlaceResourceUser(this, (sbyte)X, (sbyte)Y, (sbyte)L);
            }
        }
    }

    // HSMAP.PAS:96
    public virtual bool AddMapLevel() => _mapContainer.AddLevel(65794);

    // HSMAP.PAS:101
    public virtual void Activate()
    {
        if (_status.Contains(HexSpriteMapStatusElement.Activated))
        {
            throw new InvalidOperationException("HSMap already activated");
        }

        _status = _status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Activated);
    }

    // HSMAP.PAS:108
    public virtual void Deactivate()
    {
        if (!_status.Contains(HexSpriteMapStatusElement.Activated))
        {
            throw new InvalidOperationException("HSMap wasn't activated");
        }

        _status = _status - Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Activated);
    }

    // HSMAP.PAS:128
    // { Tests OnCenterView instead of OnViewLevel (original slip). }
    public virtual void ViewLevel(int hexLevel)
    {
        if (OnCenterView != null && hexLevel < _mapContainer.LevelCount)
        {
            OnViewLevel(hexLevel);
        }
    }

    // HSMAP.PAS:115
    public virtual void CenterViewTo(int hexX, int hexY, int hexLevel)
    {
        if (OnCenterView != null)
        {
            OnCenterView(hexX, hexY, hexLevel);
        }
    }

    // HSMAP.PAS:121
    public virtual void ChangeViewTo(int pixelX, int pixelY, int lHitPoints)
    {
        if (OnChangeView != null)
        {
            OnChangeView(pixelX, pixelY, lHitPoints);
        }
    }

    // HSMAP.PAS:579
    public virtual void GetViewCenter(out int hexX, out int hexY, out int hexLevel)
    {
        HexGeometry.PixelToHex(_viewPixelX + (int)(((uint)_viewWidth) >> 1), _viewPixelY + (int)(((uint)_viewHeight) >> 1), out hexX, out hexY);
        hexLevel = _viewMapLevel;
    }

    // HSMAP.PAS:134
    public virtual void MakeVisible(int hexX, int hexY, int hexLevel, int radius)
    {
        if (_viewMapLevel != hexLevel || hexX - radius < _viewHexX || hexX + radius >= _viewHexX + _viewWidthHex || hexY - radius < _viewHexY || hexY + radius >= _viewHexY + _viewHeightHex)
        {
            CenterViewTo(hexX, hexY, hexLevel);
        }
    }

    // HSMAP.PAS:543
    public virtual bool CreateNewMap(NewHexSpriteMapSettings settings)
    {
        bool result = default;
        Clear();
        try
        {
            SetHssFileName(settings.HssFileName);
            _engine.InitializeHexSpriteSet(settings.HssFileName, null);
            _mapContainer = (MapContainer)_engine.CreateEngineObject(settings.MapContainerClassId, this);
            _engine.ActivateEngineObject(_mapContainer);
            _mapContainer.New(settings);
            result = true;
            InitializeNewMap();
            Activate();
        }
        catch
        {
            Clear();
            throw;
        }

        return result;
    }

    // HSMAP.PAS:284
    // { ChangeTerrain, ChangeTerrainEx and ClearTerrain clip the right edge of the changed rectangle
    //   into its Left (original slip). }
    public virtual int ChangeTerrain(EngineObject mapObject, int X, int Y, int L, int radius, int terrainType)
    {
        SByteList list;
        int count;
        MapField mapField;
        HexNumberWalkData walk = default;
        Rect R = default;
        MapLevel mapLevel;
        LockTerrainChangedEvent();
        list = new SByteList();
        mapLevel = _mapContainer.GetMapLevel(L);
        count = 0;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
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

        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
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

        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
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
            R.Left = X - radius;
            if (R.Left < 0)
            {
                R.Left = 0;
            }

            R.Right = X + radius + 1;
            if (R.Right > mapLevel.Width)
            {
                R.Left = mapLevel.Width;
            }

            R.Top = Y - radius;
            if (R.Top < 0)
            {
                R.Top = 0;
            }

            R.Bottom = Y + radius + 1;
            if (R.Bottom > mapLevel.Height)
            {
                R.Bottom = mapLevel.Height;
            }

            NotifyTerrainChanged(R, (sbyte)L);
        }

        return count;
    }

    // HSMAP.PAS:357
    public virtual int ChangeTerrain(EngineObject mapObject, int X, int Y, int L, int radius, ChangeTerrainProc changeTerrainProc, TerrainChangedProc terrainChangedProc)
    {
        int result;
        SByteList orgList;
        SByteList newList;
        BitList changed;
        int count;
        sbyte terrainType = default;
        sbyte overlayType = default;
        MapField mapField;
        HexNumberWalkData walk = default;
        Rect R = default;
        MapLevel mapLevel;
        if (changeTerrainProc != null)
        {
            LockTerrainChangedEvent();
            orgList = new SByteList();
            newList = new SByteList();
            changed = new BitList();
            mapLevel = _mapContainer.GetMapLevel(L);
            count = 0;
            HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    mapField = mapLevel.GetField(walk.X, walk.Y);
                    terrainType = mapField.TerrainType;
                    overlayType = mapField.OverlayType;
                    orgList[walk.HexNumber * 2] = terrainType;
                    orgList[walk.HexNumber * 2 + 1] = overlayType;
                    changeTerrainProc(mapField, ref terrainType, ref overlayType);
                    mapField.TerrainType = terrainType;
                    mapField.OverlayType = overlayType;
                    newList[walk.HexNumber * 2] = terrainType;
                    newList[walk.HexNumber * 2 + 1] = overlayType;
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    mapField = mapLevel.GetField(walk.X, walk.Y);
                    if (!mapField.CanChangeTerrain(mapObject))
                    {
                        mapField.TerrainType = orgList[walk.HexNumber * 2];
                        mapField.OverlayType = orgList[walk.HexNumber * 2 + 1];
                        changed[walk.HexNumber] = false;
                    }
                    else
                    {
                        changed[walk.HexNumber] = true;
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    if (changed[walk.HexNumber])
                    {
                        var item = newList[walk.HexNumber * 2];
                        mapLevel.GetField(walk.X, walk.Y).TerrainType = item;
                        var newListItem = newList[walk.HexNumber * 2 + 1];
                        mapLevel.GetField(walk.X, walk.Y).OverlayType = newListItem;
                        mapLevel.GetField(walk.X, walk.Y).TerrainChanged();
                        count += 1;
                    }

                    if (terrainChangedProc != null)
                    {
                        var item = orgList[walk.HexNumber * 2 + 1];
                        var orgListItem = orgList[walk.HexNumber * 2];
                        terrainChangedProc(mapLevel.GetField(walk.X, walk.Y), orgListItem, item);
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            UnlockTerrainChangedEvent();
            changed?.Dispose();
            orgList?.Dispose();
            newList?.Dispose();
            if (count != 0)
            {
                R.Left = X - radius;
                if (R.Left < 0)
                {
                    R.Left = 0;
                }

                R.Right = X + radius + 1;
                if (R.Right > mapLevel.Width)
                {
                    R.Left = mapLevel.Width;
                }

                R.Top = Y - radius;
                if (R.Top < 0)
                {
                    R.Top = 0;
                }

                R.Bottom = Y + radius + 1;
                if (R.Bottom > mapLevel.Height)
                {
                    R.Bottom = mapLevel.Height;
                }

                NotifyTerrainChanged(R, (sbyte)L);
            }

            result = count;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWHSMap.PAS:810 shows the explored fields only.
    public virtual void UpdateStaticScene()
    {
        MapLevel level;
        int maxX;
        int maxY;
        MapField field;
        if (ExplorationEnabled && SeatedPlayer != -1 && !Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            ResetVisibleStaticHexSprite();
            maxX = ViewHexX + ViewWidthHex - 1;
            maxY = ViewHexY + ViewHeightHex - 1;
            level = MapContainer.GetMapLevel(ViewMapLevel);
            for (int X = ViewHexX; X <= maxX; X++)
            {
                for (int Y = maxY, last = ViewHexY; Y >= last; Y--)
                {
                    field = level.GetField(X, Y);
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
        int maxX;
        int maxY;
        MapLevel mapLevel;
        ResetVisibleStaticHexSprite();
        maxX = _viewHexX + _viewWidthHex - 1;
        maxY = _viewHexY + _viewHeightHex - 1;
        mapLevel = _mapContainer.GetMapLevel(_viewMapLevel);
        for (int X = _viewHexX; X <= maxX; X++)
        {
            for (int Y = maxY, last = _viewHexY; Y >= last; Y--)
            {
                mapLevel.GetField(X, Y).CopyStaticVisibleHexSprite(_staticVisibleHexSprite);
            }
        }

        for (int X = 0, last = _staticFixedVisibleHexSprite._count - 1; X <= last; X++)
        {
            _staticFixedVisibleHexSprite[X].MakeVisible(_staticVisibleHexSprite);
        }

        _staticVisibleHexSprite.Sort();
        _staticSceneInvalid = false;
    }

    // AoWHSMap.PAS:833 shows the explored fields only.
    public virtual void UpdateDynamicScene()
    {
        MapLevel level;
        int maxX;
        int maxY;
        MapField field;
        if (ExplorationEnabled && !Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            ResetVisibleDynamicHexSprite();
            maxX = ViewHexX + ViewWidthHex - 1;
            maxY = ViewHexY + ViewHeightHex - 1;
            level = MapContainer.GetMapLevel(ViewMapLevel);
            if (SeatedPlayer != -1)
            {
                for (int X = ViewHexX; X <= maxX; X++)
                {
                    for (int Y = maxY, last = ViewHexY; Y >= last; Y--)
                    {
                        field = level.GetField(X, Y);
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
            UpdateWholeDynamicScene();
        }
    }

    // HSMAP.PAS:687 shows every field of the view, explored or not.
    private void UpdateWholeDynamicScene()
    {
        int maxX;
        int maxY;
        MapLevel mapLevel;
        ResetVisibleDynamicHexSprite();
        maxX = _viewHexX + _viewWidthHex - 1;
        maxY = _viewHexY + _viewHeightHex - 1;
        mapLevel = _mapContainer.GetMapLevel(_viewMapLevel);
        for (int X = _viewHexX; X <= maxX; X++)
        {
            for (int Y = maxY, last = _viewHexY; Y >= last; Y--)
            {
                mapLevel.GetField(X, Y).CopyDynamicVisibleHexSprite(_dynamicVisibleHexSprite);
            }
        }

        for (int X = 0, last = _dynamicFixedVisibleHexSprite._count - 1; X <= last; X++)
        {
            _dynamicFixedVisibleHexSprite[X].MakeVisible(_dynamicVisibleHexSprite);
        }

        _dynamicVisibleHexSprite.Sort();
        _dynamicSceneInvalid = false;
    }

    // HSMAP.PAS:719
    public virtual void DrawStaticScene(ref LockedSurface lockedSurface)
    {
        DrawStaticHexSprite(ref lockedSurface);
        DrawStaticExtras(ref lockedSurface);
    }

    // HSMAP.PAS:739
    public virtual void DrawDynamicScene(ref LockedSurface lockedSurface)
    {
        DrawDynamicHexSprite(ref lockedSurface);
        DrawDynamicExtras(ref lockedSurface);
    }

    // HSMAP.PAS:752, then AoWHSMap.PAS:861.
    public virtual void NewFrame()
    {
        _frame += 1;
        if (_frame > 1048575)
        {
            _frame = 0;
        }

        NewFrameEventsStore.Raise(this);

        UpdateShowMessage();
        if (_hexSelectorVisible)
        {
            UpdateHexSelector();
        }
    }

    // HSMAP.PAS:91
    public void RemoveMapLevel(int index)
    {
        _mapContainer.RemoveLevel(index);
    }

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
        MapLevel mapLevel;
        Rect R = default;
        mapLevel = _mapContainer.GetMapLevel(L);
        R.Left = X - radius;
        if (R.Left < 0)
        {
            R.Left = 0;
        }

        R.Right = X + radius + 1;
        if (R.Right > mapLevel.Width)
        {
            R.Right = mapLevel.Width;
        }

        R.Top = Y - radius;
        if (R.Top < 0)
        {
            R.Top = 0;
        }

        R.Bottom = Y + radius + 1;
        if (R.Bottom > mapLevel.Height)
        {
            R.Bottom = mapLevel.Height;
        }

        NotifyTerrainChanged(R, L);
    }

    // HSMAP.PAS:178
    public void LockTerrainChangedEvent()
    {
        _terrainChangedLockCounter += 1;
    }

    // HSMAP.PAS:183
    public void UnlockTerrainChangedEvent()
    {
        if (_terrainChangedLockCounter == 0)
        {
            HexGeometry.OriginalAssert("Invalid TerrainChangedLockCounter", "D:\\GEngine\\HSENGINE\\inc\\HSMAP.PAS", 5989);
        }

        _terrainChangedLockCounter -= 1;
    }

    // HSMAP.PAS:198
    public void SetModified()
    {
        _status = _status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Modified);
    }

    // HSMAP.PAS:203
    public void ResetModified()
    {
        _status = _status - Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Modified);
    }

    // HSMAP.PAS:208
    public int GetMaxPixelX()
    {
        int X = default;
        int Y = default;
        HexGeometry.HexToPixel(this._mapContainer.Width - 1, this._mapContainer.Height - 1, out X, out Y);

        X += 40;
        return X;
    }

    // HSMAP.PAS:218
    public int GetMaxPixelY()
    {
        int Y = default;
        int X = default;
        if (this._mapContainer.Height > 1)
        {
            HexGeometry.HexToPixel(1, this._mapContainer.Height - 1, out X, out Y);
        }
        else
        {
            HexGeometry.HexToPixel(0, this._mapContainer.Height - 1, out X, out Y);
        }

        Y += 32;
        return Y;
    }

    // HSMAP.PAS:231
    public void SetHssFileName(string fileName)
    {
        var expandFileName = ExpandFileName(fileName);
        _hssFileName = ExtractRelativePath(_engine.StartupDirectory, expandFileName);
    }

    // HSMAP.PAS:459
    public void ClearTerrain(int X, int Y, int L, int radius, int terrainType)
    {
        HexNumberWalkData walk = default;
        MapLevel mapLevel;
        MapFieldMessage message = default;
        Rect R = default;
        LockTerrainChangedEvent();
        mapLevel = _mapContainer.GetMapLevel(L);
        HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
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
            ChangeTerrain(null, X, Y, L, radius, terrainType);
        }

        UnlockTerrainChangedEvent();
        R.Left = X - radius;
        if (R.Left < 0)
        {
            R.Left = 0;
        }

        R.Right = X + radius + 1;
        if (R.Right > mapLevel.Width)
        {
            R.Left = mapLevel.Width;
        }

        R.Top = Y - radius;
        if (R.Top < 0)
        {
            R.Top = 0;
        }

        R.Bottom = Y + radius + 1;
        if (R.Bottom > mapLevel.Height)
        {
            R.Bottom = mapLevel.Height;
        }

        NotifyTerrainChanged(R, (sbyte)L);
    }

    // HSMAP.PAS:561
    public bool PlaceHex(HexSprite hexSprite, int hexX, int hexY, int L)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(hexX, hexY, out pixelX, out pixelY);
        return hexSprite.MainPlace(_mapContainer, pixelX, pixelY, L);
    }

    // HSMAP.PAS:569
    public bool Place(HexSprite hexSprite, int pixelX, int pixelY, int L) => hexSprite.MainPlace(_mapContainer, pixelX, pixelY, L);

    // HSMAP.PAS:574
    public void Remove(HexSprite hexSprite)
    {
        hexSprite.MainRemove();
    }

    // HSMAP.PAS:585
    public bool SetViewPort(int pixelX, int pixelY, int lHitPoints, int width, int height)
    {
        bool result;
        int x1 = default;
        int y1 = default;
        int x2 = default;
        int y2 = default;
        if (pixelX != _viewPixelX || pixelY != _viewPixelY || lHitPoints != _viewMapLevel || width != _viewWidth || height != _viewHeight)
        {
            ClearViewPort();
            _viewMapLevel = lHitPoints;
            _viewPixelX = pixelX;
            _viewWidth = width;
            _viewPixelY = pixelY;
            _viewHeight = height;
            HexGeometry.PixelToHex(_viewPixelX - 8, _viewPixelY - 16, out x1, out y1);
            HexGeometry.PixelToHex(_viewPixelX + width - 1 + 8, _viewPixelY + height - 1 + 16, out x2, out y2);
            if (x2 >= _mapContainer.Width)
            {
                x2 = _mapContainer.Width - 1;
            }

            if (y2 >= _mapContainer.Height)
            {
                y2 = _mapContainer.Height - 1;
            }

            if (x1 < 0)
            {
                x1 = 0;
            }

            if (y1 < 0)
            {
                y1 = 0;
            }

            _viewWidthHex = x2 - x1 + 1;
            _viewHeightHex = y2 - y1 + 1;
            _viewHexX = x1;
            _viewHexY = y1;
            result = true;
            ViewPortChangedEventsStore.Raise(this);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // HSMAP.PAS:619
    public void ClearViewPort()
    {
        ResetVisibleStaticHexSprite();
        ResetVisibleDynamicHexSprite();
        _staticSceneInvalid = true;
        _dynamicSceneInvalid = true;
        _viewPixelX = -1;
        _viewPixelY = -1;
        _viewMapLevel = -1;
        _viewWidth = 0;
        _viewHeight = 0;
    }

    // HSMAP.PAS:633
    // { Viewing accepts one row below the view port, InvalidateStaticScene does not. }
    public bool Viewing(int hexX, int hexY, int hexLevel) => hexX >= _viewHexX && hexX < _viewHexX + _viewWidthHex && hexY >= _viewHexY && hexY <= _viewHexY + _viewHeightHex && hexLevel == _viewMapLevel;

    // HSMAP.PAS:640
    // { The message names ResetVisibleDynamicHS (original slip). }
    public void ResetVisibleStaticHexSprite()
    {
        try
        {
            for (int I = _staticVisibleHexSprite._count - 1; I >= 0; I--)
            {
                _staticVisibleHexSprite[I].SetVisible(false);
            }

            _staticVisibleHexSprite.QuickClear();
            _staticVisibleHexSprite.Sorted = false;
        }
        catch
        {
            throw new InvalidOperationException("Exception during ResetVisibleDynamicHS");
        }
    }

    // HSMAP.PAS:654
    public void ResetVisibleDynamicHexSprite()
    {
        for (int I = _dynamicVisibleHexSprite._count - 1; I >= 0; I--)
        {
            try
            {
                _dynamicVisibleHexSprite[I].SetVisible(false);
            }
            catch
            {
                throw new InvalidOperationException("Exception during ResetVisibleDynamicHS (" + _dynamicVisibleHexSprite.Items[I].Child.GetType().Name + ")");
            }
        }

        _dynamicVisibleHexSprite.QuickClear();
        _dynamicVisibleHexSprite.Sorted = false;
    }

    // HSMAP.PAS:745
    public void InvalidateStaticScene(int hexX, int hexY, int hexLevel)
    {
        _staticSceneInvalid = _staticSceneInvalid || (hexX >= _viewHexX && hexX < _viewHexX + _viewWidthHex && hexY >= _viewHexY && hexY < _viewHexY + _viewHeightHex && hexLevel == _viewMapLevel);
    }

    public HexSpriteMapMouse Mouse { get => _mouse; }
    public MapContainer MapContainer { get => _mapContainer; internal set => _mapContainer = value; }
    public GameEngine Engine { get => _engine; }
    public HexSpriteSelection Selection { get => _selection; }
    public VisibilityList StaticVisibleHexSprite { get => _staticVisibleHexSprite; }
    public VisibilityList DynamicVisibleHexSprite { get => _dynamicVisibleHexSprite; }
    public VisibilityList StaticFixedVisibleHexSprite { get => _staticFixedVisibleHexSprite; }
    public VisibilityList DynamicFixedVisibleHexSprite { get => _dynamicFixedVisibleHexSprite; }
    public ETimerNode Timer { get => _timer; }
    public bool StaticSceneInvalid { get => _staticSceneInvalid; set => _staticSceneInvalid = value; }
    public bool DynamicSceneInvalid { get => _dynamicSceneInvalid; set => _dynamicSceneInvalid = value; }
    public int Version { get => _version; set => _version = value; }
    public string HssFileName { get => _hssFileName; set => SetHssFileName(value); }
    public Set<HexSpriteMapStatusElement> Status { get => _status; set => _status = value; }
    public int Frame { get => _frame; }

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

    public string FileName { get => _fileName; set => _fileName = value; }
    public int ViewPixelX { get => _viewPixelX; }
    public int ViewPixelY { get => _viewPixelY; }
    public int ViewMapLevel { get => _viewMapLevel; }
    public int ViewWidth { get => _viewWidth; }
    public int ViewHeight { get => _viewHeight; }
    public int ViewHexX { get => _viewHexX; }
    public int ViewHexY { get => _viewHexY; }
    public int ViewWidthHex { get => _viewWidthHex; }
    public int ViewHeightHex { get => _viewHeightHex; }
    public int MaxPixelX { get => GetMaxPixelX(); }
    public int MaxPixelY { get => GetMaxPixelY(); }

    public delegate void CenterViewEvent(int Xhx, int Yhx, int Lhx);
    public delegate void ChangeViewEvent(int Xhp, int Yhp, int Lhp);
    public delegate void ViewLevelEvent(int Lhx);

    public sbyte CurrentPlayer;

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
    public bool FogEnabled;
    public bool ExplorationEnabled;

    public bool ShowGrid { get; set; }

    private readonly CustomFixedVisibleHexSprite _backHexSelector;
    private readonly CustomFixedVisibleHexSprite _frontHexSelector;
    private sbyte _selectorX;
    private sbyte _selectorY;
    private bool _hexSelectorVisible;

    // AoWHSMap.PAS:759
    public virtual void ShowBackHexSelector(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        if (_selectorX != -1 && _selectorY != -1 && StrategicMap.Current.Mouse.Capture == StrategicMap.Current.SelectedArmy)
        {
            HexGeometry.HexToPixel(_selectorX, _selectorY, out pixelX, out pixelY);
            pixelX = pixelX - pixelXSurface - 8;
            pixelY = pixelY - pixelYSurface - 32 + 2;
            GamePack.Current.GeneralImages[7].Show(pixelX, pixelY, lockedSurface);
        }
    }

    // AoWHSMap.PAS:772
    public virtual void ShowFrontHexSelector(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        if (_selectorX != -1 && _selectorY != -1 && StrategicMap.Current.Mouse.Capture == StrategicMap.Current.SelectedArmy)
        {
            HexGeometry.HexToPixel(_selectorX, _selectorY, out pixelX, out pixelY);
            pixelX = pixelX - pixelXSurface - 8;
            pixelY = pixelY - pixelYSurface - 32 + 2;
            GamePack.Current.GeneralImages[6].Show(pixelX, pixelY, lockedSurface);
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
                HexGeometry.HexToPixel(_selectorX, _selectorY, out pixelX, out pixelY);
                hexSprite = MapContainer.GetField(_selectorX, _selectorY, ViewMapLevel).GetMaxLevelNoneTransparentHexSprite();
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
        hero.GetLocation(out X, out Y, out L);
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
        hero.GetLocation(out X, out Y, out L);
        if (X != -1)
        {
            items = (ItemHexSprite)StrategicMap.Current.MapContainer.GetField(X, Y, L).FindOwnedChild(131697);
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
        if (!GetEngine().Registry.GetDebugMode() && !IsEmpty(text))
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
        if (!GetEngine().Registry.GetDebugMode() && !IsEmpty(text))
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
        animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
        animation.SetImageSequence(GamePack.Current.ImageSequences.GetImageSequence(11));
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
    public void NotifyMapChanged(in Rect R, int L)
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

        NotifyTerrainChanged(R, (sbyte)L);
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
            NotifyMapChanged(R, I);
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

        NotifyScannerIconsChanged(R, hexLevel);
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
            NotifyScannerIconsChanged(R, I);
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
        bool result = MapContainer.GetMapLevel(level).FogArea(X, Y, range);
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
        bool result = MapContainer.GetMapLevel(level).UnFogArea(X, Y, range);
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
            result = MapContainer.GetMapLevel(level).ExploreArea(X, Y, player, range);
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
        bool exploration = ExplorationEnabled || StrategicMap.Current.SeatedPlayer == -1;
        if (!FogEnabled && !exploration) return;
        var level = MapContainer.GetMapLevel(ViewMapLevel);
        for (int x = ViewHexX; x < ViewHexX + ViewWidthHex; x++)
        {
            int pixelX = 0, pixelY = 0;
            HexGeometry.HexToPixel(x, ViewHexY, out pixelX, out pixelY);
            pixelX -= ViewPixelX + 8;
            pixelY -= ViewPixelY + 4;
            for (int y = ViewHexY; y < ViewHexY + ViewHeightHex; y++, pixelY += 32)
            {
                var field = level.GetField(x, y);
                bool unexplored = exploration && (SeatedPlayer == -1 || !field.GetExplored(SeatedPlayer));
                if (unexplored) GamePack.Current.GeneralImages[3].Show(pixelX, pixelY, surface);
                else if (FogEnabled && ((field.FieldFlags >> 16) & 255) == 0)
                    GamePack.Current.GeneralImages[1].Show(pixelX, pixelY, surface);
            }
        }
    }

    public void DrawGrid(in LockedSurface surface)
    {
        var level = MapContainer.GetMapLevel(ViewMapLevel);
        int maxX = ViewHexX + ViewWidthHex - 1, maxY = ViewHexY + ViewHeightHex - 1;
        for (int x = ViewHexX; x <= maxX; x++)
        {
            int pixelX = 0, pixelY = 0;
            HexGeometry.HexToPixel(x, ViewHexY, out pixelX, out pixelY);
            pixelX = pixelX - ViewPixelX - 8;
            pixelY = pixelY - ViewPixelY - 4 + 4;
            for (int y = ViewHexY; y <= maxY; y++, pixelY += 32)
                if (level.GetField(x, y).TerrainType != 15)
                    GamePack.Current.FlagControl.Flags[7].Show(pixelX, pixelY, surface);
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
