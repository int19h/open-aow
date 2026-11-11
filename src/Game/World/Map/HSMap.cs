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
// FHSSFileName, FStatus, FFrame, FTerrainChangedEvents, FOnCenterView, FOnChangeView,
// FOnViewLevel, FOnProgress, FTerrainChangedLockCounter, FNewFrameEvents,
// FViewPortChangedEvents, FDestroyEvents, FHSChangedEvents, FFileName, FViewXhp, FViewYhp,
// FViewL, FViewWidth, FViewHeight (the view port in pixels), FViewXhx, FViewYhx, FViewWidthHX,
// FViewHeightHX (the hexagons it covers). Persisted (ReadWrite): 2 FHSSFileName (rwString, not
// in text streams), 3 FVersion (rwByte of a Shortint copy), 4 FViewXhp, 5 FViewYhp, 6 FViewL
// (rwInteger). FVersion is a guessed name: nothing but ReadWrite touches it
public partial class HSMap : ENode
{
    public new const int ClassID = 66048;

    public HSMap(ECustomNode owner) : base(owner)
    {
        FMouse = CreateMouse();
        FMouse.FHSMap = this;
        FSelection = new HexSpriteSelection(this);
        FSelection.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.csShared));
        FStaticVisibleHS = new VisibilityList(this);
        FStaticVisibleHS.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.csShared));
        FDynamicVisibleHS = new VisibilityList(this);
        FDynamicVisibleHS.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.csShared));
        FStaticFixedVisibleHS = new VisibilityList(this);
        FStaticFixedVisibleHS.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.csShared));
        FDynamicFixedVisibleHS = new VisibilityList(this);
        FDynamicFixedVisibleHS.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.csShared));
        FTimer = new ETimerNode(this);
        FHSChangedEventsStore = new();
        FTerrainChangedEventsStore = new();
        FViewPortChangedEventsStore = new();
        FNewFrameEventsStore = new();
        FDestroyEventsStore = new();
    }

    internal HexSpriteMapMouse FMouse;
    internal MapContainer FMapContainer;
    internal GameEngine FEngine;
    internal HexSpriteSelection FSelection;
    internal VisibilityList FStaticVisibleHS;
    internal VisibilityList FDynamicVisibleHS;
    internal VisibilityList FStaticFixedVisibleHS;
    internal VisibilityList FDynamicFixedVisibleHS;
    internal ETimerNode FTimer;
    internal bool FStaticSceneInvalid;
    internal bool FDynamicSceneInvalid;
    internal int FVersion;
    internal string FHSSFileName = "";
    internal Set<HexSpriteMapStatusElement> FStatus;
    internal int FFrame;

    internal EventStore<TerrainChangedNotification> FTerrainChangedEventsStore;

    internal event NotifyEvent<TerrainChangedNotification> FTerrainChangedEvents
    {
        add => FTerrainChangedEventsStore.Add(value);
        remove => FTerrainChangedEventsStore.Remove(value);
    }

    internal CenterViewEvent FOnCenterView;
    internal ChangeViewEvent FOnChangeView;
    internal ViewLevelEvent FOnViewLevel;
    internal Aow.Engine.ProgressEvent FOnProgress;
    internal int FTerrainChangedLockCounter;

    internal EventStore<HSMap> FNewFrameEventsStore;

    internal event NotifyEvent<HSMap> FNewFrameEvents
    {
        add => FNewFrameEventsStore.Add(value);
        remove => FNewFrameEventsStore.Remove(value);
    }

    internal EventStore<HSMap> FViewPortChangedEventsStore;

    internal event NotifyEvent<HSMap> FViewPortChangedEvents
    {
        add => FViewPortChangedEventsStore.Add(value);
        remove => FViewPortChangedEventsStore.Remove(value);
    }

    internal EventStore<HSMap> FDestroyEventsStore;

    internal event NotifyEvent<HSMap> FDestroyEvents
    {
        add => FDestroyEventsStore.Add(value);
        remove => FDestroyEventsStore.Remove(value);
    }

    internal EventStore<HexSprite> FHSChangedEventsStore;

    internal event NotifyEvent<HexSprite> FHSChangedEvents
    {
        add => FHSChangedEventsStore.Add(value);
        remove => FHSChangedEventsStore.Remove(value);
    }

    internal string FFileName = "";
    protected internal int FViewXhp;
    protected internal int FViewYhp;
    protected internal int FViewL;
    protected internal int FViewWidth;
    protected internal int FViewHeight;
    protected internal int FViewXhx;
    protected internal int FViewYhx;
    protected internal int FViewWidthHX;
    protected internal int FViewHeightHX;

    // HSMAP.PAS:37
    public override void Dispose()
    {
        EngineMessage Msg = default;
        LockTerrainChangedEvent();
        FStatus = FStatus + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Destroying);
        FSelection.FOnSelectStore.Clear();
        FSelection.FOnDeselectStore.Clear();
        FSelection.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.csShared));
        FSelection.UnselectAll();
        FDestroyEventsStore.Raise(this);
        FHSChangedEventsStore.Clear();
        FStaticVisibleHS.QuickClear();
        FDynamicVisibleHS.QuickClear();
        FStaticFixedVisibleHS.QuickClear();
        FDynamicFixedVisibleHS.QuickClear();
        EngineMessage.InitMsg(ref Msg, 536936471, this);
        SendMsg(ref Msg, this);
        FNewFrameEventsStore.Clear();
        base.Dispose();
        FHSChangedEventsStore.Clear();
        FTerrainChangedEventsStore.Clear();
        FViewPortChangedEventsStore.Clear();
        FNewFrameEventsStore.Clear();
        FMouse?.Dispose();
        FDestroyEventsStore.Clear();
        UnlockTerrainChangedEvent();
    }

    // HSMAP.PAS:76
    public override Set<ControlStyleElement> GetControlStyle() => base.GetControlStyle() + Set<ControlStyleElement>.Of(EngineServices.csReserved2);

    // HSMAP.PAS:240
    public override void ReadWrite(EStorageStream Stream)
    {
        sbyte B = default;
        base.ReadWrite(Stream);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smPartial)) == Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smPartial))
        {
            FStatus = FStatus + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.PreviewMode);
        }

        if (!Stream.StorageMode().Contains(EngineServices.smText))
        {
            Stream.rwString(ref FHSSFileName, 2);
        }

        if (!Stream.StorageMode().Contains(EngineServices.smPartial) && Stream.StorageMode().Contains(StorageModeElement.Read) && !IsEmpty(FHSSFileName))
        {
            ((GameEngine)Stream.Engine).InitializeHSSet(Stream.Engine.StartupDirectory + FHSSFileName, Stream.OnProgress);
        }

        B = (sbyte)FVersion;
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref B), 3);
        FVersion = B;
        Stream.rwInteger(ref FViewXhp, 4);
        Stream.rwInteger(ref FViewYhp, 5);
        Stream.rwInteger(ref FViewL, 6);
    }

    // HSMAP.PAS:261
    public override void MainReadWrite(EStorageStream Stream)
    {
        base.MainReadWrite(Stream);
        if (!FStatus.Contains(HexSpriteMapStatusElement.PreviewMode) && (Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText, StorageModeElement.SetupCompare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            FMapContainer.Loaded();
        }
    }

    // HSMAP.PAS:81
    public override void Clear()
    {
        FStaticVisibleHS.QuickClear();
        FDynamicVisibleHS.QuickClear();
        FSelection.QuickClear();
        base.Clear();
        FMapContainer = null;
        ClearViewPort();
    }

    // HSMAP.PAS:189
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        if (Msg.MsgID == 536870920)
        {
            FEngine = Msg.Payload.As<GameEngine>();
        }

        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            FMouse.Reset();
        }
    }

    // HSMAP.PAS:71
    public virtual HexSpriteMapMouse CreateMouse() => new HexSpriteMapMouse();

    // HSMAP.PAS:236
    public virtual void NotifyProgress(in MapProgress Progress)
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
        ref TerrainChangedNotification.TerrainChangedInfo Info = ref notification.Info;
        Info.Rect = R;
        Info.Level = L;
        var mapLevel = FMapContainer.GetMapLevel(L);
        Info.MapLevel = mapLevel;
        FTerrainChangedEventsStore.Raise(notification);
    }

    // HSMAP.PAS:705
    public virtual void DrawStaticHS(ref LockedSurface SD)
    {
        if (FStaticSceneInvalid)
        {
            throw new InvalidOperationException("Static scene is invalid, call UpdateStaticScene!");
        }

        for (int I = FStaticVisibleHS.FCount - 1; I >= 0; I--)
        {
            ((HexSprite)FStaticVisibleHS.FItems[I].Child).Show(FViewXhp, FViewYhp, SD);
        }
    }

    // HSMAP.PAS:725
    public virtual void DrawDynamicHS(ref LockedSurface SD)
    {
        if (FDynamicSceneInvalid)
        {
            throw new InvalidOperationException("Dynamic scene is invalid, call UpdateDynamicScene!");
        }

        for (int I = FDynamicVisibleHS.FCount - 1; I >= 0; I--)
        {
            ((HexSprite)FDynamicVisibleHS.FItems[I].Child).Show(FViewXhp, FViewYhp, SD);
        }
    }

    // HSMAP.PAS:715
    public virtual void DrawStaticHSEx(ref LockedSurface SD)
    {
    }

    // HSMAP.PAS:735
    public virtual void DrawDynamicHSEx(ref LockedSurface SD)
    {
    }

    // HSMAP.PAS:273
    public virtual void FillLevelWithResource(int L, MapObjectResource Resource)
    {
        for (int Y = 0, last = FMapContainer.FHeight - 1; Y <= last; Y++)
        {
            for (int X = 0, lastX = FMapContainer.FWidth - 1; X <= lastX; X++)
            {
                Resource.PlaceResourceUser(this, (sbyte)X, (sbyte)Y, (sbyte)L);
            }
        }
    }

    // HSMAP.PAS:96
    public virtual bool AddMapLevel() => FMapContainer.AddLevel(65794);

    // HSMAP.PAS:101
    public virtual void Activate()
    {
        if (FStatus.Contains(HexSpriteMapStatusElement.Activated))
        {
            throw new InvalidOperationException("HSMap already activated");
        }

        FStatus = FStatus + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Activated);
    }

    // HSMAP.PAS:108
    public virtual void Deactivate()
    {
        if (!FStatus.Contains(HexSpriteMapStatusElement.Activated))
        {
            throw new InvalidOperationException("HSMap wasn't activated");
        }

        FStatus = FStatus - Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Activated);
    }

    // HSMAP.PAS:128
    // { Tests OnCenterView instead of OnViewLevel (original slip). }
    public virtual void ViewLevel(int Lhx)
    {
        if (FOnCenterView != null && Lhx < FMapContainer.FLevelCount)
        {
            FOnViewLevel(Lhx);
        }
    }

    // HSMAP.PAS:115
    public virtual void CenterViewTo(int Xhx, int Yhx, int Lhx)
    {
        if (FOnCenterView != null)
        {
            FOnCenterView(Xhx, Yhx, Lhx);
        }
    }

    // HSMAP.PAS:121
    public virtual void ChangeViewTo(int Xhp, int Yhp, int Lhp)
    {
        if (FOnChangeView != null)
        {
            FOnChangeView(Xhp, Yhp, Lhp);
        }
    }

    // HSMAP.PAS:579
    public virtual void GetViewCenter(ref int Xhx, ref int Yhx, ref int Lhx)
    {
        HexGeometry.HPtoHX(FViewXhp + (int)(((uint)FViewWidth) >> 1), FViewYhp + (int)(((uint)FViewHeight) >> 1), ref Xhx, ref Yhx);
        Lhx = FViewL;
    }

    // HSMAP.PAS:134
    public virtual void MakeVisible(int Xhx, int Yhx, int Lhx, int Radius)
    {
        if (FViewL != Lhx || Xhx - Radius < FViewXhx || Xhx + Radius >= FViewXhx + FViewWidthHX || Yhx - Radius < FViewYhx || Yhx + Radius >= FViewYhx + FViewHeightHX)
        {
            CenterViewTo(Xhx, Yhx, Lhx);
        }
    }

    // HSMAP.PAS:543
    public virtual bool CreateNewMap(NewHexSpriteMapSettings Settings)
    {
        bool Result = default;
        Clear();
        try
        {
            SetHSSFileName(Settings.HSSFileName);
            FEngine.InitializeHSSet(Settings.HSSFileName, null);
            FMapContainer = (MapContainer)FEngine.CreateEObject(Settings.MapContainerClassID, this);
            FEngine.ActivateEObject(FMapContainer);
            FMapContainer.New(Settings);
            Result = true;
            InitializeNewMap();
            Activate();
        }
        catch
        {
            Clear();
            throw;
        }

        return Result;
    }

    // HSMAP.PAS:284
    // { ChangeTerrain, ChangeTerrainEx and ClearTerrain clip the right edge of the changed rectangle
    //   into its Left (original slip). }
    public virtual int ChangeTerrain(EngineObject MO, int X, int Y, int L, int Radius, int TerrainType)
    {
        SByteList List;
        int Count;
        MapField MapF;
        HexNumberWalkData Walk = default;
        Rect R = default;
        MapLevel MapLevel;
        LockTerrainChangedEvent();
        List = new SByteList();
        MapLevel = FMapContainer.GetMapLevel(L);
        Count = 0;
        HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (Walk.Radius <= Radius)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
            {
                MapF = MapLevel.GetField(Walk.X, Walk.Y);
                List[Walk.HN] = MapF.FTerrainType;
                MapF.FTerrainType = (sbyte)TerrainType;
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (Walk.Radius <= Radius)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
            {
                MapF = MapLevel.GetField(Walk.X, Walk.Y);
                if (!MapF.CanChangeTerrainEx(MO))
                {
                    MapF.FTerrainType = List[Walk.HN];
                    List[Walk.HN] = -127;
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 0);
        while (Walk.Radius <= Radius)
        {
            if (List[Walk.HN] != -127 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
            {
                MapLevel.GetField(Walk.X, Walk.Y).FTerrainType = (sbyte)TerrainType;
                MapLevel.GetField(Walk.X, Walk.Y).TerrainChanged();
                Count += 1;
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        UnlockTerrainChangedEvent();
        List?.Dispose();
        if (Count != 0)
        {
            R.Left = X - Radius;
            if (R.Left < 0)
            {
                R.Left = 0;
            }

            R.Right = X + Radius + 1;
            if (R.Right > MapLevel.FWidth)
            {
                R.Left = MapLevel.FWidth;
            }

            R.Top = Y - Radius;
            if (R.Top < 0)
            {
                R.Top = 0;
            }

            R.Bottom = Y + Radius + 1;
            if (R.Bottom > MapLevel.FHeight)
            {
                R.Bottom = MapLevel.FHeight;
            }

            TerrainChanged(R, (sbyte)L);
        }

        return Count;
    }

    // HSMAP.PAS:357
    public virtual int ChangeTerrainEx(EngineObject MO, int X, int Y, int L, int Radius, ChangeTerrainProc ChangeTerrainProc, TerrainChangedProc TerrainChangedProc)
    {
        int Result;
        SByteList OrgList;
        SByteList NewList;
        BitList Changed;
        int Count;
        sbyte TerrainType = default;
        sbyte OverlayType = default;
        MapField MapF;
        HexNumberWalkData Walk = default;
        Rect R = default;
        MapLevel MapLevel;
        if (ChangeTerrainProc != null)
        {
            LockTerrainChangedEvent();
            OrgList = new SByteList();
            NewList = new SByteList();
            Changed = new BitList();
            MapLevel = FMapContainer.GetMapLevel(L);
            Count = 0;
            HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= Radius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    MapF = MapLevel.GetField(Walk.X, Walk.Y);
                    TerrainType = MapF.FTerrainType;
                    OverlayType = MapF.FOverlayType;
                    OrgList[Walk.HN * 2] = TerrainType;
                    OrgList[Walk.HN * 2 + 1] = OverlayType;
                    ChangeTerrainProc(MapF, ref TerrainType, ref OverlayType);
                    MapF.FTerrainType = TerrainType;
                    MapF.FOverlayType = OverlayType;
                    NewList[Walk.HN * 2] = TerrainType;
                    NewList[Walk.HN * 2 + 1] = OverlayType;
                }

                HexNumberWalkData.WalkToNextHN(ref Walk);
            }

            HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= Radius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    MapF = MapLevel.GetField(Walk.X, Walk.Y);
                    if (!MapF.CanChangeTerrainEx(MO))
                    {
                        MapF.FTerrainType = OrgList[Walk.HN * 2];
                        MapF.FOverlayType = OrgList[Walk.HN * 2 + 1];
                        Changed[Walk.HN] = false;
                    }
                    else
                    {
                        Changed[Walk.HN] = true;
                    }
                }

                HexNumberWalkData.WalkToNextHN(ref Walk);
            }

            HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= Radius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    if (Changed[Walk.HN])
                    {
                        var item = NewList[Walk.HN * 2];
                        MapLevel.GetField(Walk.X, Walk.Y).FTerrainType = item;
                        var newListItem = NewList[Walk.HN * 2 + 1];
                        MapLevel.GetField(Walk.X, Walk.Y).FOverlayType = newListItem;
                        MapLevel.GetField(Walk.X, Walk.Y).TerrainChanged();
                        Count += 1;
                    }

                    if (TerrainChangedProc != null)
                    {
                        var item = OrgList[Walk.HN * 2 + 1];
                        var orgListItem = OrgList[Walk.HN * 2];
                        TerrainChangedProc(MapLevel.GetField(Walk.X, Walk.Y), orgListItem, item);
                    }
                }

                HexNumberWalkData.WalkToNextHN(ref Walk);
            }

            UnlockTerrainChangedEvent();
            Changed?.Dispose();
            OrgList?.Dispose();
            NewList?.Dispose();
            if (Count != 0)
            {
                R.Left = X - Radius;
                if (R.Left < 0)
                {
                    R.Left = 0;
                }

                R.Right = X + Radius + 1;
                if (R.Right > MapLevel.FWidth)
                {
                    R.Left = MapLevel.FWidth;
                }

                R.Top = Y - Radius;
                if (R.Top < 0)
                {
                    R.Top = 0;
                }

                R.Bottom = Y + Radius + 1;
                if (R.Bottom > MapLevel.FHeight)
                {
                    R.Bottom = MapLevel.FHeight;
                }

                TerrainChanged(R, (sbyte)L);
            }

            Result = Count;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // HSMAP.PAS:669
    public virtual void UpdateStaticScene()
    {
        int MaxX;
        int MaxY;
        MapLevel MapLevel;
        ResetVisibleStaticHS();
        MaxX = FViewXhx + FViewWidthHX - 1;
        MaxY = FViewYhx + FViewHeightHX - 1;
        MapLevel = FMapContainer.GetMapLevel(FViewL);
        for (int X = FViewXhx; X <= MaxX; X++)
        {
            for (int Y = MaxY, last = FViewYhx; Y >= last; Y--)
            {
                MapLevel.GetField(X, Y).CopyStaticVisibleHS(FStaticVisibleHS);
            }
        }

        for (int X = 0, last = FStaticFixedVisibleHS.FCount - 1; X <= last; X++)
        {
            ((HexSprite)FStaticFixedVisibleHS.Children[X]).MakeVisible(FStaticVisibleHS);
        }

        FStaticVisibleHS.Sort();
        FStaticSceneInvalid = false;
    }

    // HSMAP.PAS:687
    public virtual void UpdateDynamicScene()
    {
        int MaxX;
        int MaxY;
        MapLevel MapLevel;
        ResetVisibleDynamicHS();
        MaxX = FViewXhx + FViewWidthHX - 1;
        MaxY = FViewYhx + FViewHeightHX - 1;
        MapLevel = FMapContainer.GetMapLevel(FViewL);
        for (int X = FViewXhx; X <= MaxX; X++)
        {
            for (int Y = MaxY, last = FViewYhx; Y >= last; Y--)
            {
                MapLevel.GetField(X, Y).CopyDynamicVisibleHS(FDynamicVisibleHS);
            }
        }

        for (int X = 0, last = FDynamicFixedVisibleHS.FCount - 1; X <= last; X++)
        {
            ((HexSprite)FDynamicFixedVisibleHS.Children[X]).MakeVisible(FDynamicVisibleHS);
        }

        FDynamicVisibleHS.Sort();
        FDynamicSceneInvalid = false;
    }

    // HSMAP.PAS:719
    public virtual void DrawStaticScene(ref LockedSurface SD)
    {
        DrawStaticHS(ref SD);
        DrawStaticHSEx(ref SD);
    }

    // HSMAP.PAS:739
    public virtual void DrawDynamicScene(ref LockedSurface SD)
    {
        DrawDynamicHS(ref SD);
        DrawDynamicHSEx(ref SD);
    }

    // HSMAP.PAS:752
    public virtual void NewFrame()
    {
        FFrame += 1;
        if (FFrame > 1048575)
        {
            FFrame = 0;
        }

        FNewFrameEventsStore.Raise(this);
    }

    // HSMAP.PAS:91
    public void RemoveMapLevel(int Index)
    {
        FMapContainer.RemoveLevel(Index);
    }

    // HSMAP.PAS:151
    public void TerrainChanged(in Rect R, sbyte L)
    {
        if (FTerrainChangedLockCounter == 0)
        {
            TriggerTerrainChangedEvent(R, L);
        }
    }

    // HSMAP.PAS:157
    public void TerrainChangedRad(sbyte X, sbyte Y, sbyte L, int Radius)
    {
        MapLevel MapLevel;
        Rect R = default;
        MapLevel = FMapContainer.GetMapLevel(L);
        R.Left = X - Radius;
        if (R.Left < 0)
        {
            R.Left = 0;
        }

        R.Right = X + Radius + 1;
        if (R.Right > MapLevel.FWidth)
        {
            R.Right = MapLevel.FWidth;
        }

        R.Top = Y - Radius;
        if (R.Top < 0)
        {
            R.Top = 0;
        }

        R.Bottom = Y + Radius + 1;
        if (R.Bottom > MapLevel.FHeight)
        {
            R.Bottom = MapLevel.FHeight;
        }

        TerrainChanged(R, L);
    }

    // HSMAP.PAS:178
    public void LockTerrainChangedEvent()
    {
        FTerrainChangedLockCounter += 1;
    }

    // HSMAP.PAS:183
    public void UnlockTerrainChangedEvent()
    {
        if (FTerrainChangedLockCounter == 0)
        {
            HexGeometry.OriginalAssert("Invalid TerrainChangedLockCounter", "D:\\GEngine\\HSENGINE\\inc\\HSMAP.PAS", 5989);
        }

        FTerrainChangedLockCounter -= 1;
    }

    // HSMAP.PAS:198
    public void SetModified()
    {
        FStatus = FStatus + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Modified);
    }

    // HSMAP.PAS:203
    public void ResetModified()
    {
        FStatus = FStatus - Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Modified);
    }

    // HSMAP.PAS:208
    public int GetMaxXhp()
    {
        int X = default;
        int Y = default;
        HexGeometry.HXtoHP(this.FMapContainer.FWidth - 1, this.FMapContainer.FHeight - 1, ref X, ref Y);

        X += 40;
        return X;
    }

    // HSMAP.PAS:218
    public int GetMaxYhp()
    {
        int Y = default;
        int X = default;
        if (this.FMapContainer.FHeight > 1)
        {
            HexGeometry.HXtoHP(1, this.FMapContainer.FHeight - 1, ref X, ref Y);
        }
        else
        {
            HexGeometry.HXtoHP(0, this.FMapContainer.FHeight - 1, ref X, ref Y);
        }

        Y += 32;
        return Y;
    }

    // HSMAP.PAS:231
    public void SetHSSFileName(string FileName)
    {
        var expandFileName = ExpandFileName(FileName);
        FHSSFileName = ExtractRelativePath(FEngine.StartupDirectory, expandFileName);
    }

    // HSMAP.PAS:459
    public void ClearTerrain(int X, int Y, int L, int Radius, int TerrainType)
    {
        HexNumberWalkData Walk = default;
        MapLevel MapLevel;
        MapFieldMessage Msg = default;
        Rect R = default;
        LockTerrainChangedEvent();
        MapLevel = FMapContainer.GetMapLevel(L);
        HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 0);
        Msg.MsgID = 16;
        while (Walk.Radius <= Radius)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
            {
                MapLevel.GetField(Walk.X, Walk.Y).SendMapFieldMsg(ref Msg);
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        if (TerrainType != -1)
        {
            ChangeTerrain(null, X, Y, L, Radius, TerrainType);
        }

        UnlockTerrainChangedEvent();
        R.Left = X - Radius;
        if (R.Left < 0)
        {
            R.Left = 0;
        }

        R.Right = X + Radius + 1;
        if (R.Right > MapLevel.FWidth)
        {
            R.Left = MapLevel.FWidth;
        }

        R.Top = Y - Radius;
        if (R.Top < 0)
        {
            R.Top = 0;
        }

        R.Bottom = Y + Radius + 1;
        if (R.Bottom > MapLevel.FHeight)
        {
            R.Bottom = MapLevel.FHeight;
        }

        TerrainChanged(R, (sbyte)L);
    }

    // HSMAP.PAS:561
    public bool PlaceHX(HexSprite HS, int Xhx, int Yhx, int L)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
        return HS.MainPlace(FMapContainer, Xhp, Yhp, L);
    }

    // HSMAP.PAS:569
    public bool Place(HexSprite HS, int Xhp, int Yhp, int L) => HS.MainPlace(FMapContainer, Xhp, Yhp, L);

    // HSMAP.PAS:574
    public void Remove(HexSprite HS)
    {
        HS.MainRemove();
    }

    // HSMAP.PAS:585
    public bool SetViewPort(int Xhp, int Yhp, int Lhp, int Width, int Height)
    {
        bool Result;
        int X1 = default;
        int Y1 = default;
        int X2 = default;
        int Y2 = default;
        if (Xhp != FViewXhp || Yhp != FViewYhp || Lhp != FViewL || Width != FViewWidth || Height != FViewHeight)
        {
            ClearViewPort();
            FViewL = Lhp;
            FViewXhp = Xhp;
            FViewWidth = Width;
            FViewYhp = Yhp;
            FViewHeight = Height;
            HexGeometry.HPtoHX(FViewXhp - 8, FViewYhp - 16, ref X1, ref Y1);
            HexGeometry.HPtoHX(FViewXhp + Width - 1 + 8, FViewYhp + Height - 1 + 16, ref X2, ref Y2);
            if (X2 >= FMapContainer.FWidth)
            {
                X2 = FMapContainer.FWidth - 1;
            }

            if (Y2 >= FMapContainer.FHeight)
            {
                Y2 = FMapContainer.FHeight - 1;
            }

            if (X1 < 0)
            {
                X1 = 0;
            }

            if (Y1 < 0)
            {
                Y1 = 0;
            }

            FViewWidthHX = X2 - X1 + 1;
            FViewHeightHX = Y2 - Y1 + 1;
            FViewXhx = X1;
            FViewYhx = Y1;
            Result = true;
            FViewPortChangedEventsStore.Raise(this);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // HSMAP.PAS:619
    public void ClearViewPort()
    {
        ResetVisibleStaticHS();
        ResetVisibleDynamicHS();
        FStaticSceneInvalid = true;
        FDynamicSceneInvalid = true;
        FViewXhp = -1;
        FViewYhp = -1;
        FViewL = -1;
        FViewWidth = 0;
        FViewHeight = 0;
    }

    // HSMAP.PAS:633
    // { Viewing accepts one row below the view port, InvalidateStaticScene does not. }
    public bool Viewing(int Xhx, int Yhx, int Lhx) => Xhx >= FViewXhx && Xhx < FViewXhx + FViewWidthHX && Yhx >= FViewYhx && Yhx <= FViewYhx + FViewHeightHX && Lhx == FViewL;

    // HSMAP.PAS:640
    // { The message names ResetVisibleDynamicHS (original slip). }
    public void ResetVisibleStaticHS()
    {
        try
        {
            for (int I = FStaticVisibleHS.FCount - 1; I >= 0; I--)
            {
                ((HexSprite)FStaticVisibleHS.FItems[I].Child).SetVisible(false);
            }

            FStaticVisibleHS.QuickClear();
            FStaticVisibleHS.FSorted = false;
        }
        catch
        {
            throw new InvalidOperationException("Exception during ResetVisibleDynamicHS");
        }
    }

    // HSMAP.PAS:654
    public void ResetVisibleDynamicHS()
    {
        for (int I = FDynamicVisibleHS.FCount - 1; I >= 0; I--)
        {
            try
            {
                ((HexSprite)FDynamicVisibleHS.FItems[I].Child).SetVisible(false);
            }
            catch
            {
                throw new InvalidOperationException("Exception during ResetVisibleDynamicHS (" + FDynamicVisibleHS.FItems[I].Child.GetType().Name + ")");
            }
        }

        FDynamicVisibleHS.QuickClear();
        FDynamicVisibleHS.FSorted = false;
    }

    // HSMAP.PAS:745
    public void InvalidateStaticScene(int Xhx, int Yhx, int Lhx)
    {
        FStaticSceneInvalid = FStaticSceneInvalid || (Xhx >= FViewXhx && Xhx < FViewXhx + FViewWidthHX && Yhx >= FViewYhx && Yhx < FViewYhx + FViewHeightHX && Lhx == FViewL);
    }

    public HexSpriteMapMouse Mouse { get => FMouse; }
    public MapContainer MapContainer { get => FMapContainer; }
    public GameEngine Engine { get => FEngine; }
    public HexSpriteSelection Selection { get => FSelection; }
    public VisibilityList StaticVisibleHS { get => FStaticVisibleHS; }
    public VisibilityList DynamicVisibleHS { get => FDynamicVisibleHS; }
    public VisibilityList StaticFixedVisibleHS { get => FStaticFixedVisibleHS; }
    public VisibilityList DynamicFixedVisibleHS { get => FDynamicFixedVisibleHS; }
    public ETimerNode Timer { get => FTimer; }
    public bool StaticSceneInvalid { get => FStaticSceneInvalid; set => FStaticSceneInvalid = value; }
    public bool DynamicSceneInvalid { get => FDynamicSceneInvalid; set => FDynamicSceneInvalid = value; }
    public int Version { get => FVersion; set => FVersion = value; }
    public string HSSFileName { get => FHSSFileName; set => SetHSSFileName(value); }
    public Set<HexSpriteMapStatusElement> Status { get => FStatus; set => FStatus = value; }
    public int Frame { get => FFrame; }

    public event NotifyEvent<TerrainChangedNotification> TerrainChangedEvents
    {
        add => FTerrainChangedEventsStore.Add(value);
        remove => FTerrainChangedEventsStore.Remove(value);
    }

    public CenterViewEvent OnCenterView { get => FOnCenterView; set => FOnCenterView = value; }
    public ChangeViewEvent OnChangeView { get => FOnChangeView; set => FOnChangeView = value; }
    public ViewLevelEvent OnViewLevel { get => FOnViewLevel; set => FOnViewLevel = value; }
    public Aow.Engine.ProgressEvent OnProgress { get => FOnProgress; set => FOnProgress = value; }

    public event NotifyEvent<HSMap> NewFrameEvents
    {
        add => FNewFrameEventsStore.Add(value);
        remove => FNewFrameEventsStore.Remove(value);
    }

    public event NotifyEvent<HSMap> ViewPortChangedEvents
    {
        add => FViewPortChangedEventsStore.Add(value);
        remove => FViewPortChangedEventsStore.Remove(value);
    }

    public event NotifyEvent<HSMap> DestroyEvents
    {
        add => FDestroyEventsStore.Add(value);
        remove => FDestroyEventsStore.Remove(value);
    }

    public event NotifyEvent<HexSprite> HSChangedEvents
    {
        add => FHSChangedEventsStore.Add(value);
        remove => FHSChangedEventsStore.Remove(value);
    }

    public string FileName { get => FFileName; set => FFileName = value; }
    public int ViewXhp { get => FViewXhp; }
    public int ViewYhp { get => FViewYhp; }
    public int ViewL { get => FViewL; }
    public int ViewWidth { get => FViewWidth; }
    public int ViewHeight { get => FViewHeight; }
    public int ViewXhx { get => FViewXhx; }
    public int ViewYhx { get => FViewYhx; }
    public int ViewWidthHX { get => FViewWidthHX; }
    public int ViewHeightHX { get => FViewHeightHX; }
    public int MaxXhp { get => GetMaxXhp(); }
    public int MaxYhp { get => GetMaxYhp(); }

    public delegate void CenterViewEvent(int Xhx, int Yhx, int Lhx);
    public delegate void ChangeViewEvent(int Xhp, int Yhp, int Lhp);
    public delegate void ViewLevelEvent(int Lhx);

}
