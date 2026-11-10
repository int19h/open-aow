// Translated from HSEngine.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Saves;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map
{

    public enum TTerrainCompareModeElement : byte
    {
        cmSameOverlayType = 0,
        cmSameTerrainType = 1
    }

    public enum TMapFieldStatusElement : byte
    {
        msTerrainLocked = 0,
        msVisible = 1,
        msLoading = 2,
        msComparing = 3
    }

    public enum TMouseStatusElement : byte
    {
        msChangingCapture = 0
    }

    public enum THSMapStatusElement : byte
    {
        msModified = 0,
        msEditting = 1,
        msActivated = 2,
        msDestroying = 3,
        msPreviewMode = 4
    }

    public enum THSSetStatusElement : byte
    {
        ssModified = 0,
        ssEditting = 1,
        ssActivated = 2,
        ssDestroying = 3
    }

    public enum THSEngineStatusElement : byte
    {
        esLoadingHSMSettings = 0,
        esLoadingHSM = 1,
        esSavingHSM = 2,
        esLoadingHSS = 3,
        esSavingHSS = 4
    }

    public enum TControlModeElement : byte
    {
        cmMultiSelect = 0,
        cmShowSelectionSelf = 1
    }
}

namespace Aow.Game.Draw
{

    public enum TLibraryImageHSStatusElement : byte
    {
        lsVisible = 0,
        lsEditMode = 1,
        lsDestroyOnShow = 2,
        lsMapIsOwner = 3,
        lsPlacedOnMap = 4
    }
}

namespace Aow.Game.World
{

    public enum TMapObjectStatusElement : byte
    {
        moVisible = 0,
        moResourceLinked = 1,
        moEditMode = 2,
        moPlacedOnMap = 3,
        moSelected = 4,
        moActivated = 5
    }
}

namespace Aow.Game.World.Movement
{

    public delegate bool TWalkAroundRouteCallBack(int X, int Y, ref bool Cancel);
}

namespace Aow.Game.World.Map
{

    // FHSList, FCapacity, FCount, FStaticCount, FStatus, FXhx, FYhx, FLhx, FReserved, FTerrainType,
    // FOverlayType. A hexagon field of a map level. The original pooled all fields of a level in
    // one block and freed it with the level. The port allocates each field through the base
    // allocator. The sprites are sorted by their show priority. The first FStaticCount (priority
    // shr 26 <= 15) are the static ones
    public partial class TMapField : TECustomNode
    {
        // HSEngine.pas:1933
        public new const int ClassID = 65793;

        public TMapField(TECustomNode owner) : base(owner)
        {
            FTerrainType = -1;
            FOverlayType = -1;
            FStatus = TSet<TMapFieldStatusElement>.Of();
            Resize(3);
        }

        internal THexagonSprite[] FHSList = [];
        internal byte FCapacity;
        internal byte FCount;
        internal byte FStaticCount;
        internal TSet<TMapFieldStatusElement> FStatus;
        internal sbyte FXhx;
        internal sbyte FYhx;
        internal sbyte FLhx;
        internal sbyte FTerrainType;
        internal sbyte FOverlayType;

        // HSEngine.pas:1907
        public override void Dispose()
        {
            Array.Resize(ref FHSList, 0);
            FCount = 0;
        }

        // HSEngine.pas:1928
        public override TSet<TEControlStyleElement> GetControlStyle() => TSet<TEControlStyleElement>.Of(Aow.Game.Engine.Nodes.EngineServices.csStored, Aow.Game.Engine.Nodes.EngineServices.csShared);

        // HSEngine.pas:1923
        public override void SetOwner(TECustomNode AOwner)
        {
            FOwner = AOwner;
        }

        // HSEngine.pas:2029
        public override void ReadWrite(TEStorageStream Stream)
        {
            int J;
            int I;
            TEObject HS = default;
            if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(Aow.Game.Engine.Nodes.EngineServices.smText)) == TSet<TStorageModeElement>.Of())
            {
                if (Stream.StorageMode().Contains(TStorageModeElement.smRead))
                {
                    if (Stream.StorageMode().Contains(TStorageModeElement.smCompare))
                    {
                        J = 0;
                        I = Stream.PropertyTable.First;
                        int last = Stream.PropertyTable.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            if (Stream.PropertyTable.Items[I].ID >= 50)
                            {
                                HS = FHSList[J];
                                Stream.rwCreateEObject(ref HS, this, Stream.PropertyTable.Items[I].ID);
                                J += 1;
                            }
                        }
                    }
                    else
                    {
                        I = Stream.PropertyTable.First;
                        int last = Stream.PropertyTable.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            J = Stream.PropertyTable.Items[I].ID;
                            if (J >= 50)
                            {
                                Stream.rwCreateEObject(ref HS, this, J);
                            }
                        }
                    }
                }
                else
                {
                    I = 50;
                    J = 0;
                    int last = FCount - 1;
                    for (; J <= last; J += 1)
                    {
                        HS = FHSList[J];
                        if (HS != null && HS.Owner == this && !HS.GetControlStyle().Contains(Aow.Game.Engine.Nodes.EngineServices.csShared) && HS.GetControlStyle().Contains(Aow.Game.Engine.Nodes.EngineServices.csStored))
                        {
                            Stream.rwCreateEObject(ref HS, this, I);
                            I += 1;
                        }
                    }
                }
            }
            else
            {
                ReadWriteChildren(Stream);
            }
        }

        // HSEngine.pas:2075
        public override void MainReadWrite(TEStorageStream Stream)
        {
            ReadWrite(Stream);
        }

        // HSEngine.pas:1898

        // HSEngine.pas:1943
        public override TEObject GetChild(int Index)
        {
            if (Index >= FCount)
            {
                throw new IndexOutOfRangeException("Index out of bounds");
            }

            return FHSList[Index];
        }

        // HSEngine.pas:1949
        public override void SetChild(int Index, TEObject Child)
        {
            if (Index >= FCount)
            {
                throw new IndexOutOfRangeException("Index out of bounds");
            }

            FHSList[Index] = (THexagonSprite)Child;
        }

        // HSEngine.pas:1972
        public override int GetCount() => FCount;

        // HSEngine.pas:2226
        public override int RemoveChild(TEObject Child) => RemoveHS((THexagonSprite)Child);

        // HSEngine.pas:2221
        public override int AddChild(TEObject Child) => AddHS((THexagonSprite)Child, 0);

        // HSEngine.pas:2154
        public override TEObject FindChild(int AClassID)
        {
            TEObject Result;
            int I;
            I = 0;
            while (I < FCount && FHSList[I].EClass.ClassID != AClassID)
            {
                I += 1;
            }

            if (I < FCount)
            {
                Result = FHSList[I];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // HSEngine.pas:2211
        public override TEObject FindOwnedChild(int AClassID)
        {
            TEObject Result;
            int I;
            I = 0;
            while (I < FCount && !(FHSList[I].EClass.ClassID == AClassID && FHSList[I].Owner == this))
            {
                I += 1;
            }

            if (I < FCount)
            {
                Result = FHSList[I];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // HSEngine.pas:1938
        public int GetXYL() => FXhx + (FYhx << 8) + (FLhx << 16);

        // HSEngine.pas:1955
        public int HSListDataSize() => (FCapacity << 2) + ((FCapacity + 3) & 252);

        // HSEngine.pas:2163
        public THexagonSprite FindHS(Type AClass)
        {
            THexagonSprite Result;
            int I;
            I = 0;
            while (I < FCount && !AClass.IsInstanceOfType(FHSList[I]))
            {
                I += 1;
            }

            if (I < FCount)
            {
                Result = FHSList[I];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // HSEngine.pas:2172
        public THexagonSprite FindOwnedHS(Type AClass)
        {
            int I;
            THexagonSprite HS;
            I = 0;
            THexagonSprite Result = null;
            while (I < FCount && Result == null)
            {
                HS = FHSList[I];
                if (AClass.IsInstanceOfType(HS) && HS.Owner == this)
                {
                    Result = HS;
                }

                I += 1;
            }

            return Result;
        }

        // HSEngine.pas:2300
        public void CopyDynamicVisibleHS(TVisibilityList List)
        {
            byte last = FStaticCount;
            for (int I = FCount - 1; I >= last; I--)
            {
                FHSList[I].MakeVisible(List);
            }
        }

        // HSEngine.pas:2308
        public void CopyStaticVisibleHS(TVisibilityList List)
        {
            for (int I = FStaticCount - 1; I >= 0; I--)
            {
                FHSList[I].MakeVisible(List);
            }
        }

        // HSEngine.pas:2316
        public void ResetDynamicVisibleHS()
        {
            byte last = FStaticCount;
            for (int I = FCount - 1; I >= last; I--)
            {
                FHSList[I].SetVisible(false);
            }
        }

        // HSEngine.pas:2324
        public void ResetStaticVisibleHS()
        {
            for (int I = FStaticCount - 1; I >= 0; I--)
            {
                FHSList[I].SetVisible(false);
            }
        }

        // HSEngine.pas:2335
        // { The sprites with a level from MinLevel to MaxLevel that a map field owns are destroyed; the list
        //   is sorted, so the walk stops at the first sprite below MinLevel. The HP position is computed and
        //   never used. }
        public void DestroyLevelHS(int MinLevel, int MaxLevel)
        {
            int Xhp = default;
            int Yhp = default;
            bool Done;
            int I;
            int Level;
            Aow.Game.World.Map.HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            I = FCount - 1;
            // { Preserve the original stack slot. }
            Done = false;
            while (!Done && I >= 0)
            {
                Level = FHSList[I].GetLevel();
                if (Level <= MaxLevel)
                {
                    if (Level >= MinLevel)
                    {
                        if (FHSList[I].Owner is TMapField)
                        {
                            FHSList[I].Dispose();
                        }
                    }
                    else
                    {
                        Done = true;
                    }
                }

                I -= 1;
                if (I >= FCount)
                {
                    I = FCount - 1;
                }
            }
        }

        // HSEngine.pas:2360
        public void DestroyHS()
        {
            for (int I = FCount - 1; I >= 0; I--)
            {
                if (I <= FCount - 1)
                {
                    if (FHSList[I].Owner == this)
                    {
                        FHSList[I].Dispose();
                    }
                }
            }
        }

        // HSEngine.pas:2369
        public void Activate()
        {
            for (int I = FCount - 1; I >= 0; I--)
            {
                if (FHSList[I].Owner == this)
                {
                    FHSList[I].Activate();
                }
            }
        }

        // HSEngine.pas:2581
        public sbyte GetDecorationType()
        {
            TMapFieldMsg Msg = default;
            Msg.MsgID = 17;
            Msg.Result = -1;
            SendMapFieldMsg(ref Msg);
            return (sbyte)Msg.Result.Int;
        }

        // HSEngine.pas:2591
        public void UpdateNeighbourTerrain()
        {
            THSTerrainInfo Info = default;
            THNWalkData Walk = default;
            Info.MapField = this;
            Info.TerrainType = FTerrainType;
            Info.OverlayType = FOverlayType;
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            for (int I = 1; I <= 6; I++)
            {
                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
                if (Walk.X >= 0 && Walk.X < ((TMapLevel)Owner).FWidth && Walk.Y >= 0 && ((TMapLevel)Owner).FHeight > Walk.Y)
                {
                    Info.HN = (sbyte)Aow.Game.World.Map.TMapField.OppositeHTTable[I - 1];
                    ((TMapLevel)Owner).GetField(Walk.X, Walk.Y).NeighbourTerrainChanged(ref Info);
                }
            }
        }

        public THexagonSprite[] HSList { get => FHSList; }
        public byte Capacity { get => FCapacity; }
        public byte StaticCount { get => FStaticCount; }
        public TSet<TMapFieldStatusElement> Status { get => FStatus; set => FStatus = value; }
        public sbyte Xhx { get => FXhx; set => FXhx = value; }
        public sbyte Yhx { get => FYhx; set => FYhx = value; }
        public sbyte Lhx { get => FLhx; set => FLhx = value; }
        public int XYL { get => GetXYL(); }
        public sbyte TerrainType { get => FTerrainType; set => FTerrainType = value; }
        public sbyte OverlayType { get => FOverlayType; set => FOverlayType = value; }

        public sealed class HNAccessor(TMapField target)
        {
            public byte this[int Index] { get => target.GetHN(Index); set => target.SetHN(Index, value); }
        }

        public HNAccessor HN => new HNAccessor(this);

        public override void SendMsg(ref TEMsg message, TEObject sender)
        {
            if ((message.MsgID & 0x10000000) != 0)
            {
                for (int i = FCount - 1; i >= 0; i--)
                {
                    var sprite = FHSList[i];
                    if (sprite.GetLastMsg() == message.Seq) continue;
                    if ((message.MsgID & 0x20000000) != 0 && sprite.Owner != this) continue;
                    sprite.SetLastMsg(message.Seq);
                    sprite.MsgProc(ref message, this);
                }
            }
            else if ((message.MsgID & 0x20000000) == 0)
            {
                for (int i = Count - 1; i >= 0; i--) FHSList[i].MsgProc(ref message, this);
            }
            else
            {
                int count = FCount;
                for (int i = 0; i < count; i++) Aow.Game.World.Map.TMapField.SendMsgList[i] = FHSList[i];
                for (int i = count - 1; i >= 0; i--)
                    if (Aow.Game.World.Map.TMapField.SendMsgList[i].Owner == this) Aow.Game.World.Map.TMapField.SendMsgList[i].MsgProc(ref message, this);
            }
        }

        private static global::Aow.Game.Engine.Buffers.FixedArray_byte_6 OppositeHTTable = global::Aow.Game.Engine.Buffers.FixedArray_byte_6.From((global::System.ReadOnlySpan<byte>)new byte[6] { 4, 5, 6, 1, 2, 3 });
        private static global::Aow.Game.Engine.Buffers.FixedArray_Aow_Units_HSEngine_THexagonSprite_256 SendMsgList;
        private static global::Aow.Game.Engine.Buffers.FixedArray_Aow_Units_HSEngine_THexagonSprite_256 LoadedList;
    }

    // FWidth, FHeight, FLevelCount, FMap, FEngine. The levels of a map (the children, TMapLevel)
    public partial class TMapContainer : TENode
    {
        // HSEngine.pas:3208
        public new const int ClassID = 65795;

        internal int FWidth;
        internal int FHeight;
        internal int FLevelCount;
        internal THSMap FMap;
        internal TEngine FEngine;

        // HSEngine.pas:3167
        public override void SetOwner(TECustomNode AOwner)
        {
            base.SetOwner(AOwner);
            FMap = (THSMap)AOwner;
        }

        // HSEngine.pas:3203
        public override TEngine GetEngine() => FEngine;

        // HSEngine.pas:3173
        public override void ReadWrite(TEStorageStream Stream)
        {
            byte W;
            byte H;
            byte L;
            base.ReadWrite(Stream);
            if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, Aow.Game.Engine.Nodes.EngineServices.smText)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead))
            {
                FMap = (THSMap)Owner;
                FEngine = Stream.Engine;
                FMap.FMapContainer = this;
            }

            W = (byte)FWidth;
            Stream.rwByte(ref W, 3);
            FWidth = W;
            H = (byte)FHeight;
            Stream.rwByte(ref H, 4);
            FHeight = H;
            L = (byte)FLevelCount;
            Stream.rwByte(ref L, 5);
            FLevelCount = L;
        }

        // HSEngine.pas:3197
        public override void MainReadWrite(TEStorageStream Stream)
        {
            if (!((THSMap)Owner).FStatus.Contains(THSMapStatusElement.msPreviewMode))
            {
                base.MainReadWrite(Stream);
            }
            else
            {
                ReadWrite(Stream);
            }
        }

        // HSEngine.pas:3162
        public TMapContainer(TECustomNode owner) : base(owner)
        {
        }

        // HSEngine.pas:3241
        public override void Clear()
        {
            base.Clear();
            FWidth = 0;
            FHeight = 0;
            FLevelCount = 0;
        }

        // HSEngine.pas:3213
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            if (Msg.MsgID == 536870920)
            {
                FEngine = Msg.Payload.As<TEngine>();
            }

            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 536870920)
            {
                UpdateTerrain();
                Activate();
            }
        }

        // HSEngine.pas:3249
        public virtual void RemoveLevel(int Index)
        {
            if (Index >= 0 && Index < FChildren.Count)
            {
                ((TMapLevel)FChildren.List[Index]).DestroyHS();
                ((TMapLevel)FChildren.List[Index])?.Dispose();
                FLevelCount -= 1;
            }
        }

        // HSEngine.pas:3259
        public virtual bool AddLevel(int AClassID)
        {
            bool Result;
            EClass LevelClass;
            TMapLevel Level;
            LevelClass = FEngine.GetEClass(AClassID);
            if (LevelClass != null)
            {
                Level = (TMapLevel)LevelClass.Create(this);
                Level.FLhx = FLevelCount;
                FEngine.ActivateEObject(Level);
                Level.CreateMapFieldList(FWidth, FHeight);
                FLevelCount += 1;
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // HSEngine.pas:3277
        public virtual void New(TNewHSMapSettings Settings)
        {
            EClass LevelClass;
            TMapLevel Level;
            Clear();
            try
            {
                LevelClass = FEngine.GetEClass(Settings.MapLevelClassID);
                for (int I = 0, last = Settings.Levels - 1; I <= last; I++)
                {
                    Level = (TMapLevel)LevelClass.Create(this);
                    Level.FLhx = I;
                    FEngine.ActivateEObject(Level);
                    Level.New(Settings);
                }

                FWidth = Settings.Width;
                FHeight = Settings.Height;
                FLevelCount = Settings.Levels;
            }
            catch
            {
                Clear();
                throw;
            }
        }

        // HSEngine.pas:3323
        public virtual void UpdateTerrain()
        {
            for (int I = 0, last = FLevelCount - 1; I <= last; I++)
            {
                ((TMapLevel)FChildren.List[I]).UpdateTerrain();
            }
        }

        // HSEngine.pas:3224
        public TMapLevel GetMapLevel(int Index) => (TMapLevel)FChildren.List[Index];

        // HSEngine.pas:3331
        public void Loaded()
        {
            for (int I = 0, last = FLevelCount - 1; I <= last; I++)
            {
                ((TMapLevel)FChildren.List[I]).Loaded();
            }
        }

        // HSEngine.pas:3339
        public void Activate()
        {
            for (int I = 0, last = FLevelCount - 1; I <= last; I++)
            {
                ((TMapLevel)FChildren.List[I]).Activate();
            }
        }

        public int Width { get => FWidth; }
        public int Height { get => FHeight; }
        public int LevelCount { get => FLevelCount; }
        public THSMap Map { get => FMap; }

        public sealed class MapLevelsAccessor(TMapContainer target)
        {
            public TMapLevel this[int Index] { get => target.GetMapLevel(Index); }
        }

        public MapLevelsAccessor MapLevels => new MapLevelsAccessor(this);

    }

    // the sorted list of the visible sprites of a scene (THSMap). a sprite removed from it is made
    // invisible
    public partial class TVisibilityList : TEControlStyleInverseSortNode
    {
        public TVisibilityList(TECustomNode owner) : base(owner)
        { }

        // HSEngine.pas:3871
        public override void QuickClear()
        {
            FCount = 0;
        }

        // HSEngine.pas:3876
        public override int RemoveChild(TEObject Child)
        {
            var hexagonSprite = (THexagonSprite)Child;
            if (hexagonSprite.GetVisible())
            {
                hexagonSprite.SetVisible(false);
            }

            return base.RemoveChild(Child);
        }

        // HSEngine.pas:3866
        public override void Grow()
        {
            Resize(FCapacity + 500);
        }

    }

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
    public partial class THSMap : TENode
    {
        public new const int ClassID = 66048;

        public THSMap(TECustomNode owner) : base(owner)
        {
            FMouse = CreateMouse();
            FMouse.FHSMap = this;
            FSelection = new THexagonSpriteSelection(this);
            FSelection.SetControlStyle(TSet<TEControlStyleElement>.Of(Aow.Game.Engine.Nodes.EngineServices.csShared));
            FStaticVisibleHS = new TVisibilityList(this);
            FStaticVisibleHS.SetControlStyle(TSet<TEControlStyleElement>.Of(Aow.Game.Engine.Nodes.EngineServices.csShared));
            FDynamicVisibleHS = new TVisibilityList(this);
            FDynamicVisibleHS.SetControlStyle(TSet<TEControlStyleElement>.Of(Aow.Game.Engine.Nodes.EngineServices.csShared));
            FStaticFixedVisibleHS = new TVisibilityList(this);
            FStaticFixedVisibleHS.SetControlStyle(TSet<TEControlStyleElement>.Of(Aow.Game.Engine.Nodes.EngineServices.csShared));
            FDynamicFixedVisibleHS = new TVisibilityList(this);
            FDynamicFixedVisibleHS.SetControlStyle(TSet<TEControlStyleElement>.Of(Aow.Game.Engine.Nodes.EngineServices.csShared));
            FTimer = new TETimerNode(this);
            FHSChangedEventsStore = new();
            FTerrainChangedEventsStore = new();
            FViewPortChangedEventsStore = new();
            FNewFrameEventsStore = new();
            FDestroyEventsStore = new();
        }

        internal THSMapMouse FMouse;
        internal TMapContainer FMapContainer;
        internal THSEngine FEngine;
        internal THexagonSpriteSelection FSelection;
        internal TVisibilityList FStaticVisibleHS;
        internal TVisibilityList FDynamicVisibleHS;
        internal TVisibilityList FStaticFixedVisibleHS;
        internal TVisibilityList FDynamicFixedVisibleHS;
        internal TETimerNode FTimer;
        internal bool FStaticSceneInvalid;
        internal bool FDynamicSceneInvalid;
        internal int FVersion;
        internal string FHSSFileName = "";
        internal TSet<THSMapStatusElement> FStatus;
        internal int FFrame;

        internal EventStore<TerrainChangedNotification> FTerrainChangedEventsStore;

        internal event TNotifyEvent<TerrainChangedNotification> FTerrainChangedEvents
        {
            add => FTerrainChangedEventsStore.Add(value);
            remove => FTerrainChangedEventsStore.Remove(value);
        }

        internal TCenterViewEvent FOnCenterView;
        internal TChangeViewEvent FOnChangeView;
        internal TViewLevelEvent FOnViewLevel;
        internal Aow.Game.Engine.TProgressEvent FOnProgress;
        internal int FTerrainChangedLockCounter;

        internal EventStore<THSMap> FNewFrameEventsStore;

        internal event TNotifyEvent<THSMap> FNewFrameEvents
        {
            add => FNewFrameEventsStore.Add(value);
            remove => FNewFrameEventsStore.Remove(value);
        }

        internal EventStore<THSMap> FViewPortChangedEventsStore;

        internal event TNotifyEvent<THSMap> FViewPortChangedEvents
        {
            add => FViewPortChangedEventsStore.Add(value);
            remove => FViewPortChangedEventsStore.Remove(value);
        }

        internal EventStore<THSMap> FDestroyEventsStore;

        internal event TNotifyEvent<THSMap> FDestroyEvents
        {
            add => FDestroyEventsStore.Add(value);
            remove => FDestroyEventsStore.Remove(value);
        }

        internal EventStore<THexagonSprite> FHSChangedEventsStore;

        internal event TNotifyEvent<THexagonSprite> FHSChangedEvents
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
            TEMsg Msg = default;
            LockTerrainChangedEvent();
            FStatus = FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msDestroying);
            FSelection.FOnSelectStore.Clear();
            FSelection.FOnDeselectStore.Clear();
            FSelection.SetControlStyle(TSet<TEControlStyleElement>.Of(Aow.Game.Engine.Nodes.EngineServices.csShared));
            FSelection.UnselectAll();
            FDestroyEventsStore.Raise(this);
            FHSChangedEventsStore.Clear();
            FStaticVisibleHS.QuickClear();
            FDynamicVisibleHS.QuickClear();
            FStaticFixedVisibleHS.QuickClear();
            FDynamicFixedVisibleHS.QuickClear();
            Aow.Game.Engine.Nodes.TEMsg.InitMsg(ref Msg, 536936471, this);
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
        public override TSet<TEControlStyleElement> GetControlStyle() => base.GetControlStyle() + TSet<TEControlStyleElement>.Of(Aow.Game.Engine.Nodes.EngineServices.csReserved2);

        // HSMAP.PAS:240
        public override void ReadWrite(TEStorageStream Stream)
        {
            sbyte B = default;
            base.ReadWrite(Stream);
            if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, Aow.Game.Engine.Nodes.EngineServices.smPartial)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, Aow.Game.Engine.Nodes.EngineServices.smPartial))
            {
                FStatus = FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msPreviewMode);
            }

            if (!Stream.StorageMode().Contains(Aow.Game.Engine.Nodes.EngineServices.smText))
            {
                Stream.rwString(ref FHSSFileName, 2);
            }

            if (!Stream.StorageMode().Contains(Aow.Game.Engine.Nodes.EngineServices.smPartial) && Stream.StorageMode().Contains(TStorageModeElement.smRead) && !IsEmpty(FHSSFileName))
            {
                ((THSEngine)Stream.Engine).InitializeHSSet(Stream.Engine.StartupDirectory + FHSSFileName, Stream.OnProgress);
            }

            B = (sbyte)FVersion;
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref B), 3);
            FVersion = B;
            Stream.rwInteger(ref FViewXhp, 4);
            Stream.rwInteger(ref FViewYhp, 5);
            Stream.rwInteger(ref FViewL, 6);
        }

        // HSMAP.PAS:261
        public override void MainReadWrite(TEStorageStream Stream)
        {
            base.MainReadWrite(Stream);
            if (!FStatus.Contains(THSMapStatusElement.msPreviewMode) && (Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, Aow.Game.Engine.Nodes.EngineServices.smText, TStorageModeElement.smSetupCompare)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead))
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
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            if (Msg.MsgID == 536870920)
            {
                FEngine = (THSEngine)Msg.Payload.As<TEngine>();
            }

            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 536870920)
            {
                FMouse.Reset();
            }
        }

        // HSMAP.PAS:71
        public virtual THSMapMouse CreateMouse() => new THSMapMouse();

        // HSMAP.PAS:236
        public virtual void NotifyProgress(in TMapProgress Progress)
        {
        }

        // HSMAP.PAS:269
        public virtual void InitializeNewMap()
        {
        }

        // HSMAP.PAS:141
        public virtual void TriggerTerrainChangedEvent(in TRect R, sbyte L)
        {
            var notification = new TerrainChangedNotification();
            ref var Info = ref notification.Info;
            Info.Rect = R;
            Info.Level = L;
            var mapLevel = FMapContainer.GetMapLevel(L);
            Info.MapLevel = mapLevel;
            FTerrainChangedEventsStore.Raise(notification);
        }

        // HSMAP.PAS:705
        public virtual void DrawStaticHS(ref TSurfaceDesc SD)
        {
            if (FStaticSceneInvalid)
            {
                throw new InvalidOperationException("Static scene is invalid, call UpdateStaticScene!");
            }

            for (int I = FStaticVisibleHS.FCount - 1; I >= 0; I--)
            {
                ((THexagonSprite)FStaticVisibleHS.FItems[I].Child).Show(FViewXhp, FViewYhp, SD);
            }
        }

        // HSMAP.PAS:725
        public virtual void DrawDynamicHS(ref TSurfaceDesc SD)
        {
            if (FDynamicSceneInvalid)
            {
                throw new InvalidOperationException("Dynamic scene is invalid, call UpdateDynamicScene!");
            }

            for (int I = FDynamicVisibleHS.FCount - 1; I >= 0; I--)
            {
                ((THexagonSprite)FDynamicVisibleHS.FItems[I].Child).Show(FViewXhp, FViewYhp, SD);
            }
        }

        // HSMAP.PAS:715
        public virtual void DrawStaticHSEx(ref TSurfaceDesc SD)
        {
        }

        // HSMAP.PAS:735
        public virtual void DrawDynamicHSEx(ref TSurfaceDesc SD)
        {
        }

        // HSMAP.PAS:273
        public virtual void FillLevelWithResource(int L, Aow.Game.World.TMapObjectResource Resource)
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
            if (FStatus.Contains(THSMapStatusElement.msActivated))
            {
                throw new InvalidOperationException("HSMap already activated");
            }

            FStatus = FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msActivated);
        }

        // HSMAP.PAS:108
        public virtual void Deactivate()
        {
            if (!FStatus.Contains(THSMapStatusElement.msActivated))
            {
                throw new InvalidOperationException("HSMap wasn't activated");
            }

            FStatus = FStatus - TSet<THSMapStatusElement>.Of(THSMapStatusElement.msActivated);
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
            Aow.Game.World.Map.HexGeometry.HPtoHX(FViewXhp + (int)(((uint)FViewWidth) >> 1), FViewYhp + (int)(((uint)FViewHeight) >> 1), ref Xhx, ref Yhx);
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
        public virtual bool CreateNewMap(TNewHSMapSettings Settings)
        {
            bool Result = default;
            Clear();
            try
            {
                SetHSSFileName(Settings.HSSFileName);
                FEngine.InitializeHSSet(Settings.HSSFileName, null);
                FMapContainer = (TMapContainer)FEngine.CreateEObject(Settings.MapContainerClassID, this);
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
        public virtual int ChangeTerrain(TEObject MO, int X, int Y, int L, int Radius, int TerrainType)
        {
            TShortIntList List;
            int Count;
            TMapField MapF;
            THNWalkData Walk = default;
            TRect R = default;
            TMapLevel MapLevel;
            LockTerrainChangedEvent();
            List = new TShortIntList();
            MapLevel = FMapContainer.GetMapLevel(L);
            Count = 0;
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= Radius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    MapF = MapLevel.GetField(Walk.X, Walk.Y);
                    List[Walk.HN] = MapF.FTerrainType;
                    MapF.FTerrainType = (sbyte)TerrainType;
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }

            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, X, Y, 0);
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

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }

            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= Radius)
            {
                if (List[Walk.HN] != -127 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    MapLevel.GetField(Walk.X, Walk.Y).FTerrainType = (sbyte)TerrainType;
                    MapLevel.GetField(Walk.X, Walk.Y).TerrainChanged();
                    Count += 1;
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
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
        public virtual int ChangeTerrainEx(TEObject MO, int X, int Y, int L, int Radius, TChangeTerrainProc ChangeTerrainProc, TTerrainChangedProc TerrainChangedProc)
        {
            int Result;
            TShortIntList OrgList;
            TShortIntList NewList;
            TBitList Changed;
            int Count;
            sbyte TerrainType = default;
            sbyte OverlayType = default;
            TMapField MapF;
            THNWalkData Walk = default;
            TRect R = default;
            TMapLevel MapLevel;
            if (ChangeTerrainProc != null)
            {
                LockTerrainChangedEvent();
                OrgList = new TShortIntList();
                NewList = new TShortIntList();
                Changed = new TBitList();
                MapLevel = FMapContainer.GetMapLevel(L);
                Count = 0;
                Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, X, Y, 0);
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

                    Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
                }

                Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, X, Y, 0);
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

                    Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
                }

                Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, X, Y, 0);
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

                    Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
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
            TMapLevel MapLevel;
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
                ((THexagonSprite)FStaticFixedVisibleHS.Children[X]).MakeVisible(FStaticVisibleHS);
            }

            FStaticVisibleHS.Sort();
            FStaticSceneInvalid = false;
        }

        // HSMAP.PAS:687
        public virtual void UpdateDynamicScene()
        {
            int MaxX;
            int MaxY;
            TMapLevel MapLevel;
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
                ((THexagonSprite)FDynamicFixedVisibleHS.Children[X]).MakeVisible(FDynamicVisibleHS);
            }

            FDynamicVisibleHS.Sort();
            FDynamicSceneInvalid = false;
        }

        // HSMAP.PAS:719
        public virtual void DrawStaticScene(ref TSurfaceDesc SD)
        {
            DrawStaticHS(ref SD);
            DrawStaticHSEx(ref SD);
        }

        // HSMAP.PAS:739
        public virtual void DrawDynamicScene(ref TSurfaceDesc SD)
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
        public void TerrainChanged(in TRect R, sbyte L)
        {
            if (FTerrainChangedLockCounter == 0)
            {
                TriggerTerrainChangedEvent(R, L);
            }
        }

        // HSMAP.PAS:157
        public void TerrainChangedRad(sbyte X, sbyte Y, sbyte L, int Radius)
        {
            TMapLevel MapLevel;
            TRect R = default;
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
                Aow.Game.World.Map.HexGeometry.OriginalAssert("Invalid TerrainChangedLockCounter", "D:\\GEngine\\HSENGINE\\inc\\HSMAP.PAS", 5989);
            }

            FTerrainChangedLockCounter -= 1;
        }

        // HSMAP.PAS:198
        public void SetModified()
        {
            FStatus = FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msModified);
        }

        // HSMAP.PAS:203
        public void ResetModified()
        {
            FStatus = FStatus - TSet<THSMapStatusElement>.Of(THSMapStatusElement.msModified);
        }

        // HSMAP.PAS:208
        public int GetMaxXhp()
        {
            int X = default;
            int Y = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(this.FMapContainer.FWidth - 1, this.FMapContainer.FHeight - 1, ref X, ref Y);

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
                Aow.Game.World.Map.HexGeometry.HXtoHP(1, this.FMapContainer.FHeight - 1, ref X, ref Y);
            }
            else
            {
                Aow.Game.World.Map.HexGeometry.HXtoHP(0, this.FMapContainer.FHeight - 1, ref X, ref Y);
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
            THNWalkData Walk = default;
            TMapLevel MapLevel;
            TMapFieldMsg Msg = default;
            TRect R = default;
            LockTerrainChangedEvent();
            MapLevel = FMapContainer.GetMapLevel(L);
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, X, Y, 0);
            Msg.MsgID = 16;
            while (Walk.Radius <= Radius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    MapLevel.GetField(Walk.X, Walk.Y).SendMapFieldMsg(ref Msg);
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
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
        public bool PlaceHX(THexagonSprite HS, int Xhx, int Yhx, int L)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            return HS.MainPlace(FMapContainer, Xhp, Yhp, L);
        }

        // HSMAP.PAS:569
        public bool Place(THexagonSprite HS, int Xhp, int Yhp, int L) => HS.MainPlace(FMapContainer, Xhp, Yhp, L);

        // HSMAP.PAS:574
        public void Remove(THexagonSprite HS)
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
                Aow.Game.World.Map.HexGeometry.HPtoHX(FViewXhp - 8, FViewYhp - 16, ref X1, ref Y1);
                Aow.Game.World.Map.HexGeometry.HPtoHX(FViewXhp + Width - 1 + 8, FViewYhp + Height - 1 + 16, ref X2, ref Y2);
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
                    ((THexagonSprite)FStaticVisibleHS.FItems[I].Child).SetVisible(false);
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
                    ((THexagonSprite)FDynamicVisibleHS.FItems[I].Child).SetVisible(false);
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

        public THSMapMouse Mouse { get => FMouse; }
        public TMapContainer MapContainer { get => FMapContainer; }
        public THSEngine Engine { get => FEngine; }
        public THexagonSpriteSelection Selection { get => FSelection; }
        public TVisibilityList StaticVisibleHS { get => FStaticVisibleHS; }
        public TVisibilityList DynamicVisibleHS { get => FDynamicVisibleHS; }
        public TVisibilityList StaticFixedVisibleHS { get => FStaticFixedVisibleHS; }
        public TVisibilityList DynamicFixedVisibleHS { get => FDynamicFixedVisibleHS; }
        public TETimerNode Timer { get => FTimer; }
        public bool StaticSceneInvalid { get => FStaticSceneInvalid; set => FStaticSceneInvalid = value; }
        public bool DynamicSceneInvalid { get => FDynamicSceneInvalid; set => FDynamicSceneInvalid = value; }
        public int Version { get => FVersion; set => FVersion = value; }
        public string HSSFileName { get => FHSSFileName; set => SetHSSFileName(value); }
        public TSet<THSMapStatusElement> Status { get => FStatus; set => FStatus = value; }
        public int Frame { get => FFrame; }

        public event TNotifyEvent<TerrainChangedNotification> TerrainChangedEvents
        {
            add => FTerrainChangedEventsStore.Add(value);
            remove => FTerrainChangedEventsStore.Remove(value);
        }

        public TCenterViewEvent OnCenterView { get => FOnCenterView; set => FOnCenterView = value; }
        public TChangeViewEvent OnChangeView { get => FOnChangeView; set => FOnChangeView = value; }
        public TViewLevelEvent OnViewLevel { get => FOnViewLevel; set => FOnViewLevel = value; }
        public Aow.Game.Engine.TProgressEvent OnProgress { get => FOnProgress; set => FOnProgress = value; }

        public event TNotifyEvent<THSMap> NewFrameEvents
        {
            add => FNewFrameEventsStore.Add(value);
            remove => FNewFrameEventsStore.Remove(value);
        }

        public event TNotifyEvent<THSMap> ViewPortChangedEvents
        {
            add => FViewPortChangedEventsStore.Add(value);
            remove => FViewPortChangedEventsStore.Remove(value);
        }

        public event TNotifyEvent<THSMap> DestroyEvents
        {
            add => FDestroyEventsStore.Add(value);
            remove => FDestroyEventsStore.Remove(value);
        }

        public event TNotifyEvent<THexagonSprite> HSChangedEvents
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

    }

    // FStatus, FHSSet, FHSMap, FHSSetSearchPath, FHSSFileName, FOnHSMLoaded, FOnHSMClose,
    // FOnHSSLoaded, FOnHSSClose. The engine of the hex map: it owns the set (.hss: a 52-byte
    // header, the stored set and a CRC32) and the map (.hsm: a 16-byte header and the stored map,
    // compressed). the event lists are told when a map or a set is loaded or closed
    public partial class THSEngine : TEngine
    {
        // HSEngine.pas:4957
        public new const int ClassID = 65537;

        internal TSet<THSEngineStatusElement> FStatus;
        protected internal THSSet FHSSet;
        protected internal THSMap FHSMap;
        internal string FHSSetSearchPath = "";
        internal string FHSSFileName = "";

        internal EventStore<THSMap> FOnHSMLoadedStore;

        internal event TNotifyEvent<THSMap> FOnHSMLoaded
        {
            add => FOnHSMLoadedStore.Add(value);
            remove => FOnHSMLoadedStore.Remove(value);
        }

        internal EventStore<THSMap> FOnHSMCloseStore;

        internal event TNotifyEvent<THSMap> FOnHSMClose
        {
            add => FOnHSMCloseStore.Add(value);
            remove => FOnHSMCloseStore.Remove(value);
        }

        internal EventStore<THSEngine> FOnHSSLoadedStore;

        internal event TNotifyEvent<THSEngine> FOnHSSLoaded
        {
            add => FOnHSSLoadedStore.Add(value);
            remove => FOnHSSLoadedStore.Remove(value);
        }

        internal EventStore<THSEngine> FOnHSSCloseStore;

        internal event TNotifyEvent<THSEngine> FOnHSSClose
        {
            add => FOnHSSCloseStore.Add(value);
            remove => FOnHSSCloseStore.Remove(value);
        }

        // HSEngine.pas:4941
        public override void Dispose()
        {
            FOnHSMLoadedStore.Clear();
            FOnHSMCloseStore.Clear();
            CloseHSM(null);
            base.Dispose();
            FOnHSMLoadedStore.Clear();
            FOnHSMCloseStore.Clear();
            FOnHSSLoadedStore.Clear();
            FOnHSSCloseStore.Clear();
        }

        private protected THSEngine(ConstructionContext context, TECustomNode owner) : base(context, owner)
        {
            FOnHSMLoadedStore = new();
            FOnHSMCloseStore = new();
            FOnHSSLoadedStore = new();
            FOnHSSCloseStore = new();
            RegisterEClasses([EClass.Of<TMapContainer>(), EClass.Of<TMapLevel>(), EClass.Of<THSMap>(), EClass.Of<THSSet>()]);
            RegisterEClasses([EClass.Of<Aow.Game.World.TMapObjectResource>(), EClass.Of<Aow.Game.World.TMultiHexMOR>()]);
            RegisterEClasses([EClass.Of<TAbstractHexagon>(), EClass.Of<TAbstractBorderHexagon>(), EClass.Of<TAbstractRoad>()]);
            Aow.Game.World.Map.THSEngine.HSEngineDID = RegisterDebugID("HS-Engine");
            FHSSFileName = "noname.hss";
            CreateHSSet();
        }

        // HSEngine.pas:5019
        public virtual void CreateHSSet()
        {
            FHSSet = new THSSet(this);
        }

        // HSEngine.pas:4962
        public virtual void SetHSMap(THSMap Value)
        {
            FHSMap = Value;
        }

        // HSEngine.pas:5210
        // The preflight of the file boundary finishes before the current map is freed. A refused
        // file leaves the current game and the file as they were (own-formats.md 2.7 and 4.1).
        public virtual void LoadHSM(string FileName, bool Edit, Aow.Game.Engine.TProgressEvent AProgress, Aow.Formats.GameProfile Profile)
        {
            TCompressStream Stream;
            THSMHeader Header = default;
            byte[] Image = FileBoundary.Load(FileName, Profile);
            if (Image == null)
            {
                return;
            }

            if (FHSMap != null)
            {
                FHSMap?.Dispose();
                SetHSMap(null);
            }

            try
            {
                Stream = new TCompressStream();
                try
                {
                    Stream.LoadImage(Image);
                    Stream.ReadBytes(Bytes.Of(ref Header, 16));
                    Stream.Position = Header.Size;
                    if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, Aow.Game.World.Map.THSEngine.HSMID))
                    {
                        SetHSMap((THSMap)CreateEObject(Header.ClassID, this));
                        var expandFileName = ExpandFileName(FileName);
                        FHSMap.FFileName = expandFileName;
                        FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                        if (Edit)
                        {
                            FHSMap.FStatus = FHSMap.FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msEditting);
                        }

                        try
                        {
                            FHSMap.FOnProgress = AProgress;
                            ReadEObject(Stream, FHSMap, AProgress);
                            ActivateEObject(FHSMap);
                            FHSMap.FOnProgress = null;
                            FOnHSMLoadedStore.Raise(FHSMap);
                        }
                        catch
                        {
                            FHSMap?.Dispose();
                            SetHSMap(null);
                        }

                        FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                    }
                }
                finally
                {
                    Stream?.Dispose();
                }
            }
            catch
            {
            }
        }

        // HSEngine.pas:5137
        public virtual THSMap CreatePreviewHSM(string FileName, Aow.Formats.GameProfile Profile)
        {
            TCompressStream Stream;
            string FullName;
            THSMHeader Header = default;
            THSMap Result = null;
            byte[] Image;
            FullName = ExpandFileName(FileName);
            Image = FileBoundary.Load(FullName, Profile, "preview");
            if (Image == null)
            {
                return null;
            }

            try
            {
                Stream = new TCompressStream();
                try
                {
                    Stream.LoadImage(Image);
                    Stream.ReadBytes(Bytes.Of(ref Header, 16));
                    Stream.Position = Header.Size;
                    if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, Aow.Game.World.Map.THSEngine.HSMID))
                    {
                        Result = (THSMap)CreateEObject(Header.ClassID, this);
                        Result.FFileName = ExpandFileName(FileName);
                        FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                        try
                        {
                            ReadPreviewEObject(Stream, Result, null);
                        }
                        catch
                        {
                            Result?.Dispose();
                            Result = null;
                        }

                        FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                    }
                }
                finally
                {
                    Stream?.Dispose();
                }
            }
            catch
            {
            }

            return Result;
        }

        // HSEngine.pas:5291
        // The engine writes the image in memory and the file boundary writes the document, so a
        // failed conversion or write keeps the old file (own-formats.md 4.2).
        public virtual void SaveHSM(string FileName, Aow.Game.Engine.TProgressEvent AProgress, Aow.Formats.GameProfile Profile)
        {
            TCompressStream Stream;
            THSMHeader Header;
            if (FHSMap != null)
            {
                Header = default;
                Header.Size = 16;
                var classID = FHSMap.EClass.ClassID;
                Header.ClassID = classID;
                Header.Reserved = 0;
                Header.ID = Aow.Game.World.Map.THSEngine.HSMID;
                Stream = new TCompressStream();
                FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esSavingHSM);
                try
                {
                    Stream.Write(Bytes.Of(ref Header, 16));
                    WriteEObject(Stream, FHSMap, AProgress);
                    FileBoundary.Save(FileName, Profile, Stream.GetBuffer().AsSpan(0, (int)Stream.Length).ToArray());
                }
                finally
                {
                    FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esSavingHSM);
                    Stream?.Dispose();
                }
            }
        }

        // HSEngine.pas:5368
        public virtual void CloseHSM(Aow.Game.Engine.TProgressEvent AProgress)
        {
            if (FHSMap != null)
            {
                FHSMap.FOnProgress = AProgress;
                FOnHSMCloseStore.Raise(FHSMap);
                FHSMap?.Dispose();
                SetHSMap(null);
            }
        }

        // HSEngine.pas:5093
        public virtual void CloseHSS()
        {
            FOnHSSCloseStore.Raise(this);
            FHSSet.Clear();
            FHSSFileName = "noname.hss";
        }

        // HSEngine.pas:4967
        public TStringList GetHSSetFiles(string Mask)
        {
            TStringList Files;
            void Search(string Dir)
            {
                TStringList Dirs;
                THSSHeader Header = default;
                TSearchRec SR = default;
                bool Found;
                Found = FindFirst(Dir + Mask, 63 & ~16, ref SR) == 0;
                while (Found)
                {
                    Header = default;
                    // An own scenario set lists the header of its validated image (own-formats.md 4.1).
                    var head = FileBoundary.ListingHeader(Dir + SR.Name, Aow.Formats.GameProfile.ScenarioSet, 52);
                    if (head.Length == 52)
                    {
                        Header = MemoryMarshal.Read<THSSHeader>(head);
                    }

                    if (Header.ID == "HSS\0")
                    {
                        Files.Add(Header.Name + "|" + Dir + SR.Name);
                    }

                    Found = FindNext(ref SR) == 0;
                }

                FindClose(ref SR);
                Dirs = new TStringList();
                Found = FindFirst(Dir + "*.*", 16, ref SR) == 0;
                while (Found)
                {
                    if ((SR.Attr & 16) != 0 && SR.Name != "." && SR.Name != "..")
                    {
                        Dirs.Add(Dir + SR.Name + "\\");
                    }

                    Found = FindNext(ref SR) == 0;
                }

                FindClose(ref SR);
                for (int I = 0, last = Dirs.Count - 1; I <= last; I++)
                {
                    Search(Dirs[I]);
                }

                Dirs?.Dispose();
            }

            Files = new TStringList();
            Search(StartupDirectory);
            return Files;
        }

        // HSEngine.pas:5024
        // The preflight raises before the current set is cleared (own-formats.md 2.7).
        public void LoadHSS(string FileName, bool Edit, Aow.Game.Engine.TProgressEvent AProgress, Aow.Formats.GameProfile Profile)
        {
            MemoryStream Mem;
            THSSHeader Header = default;
            TEMsg Msg = default;
            int CRC;
            byte[] Image = FileBoundary.LoadOrRaise(ExpandFileName(FileName), Profile);
            FHSSet.Clear();
            FHSSFileName = ExpandFileName(FileName);
            FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSS);
            Mem = TEngine.ImageStream(Image);
            try
            {
                Mem.Read(Bytes.Of(ref Header, 52));
                Mem.Position = Header.Size;
                var size = (int)Mem.Length;
                CRC = MemoryMarshal.Read<int>(Mem.GetBuffer().AsSpan(size - 4, 4));
                bool condition = Header.ID != "HSS\0";
                if (!condition)
                {
                    var memSize = (int)Mem.Length;
                    condition = CRC != Aow.Game.Engine.Nodes.EngineServices.GetCRC32(Mem.GetBuffer(), memSize - 4);
                }

                if (condition)
                {
                    throw new ArgumentException("Invalid HSSET");
                }

                ReadEObject(Mem, FHSSet, AProgress);
                ActivateEObject(FHSSet);
                Aow.Game.Engine.Nodes.TEMsg.InitMsg(ref Msg, 65556, this);
                FHSSet.MsgProc(ref Msg, this);
                ControlNode.MsgProc(ref Msg, this);
                FOnHSSLoadedStore.Raise(this);
            }
            finally
            {
                FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSS);
                Mem?.Dispose();
            }
        }

        // HSEngine.pas:5060
        public void SaveHSS(string FileName, Aow.Game.Engine.TProgressEvent AProgress, Aow.Formats.GameProfile Profile)
        {
            MemoryStream Mem;
            int CRC = default;
            THSSHeader Header = default;
            if (FHSSet != null)
            {
                Header.Size = 52;
                Header.Name = FHSSet.Name;
                Header.ID = "HSS\0";
                FHSSFileName = ExpandFileName(FileName);
                FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esSavingHSS);
                Mem = new MemoryStream();
                try
                {
                    Mem.Write(Bytes.Of(ref Header, 52));
                    WriteEObject(Mem, FHSSet, AProgress);
                    var size = (int)Mem.Length;
                    CRC = Aow.Game.Engine.Nodes.EngineServices.GetCRC32(Mem.GetBuffer(), size);
                    Mem.Write(Bytes.Of(ref CRC, 4));
                    FileBoundary.Save(FHSSFileName, Profile, Mem.GetBuffer().AsSpan(0, (int)Mem.Length).ToArray());
                }
                finally
                {
                    FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esSavingHSS);
                    Mem?.Dispose();
                }
            }
        }

        // HSEngine.pas:5100
        public void LoadPreviewHSM(string FileName, Aow.Formats.GameProfile Profile)
        {
            TCompressStream Stream;
            THSMHeader Header = default;
            byte[] Image = FileBoundary.Load(FileName, Profile, "preview");
            if (Image == null)
            {
                return;
            }

            if (FHSMap != null)
            {
                FHSMap?.Dispose();
                SetHSMap(null);
            }

            try
            {
                Stream = new TCompressStream();
                try
                {
                    Stream.LoadImage(Image);
                    Stream.ReadBytes(Bytes.Of(ref Header, 16));
                    Stream.Position = Header.Size;
                    if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, Aow.Game.World.Map.THSEngine.HSMID))
                    {
                        SetHSMap((THSMap)CreateEObject(Header.ClassID, this));
                        FHSMap.FStatus = FHSMap.FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msPreviewMode);
                        var expandFileName = ExpandFileName(FileName);
                        FHSMap.FFileName = expandFileName;
                        FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                        try
                        {
                            ReadEObject(Stream, FHSMap, null);
                        }
                        catch
                        {
                            FHSMap?.Dispose();
                            SetHSMap(null);
                        }

                        FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                    }
                }
                finally
                {
                    Stream?.Dispose();
                }
            }
            catch
            {
            }
        }

        // HSEngine.pas:5171
        public THSMap LoadHSMEx(string FileName, bool Edit, Aow.Game.Engine.TProgressEvent AProgress, Aow.Formats.GameProfile Profile)
        {
            TCompressStream Stream;
            string FullName;
            THSMHeader Header = default;
            THSMap Result = null;
            byte[] Image;
            FullName = ExpandFileName(FileName);
            Image = FileBoundary.Load(FullName, Profile);
            if (Image == null)
            {
                return null;
            }

            try
            {
                Stream = new TCompressStream();
                try
                {
                    Stream.LoadImage(Image);
                    Stream.ReadBytes(Bytes.Of(ref Header, 16));
                    Stream.Position = Header.Size;
                    if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, Aow.Game.World.Map.THSEngine.HSMID))
                    {
                        Result = (THSMap)CreateEObject(Header.ClassID, null);
                        Result.FFileName = FullName;
                        FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                        if (Edit)
                        {
                            Result.FStatus = Result.FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msEditting);
                        }

                        try
                        {
                            Result.FOnProgress = AProgress;
                            ReadEObject(Stream, Result, AProgress);
                            ActivateEObject(Result);
                            Result.FOnProgress = null;
                            FOnHSMLoadedStore.Raise(Result);
                        }
                        catch
                        {
                            Result?.Dispose();
                            Result = null;
                        }

                        FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                    }
                }
                finally
                {
                    Stream?.Dispose();
                }
            }
            catch
            {
            }

            return Result;
        }

        // HSEngine.pas:5251
        public void LoadHSMfromStream(Stream Stream, bool Edit, Aow.Game.Engine.TProgressEvent AProgress)
        {
            TCompressStream CStream;
            THSMHeader Header = default;
            if (FHSMap != null)
            {
                FHSMap?.Dispose();
                SetHSMap(null);
            }

            try
            {
                CStream = new TCompressStream();
                try
                {
                    CStream.LoadFromStream(Stream);
                    CStream.ReadBytes(Bytes.Of(ref Header, 16));
                    CStream.Position = Header.Size;
                    if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, Aow.Game.World.Map.THSEngine.HSMID))
                    {
                        SetHSMap((THSMap)CreateEObject(Header.ClassID, this));
                        FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                        if (Edit)
                        {
                            FHSMap.FStatus = FHSMap.FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msEditting);
                        }

                        try
                        {
                            FHSMap.FOnProgress = AProgress;
                            ReadEObject(CStream, FHSMap, AProgress);
                            ActivateEObject(FHSMap);
                            FHSMap.FOnProgress = null;
                            FOnHSMLoadedStore.Raise(FHSMap);
                        }
                        catch
                        {
                            FHSMap?.Dispose();
                            SetHSMap(null);
                        }

                        FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                    }
                }
                finally
                {
                    CStream?.Dispose();
                }
            }
            catch
            {
            }
        }

        // HSEngine.pas:5316
        public void SaveHSMtoStream(Stream Stream, Aow.Game.Engine.TProgressEvent AProgress)
        {
            TCompressStream CStream;
            THSMHeader Header;
            if (FHSMap != null)
            {
                Header = default;
                Header.Size = 16;
                var classID = FHSMap.EClass.ClassID;
                Header.ClassID = classID;
                Header.Reserved = 0;
                Header.ID = Aow.Game.World.Map.THSEngine.HSMID;
                CStream = new TCompressStream();
                FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esSavingHSM);
                try
                {
                    CStream.Write(Bytes.Of(ref Header, 16));
                    WriteEObject(CStream, FHSMap, AProgress);
                    CStream.SaveToStream(Stream);
                }
                finally
                {
                    FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esSavingHSM);
                    CStream?.Dispose();
                }
            }
        }

        // HSEngine.pas:5341
        // Every caller derives the name (RELEASE.HSS, the set a map names), so the own sibling opens
        // first (own-formats.md 2.9).
        public void InitializeHSSet(string FileName, Aow.Game.Engine.TProgressEvent AProgress)
        {
            TEMsg Msg = default;
            FileName = FileBoundary.ReadName(FileName);
            if (FHSSet == null || CompareText(FileName, FHSSFileName) != 0)
            {
                LoadHSS(FileName, false, AProgress, Aow.Formats.GameProfile.ScenarioSet);
            }
            else
            {
                Aow.Game.Engine.Nodes.TEMsg.InitMsg(ref Msg, 65557, this);
                FHSSet.MsgProc(ref Msg, this);
            }
        }

        // HSEngine.pas:5354
        public THSMap NewHSM(int AClassID)
        {
            if (FHSMap != null)
            {
                FHSMap?.Dispose();
                SetHSMap(null);
            }

            SetHSMap((THSMap)CreateEObject(AClassID, this));
            FHSMap.FStatus = FHSMap.FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msEditting);
            ActivateEObject(FHSMap);
            FOnHSMLoadedStore.Raise(FHSMap);
            return FHSMap;
        }

        public TSet<THSEngineStatusElement> Status { get => FStatus; }
        public THSSet HSSet { get => FHSSet; }
        public THSMap HSMap { get => FHSMap; set => SetHSMap(value); }
        public string HSSetSearchPath { get => FHSSetSearchPath; set => FHSSetSearchPath = value; }
        public string HSSFileName { get => FHSSFileName; }

        public event TNotifyEvent<THSMap> OnHSMLoaded
        {
            add => FOnHSMLoadedStore.Add(value);
            remove => FOnHSMLoadedStore.Remove(value);
        }

        public event TNotifyEvent<THSMap> OnHSMClose
        {
            add => FOnHSMCloseStore.Add(value);
            remove => FOnHSMCloseStore.Remove(value);
        }

        public event TNotifyEvent<THSEngine> OnHSSLoaded
        {
            add => FOnHSSLoadedStore.Add(value);
            remove => FOnHSSLoadedStore.Remove(value);
        }

        public event TNotifyEvent<THSEngine> OnHSSClose
        {
            add => FOnHSSCloseStore.Add(value);
            remove => FOnHSSCloseStore.Remove(value);
        }

        // HSEngine.pas:803
        public static THSEngine Create(TECustomNode owner) =>
            ConstructionContext.Create(owner, static (context, owner) => new THSEngine(context, owner));

        private static global::Aow.Game.Engine.Buffers.FixedArray_byte_4 HSMID = global::Aow.Game.Engine.Buffers.FixedArray_byte_4.From("HSM\0"u8);
        internal static int HSEngineDID;

    }
}

namespace Aow.Game.Draw
{

    // the base of everything placed on the map. Owner is the TMapField the sprite stands on (for
    // the multi-hex sprites the field of the base hexagon)
    public partial class THexagonSprite : TEObject
    {
        public THexagonSprite(TECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 65872;

        // HXSPRITE.PAS:6
        // { HXSPRITE.PAS: THexagonSprite, THPBasedHS, TSingleHS, TFixedVisibleHS, TCustomFixedVisibleHS
        //   (include file of HSEngine.pas; the original Asserts name D:\GEngine\HSENGINE\inc\HXSPRITE.PAS) }
        public override void Dispose()
        {
            MainRemove();
            base.Dispose();
        }

        // HXSPRITE.PAS:116
        public virtual sbyte GetTerrain(int HN) => -2;

        // HXSPRITE.PAS:121
        public virtual void SetTerrain(int HN, sbyte Value)
        {
        }

        // HXSPRITE.PAS:125
        public virtual int GetTerrainCount() => 0;

        // HXSPRITE.PAS:130
        public virtual sbyte GetOverlay(int HN) => -2;

        // HXSPRITE.PAS:135
        public virtual void SetOverlay(int HN, sbyte Value)
        {
        }

        // HXSPRITE.PAS:139
        public virtual int GetOverlayCount() => 0;

        // HXSPRITE.PAS:52
        public virtual byte GetLevel()
        {
            throw new InvalidOperationException("No HexagonSprite Level defined");
        }

        // HXSPRITE.PAS:43
        public virtual void SetVisible(bool Value)
        {
        }

        // HXSPRITE.PAS:47
        public virtual bool GetVisible() => false;

        // HXSPRITE.PAS:72
        public virtual bool GetSelected() => false;

        // HXSPRITE.PAS:87
        public virtual sbyte GetXhx() => ((TMapField)Owner).FXhx;

        // HXSPRITE.PAS:92
        public virtual sbyte GetYhx() => ((TMapField)Owner).FYhx;

        // HXSPRITE.PAS:97
        public virtual sbyte GetLhx() => (sbyte)((TMapLevel)Owner.Owner).FLhx;

        // HXSPRITE.PAS:102
        public virtual int GetXYL() => GetXhx() + (GetYhx() << 8) + (GetLhx() << 16);

        // HXSPRITE.PAS:153
        public virtual bool GetEditMode() => false;

        // HXSPRITE.PAS:158
        public virtual void SetEditMode(bool Value)
        {
        }

        // HXSPRITE.PAS:162
        public virtual int GetShowPriority() => 2130706432;

        // HXSPRITE.PAS:189
        public virtual void Connect()
        {
        }

        // HXSPRITE.PAS:193
        public virtual void Disconnect()
        {
        }

        // HXSPRITE.PAS:167
        public virtual bool CanChangeTerrain(in THSSenderInfo Info) => true;

        // HXSPRITE.PAS:172
        public virtual void ChangeTerrain(in THSTerrainInfo Info)
        {
        }

        // HXSPRITE.PAS:176
        public virtual void TerrainChanged(in THSTerrainInfo Info)
        {
        }

        // HXSPRITE.PAS:180
        public virtual void NeighbourTerrainChanged(int HN, in THSTerrainInfo Info)
        {
        }

        // HSEngine.pas:881
        public virtual void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // HXSPRITE.PAS:197
        public virtual bool PlaceOnMap()
        {
            Connect();
            return true;
        }

        // HXSPRITE.PAS:203
        public virtual void RemoveFromMap()
        {
            Disconnect();
        }

        // HXSPRITE.PAS:573
        public virtual bool MainPlace(TMapContainer Map, int Xhp, int Yhp, int Lhx)
        {
            bool Result = Place(Map, Xhp, Yhp, Lhx);
            if (Result)
            {
                try
                {
                    Activate();
                }
                catch
                {
                    Result = false;
                    Deactivate();
                }
            }

            return Result;
        }

        // HXSPRITE.PAS:585
        public virtual void MainRemove()
        {
            Deactivate();
            Remove();
            Assert(!GetVisible(), "Hexagon Sprite Still Visible!", "HXSPRITE.PAS", 589);
        }

        // HXSPRITE.PAS:592
        public virtual bool MainLoaded() => Loaded();

        // HXSPRITE.PAS:184
        public virtual bool Place(TMapContainer Map, int Xhp, int Yhp, int Lhx) => PlaceOnMap();

        // HXSPRITE.PAS:208
        public virtual void Remove()
        {
            RemoveFromMap();
        }

        // HXSPRITE.PAS:213
        public virtual bool Loaded()
        {
            Connect();
            return true;
        }

        // HXSPRITE.PAS:26
        public virtual void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
        }

        // HXSPRITE.PAS:30
        public virtual void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
        {
        }

        // HXSPRITE.PAS:34
        public virtual int CanMoveOver(in THSSenderInfo Info) => 1;

        // HXSPRITE.PAS:39
        public virtual void MoveExclusive(in THSSenderInfo Info)
        {
        }

        // HXSPRITE.PAS:63
        public virtual void Changed()
        {
            TMapField MapF;
            MapF = (TMapField)Owner;
            if (MapF != null)
            {
                ((THSEngine)((TMapLevel)MapF.Owner).FEngine).FHSMap.FHSChangedEventsStore.Raise(this);
            }
        }

        // HXSPRITE.PAS:563
        public virtual void Activate()
        {
        }

        // HXSPRITE.PAS:567
        public virtual void Deactivate()
        {
            if (GetSelected())
            {
                Unselect();
            }
        }

        // HXSPRITE.PAS:22
        public virtual void UpdateMapField(in THSMapFieldInfo Info)
        {
        }

        // HXSPRITE.PAS:144
        public virtual void MakeVisible(TVisibilityList VisibilityList)
        {
            if (!GetVisible())
            {
                SetVisible(true);
                VisibilityList.AddSortChild(this, GetShowPriority());
            }
        }

        // HXSPRITE.PAS:82
        public virtual void GetBaseHX(ref int XYL)
        {
            XYL = ((TMapField)Owner).FXhx + (((TMapField)Owner).FYhx << 8) + (((TMapLevel)Owner.Owner).FLhx << 16);
        }

        // HXSPRITE.PAS:57
        public virtual bool CanSelect(int Xhp, int Yhp) => Owner != null && ((THSEngine)((TMapLevel)Owner.Owner).FEngine).FHSMap.FStatus.Contains(THSMapStatusElement.msEditting);

        // HXSPRITE.PAS:107
        public virtual void Select()
        {
        }

        // HXSPRITE.PAS:111
        public virtual void Unselect()
        {
            ((TMapLevel)Owner.Owner).FMap.FSelection.Unselect(this);
        }

        // HXSPRITE.PAS:17
        public virtual string EditName() => "Hexagon Sprite";

        // HXSPRITE.PAS:219
        public bool PlaceOnMapEx(TMapContainer Map, int Xhp, int Yhp, int Lhx, int Width, int Height)
        {
            bool Result;
            int X = default;
            int Y = default;
            int Xmin = default;
            int YminOdd = default;
            int YminEven;
            int Xmax = default;
            int YmaxOdd = default;
            int YmaxEven;
            TMapLevel MapL;
            Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
            Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
            if (!((Xmin & 1) != 0))
            {
                YminEven = YminOdd;
                YminOdd = Y;
            }
            else
            {
                YminEven = Y;
            }

            Y = Yhp + Height - 1;
            Aow.Game.World.Map.HexGeometry.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
            Aow.Game.World.Map.HexGeometry.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
            if (!((Xmax & 1) != 0))
            {
                YmaxEven = YmaxOdd;
                YmaxOdd = Y;
            }
            else
            {
                YmaxEven = Y;
            }

            if (Xmax >= 0 && Map.Width > Xmin && (YmaxEven >= 0 || YmaxOdd >= 0) && (Map.Height > YminEven || Map.Height > YminOdd))
            {
                if (Xmin < 0)
                {
                    Xmin = 0;
                }

                if (Xmax >= Map.Width)
                {
                    Xmax = Map.Width - 1;
                }

                if (YminEven < 0)
                {
                    YminEven = 0;
                    if (YmaxEven < 0)
                    {
                        YmaxEven = 0;
                    }
                }

                if (YmaxEven >= Map.Height)
                {
                    YmaxEven = Map.Height - 1;
                    if (YminEven >= Map.Height)
                    {
                        YminEven = Map.Height - 1;
                    }
                }

                if (YminOdd < 0)
                {
                    YminOdd = 0;
                    if (YmaxOdd < 0)
                    {
                        YmaxOdd = 0;
                    }
                }

                if (YmaxOdd >= Map.Height)
                {
                    YmaxOdd = Map.Height - 1;
                    if (YminOdd >= Map.Height)
                    {
                        YminOdd = Map.Height - 1;
                    }
                }

                MapL = Map.GetMapLevel(Lhx);
                X = (int)(((uint)(Xmin + Xmax)) >> 1);
                if (!((X & 1) != 0))
                {
                    Y = (int)(((uint)(YminEven + YmaxEven)) >> 1);
                }
                else
                {
                    Y = (int)(((uint)(YminOdd + YmaxOdd)) >> 1);
                }

                FOwner = MapL.GetField(X, Y);
                X = Xmin;
                int last = Xmax;
                for (; X <= last; X += 1)
                {
                    if (!((X & 1) != 0))
                    {
                        Y = YminEven;
                        for (; Y <= YmaxEven; Y += 1)
                        {
                            MapL.GetField(X, Y).AddHS(this, 1);
                        }
                    }
                    else
                    {
                        Y = YminOdd;
                        int limit = YmaxOdd;
                        for (; Y <= limit; Y += 1)
                        {
                            MapL.GetField(X, Y).AddHS(this, 1);
                        }
                    }
                }

                ((TMapField)FOwner).SetHNofHS(this, 0);
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // HXSPRITE.PAS:295
        public void RemoveFromMapEx(int Xhp, int Yhp, int Width, int Height)
        {
            int X = default;
            int Y = default;
            int Xmin = default;
            int YminOdd = default;
            int YminEven;
            int Xmax = default;
            int YmaxOdd = default;
            int YmaxEven;
            TMapLevel MapL;
            MapL = (TMapLevel)FOwner.Owner;
            Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
            Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
            if (!((Xmin & 1) != 0))
            {
                YminEven = YminOdd;
                YminOdd = Y;
            }
            else
            {
                YminEven = Y;
            }

            Y = Yhp + Height - 1;
            Aow.Game.World.Map.HexGeometry.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
            Aow.Game.World.Map.HexGeometry.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
            if (!((Xmax & 1) != 0))
            {
                YmaxEven = YmaxOdd;
                YmaxOdd = Y;
            }
            else
            {
                YmaxEven = Y;
            }

            if (Xmax >= 0 && Xmin < MapL.Width && (YmaxEven >= 0 || YmaxOdd >= 0) && (MapL.Height > YminEven || MapL.Height > YminOdd))
            {
                if (Xmin < 0)
                {
                    Xmin = 0;
                }

                if (Xmax >= MapL.Width)
                {
                    Xmax = MapL.Width - 1;
                }

                if (YminEven < 0)
                {
                    YminEven = 0;
                    if (YmaxEven < 0)
                    {
                        YmaxEven = 0;
                    }
                }

                if (YmaxEven >= MapL.Height)
                {
                    YmaxEven = MapL.Height - 1;
                    if (YminEven >= MapL.Height)
                    {
                        YminEven = MapL.Height - 1;
                    }
                }

                if (YminOdd < 0)
                {
                    YminOdd = 0;
                    if (YmaxOdd < 0)
                    {
                        YmaxOdd = 0;
                    }
                }

                if (YmaxOdd >= MapL.Height)
                {
                    YmaxOdd = MapL.Height - 1;
                    if (YminOdd >= MapL.Height)
                    {
                        YminOdd = MapL.Height - 1;
                    }
                }

                X = Xmin;
                int last = Xmax;
                for (; X <= last; X += 1)
                {
                    if (!((X & 1) != 0))
                    {
                        Y = YminEven;
                        for (; Y <= YmaxEven; Y += 1)
                        {
                            MapL.GetField(X, Y).RemoveHS(this);
                        }
                    }
                    else
                    {
                        Y = YminOdd;
                        int limit = YmaxOdd;
                        for (; Y <= limit; Y += 1)
                        {
                            MapL.GetField(X, Y).RemoveHS(this);
                        }
                    }
                }
            }

            FOwner = null;
        }

        // HXSPRITE.PAS:362
        public bool PlaceUserOnMapEx(TMapContainer Map, int Xhp, int Yhp, int Lhx, int Width, int Height)
        {
            bool Result;
            int X = default;
            int Y = default;
            int Xmin = default;
            int YminOdd = default;
            int YminEven;
            int Xmax = default;
            int YmaxOdd = default;
            int YmaxEven;
            TMapLevel MapL;
            Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
            Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
            if (!((Xmin & 1) != 0))
            {
                YminEven = YminOdd;
                YminOdd = Y;
            }
            else
            {
                YminEven = Y;
            }

            Y = Yhp + Height - 1;
            Aow.Game.World.Map.HexGeometry.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
            Aow.Game.World.Map.HexGeometry.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
            if (!((Xmax & 1) != 0))
            {
                YmaxEven = YmaxOdd;
                YmaxOdd = Y;
            }
            else
            {
                YmaxEven = Y;
            }

            if (Xmax >= 0 && Map.Width > Xmin && (YmaxEven >= 0 || YmaxOdd >= 0) && (Map.Height > YminEven || Map.Height > YminOdd))
            {
                if (Xmin < 0)
                {
                    Xmin = 0;
                }

                if (Xmax >= Map.Width)
                {
                    Xmax = Map.Width - 1;
                }

                if (YminEven < 0)
                {
                    YminEven = 0;
                    if (YmaxEven < 0)
                    {
                        YmaxEven = 0;
                    }
                }

                if (YmaxEven >= Map.Height)
                {
                    YmaxEven = Map.Height - 1;
                    if (YminEven >= Map.Height)
                    {
                        YminEven = Map.Height - 1;
                    }
                }

                if (YminOdd < 0)
                {
                    YminOdd = 0;
                    if (YmaxOdd < 0)
                    {
                        YmaxOdd = 0;
                    }
                }

                if (YmaxOdd >= Map.Height)
                {
                    YmaxOdd = Map.Height - 1;
                    if (YminOdd >= Map.Height)
                    {
                        YminOdd = Map.Height - 1;
                    }
                }

                MapL = Map.GetMapLevel(Lhx);
                X = Xmin;
                int last = Xmax;
                for (; X <= last; X += 1)
                {
                    if (!((X & 1) != 0))
                    {
                        Y = YminEven;
                        for (; Y <= YmaxEven; Y += 1)
                        {
                            MapL.GetField(X, Y).AddHS(this, 1);
                        }
                    }
                    else
                    {
                        Y = YminOdd;
                        int limit = YmaxOdd;
                        for (; Y <= limit; Y += 1)
                        {
                            MapL.GetField(X, Y).AddHS(this, 1);
                        }
                    }
                }

                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // HXSPRITE.PAS:431
        public void RemoveUserFromMapEx(TMapContainer Map, int Xhp, int Yhp, int Lhx, int Width, int Height)
        {
            int X = default;
            int Y = default;
            int Xmin = default;
            int YminOdd = default;
            int YminEven;
            int Xmax = default;
            int YmaxOdd = default;
            int YmaxEven;
            TMapLevel MapL;
            MapL = Map.GetMapLevel(Lhx);
            Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
            Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
            if (!((Xmin & 1) != 0))
            {
                YminEven = YminOdd;
                YminOdd = Y;
            }
            else
            {
                YminEven = Y;
            }

            Y = Yhp + Height - 1;
            Aow.Game.World.Map.HexGeometry.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
            Aow.Game.World.Map.HexGeometry.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
            if (!((Xmax & 1) != 0))
            {
                YmaxEven = YmaxOdd;
                YmaxOdd = Y;
            }
            else
            {
                YmaxEven = Y;
            }

            if (Xmax >= 0 && Xmin < MapL.Width && (YmaxEven >= 0 || YmaxOdd >= 0) && (MapL.Height > YminEven || MapL.Height > YminOdd))
            {
                if (Xmin < 0)
                {
                    Xmin = 0;
                }

                if (Xmax >= MapL.Width)
                {
                    Xmax = MapL.Width - 1;
                }

                if (YminEven < 0)
                {
                    YminEven = 0;
                    if (YmaxEven < 0)
                    {
                        YmaxEven = 0;
                    }
                }

                if (YmaxEven >= MapL.Height)
                {
                    YmaxEven = MapL.Height - 1;
                    if (YminEven >= MapL.Height)
                    {
                        YminEven = MapL.Height - 1;
                    }
                }

                if (YminOdd < 0)
                {
                    YminOdd = 0;
                    if (YmaxOdd < 0)
                    {
                        YmaxOdd = 0;
                    }
                }

                if (YmaxOdd >= MapL.Height)
                {
                    YmaxOdd = MapL.Height - 1;
                    if (YminOdd >= MapL.Height)
                    {
                        YminOdd = MapL.Height - 1;
                    }
                }

                X = Xmin;
                int last = Xmax;
                for (; X <= last; X += 1)
                {
                    if (!((X & 1) != 0))
                    {
                        Y = YminEven;
                        for (; Y <= YmaxEven; Y += 1)
                        {
                            MapL.GetField(X, Y).RemoveHS(this);
                        }
                    }
                    else
                    {
                        Y = YminOdd;
                        int limit = YmaxOdd;
                        for (; Y <= limit; Y += 1)
                        {
                            MapL.GetField(X, Y).RemoveHS(this);
                        }
                    }
                }
            }
        }

        // HXSPRITE.PAS:497
        public void SortMapFieldEx(TMapContainer Map, int Xhp, int Yhp, int Lhx, int Width, int Height)
        {
            int X = default;
            int Y = default;
            int Xmin = default;
            int YminOdd = default;
            int YminEven;
            int Xmax = default;
            int YmaxOdd = default;
            int YmaxEven;
            TMapLevel MapL;
            MapL = Map.GetMapLevel(Lhx);
            Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
            Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
            if (!((Xmin & 1) != 0))
            {
                YminEven = YminOdd;
                YminOdd = Y;
            }
            else
            {
                YminEven = Y;
            }

            Y = Yhp + Height - 1;
            Aow.Game.World.Map.HexGeometry.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
            Aow.Game.World.Map.HexGeometry.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
            if (!((Xmax & 1) != 0))
            {
                YmaxEven = YmaxOdd;
                YmaxOdd = Y;
            }
            else
            {
                YmaxEven = Y;
            }

            if (Xmax >= 0 && Xmin < MapL.Width && (YmaxEven >= 0 || YmaxOdd >= 0) && (MapL.Height > YminEven || MapL.Height > YminOdd))
            {
                if (Xmin < 0)
                {
                    Xmin = 0;
                }

                if (Xmax >= MapL.Width)
                {
                    Xmax = MapL.Width - 1;
                }

                if (YminEven < 0)
                {
                    YminEven = 0;
                    if (YmaxEven < 0)
                    {
                        YmaxEven = 0;
                    }
                }

                if (YmaxEven >= MapL.Height)
                {
                    YmaxEven = MapL.Height - 1;
                    if (YminEven >= MapL.Height)
                    {
                        YminEven = MapL.Height - 1;
                    }
                }

                if (YminOdd < 0)
                {
                    YminOdd = 0;
                    if (YmaxOdd < 0)
                    {
                        YmaxOdd = 0;
                    }
                }

                if (YmaxOdd >= MapL.Height)
                {
                    YmaxOdd = MapL.Height - 1;
                    if (YminOdd >= MapL.Height)
                    {
                        YminOdd = MapL.Height - 1;
                    }
                }

                X = Xmin;
                int last = Xmax;
                for (; X <= last; X += 1)
                {
                    if (!((X & 1) != 0))
                    {
                        Y = YminEven;
                        for (; Y <= YmaxEven; Y += 1)
                        {
                            MapL.GetField(X, Y).Sort();
                        }
                    }
                    else
                    {
                        Y = YminOdd;
                        int limit = YmaxOdd;
                        for (; Y <= limit; Y += 1)
                        {
                            MapL.GetField(X, Y).Sort();
                        }
                    }
                }
            }
        }

        public sealed class TerrainAccessor(THexagonSprite target)
        {
            public sbyte this[int HN] { get => target.GetTerrain(HN); set => target.SetTerrain(HN, value); }
        }

        public TerrainAccessor Terrain => new TerrainAccessor(this);
        public int TerrainCount { get => GetTerrainCount(); }

        public sealed class OverlayAccessor(THexagonSprite target)
        {
            public sbyte this[int HN] { get => target.GetOverlay(HN); set => target.SetOverlay(HN, value); }
        }

        public OverlayAccessor Overlay => new OverlayAccessor(this);
        public int OverlayCount { get => GetOverlayCount(); }
        public byte Level { get => GetLevel(); }
        public bool Visible { get => GetVisible(); set => SetVisible(value); }
        public bool Selected { get => GetSelected(); }
        public sbyte Xhx { get => GetXhx(); }
        public sbyte Yhx { get => GetYhx(); }
        public sbyte Lhx { get => GetLhx(); }
        public int XYL { get => GetXYL(); }
        public bool EditMode { get => GetEditMode(); set => SetEditMode(value); }
        public int ShowPriority { get => GetShowPriority(); }

        // HXSPRITE.PAS:77
        public virtual TSet<TControlModeElement> ControlMode() => TSet<TControlModeElement>.Of();

    }
}

namespace Aow.Game.World
{

    // a hexagon sprite drawn from a resource. FResource (the resource. until the object is linked
    // it holds the resource id, persisted as id 1), FStatus
    public partial class TMapObject : THexagonSprite
    {
        public TMapObject(TECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 65878;

        protected internal TMapObjectResource FResource;
        protected internal TSet<TMapObjectStatusElement> FStatus;

        // MAPOBJ.PAS:88
        protected internal bool GetResourceLinked() => FStatus.Contains(TMapObjectStatusElement.moResourceLinked);

        // MAPOBJ.PAS:93
        protected internal void SetResourceLinked(bool Value)
        {
            if (Value)
            {
                FStatus = FStatus + TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moResourceLinked);
            }
            else
            {
                FStatus = FStatus - TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moResourceLinked);
            }
        }

        // MAPOBJ.PAS:138
        public override sbyte GetTerrain(int Index)
        {
            sbyte Result = FResource.FTerrainList.GetTerrain(Index);
            if (FStatus.Contains(TMapObjectStatusElement.moEditMode) && Result >= 0)
            {
                Result = -1;
            }

            return Result;
        }

        // MAPOBJ.PAS:145
        public override void SetTerrain(int Index, sbyte Value)
        {
            FResource.FTerrainList.SetTerrain(Index, Value);
        }

        // MAPOBJ.PAS:150
        public override int GetTerrainCount() => FResource.FTerrainList.FCount;

        // MAPOBJ.PAS:155
        public override sbyte GetOverlay(int Index)
        {
            sbyte Result = FResource.FTerrainList.GetOverlay(Index);
            if (FStatus.Contains(TMapObjectStatusElement.moEditMode) && Result >= 0)
            {
                Result = -1;
            }

            return Result;
        }

        // MAPOBJ.PAS:162
        public override void SetOverlay(int Index, sbyte Value)
        {
            FResource.FTerrainList.SetOverlay(Index, Value);
        }

        // MAPOBJ.PAS:167
        public override int GetOverlayCount() => FResource.FTerrainList.FCount;

        // MAPOBJ.PAS:16
        public override byte GetLevel() => FResource.FLevel;

        // MAPOBJ.PAS:8
        public override void SetVisible(bool Value)
        {
            if (Value)
            {
                FStatus = FStatus + TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moVisible);
            }
            else
            {
                if (FStatus.Contains(TMapObjectStatusElement.moVisible))
                {
                    FStatus = FStatus - TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moVisible);
                }
            }
        }

        // MAPOBJ.PAS:29
        public override bool GetVisible() => FStatus.Contains(TMapObjectStatusElement.moVisible);

        // MAPOBJ.PAS:133
        public override bool GetSelected() => FStatus.Contains(TMapObjectStatusElement.moSelected);

        // MAPOBJ.PAS:45
        public override sbyte GetXhx()
        {
            Assert(Owner != null, "Owner not set", "MAPOBJ.PAS", 47);
            return ((TMapField)Owner).FXhx;
        }

        // MAPOBJ.PAS:51
        public override sbyte GetYhx()
        {
            Assert(Owner != null, "Owner not set", "MAPOBJ.PAS", 53);
            return ((TMapField)Owner).FYhx;
        }

        // MAPOBJ.PAS:57
        public override sbyte GetLhx()
        {
            Assert(Owner != null, "Owner not set", "MAPOBJ.PAS", 59);
            return ((TMapField)Owner).FLhx;
        }

        // MAPOBJ.PAS:40
        public override bool GetEditMode() => FStatus.Contains(TMapObjectStatusElement.moEditMode);

        // MAPOBJ.PAS:35
        // { Value is not used: the edit mode is only ever switched on }
        public override void SetEditMode(bool Value)
        {
            FStatus = FStatus + TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moEditMode);
        }

        // MAPOBJ.PAS:214
        public override int GetShowPriority()
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(((TMapField)Owner).FXhx, ((TMapField)Owner).FYhx, ref Xhp, ref Yhp);
            return FResource.FShowPriorityOffset + (Yhp << 13) + Xhp;
        }

        // MAPOBJ.PAS:235
        public override void Connect()
        {
            FStatus = FStatus + TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moPlacedOnMap);
        }

        // MAPOBJ.PAS:240
        public override void Disconnect()
        {
            Owner = null;
            FStatus = FStatus - TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moPlacedOnMap);
        }

        // MAPOBJ.PAS:222
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(((TMapField)Owner).FXhx, ((TMapField)Owner).FYhx, ref Xhp, ref Yhp);
            if (FResource.FImageSequenceList == null)
            {
                FResource.FImageLibrary.Get(0).Show(Xhp - XhpSurf - 8, Yhp - YhpSurf, SurfaceDesc);
            }
            else
            {
                FResource.ShowComplexImage(Xhp - XhpSurf - 8, Yhp - YhpSurf, SurfaceDesc, 0, (uint)(((TMapField)Owner).FXhx + ((TMapField)Owner).FYhx * 16));
            }
        }

        // MAPOBJ.PAS:252
        public override bool PlaceOnMap()
        {
            Connect();
            if (FResource.FTerrainList.GetTerrain(0) >= 0 || FResource.FTerrainList.GetOverlay(0) >= 0)
            {
                ((TMapField)Owner).UpdateTerrain();
            }

            if (GetLevel() <= 15)
            {
                var mapField = (TMapField)Owner;
                ((TMapLevel)mapField.Owner).FMap.InvalidateStaticScene(mapField.FXhx, mapField.FYhx, mapField.FLhx);
            }

            if (!GetEditMode())
            {
                var mapField = (TMapField)Owner;
                ((TMapLevel)mapField.Owner).FMap.TerrainChangedRad(mapField.FXhx, mapField.FYhx, mapField.FLhx, 0);
            }

            return true;
        }

        // MAPOBJ.PAS:266
        public override void RemoveFromMap()
        {
            TMapField MapField;
            if (FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
            {
                if (Owner != null)
                {
                    MapField = (TMapField)Owner;
                    Disconnect();
                    if (!((TMapLevel)MapField.Owner).FMap.FStatus.Contains(THSMapStatusElement.msDestroying))
                    {
                        if (GetTerrain(0) >= 0 || GetOverlay(0) >= 0)
                        {
                            MapField.UpdateTerrain();
                        }

                        if (GetLevel() <= 15 && GetVisible())
                        {
                            ((TMapLevel)MapField.Owner).FMap.FStaticSceneInvalid = true;
                        }
                    }

                    if (!GetEditMode())
                    {
                        ((TMapLevel)MapField.Owner).FMap.TerrainChangedRad(MapField.FXhx, MapField.FYhx, MapField.FLhx, 0);
                    }
                }

                FStatus = FStatus - TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moPlacedOnMap);
            }
        }

        // MAPOBJ.PAS:341
        public override bool MainPlace(TMapContainer MapContainer, int Xhp, int Yhp, int Lhx)
        {
            bool Result;
            int Xhx = default;
            int Yhx = default;
            if (!FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
            {
                Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
                if (GetEditMode() || CanPlace(MapContainer, (sbyte)Xhx, (sbyte)Yhx, (sbyte)Lhx))
                {
                    Result = PlaceHX(MapContainer, (sbyte)Xhx, (sbyte)Yhx, (sbyte)Lhx);
                    if (Result)
                    {
                        Activate();
                    }
                    else
                    {
                        MainRemove();
                    }
                }
                else
                {
                    Result = false;
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // MAPOBJ.PAS:363
        public override void MainRemove()
        {
            if (FStatus.Contains(TMapObjectStatusElement.moSelected))
            {
                Unselect();
            }

            if (FStatus.Contains(TMapObjectStatusElement.moActivated))
            {
                Deactivate();
            }

            if (FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
            {
                Remove();
            }
        }

        // MAPOBJ.PAS:206
        public override bool Place(TMapContainer MapContainer, int Xhp, int Yhp, int Lhx)
        {
            int Xhx = default;
            int Yhx = default;
            Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
            return PlaceHX(MapContainer, (sbyte)Xhx, (sbyte)Yhx, (sbyte)Lhx);
        }

        // MAPOBJ.PAS:291
        public override void Remove()
        {
            TMapField MapField;
            MapField = (TMapField)Owner;
            RemoveFromMap();
            if (GetVisible())
            {
                if (GetLevel() <= 15)
                {
                    ((TMapLevel)MapField.Owner).FMapContainer.FMap.FStaticVisibleHS.RemoveChild(this);
                }
                else
                {
                    ((TMapLevel)MapField.Owner).FMapContainer.FMap.FDynamicVisibleHS.RemoveChild(this);
                }

                SetVisible(false);
            }
        }

        // MAPOBJ.PAS:246
        public override bool Loaded()
        {
            Connect();
            return true;
        }

        // MAPOBJ.PAS:373
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 16)
            {
                if (GetTerrain(Msg.HN) >= 0 || Msg.HN == 0)
                {
                    Dispose();
                }
            }
        }

        // MAPOBJ.PAS:330
        public override void Activate()
        {
            FStatus = FStatus + TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moActivated);
        }

        // MAPOBJ.PAS:335
        public override void Deactivate()
        {
            base.Deactivate();
            FStatus = FStatus - TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moActivated);
        }

        // MAPOBJ.PAS:117
        public override void MakeVisible(TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(TMapObjectStatusElement.moVisible))
            {
                FStatus = FStatus + TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moVisible);
                VisibilityList.AddSortChild(this, GetShowPriority());
            }
        }

        // MAPOBJ.PAS:126
        public override bool CanSelect(int Xhp, int Yhp) => FResource.FEngine.FHSMap.FStatus.Contains(THSMapStatusElement.msEditting) && FResource.FTerrainList.FCount != 0 && (FResource.FTerrainList.GetTerrain(0) >= 0 || FResource.FTerrainList.GetOverlay(0) >= 0);

        // MAPOBJ.PAS:172
        public override void Select()
        {
            FStatus = FStatus + TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moSelected);
        }

        // MAPOBJ.PAS:177
        public override void Unselect()
        {
            base.Unselect();
            FStatus = FStatus - TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moSelected);
        }

        // MAPOBJ.PAS:21
        public override string EditName()
        {
            string Result;
            if (IsEmpty(FResource.FName))
            {
                Result = FResource.ResourceEditName();
            }
            else
            {
                Result = FResource.FName;
            }

            return Result;
        }

        // MAPOBJ.PAS:63
        public virtual TMapObjectResourceList GetResourceList(TEngine AEngine) => ((THSEngine)AEngine).FHSSet.FMapObjectResourceList;

        // MAPOBJ.PAS:68
        public virtual void SetResource(TMapObjectResource Value)
        {
            FResource = Value;
        }

        // MAPOBJ.PAS:307
        public virtual bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result;
            THexagonSprite HS;
            TMapField MapField;
            if (Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < MapContainer.FWidth && Yhx < MapContainer.FHeight && Lhx < MapContainer.FLevelCount)
            {
                MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
                HS = MapField.GetMaxLevelNoneTransparentHS();
                bool condition = HS == null || HS.GetLevel() < 10;
                if (condition)
                {
                    var overlay = GetOverlay(0);
                    condition = MapField.CanChangeTerrain(this, GetTerrain(0), overlay);
                }

                Result = condition;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // MAPOBJ.PAS:324
        public virtual bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            Owner = MapContainer.GetField(Xhx, Yhx, Lhx);
            return PlaceOnMap();
        }

        public TMapObjectResource Resource { get => FResource; set => SetResource(value); }
        public int ResourceID { get => GetResourceID(); set => SetResourceID(value); }
        public bool ResourceLinked { get => GetResourceLinked(); set => SetResourceLinked(value); }
        public TSet<TMapObjectStatusElement> Status { get => FStatus; }

    }

    // the resource of map objects. FEngine (set by the engine activation message),
    // FShowPriorityOffset (id 4), FImageLibrary (id 8), FImageSequenceList (nil without complex
    // images. ids 9, 10), FTerrainList (id 7), FLevel (id 5), FName (id 6)
    public partial class TMapObjectResource : TEResource
    {
        public new const int ClassID = 65876;

        public TMapObjectResource(TECustomNode owner) : base(owner)
        {
            FTerrainList = new TTerrainList();
            FImageLibrary = new TImageLibrary(null);
            SetLevel(26);
        }

        protected internal THSEngine FEngine;
        protected internal int FShowPriorityOffset;
        protected internal TImageLibrary FImageLibrary;
        protected internal TImageSequenceList FImageSequenceList;
        protected internal TTerrainList FTerrainList;
        protected internal byte FLevel;
        protected internal string FName = "";

        // MAPOBJ.PAS:391
        public override void Dispose()
        {
            FTerrainList?.Dispose();
            FImageLibrary?.Dispose();
            if (FImageSequenceList != null)
            {
                FImageSequenceList?.Dispose();
            }

            base.Dispose();
        }

        // MAPOBJ.PAS:405
        public override int ClassVersion() => 1;

        // MAPOBJ.PAS:400
        public override TEngine GetEngine() => FEngine;

        // MAPOBJ.PAS:470
        public override void ReadWrite(TEStorageStream Stream)
        {
            bool Complex = default;
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FShowPriorityOffset, 4);
            Stream.rwByte(ref FLevel, 5);
            Stream.rwString(ref FName, 6);
            Stream.rwStreamProc(7, FTerrainList.LoadFromStream, FTerrainList.SaveToStream);
            FImageLibrary.ReduceCount();
            Stream.rwStreamProc(8, FImageLibrary.LoadFromStream, FImageLibrary.SaveToStream);
            if (!Stream.StorageMode().Contains(Aow.Game.Engine.Nodes.EngineServices.smText))
            {
                if (Stream.StorageMode().Contains(TStorageModeElement.smRead))
                {
                    Stream.rwBool(ref Complex, 9);
                    if (Complex)
                    {
                        if (FImageSequenceList == null)
                        {
                            FImageSequenceList = new TImageSequenceList(null, FImageLibrary);
                        }
                    }
                    else
                    {
                        FImageSequenceList?.Dispose();
                        FImageSequenceList = null;
                    }
                }
                else
                {
                    Complex = FImageSequenceList != null;
                    Stream.rwBool(ref Complex, 9);
                }

                if (Complex)
                {
                    Stream.rwStreamProc(10, FImageSequenceList.LoadFromStream, FImageSequenceList.SaveToStream);
                }
            }
        }

        // MAPOBJ.PAS:383

        // MAPOBJ.PAS:620
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 536870920)
            {
                FEngine = (THSEngine)Msg.Payload.As<TEngine>();
                if (FImageSequenceList != null)
                {
                    FImageSequenceList.FILManager = FEngine.FHSSet.FImageLibraryManager;
                    FImageSequenceList.LinkToIL();
                }
            }
            else
            {
                if (Msg.MsgID == 65541)
                {
                    EditDropMsg(ref Msg);
                }
                else
                {
                    if (Msg.MsgID == 65621)
                    {
                        Msg.Result = 8;
                    }
                }
            }
        }

        // MAPOBJ.PAS:450
        public override Aow.Game.Draw.Images.TLibraryImage GetCellImage()
        {
            int I;
            I = 0;
            Aow.Game.Draw.Images.TLibraryImage Result = null;
            while (I != FImageLibrary.Count && Result == null)
            {
                Result = FImageLibrary.Get(I);
                I += 1;
            }

            if (Result == null && FImageSequenceList != null)
            {
                Result = FImageSequenceList.GetPreviewImage();
            }

            return Result;
        }

        // MAPOBJ.PAS:465
        public override void SetCellImage(Aow.Game.Draw.Images.TLibraryImage Value)
        {
            FImageLibrary.SetImage(0, Value);
        }

        // MAPOBJ.PAS:525
        public override TEObject CreateResourceUser()
        {
            int ID;
            TMapObject MO;
            ID = ResourceUserClassID();
            if (ID != 0)
            {
                MO = (TMapObject)FEngine.CreateEObject(ID, null);
                if (MO != null)
                {
                    MO.SetResource(this);
                    MO.SetResourceLinked(true);
                }
            }
            else
            {
                MO = null;
            }

            return MO;
        }

        // MAPOBJ.PAS:420
        public override void Draw(TCanvas Canvas, TRect Rect, Aow.Game.Draw.Images.TBltMode Mode, int ScaleX, int ScaleY, int BkColor, int Color)
        {
            int I;
            Aow.Game.Draw.Images.TLibraryImage Image;
            TImageSequence Sequence;
            TRect DrawRect = default;
            I = 0;
            Image = null;
            while (I != FImageLibrary.Count && Image == null)
            {
                Image = FImageLibrary.Get(I);
                I += 1;
            }

            if (Image == null && FImageSequenceList != null)
            {
                I = 0;
                Sequence = null;
                while (I != FImageSequenceList.Count && Sequence == null)
                {
                    Sequence = FImageSequenceList.GetImageSequence(I);
                    if (Sequence == null)
                    {
                        I += 1;
                    }
                }

                Aow.Game.World.TMapObjectResource.BltImageSequenceEx(Canvas, Sequence, 0, Rect, Mode, ScaleX, ScaleY, BkColor, Color, false, ref DrawRect);
            }
            else
            {
                Aow.Game.Draw.Images.LibraryDrawing.BltLibraryImage(Canvas, Image, Rect, Mode, ScaleX, ScaleY, BkColor, Color, false, ref DrawRect);
            }
        }

        // MAPOBJ.PAS:550
        public virtual void SetLevel(byte Value)
        {
            FLevel = Value;
            CalcShowPriorityOffset(16, 32, FLevel);
        }

        // MAPOBJ.PAS:556
        public virtual TMapObject PlaceResourceUser(THSMap HSMap, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            int Xhp = default;
            int Yhp = default;
            TMapObject Result = (TMapObject)CreateResourceUser();
            Aow.Game.World.Map.HexGeometry.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            Xhp += 16;
            Yhp += 16;
            if (!HSMap.Place(Result, Xhp, Yhp, Lhx))
            {
                Result?.Dispose();
                Result = null;
            }

            return Result;
        }

        // MAPOBJ.PAS:545
        public virtual void CalcShowPriorityOffset(int dX, int dY, int Level)
        {
            FShowPriorityOffset = dX + ((Level << 26) + (dY << 13));
        }

        // MAPOBJ.PAS:507
        public void ShowComplexImage(int X, int Y, in TSurfaceDesc SurfaceDesc, int Index, uint Offset)
        {
            TImageSequence Sequence;
            int Frame;
            if (Index < FImageSequenceList.Count)
            {
                Sequence = (TImageSequence)FImageSequenceList.List[Index];
            }
            else
            {
                Sequence = null;
            }

            while (Sequence != null)
            {
                Frame = Sequence.GetFrameIndex(FEngine.FHSMap.FFrame, (int)Offset);
                if (Frame != -1)
                {
                    Sequence.ImageLibrary.Get(Frame).Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                }

                Sequence = Sequence.Next;
            }
        }

        // MAPOBJ.PAS:571
        public void ClearTerrain(int X, int Y, int Lhx)
        {
            int Xhx = default;
            int Yhx = default;
            for (int I = FTerrainList.FCount - 1; I >= 0; I--)
            {
                if (FTerrainList.GetTerrain(I) >= 0)
                {
                    Aow.Game.World.Map.HexGeometry.CenterHNtoHX(X, Y, I, ref Xhx, ref Yhx);
                    FEngine.FHSMap.ClearTerrain(Xhx, Yhx, Lhx, 0, -1);
                }
            }
        }

        // MAPOBJ.PAS:586
        // { The resource was dropped on the map (message $10005): fill the hexagons in Param[1] rings around
        //   the drop point with its users, clearing the terrain first when Param[2] = 1 }
        public void EditDropMsg(ref TEMsg Msg)
        {
            TMapLevel MapLevel;
            int Xhx = default;
            int Yhx = default;
            int Lhx;
            int Rad;
            bool Clear;
            bool Placed;
            THNWalkData Walk = default;
            Placed = true;
            MapLevel = Msg.Param[0].As<TMapLevel>();
            Lhx = Msg.Data[1];
            Rad = Msg.Param[1].Int;
            Aow.Game.World.Map.HexGeometry.HPtoHX(Msg.Payload.Int, Msg.Data[0], ref Xhx, ref Yhx);
            Clear = Msg.Param[2].Int == 1;
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (Walk.Radius <= Rad)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    if (Clear)
                    {
                        ClearTerrain(Walk.X, Walk.Y, Lhx);
                    }

                    Placed = Placed && PlaceResourceUser(FEngine.FHSMap, (sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)Lhx) != null;
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }

            if (Placed)
            {
                Msg.Result = 0;
            }
            else
            {
                Msg.Result = -1;
            }
        }

        public THSEngine Engine { get => FEngine; }
        public int ShowPriorityOffset { get => FShowPriorityOffset; set => FShowPriorityOffset = value; }
        public TImageLibrary ImageLibrary { get => FImageLibrary; }
        public TImageSequenceList ImageSequenceList { get => FImageSequenceList; set => FImageSequenceList = value; }
        public TTerrainList TerrainList { get => FTerrainList; }
        public byte Level { get => FLevel; set => SetLevel(value); }
        public string Name { get => FName; set => FName = value; }

        // MAPOBJ.PAS:415
        public override string ResourceEditName() => "";

        internal static void BltImageSequenceEx(TCanvas Canvas, global::Aow.Game.Draw.Images.TImageSequence Sequence, int Frame, TRect Rect, global::Aow.Game.Draw.Images.TBltMode Mode, int ScaleX, int ScaleY, int BkColor, int Color, bool Selected, ref TRect DrawRect)
        {
            throw new NotImplementedException();
        }

    }

    public partial class TMapObjectResourceList : TEResourceList
    {
        public TMapObjectResourceList(TECustomNode owner) : base(owner)
        { }

        // MAPOBJ.PAS:641
        public TMapObjectResource GetMOR(int Index) => (TMapObjectResource)FChildren.List[Index];

        // MAPOBJ.PAS:646
        public bool FindMOR(int AClassID, TTerrainList TerrainList, TSet<TTerrainCompareModeElement> Mode, ref TList List)
        {
            TMapObjectResource R;
            int[] I__cell = new int[1];
            ref int I = ref I__cell[0];
            // { Preserve the original stack locals. }
            bool Result = false;
            I = 0;
            int last = FChildren.Count - 1;
            for (; I <= last; I += 1)
            {
                R = (TMapObjectResource)FChildren.List[I];
                if (R.EClass.ClassID == AClassID)
                {
                    if (TerrainList == null || R.FTerrainList.CompareTerrain(TerrainList, Mode))
                    {
                        Result = true;
                        List.Add(R);
                    }
                }
            }

            return Result;
        }

        public sealed class MORsAccessor(TMapObjectResourceList target)
        {
            public TMapObjectResource this[int Index] { get => target.GetMOR(Index); }
        }

        public MORsAccessor MORs => new MORsAccessor(this);

    }
}

namespace Aow.Game.World.Map
{

    // { A map location: hexagon column X, row Y, map level L and a flag byte (LocationsConnected
    //     marks the locations it has reached with it); the items of TXYLList, made by PackXYL }
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct TXYL
    {
        [FieldOffset(0)]
        public sbyte X;
        [FieldOffset(1)]
        public sbyte Y;
        [FieldOffset(2)]
        public sbyte L;
        [FieldOffset(3)]
        public sbyte Flag;
        [FieldOffset(0)]
        public ushort XY;
        [FieldOffset(0)]
        public int XYL;
    }

    // { a column and row offset (HNtoXYTable) }
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct XYTableEntry
    {
        [FieldOffset(0)]
        public int X;
        [FieldOffset(4)]
        public int Y;
    }
}

namespace Aow.Game.World.Movement
{

    // { State of a walk around a centre hexagon (InitHNWalk, WalkToNextHN): the hexagon numbers
    //     ring after ring, 6 * Radius hexagons on ring Radius }
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public struct THNWalkData
    {
        [FieldOffset(0)]
        public int Side;
        [FieldOffset(4)]
        public int Step;
        [FieldOffset(8)]
        public int HN;
        [FieldOffset(12)]
        public int Radius;
        [FieldOffset(16)]
        public int X;
        [FieldOffset(20)]
        public int Y;
        [FieldOffset(24)]
        public int CenterX;
        [FieldOffset(28)]
        public int CenterY;

        internal static void WalkToNextHN(ref THNWalkData walk)
        {
            walk.HN = walk.HN + 1;
            int parity = walk.X & 1;
            int side = walk.Side;
            walk.Step = walk.Step + 1;
            if (walk.Step < walk.Radius || side != 5)
            {
                walk.X = walk.X + Aow.Game.World.Map.HexGeometry.HNWalkTable[parity][side].X;
                walk.Y = walk.Y + Aow.Game.World.Map.HexGeometry.HNWalkTable[parity][side].Y;
                if (walk.Step >= walk.Radius)
                {
                    walk.Side = side + 1;
                    walk.Step = 0;
                }
            }
            else
            {
                walk.Radius = walk.Radius + 1;
                walk.Side = 0;
                walk.Step = 0;
                walk.X = walk.CenterX;
                walk.Y = walk.CenterY - walk.Radius;
            }
        }

        internal static void InitHNWalk(ref THNWalkData Walk, int Xhx, int Yhx, int Radius)
        {
            if (Radius == 0)
            {
                Walk.HN = 0;
                Walk.Side = 5;
            }
            else
            {
                Walk.HN = (Radius - 1) * Radius * 3 + 1;
                Walk.Side = 0;
            }

            Walk.CenterX = Xhx;
            Walk.CenterY = Yhx;
            Walk.Radius = Radius;
            Walk.X = Xhx;
            Walk.Y = Yhx - Radius;
            Walk.Step = 0;
        }

    }
}

namespace Aow.Game.World.Map
{

    [LegacyObject]
    // the parameters of a new map (THSMap.CreateNewMap)
    public partial class TNewHSMapSettings
    {

        public TNewHSMapSettings()
        {
            CaptureAddress.Assign(this);
            Width = 64;
            Height = 64;
            Levels = 1;
            HSSFileName = "";
            // { THSMap.ClassID }
            HSMapClassID = 66048;
            // { TMapContainer.ClassID }
            MapContainerClassID = 65795;
            // { TMapLevel.ClassID }
            MapLevelClassID = 65794;
        }

        public int Width;
        public int Height;
        public int Levels;
        public string HSSFileName = "";
        public int HSMapClassID;
        public int MapContainerClassID;
        public int MapLevelClassID;
        // HSEngine.pas:2802

    }

    [LegacyObject]
    // terrain and overlay types by hexagon number, two bytes per hexagon (terrain type, overlay
    // type. -2: none)
    public partial class TTerrainList : IDisposable
    {
        internal int FCount;
        internal byte[] FList = [];

        // HSEngine.pas:1411
        public virtual void Dispose()
        {
            Array.Resize(ref FList, 0);
        }

        // HSEngine.pas:1417
        public virtual void Assign(TTerrainList Source)
        {
            SetCount(Source.FCount);
            Array.Copy(Source.FList, 0, FList, 0, FCount * 2);
        }

        // HSEngine.pas:1562
        public virtual void LoadFromStream(Stream Stream)
        {
            Stream.ReadBytes(Bytes.Of(ref FCount, 4));
            SetCount(FCount);
            Stream.ReadBytes(FList.AsSpan(0, FCount * 2));
        }

        // HSEngine.pas:1569
        public virtual void SaveToStream(Stream Stream)
        {
            Pack();
            Stream.Write(Bytes.Of(ref FCount, 4));
            Stream.Write(FList.AsSpan(0, FCount * 2));
        }

        // HSEngine.pas:1423
        public sbyte GetTerrain(int Index)
        {
            sbyte Result;
            if (Index < FCount)
            {
                Result = MemoryMarshal.Read<sbyte>(FList.AsSpan(Index * 2, 1));
            }
            else
            {
                Result = -2;
            }

            return Result;
        }

        // HSEngine.pas:1431
        public void SetTerrain(int Index, sbyte Value)
        {
            if (Index >= FCount)
            {
                SetCount(Index + 1);
            }

            sbyte value__206 = Value;
            MemoryMarshal.Write(FList.AsSpan(Index * 2, 1), in value__206);
        }

        // HSEngine.pas:1437
        public void SetCount(int NewCount)
        {
            Array.Resize(ref FList, NewCount + NewCount);
            if (NewCount > FCount)
            {
                FillBytes(FList.AsSpan(FCount * 2, (NewCount - FCount) * 2), -2);
            }

            FCount = NewCount;
        }

        // HSEngine.pas:1445
        public sbyte GetOverlay(int Index)
        {
            sbyte Result;
            if (Index < FCount)
            {
                Result = MemoryMarshal.Read<sbyte>(FList.AsSpan(Index * 2 + 1, 1));
            }
            else
            {
                Result = -2;
            }

            return Result;
        }

        // HSEngine.pas:1453
        public void SetOverlay(int Index, sbyte Value)
        {
            if (Index >= FCount)
            {
                SetCount(Index + 1);
            }

            sbyte value__207 = Value;
            MemoryMarshal.Write(FList.AsSpan(Index * 2 + 1, 1), in value__207);
        }

        // HSEngine.pas:1459
        public ushort GetTandO(int Index)
        {
            ushort Result;
            if (Index < FCount)
            {
                Result = MemoryMarshal.Read<ushort>(FList.AsSpan(Index * 2, 2));
            }
            else
            {
                Result = 65278;
            }

            return Result;
        }

        // HSEngine.pas:1467
        public void SetTandO(int Index, ushort Value)
        {
            if (Index >= FCount)
            {
                SetCount(Index + 1);
            }

            ushort value__208 = Value;
            MemoryMarshal.Write(FList.AsSpan(Index * 2, 2), in value__208);
        }

        // HSEngine.pas:1473
        public bool GetUsed(int Index) => Index < FCount && (MemoryMarshal.Read<ushort>(FList.AsSpan(Index * 2, 2)) & 32896) != 32896;

        // HSEngine.pas:1478
        public int UsedTerrain()
        {
            int count = 0;
            for (int i = 0; i < FCount; i++)
                if ((sbyte)FList[i * 2 + 0] > -1) count++;
            return count;
        }

        // HSEngine.pas:1492
        public int UsedOverlays()
        {
            int count = 0;
            for (int i = 0; i < FCount; i++)
                if ((sbyte)FList[i * 2 + 1] > -1) count++;
            return count;
        }

        // HSEngine.pas:1506
        public void Pack()
        {
            int NewCount;
            NewCount = FCount;
            while (NewCount != 0 && GetOverlay(NewCount - 1) == -2 && GetTerrain(NewCount - 1) == -2)
            {
                NewCount -= 1;
            }

            if (NewCount != FCount)
            {
                SetCount(NewCount);
            }
        }

        // HSEngine.pas:1516
        public TWordList CreateSurroundingHNTable()
        {
            int HN;
            int Xhx = default;
            int Yhx = default;
            THNWalkData Walk = default;
            TWordList Result = new TWordList();
            for (int I = 0, last = FCount - 1; I <= last; I++)
            {
                if (GetTerrain(I) >= 0)
                {
                    Aow.Game.World.Map.HexGeometry.HNtoHX(I, ref Xhx, ref Yhx);
                    Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 1);
                    while (Walk.HN != 7)
                    {
                        HN = Aow.Game.World.Map.HexGeometry.HXtoHN(Walk.X, Walk.Y);
                        if (HN >= FCount || HN < FCount && GetTerrain(HN) < 0)
                        {
                            if (Result.IndexOf((ushort)HN) == -1)
                            {
                                Result.Add((ushort)HN);
                            }
                        }

                        Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
                    }
                }
            }

            return Result;
        }

        // HSEngine.pas:1538
        public bool CompareTerrain(TTerrainList List, TSet<TTerrainCompareModeElement> Mode)
        {
            int I;
            int N;
            int T1;
            int T2;
            int O1;
            int O2;
            bool Result = true;
            I = 0;
            if (FCount >= List.FCount)
            {
                N = FCount;
            }
            else
            {
                N = List.FCount;
            }

            while (Result && I != N)
            {
                T1 = GetTerrain(I);
                T2 = List.GetTerrain(I);
                if (!(T1 < 0 && T2 < 0))
                {
                    Result = T1 >= 0 && T2 >= 0 && (!Mode.Contains(TTerrainCompareModeElement.cmSameTerrainType) || T1 == T2);
                }

                if (Result)
                {
                    O1 = GetOverlay(I);
                    O2 = List.GetOverlay(I);
                    if (!(O1 < 0 && O2 < 0))
                    {
                        Result = !Mode.Contains(TTerrainCompareModeElement.cmSameOverlayType) || O1 == O2;
                    }
                }

                I += 1;
            }

            return Result;
        }

        public int Count { get => FCount; set => SetCount(value); }

        public sealed class TerrainAccessor(TTerrainList target)
        {
            public sbyte this[int Index] { get => target.GetTerrain(Index); set => target.SetTerrain(Index, value); }
        }

        public TerrainAccessor Terrain => new TerrainAccessor(this);

        public sealed class OverlayAccessor(TTerrainList target)
        {
            public sbyte this[int Index] { get => target.GetOverlay(Index); set => target.SetOverlay(Index, value); }
        }

        public OverlayAccessor Overlay => new OverlayAccessor(this);

        public sealed class TandOAccessor(TTerrainList target)
        {
            public ushort this[int Index] { get => target.GetTandO(Index); set => target.SetTandO(Index, value); }
        }

        public TandOAccessor TandO => new TandOAccessor(this);

        public sealed class UsedAccessor(TTerrainList target)
        {
            public bool this[int Index] { get => target.GetUsed(Index); }
        }

        public UsedAccessor Used => new UsedAccessor(this);

        public TTerrainList()
        {
            CaptureAddress.Assign(this);
        }

    }

    // { The records a map field passes by reference to the methods of its sprites. }
    // { THexagonSprite.UpdateMapField (8 bytes) }
    public struct THSMapFieldInfo
    {
        public TMapField MapField;
        public sbyte HN;
    }

    // { THexagonSprite.CanChangeTerrain, CanMoveOver, MoveExclusive (12 bytes): Sender is the object
    //     that asks (the sprite that changes the terrain, the mover) }
    public struct THSSenderInfo
    {
        public object Sender;
        public TMapField MapField;
        public sbyte HN;
    }

    // { THexagonSprite.ChangeTerrain, TerrainChanged, NeighbourTerrainChanged (12 bytes) }
    public struct THSTerrainInfo
    {
        public sbyte TerrainType;
        public sbyte OverlayType;
        public TMapField MapField;
        public sbyte HN;
    }
}

namespace Aow.Game.World.Movement
{

    // { THexagonSprite.CanMoveOn (16 and 12 bytes) }
    public struct THSMoveOnInfo
    {
        public object Sender;
        public TMapField MapField;
        public int MoveOverCode;
        public sbyte HN;
    }

    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct THSMoveOnResult
    {
        [FieldOffset(0)]
        public int Code;
        [FieldOffset(4)]
        public int Data;
        [FieldOffset(8)]
        public bool Done;
    }
}

namespace Aow.Game.World.Map
{

    public struct TMapFieldMsg
    {
        public int MsgID;
        public TMapField MapField;
        public TMapFieldMsgParams Param;
        public int HN;
        public MsgArg Result;
    }

    // The parameter slots of a map field message. The count is the arity of the message protocol
    // (HSEngine.pas TMapFieldMsg.Param), not a count of content.
    [InlineArray(Arity)]
    public struct TMapFieldMsgParams
    {
        public const int Arity = 4;

        private MsgArg element0;
    }

    // A map level owns its fields in row order. The fields are its children.
    // Field size and neighbour offsets keep the recovered layout values.
    public partial class TMapLevel : TECustomNode
    {
        // HSEngine.pas:2837
        public new const int ClassID = 65794;

        public TMapLevel(TECustomNode owner) : base(owner)
        {
            FMapFieldCID = 65793;
        }

        protected internal int FLhx;
        protected internal int FWidth;
        protected internal int FHeight;
        protected internal int FCount;
        protected internal TMapContainer FMapContainer;
        protected internal THSMap FMap;
        protected internal TEngine FEngine;
        protected internal int FMapFieldCID;
        protected internal int FFieldSize;
        protected internal int FFieldsSize;
        public FixedArray_Aow_Units_FixedArray_int_8_2 FHNOffsets;

        // HSEngine.pas:2925
        public override TEngine GetEngine() => FEngine;

        // HSEngine.pas:2913
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            if (Msg.MsgID == 536870920)
            {
                FEngine = Msg.Payload.As<TEngine>();
                FMapContainer = (TMapContainer)Owner;
                FMap = FMapContainer.FMap;
            }
            else
            {
                if (Msg.MsgID == 536936471)
                {
                    DestroyHS();
                }
                else
                {
                    base.MsgProc(ref Msg, Sender);
                }
            }
        }

        // HSEngine.pas:2935
        public override void SetChild(int Index, TEObject Child)
        {
            throw new InvalidOperationException("Cannot set child in TMapLevel");
        }

        // HSEngine.pas:2950
        public override int GetCount() => FCount;

        // HSEngine.pas:2945
        public override int RemoveChild(TEObject Child)
        {
            throw new InvalidOperationException("Cannot remove from TMapLevel");
        }

        // HSEngine.pas:2940
        public override int AddChild(TEObject Child)
        {
            throw new InvalidOperationException("Cannot add child to TMapLevel");
        }

        // HSEngine.pas:3140
        public virtual void New(TNewHSMapSettings Settings)
        {
            Clear();
            try
            {
                CreateMapFieldList(Settings.Width, Settings.Height);
            }
            catch
            {
                Clear();
                throw;
            }
        }

        // HSEngine.pas:2842
        public virtual EClass MapFieldClass() => EClass.Of<TMapField>();

        public int Lhx { get => FLhx; set => FLhx = value; }
        public int Width { get => FWidth; }
        public int Height { get => FHeight; }
        public TMapContainer MapContainer { get => FMapContainer; }
        public THSMap Map { get => FMap; }
        public int MapFieldCID { get => FMapFieldCID; set => FMapFieldCID = value; }
        public int FieldSize { get => FFieldSize; }
        public ReadOnlySpan<TMapField> Fields => fields;

        public sealed class FieldAccessor(TMapLevel target)
        {
            public TMapField this[int X, int Y] { get => target.GetField(X, Y); }
        }

        public FieldAccessor Field => new FieldAccessor(this);

    }

    // the selected sprites of a map. The children (TENode.FChildren,) are every selected sprite.
    // FLocationHS the sprites selected by location (clicking on a hexagon), FOnSelect, FOnDeselect
    // (triggered with the map as the sender), FEventHS the sprite of the last event. The owner is
    // the map
    public partial class THexagonSpriteSelection : TEControlStyleNode
    {
        internal TList FLocationHS;

        internal EventStore<TECustomNode> FOnSelectStore;

        internal event TNotifyEvent<TECustomNode> FOnSelect
        {
            add => FOnSelectStore.Add(value);
            remove => FOnSelectStore.Remove(value);
        }

        internal EventStore<TECustomNode> FOnDeselectStore;

        internal event TNotifyEvent<TECustomNode> FOnDeselect
        {
            add => FOnDeselectStore.Add(value);
            remove => FOnDeselectStore.Remove(value);
        }

        internal THexagonSprite FEventHS;

        // HSEngine.pas:3681
        public THexagonSpriteSelection(TECustomNode owner) : base(owner)
        {
            FLocationHS = new TList();
            FOnSelectStore = new();
            FOnDeselectStore = new();
        }

        // HSEngine.pas:3689
        public override void Dispose()
        {
            base.Dispose();
            FLocationHS?.Dispose();
            FOnSelectStore.Clear();
            FOnDeselectStore.Clear();
        }

        // HSEngine.pas:3761
        public override void Clear()
        {
            FChildren.Clear();
            FLocationHS.Clear();
        }

        // HSEngine.pas:3699
        public void TriggerDeselectEvent(THexagonSprite HS)
        {
            FEventHS = HS;
            FOnDeselectStore.Raise(Owner);
        }

        // HSEngine.pas:3705
        public void TriggerSelectEvent(THexagonSprite HS)
        {
            FEventHS = HS;
            FOnSelectStore.Raise(Owner);
        }

        // HSEngine.pas:3711
        public void Unselect(THexagonSprite HS)
        {
            int I;
            I = FChildren.IndexOf(HS);
            FLocationHS.Remove(HS);
            if (I != -1)
            {
                FChildren.Remove(HS);
                if (FChildren.IndexOf(HS) == -1)
                {
                    if (HS.GetSelected())
                    {
                        HS.Unselect();
                    }
                }

                TriggerDeselectEvent(HS);
            }
        }

        // HSEngine.pas:3727
        public void Select(THexagonSprite HS)
        {
            FChildren.Add(HS);
            HS.Select();
            TriggerSelectEvent(HS);
        }

        // HSEngine.pas:3734
        public void UnselectAll()
        {
            while (FChildren.Count != 0)
            {
                Unselect((THexagonSprite)FChildren.List[FChildren.Count - 1]);
            }
        }

        // HSEngine.pas:3740
        public THexagonSprite FindClass(Type AClass)
        {
            THexagonSprite HS;
            int I;
            THexagonSprite Result = null;
            I = 0;
            while (I != Count && Result == null)
            {
                HS = (THexagonSprite)FChildren.List[I];
                if (AClass.IsInstanceOfType(HS))
                {
                    Result = (THexagonSprite)FChildren.List[I];
                }

                I += 1;
            }

            return Result;
        }

        // HSEngine.pas:3756
        public THexagonSprite GetHS(int Index) => (THexagonSprite)FChildren.List[Index];

        // HSEngine.pas:3790
        public void SelectLocationHS(THexagonSprite HS)
        {
            FLocationHS.Add(HS);
            Select(HS);
        }

        // HSEngine.pas:3829
        public void SelectLocationHX(int Xhx, int Yhx, int Lhx)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            SelectLocation(Xhp, Yhp, Lhx);
        }

        // HSEngine.pas:3837
        public void UnselectLocation()
        {
            while (FLocationHS.Count != 0)
            {
                Unselect((THexagonSprite)FLocationHS.Last());
            }
        }

        public sealed class HSAccessor(THexagonSpriteSelection target)
        {
            public THexagonSprite this[int Index] { get => target.GetHS(Index); }
        }

        public HSAccessor HS => new HSAccessor(this);
        public TList LocationHS { get => FLocationHS; }

        public event TNotifyEvent<TECustomNode> OnSelect
        {
            add => FOnSelectStore.Add(value);
            remove => FOnSelectStore.Remove(value);
        }

        public event TNotifyEvent<TECustomNode> OnDeselect
        {
            add => FOnDeselectStore.Add(value);
            remove => FOnDeselectStore.Remove(value);
        }

        public THexagonSprite EventHS { get => FEventHS; }

    }

    [LegacyObject]
    // the mouse of a map. FHSMap, FX, FY (the position in the view port in pixels, Low(Integer)
    // when outside), FButton, FOnUp, FOnDown, FOnMove, FOnDblClick, FOnCaptureChanged, FCapture
    // (the object that gets the mouse events), FStatus, FCursor, FCursorMode, FIgnoreDown, FShift,
    // FCursorVisible (and are used by the game only)
    public partial class THSMapMouse : IDisposable
    {

        public THSMapMouse()
        {
            CaptureAddress.Assign(this);
            FOnUpStore = new();
            FOnDownStore = new();
            FOnMoveStore = new();
            FOnDblClickStore = new();
            FOnCaptureChangedStore = new();
            Reset();
        }

        internal THSMap FHSMap;
        internal int FX;
        internal int FY;
        internal TMouseButton FButton;

        internal EventStore<THSMapMouse> FOnUpStore;

        internal event TNotifyEvent<THSMapMouse> FOnUp
        {
            add => FOnUpStore.Add(value);
            remove => FOnUpStore.Remove(value);
        }

        internal EventStore<THSMapMouse> FOnDownStore;

        internal event TNotifyEvent<THSMapMouse> FOnDown
        {
            add => FOnDownStore.Add(value);
            remove => FOnDownStore.Remove(value);
        }

        internal EventStore<THSMapMouse> FOnMoveStore;

        internal event TNotifyEvent<THSMapMouse> FOnMove
        {
            add => FOnMoveStore.Add(value);
            remove => FOnMoveStore.Remove(value);
        }

        internal EventStore<THSMapMouse> FOnDblClickStore;

        internal event TNotifyEvent<THSMapMouse> FOnDblClick
        {
            add => FOnDblClickStore.Add(value);
            remove => FOnDblClickStore.Remove(value);
        }

        internal EventStore<THSMapMouse> FOnCaptureChangedStore;

        internal event TNotifyEvent<THSMapMouse> FOnCaptureChanged
        {
            add => FOnCaptureChangedStore.Add(value);
            remove => FOnCaptureChangedStore.Remove(value);
        }

        internal object FCapture;
        internal TSet<TMouseStatusElement> FStatus;
        internal int FCursor;
        internal int FCursorMode;
        internal bool FIgnoreDown;
        internal TSet<TShiftState> FShift;
        internal bool FCursorVisible;

        // HSEngine.pas:3475

        // HSEngine.pas:3486
        public virtual void Dispose()
        {
            FOnUpStore.Clear();
            FOnDownStore.Clear();
            FOnMoveStore.Clear();
            FOnDblClickStore.Clear();
            FOnCaptureChangedStore.Clear();
        }

        // HSEngine.pas:3505
        public virtual int GetCursor() => FCursor;

        // HSEngine.pas:3510
        public virtual void SetCursor(int Value)
        {
            FCursor = Value;
        }

        // HSEngine.pas:3583
        public virtual bool MouseDown()
        {
            bool Result = true;
            if (FCapture == null && GetXhx() != -1)
            {
                if (FButton == TMouseButton.mbLeft)
                {
                    var yhp = GetYhp();
                    var xhp = GetXhp();
                    FHSMap.FSelection.SelectLocation(xhp, yhp, FHSMap.FViewL);
                    Result = false;
                }
                else
                {
                    if (FButton == TMouseButton.mbRight && !FHSMap.FStatus.Contains(THSMapStatusElement.msEditting))
                    {
                        FHSMap.FSelection.UnselectLocation();
                        Result = false;
                    }
                }
            }

            return Result;
        }

        // HSEngine.pas:3623
        public virtual bool MouseUp() => true;

        // HSEngine.pas:3646
        public virtual bool MouseMove() => true;

        // HSEngine.pas:3667
        public virtual bool MouseDblClick() => true;

        // HSEngine.pas:3600
        // { Down and Up set FX twice when the position is outside (an original slip for FY). }
        public virtual void Down(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (!FIgnoreDown)
            {
                if (X >= 0 && Y >= 0)
                {
                    FX = X;
                    FY = Y;
                }
                else
                {
                    FX = -2147483648;
                    FX = -2147483648;
                }

                FButton = Button;
                FShift = Shift;
                if (MouseDown())
                {
                    FOnDownStore.Raise(this);
                }
            }
            else
            {
                FIgnoreDown = false;
            }
        }

        // HSEngine.pas:3628
        public virtual void Up(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (X >= 0 && Y >= 0)
            {
                FX = X;
                FY = Y;
            }
            else
            {
                FX = -2147483648;
                FX = -2147483648;
            }

            FButton = Button;
            FShift = Shift;
            if (MouseUp())
            {
                FOnUpStore.Raise(this);
            }
        }

        // HSEngine.pas:3651
        public virtual void Move(TSet<TShiftState> Shift, int X, int Y)
        {
            // Port extension: a view wider or taller than the map takes scene points past MaxXhp or MaxYhp.
            // GetXhp and GetYhp still test the map coordinates.
            if (X >= 0 && Y >= 0 && X <= Math.Max(FHSMap.GetMaxXhp(), FHSMap.FViewWidth) && Y <= Math.Max(FHSMap.GetMaxYhp(), FHSMap.FViewHeight))
            {
                FX = X;
                FY = Y;
            }
            else
            {
                FX = -2147483648;
                FY = -2147483648;
            }

            if (MouseMove())
            {
                FOnMoveStore.Raise(this);
            }
        }

        // HSEngine.pas:3672
        public virtual void DblClick()
        {
            if (GetXhp() != -2147483648)
            {
                if (MouseDblClick())
                {
                    FOnDblClickStore.Raise(this);
                }
            }
        }

        // HSEngine.pas:3496
        public void Reset()
        {
            FX = -2147483648;
            FY = -2147483648;
            FShift = TSet<TShiftState>.Of();
            FCursor = 0;
            SetCapture(null);
        }

        // HSEngine.pas:3515
        public int GetXhp()
        {
            int Result;
            if (FX != -2147483648)
            {
                Result = FHSMap.FViewXhp + FX;
                if (Result > FHSMap.GetMaxXhp() || Result < 0)
                {
                    Result = -2147483648;
                }
            }
            else
            {
                Result = -2147483648;
            }

            return Result;
        }

        // HSEngine.pas:3527
        public int GetYhp()
        {
            int Result;
            if (FY != -2147483648)
            {
                Result = FHSMap.FViewYhp + FY;
                if (Result > FHSMap.GetMaxYhp() || Result < 0)
                {
                    Result = -2147483648;
                }
            }
            else
            {
                Result = -2147483648;
            }

            return Result;
        }

        // HSEngine.pas:3540
        // { Both GetXhx and GetYhx test the result against the width and the height of the map. }
        public int GetXhx()
        {
            int Result = default;
            int Y = default;
            if (FX != -2147483648)
            {
                Aow.Game.World.Map.HexGeometry.HPtoHX(FHSMap.FViewXhp + FX, 0, ref Result, ref Y);
                if (Result < 0 || FHSMap.FMapContainer.FWidth <= Result || FHSMap.FMapContainer.FHeight <= Result)
                {
                    Result = -1;
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // HSEngine.pas:3555
        public int GetYhx()
        {
            int Result = default;
            int X = default;
            if (FY != -2147483648)
            {
                Aow.Game.World.Map.HexGeometry.HPtoHX(FHSMap.FViewXhp + FX, FHSMap.FViewYhp + FY, ref X, ref Result);
                if (Result < 0 || FHSMap.FMapContainer.FWidth <= Result || FHSMap.FMapContainer.FHeight <= Result)
                {
                    Result = -1;
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // HSEngine.pas:3570
        public void SetCapture(object Value)
        {
            if (!FStatus.Contains(TMouseStatusElement.msChangingCapture) && Value != FCapture)
            {
                FStatus = FStatus + TSet<TMouseStatusElement>.Of(TMouseStatusElement.msChangingCapture);
                FCapture = Value;
                if (Value == null)
                {
                    FCursor = 0;
                }

                FOnCaptureChangedStore.Raise(this);
                FStatus = FStatus - TSet<TMouseStatusElement>.Of(TMouseStatusElement.msChangingCapture);
            }
        }

        public THSMap HSMap { get => FHSMap; }
        public int X { get => FX; }
        public int Y { get => FY; }
        public int Xhp { get => GetXhp(); }
        public int Yhp { get => GetYhp(); }
        public int Xhx { get => GetXhx(); }
        public int Yhx { get => GetYhx(); }
        public TMouseButton Button { get => FButton; }
        public TSet<TShiftState> Shift { get => FShift; }

        public event TNotifyEvent<THSMapMouse> OnUp
        {
            add => FOnUpStore.Add(value);
            remove => FOnUpStore.Remove(value);
        }

        public event TNotifyEvent<THSMapMouse> OnDown
        {
            add => FOnDownStore.Add(value);
            remove => FOnDownStore.Remove(value);
        }

        public event TNotifyEvent<THSMapMouse> OnMove
        {
            add => FOnMoveStore.Add(value);
            remove => FOnMoveStore.Remove(value);
        }

        public event TNotifyEvent<THSMapMouse> OnDblClick
        {
            add => FOnDblClickStore.Add(value);
            remove => FOnDblClickStore.Remove(value);
        }

        public event TNotifyEvent<THSMapMouse> OnCaptureChanged
        {
            add => FOnCaptureChangedStore.Add(value);
            remove => FOnCaptureChangedStore.Remove(value);
        }

        public object Capture { get => FCapture; set => SetCapture(value); }
        public int Cursor { get => GetCursor(); set => SetCursor(value); }
        public int CursorMode { get => FCursorMode; set => FCursorMode = value; }
        public bool IgnoreDown { get => FIgnoreDown; set => FIgnoreDown = value; }
        public bool CursorVisible { get => FCursorVisible; set => FCursorVisible = value; }

    }

    public delegate void TCenterViewEvent(int Xhx, int Yhx, int Lhx);
    public delegate void TChangeViewEvent(int Xhp, int Yhp, int Lhp);
    public delegate void TViewLevelEvent(int Lhx);

    public enum TMapProgressType : byte
    {
        ptLoading = 0,
        ptSaving = 1,
        ptInitializing = 2,
        ptUpdatingTerrain = 3,
        ptActivating = 4,
        ptDestroying = 5
    }

    public delegate void TChangeTerrainProc(TMapField MapF, ref sbyte TerrainType, ref sbyte OverlayType);
    public delegate void TTerrainChangedProc(TMapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType);

    // { The record of THSMap.NotifyProgress (TMapLevel.Loaded fills one): the kind, the percentage
    //     done of the level and the level index. }
    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct TMapProgress
    {
        [FieldOffset(0)]
        public TMapProgressType ProgressType;
        [FieldOffset(4)]
        public int Percent;
        [FieldOffset(8)]
        public int Level;
    }

    public struct TTerrainChangedInfo
    {
        public TRect Rect;
        public int Level;
        public TMapLevel MapLevel;
    }

    // shows a map on a surface. FStaticSurface (the static scene, kept between frames),
    // FSceneWidth, FSceneHeight, FSceneX, FSceneY, FSceneL, FStaticSurfaceValid, FOnSceneChanged
    // (published), FHSMap. SetHSMap points the map's OnCenterView, OnChangeView and OnViewLevel at
    // CenterScene, ChangeScene and SetSceneL
    public partial class THSMapViewer : TComponent
    {
        internal TSurface FStaticSurface;
        internal int FSceneWidth;
        internal int FSceneHeight;
        internal int FSceneX;
        internal int FSceneY;
        internal int FSceneL;
        internal bool FStaticSurfaceValid;
        internal TNotifyEvent<object> FOnSceneChanged;
        internal THSMap FHSMap;
        // Port extension: the scene drawn under the original clamp before ShowCenteredMap centers it.
        internal TSurface FCenteredSurface;

        // HSEngine.pas:4670
        public THSMapViewer(TComponent owner) : base(owner)
        {
            FStaticSurfaceValid = false;
            FOnSceneChanged = null;
        }

        // HSEngine.pas:4678
        // { Leaves the map's OnChangeView set (original slip). }
        public override void Dispose()
        {
            if (FHSMap != null)
            {
                FHSMap.FOnCenterView = null;
                FHSMap.FOnViewLevel = null;
            }

            FStaticSurface?.Dispose();
            FCenteredSurface?.Dispose();
            base.Dispose();
        }

        // HSEngine.pas:4718
        public virtual void SetHSMap(THSMap Value)
        {
            FHSMap = Value;
            if (FHSMap != null)
            {
                // { Install viewer callbacks on the assigned map. }
                FHSMap.FOnCenterView = CenterScene;
                FHSMap.FOnChangeView = ChangeScene;
                FHSMap.FOnViewLevel = SetSceneL;
            }
        }

        // HSEngine.pas:4794
        public virtual void VerifyStaticSurface(TSurface Surface)
        {
            bool IsDDraw;
            TSet<TDDSCreationFlagsElement> Flags;
            IsDDraw = FStaticSurface is TDDrawSurface;
            if (FStaticSurface != null && (FStaticSurface.GetPixelFormatID() != Surface.GetPixelFormatID() || (Surface is TDDrawSurface) ^ IsDDraw))
            {
                FStaticSurface?.Dispose();
                FStaticSurface = null;
            }

            if (FStaticSurface == null || FStaticSurface.GetWidth() != FSceneWidth || FStaticSurface.GetHeight() != FSceneHeight)
            {
                if (FStaticSurface == null)
                {
                    if (Surface is TDDrawSurface)
                    {
                        if (((TDDrawSurface)Surface).Status.Contains(TDDrawSurfaceStatusElement.ssVideoMemory))
                        {
                            Flags = TSet<TDDSCreationFlagsElement>.Of(TDDSCreationFlagsElement.cfVideoMemory, TDDSCreationFlagsElement.cfSystemMemory);
                        }
                        else
                        {
                            Flags = TSet<TDDSCreationFlagsElement>.Of(TDDSCreationFlagsElement.cfSystemMemory);
                        }

                        FStaticSurface = new TDDrawSurface();
                        var pixelFormatID = Surface.GetPixelFormatID();
                        ((TDDrawSurface)FStaticSurface).CreateSurface(FSceneWidth, FSceneHeight, pixelFormatID, Flags);
                    }
                    else
                    {
                        var pixelFormatID = Surface.GetPixelFormatID();
                        FStaticSurface = new TDataSurface(FSceneWidth, FSceneHeight, pixelFormatID, null);
                    }
                }
                else
                {
                    FStaticSurface.Resize(FSceneWidth, FSceneHeight);
                }

                IsDDraw = FStaticSurface is TDDrawSurface;
                FHSMap.FStaticSceneInvalid = true;
                FStaticSurfaceValid = false;
            }

            if (IsDDraw && ((TDDrawSurface)FStaticSurface).Lost())
            {
                ((TDDrawSurface)FStaticSurface).Restore();
                FStaticSurfaceValid = false;
            }
        }

        // HSEngine.pas:4689
        public void ChangeScene(int Xhp, int Yhp, int Lhp)
        {
            if (FHSMap != null)
            {
                SetSceneX(Xhp);
                SetSceneY(Yhp);
                SetSceneL(Lhp);
                if (FHSMap.SetViewPort(FSceneX, FSceneY, FSceneL, FSceneWidth, FSceneHeight) && FOnSceneChanged != null)
                {
                    FOnSceneChanged(this);
                }
            }
        }

        // HSEngine.pas:4702
        public void CenterScene(int Xhx, int Yhx, int Lhx)
        {
            int Xhp = default;
            int Yhp = default;
            if (FHSMap != null)
            {
                Aow.Game.World.Map.HexGeometry.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
                SetSceneX(Xhp + 16 - (int)(((uint)FSceneWidth) >> 1));
                SetSceneY(Yhp + 16 - (int)(((uint)FSceneHeight) >> 1));
                SetSceneL(Lhx);
                if (FHSMap.SetViewPort(FSceneX, FSceneY, FSceneL, FSceneWidth, FSceneHeight) && FOnSceneChanged != null)
                {
                    FOnSceneChanged(this);
                }
            }
        }

        // HSEngine.pas:4730
        public void SetSceneL(int Value)
        {
            if (FHSMap != null)
            {
                if (Value < 0)
                {
                    Value = 0;
                }

                if (Value >= FHSMap.FMapContainer.FLevelCount)
                {
                    Value = FHSMap.FMapContainer.FLevelCount - 1;
                }

                FSceneL = Value;
            }
        }

        // HSEngine.pas:4742
        public void SetSceneX(int Value)
        {
            if (FHSMap != null)
            {
                if (Value < 16)
                {
                    Value = 16;
                }

                if (this.FHSMap.GetMaxXhp() < FSceneWidth + Value + 16)
                {
                    this.FSceneX = this.FHSMap.GetMaxXhp() - FSceneWidth - 16;
                }
                else
                {
                    this.FSceneX = Value;
                }
            }
        }

        // HSEngine.pas:4756
        public void SetSceneY(int Value)
        {
            if (FHSMap != null)
            {
                if (Value < 16)
                {
                    Value = 16;
                }

                if (this.FHSMap.GetMaxYhp() - 16 < FSceneHeight + Value)
                {
                    this.FSceneY = this.FHSMap.GetMaxYhp() - FSceneHeight - 16;
                }
                else
                {
                    this.FSceneY = Value;
                }
            }
        }

        // HSEngine.pas:4770
        public void SetSceneWidth(int Value)
        {
            THSMap M;
            M = FHSMap;
            if (M != null && Value > 0)
            {
                FSceneWidth = Value;
                SetSceneX(FSceneX);
            }
        }

        // HSEngine.pas:4782
        public void SetSceneHeight(int Value)
        {
            THSMap M;
            M = FHSMap;
            if (M != null && Value > 0)
            {
                FSceneHeight = Value;
                SetSceneY(FSceneY);
            }
        }

        // HSEngine.pas:4837
        public void ResetStaticSurface()
        {
            FStaticSurface?.Dispose();
            FStaticSurface = null;
        }

        // HSEngine.pas:4843
        public void ShowScene(int X, int Y, TSurface Surface)
        {
            THSMap M;
            object MP;
            TSurfaceDesc SD = default;
            TRect R = default;
            TSurface Target = Surface;
            int TargetX = X;
            int TargetY = Y;
            try
            {
                MP = FHSMap;
                if (MP != null)
                {
                    M = (THSMap)MP;
                    if (M.SetViewPort(FSceneX, FSceneY, FSceneL, FSceneWidth, FSceneHeight) && FOnSceneChanged != null)
                    {
                        FOnSceneChanged(this);
                    }

                    // Port extension: a scene larger than the map draws as the original does into
                    // FCenteredSurface, then ShowCenteredMap centers the map on Target and fills the rest black.
                    if (GetSceneExceedsMap())
                    {
                        Surface = VerifyCenteredSurface(Target);
                        X = 0;
                        Y = 0;
                    }
                    else if (FCenteredSurface != null)
                    {
                        FCenteredSurface?.Dispose();
                        FCenteredSurface = null;
                    }

                    VerifyStaticSurface(Surface);
                    R.Left = X;
                    R.Right = FSceneWidth + X;
                    R.Top = Y;
                    R.Bottom = FSceneHeight + Y;
                    if (M.FStaticSceneInvalid)
                    {
                        M.UpdateStaticScene();
                        M.UpdateDynamicScene();
                        if (Surface.LockEx(ref SD, R) >= 0)
                        {
                            M.DrawStaticScene(ref SD);
                            M.DrawDynamicScene(ref SD);
                            Surface.UnlockEx(SD);
                        }

                        FStaticSurfaceValid = false;
                    }
                    else
                    {
                        if (FStaticSurfaceValid)
                        {
                            FStaticSurface.BltClipped(Surface, X, Y);
                            M.UpdateDynamicScene();
                            if (Surface.LockEx(ref SD, R) >= 0)
                            {
                                M.DrawDynamicScene(ref SD);
                                Surface.UnlockEx(SD);
                            }
                        }
                        else
                        {
                            M.UpdateStaticScene();
                            if (FStaticSurface.Lock(ref SD) >= 0)
                            {
                                M.DrawStaticScene(ref SD);
                                FStaticSurface.Unlock();
                                FStaticSurfaceValid = true;
                            }

                            if (FStaticSurface.BltClipped(Surface, X, Y) >= 0)
                            {
                                M.UpdateDynamicScene();
                                if (Surface.LockEx(ref SD, R) >= 0)
                                {
                                    M.DrawDynamicScene(ref SD);
                                    Surface.UnlockEx(SD);
                                }
                            }
                        }
                    }

                    if (Surface != Target)
                    {
                        ShowCenteredMap(Target, TargetX, TargetY);
                    }
                }
            }
            catch
            {
                ShowMessage("Exception during MapViewer.ShowScene");
            }
        }

        // Port extension: the original never showed a scene wider or taller than the map. SceneX and
        // SceneY keep the original clamp there, so the view port and the saved view stay the original's.
        public bool GetSceneExceedsMap() => FHSMap != null && (FSceneWidth > FHSMap.GetMaxXhp() - 32 || FSceneHeight > FHSMap.GetMaxYhp() - 32);

        // Port extension: the part of the scene that shows the map under the original clamp. On an axis
        // where the scene fits inside the map it spans the scene.
        public TRect GetMapSceneRect()
        {
            TRect R = default;
            if (FHSMap != null)
            {
                R.Left = Math.Max(16 - FSceneX, 0);
                R.Top = Math.Max(16 - FSceneY, 0);
                R.Right = Math.Min(FHSMap.GetMaxXhp() - 16 - FSceneX, FSceneWidth);
                R.Bottom = Math.Min(FHSMap.GetMaxYhp() - 16 - FSceneY, FSceneHeight);
            }

            return R;
        }

        // Port extension: where ShowScene shows the map, centered in the scene. It equals
        // GetMapSceneRect when the scene fits inside the map.
        public TRect GetMapDisplayRect()
        {
            TRect R = GetMapSceneRect();
            int Width = R.Right - R.Left;
            int Height = R.Bottom - R.Top;
            R.Left = (FSceneWidth - Width) >> 1;
            R.Top = (FSceneHeight - Height) >> 1;
            R.Right = R.Left + Width;
            R.Bottom = R.Top + Height;
            return R;
        }

        // Port extension: turns a point of the shown scene into the scene point of the original clamp,
        // which the map mouse takes. A point outside the centered map gives false.
        public bool SceneToMapMouse(ref int X, ref int Y)
        {
            TRect Shown;
            TRect Source;
            if (!GetSceneExceedsMap())
            {
                return true;
            }

            Shown = GetMapDisplayRect();
            if (X < Shown.Left || X >= Shown.Right || Y < Shown.Top || Y >= Shown.Bottom)
            {
                return false;
            }

            Source = GetMapSceneRect();
            X += Source.Left - Shown.Left;
            Y += Source.Top - Shown.Top;
            return true;
        }

        // Port extension: the scene-sized surface that ShowScene draws on when the scene exceeds the map.
        public TSurface VerifyCenteredSurface(TSurface Surface)
        {
            if (FCenteredSurface != null && FCenteredSurface.GetPixelFormatID() != Surface.GetPixelFormatID())
            {
                FCenteredSurface?.Dispose();
                FCenteredSurface = null;
            }

            if (FCenteredSurface == null)
            {
                FCenteredSurface = new TDataSurface(FSceneWidth, FSceneHeight, Surface.GetPixelFormatID(), null);
            }
            else if (FCenteredSurface.GetWidth() != FSceneWidth || FCenteredSurface.GetHeight() != FSceneHeight)
            {
                FCenteredSurface.Resize(FSceneWidth, FSceneHeight);
            }

            return FCenteredSurface;
        }

        // Port extension: fills the scene outside the centered map black and copies the map part of
        // FCenteredSurface into the centered rectangle.
        public void ShowCenteredMap(TSurface Target, int X, int Y)
        {
            TSurfaceDesc SD = default;
            TRect Source = GetMapSceneRect();
            TRect Shown = GetMapDisplayRect();
            if (Target.LockEx(ref SD, Rect(X, Y, X + FSceneWidth, Y + FSceneHeight)) >= 0)
            {
                FillOutsideRect(Shown, SD);
                Target.UnlockEx(SD);
            }

            FCenteredSurface.BltClipped(Target, Source, X + Shown.Left, Y + Shown.Top);
        }

        // Port extension: fills every pixel of SD outside Inside black.
        public static void FillOutsideRect(TRect Inside, in TSurfaceDesc SD)
        {
            Aow.Game.Draw.PixelFormats.ColorFillSD(Rect(0, 0, SD.Width, Inside.Top), 0, SD);
            Aow.Game.Draw.PixelFormats.ColorFillSD(Rect(0, Inside.Bottom, SD.Width, SD.Height), 0, SD);
            Aow.Game.Draw.PixelFormats.ColorFillSD(Rect(0, Inside.Top, Inside.Left, Inside.Bottom), 0, SD);
            Aow.Game.Draw.PixelFormats.ColorFillSD(Rect(Inside.Right, Inside.Top, SD.Width, Inside.Bottom), 0, SD);
        }

        public THSMap HSMap { get => FHSMap; set => SetHSMap(value); }
        public TSurface StaticSurface { get => FStaticSurface; }
        public bool StaticSurfaceValid { get => FStaticSurfaceValid; set => FStaticSurfaceValid = value; }
        public int SceneX { get => FSceneX; set => SetSceneX(value); }
        public int SceneY { get => FSceneY; set => SetSceneY(value); }
        public int SceneL { get => FSceneL; set => SetSceneL(value); }
        public int SceneWidth { get => FSceneWidth; set => SetSceneWidth(value); }
        public int SceneHeight { get => FSceneHeight; set => SetSceneHeight(value); }

        [Published]
        public TNotifyEvent<object> OnSceneChanged { get => FOnSceneChanged; set => FOnSceneChanged = value; }
    }

    // FHSEngine, FTries, FResources, FMOClass. The map object resources of one kind
    // (THexagonControl: hexagons, TRoadControl: roads) sorted by terrain type: FResources holds one
    // TList of resources per terrain type name of the set (THSSet.TerrainTypes), filled by
    // RegisterResource when a resource is activated. Place and PlaceClass put a random resource of
    // a terrain type on a map field
    public partial class TMORTerrainControl : TEObject
    {
        internal THSEngine FHSEngine;
        internal int FTries;
        internal TList FResources;
        internal Type FMOClass;

        // HSEngine.pas:4522
        internal void Initialize()
        {
            Clear();
            for (int I = 0, last = FHSEngine.FHSSet.TerrainTypes.Count - 1; I <= last; I++)
            {
                var list = new TList();
                FResources.Add(list);
            }
        }

        // HSEngine.pas:4497
        public override void Dispose()
        {
            Clear();
            FResources?.Dispose();
            FResources = null;
            base.Dispose();
        }

        // HSEngine.pas:4505
        public override TSet<TEControlStyleElement> GetControlStyle() => TSet<TEControlStyleElement>.Of(Aow.Game.Engine.Nodes.EngineServices.csShared);

        // HSEngine.pas:4489
        public TMORTerrainControl(TECustomNode owner) : base(owner)
        {
            FResources = new TList();
            FMOClass = typeof(Aow.Game.World.TMapObject);
            FTries = 5;
        }

        // HSEngine.pas:4510
        public override void Clear()
        {
            if (FResources != null)
            {
                for (int I = FResources.Count - 1; I >= 0; I--)
                {
                    (FResources[I] as IDisposable)?.Dispose();
                    FResources.Delete(I);
                }
            }
        }

        // HSEngine.pas:4531
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 536870920)
            {
                FHSEngine = (THSEngine)Msg.Payload.As<TEngine>();
                Initialize();
            }
        }

        // HSEngine.pas:4585
        public virtual Aow.Game.World.TMapObject CreateMO(sbyte TerrainType)
        {
            Aow.Game.World.TMapObject Result;
            int I;
            TList List;
            int Count;
            Aow.Game.World.TMapObjectResource R;
            I = TerrainType;
            List = (TList)FResources[I];
            Count = List.Count;
            if (Count != 0)
            {
                var random = Random(Count);
                R = (Aow.Game.World.TMapObjectResource)List.List[random];
                Result = (Aow.Game.World.TMapObject)R.CreateResourceUser();
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // HSEngine.pas:4604
        public virtual Aow.Game.World.TMapObject Place(TMapContainer Container, sbyte X, sbyte Y, sbyte L, sbyte TerrainType)
        {
            TList List;
            Aow.Game.World.TMapObjectResource R;
            TMapField MapF;
            Aow.Game.World.TMapObject MO;
            Aow.Game.World.TMapObject Result = null;
            List = (TList)FResources[TerrainType];
            if (List.Count != 0)
            {
                R = null;
                MapF = Container.GetField(X, Y, L);
                MO = (Aow.Game.World.TMapObject)MapF.FindOwnedHS(FMOClass);
                if (MO != null)
                {
                    R = GetRndResource(TerrainType, MO.FResource.EClass.ClassID);
                    if (R != null)
                    {
                        MO.SetResource(R);
                        MapF.UpdateTerrain();
                        Result = MO;
                    }
                }

                if (R == null)
                {
                    var random = Random(List.Count);
                    R = (Aow.Game.World.TMapObjectResource)List.List[random];
                    if (R != null)
                    {
                        Result = R.PlaceResourceUser(Container.FMap, X, Y, L);
                    }
                }
            }

            return Result;
        }

        // HSEngine.pas:4637
        public virtual Aow.Game.World.TMapObject PlaceClass(TMapContainer Container, sbyte X, sbyte Y, sbyte L, sbyte TerrainType, int AClassID)
        {
            Aow.Game.World.TMapObjectResource R;
            TMapField MapF;
            Aow.Game.World.TMapObject MO;
            Aow.Game.World.TMapObject Result = null;
            if (((TList)FResources[TerrainType]).Count != 0)
            {
                R = null;
                MapF = Container.GetField(X, Y, L);
                MO = (Aow.Game.World.TMapObject)MapF.FindOwnedHS(FMOClass);
                if (MO != null && MO.FResource.EClass.ClassID == AClassID)
                {
                    R = GetRndResource(TerrainType, MO.FResource.EClass.ClassID);
                    if (R != null)
                    {
                        MO.SetResource(R);
                        MapF.UpdateTerrain();
                        Result = MO;
                    }
                }

                if (R == null)
                {
                    R = GetRndResource(TerrainType, AClassID);
                    if (R != null)
                    {
                        Result = R.PlaceResourceUser(Container.FMap, X, Y, L);
                    }
                }
            }

            return Result;
        }

        // HSEngine.pas:4541
        public Aow.Game.World.TMapObjectResource GetRndResource(int TerrainType, int AClassID)
        {
            Aow.Game.World.TMapObjectResource Result;
            TList List;
            int I;
            List = (TList)FResources[TerrainType];
            if (List.Count != 0)
            {
                I = 0;
                Result = null;
                while (I != FTries && Result == null)
                {
                    var random = Random(List.Count);
                    Result = (Aow.Game.World.TMapObjectResource)List.List[random];
                    if (Result.EClass.ClassID != AClassID)
                    {
                        Result = null;
                    }

                    I += 1;
                }

                if (Result == null)
                {
                    I = 0;
                    while (I != List.Count && Result == null)
                    {
                        Result = (Aow.Game.World.TMapObjectResource)List.List[I];
                        if (Result.EClass.ClassID != AClassID)
                        {
                            Result = null;
                        }

                        I += 1;
                    }
                }
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // HSEngine.pas:4572
        public void RegisterResource(Aow.Game.World.TMapObjectResource Resource)
        {
            sbyte TerrainType;
            TerrainType = Resource.FTerrainList.GetTerrain(0);
            if (TerrainType >= 0 && TerrainType < FResources.Count)
            {
                if (((TList)FResources.List[TerrainType]).IndexOf(Resource) != -1 || Resource == null)
                {
                    throw new InvalidOperationException("Resource double defined (TMORTerrainControl)");
                }

                ((TList)FResources.List[TerrainType]).Add(Resource);
            }
        }

        public THSEngine HSEngine { get => FHSEngine; }
        public int Tries { get => FTries; set => FTries = value; }
        public TList Resources { get => FResources; }
        public Type MOClass { get => FMOClass; set => FMOClass = value; }

    }

    // the hexagon resources of a set
    public partial class THexagonControl : TMORTerrainControl
    {
        public THexagonControl(TECustomNode owner) : base(owner)
        { }

    }

    // the road resources of a set
    public partial class TRoadControl : TMORTerrainControl
    {
        public TRoadControl(TECustomNode owner) : base(owner)
        { }

    }

    // FStatus, FHexagonControl, FRoadControl, FActivatedEngine, FMapObjectResourceList,
    // FResourceEditSettings, FImageLibraryManager, FSFXManager, FPlugInControl, FName,
    // FTerrainTypes, FOverlayTypes, FDecorationTypes. The hexagon sprite set: the resources a map
    // uses (an.hss file, THSEngine.LoadHSS). ReadWrite stores the name (id 4), the map object
    // resources (5), the resource edit settings (6) and the plug-ins (7). the type name lists are
    // filled by the game (TAoWHSSet.Create) and not stored
    public partial class THSSet : TECustomOwner
    {
        // HSEngine.pas:3394
        public new const int ClassID = 66049;

        public THSSet(TECustomNode owner) : this(null, owner) { }

        private protected THSSet(ConstructionContext context, TECustomNode owner) : base(context, owner)
        {
            SetControlStyle(GetControlStyle() + TSet<TEControlStyleElement>.Of(Aow.Game.Engine.Nodes.EngineServices.csReserved2));
            FPlugInControl = new TPlugInControl(null);
            FTerrainTypes = new TStringList();
            FOverlayTypes = new TStringList();
            FDecorationTypes = new TStringList();
            FHexagonControl = new THexagonControl(this);
            FHexagonControl.MOClass = typeof(TAbstractHexagon);
            FRoadControl = new TRoadControl(this);
            FRoadControl.MOClass = typeof(TAbstractRoad);
            FMapObjectResourceList = new Aow.Game.World.TMapObjectResourceList(this);
            FMapObjectResourceList.SetControlStyle(TSet<TEControlStyleElement>.Of(Aow.Game.Engine.Nodes.EngineServices.csStored, Aow.Game.Engine.Nodes.EngineServices.csShared));
            FResourceEditSettings = new TEResourceEditSettings(this);
            FResourceEditSettings.SetControlStyle(TSet<TEControlStyleElement>.Of(Aow.Game.Engine.Nodes.EngineServices.csStored, Aow.Game.Engine.Nodes.EngineServices.csShared));
            FImageLibraryManager = new TImageLibraryManager(null);
            FSFXManager = new Aow.Game.Audio.TSFXManager(null);
        }

        internal TSet<THSSetStatusElement> FStatus;
        internal THexagonControl FHexagonControl;
        internal TRoadControl FRoadControl;
        internal TEngine FActivatedEngine;

        public TEngine ActivatedEngine { get => FActivatedEngine; }

        internal Aow.Game.World.TMapObjectResourceList FMapObjectResourceList;
        internal TEResourceEditSettings FResourceEditSettings;
        internal TImageLibraryManager FImageLibraryManager;
        internal Aow.Game.Audio.TSFXManager FSFXManager;
        internal TPlugInControl FPlugInControl;
        internal string FName = "";
        internal TStringList FTerrainTypes;
        internal TStringList FOverlayTypes;
        internal TStringList FDecorationTypes;

        // HSEngine.pas:3368
        public override void Dispose()
        {
            TEMsg Msg = default;
            FStatus = FStatus + TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssDestroying);
            Aow.Game.Engine.Nodes.TEMsg.InitMsg(ref Msg, 66564, this);
            if (FActivatedEngine != null && FActivatedEngine.ControlNode != null)
            {
                FActivatedEngine.ControlNode.MsgProc(ref Msg, this);
            }

            base.Dispose();
            FPlugInControl?.Dispose();
            FTerrainTypes?.Dispose();
            FTerrainTypes = null;
            FOverlayTypes?.Dispose();
            FOverlayTypes = null;
            FDecorationTypes?.Dispose();
            FDecorationTypes = null;
            FImageLibraryManager?.Dispose();
            FImageLibraryManager = null;
            FSFXManager?.Dispose();
            FSFXManager = null;
            Aow.Game.Engine.Nodes.TEMsg.InitMsg(ref Msg, 66565, this);
            if (FActivatedEngine != null && FActivatedEngine.ControlNode != null)
            {
                FActivatedEngine.ControlNode.MsgProc(ref Msg, this);
            }

            FStatus = FStatus - TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssDestroying);
        }

        // HSEngine.pas:3410
        public override void ReadWrite(TEStorageStream Stream)
        {
            SetEngine(Stream.Engine);
            FPlugInControl.Engine = Stream.Engine;
            base.ReadWrite(Stream);
            Stream.rwString(ref FName, 4);
            Stream.rwEObject(FMapObjectResourceList, 5);
            Stream.rwEObject(FResourceEditSettings, 6);
            Stream.rwEObject(FPlugInControl, 7);
        }

        // HSEngine.pas:3405
        public override void Clear()
        {
            base.Clear();
        }

        // HSEngine.pas:3399
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 536870920)
            {
                FActivatedEngine = Msg.Payload.As<TEngine>();
            }
        }

        // HSEngine.pas:3421
        public void Activate(bool Edit)
        {
            TEMsg Msg = default;
            Aow.Game.Engine.Nodes.TEMsg.InitMsg(ref Msg, 66560, this);
            GetEngine().ControlNode.MsgProc(ref Msg, this);
            FStatus = FStatus + TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssActivated);
            if (Edit)
            {
                Aow.Game.Engine.Nodes.TEMsg.InitMsg(ref Msg, 66562, this);
                GetEngine().ControlNode.MsgProc(ref Msg, this);
                FStatus = FStatus + TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssEditting);
            }
        }

        // HSEngine.pas:3436
        public void DeActivate()
        {
            TEMsg Msg = default;
            if (FStatus.Contains(THSSetStatusElement.ssActivated))
            {
                Aow.Game.Engine.Nodes.TEMsg.InitMsg(ref Msg, 66561, this);
                GetEngine().ControlNode.MsgProc(ref Msg, this);
                FStatus = FStatus - TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssActivated);
                if (FStatus.Contains(THSSetStatusElement.ssEditting))
                {
                    Aow.Game.Engine.Nodes.TEMsg.InitMsg(ref Msg, 66563, this);
                    GetEngine().ControlNode.MsgProc(ref Msg, this);
                    FStatus = FStatus - TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssEditting);
                }
            }
        }

        // HSEngine.pas:3454
        public void SetModified()
        {
            if (FStatus.Contains(THSSetStatusElement.ssEditting))
            {
                FStatus = FStatus + TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssModified);
            }
        }

        // HSEngine.pas:3459
        public void ResetModified()
        {
            FStatus = FStatus - TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssModified);
        }

        // HSEngine.pas:3464
        public void SetEditting()
        {
            FStatus = FStatus + TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssEditting);
        }

        // HSEngine.pas:3468
        public void ResetEditting()
        {
            FStatus = FStatus - TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssEditting);
        }

        public TSet<THSSetStatusElement> Status { get => FStatus; }
        public THexagonControl HexagonControl { get => FHexagonControl; }
        public TRoadControl RoadControl { get => FRoadControl; }
        public Aow.Game.World.TMapObjectResourceList MapObjectResourceList { get => FMapObjectResourceList; }
        public TEResourceEditSettings ResourceEditSettings { get => FResourceEditSettings; }
        public TImageLibraryManager ImageLibraryManager { get => FImageLibraryManager; }
        public Aow.Game.Audio.TSFXManager SFXManager { get => FSFXManager; }
        public TPlugInControl PlugInControl { get => FPlugInControl; }
        public string Name { get => FName; set => FName = value; }
        public TStringList TerrainTypes { get => FTerrainTypes; }
        public TStringList OverlayTypes { get => FOverlayTypes; }
        public TStringList DecorationTypes { get => FDecorationTypes; }

    }

    // FHSEngine. The component that creates the engine (CreateEngine) and keeps it as
    // TEComponent.Engine. OnRegisterClasses and OnUnRegisterClasses are TEComponent's events,
    // called from Loaded and Destroy
    public partial class THexagonSpriteEngine : TEComponent
    {
        internal THSEngine FHSEngine;

        // HSEngine.pas:5381
        public THexagonSpriteEngine(TComponent owner) : base(owner)
        {
            CreateEngine();
            if (FHSEngine != null)
            {
                FHSEngine.FComponent = this;
            }
        }

        // HSEngine.pas:5388
        public virtual void SetHSEngine(THSEngine Value)
        {
            FHSEngine = Value;
            Engine = FHSEngine;
        }

        // HSEngine.pas:5394
        public virtual void CreateEngine()
        {
            SetHSEngine(THSEngine.Create(null));
        }

        // HSEngine.pas:5399
        public void SetHSSetSearchPath(string Value)
        {
            if (FHSEngine != null)
            {
                FHSEngine.FHSSetSearchPath = Value;
            }
        }

        // HSEngine.pas:5404
        public string GetHSSetSearchPath()
        {
            string Result;
            if (FHSEngine != null)
            {
                Result = FHSEngine.FHSSetSearchPath;
            }
            else
            {
                Result = "";
            }

            return Result;
        }

        public THSEngine HSEngine { get => FHSEngine; set => SetHSEngine(value); }
        public string HSSetSearchPath { get => GetHSSetSearchPath(); set => SetHSSetSearchPath(value); }
    }
}

namespace Aow.Game.Draw
{

    // a sprite on one hexagon. FStatus: bit 1 visible, 2 edit mode, 4 selected, 8 activated (a
    // Byte: the original sets the bits with or/and, which a set with Include does not give)
    public partial class TSingleHS : THexagonSprite
    {
        public TSingleHS(TECustomNode owner) : base(owner)
        { }

        internal byte FStatus;

        // HXSPRITE.PAS:640
        public override void SetVisible(bool Value)
        {
            if (Value)
            {
                FStatus = (byte)(FStatus | 1);
            }
            else
            {
                FStatus = (byte)(FStatus & 254);
            }
        }

        // HXSPRITE.PAS:646
        public override bool GetVisible() => (FStatus & 1) != 0;

        // HXSPRITE.PAS:683
        public override bool GetSelected() => (FStatus & 4) != 0;

        // HXSPRITE.PAS:660
        public override bool GetEditMode() => (FStatus & 2) != 0;

        // HXSPRITE.PAS:665
        public override void SetEditMode(bool Value)
        {
            if (Value)
            {
                FStatus = (byte)(FStatus | 2);
            }
            else
            {
                FStatus = (byte)(FStatus & 253);
            }
        }

        // HXSPRITE.PAS:713
        public override int GetShowPriority()
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(((TMapField)Owner).FXhx, ((TMapField)Owner).FYhx, ref Xhp, ref Yhp);
            return (GetLevel() << 26) + ((Yhp + 32) << 13) + (Xhp + 16);
        }

        // HXSPRITE.PAS:704
        public override void Connect()
        {
        }

        // HXSPRITE.PAS:708
        public override void Disconnect()
        {
            SetOwner(null);
        }

        // HXSPRITE.PAS:749
        public override bool Place(TMapContainer Map, int Xhp, int Yhp, int Lhx)
        {
            int Xhx = default;
            int Yhx = default;
            Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
            return PlaceHX(Map, (sbyte)Xhx, (sbyte)Yhx, (sbyte)Lhx);
        }

        // HXSPRITE.PAS:721
        public override void Remove()
        {
            TMapField MapF;
            if (Owner != null)
            {
                MapF = (TMapField)Owner;
                RemoveFromMap();
                if (GetVisible())
                {
                    if (GetLevel() <= 15)
                    {
                        ((TMapLevel)MapF.Owner).FMapContainer.FMap.FStaticVisibleHS.RemoveChild(this);
                        ((TMapLevel)MapF.Owner).FMapContainer.FMap.FStaticSceneInvalid = true;
                    }
                    else
                    {
                        ((TMapLevel)MapF.Owner).FMapContainer.FMap.FDynamicVisibleHS.RemoveChild(this);
                    }
                }

                SetVisible(false);
            }
        }

        // HXSPRITE.PAS:688
        public override void Activate()
        {
            FStatus = (byte)(FStatus | 8);
        }

        // HXSPRITE.PAS:693
        public override void Deactivate()
        {
            base.Deactivate();
            FStatus = (byte)(FStatus & 247);
        }

        // HXSPRITE.PAS:651
        public override void MakeVisible(TVisibilityList VisibilityList)
        {
            if ((FStatus & 1) == 0)
            {
                FStatus = (byte)(FStatus | 1);
                VisibilityList.AddSortChild(this, GetShowPriority());
            }
        }

        // HXSPRITE.PAS:671
        public override void Select()
        {
            base.Select();
            FStatus = (byte)(FStatus | 4);
        }

        // HXSPRITE.PAS:677
        public override void Unselect()
        {
            base.Unselect();
            FStatus = (byte)(FStatus & 251);
        }

        // HXSPRITE.PAS:743
        public virtual bool CanPlace(TMapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx) => Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < Map.FWidth && Yhx < Map.FHeight && Lhx < Map.FLevelCount;

        // HXSPRITE.PAS:757
        public virtual bool PlaceHX(TMapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result;
            if (GetEditMode() || CanPlace(Map, Xhx, Yhx, Lhx))
            {
                Remove();
                SetOwner(Map.GetField(Xhx, Yhx, Lhx));
                Result = PlaceOnMap();
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // HXSPRITE.PAS:699
        public bool Activated() => (FStatus & 8) != 0;
    }

    // a sprite that is always in the visibility list (not on a field). FStatus: bit 1 visible
    public partial class TFixedVisibleHS : THexagonSprite
    {
        public TFixedVisibleHS(TECustomNode owner) : base(owner)
        { }

        internal byte FStatus;

        // HXSPRITE.PAS:770
        public override void SetVisible(bool Value)
        {
            if (Value)
            {
                FStatus = (byte)(FStatus | 1);
            }
            else
            {
                FStatus = (byte)(FStatus & 254);
            }
        }

        // HXSPRITE.PAS:775
        public override bool GetVisible() => (FStatus & 1) != 0;

        // HXSPRITE.PAS:787
        public override int GetShowPriority() => GetLevel() << 26;

        // HXSPRITE.PAS:791
        public override void MainRemove()
        {
            Deactivate();
            Remove();
            Assert(!GetVisible(), "FixedVisibleHS still in VisibilityList!!!", "HXSPRITE.PAS", 795);
        }

        // HXSPRITE.PAS:779
        public override void MakeVisible(TVisibilityList VisibilityList)
        {
            if ((FStatus & 1) == 0)
            {
                FStatus = (byte)(FStatus | 1);
                VisibilityList.AddSortChild(this, GetShowPriority());
            }
        }
    }

    public delegate void TOnShowHSEvent(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc);

    // FShowPriority, FOnShow (Show calls it). TAbstractAoWHSMap (AoWEPACK) creates two and sets
    // OnShow and ShowPriority
    public partial class TCustomFixedVisibleHS : TFixedVisibleHS
    {
        public TCustomFixedVisibleHS(TECustomNode owner) : base(owner)
        { }

        internal int FShowPriority;
        internal TOnShowHSEvent FOnShow;

        // HXSPRITE.PAS:800
        public override int GetShowPriority() => FShowPriority;

        // HXSPRITE.PAS:805
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            if (FOnShow != null)
            {
                FOnShow(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        public new int ShowPriority { get => GetShowPriority(); set => FShowPriority = value; }
        public TOnShowHSEvent OnShow { get => FOnShow; set => FOnShow = value; }

    }

    // an image of an image library shown at a pixel position. FStatus, FImage, FXhp, FYhp, FLhx,
    // FShowPriority (the top six bits are the level, the rest the offset SetShowPriorityOffset
    // adds). Persisted: 1 FStatus (rwData), 2 FShowPriority, 3 FXhp, 4 FYhp, 5 FLhx (rwByte)
    public partial class TLibraryImageHS : THexagonSprite
    {
        public new const int ClassID = 65875;

        internal TSet<TLibraryImageHSStatusElement> FStatus;
        internal Aow.Game.Draw.Images.TLibraryImage FImage;
        internal int FXhp;
        internal int FYhp;
        internal byte FLhx;
        internal int FShowPriority;

        // LibImgHS.PAS:111
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            RwSet(ref FStatus, 1, 1, Stream.rwData, 0);
            Stream.rwInteger(ref FShowPriority, 2);
            Stream.rwInteger(ref FXhp, 3);
            Stream.rwInteger(ref FYhp, 4);
            Stream.rwByte(ref FLhx, 5);
        }

        // LibImgHS.PAS:6
        // { LibImgHS.PAS: TLibraryImageHS (include file of HSEngine.pas; the original Asserts name
        //   D:\GEngine\HSENGINE\inc\LibImgHS.PAS) }
        public TLibraryImageHS(TECustomNode owner) : base(owner)
        {
            FStatus = TSet<TLibraryImageHSStatusElement>.Of(TLibraryImageHSStatusElement.lsMapIsOwner);
            SetLevel(26);
        }

        // LibImgHS.PAS:85
        public override byte GetLevel() => (byte)(int)(((uint)FShowPriority) >> 26);

        // LibImgHS.PAS:18
        public override void SetVisible(bool Value)
        {
            if (Value)
            {
                FStatus = FStatus + TSet<TLibraryImageHSStatusElement>.Of(TLibraryImageHSStatusElement.lsVisible);
            }
            else
            {
                FStatus = FStatus - TSet<TLibraryImageHSStatusElement>.Of(TLibraryImageHSStatusElement.lsVisible);
            }
        }

        // LibImgHS.PAS:26
        public override bool GetVisible() => FStatus.Contains(TLibraryImageHSStatusElement.lsVisible);

        // LibImgHS.PAS:40
        public override bool GetEditMode() => FStatus.Contains(TLibraryImageHSStatusElement.lsEditMode);

        // LibImgHS.PAS:45
        public override void SetEditMode(bool Value)
        {
            if (Value)
            {
                FStatus = FStatus + TSet<TLibraryImageHSStatusElement>.Of(TLibraryImageHSStatusElement.lsEditMode);
            }
            else
            {
                FStatus = FStatus - TSet<TLibraryImageHSStatusElement>.Of(TLibraryImageHSStatusElement.lsEditMode);
            }
        }

        // LibImgHS.PAS:90
        public override int GetShowPriority() => (FYhp << 13) + FXhp + FShowPriority;

        // LibImgHS.PAS:194
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            FImage.Show(FXhp - XhpSurf, FYhp - YhpSurf, SurfaceDesc);
            if (FStatus.Contains(TLibraryImageHSStatusElement.lsDestroyOnShow))
            {
                Dispose();
            }
        }

        // LibImgHS.PAS:126
        public override bool Place(TMapContainer Map, int Xhp, int Yhp, int Lhx)
        {
            bool Result;
            if (CanPlace(Map, Xhp, Yhp, Lhx))
            {
                FXhp = Xhp;
                FYhp = Yhp;
                FLhx = (byte)Lhx;
                if (FStatus.Contains(TLibraryImageHSStatusElement.lsMapIsOwner))
                {
                    Result = PlaceOnMapEx(Map, Xhp, Yhp, Lhx, FImage.Width, FImage.Height);
                }
                else
                {
                    Result = PlaceUserOnMapEx(Map, Xhp, Yhp, Lhx, FImage.Width, FImage.Height);
                }

                if (Result)
                {
                    FStatus = FStatus + TSet<TLibraryImageHSStatusElement>.Of(TLibraryImageHSStatusElement.lsPlacedOnMap);
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // LibImgHS.PAS:152
        public override void Remove()
        {
            THSEngine Engine;
            if (FStatus.Contains(TLibraryImageHSStatusElement.lsPlacedOnMap))
            {
                if (FStatus.Contains(TLibraryImageHSStatusElement.lsMapIsOwner))
                {
                    if (GetVisible())
                    {
                        if (GetLevel() <= 15)
                        {
                            ((TMapLevel)Owner.Owner).FMapContainer.FMap.FStaticVisibleHS.RemoveChild(this);
                            ((TMapLevel)Owner.Owner).FMapContainer.FMap.FStaticSceneInvalid = true;
                        }
                        else
                        {
                            ((TMapLevel)Owner.Owner).FMapContainer.FMap.FDynamicVisibleHS.RemoveChild(this);
                        }

                        SetVisible(false);
                    }

                    RemoveFromMapEx(FXhp, FYhp, FImage.Width, FImage.Height);
                }
                else
                {
                    Engine = (THSEngine)GetEngine();
                    if (GetVisible())
                    {
                        if (GetLevel() <= 15)
                        {
                            Engine.FHSMap.FStaticVisibleHS.RemoveChild(this);
                            Engine.FHSMap.FStaticSceneInvalid = true;
                        }
                        else
                        {
                            Engine.FHSMap.FDynamicVisibleHS.RemoveChild(this);
                        }

                        SetVisible(false);
                    }

                    RemoveUserFromMapEx(Engine.FHSMap.FMapContainer, FXhp, FYhp, FLhx, FImage.Width, FImage.Height);
                }

                FStatus = FStatus - TSet<TLibraryImageHSStatusElement>.Of(TLibraryImageHSStatusElement.lsPlacedOnMap);
            }
        }

        // LibImgHS.PAS:31
        public override void MakeVisible(TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(TLibraryImageHSStatusElement.lsVisible))
            {
                FStatus = FStatus + TSet<TLibraryImageHSStatusElement>.Of(TLibraryImageHSStatusElement.lsVisible);
                VisibilityList.AddSortChild(this, GetShowPriority());
            }
        }

        // LibImgHS.PAS:101
        public virtual void SetImage(Aow.Game.Draw.Images.TLibraryImage Value)
        {
            if (FStatus.Contains(TLibraryImageHSStatusElement.lsPlacedOnMap))
            {
                if (Value == null || (FImage.Width != Value.Width || FImage.Height != Value.Height))
                {
                    throw new InvalidOperationException("Cannot change image when placed on map (TLibraryImageHS)");
                }
            }

            FImage = Value;
            if (FImage != null)
            {
                SetShowPriorityOffset((int)(((uint)FImage.Width) >> 1), (int)(((uint)FImage.Height) >> 1));
            }
        }

        // LibImgHS.PAS:121
        public virtual bool CanPlace(TMapContainer Map, int Xhp, int Yhp, int Lhx) => true;

        // LibImgHS.PAS:144
        public virtual bool PlaceHX(TMapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            return Place(Map, (int)(((uint)FImage.Width) >> 2) + Xhp + 16, (int)(((uint)FImage.Height) >> 2) + Yhp + 16, Lhx);
        }

        // LibImgHS.PAS:53
        public bool GetMapIsOwner() => FStatus.Contains(TLibraryImageHSStatusElement.lsMapIsOwner);

        // LibImgHS.PAS:58
        public void SetMapIsOwner(bool Value)
        {
            if (FStatus.Contains(TLibraryImageHSStatusElement.lsPlacedOnMap))
            {
                Aow.Game.World.Map.HexGeometry.OriginalAssert("Cannot change MapIsOwner when places on map (TLibraryImageHS)", "D:\\GEngine\\HSENGINE\\inc\\LibImgHS.PAS", 6626);
            }

            if (Value)
            {
                FStatus = FStatus + TSet<TLibraryImageHSStatusElement>.Of(TLibraryImageHSStatusElement.lsMapIsOwner);
            }
            else
            {
                FStatus = FStatus - TSet<TLibraryImageHSStatusElement>.Of(TLibraryImageHSStatusElement.lsMapIsOwner);
            }
        }

        // LibImgHS.PAS:67
        public bool GetDestroyOnShow() => FStatus.Contains(TLibraryImageHSStatusElement.lsDestroyOnShow);

        // LibImgHS.PAS:72
        public void SetDestroyOnShow(bool Value)
        {
            if (Value)
            {
                FStatus = FStatus + TSet<TLibraryImageHSStatusElement>.Of(TLibraryImageHSStatusElement.lsDestroyOnShow);
            }
            else
            {
                FStatus = FStatus - TSet<TLibraryImageHSStatusElement>.Of(TLibraryImageHSStatusElement.lsDestroyOnShow);
            }
        }

        // LibImgHS.PAS:80
        public void SetLevel(byte Value)
        {
            FShowPriority = (FShowPriority & 67108863) + (Value << 26);
        }

        // LibImgHS.PAS:95
        public void SetShowPriorityOffset(int OffsetX, int OffsetY)
        {
            // { both offsets are shifted into the Y part (original) }
            FShowPriority = (FShowPriority & -67108864) + (OffsetY << 13) + (OffsetX << 13);
        }

        public Aow.Game.Draw.Images.TLibraryImage Image { get => FImage; set => SetImage(value); }
        public bool MapIsOwner { get => GetMapIsOwner(); set => SetMapIsOwner(value); }
        public bool DestroyOnShow { get => GetDestroyOnShow(); set => SetDestroyOnShow(value); }
        public new byte Level { get => GetLevel(); set => SetLevel(value); }

    }

    // a sprite placed at a pixel position. FVisible, FXhp, FYhp, FLhx (HSLib.TLibraryHS reads FXhp
    // and FYhp). Persisted: 4 FXhp, 5 FYhp, 6 FLhx (rwByte)
    public partial class THPBasedHS : THexagonSprite
    {
        public new const int ClassID = 65874;

        public THPBasedHS(TECustomNode owner) : base(owner)
        {
            FXhp = -2147483648;
            FYhp = -2147483648;
            FLhx = -1;
        }

        protected internal bool FVisible;
        protected internal int FXhp;
        protected internal int FYhp;
        protected internal sbyte FLhx;

        // HXSPRITE.PAS:622
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FXhp, 4);
            Stream.rwInteger(ref FYhp, 5);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLhx), 6);
        }

        // HXSPRITE.PAS:599

        // HXSPRITE.PAS:612
        public override void SetVisible(bool Value)
        {
            FVisible = Value;
        }

        // HXSPRITE.PAS:617
        public override bool GetVisible() => FVisible;

        // HXSPRITE.PAS:630
        public override bool Place(TMapContainer Map, int Xhp, int Yhp, int Lhx)
        {
            FXhp = Xhp;
            FYhp = Yhp;
            FLhx = (sbyte)Lhx;
            return true;
        }

    }
}

namespace Aow.Game.World
{

    // HSEngine.EMapObjectResourceMissing: TMapObject.ReadWrite raises it when the resource id read
    // from the stream is not in the resource list
    public partial class EMapObjectResourceMissing : System.Exception
    {
        // SysUtils.pas:174
        public EMapObjectResourceMissing(string Msg) : base(Msg)
        {
        }
    }

    // a map object that covers the hexagons of its terrain list around its main hexagon. FXhx, FYhx
    // (the main hexagon, ids 3 and 4)
    public partial class TMultiHexMO : TMapObject
    {

        public TMultiHexMO(TECustomNode owner) : base(owner)
        { }

        protected internal sbyte FXhx;
        protected internal sbyte FYhx;

        // MAPOBJ.PAS:923
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FXhx), 3);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FYhx), 4);
        }

        // MAPOBJ.PAS:900
        public override sbyte GetXhx() => FXhx;

        // MAPOBJ.PAS:905
        public override sbyte GetYhx() => FYhx;

        // MAPOBJ.PAS:910
        public override int GetXYL() => (GetLhx() << 16) + (FXhx + (FYhx << 8));

        // MAPOBJ.PAS:915
        public override int GetShowPriority()
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            return FResource.FShowPriorityOffset + (Yhp << 13) + Xhp;
        }

        // MAPOBJ.PAS:944
        // { Connect adds the object to the fields of its terrain list around the main hexagon }
        public override void Connect()
        {
            int Count;
            TMapLevel MapLevel;
            TMapField MapField;
            THNWalkData Walk = default;
            if (!FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
            {
                FStatus = FStatus + TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moPlacedOnMap);
                Count = GetTerrainCount();
                MapLevel = (TMapLevel)Owner.Owner;
                Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                while (Walk.HN < Count)
                {
                    if ((GetTerrain(Walk.HN) >= -1 || GetOverlay(Walk.HN) >= -1) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                    {
                        MapField = MapLevel.GetField(Walk.X, Walk.Y);
                        if (MapField == Owner)
                        {
                            MapField.RemoveHS(this);
                        }

                        MapField.AddHS(this, Walk.HN);
                    }

                    Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
                }
            }
        }

        // MAPOBJ.PAS:972
        public override void Disconnect()
        {
            int Count;
            TMapLevel MapLevel;
            THNWalkData Walk = default;
            if (FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
            {
                Count = GetTerrainCount();
                MapLevel = (TMapLevel)Owner.Owner;
                FStatus = FStatus - TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moPlacedOnMap);
                Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                while (Walk.HN < Count)
                {
                    if ((GetTerrain(Walk.HN) >= -1 || GetOverlay(Walk.HN) >= -1) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                    {
                        MapLevel.GetField(Walk.X, Walk.Y).RemoveHS(this);
                    }

                    Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
                }

                Owner = null;
            }
        }

        // MAPOBJ.PAS:930
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Game.World.Map.HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            if (FResource.FImageSequenceList == null)
            {
                FResource.FImageLibrary.Get(0).Show(Xhp - XhpSurf + ((TMultiHexMOR)FResource).FdXImage, Yhp - YhpSurf + ((TMultiHexMOR)FResource).FdYImage, SurfaceDesc);
            }
            else
            {
                FResource.ShowComplexImage(Xhp - XhpSurf, Yhp - YhpSurf, SurfaceDesc, 0, (uint)(Xhp + Yhp));
            }
        }

        // MAPOBJ.PAS:995
        public override bool PlaceOnMap()
        {
            int Count;
            TMapLevel MapLevel;
            THNWalkData Walk = default;
            if (!FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
            {
                Connect();
                Count = GetTerrainCount();
                MapLevel = (TMapLevel)Owner.Owner;
                if (GetLevel() <= 15)
                {
                    Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                    while (Walk.HN < Count)
                    {
                        if ((GetTerrain(Walk.HN) >= -1 || GetOverlay(Walk.HN) >= -1) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                        {
                            MapLevel.FMap.InvalidateStaticScene(Walk.X, Walk.Y, ((TMapField)Owner).FLhx);
                        }

                        Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
                    }
                }

                if (!GetEditMode())
                {
                    var hNtoRad = Aow.Game.World.Map.HexGeometry.HNtoRad(Count);
                    MapLevel.FMap.TerrainChangedRad(FXhx, FYhx, (sbyte)MapLevel.FLhx, hNtoRad);
                }
            }

            return true;
        }

        // MAPOBJ.PAS:1023
        public override void RemoveFromMap()
        {
            int Count;
            TMapLevel MapLevel;
            TMapField MapField;
            sbyte TerrainType;
            sbyte OverlayType;
            THNWalkData Walk = default;
            if (FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
            {
                MapLevel = (TMapLevel)Owner.Owner;
                Disconnect();
                if (!MapLevel.FMap.FStatus.Contains(THSMapStatusElement.msDestroying))
                {
                    Count = GetTerrainCount();
                    Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                    while (Walk.HN < Count)
                    {
                        TerrainType = GetTerrain(Walk.HN);
                        OverlayType = GetOverlay(Walk.HN);
                        if ((TerrainType >= -1 || OverlayType >= -1) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                        {
                            MapField = MapLevel.GetField(Walk.X, Walk.Y);
                            if (TerrainType >= 0 || OverlayType >= 0)
                            {
                                MapField.UpdateTerrain();
                            }

                            if (GetLevel() <= 15)
                            {
                                MapLevel.FMap.InvalidateStaticScene(Walk.X, Walk.Y, MapLevel.FLhx);
                            }
                        }

                        Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
                    }

                    var hNtoRad = Aow.Game.World.Map.HexGeometry.HNtoRad(Count);
                    MapLevel.FMap.TerrainChangedRad(FXhx, FYhx, (sbyte)MapLevel.FLhx, hNtoRad);
                }
            }
        }

        // MAPOBJ.PAS:1122
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result;
            int Xhx = default;
            int Yhx = default;
            int HN;
            if (FResource.FEngine.FHSMap.FStatus.Contains(THSMapStatusElement.msEditting))
            {
                Aow.Game.World.Map.HexGeometry.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
                HN = Aow.Game.World.Map.HexGeometry.dHXtoHN(((TMapField)Owner).FXhx, ((TMapField)Owner).FYhx, Xhx, Yhx);
                Result = HN < FResource.FTerrainList.FCount && (FResource.FTerrainList.GetTerrain(HN) >= 0 || FResource.FTerrainList.GetOverlay(HN) >= 0);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // MAPOBJ.PAS:1096
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            int Count;
            sbyte TerrainType;
            sbyte OverlayType;
            THexagonSprite HS;
            THNWalkData Walk = default;
            bool Result = true;
            Count = FResource.FTerrainList.FCount;
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (Result && Walk.HN < Count)
            {
                TerrainType = GetTerrain(Walk.HN);
                OverlayType = GetOverlay(Walk.HN);
                if ((TerrainType >= 0 || OverlayType >= 0) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapContainer.FWidth && Walk.Y < MapContainer.FHeight)
                {
                    HS = MapContainer.GetField(Walk.X, Walk.Y, Lhx).GetMaxLevelNoneTransparentHS();
                    Result = HS == null || CanPlaceOnHS(HS);
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }

            return Result && MapContainer.FMap.CanChangeTerrain(this, Xhx, Yhx, Lhx, FResource.FTerrainList);
        }

        // MAPOBJ.PAS:1138
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            FXhx = Xhx;
            FYhx = Yhx;
            return base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
        }

        // MAPOBJ.PAS:1214
        public virtual void SortMapFields()
        {
            TMapLevel MapLevel;
            THNWalkData Walk = default;
            MapLevel = (TMapLevel)Owner.Owner;
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (true)
            {
                var terrainCount = GetTerrainCount();
                if (!(Walk.HN < terrainCount))
                    break;
                if (GetTerrain(Walk.HN) >= 0 || GetOverlay(Walk.HN) >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    // { Original skips bounds for terrain. }
                    MapLevel.GetField(Walk.X, Walk.Y).Sort();
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }
        }

        // MAPOBJ.PAS:1091
        public virtual bool CanPlaceOnHS(THexagonSprite HS) => HS.GetLevel() < 10;

        // MAPOBJ.PAS:1146
        // { The terrain type that most of the fields under the object have (the counts of Default win ties) }
        public virtual sbyte GetMostFrequentMapFieldTerrainType(TMapLevel MapLevel, sbyte Xhx, sbyte Yhx, sbyte Default)
        {
            TTerrainList TerrainList;
            sbyte TerrainType;
            FixedArray_byte_128 Counts;
            THNWalkData Walk = default;
            sbyte Result = Default;
            TerrainList = FResource.FTerrainList;
            if (Result < 0)
            {
                Result = 0;
            }

            Counts = default;
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (Walk.HN < TerrainList.FCount)
            {
                if ((TerrainList.GetTerrain(Walk.HN) >= 0 || GetOverlay(Walk.HN) >= 0) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    TerrainType = MapLevel.GetField(Walk.X, Walk.Y).FTerrainType;
                    if (TerrainType >= 0)
                    {
                        Counts[TerrainType] = (byte)(Counts[TerrainType] + 1);
                        if (Counts[TerrainType] > Counts[Result])
                        {
                            Result = TerrainType;
                        }
                    }
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }

            if (Default >= 0 && Counts[Result] == Counts[Default])
            {
                Result = Default;
            }

            return Result;
        }

        // MAPOBJ.PAS:1059
        public void UpdateTerrain(TMapLevel MapLevel, int Xhx, int Yhx)
        {
            int Count;
            THNWalkData Walk = default;
            Count = FResource.FTerrainList.FCount;
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (Walk.HN != Count)
            {
                if ((GetTerrain(Walk.HN) >= 0 || GetOverlay(Walk.HN) >= 0) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    MapLevel.GetField(Walk.X, Walk.Y).UpdateTerrain();
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }
        }

        // MAPOBJ.PAS:1075
        public void UpdateMapFields(TMapLevel MapLevel, int Xhx, int Yhx)
        {
            int Count;
            THNWalkData Walk = default;
            Count = FResource.FTerrainList.FCount;
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (Walk.HN != Count)
            {
                if ((GetTerrain(Walk.HN) >= 0 || GetOverlay(Walk.HN) >= 0) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    MapLevel.GetField(Walk.X, Walk.Y).UpdateMapField();
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }
        }

        // MAPOBJ.PAS:1178
        public void OverrideTerrain()
        {
            int Count;
            TMapLevel MapLevel;
            TMapField MapField;
            FixedArray_sbyte_2 Terrain = default;
            THNWalkData Walk = default;
            MapLevel = (TMapLevel)Owner.Owner;
            Count = GetTerrainCount();
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (Walk.HN < Count)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    var terrain = GetTerrain(Walk.HN);
                    Terrain[0] = terrain;
                    var overlay = GetOverlay(Walk.HN);
                    Terrain[1] = overlay;
                    MapField = MapLevel.GetField(Walk.X, Walk.Y);
                    if (Terrain[0] >= 0)
                    {
                        MapField.FTerrainType = Terrain[0];
                    }

                    if (Terrain[1] >= 0)
                    {
                        MapField.FOverlayType = Terrain[1];
                    }
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }

            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (Walk.HN < Count)
            {
                if (GetTerrain(Walk.HN) >= 0 || GetOverlay(Walk.HN) >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    // { Original skips bounds for terrain. }
                    MapLevel.GetField(Walk.X, Walk.Y).TerrainChanged();
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }
        }
    }

    // FdXImage, FdYImage (the image offset, ids 12 and 13), FdXmainP, FdYmainP (the main point, ids
    // 14 and 15, part of the show priority offset)
    public partial class TMultiHexMOR : TMapObjectResource
    {
        public new const int ClassID = 65877;

        public TMultiHexMOR(TECustomNode owner) : base(owner)
        {
            SetdXmainP(16);
            SetdYmainP(32);
        }

        protected internal int FdXImage;
        protected internal int FdYImage;
        protected internal int FdXmainP;
        protected internal int FdYmainP;

        // MAPOBJ.PAS:1256
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FdXImage, 12);
            Stream.rwInteger(ref FdYImage, 13);
            Stream.rwInteger(ref FdXmainP, 14);
            Stream.rwInteger(ref FdYmainP, 15);
        }

        // MAPOBJ.PAS:1232

        // MAPOBJ.PAS:1244
        public virtual void SetdXmainP(int Value)
        {
            FdXmainP = Value;
            CalcShowPriorityOffset(FdXmainP, FdYmainP, FLevel);
        }

        // MAPOBJ.PAS:1250
        public virtual void SetdYmainP(int Value)
        {
            FdYmainP = Value;
            CalcShowPriorityOffset(FdXmainP, FdYmainP, FLevel);
        }

        public int dXImage { get => FdXImage; set => FdXImage = value; }
        public int dYImage { get => FdYImage; set => FdYImage = value; }
        public int dXmainP { get => FdXmainP; set => SetdXmainP(value); }
        public int dYmainP { get => FdYmainP; set => SetdYmainP(value); }

    }
}

namespace Aow.Game.World.Map
{

    // the base of the terrain hexagons
    public partial class TAbstractHexagon : Aow.Game.World.TMapObject
    {
        public new const int ClassID = 65904;

        public TAbstractHexagon(TECustomNode owner) : base(owner)
        { }

        // MAPOBJ.PAS:682
        public override byte GetLevel() => 0;

        // MAPOBJ.PAS:671
        public override int GetShowPriority()
        {
            int Xhp;
            int Yhp;
            Xhp = ((TMapField)Owner).FXhx * 32 + 8;
            Yhp = (((TMapField)Owner).FXhx & 1) * 16 + ((TMapField)Owner).FYhx * 32;
            return Xhp + (Yhp << 13);
        }

        // MAPOBJ.PAS:704
        public override void ChangeTerrain(in THSTerrainInfo Msg)
        {
            THexagonControl Control;
            Aow.Game.World.TMapObjectResource R;
            if (Msg.HN == 0 && GetTerrain(0) != Msg.TerrainType)
            {
                Control = ((TAbstractHexagonResource)FResource).GetHexagonControl();
                if (Control != null)
                {
                    var classID = FResource.EClass.ClassID;
                    R = Control.GetRndResource(Msg.TerrainType, classID);
                    if (R != null)
                    {
                        SetResource(R);
                        UpdateTransitions();
                        var lhx = GetLhx();
                        var yhx = GetYhx();
                        var xhx = GetXhx();
                        FResource.FEngine.FHSMap.FMapContainer.FMap.InvalidateStaticScene(xhx, yhx, lhx);
                    }
                    else
                    {
                        var lhx = GetLhx();
                        var yhx = GetYhx();
                        var xhx = GetXhx();
                        ((TAbstractHexagonResource)FResource).GetHexagonControl().Place((TMapContainer)Owner.Owner.Owner, xhx, yhx, lhx, Msg.TerrainType);
                    }
                }
            }
        }

        // MAPOBJ.PAS:728
        public override void TerrainChanged(in THSTerrainInfo Msg)
        {
            ChangeTerrain(Msg);
        }

        // MAPOBJ.PAS:762
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            if (Msg.MsgID != 16)
            {
                base.MapFieldMsgProc(ref Msg);
            }
        }

        // MAPOBJ.PAS:698
        public override void Activate()
        {
            base.Activate();
            UpdateTransitions();
        }

        // MAPOBJ.PAS:691
        public override void SetResource(Aow.Game.World.TMapObjectResource Value)
        {
            base.SetResource(Value);
            if (FStatus.Contains(Aow.Game.World.TMapObjectStatusElement.moActivated))
            {
                UpdateTransitions();
            }
        }

        // MAPOBJ.PAS:733
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result;
            TMapField MapField;
            THexagonSprite HS;
            if (Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < MapContainer.FWidth && Yhx < MapContainer.FHeight && Lhx < MapContainer.FLevelCount)
            {
                MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
                HS = MapField.GetMaxLevelNoneTransparentHS();
                bool condition = HS == null;
                if (!condition)
                {
                    bool test = HS.GetLevel() < 10;
                    if (!test)
                    {
                        var overlay = GetOverlay(0);
                        test = MapField.CanChangeTerrain(this, GetTerrain(0), overlay);
                    }

                    condition = test && !(HS is TAbstractBorderHexagon);
                }

                Result = condition;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // MAPOBJ.PAS:750
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            THexagonSprite HS;
            HS = MapContainer.GetField(Xhx, Yhx, Lhx).FindOwnedHS(typeof(TAbstractHexagon));
            if (HS != null)
            {
                HS.Dispose();
            }

            bool Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
            if (Result)
            {
                UpdateTransitions();
            }

            return Result;
        }

        // MAPOBJ.PAS:687
        public virtual void UpdateTransitions()
        {
        }

    }

    public partial class TAbstractHexagonResource : Aow.Game.World.TMapObjectResource
    {

        public TAbstractHexagonResource(TECustomNode owner) : base(owner)
        {
            SetLevel(0);
        }

        // MAPOBJ.PAS:770

        // MAPOBJ.PAS:781
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 65556)
            {
                if (GetHexagonControl() != null)
                {
                    GetHexagonControl().RegisterResource(this);
                }
            }
        }

        // MAPOBJ.PAS:776
        public virtual THexagonControl GetHexagonControl() => FEngine.FHSSet.FHexagonControl;

    }

    public partial class TAbstractBorderHexagon : TAbstractHexagon
    {
        public new const int ClassID = 65905;

        public TAbstractBorderHexagon(TECustomNode owner) : base(owner)
        { }

    }

    public partial class TAbstractBorderHexagonResource : TAbstractHexagonResource
    {
        public TAbstractBorderHexagonResource(TECustomNode owner) : base(owner)
        { }

    }

    // the base of roads and bridges
    public partial class TAbstractRoad : Aow.Game.World.TMapObject
    {
        public new const int ClassID = 65906;

        public TAbstractRoad(TECustomNode owner) : base(owner)
        { }

        // MAPOBJ.PAS:803
        public override byte GetLevel() => 10;

        // MAPOBJ.PAS:808
        public override int GetShowPriority()
        {
            int Xhp;
            int Yhp;
            Xhp = ((TMapField)Owner).FXhx * 32 + 8;
            Yhp = (((TMapField)Owner).FXhx & 1) * 16 + ((TMapField)Owner).FYhx * 32;
            return Xhp + ((Yhp << 13) + 671088640);
        }

        // MAPOBJ.PAS:848
        public override void ChangeTerrain(in THSTerrainInfo Msg)
        {
            Aow.Game.World.TMapObjectResource R;
            if (Msg.HN == 0 && GetTerrain(0) != Msg.TerrainType)
            {
                var classID = FResource.EClass.ClassID;
                R = ((TAbstractRoadResource)FResource).GetRoadControl().GetRndResource(Msg.TerrainType, classID);
                if (R != null)
                {
                    SetResource(R);
                    UpdateTransitions();
                    var lhx = GetLhx();
                    var yhx = GetYhx();
                    var xhx = GetXhx();
                    FResource.FEngine.FHSMap.FMapContainer.FMap.InvalidateStaticScene(xhx, yhx, lhx);
                }
                else
                {
                    var lhx = GetLhx();
                    var yhx = GetYhx();
                    var xhx = GetXhx();
                    if (((TAbstractRoadResource)FResource).GetRoadControl().Place((TMapContainer)Owner.Owner.Owner, xhx, yhx, lhx, Msg.TerrainType) == null)
                    {
                        Dispose();
                    }
                }
            }
        }

        // MAPOBJ.PAS:843
        public override void NeighbourTerrainChanged(int HN, in THSTerrainInfo Msg)
        {
            UpdateTransitions();
        }

        // MAPOBJ.PAS:823
        public override void Activate()
        {
            UpdateTransitions();
        }

        // MAPOBJ.PAS:828
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result;
            THexagonSprite HS;
            bool condition = Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < MapContainer.FWidth && Yhx < MapContainer.FHeight && Lhx < MapContainer.FLevelCount;
            if (condition)
            {
                var fieldValue = MapContainer.GetField(Xhx, Yhx, Lhx);
                var terrain = FResource.FTerrainList.GetTerrain(0);
                condition = fieldValue.FTerrainType == terrain;
            }

            if (condition)
            {
                HS = MapContainer.GetField(Xhx, Yhx, Lhx).GetMaxLevelNoneTransparentHS();
                Result = HS == null || HS.GetLevel() < 10 || HS is TAbstractRoad;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // MAPOBJ.PAS:819
        public virtual void UpdateTransitions()
        {
        }

    }

    public partial class TAbstractRoadResource : Aow.Game.World.TMapObjectResource
    {
        public TAbstractRoadResource(TECustomNode owner) : base(owner)
        { }

        // MAPOBJ.PAS:874
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 65556)
            {
                GetRoadControl().RegisterResource(this);
            }

            if (Msg.MsgID == 65621)
            {
                Msg.Result = 6;
            }
        }

        // MAPOBJ.PAS:883
        public override Aow.Game.World.TMapObject PlaceResourceUser(THSMap HSMap, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Game.World.TMapObject Result = (Aow.Game.World.TMapObject)CreateResourceUser();
            Aow.Game.World.Map.HexGeometry.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            Xhp += 16;
            Yhp += 16;
            if (!HSMap.Place(Result, Xhp, Yhp, Lhx))
            {
                Result?.Dispose();
                Result = null;
            }

            return Result;
        }

        // MAPOBJ.PAS:869
        public virtual TRoadControl GetRoadControl() => FEngine.FHSSet.FRoadControl;
    }

    public delegate int TXYLSortCompare(TXYL Item1, TXYL Item2);

    // Engine's value list (Classes.TList adapted to a value type, with a default item for new and
    // out-of-range items and a run-length encoded stream form) for map locations
    public partial class TXYLList : TPersistent
    {
        internal TXYL[] FList = [];
        internal int FCount;
        internal int FCapacity;
        internal TXYL FDefault;

        // HSEngine.pas:1673
        internal TXYL Get(int Index)
        {
            TXYL Result;
            if (Index < 0 || Index >= FCount)
            {
                Result = FDefault;
            }
            else
            {
                Result = FList[Index];
            }

            return Result;
        }

        // HSEngine.pas:1746
        internal void Put(int Index, TXYL Item)
        {
            if (Index < 0)
            {
                Error("List index out of bounds (%d)", Index);
            }

            if (Index >= FCount)
            {
                SetCount(Index + 1);
            }

            FList[Index] = Item;
        }

        // HSEngine.pas:1767
        internal void SetCapacity(int NewCapacity)
        {
            if (NewCapacity < FCount || NewCapacity > Aow.Game.Engine.Nodes.EngineServices.MaxListSize)
            {
                Error("List capacity out of bounds (%d)", NewCapacity);
            }

            if (NewCapacity != FCapacity)
            {
                Array.Resize(ref FList, NewCapacity);
                FCapacity = NewCapacity;
            }
        }

        // HSEngine.pas:1778
        internal void SetCount(int NewCount)
        {
            if (NewCount < 0 || NewCount > Aow.Game.Engine.Nodes.EngineServices.MaxListSize)
            {
                Error("List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                FList.AsSpan(FCount, NewCount - FCount).Fill(FDefault);
            }

            FCount = NewCount;
        }

        // HSEngine.pas:1681
        protected internal virtual void Grow()
        {
            int Delta;
            if (FCapacity > 64)
            {
                Delta = FCapacity / 4;
            }
            else
            {
                if (FCapacity > 8)
                {
                    Delta = 16;
                }
                else
                {
                    Delta = 4;
                }
            }

            SetCapacity(FCapacity + Delta);
        }

        // HSEngine.pas:1578
        public TXYLList()
        {
            FDefault.X = -1;
            FDefault.Y = -1;
            FDefault.L = -1;
            FDefault.Flag = 0;
        }

        // HSEngine.pas:1587
        public override void Dispose()
        {
            Clear();
        }

        // HSEngine.pas:1592
        public override void Assign(TPersistent Source)
        {
            if (Source is TXYLList)
            {
                var xylList = (TXYLList)Source;
                this.FCount = xylList.FCount;
                this.SetCapacity(xylList.FCapacity);
                Array.Copy(xylList.FList, 0, this.FList, 0, this.FCount);
                this.FDefault = xylList.FDefault;
            }
            else
            {
                base.Assign(Source);
            }
        }

        // HSEngine.pas:1606
        public int Add(TXYL Item)
        {
            int Result = FCount;
            if (Result == FCapacity)
            {
                Grow();
            }

            FList[Result] = Item;
            FCount += 1;
            return Result;
        }

        // HSEngine.pas:1614
        public int AddXYL(sbyte X, sbyte Y, sbyte L)
        {
            int Result = FCount;
            if (Result == FCapacity)
            {
                Grow();
            }

            FList[Result].X = X;
            FList[Result].Y = Y;
            FList[Result].L = L;
            FList[Result].Flag = 0;
            FCount += 1;
            return Result;
        }

        // HSEngine.pas:1625
        public void Clear()
        {
            SetCount(0);
            SetCapacity(0);
        }

        // HSEngine.pas:1631
        public void Delete(int Index)
        {
            if (Index < 0 || Index >= FCount)
            {
                Error("List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                RtlUnit.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), (FCount - Index) * 4);
            }
        }

        // HSEngine.pas:1651
        public void Exchange(int Index1, int Index2)
        {
            TXYL Item;
            if (Index1 < 0 || Index1 >= FCount)
            {
                Error("List index out of bounds (%d)", Index1);
            }

            if (Index2 < 0 || Index2 >= FCount)
            {
                Error("List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // HSEngine.pas:1662
        public TXYLList Expand()
        {
            if (FCount == FCapacity)
            {
                Grow();
            }

            return this;
        }

        // HSEngine.pas:1668
        public TXYL First() => Get(0);

        // HSEngine.pas:1691
        public int IndexOf(TXYL Item)
        {
            int Result = 0;
            while (Result < FCount && Unsafe.As<TXYL, int>(ref FList[Result]) != Unsafe.As<TXYL, int>(ref Item))
            {
                Result += 1;
            }

            if (Result == FCount)
            {
                Result = -1;
            }

            return Result;
        }

        // HSEngine.pas:1698
        public int IndexOfXY(sbyte X, sbyte Y)
        {
            TXYL Item = default;
            Item.X = X;
            Item.Y = Y;
            int Result = 0;
            while (Result < FCount && FList[Result].XY != Item.XY)
            {
                Result += 1;
            }

            if (Result == FCount)
            {
                Result = -1;
            }

            return Result;
        }

        // HSEngine.pas:1709
        public int IndexOfXYL(sbyte X, sbyte Y, sbyte L)
        {
            int Result = 0;
            while (Result < FCount && (FList[Result].X != X || FList[Result].Y != Y || FList[Result].L != L))
            {
                Result += 1;
            }

            if (Result == FCount)
            {
                Result = -1;
            }

            return Result;
        }

        // HSEngine.pas:1717
        public void Insert(int Index, TXYL Item)
        {
            if (Index < 0 || Index > FCount)
            {
                Error("List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            if (Index < FCount)
            {
                RtlUnit.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), (FCount - Index) * 4);
            }

            FList[Index] = Item;
            FCount += 1;
        }

        // HSEngine.pas:1728
        public TXYL Last() => Get(FCount - 1);

        // HSEngine.pas:1733
        public void Move(int CurIndex, int NewIndex)
        {
            TXYL Item;
            if (CurIndex != NewIndex)
            {
                if (NewIndex < 0 || NewIndex >= FCount)
                {
                    Error("List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // HSEngine.pas:1753
        public int Remove(TXYL Item)
        {
            int Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // HSEngine.pas:1759
        public void Pack()
        {
            for (int I = FCount - 1; I >= 0; I--)
            {
                if (Unsafe.BitCast<TXYL, int>(this[I]) == Unsafe.As<TXYL, int>(ref FDefault))
                {
                    Delete(I);
                }
            }
        }

        // HSEngine.pas:1815
        public void Sort(TXYLSortCompare Compare)
        {
            if (FList.Length != 0 && Count > 0)
            {
                Aow.Game.World.Map.TXYLList.QuickSortXYL(FList, 0, Count - 1, Compare);
            }
        }

        // HSEngine.pas:1821
        public bool Compare(TXYLList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 4);

        // HSEngine.pas:1828
        public void LoadFromStream(Stream Stream)
        {
            int Count = default;
            int Run = default;
            TXYL Item = default;
            Clear();
            Stream.ReadBytes(Bytes.Of(ref FDefault, 4));
            Stream.ReadBytes(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (Count != FCount)
            {
                Stream.ReadBytes(Bytes.Of(ref Item, 4));
                if (Unsafe.As<TXYL, int>(ref Item) == Unsafe.As<TXYL, int>(ref FDefault))
                {
                    Stream.ReadBytes(Bytes.Of(ref Run, 4));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // HSEngine.pas:1850
        public void SaveToStream(Stream Stream)
        {
            int I;
            int Run = default;
            int DefaultRun()
            {
                int J;
                int Result = 0;
                J = I;
                while (J < FCount && Unsafe.As<TXYL, int>(ref FList[J]) == Unsafe.As<TXYL, int>(ref FDefault))
                {
                    Result += 1;
                    J += 1;
                }

                return Result;
            }

            I = 0;
            Stream.Write(Bytes.Of(ref FDefault, 4));
            Stream.Write(Bytes.Of(ref FCount, 4));
            while (FCount != I)
            {
                Run = DefaultRun();
                if (Run > 0)
                {
                    Stream.Write(Bytes.Of(ref FDefault, 4));
                    Stream.Write(Bytes.Of(ref Run, 4));
                    I += Run;
                }
                else
                {
                    Stream.Write(MemoryMarshal.AsBytes(FList.AsSpan(I, 1)));
                    I += 1;
                }
            }
        }

        public int Capacity { get => FCapacity; set => SetCapacity(value); }
        public int Count { get => FCount; set => SetCount(value); }
        public TXYL Default { get => FDefault; set => FDefault = value; }

        public TXYL this[int Index] { get => Get(Index); set => Put(Index, value); }

        public TXYL[] List { get => FList; }

        // HSEngine.pas:1640
        protected internal static void Error(string Msg, int Data)
        {
            throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
        }

        private static void QuickSortXYL(Span<TXYL> SortList, int L, int R, TXYLSortCompare SCompare)
        {
            int I;
            int J;
            TXYL P;
            TXYL T = default;
            do
            {
                I = L;
                J = R;
                P = SortList[(int)(((uint)(L + R)) >> 1)];
                do
                {
                    while (SCompare(SortList[I], P) < 0)
                    {
                        I += 1;
                    }

                    while (SCompare(SortList[J], P) > 0)
                    {
                        J -= 1;
                    }

                    if (I <= J)
                    {
                        T = SortList[I];
                        SortList[I] = SortList[J];
                        SortList[J] = T;
                        I += 1;
                        J -= 1;
                    }
                }
                while (!(I > J));
                if (L < J)
                {
                    QuickSortXYL(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        internal static bool LocationsConnected(TXYLList List)
        {
            bool Result = default;
            TXYLList Locations;
            TXYL XYL = default;
            void Connect(TXYL XYL)
            {
                THNWalkData Walk = default;
                Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, XYL.X, XYL.Y, 1);
                while (Walk.Radius <= 1)
                {
                    for (int I = 0, last = Locations.Count - 1; I <= last; I++)
                    {
                        if (Locations[I].Flag == 0)
                        {
                            var item = Locations[I];
                            var packXYL = Aow.Game.World.Map.HexGeometry.PackXYL((sbyte)Walk.X, (sbyte)Walk.Y, XYL.L);
                            if (Unsafe.BitCast<TXYL, int>(item) == Unsafe.BitCast<TXYL, int>(packXYL))
                            {
                                Locations.FList[I].Flag = 1;
                                Connect(Locations[I]);
                            }
                        }
                    }

                    Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
                }
            }

            Locations = new TXYLList();
            for (int I = 0, last = List.Count - 1; I <= last; I++)
            {
                XYL = List[I];
                XYL.Flag = 0;
                if (Locations.IndexOf(XYL) == -1)
                {
                    Locations.Add(XYL);
                }
            }

            if (Locations.Count > 0)
            {
                if (Locations.Count > 1)
                {
                    Locations.FList[0].Flag = 1;
                    Connect(Locations[0]);
                    Result = true;
                    for (int I = 0, last = Locations.Count - 1; I <= last; I++)
                    {
                        if (Locations[I].Flag != 1)
                        {
                            Result = false;
                        }
                    }
                }
                else
                {
                    Result = true;
                }
            }
            else
            {
                Result = false;
            }

            Locations?.Dispose();
            return Result;
        }

    }

    // { an XYL value (TMapField.GetXYL, PackXYL) as its bytes }
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct TXYLRec
    {
        [FieldOffset(0)]
        public sbyte X;
        [FieldOffset(1)]
        public sbyte Y;
        [FieldOffset(2)]
        public sbyte L;
        [FieldOffset(3)]
        public sbyte Reserved;
    }

    // The layout of TMapField up to its terrain and overlay types, which THSMap.CanChangeTerrain
    // saves and restores as one Word (the original reads word ptr [MapF])
    public partial class TMapFieldTandO : TECustomNode
    {
        public TMapFieldTandO(TECustomNode owner) : base(owner)
        { }

        internal object FHSList;
        internal int FCounts;
        internal int FXYL;
        internal ushort FTandO;
    }

    // { The header of an .hss file (52 bytes): the set follows at Size, a CRC32 of everything before it
    //     closes the file }
    [StructLayout(LayoutKind.Explicit, Size = 52)]
    public struct THSSHeader
    {
        [FieldOffset(0)]
        public int Size;
        [FieldOffset(4)]
        public FixedArray_byte_5 ID__image;

        public string ID { get => ReadShortString(ID__image); set => WriteShortString(ID__image, value); }

        [FieldOffset(9)]
        public FixedArray_byte_41 Name__image;

        public string Name { get => ReadShortString(Name__image); set => WriteShortString(Name__image, value); }
    }

    // { The header of an .hsm file (16 bytes) in front of the stored map, inside the compressed stream }
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct THSMHeader
    {
        [FieldOffset(0)]
        public int Size;
        [FieldOffset(4)]
        public FixedArray_byte_4 ID;
        [FieldOffset(8)]
        public int Reserved;
        [FieldOffset(12)]
        public int ClassID;
    }

    [LegacyObject]
    public sealed class TerrainChangedNotification
    {
        public TTerrainChangedInfo Info;

        public TerrainChangedNotification()
        {
            CaptureAddress.Assign(this);
        }
    }
}
