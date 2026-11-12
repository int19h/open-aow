// Translated from HSEngine.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

// the map. FMouse, FMapContainer, FEngine, FSelection, FStaticVisibleHS, FDynamicVisibleHS (the
// sprites in the view port), FStaticFixedVisibleHS, FDynamicFixedVisibleHS (sprites shown
// wherever the view is), FTimer, FStaticSceneInvalid, FDynamicSceneInvalid, FVersion,
// _hssFileName, _status, _frame, FTerrainChangedEvents, _onCenterView, _onChangeView,
// _onViewLevel, _onProgress, _terrainChangedLockCounter, FNewFrameEvents,
// FViewPortChangedEvents, FDestroyEvents, _hexSpriteChangedEvents, _fileName, _viewPixelX, _viewPixelY,
// FViewL, FViewWidth, FViewHeight (the view port in pixels), FViewXhx, FViewYhx, FViewWidthHX,
// FViewHeightHX (the hexagons it covers). Persisted (ReadWrite): 2 FHSSFileName (rwString, not
// in text streams), 3 FVersion (rwByte of a Shortint copy), 4 FViewXhp, 5 FViewYhp, 6 FViewL
// (rwInteger). FVersion is a guessed name: nothing but ReadWrite touches it
public partial class HSMap : ENode
{
    public new const int ClassId = 66048;

    public HSMap(ECustomNode owner) : base(owner)
    {
        _mouse = CreateMouse();
        _mouse._hexSpriteMap = this;
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
        _hexSpriteChangedEventsStore = new();
        _terrainChangedEventsStore = new();
        _viewPortChangedEventsStore = new();
        _newFrameEventsStore = new();
        _destroyEventsStore = new();
    }

    internal HexSpriteMapMouse _mouse;
    internal MapContainer _mapContainer;
    internal GameEngine _engine;
    internal HexSpriteSelection _selection;
    internal VisibilityList _staticVisibleHexSprite;
    internal VisibilityList _dynamicVisibleHexSprite;
    internal VisibilityList _staticFixedVisibleHexSprite;
    internal VisibilityList _dynamicFixedVisibleHexSprite;
    internal ETimerNode _timer;
    internal bool _staticSceneInvalid;
    internal bool _dynamicSceneInvalid;
    internal int _version;
    internal string _hssFileName = "";
    internal Set<HexSpriteMapStatusElement> _status;
    internal int _frame;

    internal EventStore<TerrainChangedNotification> _terrainChangedEventsStore;

    internal event NotifyEvent<TerrainChangedNotification> FTerrainChangedEvents
    {
        add => _terrainChangedEventsStore.Add(value);
        remove => _terrainChangedEventsStore.Remove(value);
    }

    internal CenterViewEvent _onCenterView;
    internal ChangeViewEvent _onChangeView;
    internal ViewLevelEvent _onViewLevel;
    internal Aow.Engine.ProgressEvent _onProgress;
    internal int _terrainChangedLockCounter;

    internal EventStore<HSMap> _newFrameEventsStore;

    internal event NotifyEvent<HSMap> FNewFrameEvents
    {
        add => _newFrameEventsStore.Add(value);
        remove => _newFrameEventsStore.Remove(value);
    }

    internal EventStore<HSMap> _viewPortChangedEventsStore;

    internal event NotifyEvent<HSMap> FViewPortChangedEvents
    {
        add => _viewPortChangedEventsStore.Add(value);
        remove => _viewPortChangedEventsStore.Remove(value);
    }

    internal EventStore<HSMap> _destroyEventsStore;

    internal event NotifyEvent<HSMap> FDestroyEvents
    {
        add => _destroyEventsStore.Add(value);
        remove => _destroyEventsStore.Remove(value);
    }

    internal EventStore<HexSprite> _hexSpriteChangedEventsStore;

    internal event NotifyEvent<HexSprite> _hexSpriteChangedEvents
    {
        add => _hexSpriteChangedEventsStore.Add(value);
        remove => _hexSpriteChangedEventsStore.Remove(value);
    }

    internal string _fileName = "";
    protected internal int _viewPixelX;
    protected internal int _viewPixelY;
    protected internal int _viewL;
    protected internal int _viewWidth;
    protected internal int _viewHeight;
    protected internal int _viewHexX;
    protected internal int _viewHexY;
    protected internal int _viewWidthHex;
    protected internal int _viewHeightHex;

    // HSMAP.PAS:37
    public override void Dispose()
    {
        EngineMessage message = default;
        LockTerrainChangedEvent();
        _status = _status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Destroying);
        _selection._onSelectStore.Clear();
        _selection._onDeselectStore.Clear();
        _selection.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Shared));
        _selection.UnselectAll();
        _destroyEventsStore.Raise(this);
        _hexSpriteChangedEventsStore.Clear();
        _staticVisibleHexSprite.QuickClear();
        _dynamicVisibleHexSprite.QuickClear();
        _staticFixedVisibleHexSprite.QuickClear();
        _dynamicFixedVisibleHexSprite.QuickClear();
        EngineMessage.InitializeMessage(ref message, 536936471, this);
        SendMessage(ref message, this);
        _newFrameEventsStore.Clear();
        base.Dispose();
        _hexSpriteChangedEventsStore.Clear();
        _terrainChangedEventsStore.Clear();
        _viewPortChangedEventsStore.Clear();
        _newFrameEventsStore.Clear();
        _mouse?.Dispose();
        _destroyEventsStore.Clear();
        UnlockTerrainChangedEvent();
    }

    // HSMAP.PAS:76
    public override Set<ControlStyleElement> GetControlStyle() => base.GetControlStyle() + Set<ControlStyleElement>.Of(EngineServices.Reserved2);

    // HSMAP.PAS:240
    public override void ReadWrite(EStorageStream stream)
    {
        sbyte B = default;
        base.ReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Partial)) == Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Partial))
        {
            _status = _status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.PreviewMode);
        }

        if (!stream.StorageMode().Contains(EngineServices.Text))
        {
            stream.rwString(ref _hssFileName, 2);
        }

        if (!stream.StorageMode().Contains(EngineServices.Partial) && stream.StorageMode().Contains(StorageModeElement.Read) && !IsEmpty(_hssFileName))
        {
            ((GameEngine)stream.Engine).InitializeHexSpriteSet(stream.Engine.StartupDirectory + _hssFileName, stream.OnProgress);
        }

        B = (sbyte)_version;
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref B), 3);
        _version = B;
        stream.rwInteger(ref _viewPixelX, 4);
        stream.rwInteger(ref _viewPixelY, 5);
        stream.rwInteger(ref _viewL, 6);
    }

    // HSMAP.PAS:261
    public override void MainReadWrite(EStorageStream stream)
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

    // HSMAP.PAS:71
    public virtual HexSpriteMapMouse CreateMouse() => new HexSpriteMapMouse();

    // HSMAP.PAS:236
    public virtual void NotifyProgress(in MapProgress progress)
    {
    }

    // HSMAP.PAS:269
    public virtual void InitializeNewMap()
    {
    }

    // HSMAP.PAS:141
    public virtual void TriggerTerrainChangedEvent(in Rect R, sbyte L)
    {
        var notification = new TerrainChangedNotification();
        ref TerrainChangedNotification.TerrainChangedInfo info = ref notification.Info;
        info.Rectangle = R;
        info.Level = L;
        var mapLevel = _mapContainer.GetMapLevel(L);
        info.MapLevel = mapLevel;
        _terrainChangedEventsStore.Raise(notification);
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
            ((HexSprite)_staticVisibleHexSprite.Items[I].Child).Show(_viewPixelX, _viewPixelY, lockedSurface);
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
            ((HexSprite)_dynamicVisibleHexSprite.Items[I].Child).Show(_viewPixelX, _viewPixelY, lockedSurface);
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
        for (int Y = 0, last = _mapContainer._height - 1; Y <= last; Y++)
        {
            for (int X = 0, lastX = _mapContainer._width - 1; X <= lastX; X++)
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
        if (_onCenterView != null && hexLevel < _mapContainer._levelCount)
        {
            _onViewLevel(hexLevel);
        }
    }

    // HSMAP.PAS:115
    public virtual void CenterViewTo(int hexX, int hexY, int hexLevel)
    {
        if (_onCenterView != null)
        {
            _onCenterView(hexX, hexY, hexLevel);
        }
    }

    // HSMAP.PAS:121
    public virtual void ChangeViewTo(int pixelX, int pixelY, int lHitPoints)
    {
        if (_onChangeView != null)
        {
            _onChangeView(pixelX, pixelY, lHitPoints);
        }
    }

    // HSMAP.PAS:579
    public virtual void GetViewCenter(ref int hexX, ref int hexY, ref int hexLevel)
    {
        HexGeometry.PixelToHex(_viewPixelX + (int)(((uint)_viewWidth) >> 1), _viewPixelY + (int)(((uint)_viewHeight) >> 1), ref hexX, ref hexY);
        hexLevel = _viewL;
    }

    // HSMAP.PAS:134
    public virtual void MakeVisible(int hexX, int hexY, int hexLevel, int radius)
    {
        if (_viewL != hexLevel || hexX - radius < _viewHexX || hexX + radius >= _viewHexX + _viewWidthHex || hexY - radius < _viewHexY || hexY + radius >= _viewHexY + _viewHeightHex)
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
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
            {
                mapField = mapLevel.GetField(walk.X, walk.Y);
                list[walk.HexNumber] = mapField._terrainType;
                mapField._terrainType = (sbyte)terrainType;
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
            {
                mapField = mapLevel.GetField(walk.X, walk.Y);
                if (!mapField.CanChangeTerrain(mapObject))
                {
                    mapField._terrainType = list[walk.HexNumber];
                    list[walk.HexNumber] = -127;
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
        while (walk.Radius <= radius)
        {
            if (list[walk.HexNumber] != -127 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
            {
                mapLevel.GetField(walk.X, walk.Y)._terrainType = (sbyte)terrainType;
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
            if (R.Right > mapLevel._width)
            {
                R.Left = mapLevel._width;
            }

            R.Top = Y - radius;
            if (R.Top < 0)
            {
                R.Top = 0;
            }

            R.Bottom = Y + radius + 1;
            if (R.Bottom > mapLevel._height)
            {
                R.Bottom = mapLevel._height;
            }

            TerrainChanged(R, (sbyte)L);
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
            HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
                {
                    mapField = mapLevel.GetField(walk.X, walk.Y);
                    terrainType = mapField._terrainType;
                    overlayType = mapField._overlayType;
                    orgList[walk.HexNumber * 2] = terrainType;
                    orgList[walk.HexNumber * 2 + 1] = overlayType;
                    changeTerrainProc(mapField, ref terrainType, ref overlayType);
                    mapField._terrainType = terrainType;
                    mapField._overlayType = overlayType;
                    newList[walk.HexNumber * 2] = terrainType;
                    newList[walk.HexNumber * 2 + 1] = overlayType;
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
                {
                    mapField = mapLevel.GetField(walk.X, walk.Y);
                    if (!mapField.CanChangeTerrain(mapObject))
                    {
                        mapField._terrainType = orgList[walk.HexNumber * 2];
                        mapField._overlayType = orgList[walk.HexNumber * 2 + 1];
                        changed[walk.HexNumber] = false;
                    }
                    else
                    {
                        changed[walk.HexNumber] = true;
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
            while (walk.Radius <= radius)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
                {
                    if (changed[walk.HexNumber])
                    {
                        var item = newList[walk.HexNumber * 2];
                        mapLevel.GetField(walk.X, walk.Y)._terrainType = item;
                        var newListItem = newList[walk.HexNumber * 2 + 1];
                        mapLevel.GetField(walk.X, walk.Y)._overlayType = newListItem;
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
                if (R.Right > mapLevel._width)
                {
                    R.Left = mapLevel._width;
                }

                R.Top = Y - radius;
                if (R.Top < 0)
                {
                    R.Top = 0;
                }

                R.Bottom = Y + radius + 1;
                if (R.Bottom > mapLevel._height)
                {
                    R.Bottom = mapLevel._height;
                }

                TerrainChanged(R, (sbyte)L);
            }

            result = count;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // HSMAP.PAS:669
    public virtual void UpdateStaticScene()
    {
        int maxX;
        int maxY;
        MapLevel mapLevel;
        ResetVisibleStaticHexSprite();
        maxX = _viewHexX + _viewWidthHex - 1;
        maxY = _viewHexY + _viewHeightHex - 1;
        mapLevel = _mapContainer.GetMapLevel(_viewL);
        for (int X = _viewHexX; X <= maxX; X++)
        {
            for (int Y = maxY, last = _viewHexY; Y >= last; Y--)
            {
                mapLevel.GetField(X, Y).CopyStaticVisibleHexSprite(_staticVisibleHexSprite);
            }
        }

        for (int X = 0, last = _staticFixedVisibleHexSprite._count - 1; X <= last; X++)
        {
            ((HexSprite)_staticFixedVisibleHexSprite.Children[X]).MakeVisible(_staticVisibleHexSprite);
        }

        _staticVisibleHexSprite.Sort();
        _staticSceneInvalid = false;
    }

    // HSMAP.PAS:687
    public virtual void UpdateDynamicScene()
    {
        int maxX;
        int maxY;
        MapLevel mapLevel;
        ResetVisibleDynamicHexSprite();
        maxX = _viewHexX + _viewWidthHex - 1;
        maxY = _viewHexY + _viewHeightHex - 1;
        mapLevel = _mapContainer.GetMapLevel(_viewL);
        for (int X = _viewHexX; X <= maxX; X++)
        {
            for (int Y = maxY, last = _viewHexY; Y >= last; Y--)
            {
                mapLevel.GetField(X, Y).CopyDynamicVisibleHexSprite(_dynamicVisibleHexSprite);
            }
        }

        for (int X = 0, last = _dynamicFixedVisibleHexSprite._count - 1; X <= last; X++)
        {
            ((HexSprite)_dynamicFixedVisibleHexSprite.Children[X]).MakeVisible(_dynamicVisibleHexSprite);
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

    // HSMAP.PAS:752
    public virtual void NewFrame()
    {
        _frame += 1;
        if (_frame > 1048575)
        {
            _frame = 0;
        }

        _newFrameEventsStore.Raise(this);
    }

    // HSMAP.PAS:91
    public void RemoveMapLevel(int index)
    {
        _mapContainer.RemoveLevel(index);
    }

    // HSMAP.PAS:151
    public void TerrainChanged(in Rect R, sbyte L)
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
        if (R.Right > mapLevel._width)
        {
            R.Right = mapLevel._width;
        }

        R.Top = Y - radius;
        if (R.Top < 0)
        {
            R.Top = 0;
        }

        R.Bottom = Y + radius + 1;
        if (R.Bottom > mapLevel._height)
        {
            R.Bottom = mapLevel._height;
        }

        TerrainChanged(R, L);
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
        HexGeometry.HexToPixel(this._mapContainer._width - 1, this._mapContainer._height - 1, ref X, ref Y);

        X += 40;
        return X;
    }

    // HSMAP.PAS:218
    public int GetMaxPixelY()
    {
        int Y = default;
        int X = default;
        if (this._mapContainer._height > 1)
        {
            HexGeometry.HexToPixel(1, this._mapContainer._height - 1, ref X, ref Y);
        }
        else
        {
            HexGeometry.HexToPixel(0, this._mapContainer._height - 1, ref X, ref Y);
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
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
        message.MessageId = 16;
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel._width && walk.Y < mapLevel._height)
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
        if (R.Right > mapLevel._width)
        {
            R.Left = mapLevel._width;
        }

        R.Top = Y - radius;
        if (R.Top < 0)
        {
            R.Top = 0;
        }

        R.Bottom = Y + radius + 1;
        if (R.Bottom > mapLevel._height)
        {
            R.Bottom = mapLevel._height;
        }

        TerrainChanged(R, (sbyte)L);
    }

    // HSMAP.PAS:561
    public bool PlaceHex(HexSprite hexSprite, int hexX, int hexY, int L)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(hexX, hexY, ref pixelX, ref pixelY);
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
        if (pixelX != _viewPixelX || pixelY != _viewPixelY || lHitPoints != _viewL || width != _viewWidth || height != _viewHeight)
        {
            ClearViewPort();
            _viewL = lHitPoints;
            _viewPixelX = pixelX;
            _viewWidth = width;
            _viewPixelY = pixelY;
            _viewHeight = height;
            HexGeometry.PixelToHex(_viewPixelX - 8, _viewPixelY - 16, ref x1, ref y1);
            HexGeometry.PixelToHex(_viewPixelX + width - 1 + 8, _viewPixelY + height - 1 + 16, ref x2, ref y2);
            if (x2 >= _mapContainer._width)
            {
                x2 = _mapContainer._width - 1;
            }

            if (y2 >= _mapContainer._height)
            {
                y2 = _mapContainer._height - 1;
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
            _viewPortChangedEventsStore.Raise(this);
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
        _viewL = -1;
        _viewWidth = 0;
        _viewHeight = 0;
    }

    // HSMAP.PAS:633
    // { Viewing accepts one row below the view port, InvalidateStaticScene does not. }
    public bool Viewing(int hexX, int hexY, int hexLevel) => hexX >= _viewHexX && hexX < _viewHexX + _viewWidthHex && hexY >= _viewHexY && hexY <= _viewHexY + _viewHeightHex && hexLevel == _viewL;

    // HSMAP.PAS:640
    // { The message names ResetVisibleDynamicHS (original slip). }
    public void ResetVisibleStaticHexSprite()
    {
        try
        {
            for (int I = _staticVisibleHexSprite._count - 1; I >= 0; I--)
            {
                ((HexSprite)_staticVisibleHexSprite.Items[I].Child).SetVisible(false);
            }

            _staticVisibleHexSprite.QuickClear();
            _staticVisibleHexSprite._sorted = false;
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
                ((HexSprite)_dynamicVisibleHexSprite.Items[I].Child).SetVisible(false);
            }
            catch
            {
                throw new InvalidOperationException("Exception during ResetVisibleDynamicHS (" + _dynamicVisibleHexSprite.Items[I].Child.GetType().Name + ")");
            }
        }

        _dynamicVisibleHexSprite.QuickClear();
        _dynamicVisibleHexSprite._sorted = false;
    }

    // HSMAP.PAS:745
    public void InvalidateStaticScene(int hexX, int hexY, int hexLevel)
    {
        _staticSceneInvalid = _staticSceneInvalid || (hexX >= _viewHexX && hexX < _viewHexX + _viewWidthHex && hexY >= _viewHexY && hexY < _viewHexY + _viewHeightHex && hexLevel == _viewL);
    }

    public HexSpriteMapMouse Mouse { get => _mouse; }
    public MapContainer MapContainer { get => _mapContainer; }
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
        add => _terrainChangedEventsStore.Add(value);
        remove => _terrainChangedEventsStore.Remove(value);
    }

    public CenterViewEvent OnCenterView { get => _onCenterView; set => _onCenterView = value; }
    public ChangeViewEvent OnChangeView { get => _onChangeView; set => _onChangeView = value; }
    public ViewLevelEvent OnViewLevel { get => _onViewLevel; set => _onViewLevel = value; }
    public Aow.Engine.ProgressEvent OnProgress { get => _onProgress; set => _onProgress = value; }

    public event NotifyEvent<HSMap> NewFrameEvents
    {
        add => _newFrameEventsStore.Add(value);
        remove => _newFrameEventsStore.Remove(value);
    }

    public event NotifyEvent<HSMap> ViewPortChangedEvents
    {
        add => _viewPortChangedEventsStore.Add(value);
        remove => _viewPortChangedEventsStore.Remove(value);
    }

    public event NotifyEvent<HSMap> DestroyEvents
    {
        add => _destroyEventsStore.Add(value);
        remove => _destroyEventsStore.Remove(value);
    }

    public event NotifyEvent<HexSprite> HexSpriteChangedEvents
    {
        add => _hexSpriteChangedEventsStore.Add(value);
        remove => _hexSpriteChangedEventsStore.Remove(value);
    }

    public string FileName { get => _fileName; set => _fileName = value; }
    public int ViewPixelX { get => _viewPixelX; }
    public int ViewPixelY { get => _viewPixelY; }
    public int ViewL { get => _viewL; }
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

}
