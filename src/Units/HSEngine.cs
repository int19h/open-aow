// Translated from HSEngine.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.GFXE;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;

namespace Aow.Units.HSEngine
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

    public enum TLibraryImageHSStatusElement : byte
    {
        lsVisible = 0,
        lsEditMode = 1,
        lsDestroyOnShow = 2,
        lsMapIsOwner = 3,
        lsPlacedOnMap = 4
    }

    public enum TMapObjectStatusElement : byte
    {
        moVisible = 0,
        moResourceLinked = 1,
        moEditMode = 2,
        moPlacedOnMap = 3,
        moSelected = 4,
        moActivated = 5
    }

    public delegate bool TWalkAroundRouteCallBack(int X, int Y, ref bool Cancel);
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
        // HSEngine.pas:1919
        public override void FreeInstance()
        {
        }

        // HSEngine.pas:1907
        public override void Destroy()
        {
            Array.Resize(ref FHSList, 0);
            FCount = 0;
        }

        // HSEngine.pas:1928
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            TSet<TEControlStyleElement> Result = default;
            Result = TSet<TEControlStyleElement>.Of(csStored, csShared);
            return Result;
        }

        // HSEngine.pas:1923
        public override void SetOwner(TECustomNode AOwner)
        {
            FOwner = AOwner;
        }

        // HSEngine.pas:2029
        public override void ReadWrite(TEStorageStream Stream)
        {
            int J = default;
            int I = default;
            TEObject HS = default;
            if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(smText)) == TSet<TStorageModeElement>.Of())
            {
                if (Stream.StorageMode().Contains(TStorageModeElement.smRead))
                {
                    if (Stream.StorageMode().Contains(TStorageModeElement.smCompare))
                    {
                        J = 0;
                        {
                            I = Stream.PropertyTable.First;
                            int end__1 = Stream.PropertyTable.Count - 1;
                            for (; I <= end__1; I += 1)
                            {
                                if (Stream.PropertyTable.Items[I].ID >= 50)
                                {
                                    HS = FHSList[J];
                                    Stream.rwCreateEObject(ref HS, this, Stream.PropertyTable.Items[I].ID);
                                    J += 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        {
                            I = Stream.PropertyTable.First;
                            int end__2 = Stream.PropertyTable.Count - 1;
                            for (; I <= end__2; I += 1)
                            {
                                J = Stream.PropertyTable.Items[I].ID;
                                if (J >= 50)
                                {
                                    Stream.rwCreateEObject(ref HS, this, J);
                                }
                            }
                        }
                    }
                }
                else
                {
                    I = 50;
                    {
                        J = 0;
                        int end__3 = FCount - 1;
                        for (; J <= end__3; J += 1)
                        {
                            HS = FHSList[J];
                            bool bool__4 = HS != null && HS.Owner == this;
                            if (bool__4)
                            {
                                var call__5 = HS.GetControlStyle();
                                bool__4 = !call__5.Contains(csShared);
                            }

                            bool bool__6 = bool__4;
                            if (bool__6)
                            {
                                var call__7 = HS.GetControlStyle();
                                bool__6 = call__7.Contains(csStored);
                            }

                            if (bool__6)
                            {
                                Stream.rwCreateEObject(ref HS, this, I);
                                I += 1;
                            }
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
            TEObject Result = default;
            if (Index >= FCount)
            {
                throw Exception.Create((AnsiString)"Index out of bounds");
            }

            Result = FHSList[Index];
            return Result;
        }

        // HSEngine.pas:1949
        public override void SetChild(int Index, TEObject Child)
        {
            if (Index >= FCount)
            {
                throw Exception.Create((AnsiString)"Index out of bounds");
            }

            FHSList[Index] = (THexagonSprite)Child;
        }

        // HSEngine.pas:1972
        public override int GetCount()
        {
            int Result = default;
            Result = FCount;
            return Result;
        }

        // HSEngine.pas:2226
        public override int RemoveChild(TEObject Child)
        {
            int Result = default;
            Result = RemoveHS((THexagonSprite)Child);
            return Result;
        }

        // HSEngine.pas:2221
        public override int AddChild(TEObject Child)
        {
            int Result = default;
            Result = AddHS((THexagonSprite)Child, 0);
            return Result;
        }

        // HSEngine.pas:2154
        public override TEObject FindChild(int AClassID)
        {
            TEObject Result = default;
            int I = default;
            I = 0;
            while (true)
            {
                bool bool__8 = I < FCount;
                if (bool__8)
                {
                    var call__9 = FHSList[I].EClass.ClassID;
                    bool__8 = call__9 != AClassID;
                }

                if (!bool__8)
                    break;
                {
                    I += 1;
                }
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
            TEObject Result = default;
            int I = default;
            I = 0;
            while (true)
            {
                bool bool__10 = I < FCount;
                if (bool__10)
                {
                    var call__11 = FHSList[I].EClass.ClassID;
                    bool__10 = !(call__11 == AClassID && FHSList[I].Owner == this);
                }

                if (!bool__10)
                    break;
                {
                    I += 1;
                }
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
        public int GetXYL()
        {
            int Result = default;
            Result = FXhx + (FYhx << 8) + (FLhx << 16);
            return Result;
        }

        // HSEngine.pas:1955
        public int HSListDataSize()
        {
            int Result = default;
            Result = (FCapacity << 2) + ((FCapacity + 3) & 252);
            return Result;
        }

        // HSEngine.pas:2163
        public THexagonSprite FindHS(Type AClass)
        {
            THexagonSprite Result = default;
            int I = default;
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
            THexagonSprite Result = default;
            int I = default;
            THexagonSprite HS = default;
            I = 0;
            Result = null;
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
            int I = default;
            {
                I = FCount - 1;
                byte end__12 = FStaticCount;
                for (; I >= end__12; I -= 1)
                {
                    FHSList[I].MakeVisible(List);
                }
            }
        }

        // HSEngine.pas:2308
        public void CopyStaticVisibleHS(TVisibilityList List)
        {
            int I = default;
            {
                I = FStaticCount - 1;
                byte end__13 = 0;
                for (; I >= end__13; I -= 1)
                {
                    FHSList[I].MakeVisible(List);
                }
            }
        }

        // HSEngine.pas:2316
        public void ResetDynamicVisibleHS()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__14 = FStaticCount;
                for (; I >= end__14; I -= 1)
                {
                    FHSList[I].SetVisible(false);
                }
            }
        }

        // HSEngine.pas:2324
        public void ResetStaticVisibleHS()
        {
            int I = default;
            {
                I = FStaticCount - 1;
                byte end__15 = 0;
                for (; I >= end__15; I -= 1)
                {
                    FHSList[I].SetVisible(false);
                }
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
            bool Done = default;
            int I = default;
            int Level = default;
            object DonePtr = default;
            HSEngine.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            I = FCount - 1;
            ;
            // { Preserve the original stack slot. }
            Done = false;
            while (!Done && I >= 0)
            {
                var call__16 = FHSList[I].GetLevel();
                Level = call__16;
                if (Level <= MaxLevel)
                {
                    if (Level >= MinLevel)
                    {
                        if (FHSList[I].Owner is TMapField)
                        {
                            FHSList[I].Destroy();
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
            int I = default;
            {
                I = FCount - 1;
                byte end__17 = 0;
                for (; I >= end__17; I -= 1)
                {
                    if (I <= FCount - 1)
                    {
                        if (FHSList[I].Owner == this)
                        {
                            FHSList[I].Destroy();
                        }
                    }
                }
            }
        }

        // HSEngine.pas:2369
        public void Activate()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__18 = 0;
                for (; I >= end__18; I -= 1)
                {
                    if (FHSList[I].Owner == this)
                    {
                        FHSList[I].Activate();
                    }
                }
            }
        }

        // HSEngine.pas:2581
        public sbyte GetDecorationType()
        {
            sbyte Result = default;
            TMapFieldMsg Msg = default;
            Msg.MsgID = 17;
            Msg.Result = -1;
            SendMapFieldMsg(ref Msg);
            Result = (sbyte)Msg.Result.Value;
            return Result;
        }

        // HSEngine.pas:2591
        public void UpdateNeighbourTerrain()
        {
            THSTerrainInfo Info = default;
            THNWalkData Walk = default;
            int I = default;
            Info.MapField = this;
            Info.TerrainType = FTerrainType;
            Info.OverlayType = FOverlayType;
            HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            {
                I = 1;
                byte end__19 = 6;
                for (; I <= end__19; I += 1)
                {
                    HSEngine.WalkToNextHN(ref Walk);
                    if (Walk.X >= 0 && Walk.X < ((TMapLevel)Owner).FWidth && Walk.Y >= 0 && ((TMapLevel)Owner).FHeight > Walk.Y)
                    {
                        Info.HN = (sbyte)HSEngine.OppositeHTTable[I - 1];
                        var call__20 = ((TMapLevel)Owner).GetField(Walk.X, Walk.Y);
                        call__20.NeighbourTerrainChanged(ref Info);
                    }
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
        public override TEngine GetEngine()
        {
            TEngine Result = default;
            Result = FEngine;
            return Result;
        }

        // HSEngine.pas:3173
        public override void ReadWrite(TEStorageStream Stream)
        {
            byte W = default;
            byte H = default;
            byte L = default;
            base.ReadWrite(Stream);
            if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, smText)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead))
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
                FEngine = Msg.Engine;
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
                ((TMapLevel)FChildren.List[Index]).Free();
                FLevelCount -= 1;
            }
        }

        // HSEngine.pas:3259
        public virtual bool AddLevel(int AClassID)
        {
            bool Result = default;
            EClass LevelClass = default;
            TMapLevel Level = default;
            var call__21 = FEngine.GetEClass(AClassID);
            LevelClass = call__21;
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
            EClass LevelClass = default;
            int I = default;
            TMapLevel Level = default;
            Clear();
            try
            {
                var call__22 = FEngine.GetEClass(Settings.MapLevelClassID);
                LevelClass = call__22;
                {
                    I = 0;
                    int end__23 = Settings.Levels - 1;
                    for (; I <= end__23; I += 1)
                    {
                        Level = (TMapLevel)LevelClass.Create(this);
                        Level.FLhx = I;
                        FEngine.ActivateEObject(Level);
                        Level.New(Settings);
                    }
                }

                FWidth = Settings.Width;
                FHeight = Settings.Height;
                FLevelCount = Settings.Levels;
            }
            catch (System.Exception)
            {
                Clear();
                throw;
            }
        }

        // HSEngine.pas:3323
        public virtual void UpdateTerrain()
        {
            int I = default;
            {
                I = 0;
                int end__24 = FLevelCount - 1;
                for (; I <= end__24; I += 1)
                {
                    ((TMapLevel)FChildren.List[I]).UpdateTerrain();
                }
            }
        }

        // HSEngine.pas:3224
        public TMapLevel GetMapLevel(int Index)
        {
            TMapLevel Result = default;
            Result = (TMapLevel)FChildren.List[Index];
            return Result;
        }

        // HSEngine.pas:3331
        public void Loaded()
        {
            int I = default;
            {
                I = 0;
                int end__25 = FLevelCount - 1;
                for (; I <= end__25; I += 1)
                {
                    ((TMapLevel)FChildren.List[I]).Loaded();
                }
            }
        }

        // HSEngine.pas:3339
        public void Activate()
        {
            int I = default;
            {
                I = 0;
                int end__26 = FLevelCount - 1;
                for (; I <= end__26; I += 1)
                {
                    ((TMapLevel)FChildren.List[I]).Activate();
                }
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
            int Result = default;
            var hexagonSprite = (THexagonSprite)Child;
            if (hexagonSprite.GetVisible())
            {
                hexagonSprite.SetVisible(false);
            }

            Result = base.RemoveChild(Child);
            return Result;
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
            var call__33 = CreateMouse();
            FMouse = call__33;
            FMouse.FHSMap = this;
            var call__34 = new THexagonSpriteSelection(this);
            FSelection = call__34;
            FSelection.SetControlStyle(TSet<TEControlStyleElement>.Of(csShared));
            var call__35 = new TVisibilityList(this);
            FStaticVisibleHS = call__35;
            FStaticVisibleHS.SetControlStyle(TSet<TEControlStyleElement>.Of(csShared));
            var call__36 = new TVisibilityList(this);
            FDynamicVisibleHS = call__36;
            FDynamicVisibleHS.SetControlStyle(TSet<TEControlStyleElement>.Of(csShared));
            var call__37 = new TVisibilityList(this);
            FStaticFixedVisibleHS = call__37;
            FStaticFixedVisibleHS.SetControlStyle(TSet<TEControlStyleElement>.Of(csShared));
            var call__38 = new TVisibilityList(this);
            FDynamicFixedVisibleHS = call__38;
            FDynamicFixedVisibleHS.SetControlStyle(TSet<TEControlStyleElement>.Of(csShared));
            var call__39 = new TETimerNode(this);
            FTimer = call__39;
            var call__40 = new TEventList();
            FHSChangedEvents = call__40;
            var call__41 = new TEventList();
            FTerrainChangedEvents = call__41;
            var call__42 = new TEventList();
            FViewPortChangedEvents = call__42;
            var call__43 = new TEventList();
            FNewFrameEvents = call__43;
            var call__44 = new TEventList();
            FDestroyEvents = call__44;
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
        internal AnsiString FHSSFileName;
        internal TSet<THSMapStatusElement> FStatus;
        internal int FFrame;
        internal TEventList FTerrainChangedEvents;
        internal TCenterViewEvent FOnCenterView;
        internal TChangeViewEvent FOnChangeView;
        internal TViewLevelEvent FOnViewLevel;
        internal Engine.TProgressEvent FOnProgress;
        internal int FTerrainChangedLockCounter;
        internal TEventList FNewFrameEvents;
        internal TEventList FViewPortChangedEvents;
        internal TEventList FDestroyEvents;
        internal TEventList FHSChangedEvents;
        internal AnsiString FFileName;
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
        public override void Destroy()
        {
            TEMsg Msg = default;
            LockTerrainChangedEvent();
            FStatus = FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msDestroying);
            FSelection.FOnSelect.Clear();
            FSelection.FOnDeselect.Clear();
            FSelection.SetControlStyle(TSet<TEControlStyleElement>.Of(csShared));
            FSelection.UnselectAll();
            FDestroyEvents.TriggerNotifyEvents(this);
            FHSChangedEvents.Clear();
            FStaticVisibleHS.QuickClear();
            FDynamicVisibleHS.QuickClear();
            FStaticFixedVisibleHS.QuickClear();
            FDynamicFixedVisibleHS.QuickClear();
            InitMsg(ref Msg, 536936471, this);
            SendMsg(ref Msg, this);
            FNewFrameEvents.Clear();
            base.Destroy();
            FHSChangedEvents.Free();
            FTerrainChangedEvents.Free();
            FViewPortChangedEvents.Free();
            FNewFrameEvents.Free();
            FMouse.Free();
            FDestroyEvents.Free();
            UnlockTerrainChangedEvent();
        }

        // HSMAP.PAS:76
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            TSet<TEControlStyleElement> Result = default;
            Result = base.GetControlStyle() + TSet<TEControlStyleElement>.Of(csReserved2);
            return Result;
        }

        // HSMAP.PAS:240
        public override void ReadWrite(TEStorageStream Stream)
        {
            sbyte B = default;
            base.ReadWrite(Stream);
            if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, smPartial)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, smPartial))
            {
                FStatus = FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msPreviewMode);
            }

            if (!Stream.StorageMode().Contains(smText))
            {
                Stream.rwString(ref FHSSFileName, 2);
            }

            var call__28 = Stream.StorageMode();
            bool bool__29 = !call__28.Contains(smPartial);
            if (bool__29)
            {
                var call__30 = Stream.StorageMode();
                bool__29 = call__30.Contains(TStorageModeElement.smRead);
            }

            if (bool__29 && FHSSFileName != (AnsiString)"")
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
            bool bool__31 = !FStatus.Contains(THSMapStatusElement.msPreviewMode);
            if (bool__31)
            {
                var call__32 = Stream.StorageMode();
                bool__31 = (call__32 & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, smText, TStorageModeElement.smSetupCompare)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead);
            }

            if (bool__31)
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
                FEngine = (THSEngine)Msg.Engine;
            }

            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 536870920)
            {
                FMouse.Reset();
            }
        }

        // HSMAP.PAS:71
        public virtual THSMapMouse CreateMouse()
        {
            THSMapMouse Result = default;
            Result = new THSMapMouse();
            return Result;
        }

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
            var call__45 = FMapContainer.GetMapLevel(L);
            Info.MapLevel = call__45;
            FTerrainChangedEvents.TriggerNotifyEvents(notification);
        }

        // HSMAP.PAS:705
        public virtual void DrawStaticHS(ref TSurfaceDesc SD)
        {
            int I = default;
            if (FStaticSceneInvalid)
            {
                throw Exception.Create((AnsiString)"Static scene is invalid, call UpdateStaticScene!");
            }

            {
                I = FStaticVisibleHS.FCount - 1;
                byte end__46 = 0;
                for (; I >= end__46; I -= 1)
                {
                    ((THexagonSprite)FStaticVisibleHS.FItems[I].Child).Show(FViewXhp, FViewYhp, SD);
                }
            }
        }

        // HSMAP.PAS:725
        public virtual void DrawDynamicHS(ref TSurfaceDesc SD)
        {
            int I = default;
            if (FDynamicSceneInvalid)
            {
                throw Exception.Create((AnsiString)"Dynamic scene is invalid, call UpdateDynamicScene!");
            }

            {
                I = FDynamicVisibleHS.FCount - 1;
                byte end__47 = 0;
                for (; I >= end__47; I -= 1)
                {
                    ((THexagonSprite)FDynamicVisibleHS.FItems[I].Child).Show(FViewXhp, FViewYhp, SD);
                }
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
        public virtual void FillLevelWithResource(int L, TMapObjectResource Resource)
        {
            int X = default;
            int Y = default;
            {
                Y = 0;
                int end__48 = FMapContainer.FHeight - 1;
                for (; Y <= end__48; Y += 1)
                {
                    {
                        X = 0;
                        int end__49 = FMapContainer.FWidth - 1;
                        for (; X <= end__49; X += 1)
                        {
                            Resource.PlaceResourceUser(this, (sbyte)X, (sbyte)Y, (sbyte)L);
                        }
                    }
                }
            }
        }

        // HSMAP.PAS:96
        public virtual bool AddMapLevel()
        {
            bool Result = default;
            var call__50 = FMapContainer.AddLevel(65794);
            Result = call__50;
            return Result;
        }

        // HSMAP.PAS:101
        public virtual void Activate()
        {
            if (FStatus.Contains(THSMapStatusElement.msActivated))
            {
                throw Exception.Create((AnsiString)"HSMap already activated");
            }

            FStatus = FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msActivated);
        }

        // HSMAP.PAS:108
        public virtual void Deactivate()
        {
            if (!FStatus.Contains(THSMapStatusElement.msActivated))
            {
                throw Exception.Create((AnsiString)"HSMap wasn't activated");
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
            HSEngine.HPtoHX(FViewXhp + (int)(((uint)FViewWidth) >> 1), FViewYhp + (int)(((uint)FViewHeight) >> 1), ref Xhx, ref Yhx);
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
                var call__51 = FEngine.CreateEObject(Settings.MapContainerClassID, this);
                FMapContainer = (TMapContainer)call__51;
                FEngine.ActivateEObject(FMapContainer);
                FMapContainer.New(Settings);
                Result = true;
                InitializeNewMap();
                Activate();
            }
            catch (System.Exception)
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
            int Result = default;
            TShortIntList List = default;
            int Count = default;
            TMapField MapF = default;
            THNWalkData Walk = default;
            TRect R = default;
            TMapLevel MapLevel = default;
            LockTerrainChangedEvent();
            List = new TShortIntList();
            var call__52 = FMapContainer.GetMapLevel(L);
            MapLevel = call__52;
            Count = 0;
            HSEngine.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= Radius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    var call__53 = MapLevel.GetField(Walk.X, Walk.Y);
                    MapF = call__53;
                    List[Walk.HN] = MapF.FTerrainType;
                    MapF.FTerrainType = (sbyte)TerrainType;
                }

                HSEngine.WalkToNextHN(ref Walk);
            }

            HSEngine.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= Radius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    var call__54 = MapLevel.GetField(Walk.X, Walk.Y);
                    MapF = call__54;
                    if (!MapF.CanChangeTerrainEx(MO))
                    {
                        var call__55 = List[Walk.HN];
                        MapF.FTerrainType = call__55;
                        List[Walk.HN] = -127;
                    }
                }

                HSEngine.WalkToNextHN(ref Walk);
            }

            HSEngine.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.Radius <= Radius)
            {
                var call__56 = List[Walk.HN];
                if (call__56 != -127 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    var call__57 = MapLevel.GetField(Walk.X, Walk.Y);
                    call__57.FTerrainType = (sbyte)TerrainType;
                    var call__58 = MapLevel.GetField(Walk.X, Walk.Y);
                    call__58.TerrainChanged();
                    Count += 1;
                }

                HSEngine.WalkToNextHN(ref Walk);
            }

            UnlockTerrainChangedEvent();
            List.Free();
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
            return Result;
        }

        // HSMAP.PAS:357
        public virtual int ChangeTerrainEx(TEObject MO, int X, int Y, int L, int Radius, TChangeTerrainProc ChangeTerrainProc, TTerrainChangedProc TerrainChangedProc)
        {
            int Result = default;
            TShortIntList OrgList = default;
            TShortIntList NewList = default;
            TBitList Changed = default;
            int Count = default;
            sbyte TerrainType = default;
            sbyte OverlayType = default;
            TMapField MapF = default;
            THNWalkData Walk = default;
            TRect R = default;
            TMapLevel MapLevel = default;
            if (ChangeTerrainProc != null)
            {
                LockTerrainChangedEvent();
                OrgList = new TShortIntList();
                NewList = new TShortIntList();
                Changed = new TBitList();
                var call__59 = FMapContainer.GetMapLevel(L);
                MapLevel = call__59;
                Count = 0;
                HSEngine.InitHNWalk(ref Walk, X, Y, 0);
                while (Walk.Radius <= Radius)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                    {
                        var call__60 = MapLevel.GetField(Walk.X, Walk.Y);
                        MapF = call__60;
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

                    HSEngine.WalkToNextHN(ref Walk);
                }

                HSEngine.InitHNWalk(ref Walk, X, Y, 0);
                while (Walk.Radius <= Radius)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                    {
                        var call__61 = MapLevel.GetField(Walk.X, Walk.Y);
                        MapF = call__61;
                        if (!MapF.CanChangeTerrainEx(MO))
                        {
                            var call__62 = OrgList[Walk.HN * 2];
                            MapF.FTerrainType = call__62;
                            var call__63 = OrgList[Walk.HN * 2 + 1];
                            MapF.FOverlayType = call__63;
                            Changed[Walk.HN] = false;
                        }
                        else
                        {
                            Changed[Walk.HN] = true;
                        }
                    }

                    HSEngine.WalkToNextHN(ref Walk);
                }

                HSEngine.InitHNWalk(ref Walk, X, Y, 0);
                while (Walk.Radius <= Radius)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                    {
                        var call__64 = Changed[Walk.HN];
                        if (call__64)
                        {
                            var call__65 = NewList[Walk.HN * 2];
                            var call__66 = MapLevel.GetField(Walk.X, Walk.Y);
                            call__66.FTerrainType = call__65;
                            var call__67 = NewList[Walk.HN * 2 + 1];
                            var call__68 = MapLevel.GetField(Walk.X, Walk.Y);
                            call__68.FOverlayType = call__67;
                            var call__69 = MapLevel.GetField(Walk.X, Walk.Y);
                            call__69.TerrainChanged();
                            Count += 1;
                        }

                        if (TerrainChangedProc != null)
                        {
                            var call__70 = OrgList[Walk.HN * 2 + 1];
                            var call__71 = OrgList[Walk.HN * 2];
                            var call__72 = MapLevel.GetField(Walk.X, Walk.Y);
                            TerrainChangedProc(call__72, call__71, call__70);
                        }
                    }

                    HSEngine.WalkToNextHN(ref Walk);
                }

                UnlockTerrainChangedEvent();
                Changed.Free();
                OrgList.Free();
                NewList.Free();
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
            int X = default;
            int Y = default;
            int MaxX = default;
            int MaxY = default;
            TMapLevel MapLevel = default;
            ResetVisibleStaticHS();
            MaxX = FViewXhx + FViewWidthHX - 1;
            MaxY = FViewYhx + FViewHeightHX - 1;
            var call__73 = FMapContainer.GetMapLevel(FViewL);
            MapLevel = call__73;
            {
                X = FViewXhx;
                int end__74 = MaxX;
                for (; X <= end__74; X += 1)
                {
                    {
                        Y = MaxY;
                        int end__75 = FViewYhx;
                        for (; Y >= end__75; Y -= 1)
                        {
                            var call__76 = MapLevel.GetField(X, Y);
                            call__76.CopyStaticVisibleHS(FStaticVisibleHS);
                        }
                    }
                }
            }

            {
                X = 0;
                int end__77 = FStaticFixedVisibleHS.FCount - 1;
                for (; X <= end__77; X += 1)
                {
                    var call__78 = FStaticFixedVisibleHS.Children[X];
                    ((THexagonSprite)call__78).MakeVisible(FStaticVisibleHS);
                }
            }

            FStaticVisibleHS.Sort();
            FStaticSceneInvalid = false;
        }

        // HSMAP.PAS:687
        public virtual void UpdateDynamicScene()
        {
            int X = default;
            int Y = default;
            int MaxX = default;
            int MaxY = default;
            TMapLevel MapLevel = default;
            ResetVisibleDynamicHS();
            MaxX = FViewXhx + FViewWidthHX - 1;
            MaxY = FViewYhx + FViewHeightHX - 1;
            var call__79 = FMapContainer.GetMapLevel(FViewL);
            MapLevel = call__79;
            {
                X = FViewXhx;
                int end__80 = MaxX;
                for (; X <= end__80; X += 1)
                {
                    {
                        Y = MaxY;
                        int end__81 = FViewYhx;
                        for (; Y >= end__81; Y -= 1)
                        {
                            var call__82 = MapLevel.GetField(X, Y);
                            call__82.CopyDynamicVisibleHS(FDynamicVisibleHS);
                        }
                    }
                }
            }

            {
                X = 0;
                int end__83 = FDynamicFixedVisibleHS.FCount - 1;
                for (; X <= end__83; X += 1)
                {
                    var call__84 = FDynamicFixedVisibleHS.Children[X];
                    ((THexagonSprite)call__84).MakeVisible(FDynamicVisibleHS);
                }
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

            FNewFrameEvents.TriggerNotifyEvents(this);
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
            TMapLevel MapLevel = default;
            TRect R = default;
            var call__85 = FMapContainer.GetMapLevel(L);
            MapLevel = call__85;
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
                HSEngine.OriginalAssert((AnsiString)"Invalid TerrainChangedLockCounter", (AnsiString)"D:\\GEngine\\HSENGINE\\inc\\HSMAP.PAS", 5989);
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
            int Result = default;
            int X = default;
            int Y = default;
            HSEngine.HXtoHP(this.FMapContainer.FWidth - 1, this.FMapContainer.FHeight - 1, ref X, ref Y);

            X += 40;
            Result = X;
            return Result;
        }

        // HSMAP.PAS:218
        public int GetMaxYhp()
        {
            int Result = default;
            int Y = default;
            int X = default;
            if (this.FMapContainer.FHeight > 1)
            {
                HSEngine.HXtoHP(1, this.FMapContainer.FHeight - 1, ref X, ref Y);
            }
            else
            {
                HSEngine.HXtoHP(0, this.FMapContainer.FHeight - 1, ref X, ref Y);
            }

            Y += 32;
            Result = Y;
            return Result;
        }

        // HSMAP.PAS:231
        public void SetHSSFileName(AnsiString FileName)
        {
            var call__88 = ExpandFileName(FileName);
            var call__89 = ExtractRelativePath(FEngine.StartupDirectory, call__88);
            FHSSFileName = call__89;
        }

        // HSMAP.PAS:459
        public void ClearTerrain(int X, int Y, int L, int Radius, int TerrainType)
        {
            THNWalkData Walk = default;
            TMapLevel MapLevel = default;
            TMapFieldMsg Msg = default;
            TRect R = default;
            LockTerrainChangedEvent();
            var call__90 = FMapContainer.GetMapLevel(L);
            MapLevel = call__90;
            HSEngine.InitHNWalk(ref Walk, X, Y, 0);
            Msg.MsgID = 16;
            while (Walk.Radius <= Radius)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    var call__91 = MapLevel.GetField(Walk.X, Walk.Y);
                    call__91.SendMapFieldMsg(ref Msg);
                }

                HSEngine.WalkToNextHN(ref Walk);
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
            bool Result = default;
            int Xhp = default;
            int Yhp = default;
            HSEngine.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            var call__92 = HS.MainPlace(FMapContainer, Xhp, Yhp, L);
            Result = call__92;
            return Result;
        }

        // HSMAP.PAS:569
        public bool Place(THexagonSprite HS, int Xhp, int Yhp, int L)
        {
            bool Result = default;
            var call__93 = HS.MainPlace(FMapContainer, Xhp, Yhp, L);
            Result = call__93;
            return Result;
        }

        // HSMAP.PAS:574
        public void Remove(THexagonSprite HS)
        {
            HS.MainRemove();
        }

        // HSMAP.PAS:585
        public bool SetViewPort(int Xhp, int Yhp, int Lhp, int Width, int Height)
        {
            bool Result = default;
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
                HSEngine.HPtoHX(FViewXhp - 8, FViewYhp - 16, ref X1, ref Y1);
                HSEngine.HPtoHX(FViewXhp + Width - 1 + 8, FViewYhp + Height - 1 + 16, ref X2, ref Y2);
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
                FViewPortChangedEvents.TriggerNotifyEvents(this);
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
        public bool Viewing(int Xhx, int Yhx, int Lhx)
        {
            bool Result = default;
            Result = Xhx >= FViewXhx && Xhx < FViewXhx + FViewWidthHX && Yhx >= FViewYhx && Yhx <= FViewYhx + FViewHeightHX && Lhx == FViewL;
            return Result;
        }

        // HSMAP.PAS:640
        // { The message names ResetVisibleDynamicHS (original slip). }
        public void ResetVisibleStaticHS()
        {
            int I = default;
            try
            {
                {
                    I = FStaticVisibleHS.FCount - 1;
                    byte end__94 = 0;
                    for (; I >= end__94; I -= 1)
                    {
                        ((THexagonSprite)FStaticVisibleHS.FItems[I].Child).SetVisible(false);
                    }
                }

                FStaticVisibleHS.QuickClear();
                FStaticVisibleHS.FSorted = false;
            }
            catch (System.Exception)
            {
                throw Exception.Create((AnsiString)"Exception during ResetVisibleDynamicHS");
            }
        }

        // HSMAP.PAS:654
        public void ResetVisibleDynamicHS()
        {
            int I = default;
            {
                I = FDynamicVisibleHS.FCount - 1;
                byte end__95 = 0;
                for (; I >= end__95; I -= 1)
                {
                    try
                    {
                        ((THexagonSprite)FDynamicVisibleHS.FItems[I].Child).SetVisible(false);
                    }
                    catch (System.Exception)
                    {
                        var call__96 = Exception.Create((AnsiString)"Exception during ResetVisibleDynamicHS (" + (AnsiString)ClassNameOf(FDynamicVisibleHS.FItems[I].Child.GetType()) + AnsiString.FromBytes(41));
                        throw call__96;
                    }
                }
            }

            FDynamicVisibleHS.QuickClear();
            FDynamicVisibleHS.FSorted = false;
        }

        // HSMAP.PAS:745
        public void InvalidateStaticScene(int Xhx, int Yhx, int Lhx)
        {
            FStaticSceneInvalid = FStaticSceneInvalid || Xhx >= FViewXhx && Xhx < FViewXhx + FViewWidthHX && Yhx >= FViewYhx && Yhx < FViewYhx + FViewHeightHX && Lhx == FViewL;
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
        public AnsiString HSSFileName { get => FHSSFileName; set => SetHSSFileName(value); }
        public TSet<THSMapStatusElement> Status { get => FStatus; set => FStatus = value; }
        public int Frame { get => FFrame; }
        public TEventList TerrainChangedEvents { get => FTerrainChangedEvents; }
        public TCenterViewEvent OnCenterView { get => FOnCenterView; set => FOnCenterView = value; }
        public TChangeViewEvent OnChangeView { get => FOnChangeView; set => FOnChangeView = value; }
        public TViewLevelEvent OnViewLevel { get => FOnViewLevel; set => FOnViewLevel = value; }
        public Engine.TProgressEvent OnProgress { get => FOnProgress; set => FOnProgress = value; }
        public TEventList NewFrameEvents { get => FNewFrameEvents; }
        public TEventList ViewPortChangedEvents { get => FViewPortChangedEvents; }
        public TEventList DestroyEvents { get => FDestroyEvents; }
        public TEventList HSChangedEvents { get => FHSChangedEvents; }
        public AnsiString FileName { get => FFileName; set => FFileName = value; }
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
        internal AnsiString FHSSetSearchPath;
        internal AnsiString FHSSFileName;
        internal TEventList FOnHSMLoaded;
        internal TEventList FOnHSMClose;
        internal TEventList FOnHSSLoaded;
        internal TEventList FOnHSSClose;
        // HSEngine.pas:4941
        public override void Destroy()
        {
            FOnHSMLoaded.Clear();
            FOnHSMClose.Clear();
            CloseHSM(null);
            base.Destroy();
            FOnHSMLoaded.Free();
            FOnHSMLoaded = null;
            FOnHSMClose.Free();
            FOnHSMClose = null;
            FOnHSSLoaded.Free();
            FOnHSSLoaded = null;
            FOnHSSClose.Free();
            FOnHSSClose = null;
        }

        private protected THSEngine(ConstructionContext context, TECustomNode owner) : base(context, owner)
        {
            var call__97 = new TEventList();
            FOnHSMLoaded = call__97;
            var call__98 = new TEventList();
            FOnHSMClose = call__98;
            var call__99 = new TEventList();
            FOnHSSLoaded = call__99;
            var call__100 = new TEventList();
            FOnHSSClose = call__100;
            RegisterEClasses([EClass.Of<TMapContainer>(), EClass.Of<TMapLevel>(), EClass.Of<THSMap>(), EClass.Of<THSSet>()]);
            RegisterEClasses([EClass.Of<TMapObjectResource>(), EClass.Of<TMultiHexMOR>()]);
            RegisterEClasses([EClass.Of<TAbstractHexagon>(), EClass.Of<TAbstractBorderHexagon>(), EClass.Of<TAbstractRoad>()]);
            var call__101 = RegisterDebugID((AnsiString)"HS-Engine");
            HSEngine.HSEngineDID = call__101;
            FHSSFileName = (AnsiString)"noname.hss";
            CreateHSSet();
        }

        // HSEngine.pas:5019
        public virtual void CreateHSSet()
        {
            var call__102 = new THSSet(this);
            FHSSet = call__102;
        }

        // HSEngine.pas:4962
        public virtual void SetHSMap(THSMap Value)
        {
            FHSMap = Value;
        }

        // HSEngine.pas:5210
        public virtual void LoadHSM(AnsiString FileName, bool Edit, Engine.TProgressEvent AProgress)
        {
            TCompressStream Stream = default;
            THSMHeader Header = default;
            if (FHSMap != null)
            {
                FHSMap.Free();
                SetHSMap(null);
            }

            try
            {
                Stream = new TCompressStream();
                try
                {
                    Stream.LoadFromFile(FileName);
                    Stream.Read(Bytes.Of(ref Header, 16));
                    Stream.Position = Header.Size;
                    if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, HSEngine.HSMID))
                    {
                        var call__103 = CreateEObject(Header.ClassID, this);
                        SetHSMap((THSMap)call__103);
                        var call__104 = ExpandFileName(FileName);
                        FHSMap.FFileName = call__104;
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
                            FOnHSMLoaded.TriggerNotifyEvents(FHSMap);
                        }
                        catch (System.Exception)
                        {
                            FHSMap.Free();
                            SetHSMap(null);
                        }

                        FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                    }
                }
                finally
                {
                    Stream.Free();
                }
            }
            catch (System.Exception)
            {
            }
        }

        // HSEngine.pas:5137
        public virtual THSMap CreatePreviewHSM(AnsiString FileName)
        {
            THSMap Result = default;
            TCompressStream Stream = default;
            AnsiString FullName = default;
            THSMHeader Header = default;
            Result = null;
            FullName = ExpandFileName(FileName);
            try
            {
                Stream = new TCompressStream();
                try
                {
                    Stream.LoadFromFile(FullName);
                    Stream.Read(Bytes.Of(ref Header, 16));
                    Stream.Position = Header.Size;
                    if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, HSEngine.HSMID))
                    {
                        var call__105 = CreateEObject(Header.ClassID, this);
                        Result = (THSMap)call__105;
                        var call__106 = ExpandFileName(FileName);
                        Result.FFileName = call__106;
                        FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                        try
                        {
                            ReadPreviewEObject(Stream, Result, null);
                        }
                        catch (System.Exception)
                        {
                            Result.Free();
                            Result = null;
                        }

                        FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                    }
                }
                finally
                {
                    Stream.Free();
                }
            }
            catch (System.Exception)
            {
            }

            return Result;
        }

        // HSEngine.pas:5291
        public virtual void SaveHSM(AnsiString FileName, Engine.TProgressEvent AProgress)
        {
            TCompressStream Stream = default;
            THSMHeader Header = default;
            if (FHSMap != null)
            {
                Header = default;
                Header.Size = 16;
                var call__107 = FHSMap.EClass.ClassID;
                Header.ClassID = call__107;
                Header.Reserved = 0;
                Header.ID = HSEngine.HSMID;
                Stream = new TCompressStream();
                FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esSavingHSM);
                try
                {
                    Stream.Write(Bytes.Of(ref Header, 16));
                    WriteEObject(Stream, FHSMap, AProgress);
                    Stream.SaveToFile(FileName);
                }
                finally
                {
                    FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esSavingHSM);
                    Stream.Free();
                }
            }
        }

        // HSEngine.pas:5368
        public virtual void CloseHSM(Engine.TProgressEvent AProgress)
        {
            if (FHSMap != null)
            {
                FHSMap.FOnProgress = AProgress;
                FOnHSMClose.TriggerNotifyEvents(FHSMap);
                FHSMap.Free();
                SetHSMap(null);
            }
        }

        // HSEngine.pas:5093
        public virtual void CloseHSS()
        {
            FOnHSSClose.TriggerNotifyEvents(this);
            FHSSet.Clear();
            FHSSFileName = (AnsiString)"noname.hss";
        }

        // HSEngine.pas:4967
        public TStringList GetHSSetFiles(AnsiString Mask)
        {
            TStringList Result = default;
            TStringList Files = default;
            void Search(AnsiString Dir)
            {
                TStringList Dirs = default;
                TFileStream Stream = default;
                THSSHeader Header = default;
                TSearchRec SR = default;
                bool Found = default;
                int I = default;
                Found = FindFirst(Dir + Mask, 63 & ~16, ref SR) == 0;
                while (Found)
                {
                    Header = default;
                    var call__108 = new TFileStream(Dir + SR.Name, 0);
                    Stream = call__108;
                    try
                    {
                        try
                        {
                            Stream.Read(Bytes.Of(ref Header, 52));
                        }
                        finally
                        {
                            Stream.Free();
                        }
                    }
                    catch (System.Exception)
                    {
                    }

                    if (Header.ID == AnsiString.FromBytes(72, 83, 83, 0))
                    {
                        Files.Add(Header.Name + AnsiString.FromBytes(124) + Dir + SR.Name);
                    }

                    Found = FindNext(ref SR) == 0;
                }

                FindClose(ref SR);
                Dirs = new TStringList();
                Found = FindFirst(Dir + (AnsiString)"*.*", 16, ref SR) == 0;
                while (Found)
                {
                    if ((SR.Attr & 16) != 0 && SR.Name != AnsiString.FromBytes(46) && SR.Name != (AnsiString)"..")
                    {
                        Dirs.Add(Dir + SR.Name + AnsiString.FromBytes(92));
                    }

                    Found = FindNext(ref SR) == 0;
                }

                FindClose(ref SR);
                {
                    I = 0;
                    int end__109 = Dirs.Count - 1;
                    for (; I <= end__109; I += 1)
                    {
                        var call__110 = Dirs[I];
                        Search(call__110);
                    }
                }

                Dirs.Free();
            }

            Files = new TStringList();
            Search(StartupDirectory);
            Result = Files;
            return Result;
        }

        // HSEngine.pas:5024
        public void LoadHSS(AnsiString FileName, bool Edit, Engine.TProgressEvent AProgress)
        {
            TMemoryStream Mem = default;
            TFileStream F = default;
            THSSHeader Header = default;
            TEMsg Msg = default;
            int CRC = default;
            FHSSet.Clear();
            var call__111 = ExpandFileName(FileName);
            FHSSFileName = call__111;
            FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSS);
            Mem = new TMemoryStream();
            try
            {
                var call__112 = new TFileStream(FHSSFileName, 32);
                F = call__112;
                try
                {
                    Mem.LoadFromStream(F);
                }
                finally
                {
                    F.Destroy();
                }

                Mem.Read(Bytes.Of(ref Header, 52));
                Mem.Position = Header.Size;
                var call__113 = Mem.Size;
                CRC = MemoryMarshal.Read<int>(Mem.Memory.AsSpan(call__113 - 4, 4));
                bool bool__114 = Header.ID != AnsiString.FromBytes(72, 83, 83, 0);
                if (!bool__114)
                {
                    var call__115 = Mem.Size;
                    var call__116 = GetCRC32(Mem.Memory, call__115 - 4);
                    bool__114 = CRC != call__116;
                }

                if (bool__114)
                {
                    throw Exception.Create((AnsiString)"Invalid HSSET");
                }

                ReadEObject(Mem, FHSSet, AProgress);
                ActivateEObject(FHSSet);
                InitMsg(ref Msg, 65556, this);
                FHSSet.MsgProc(ref Msg, this);
                ControlNode.MsgProc(ref Msg, this);
                FOnHSSLoaded.TriggerNotifyEvents(this);
            }
            finally
            {
                FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSS);
                Mem.Free();
            }
        }

        // HSEngine.pas:5060
        public void SaveHSS(AnsiString FileName, Engine.TProgressEvent AProgress)
        {
            TMemoryStream Mem = default;
            TFileStream F = default;
            int CRC = default;
            THSSHeader Header = default;
            if (FHSSet != null)
            {
                Header.Size = 52;
                Header.Name = FHSSet.Name;
                Header.ID = AnsiString.FromBytes(72, 83, 83, 0);
                var call__117 = ExpandFileName(FileName);
                FHSSFileName = call__117;
                FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esSavingHSS);
                Mem = new TMemoryStream();
                try
                {
                    Mem.Write(Bytes.Of(ref Header, 52));
                    WriteEObject(Mem, FHSSet, AProgress);
                    var call__118 = Mem.Size;
                    var call__119 = GetCRC32(Mem.Memory, call__118);
                    CRC = call__119;
                    Mem.Write(Bytes.Of(ref CRC, 4));
                    var call__120 = new TFileStream(FHSSFileName, 65535);
                    F = call__120;
                    try
                    {
                        Mem.SaveToStream(F);
                    }
                    finally
                    {
                        F.Free();
                    }
                }
                finally
                {
                    FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esSavingHSS);
                    Mem.Free();
                }
            }
        }

        // HSEngine.pas:5100
        public void LoadPreviewHSM(AnsiString FileName)
        {
            TCompressStream Stream = default;
            THSMHeader Header = default;
            if (FHSMap != null)
            {
                FHSMap.Free();
                SetHSMap(null);
            }

            try
            {
                Stream = new TCompressStream();
                try
                {
                    Stream.LoadFromFile(FileName);
                    Stream.Read(Bytes.Of(ref Header, 16));
                    Stream.Position = Header.Size;
                    if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, HSEngine.HSMID))
                    {
                        var call__121 = CreateEObject(Header.ClassID, this);
                        SetHSMap((THSMap)call__121);
                        FHSMap.FStatus = FHSMap.FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msPreviewMode);
                        var call__122 = ExpandFileName(FileName);
                        FHSMap.FFileName = call__122;
                        FStatus = FStatus + TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                        try
                        {
                            ReadEObject(Stream, FHSMap, null);
                        }
                        catch (System.Exception)
                        {
                            FHSMap.Free();
                            SetHSMap(null);
                        }

                        FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                    }
                }
                finally
                {
                    Stream.Free();
                }
            }
            catch (System.Exception)
            {
            }
        }

        // HSEngine.pas:5171
        public THSMap LoadHSMEx(AnsiString FileName, bool Edit, Engine.TProgressEvent AProgress)
        {
            THSMap Result = default;
            TCompressStream Stream = default;
            AnsiString FullName = default;
            THSMHeader Header = default;
            Result = null;
            FullName = ExpandFileName(FileName);
            try
            {
                Stream = new TCompressStream();
                try
                {
                    Stream.LoadFromFile(FullName);
                    Stream.Read(Bytes.Of(ref Header, 16));
                    Stream.Position = Header.Size;
                    if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, HSEngine.HSMID))
                    {
                        var call__123 = CreateEObject(Header.ClassID, null);
                        Result = (THSMap)call__123;
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
                            FOnHSMLoaded.TriggerNotifyEvents(Result);
                        }
                        catch (System.Exception)
                        {
                            Result.Free();
                            Result = null;
                        }

                        FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                    }
                }
                finally
                {
                    Stream.Free();
                }
            }
            catch (System.Exception)
            {
            }

            return Result;
        }

        // HSEngine.pas:5251
        public void LoadHSMfromStream(TStream Stream, bool Edit, Engine.TProgressEvent AProgress)
        {
            TCompressStream CStream = default;
            THSMHeader Header = default;
            if (FHSMap != null)
            {
                FHSMap.Free();
                SetHSMap(null);
            }

            try
            {
                CStream = new TCompressStream();
                try
                {
                    CStream.LoadFromStream(Stream);
                    CStream.Read(Bytes.Of(ref Header, 16));
                    CStream.Position = Header.Size;
                    if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, HSEngine.HSMID))
                    {
                        var call__124 = CreateEObject(Header.ClassID, this);
                        SetHSMap((THSMap)call__124);
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
                            FOnHSMLoaded.TriggerNotifyEvents(FHSMap);
                        }
                        catch (System.Exception)
                        {
                            FHSMap.Free();
                            SetHSMap(null);
                        }

                        FStatus = FStatus - TSet<THSEngineStatusElement>.Of(THSEngineStatusElement.esLoadingHSM);
                    }
                }
                finally
                {
                    CStream.Free();
                }
            }
            catch (System.Exception)
            {
            }
        }

        // HSEngine.pas:5316
        public void SaveHSMtoStream(TStream Stream, Engine.TProgressEvent AProgress)
        {
            TCompressStream CStream = default;
            THSMHeader Header = default;
            if (FHSMap != null)
            {
                Header = default;
                Header.Size = 16;
                var call__125 = FHSMap.EClass.ClassID;
                Header.ClassID = call__125;
                Header.Reserved = 0;
                Header.ID = HSEngine.HSMID;
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
                    CStream.Free();
                }
            }
        }

        // HSEngine.pas:5341
        public void InitializeHSSet(AnsiString FileName, Engine.TProgressEvent AProgress)
        {
            TEMsg Msg = default;
            if (FHSSet == null || CompareText(FileName, FHSSFileName) != 0)
            {
                LoadHSS(FileName, false, AProgress);
            }
            else
            {
                InitMsg(ref Msg, 65557, this);
                FHSSet.MsgProc(ref Msg, this);
            }
        }

        // HSEngine.pas:5354
        public THSMap NewHSM(int AClassID)
        {
            THSMap Result = default;
            if (FHSMap != null)
            {
                FHSMap.Free();
                SetHSMap(null);
            }

            var call__126 = CreateEObject(AClassID, this);
            SetHSMap((THSMap)call__126);
            FHSMap.FStatus = FHSMap.FStatus + TSet<THSMapStatusElement>.Of(THSMapStatusElement.msEditting);
            ActivateEObject(FHSMap);
            FOnHSMLoaded.TriggerNotifyEvents(FHSMap);
            Result = FHSMap;
            return Result;
        }

        public TSet<THSEngineStatusElement> Status { get => FStatus; }
        public THSSet HSSet { get => FHSSet; }
        public THSMap HSMap { get => FHSMap; set => SetHSMap(value); }
        public AnsiString HSSetSearchPath { get => FHSSetSearchPath; set => FHSSetSearchPath = value; }
        public AnsiString HSSFileName { get => FHSSFileName; }
        public TEventList OnHSMLoaded { get => FOnHSMLoaded; }
        public TEventList OnHSMClose { get => FOnHSMClose; }
        public TEventList OnHSSLoaded { get => FOnHSSLoaded; }
        public TEventList OnHSSClose { get => FOnHSSClose; }

        // HSEngine.pas:803
        public static THSEngine Create(TECustomNode owner) =>
            ConstructionContext.Create(owner, static (context, owner) => new THSEngine(context, owner));

    }

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
        public override void Destroy()
        {
            MainRemove();
            base.Destroy();
        }

        // HXSPRITE.PAS:116
        public virtual sbyte GetTerrain(int HN)
        {
            sbyte Result = default;
            Result = -2;
            return Result;
        }

        // HXSPRITE.PAS:121
        public virtual void SetTerrain(int HN, sbyte Value)
        {
        }

        // HXSPRITE.PAS:125
        public virtual int GetTerrainCount()
        {
            int Result = default;
            Result = 0;
            return Result;
        }

        // HXSPRITE.PAS:130
        public virtual sbyte GetOverlay(int HN)
        {
            sbyte Result = default;
            Result = -2;
            return Result;
        }

        // HXSPRITE.PAS:135
        public virtual void SetOverlay(int HN, sbyte Value)
        {
        }

        // HXSPRITE.PAS:139
        public virtual int GetOverlayCount()
        {
            int Result = default;
            Result = 0;
            return Result;
        }

        // HXSPRITE.PAS:52
        public virtual byte GetLevel()
        {
            byte Result = default;
            throw Exception.Create((AnsiString)"No HexagonSprite Level defined");
            return Result;
        }

        // HXSPRITE.PAS:43
        public virtual void SetVisible(bool Value)
        {
        }

        // HXSPRITE.PAS:47
        public virtual bool GetVisible()
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // HXSPRITE.PAS:72
        public virtual bool GetSelected()
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // HXSPRITE.PAS:87
        public virtual sbyte GetXhx()
        {
            sbyte Result = default;
            Result = ((TMapField)Owner).FXhx;
            return Result;
        }

        // HXSPRITE.PAS:92
        public virtual sbyte GetYhx()
        {
            sbyte Result = default;
            Result = ((TMapField)Owner).FYhx;
            return Result;
        }

        // HXSPRITE.PAS:97
        public virtual sbyte GetLhx()
        {
            sbyte Result = default;
            Result = (sbyte)((TMapLevel)((TMapField)Owner).Owner).FLhx;
            return Result;
        }

        // HXSPRITE.PAS:102
        public virtual int GetXYL()
        {
            int Result = default;
            var call__127 = GetXhx();
            var call__128 = GetYhx();
            var call__129 = GetLhx();
            Result = call__127 + (call__128 << 8) + (call__129 << 16);
            return Result;
        }

        // HXSPRITE.PAS:153
        public virtual bool GetEditMode()
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // HXSPRITE.PAS:158
        public virtual void SetEditMode(bool Value)
        {
        }

        // HXSPRITE.PAS:162
        public virtual int GetShowPriority()
        {
            int Result = default;
            Result = 2130706432;
            return Result;
        }

        // HXSPRITE.PAS:189
        public virtual void Connect()
        {
        }

        // HXSPRITE.PAS:193
        public virtual void Disconnect()
        {
        }

        // HXSPRITE.PAS:167
        public virtual bool CanChangeTerrain(in THSSenderInfo Info)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

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
            bool Result = default;
            Connect();
            Result = true;
            return Result;
        }

        // HXSPRITE.PAS:203
        public virtual void RemoveFromMap()
        {
            Disconnect();
        }

        // HXSPRITE.PAS:573
        public virtual bool MainPlace(TMapContainer Map, int Xhp, int Yhp, int Lhx)
        {
            bool Result = default;
            Result = Place(Map, Xhp, Yhp, Lhx);
            if (Result)
            {
                try
                {
                    Activate();
                }
                catch (System.Exception)
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
            var call__130 = GetVisible();
            Assert(!call__130, (AnsiString)"Hexagon Sprite Still Visible!", "HXSPRITE.PAS", 589);
        }

        // HXSPRITE.PAS:592
        public virtual bool MainLoaded()
        {
            bool Result = default;
            Result = Loaded();
            return Result;
        }

        // HXSPRITE.PAS:184
        public virtual bool Place(TMapContainer Map, int Xhp, int Yhp, int Lhx)
        {
            bool Result = default;
            Result = PlaceOnMap();
            return Result;
        }

        // HXSPRITE.PAS:208
        public virtual void Remove()
        {
            RemoveFromMap();
        }

        // HXSPRITE.PAS:213
        public virtual bool Loaded()
        {
            bool Result = default;
            Connect();
            Result = true;
            return Result;
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
        public virtual int CanMoveOver(in THSSenderInfo Info)
        {
            int Result = default;
            Result = 1;
            return Result;
        }

        // HXSPRITE.PAS:39
        public virtual void MoveExclusive(in THSSenderInfo Info)
        {
        }

        // HXSPRITE.PAS:63
        public virtual void Changed()
        {
            TMapField MapF = default;
            MapF = (TMapField)Owner;
            if (MapF != null)
            {
                ((THSEngine)((TMapLevel)MapF.Owner).FEngine).FHSMap.FHSChangedEvents.TriggerNotifyEvents(this);
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
                var call__131 = GetShowPriority();
                VisibilityList.AddSortChild(this, call__131);
            }
        }

        // HXSPRITE.PAS:82
        public virtual void GetBaseHX(ref int XYL)
        {
            XYL = ((TMapField)Owner).FXhx + (((TMapField)Owner).FYhx << 8) + (((TMapLevel)((TMapField)Owner).Owner).FLhx << 16);
        }

        // HXSPRITE.PAS:57
        public virtual bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            Result = Owner != null && ((THSEngine)((TMapLevel)((TMapField)Owner).Owner).FEngine).FHSMap.FStatus.Contains(THSMapStatusElement.msEditting);
            return Result;
        }

        // HXSPRITE.PAS:107
        public virtual void Select()
        {
        }

        // HXSPRITE.PAS:111
        public virtual void Unselect()
        {
            ((TMapLevel)((TMapField)Owner).Owner).FMap.FSelection.Unselect(this);
        }

        // HXSPRITE.PAS:17
        public virtual AnsiString EditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Hexagon Sprite";
            return Result;
        }

        // HXSPRITE.PAS:219
        public bool PlaceOnMapEx(TMapContainer Map, int Xhp, int Yhp, int Lhx, int Width, int Height)
        {
            bool Result = default;
            int X = default;
            int Y = default;
            int Xmin = default;
            int YminOdd = default;
            int YminEven = default;
            int Xmax = default;
            int YmaxOdd = default;
            int YmaxEven = default;
            TMapLevel MapL = default;
            HSEngine.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
            HSEngine.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
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
            HSEngine.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
            HSEngine.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
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

                var call__132 = MapL.GetField(X, Y);
                FOwner = call__132;
                {
                    X = Xmin;
                    int end__133 = Xmax;
                    for (; X <= end__133; X += 1)
                    {
                        if (!((X & 1) != 0))
                        {
                            {
                                Y = YminEven;
                                int end__134 = YmaxEven;
                                for (; Y <= end__134; Y += 1)
                                {
                                    var call__135 = MapL.GetField(X, Y);
                                    call__135.AddHS(this, 1);
                                }
                            }
                        }
                        else
                        {
                            {
                                Y = YminOdd;
                                int end__136 = YmaxOdd;
                                for (; Y <= end__136; Y += 1)
                                {
                                    var call__137 = MapL.GetField(X, Y);
                                    call__137.AddHS(this, 1);
                                }
                            }
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
            int YminEven = default;
            int Xmax = default;
            int YmaxOdd = default;
            int YmaxEven = default;
            TMapLevel MapL = default;
            MapL = (TMapLevel)FOwner.Owner;
            HSEngine.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
            HSEngine.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
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
            HSEngine.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
            HSEngine.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
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

                {
                    X = Xmin;
                    int end__138 = Xmax;
                    for (; X <= end__138; X += 1)
                    {
                        if (!((X & 1) != 0))
                        {
                            {
                                Y = YminEven;
                                int end__139 = YmaxEven;
                                for (; Y <= end__139; Y += 1)
                                {
                                    var call__140 = MapL.GetField(X, Y);
                                    call__140.RemoveHS(this);
                                }
                            }
                        }
                        else
                        {
                            {
                                Y = YminOdd;
                                int end__141 = YmaxOdd;
                                for (; Y <= end__141; Y += 1)
                                {
                                    var call__142 = MapL.GetField(X, Y);
                                    call__142.RemoveHS(this);
                                }
                            }
                        }
                    }
                }
            }

            FOwner = null;
        }

        // HXSPRITE.PAS:362
        public bool PlaceUserOnMapEx(TMapContainer Map, int Xhp, int Yhp, int Lhx, int Width, int Height)
        {
            bool Result = default;
            int X = default;
            int Y = default;
            int Xmin = default;
            int YminOdd = default;
            int YminEven = default;
            int Xmax = default;
            int YmaxOdd = default;
            int YmaxEven = default;
            TMapLevel MapL = default;
            HSEngine.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
            HSEngine.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
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
            HSEngine.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
            HSEngine.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
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
                {
                    X = Xmin;
                    int end__143 = Xmax;
                    for (; X <= end__143; X += 1)
                    {
                        if (!((X & 1) != 0))
                        {
                            {
                                Y = YminEven;
                                int end__144 = YmaxEven;
                                for (; Y <= end__144; Y += 1)
                                {
                                    var call__145 = MapL.GetField(X, Y);
                                    call__145.AddHS(this, 1);
                                }
                            }
                        }
                        else
                        {
                            {
                                Y = YminOdd;
                                int end__146 = YmaxOdd;
                                for (; Y <= end__146; Y += 1)
                                {
                                    var call__147 = MapL.GetField(X, Y);
                                    call__147.AddHS(this, 1);
                                }
                            }
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
            int YminEven = default;
            int Xmax = default;
            int YmaxOdd = default;
            int YmaxEven = default;
            TMapLevel MapL = default;
            MapL = Map.GetMapLevel(Lhx);
            HSEngine.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
            HSEngine.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
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
            HSEngine.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
            HSEngine.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
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

                {
                    X = Xmin;
                    int end__148 = Xmax;
                    for (; X <= end__148; X += 1)
                    {
                        if (!((X & 1) != 0))
                        {
                            {
                                Y = YminEven;
                                int end__149 = YmaxEven;
                                for (; Y <= end__149; Y += 1)
                                {
                                    var call__150 = MapL.GetField(X, Y);
                                    call__150.RemoveHS(this);
                                }
                            }
                        }
                        else
                        {
                            {
                                Y = YminOdd;
                                int end__151 = YmaxOdd;
                                for (; Y <= end__151; Y += 1)
                                {
                                    var call__152 = MapL.GetField(X, Y);
                                    call__152.RemoveHS(this);
                                }
                            }
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
            int YminEven = default;
            int Xmax = default;
            int YmaxOdd = default;
            int YmaxEven = default;
            TMapLevel MapL = default;
            MapL = Map.GetMapLevel(Lhx);
            HSEngine.HPtoHX(Xhp - 8, Yhp, ref Xmin, ref YminOdd);
            HSEngine.HPtoHX(Xhp - 8 + 32, Yhp, ref X, ref Y);
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
            HSEngine.HPtoHX(Width + Xhp + 8 - 1, Y, ref Xmax, ref YmaxOdd);
            HSEngine.HPtoHX(Width + Xhp + 8 - 1 + 32, Y, ref X, ref Y);
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

                {
                    X = Xmin;
                    int end__153 = Xmax;
                    for (; X <= end__153; X += 1)
                    {
                        if (!((X & 1) != 0))
                        {
                            {
                                Y = YminEven;
                                int end__154 = YmaxEven;
                                for (; Y <= end__154; Y += 1)
                                {
                                    var call__155 = MapL.GetField(X, Y);
                                    call__155.Sort();
                                }
                            }
                        }
                        else
                        {
                            {
                                Y = YminOdd;
                                int end__156 = YmaxOdd;
                                for (; Y <= end__156; Y += 1)
                                {
                                    var call__157 = MapL.GetField(X, Y);
                                    call__157.Sort();
                                }
                            }
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
        public virtual TSet<TControlModeElement> ControlMode()
        {
            TSet<TControlModeElement> Result = default;
            Result = TSet<TControlModeElement>.Of();
            return Result;
        }


    }

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
        protected internal bool GetResourceLinked()
        {
            bool Result = default;
            Result = FStatus.Contains(TMapObjectStatusElement.moResourceLinked);
            return Result;
        }

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
            sbyte Result = default;
            var call__158 = FResource.FTerrainList.GetTerrain(Index);
            Result = call__158;
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
        public override int GetTerrainCount()
        {
            int Result = default;
            Result = FResource.FTerrainList.FCount;
            return Result;
        }

        // MAPOBJ.PAS:155
        public override sbyte GetOverlay(int Index)
        {
            sbyte Result = default;
            var call__159 = FResource.FTerrainList.GetOverlay(Index);
            Result = call__159;
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
        public override int GetOverlayCount()
        {
            int Result = default;
            Result = FResource.FTerrainList.FCount;
            return Result;
        }

        // MAPOBJ.PAS:16
        public override byte GetLevel()
        {
            byte Result = default;
            Result = FResource.FLevel;
            return Result;
        }

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
        public override bool GetVisible()
        {
            bool Result = default;
            Result = FStatus.Contains(TMapObjectStatusElement.moVisible);
            return Result;
        }

        // MAPOBJ.PAS:133
        public override bool GetSelected()
        {
            bool Result = default;
            Result = FStatus.Contains(TMapObjectStatusElement.moSelected);
            return Result;
        }

        // MAPOBJ.PAS:45
        public override sbyte GetXhx()
        {
            sbyte Result = default;
            Assert(Owner != null, (AnsiString)"Owner not set", "MAPOBJ.PAS", 47);
            Result = ((TMapField)Owner).FXhx;
            return Result;
        }

        // MAPOBJ.PAS:51
        public override sbyte GetYhx()
        {
            sbyte Result = default;
            Assert(Owner != null, (AnsiString)"Owner not set", "MAPOBJ.PAS", 53);
            Result = ((TMapField)Owner).FYhx;
            return Result;
        }

        // MAPOBJ.PAS:57
        public override sbyte GetLhx()
        {
            sbyte Result = default;
            Assert(Owner != null, (AnsiString)"Owner not set", "MAPOBJ.PAS", 59);
            Result = ((TMapField)Owner).FLhx;
            return Result;
        }

        // MAPOBJ.PAS:40
        public override bool GetEditMode()
        {
            bool Result = default;
            Result = FStatus.Contains(TMapObjectStatusElement.moEditMode);
            return Result;
        }

        // MAPOBJ.PAS:35
        // { Value is not used: the edit mode is only ever switched on }
        public override void SetEditMode(bool Value)
        {
            FStatus = FStatus + TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moEditMode);
        }

        // MAPOBJ.PAS:214
        public override int GetShowPriority()
        {
            int Result = default;
            int Xhp = default;
            int Yhp = default;
            HSEngine.HXtoHP(((TMapField)Owner).FXhx, ((TMapField)Owner).FYhx, ref Xhp, ref Yhp);
            Result = FResource.FShowPriorityOffset + (Yhp << 13) + Xhp;
            return Result;
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
            TMapObjectResource R = default;
            HSEngine.HXtoHP(((TMapField)Owner).FXhx, ((TMapField)Owner).FYhx, ref Xhp, ref Yhp);
            if (FResource.FImageSequenceList == null)
            {
                var call__160 = FResource.FImageLibrary.Get(0);
                call__160.Show(Xhp - XhpSurf - 8, Yhp - YhpSurf, SurfaceDesc);
            }
            else
            {
                FResource.ShowComplexImage(Xhp - XhpSurf - 8, Yhp - YhpSurf, SurfaceDesc, 0, (uint)(((TMapField)Owner).FXhx + ((TMapField)Owner).FYhx * 16));
            }
        }

        // MAPOBJ.PAS:252
        public override bool PlaceOnMap()
        {
            bool Result = default;
            Connect();
            var call__161 = FResource.FTerrainList.GetTerrain(0);
            bool bool__162 = call__161 >= 0;
            if (!bool__162)
            {
                var call__163 = FResource.FTerrainList.GetOverlay(0);
                bool__162 = call__163 >= 0;
            }

            if (bool__162)
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
                var mapField2 = (TMapField)Owner;
                ((TMapLevel)mapField2.Owner).FMap.TerrainChangedRad(mapField2.FXhx, mapField2.FYhx, mapField2.FLhx, 0);
            }

            Result = true;
            return Result;
        }

        // MAPOBJ.PAS:266
        public override void RemoveFromMap()
        {
            TMapField MapField = default;
            if (FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
            {
                if (Owner != null)
                {
                    MapField = (TMapField)Owner;
                    Disconnect();
                    if (!((TMapLevel)MapField.Owner).FMap.FStatus.Contains(THSMapStatusElement.msDestroying))
                    {
                        var call__166 = GetTerrain(0);
                        bool bool__167 = call__166 >= 0;
                        if (!bool__167)
                        {
                            var call__168 = GetOverlay(0);
                            bool__167 = call__168 >= 0;
                        }

                        if (bool__167)
                        {
                            MapField.UpdateTerrain();
                        }

                        var call__169 = GetLevel();
                        bool bool__170 = call__169 <= 15;
                        if (bool__170)
                        {
                            var call__171 = GetVisible();
                            bool__170 = call__171;
                        }

                        if (bool__170)
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
            bool Result = default;
            int Xhx = default;
            int Yhx = default;
            if (!FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
            {
                HSEngine.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
                var call__172 = GetEditMode();
                bool bool__173 = call__172;
                if (!bool__173)
                {
                    var call__174 = CanPlace(MapContainer, (sbyte)Xhx, (sbyte)Yhx, (sbyte)Lhx);
                    bool__173 = call__174;
                }

                if (bool__173)
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
            bool Result = default;
            int Xhx = default;
            int Yhx = default;
            HSEngine.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
            Result = PlaceHX(MapContainer, (sbyte)Xhx, (sbyte)Yhx, (sbyte)Lhx);
            return Result;
        }

        // MAPOBJ.PAS:291
        public override void Remove()
        {
            TMapField MapField = default;
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
            bool Result = default;
            Connect();
            Result = true;
            return Result;
        }

        // MAPOBJ.PAS:373
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 16)
            {
                var call__175 = GetTerrain(Msg.HN);
                if (call__175 >= 0 || Msg.HN == 0)
                {
                    Destroy();
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
                var call__176 = GetShowPriority();
                VisibilityList.AddSortChild(this, call__176);
            }
        }

        // MAPOBJ.PAS:126
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            bool bool__177 = FResource.FEngine.FHSMap.FStatus.Contains(THSMapStatusElement.msEditting) && FResource.FTerrainList.FCount != 0;
            if (bool__177)
            {
                var call__178 = FResource.FTerrainList.GetTerrain(0);
                bool bool__179 = call__178 >= 0;
                if (!bool__179)
                {
                    var call__180 = FResource.FTerrainList.GetOverlay(0);
                    bool__179 = call__180 >= 0;
                }

                bool__177 = bool__179;
            }

            Result = bool__177;
            return Result;
        }

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
        public override AnsiString EditName()
        {
            AnsiString Result = default;
            if (FResource.FName == (AnsiString)"")
            {
                var call__181 = FResource.ResourceEditName();
                Result = call__181;
            }
            else
            {
                Result = FResource.FName;
            }

            return Result;
        }

        // MAPOBJ.PAS:63
        public virtual TMapObjectResourceList GetResourceList(TEngine AEngine)
        {
            TMapObjectResourceList Result = default;
            Result = ((THSEngine)AEngine).FHSSet.FMapObjectResourceList;
            return Result;
        }

        // MAPOBJ.PAS:68
        public virtual void SetResource(TMapObjectResource Value)
        {
            FResource = Value;
        }

        // MAPOBJ.PAS:307
        public virtual bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            THexagonSprite HS = default;
            TMapField MapField = default;
            if (Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < MapContainer.FWidth && Yhx < MapContainer.FHeight && Lhx < MapContainer.FLevelCount)
            {
                MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
                HS = MapField.GetMaxLevelNoneTransparentHS();
                bool bool__182 = HS == null;
                if (!bool__182)
                {
                    var call__183 = HS.GetLevel();
                    bool__182 = call__183 < 10;
                }

                bool bool__184 = bool__182;
                if (bool__184)
                {
                    var call__185 = GetOverlay(0);
                    var call__186 = GetTerrain(0);
                    var call__187 = MapField.CanChangeTerrain(this, call__186, call__185);
                    bool__184 = call__187;
                }

                Result = bool__184;
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
            bool Result = default;
            var call__188 = MapContainer.GetField(Xhx, Yhx, Lhx);
            Owner = call__188;
            Result = PlaceOnMap();
            return Result;
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
            var call__190 = new TTerrainList();
            FTerrainList = call__190;
            var call__191 = new TImageLibrary(null);
            FImageLibrary = call__191;
            SetLevel(26);
        }

        protected internal THSEngine FEngine;
        protected internal int FShowPriorityOffset;
        protected internal TImageLibrary FImageLibrary;
        protected internal TImageSequenceList FImageSequenceList;
        protected internal TTerrainList FTerrainList;
        protected internal byte FLevel;
        protected internal AnsiString FName;
        // MAPOBJ.PAS:391
        public override void Destroy()
        {
            FTerrainList.Free();
            FImageLibrary.Free();
            if (FImageSequenceList != null)
            {
                FImageSequenceList.Free();
            }

            base.Destroy();
        }

        // MAPOBJ.PAS:405
        public override int ClassVersion()
        {
            int Result = default;
            Result = 1;
            return Result;
        }

        // MAPOBJ.PAS:400
        public override TEngine GetEngine()
        {
            TEngine Result = default;
            Result = FEngine;
            return Result;
        }

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
            if (!Stream.StorageMode().Contains(smText))
            {
                if (Stream.StorageMode().Contains(TStorageModeElement.smRead))
                {
                    Stream.rwBool(ref Complex, 9);
                    if (Complex)
                    {
                        if (FImageSequenceList == null)
                        {
                            var call__189 = new TImageSequenceList(null, FImageLibrary);
                            FImageSequenceList = call__189;
                        }
                    }
                    else
                    {
                        FImageSequenceList.Free();
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
                FEngine = (THSEngine)Msg.Engine;
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
        public override ImageLib.TLibraryImage GetCellImage()
        {
            ImageLib.TLibraryImage Result = default;
            int I = default;
            I = 0;
            Result = null;
            while (I != FImageLibrary.Count && Result == null)
            {
                var call__192 = FImageLibrary.Get(I);
                Result = call__192;
                I += 1;
            }

            if (Result == null && FImageSequenceList != null)
            {
                var call__193 = FImageSequenceList.GetPreviewImage();
                Result = call__193;
            }

            return Result;
        }

        // MAPOBJ.PAS:465
        public override void SetCellImage(ImageLib.TLibraryImage Value)
        {
            FImageLibrary.SetImage(0, Value);
        }

        // MAPOBJ.PAS:525
        public override TEObject CreateResourceUser()
        {
            TEObject Result = default;
            int ID = default;
            TMapObject MO = default;
            ID = ResourceUserClassID();
            if (ID != 0)
            {
                var call__194 = FEngine.CreateEObject(ID, null);
                MO = (TMapObject)call__194;
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

            Result = MO;
            return Result;
        }

        // MAPOBJ.PAS:420
        public override void Draw(TCanvas Canvas, TRect Rect, ImageLib.TBltMode Mode, int ScaleX, int ScaleY, int BkColor, int Color)
        {
            int I = default;
            ImageLib.TLibraryImage Image = default;
            TImageSequence Sequence = default;
            TRect DrawRect = default;
            I = 0;
            Image = null;
            while (I != FImageLibrary.Count && Image == null)
            {
                var call__195 = FImageLibrary.Get(I);
                Image = call__195;
                I += 1;
            }

            if (Image == null && FImageSequenceList != null)
            {
                I = 0;
                Sequence = null;
                while (I != FImageSequenceList.Count && Sequence == null)
                {
                    var call__196 = FImageSequenceList.GetImageSequence(I);
                    Sequence = call__196;
                    if (Sequence == null)
                    {
                        I += 1;
                    }
                }

                BltImageSequenceEx(Canvas, Sequence, 0, Rect, Mode, ScaleX, ScaleY, BkColor, Color, false, ref DrawRect);
            }
            else
            {
                BltLibraryImage(Canvas, Image, Rect, Mode, ScaleX, ScaleY, BkColor, Color, false, ref DrawRect);
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
            TMapObject Result = default;
            int Xhp = default;
            int Yhp = default;
            Result = (TMapObject)CreateResourceUser();
            HSEngine.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            Xhp += 16;
            Yhp += 16;
            if (!HSMap.Place(Result, Xhp, Yhp, Lhx))
            {
                Result.Free();
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
            TImageSequence Sequence = default;
            int Frame = default;
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
                var call__197 = Sequence.GetFrameIndex(FEngine.FHSMap.FFrame, (int)Offset);
                Frame = call__197;
                if (Frame != -1)
                {
                    var call__198 = Sequence.ImageLibrary.Get(Frame);
                    call__198.Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                }

                Sequence = Sequence.Next;
            }
        }

        // MAPOBJ.PAS:571
        public void ClearTerrain(int X, int Y, int Lhx)
        {
            int I = default;
            int Xhx = default;
            int Yhx = default;
            {
                I = FTerrainList.FCount - 1;
                byte end__199 = 0;
                for (; I >= end__199; I -= 1)
                {
                    var call__200 = FTerrainList.GetTerrain(I);
                    if (call__200 >= 0)
                    {
                        HSEngine.CenterHNtoHX(X, Y, I, ref Xhx, ref Yhx);
                        FEngine.FHSMap.ClearTerrain(Xhx, Yhx, Lhx, 0, -1);
                    }
                }
            }
        }

        // MAPOBJ.PAS:586
        // { The resource was dropped on the map (message $10005): fill the hexagons in Param[1] rings around
//   the drop point with its users, clearing the terrain first when Param[2] = 1 }
        public void EditDropMsg(ref TEMsg Msg)
        {
            TMapLevel MapLevel = default;
            int Xhx = default;
            int Yhx = default;
            int Lhx = default;
            int Rad = default;
            bool Clear = default;
            bool Placed = default;
            THNWalkData Walk = default;
            Placed = true;
            MapLevel = Msg.Param[0].As<TMapLevel>();
            Lhx = Msg.Data[1];
            Rad = Msg.Param[1].Value;
            HSEngine.HPtoHX(Msg.Value, Msg.Data[0], ref Xhx, ref Yhx);
            Clear = Msg.Param[2].Value == 1;
            HSEngine.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (Walk.Radius <= Rad)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    if (Clear)
                    {
                        ClearTerrain(Walk.X, Walk.Y, Lhx);
                    }

                    bool bool__201 = Placed;
                    if (bool__201)
                    {
                        var call__202 = PlaceResourceUser(FEngine.FHSMap, (sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)Lhx);
                        bool__201 = call__202 != null;
                    }

                    Placed = bool__201;
                }

                HSEngine.WalkToNextHN(ref Walk);
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
        public AnsiString Name { get => FName; set => FName = value; }

        // MAPOBJ.PAS:415
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"";
            return Result;
        }

    }

    public partial class TMapObjectResourceList : TEResourceList
    {
        public TMapObjectResourceList(TECustomNode owner) : base(owner)
        { }

        // MAPOBJ.PAS:641
        public TMapObjectResource GetMOR(int Index)
        {
            TMapObjectResource Result = default;
            Result = (TMapObjectResource)FChildren.List[Index];
            return Result;
        }

        // MAPOBJ.PAS:646
        public bool FindMOR(int AClassID, TTerrainList TerrainList, TSet<TTerrainCompareModeElement> Mode, ref TList List)
        {
            bool Result = default;
            TMapObjectResource R = default;
            int[] I__cell = new int[1];
            ref int I = ref I__cell[0];
            object IndexPtr = default;
            ;
            ;
            // { Preserve the original stack locals. }
            Result = false;
            {
                I = 0;
                int end__203 = FChildren.Count - 1;
                for (; I <= end__203; I += 1)
                {
                    R = (TMapObjectResource)FChildren.List[I];
                    if (R.EClass.ClassID == AClassID)
                    {
                        bool bool__204 = TerrainList == null;
                        if (!bool__204)
                        {
                            var call__205 = R.FTerrainList.CompareTerrain(TerrainList, Mode);
                            bool__204 = call__205;
                        }

                        if (bool__204)
                        {
                            Result = true;
                            List.Add(R);
                        }
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
    }

    // the parameters of a new map (THSMap.CreateNewMap)
    public partial class TNewHSMapSettings : TObject
    {

        public TNewHSMapSettings()
        {
            Width = 64;
            Height = 64;
            Levels = 1;
            HSSFileName = (AnsiString)"";
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
        public AnsiString HSSFileName;
        public int HSMapClassID;
        public int MapContainerClassID;
        public int MapLevelClassID;
        // HSEngine.pas:2802


    }

    // terrain and overlay types by hexagon number, two bytes per hexagon (terrain type, overlay
    // type. -2: none)
    public partial class TTerrainList : TObject
    {
        internal int FCount;
        internal byte[] FList = [];
        // HSEngine.pas:1411
        public override void Destroy()
        {
            Array.Resize(ref FList, 0);
            base.Destroy();
        }

        // HSEngine.pas:1417
        public virtual void Assign(TTerrainList Source)
        {
            SetCount(Source.FCount);
            Array.Copy(Source.FList, 0, FList, 0, FCount * 2);
        }

        // HSEngine.pas:1562
        public virtual void LoadFromStream(TStream Stream)
        {
            Stream.Read(Bytes.Of(ref FCount, 4));
            SetCount(FCount);
            Stream.Read(FList.AsSpan(0, FCount * 2));
        }

        // HSEngine.pas:1569
        public virtual void SaveToStream(TStream Stream)
        {
            Pack();
            Stream.Write(Bytes.Of(ref FCount, 4));
            Stream.Write(FList.AsSpan(0, FCount * 2));
        }

        // HSEngine.pas:1423
        public sbyte GetTerrain(int Index)
        {
            sbyte Result = default;
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

            {
                sbyte value__206 = Value;
                MemoryMarshal.Write(FList.AsSpan(Index * 2, 1), in value__206);
            }
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
            sbyte Result = default;
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

            {
                sbyte value__207 = Value;
                MemoryMarshal.Write(FList.AsSpan(Index * 2 + 1, 1), in value__207);
            }
        }

        // HSEngine.pas:1459
        public ushort GetTandO(int Index)
        {
            ushort Result = default;
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

            {
                ushort value__208 = Value;
                MemoryMarshal.Write(FList.AsSpan(Index * 2, 2), in value__208);
            }
        }

        // HSEngine.pas:1473
        public bool GetUsed(int Index)
        {
            bool Result = default;
            Result = Index < FCount && (MemoryMarshal.Read<ushort>(FList.AsSpan(Index * 2, 2)) & 32896) != 32896;
            return Result;
        }

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
            int NewCount = default;
            NewCount = FCount;
            while (true)
            {
                bool bool__211 = NewCount != 0;
                if (bool__211)
                {
                    var call__212 = GetOverlay(NewCount - 1);
                    bool__211 = call__212 == -2;
                }

                bool bool__213 = bool__211;
                if (bool__213)
                {
                    var call__214 = GetTerrain(NewCount - 1);
                    bool__213 = call__214 == -2;
                }

                if (!bool__213)
                    break;
                {
                    NewCount -= 1;
                }
            }

            if (NewCount != FCount)
            {
                SetCount(NewCount);
            }
        }

        // HSEngine.pas:1516
        public TWordList CreateSurroundingHNTable()
        {
            TWordList Result = default;
            int I = default;
            int HN = default;
            int Xhx = default;
            int Yhx = default;
            THNWalkData Walk = default;
            Result = new TWordList();
            {
                I = 0;
                int end__215 = FCount - 1;
                for (; I <= end__215; I += 1)
                {
                    if (GetTerrain(I) >= 0)
                    {
                        HSEngine.HNtoHX(I, ref Xhx, ref Yhx);
                        HSEngine.InitHNWalk(ref Walk, Xhx, Yhx, 1);
                        while (Walk.HN != 7)
                        {
                            var call__216 = HSEngine.HXtoHN(Walk.X, Walk.Y);
                            HN = call__216;
                            bool bool__217 = HN >= FCount;
                            if (!bool__217)
                            {
                                bool bool__218 = HN < FCount;
                                if (bool__218)
                                {
                                    var call__219 = GetTerrain(HN);
                                    bool__218 = call__219 < 0;
                                }

                                bool__217 = bool__218;
                            }

                            if (bool__217)
                            {
                                if (Result.IndexOf((ushort)HN) == -1)
                                {
                                    Result.Add((ushort)HN);
                                }
                            }

                            HSEngine.WalkToNextHN(ref Walk);
                        }
                    }
                }
            }

            return Result;
        }

        // HSEngine.pas:1538
        public bool CompareTerrain(TTerrainList List, TSet<TTerrainCompareModeElement> Mode)
        {
            bool Result = default;
            int I = default;
            int N = default;
            int T1 = default;
            int T2 = default;
            int O1 = default;
            int O2 = default;
            Result = true;
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
        public TObject Sender;
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

    // { THexagonSprite.CanMoveOn (16 and 12 bytes) }
    public struct THSMoveOnInfo
    {
        public TObject Sender;
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

    public struct TMapFieldMsg
    {
        public int MsgID;
        public TMapField MapField;
        public FixedArray_IntOrRef_4 Param;
        public int HN;
        public IntOrRef Result;
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
        public override TEngine GetEngine()
        {
            TEngine Result = default;
            Result = FEngine;
            return Result;
        }

        // HSEngine.pas:2913
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            if (Msg.MsgID == 536870920)
            {
                FEngine = Msg.Engine;
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
            throw Exception.Create((AnsiString)"Cannot set child in TMapLevel");
        }

        // HSEngine.pas:2950
        public override int GetCount()
        {
            int Result = default;
            Result = FCount;
            return Result;
        }

        // HSEngine.pas:2945
        public override int RemoveChild(TEObject Child)
        {
            int Result = default;
            throw Exception.Create((AnsiString)"Cannot remove from TMapLevel");
            return Result;
        }

        // HSEngine.pas:2940
        public override int AddChild(TEObject Child)
        {
            int Result = default;
            throw Exception.Create((AnsiString)"Cannot add child to TMapLevel");
            return Result;
        }

        // HSEngine.pas:3140
        public virtual void New(TNewHSMapSettings Settings)
        {
            Clear();
            try
            {
                CreateMapFieldList(Settings.Width, Settings.Height);
            }
            catch (System.Exception)
            {
                Clear();
                throw;
            }
        }

        // HSEngine.pas:2842
        public virtual EClass MapFieldClass()
        {
            EClass Result = default;
            Result = EClass.Of<TMapField>();
            return Result;
        }

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
        internal TEventList FOnSelect;
        internal TEventList FOnDeselect;
        internal THexagonSprite FEventHS;
        // HSEngine.pas:3681
        public THexagonSpriteSelection(TECustomNode owner) : base(owner)
        {
            var call__220 = new TList();
            FLocationHS = call__220;
            var call__221 = new TEventList();
            FOnSelect = call__221;
            var call__222 = new TEventList();
            FOnDeselect = call__222;
        }

        // HSEngine.pas:3689
        public override void Destroy()
        {
            base.Destroy();
            FLocationHS.Free();
            FOnSelect.Free();
            FOnSelect = null;
            FOnDeselect.Free();
            FOnDeselect = null;
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
            FOnDeselect.TriggerNotifyEvents(Owner);
        }

        // HSEngine.pas:3705
        public void TriggerSelectEvent(THexagonSprite HS)
        {
            FEventHS = HS;
            FOnSelect.TriggerNotifyEvents(Owner);
        }

        // HSEngine.pas:3711
        public void Unselect(THexagonSprite HS)
        {
            int I = default;
            var call__223 = FChildren.IndexOf(HS);
            I = call__223;
            FLocationHS.Remove(HS);
            if (I != -1)
            {
                FChildren.Remove(HS);
                var call__224 = FChildren.IndexOf(HS);
                if (call__224 == -1)
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
            THexagonSprite Result = default;
            THexagonSprite HS = default;
            int I = default;
            Result = null;
            I = 0;
            while (true)
            {
                var call__225 = Count;
                if (!(I != call__225 && Result == null))
                    break;
                {
                    HS = (THexagonSprite)FChildren.List[I];
                    if (AClass.IsInstanceOfType(HS))
                    {
                        Result = (THexagonSprite)FChildren.List[I];
                    }

                    I += 1;
                }
            }

            return Result;
        }

        // HSEngine.pas:3756
        public THexagonSprite GetHS(int Index)
        {
            THexagonSprite Result = default;
            Result = (THexagonSprite)FChildren.List[Index];
            return Result;
        }

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
            HSEngine.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            SelectLocation(Xhp, Yhp, Lhx);
        }

        // HSEngine.pas:3837
        public void UnselectLocation()
        {
            while (FLocationHS.Count != 0)
            {
                var call__226 = FLocationHS.Last();
                Unselect((THexagonSprite)call__226);
            }
        }

        public sealed class HSAccessor(THexagonSpriteSelection target)
        {
            public THexagonSprite this[int Index] { get => target.GetHS(Index); }
        }

        public HSAccessor HS => new HSAccessor(this);
        public TList LocationHS { get => FLocationHS; }
        public TEventList OnSelect { get => FOnSelect; }
        public TEventList OnDeselect { get => FOnDeselect; }
        public THexagonSprite EventHS { get => FEventHS; }

    }

    // the mouse of a map. FHSMap, FX, FY (the position in the view port in pixels, Low(Integer)
    // when outside), FButton, FOnUp, FOnDown, FOnMove, FOnDblClick, FOnCaptureChanged, FCapture
    // (the object that gets the mouse events), FStatus, FCursor, FCursorMode, FIgnoreDown, FShift,
    // FCursorVisible (and are used by the game only)
    public partial class THSMapMouse : TObject
    {

        public THSMapMouse()
        {
            var call__227 = new TEventList();
            FOnUp = call__227;
            var call__228 = new TEventList();
            FOnDown = call__228;
            var call__229 = new TEventList();
            FOnMove = call__229;
            var call__230 = new TEventList();
            FOnDblClick = call__230;
            var call__231 = new TEventList();
            FOnCaptureChanged = call__231;
            Reset();
        }

        internal THSMap FHSMap;
        internal int FX;
        internal int FY;
        internal TMouseButton FButton;
        internal TEventList FOnUp;
        internal TEventList FOnDown;
        internal TEventList FOnMove;
        internal TEventList FOnDblClick;
        internal TEventList FOnCaptureChanged;
        internal TObject FCapture;
        internal TSet<TMouseStatusElement> FStatus;
        internal int FCursor;
        internal int FCursorMode;
        internal bool FIgnoreDown;
        internal TSet<TShiftState> FShift;
        internal bool FCursorVisible;
        // HSEngine.pas:3475


        // HSEngine.pas:3486
        public override void Destroy()
        {
            FOnUp.Free();
            FOnDown.Free();
            FOnMove.Free();
            FOnDblClick.Free();
            FOnCaptureChanged.Free();
            base.Destroy();
        }

        // HSEngine.pas:3505
        public virtual int GetCursor()
        {
            int Result = default;
            Result = FCursor;
            return Result;
        }

        // HSEngine.pas:3510
        public virtual void SetCursor(int Value)
        {
            FCursor = Value;
        }

        // HSEngine.pas:3583
        public virtual bool MouseDown()
        {
            bool Result = default;
            Result = true;
            bool bool__232 = FCapture == null;
            if (bool__232)
            {
                var call__233 = GetXhx();
                bool__232 = call__233 != -1;
            }

            if (bool__232)
            {
                if (FButton == TMouseButton.mbLeft)
                {
                    var call__234 = GetYhp();
                    var call__235 = GetXhp();
                    FHSMap.FSelection.SelectLocation(call__235, call__234, FHSMap.FViewL);
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
        public virtual bool MouseUp()
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // HSEngine.pas:3646
        public virtual bool MouseMove()
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // HSEngine.pas:3667
        public virtual bool MouseDblClick()
        {
            bool Result = default;
            Result = true;
            return Result;
        }

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
                    FOnDown.TriggerNotifyEvents(this);
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
                FOnUp.TriggerNotifyEvents(this);
            }
        }

        // HSEngine.pas:3651
        public virtual void Move(TSet<TShiftState> Shift, int X, int Y)
        {
            bool bool__236 = X >= 0 && Y >= 0;
            if (bool__236)
            {
                var call__237 = FHSMap.GetMaxXhp();
                bool__236 = X <= call__237;
            }

            bool bool__238 = bool__236;
            if (bool__238)
            {
                var call__239 = FHSMap.GetMaxYhp();
                bool__238 = Y <= call__239;
            }

            if (bool__238)
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
                FOnMove.TriggerNotifyEvents(this);
            }
        }

        // HSEngine.pas:3672
        public virtual void DblClick()
        {
            if (GetXhp() != -2147483648)
            {
                if (MouseDblClick())
                {
                    FOnDblClick.TriggerNotifyEvents(this);
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
            int Result = default;
            if (FX != -2147483648)
            {
                Result = FHSMap.FViewXhp + FX;
                var call__240 = FHSMap.GetMaxXhp();
                if (Result > call__240 || Result < 0)
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
            int Result = default;
            if (FY != -2147483648)
            {
                Result = FHSMap.FViewYhp + FY;
                var call__241 = FHSMap.GetMaxYhp();
                if (Result > call__241 || Result < 0)
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
                HSEngine.HPtoHX(FHSMap.FViewXhp + FX, 0, ref Result, ref Y);
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
                HSEngine.HPtoHX(FHSMap.FViewXhp + FX, FHSMap.FViewYhp + FY, ref X, ref Result);
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
        public void SetCapture(TObject Value)
        {
            if (!FStatus.Contains(TMouseStatusElement.msChangingCapture) && Value != FCapture)
            {
                FStatus = FStatus + TSet<TMouseStatusElement>.Of(TMouseStatusElement.msChangingCapture);
                FCapture = Value;
                if (Value == null)
                {
                    FCursor = 0;
                }

                FOnCaptureChanged.TriggerNotifyEvents(this);
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
        public TEventList OnUp { get => FOnUp; }
        public TEventList OnDown { get => FOnDown; }
        public TEventList OnMove { get => FOnMove; }
        public TEventList OnDblClick { get => FOnDblClick; }
        public TEventList OnCaptureChanged { get => FOnCaptureChanged; }
        public TObject Capture { get => FCapture; set => SetCapture(value); }
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
        internal TNotifyEvent FOnSceneChanged;
        internal THSMap FHSMap;
        // HSEngine.pas:4670
        public THSMapViewer(TComponent owner) : base(owner)
        {
            FStaticSurfaceValid = false;
            FOnSceneChanged = null;
        }

        // HSEngine.pas:4678
        // { Leaves the map's OnChangeView set (original slip). }
        public override void Destroy()
        {
            if (FHSMap != null)
            {
                FHSMap.FOnCenterView = null;
                FHSMap.FOnViewLevel = null;
            }

            FStaticSurface.Free();
            base.Destroy();
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
            bool IsDDraw = default;
            TSet<TDDSCreationFlagsElement> Flags = default;
            IsDDraw = FStaticSurface is TDDrawSurface;
            bool bool__242 = FStaticSurface != null;
            if (bool__242)
            {
                var call__243 = FStaticSurface.GetPixelFormatID();
                var call__244 = Surface.GetPixelFormatID();
                bool__242 = call__243 != call__244 || (Surface is TDDrawSurface) ^ IsDDraw;
            }

            if (bool__242)
            {
                FStaticSurface.Free();
                FStaticSurface = null;
            }

            bool bool__245 = FStaticSurface == null;
            if (!bool__245)
            {
                var call__246 = FStaticSurface.GetWidth();
                bool__245 = call__246 != FSceneWidth;
            }

            bool bool__247 = bool__245;
            if (!bool__247)
            {
                var call__248 = FStaticSurface.GetHeight();
                bool__247 = call__248 != FSceneHeight;
            }

            if (bool__247)
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

                        var call__249 = new TDDrawSurface();
                        FStaticSurface = call__249;
                        var call__250 = Surface.GetPixelFormatID();
                        ((TDDrawSurface)FStaticSurface).CreateSurface(FSceneWidth, FSceneHeight, call__250, Flags);
                    }
                    else
                    {
                        var call__251 = Surface.GetPixelFormatID();
                        var call__252 = new TDataSurface(FSceneWidth, FSceneHeight, call__251, null);
                        FStaticSurface = call__252;
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

            bool bool__253 = IsDDraw;
            if (bool__253)
            {
                var call__254 = ((TDDrawSurface)FStaticSurface).Lost();
                bool__253 = call__254;
            }

            if (bool__253)
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
                var call__255 = FHSMap.SetViewPort(FSceneX, FSceneY, FSceneL, FSceneWidth, FSceneHeight);
                if (call__255 && FOnSceneChanged != null)
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
                HSEngine.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
                SetSceneX(Xhp + 16 - (int)(((uint)FSceneWidth) >> 1));
                SetSceneY(Yhp + 16 - (int)(((uint)FSceneHeight) >> 1));
                SetSceneL(Lhx);
                var call__256 = FHSMap.SetViewPort(FSceneX, FSceneY, FSceneL, FSceneWidth, FSceneHeight);
                if (call__256 && FOnSceneChanged != null)
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

                var call__258 = this.FHSMap.GetMaxXhp();
                if (call__258 < FSceneWidth + Value + 16)
                {
                    var call__259 = this.FHSMap.GetMaxXhp();
                    this.FSceneX = call__259 - FSceneWidth - 16;
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

                var call__261 = this.FHSMap.GetMaxYhp();
                if (call__261 - 16 < FSceneHeight + Value)
                {
                    var call__262 = this.FHSMap.GetMaxYhp();
                    this.FSceneY = call__262 - FSceneHeight - 16;
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
            THSMap M = default;
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
            THSMap M = default;
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
            FStaticSurface.Free();
            FStaticSurface = null;
        }

        // HSEngine.pas:4843
        public void ShowScene(int X, int Y, TSurface Surface)
        {
            THSMap M = default;
            object MP = default;
            TSurfaceDesc SD = default;
            TRect R = default;
            try
            {
                MP = FHSMap;
                if (MP != null)
                {
                    M = (THSMap)MP;
                    var call__263 = M.SetViewPort(FSceneX, FSceneY, FSceneL, FSceneWidth, FSceneHeight);
                    if (call__263 && FOnSceneChanged != null)
                    {
                        FOnSceneChanged(this);
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
                            var call__264 = FStaticSurface.Lock(ref SD);
                            if (call__264 >= 0)
                            {
                                M.DrawStaticScene(ref SD);
                                FStaticSurface.Unlock();
                                FStaticSurfaceValid = true;
                            }

                            var call__265 = FStaticSurface.BltClipped(Surface, X, Y);
                            if (call__265 >= 0)
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
                }
            }
            catch (System.Exception)
            {
                ShowMessage((AnsiString)"Exception during MapViewer.ShowScene");
            }
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
        public TNotifyEvent OnSceneChanged { get => FOnSceneChanged; set => FOnSceneChanged = value; }
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
            int I = default;
            Clear();
            {
                I = 0;
                int end__266 = FHSEngine.FHSSet.TerrainTypes.Count - 1;
                for (; I <= end__266; I += 1)
                {
                    var call__267 = new TList();
                    FResources.Add(call__267);
                }
            }
        }

        // HSEngine.pas:4497
        public override void Destroy()
        {
            Clear();
            FResources.Free();
            FResources = null;
            base.Destroy();
        }

        // HSEngine.pas:4505
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            TSet<TEControlStyleElement> Result = default;
            Result = TSet<TEControlStyleElement>.Of(csShared);
            return Result;
        }

        // HSEngine.pas:4489
        public TMORTerrainControl(TECustomNode owner) : base(owner)
        {
            var call__268 = new TList();
            FResources = call__268;
            FMOClass = typeof(TMapObject);
            FTries = 5;
        }

        // HSEngine.pas:4510
        public override void Clear()
        {
            int I = default;
            if (FResources != null)
            {
                {
                    I = FResources.Count - 1;
                    byte end__269 = 0;
                    for (; I >= end__269; I -= 1)
                    {
                        var call__270 = FResources[I];
                        ((TObject)call__270).Free();
                        FResources.Delete(I);
                    }
                }
            }
        }

        // HSEngine.pas:4531
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 536870920)
            {
                FHSEngine = (THSEngine)Msg.Engine;
                Initialize();
            }
        }

        // HSEngine.pas:4585
        public virtual TMapObject CreateMO(sbyte TerrainType)
        {
            TMapObject Result = default;
            int I = default;
            TList List = default;
            int Count = default;
            TMapObjectResource R = default;
            I = TerrainType;
            var call__271 = FResources[I];
            List = (TList)call__271;
            Count = List.Count;
            if (Count != 0)
            {
                var call__272 = Random(Count);
                R = (TMapObjectResource)List.List[call__272];
                Result = (TMapObject)R.CreateResourceUser();
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // HSEngine.pas:4604
        public virtual TMapObject Place(TMapContainer Container, sbyte X, sbyte Y, sbyte L, sbyte TerrainType)
        {
            TMapObject Result = default;
            TList List = default;
            TMapObjectResource R = default;
            TMapField MapF = default;
            TMapObject MO = default;
            Result = null;
            var call__273 = FResources[TerrainType];
            List = (TList)call__273;
            if (List.Count != 0)
            {
                R = null;
                MapF = Container.GetField(X, Y, L);
                var call__274 = MapF.FindOwnedHS(FMOClass);
                MO = (TMapObject)call__274;
                if (MO != null)
                {
                    var call__275 = MO.FResource.EClass.ClassID;
                    var call__276 = GetRndResource(TerrainType, call__275);
                    R = call__276;
                    if (R != null)
                    {
                        MO.SetResource(R);
                        MapF.UpdateTerrain();
                        Result = MO;
                    }
                }

                if (R == null)
                {
                    var call__277 = Random(List.Count);
                    R = (TMapObjectResource)List.List[call__277];
                    if (R != null)
                    {
                        var call__278 = R.PlaceResourceUser(Container.FMap, X, Y, L);
                        Result = call__278;
                    }
                }
            }

            return Result;
        }

        // HSEngine.pas:4637
        public virtual TMapObject PlaceClass(TMapContainer Container, sbyte X, sbyte Y, sbyte L, sbyte TerrainType, int AClassID)
        {
            TMapObject Result = default;
            TMapObjectResource R = default;
            TMapField MapF = default;
            TMapObject MO = default;
            Result = null;
            var call__279 = FResources[TerrainType];
            if (((TList)call__279).Count != 0)
            {
                R = null;
                MapF = Container.GetField(X, Y, L);
                var call__280 = MapF.FindOwnedHS(FMOClass);
                MO = (TMapObject)call__280;
                bool bool__281 = MO != null;
                if (bool__281)
                {
                    var call__282 = MO.FResource.EClass.ClassID;
                    bool__281 = call__282 == AClassID;
                }

                if (bool__281)
                {
                    var call__283 = MO.FResource.EClass.ClassID;
                    var call__284 = GetRndResource(TerrainType, call__283);
                    R = call__284;
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
                        var call__285 = R.PlaceResourceUser(Container.FMap, X, Y, L);
                        Result = call__285;
                    }
                }
            }

            return Result;
        }

        // HSEngine.pas:4541
        public TMapObjectResource GetRndResource(int TerrainType, int AClassID)
        {
            TMapObjectResource Result = default;
            TList List = default;
            int I = default;
            var call__286 = FResources[TerrainType];
            List = (TList)call__286;
            if (List.Count != 0)
            {
                I = 0;
                Result = null;
                while (I != FTries && Result == null)
                {
                    var call__287 = Random(List.Count);
                    Result = (TMapObjectResource)List.List[call__287];
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
                        Result = (TMapObjectResource)List.List[I];
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
        public void RegisterResource(TMapObjectResource Resource)
        {
            sbyte TerrainType = default;
            var call__288 = Resource.FTerrainList.GetTerrain(0);
            TerrainType = call__288;
            if (TerrainType >= 0 && TerrainType < FResources.Count)
            {
                var call__289 = ((TList)FResources.List[TerrainType]).IndexOf(Resource);
                if (call__289 != -1 || Resource == null)
                {
                    throw Exception.Create((AnsiString)"Resource double defined (TMORTerrainControl)");
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
            var call__290 = GetControlStyle();
            SetControlStyle(call__290 + TSet<TEControlStyleElement>.Of(csReserved2));
            var call__291 = new TPlugInControl(null);
            FPlugInControl = call__291;
            var call__292 = new TStringList();
            FTerrainTypes = call__292;
            var call__293 = new TStringList();
            FOverlayTypes = call__293;
            var call__294 = new TStringList();
            FDecorationTypes = call__294;
            var call__295 = new THexagonControl(this);
            FHexagonControl = call__295;
            FHexagonControl.MOClass = typeof(TAbstractHexagon);
            var call__296 = new TRoadControl(this);
            FRoadControl = call__296;
            FRoadControl.MOClass = typeof(TAbstractRoad);
            var call__297 = new TMapObjectResourceList(this);
            FMapObjectResourceList = call__297;
            FMapObjectResourceList.SetControlStyle(TSet<TEControlStyleElement>.Of(csStored, csShared));
            var call__298 = new TEResourceEditSettings(this);
            FResourceEditSettings = call__298;
            FResourceEditSettings.SetControlStyle(TSet<TEControlStyleElement>.Of(csStored, csShared));
            var call__299 = new TImageLibraryManager(null);
            FImageLibraryManager = call__299;
            var call__300 = new Sound.TSFXManager(null);
            FSFXManager = call__300;
        }

        internal TSet<THSSetStatusElement> FStatus;
        internal THexagonControl FHexagonControl;
        internal TRoadControl FRoadControl;
        internal TEngine FActivatedEngine;
        public TEngine ActivatedEngine { get => FActivatedEngine; }

        internal TMapObjectResourceList FMapObjectResourceList;
        internal TEResourceEditSettings FResourceEditSettings;
        internal TImageLibraryManager FImageLibraryManager;
        internal Sound.TSFXManager FSFXManager;
        internal TPlugInControl FPlugInControl;
        internal AnsiString FName;
        internal TStringList FTerrainTypes;
        internal TStringList FOverlayTypes;
        internal TStringList FDecorationTypes;
        // HSEngine.pas:3368
        public override void Destroy()
        {
            TEMsg Msg = default;
            FStatus = FStatus + TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssDestroying);
            InitMsg(ref Msg, 66564, this);
            if (FActivatedEngine != null && FActivatedEngine.ControlNode != null)
            {
                FActivatedEngine.ControlNode.MsgProc(ref Msg, this);
            }

            base.Destroy();
            FPlugInControl.Free();
            FTerrainTypes.Free();
            FTerrainTypes = null;
            FOverlayTypes.Free();
            FOverlayTypes = null;
            FDecorationTypes.Free();
            FDecorationTypes = null;
            FImageLibraryManager.Free();
            FImageLibraryManager = null;
            FSFXManager.Free();
            FSFXManager = null;
            InitMsg(ref Msg, 66565, this);
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
                FActivatedEngine = Msg.Engine;
            }
        }

        // HSEngine.pas:3421
        public void Activate(bool Edit)
        {
            TEMsg Msg = default;
            InitMsg(ref Msg, 66560, this);
            var call__301 = GetEngine();
            call__301.ControlNode.MsgProc(ref Msg, this);
            FStatus = FStatus + TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssActivated);
            if (Edit)
            {
                InitMsg(ref Msg, 66562, this);
                var call__302 = GetEngine();
                call__302.ControlNode.MsgProc(ref Msg, this);
                FStatus = FStatus + TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssEditting);
            }
        }

        // HSEngine.pas:3436
        public void DeActivate()
        {
            TEMsg Msg = default;
            if (FStatus.Contains(THSSetStatusElement.ssActivated))
            {
                InitMsg(ref Msg, 66561, this);
                var call__303 = GetEngine();
                call__303.ControlNode.MsgProc(ref Msg, this);
                FStatus = FStatus - TSet<THSSetStatusElement>.Of(THSSetStatusElement.ssActivated);
                if (FStatus.Contains(THSSetStatusElement.ssEditting))
                {
                    InitMsg(ref Msg, 66563, this);
                    var call__304 = GetEngine();
                    call__304.ControlNode.MsgProc(ref Msg, this);
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
        public TMapObjectResourceList MapObjectResourceList { get => FMapObjectResourceList; }
        public TEResourceEditSettings ResourceEditSettings { get => FResourceEditSettings; }
        public TImageLibraryManager ImageLibraryManager { get => FImageLibraryManager; }
        public Sound.TSFXManager SFXManager { get => FSFXManager; }
        public TPlugInControl PlugInControl { get => FPlugInControl; }
        public AnsiString Name { get => FName; set => FName = value; }
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
            var call__305 = THSEngine.Create(null);
            SetHSEngine(call__305);
        }

        // HSEngine.pas:5399
        public void SetHSSetSearchPath(AnsiString Value)
        {
            if (FHSEngine != null)
            {
                FHSEngine.FHSSetSearchPath = Value;
            }
        }

        // HSEngine.pas:5404
        public AnsiString GetHSSetSearchPath()
        {
            AnsiString Result = default;
            if (FHSEngine != null)
            {
                Result = FHSEngine.FHSSetSearchPath;
            }
            else
            {
                Result = (AnsiString)"";
            }

            return Result;
        }

        public THSEngine HSEngine { get => FHSEngine; set => SetHSEngine(value); }
        public AnsiString HSSetSearchPath { get => GetHSSetSearchPath(); set => SetHSSetSearchPath(value); }
    }

    // a sprite on one hexagon. FStatus: bit 1 visible, 2 edit mode, 4 selected, 8 activated (a
    // Byte: the original sets the bits with or/and, which a set with Include does not give)
    public partial class TSingleHS : THexagonSprite
    {
        public TSingleHS(TECustomNode owner) : base(owner)
        {}

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
        public override bool GetVisible()
        {
            bool Result = default;
            Result = (FStatus & 1) != 0;
            return Result;
        }

        // HXSPRITE.PAS:683
        public override bool GetSelected()
        {
            bool Result = default;
            Result = (FStatus & 4) != 0;
            return Result;
        }

        // HXSPRITE.PAS:660
        public override bool GetEditMode()
        {
            bool Result = default;
            Result = (FStatus & 2) != 0;
            return Result;
        }

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
            int Result = default;
            int Xhp = default;
            int Yhp = default;
            HSEngine.HXtoHP(((TMapField)Owner).FXhx, ((TMapField)Owner).FYhx, ref Xhp, ref Yhp);
            Result = (GetLevel() << 26) + ((Yhp + 32) << 13) + (Xhp + 16);
            return Result;
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
            bool Result = default;
            int Xhx = default;
            int Yhx = default;
            HSEngine.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
            Result = PlaceHX(Map, (sbyte)Xhx, (sbyte)Yhx, (sbyte)Lhx);
            return Result;
        }

        // HXSPRITE.PAS:721
        public override void Remove()
        {
            TMapField MapF = default;
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
                var call__306 = GetShowPriority();
                VisibilityList.AddSortChild(this, call__306);
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
        public virtual bool CanPlace(TMapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < Map.FWidth && Yhx < Map.FHeight && Lhx < Map.FLevelCount;
            return Result;
        }

        // HXSPRITE.PAS:757
        public virtual bool PlaceHX(TMapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            var call__307 = GetEditMode();
            bool bool__308 = call__307;
            if (!bool__308)
            {
                var call__309 = CanPlace(Map, Xhx, Yhx, Lhx);
                bool__308 = call__309;
            }

            if (bool__308)
            {
                Remove();
                var call__310 = Map.GetField(Xhx, Yhx, Lhx);
                SetOwner(call__310);
                Result = PlaceOnMap();
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // HXSPRITE.PAS:699
        public bool Activated()
        {
            bool Result = default;
            Result = (FStatus & 8) != 0;
            return Result;
        }
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
        public override bool GetVisible()
        {
            bool Result = default;
            Result = (FStatus & 1) != 0;
            return Result;
        }

        // HXSPRITE.PAS:787
        public override int GetShowPriority()
        {
            int Result = default;
            Result = GetLevel() << 26;
            return Result;
        }

        // HXSPRITE.PAS:791
        public override void MainRemove()
        {
            Deactivate();
            Remove();
            var call__311 = GetVisible();
            Assert(!call__311, (AnsiString)"FixedVisibleHS still in VisibilityList!!!", "HXSPRITE.PAS", 795);
        }

        // HXSPRITE.PAS:779
        public override void MakeVisible(TVisibilityList VisibilityList)
        {
            if ((FStatus & 1) == 0)
            {
                FStatus = (byte)(FStatus | 1);
                var call__312 = GetShowPriority();
                VisibilityList.AddSortChild(this, call__312);
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
        public override int GetShowPriority()
        {
            int Result = default;
            Result = FShowPriority;
            return Result;
        }

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
        internal ImageLib.TLibraryImage FImage;
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
        public override byte GetLevel()
        {
            byte Result = default;
            Result = (byte)(int)(((uint)FShowPriority) >> 26);
            return Result;
        }

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
        public override bool GetVisible()
        {
            bool Result = default;
            Result = FStatus.Contains(TLibraryImageHSStatusElement.lsVisible);
            return Result;
        }

        // LibImgHS.PAS:40
        public override bool GetEditMode()
        {
            bool Result = default;
            Result = FStatus.Contains(TLibraryImageHSStatusElement.lsEditMode);
            return Result;
        }

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
        public override int GetShowPriority()
        {
            int Result = default;
            Result = (FYhp << 13) + FXhp + FShowPriority;
            return Result;
        }

        // LibImgHS.PAS:194
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            FImage.Show(FXhp - XhpSurf, FYhp - YhpSurf, SurfaceDesc);
            if (FStatus.Contains(TLibraryImageHSStatusElement.lsDestroyOnShow))
            {
                Destroy();
            }
        }

        // LibImgHS.PAS:126
        public override bool Place(TMapContainer Map, int Xhp, int Yhp, int Lhx)
        {
            bool Result = default;
            if (CanPlace(Map, Xhp, Yhp, Lhx))
            {
                FXhp = Xhp;
                FYhp = Yhp;
                FLhx = (byte)Lhx;
                if (FStatus.Contains(TLibraryImageHSStatusElement.lsMapIsOwner))
                {
                    var call__313 = PlaceOnMapEx(Map, Xhp, Yhp, Lhx, FImage.Width, FImage.Height);
                    Result = call__313;
                }
                else
                {
                    var call__314 = PlaceUserOnMapEx(Map, Xhp, Yhp, Lhx, FImage.Width, FImage.Height);
                    Result = call__314;
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
            THSEngine Engine = default;
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
                var call__315 = GetShowPriority();
                VisibilityList.AddSortChild(this, call__315);
            }
        }

        // LibImgHS.PAS:101
        public virtual void SetImage(ImageLib.TLibraryImage Value)
        {
            if (FStatus.Contains(TLibraryImageHSStatusElement.lsPlacedOnMap))
            {
                if (Value == null || (FImage.Width != Value.Width || FImage.Height != Value.Height))
                {
                    throw Exception.Create((AnsiString)"Cannot change image when placed on map (TLibraryImageHS)");
                }
            }

            FImage = Value;
            if (FImage != null)
            {
                SetShowPriorityOffset((int)(((uint)FImage.Width) >> 1), (int)(((uint)FImage.Height) >> 1));
            }
        }

        // LibImgHS.PAS:121
        public virtual bool CanPlace(TMapContainer Map, int Xhp, int Yhp, int Lhx)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // LibImgHS.PAS:144
        public virtual bool PlaceHX(TMapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            int Xhp = default;
            int Yhp = default;
            HSEngine.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            var call__316 = Place(Map, (int)(((uint)FImage.Width) >> 2) + Xhp + 16, (int)(((uint)FImage.Height) >> 2) + Yhp + 16, Lhx);
            Result = call__316;
            return Result;
        }

        // LibImgHS.PAS:53
        public bool GetMapIsOwner()
        {
            bool Result = default;
            Result = FStatus.Contains(TLibraryImageHSStatusElement.lsMapIsOwner);
            return Result;
        }

        // LibImgHS.PAS:58
        public void SetMapIsOwner(bool Value)
        {
            if (FStatus.Contains(TLibraryImageHSStatusElement.lsPlacedOnMap))
            {
                HSEngine.OriginalAssert((AnsiString)"Cannot change MapIsOwner when places on map (TLibraryImageHS)", (AnsiString)"D:\\GEngine\\HSENGINE\\inc\\LibImgHS.PAS", 6626);
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
        public bool GetDestroyOnShow()
        {
            bool Result = default;
            Result = FStatus.Contains(TLibraryImageHSStatusElement.lsDestroyOnShow);
            return Result;
        }

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

        public ImageLib.TLibraryImage Image { get => FImage; set => SetImage(value); }
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
        public override bool GetVisible()
        {
            bool Result = default;
            Result = FVisible;
            return Result;
        }

        // HXSPRITE.PAS:630
        public override bool Place(TMapContainer Map, int Xhp, int Yhp, int Lhx)
        {
            bool Result = default;
            FXhp = Xhp;
            FYhp = Yhp;
            FLhx = (sbyte)Lhx;
            Result = true;
            return Result;
        }

    }

    // HSEngine.EMapObjectResourceMissing: TMapObject.ReadWrite raises it when the resource id read
    // from the stream is not in the resource list
    public partial class EMapObjectResourceMissing : Exception
    {
        // SysUtils.pas:174
        public EMapObjectResourceMissing(AnsiString Msg)
        {
    Message = Msg;
}
    }

    // a map object that covers the hexagons of its terrain list around its main hexagon. FXhx, FYhx
    // (the main hexagon, ids 3 and 4)
    public partial class TMultiHexMO : TMapObject
    {

        public TMultiHexMO(TECustomNode owner) : base(owner)
        {}

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
        public override sbyte GetXhx()
        {
            sbyte Result = default;
            Result = FXhx;
            return Result;
        }

        // MAPOBJ.PAS:905
        public override sbyte GetYhx()
        {
            sbyte Result = default;
            Result = FYhx;
            return Result;
        }

        // MAPOBJ.PAS:910
        public override int GetXYL()
        {
            int Result = default;
            var call__317 = GetLhx();
            Result = (call__317 << 16) + (FXhx + (FYhx << 8));
            return Result;
        }

        // MAPOBJ.PAS:915
        public override int GetShowPriority()
        {
            int Result = default;
            int Xhp = default;
            int Yhp = default;
            HSEngine.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            Result = FResource.FShowPriorityOffset + (Yhp << 13) + Xhp;
            return Result;
        }

        // MAPOBJ.PAS:944
        // { Connect adds the object to the fields of its terrain list around the main hexagon }
        public override void Connect()
        {
            int Count = default;
            TMapLevel MapLevel = default;
            TMapField MapField = default;
            THNWalkData Walk = default;
            if (!FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
            {
                FStatus = FStatus + TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moPlacedOnMap);
                Count = GetTerrainCount();
                MapLevel = (TMapLevel)Owner.Owner;
                HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                while (Walk.HN < Count)
                {
                    var call__318 = GetTerrain(Walk.HN);
                    bool bool__319 = call__318 >= -1;
                    if (!bool__319)
                    {
                        var call__320 = GetOverlay(Walk.HN);
                        bool__319 = call__320 >= -1;
                    }

                    if (bool__319 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                    {
                        var call__321 = MapLevel.GetField(Walk.X, Walk.Y);
                        MapField = call__321;
                        if (MapField == Owner)
                        {
                            MapField.RemoveHS(this);
                        }

                        MapField.AddHS(this, Walk.HN);
                    }

                    HSEngine.WalkToNextHN(ref Walk);
                }
            }
        }

        // MAPOBJ.PAS:972
        public override void Disconnect()
        {
            int Count = default;
            TMapLevel MapLevel = default;
            THNWalkData Walk = default;
            if (FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
            {
                Count = GetTerrainCount();
                MapLevel = (TMapLevel)Owner.Owner;
                FStatus = FStatus - TSet<TMapObjectStatusElement>.Of(TMapObjectStatusElement.moPlacedOnMap);
                HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                while (Walk.HN < Count)
                {
                    var call__322 = GetTerrain(Walk.HN);
                    bool bool__323 = call__322 >= -1;
                    if (!bool__323)
                    {
                        var call__324 = GetOverlay(Walk.HN);
                        bool__323 = call__324 >= -1;
                    }

                    if (bool__323 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                    {
                        var call__325 = MapLevel.GetField(Walk.X, Walk.Y);
                        call__325.RemoveHS(this);
                    }

                    HSEngine.WalkToNextHN(ref Walk);
                }

                Owner = null;
            }
        }

        // MAPOBJ.PAS:930
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            TMapObjectResource R = default;
            HSEngine.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            if (FResource.FImageSequenceList == null)
            {
                var call__326 = FResource.FImageLibrary.Get(0);
                call__326.Show(Xhp - XhpSurf + ((TMultiHexMOR)FResource).FdXImage, Yhp - YhpSurf + ((TMultiHexMOR)FResource).FdYImage, SurfaceDesc);
            }
            else
            {
                FResource.ShowComplexImage(Xhp - XhpSurf, Yhp - YhpSurf, SurfaceDesc, 0, (uint)(Xhp + Yhp));
            }
        }

        // MAPOBJ.PAS:995
        public override bool PlaceOnMap()
        {
            bool Result = default;
            int Count = default;
            TMapLevel MapLevel = default;
            THNWalkData Walk = default;
            if (!FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
            {
                Connect();
                Count = GetTerrainCount();
                MapLevel = (TMapLevel)Owner.Owner;
                if (GetLevel() <= 15)
                {
                    HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                    while (Walk.HN < Count)
                    {
                        var call__327 = GetTerrain(Walk.HN);
                        bool bool__328 = call__327 >= -1;
                        if (!bool__328)
                        {
                            var call__329 = GetOverlay(Walk.HN);
                            bool__328 = call__329 >= -1;
                        }

                        if (bool__328 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                        {
                            MapLevel.FMap.InvalidateStaticScene(Walk.X, Walk.Y, ((TMapField)Owner).FLhx);
                        }

                        HSEngine.WalkToNextHN(ref Walk);
                    }
                }

                if (!GetEditMode())
                {
                    var call__330 = HSEngine.HNtoRad(Count);
                    MapLevel.FMap.TerrainChangedRad(FXhx, FYhx, (sbyte)MapLevel.FLhx, call__330);
                }
            }

            Result = true;
            return Result;
        }

        // MAPOBJ.PAS:1023
        public override void RemoveFromMap()
        {
            int Count = default;
            TMapLevel MapLevel = default;
            TMapField MapField = default;
            sbyte TerrainType = default;
            sbyte OverlayType = default;
            THNWalkData Walk = default;
            if (FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap))
            {
                MapLevel = (TMapLevel)Owner.Owner;
                Disconnect();
                if (!MapLevel.FMap.FStatus.Contains(THSMapStatusElement.msDestroying))
                {
                    Count = GetTerrainCount();
                    HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                    while (Walk.HN < Count)
                    {
                        var call__331 = GetTerrain(Walk.HN);
                        TerrainType = call__331;
                        var call__332 = GetOverlay(Walk.HN);
                        OverlayType = call__332;
                        if ((TerrainType >= -1 || OverlayType >= -1) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                        {
                            var call__333 = MapLevel.GetField(Walk.X, Walk.Y);
                            MapField = call__333;
                            if (TerrainType >= 0 || OverlayType >= 0)
                            {
                                MapField.UpdateTerrain();
                            }

                            if (GetLevel() <= 15)
                            {
                                MapLevel.FMap.InvalidateStaticScene(Walk.X, Walk.Y, MapLevel.FLhx);
                            }
                        }

                        HSEngine.WalkToNextHN(ref Walk);
                    }

                    var call__334 = HSEngine.HNtoRad(Count);
                    MapLevel.FMap.TerrainChangedRad(FXhx, FYhx, (sbyte)MapLevel.FLhx, call__334);
                }
            }
        }

        // MAPOBJ.PAS:1122
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            int Xhx = default;
            int Yhx = default;
            int HN = default;
            if (FResource.FEngine.FHSMap.FStatus.Contains(THSMapStatusElement.msEditting))
            {
                HSEngine.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
                var call__335 = HSEngine.dHXtoHN(((TMapField)Owner).FXhx, ((TMapField)Owner).FYhx, Xhx, Yhx);
                HN = call__335;
                bool bool__336 = HN < FResource.FTerrainList.FCount;
                if (bool__336)
                {
                    var call__337 = FResource.FTerrainList.GetTerrain(HN);
                    bool bool__338 = call__337 >= 0;
                    if (!bool__338)
                    {
                        var call__339 = FResource.FTerrainList.GetOverlay(HN);
                        bool__338 = call__339 >= 0;
                    }

                    bool__336 = bool__338;
                }

                Result = bool__336;
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
            bool Result = default;
            int Count = default;
            sbyte TerrainType = default;
            sbyte OverlayType = default;
            THexagonSprite HS = default;
            THNWalkData Walk = default;
            Result = true;
            Count = FResource.FTerrainList.FCount;
            HSEngine.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (Result && Walk.HN < Count)
            {
                var call__340 = GetTerrain(Walk.HN);
                TerrainType = call__340;
                var call__341 = GetOverlay(Walk.HN);
                OverlayType = call__341;
                if ((TerrainType >= 0 || OverlayType >= 0) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapContainer.FWidth && Walk.Y < MapContainer.FHeight)
                {
                    var call__342 = MapContainer.GetField(Walk.X, Walk.Y, Lhx);
                    var call__343 = call__342.GetMaxLevelNoneTransparentHS();
                    HS = call__343;
                    Result = HS == null || CanPlaceOnHS(HS);
                }

                HSEngine.WalkToNextHN(ref Walk);
            }

            bool bool__344 = Result;
            if (bool__344)
            {
                var call__345 = MapContainer.FMap.CanChangeTerrain(this, Xhx, Yhx, Lhx, FResource.FTerrainList);
                bool__344 = call__345;
            }

            Result = bool__344;
            return Result;
        }

        // MAPOBJ.PAS:1138
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            FXhx = Xhx;
            FYhx = Yhx;
            Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
            return Result;
        }

        // MAPOBJ.PAS:1214
        public virtual void SortMapFields()
        {
            TMapLevel MapLevel = default;
            THNWalkData Walk = default;
            MapLevel = (TMapLevel)Owner.Owner;
            HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (true)
            {
                var call__346 = GetTerrainCount();
                if (!(Walk.HN < call__346))
                    break;
                {
                    var call__347 = GetTerrain(Walk.HN);
                    bool bool__348 = call__347 >= 0;
                    if (!bool__348)
                    {
                        var call__349 = GetOverlay(Walk.HN);
                        bool__348 = call__349 >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight;
                    }

                    if (bool__348)
                    {
                        // { Original skips bounds for terrain. }
                        var call__350 = MapLevel.GetField(Walk.X, Walk.Y);
                        call__350.Sort();
                    }

                    HSEngine.WalkToNextHN(ref Walk);
                }
            }
        }

        // MAPOBJ.PAS:1091
        public virtual bool CanPlaceOnHS(THexagonSprite HS)
        {
            bool Result = default;
            Result = HS.GetLevel() < 10;
            return Result;
        }

        // MAPOBJ.PAS:1146
        // { The terrain type that most of the fields under the object have (the counts of Default win ties) }
        public virtual sbyte GetMostFrequentMapFieldTerrainType(TMapLevel MapLevel, sbyte Xhx, sbyte Yhx, sbyte Default)
        {
            sbyte Result = default;
            TTerrainList TerrainList = default;
            sbyte TerrainType = default;
            FixedArray_byte_128 Counts = default;
            THNWalkData Walk = default;
            Result = Default;
            TerrainList = FResource.FTerrainList;
            if (Result < 0)
            {
                Result = 0;
            }

            Counts = default;
            HSEngine.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (Walk.HN < TerrainList.FCount)
            {
                var call__351 = TerrainList.GetTerrain(Walk.HN);
                bool bool__352 = call__351 >= 0;
                if (!bool__352)
                {
                    var call__353 = GetOverlay(Walk.HN);
                    bool__352 = call__353 >= 0;
                }

                if (bool__352 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    var call__354 = MapLevel.GetField(Walk.X, Walk.Y);
                    TerrainType = call__354.FTerrainType;
                    if (TerrainType >= 0)
                    {
                        Counts[TerrainType] = (byte)(Counts[TerrainType] + 1);
                        if (Counts[TerrainType] > Counts[Result])
                        {
                            Result = TerrainType;
                        }
                    }
                }

                HSEngine.WalkToNextHN(ref Walk);
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
            int Count = default;
            THNWalkData Walk = default;
            Count = FResource.FTerrainList.FCount;
            HSEngine.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (Walk.HN != Count)
            {
                var call__355 = GetTerrain(Walk.HN);
                bool bool__356 = call__355 >= 0;
                if (!bool__356)
                {
                    var call__357 = GetOverlay(Walk.HN);
                    bool__356 = call__357 >= 0;
                }

                if (bool__356 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    var call__358 = MapLevel.GetField(Walk.X, Walk.Y);
                    call__358.UpdateTerrain();
                }

                HSEngine.WalkToNextHN(ref Walk);
            }
        }

        // MAPOBJ.PAS:1075
        public void UpdateMapFields(TMapLevel MapLevel, int Xhx, int Yhx)
        {
            int Count = default;
            THNWalkData Walk = default;
            Count = FResource.FTerrainList.FCount;
            HSEngine.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (Walk.HN != Count)
            {
                var call__359 = GetTerrain(Walk.HN);
                bool bool__360 = call__359 >= 0;
                if (!bool__360)
                {
                    var call__361 = GetOverlay(Walk.HN);
                    bool__360 = call__361 >= 0;
                }

                if (bool__360 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    var call__362 = MapLevel.GetField(Walk.X, Walk.Y);
                    call__362.UpdateMapField();
                }

                HSEngine.WalkToNextHN(ref Walk);
            }
        }

        // MAPOBJ.PAS:1178
        public void OverrideTerrain()
        {
            int Count = default;
            TMapLevel MapLevel = default;
            TMapField MapField = default;
            FixedArray_sbyte_2 Terrain = default;
            THNWalkData Walk = default;
            MapLevel = (TMapLevel)Owner.Owner;
            Count = GetTerrainCount();
            HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (Walk.HN < Count)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
                {
                    var call__363 = GetTerrain(Walk.HN);
                    Terrain[0] = call__363;
                    var call__364 = GetOverlay(Walk.HN);
                    Terrain[1] = call__364;
                    var call__365 = MapLevel.GetField(Walk.X, Walk.Y);
                    MapField = call__365;
                    if (Terrain[0] >= 0)
                    {
                        MapField.FTerrainType = Terrain[0];
                    }

                    if (Terrain[1] >= 0)
                    {
                        MapField.FOverlayType = Terrain[1];
                    }
                }

                HSEngine.WalkToNextHN(ref Walk);
            }

            HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (Walk.HN < Count)
            {
                var call__366 = GetTerrain(Walk.HN);
                bool bool__367 = call__366 >= 0;
                if (!bool__367)
                {
                    var call__368 = GetOverlay(Walk.HN);
                    bool__367 = call__368 >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight;
                }

                if (bool__367)
                {
                    // { Original skips bounds for terrain. }
                    var call__369 = MapLevel.GetField(Walk.X, Walk.Y);
                    call__369.TerrainChanged();
                }

                HSEngine.WalkToNextHN(ref Walk);
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

    // the base of the terrain hexagons
    public partial class TAbstractHexagon : TMapObject
    {
        public new const int ClassID = 65904;

        public TAbstractHexagon(TECustomNode owner) : base(owner)
        { }

        // MAPOBJ.PAS:682
        public override byte GetLevel()
        {
            byte Result = default;
            Result = 0;
            return Result;
        }

        // MAPOBJ.PAS:671
        public override int GetShowPriority()
        {
            int Result = default;
            int Xhp = default;
            int Yhp = default;
            Xhp = ((TMapField)Owner).FXhx * 32 + 8;
            Yhp = (((TMapField)Owner).FXhx & 1) * 16 + ((TMapField)Owner).FYhx * 32;
            Result = Xhp + (Yhp << 13);
            return Result;
        }

        // MAPOBJ.PAS:704
        public override void ChangeTerrain(in THSTerrainInfo Msg)
        {
            THexagonControl Control = default;
            TMapObjectResource R = default;
            bool bool__370 = Msg.HN == 0;
            if (bool__370)
            {
                var call__371 = GetTerrain(0);
                bool__370 = call__371 != Msg.TerrainType;
            }

            if (bool__370)
            {
                var call__372 = ((TAbstractHexagonResource)FResource).GetHexagonControl();
                Control = call__372;
                if (Control != null)
                {
                    var call__373 = FResource.EClass.ClassID;
                    var call__374 = Control.GetRndResource(Msg.TerrainType, call__373);
                    R = call__374;
                    if (R != null)
                    {
                        SetResource(R);
                        UpdateTransitions();
                        var call__375 = GetLhx();
                        var call__376 = GetYhx();
                        var call__377 = GetXhx();
                        FResource.FEngine.FHSMap.FMapContainer.FMap.InvalidateStaticScene(call__377, call__376, call__375);
                    }
                    else
                    {
                        var call__378 = GetLhx();
                        var call__379 = GetYhx();
                        var call__380 = GetXhx();
                        var call__381 = ((TAbstractHexagonResource)FResource).GetHexagonControl();
                        call__381.Place((TMapContainer)Owner.Owner.Owner, call__380, call__379, call__378, Msg.TerrainType);
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
        public override void SetResource(TMapObjectResource Value)
        {
            base.SetResource(Value);
            if (FStatus.Contains(TMapObjectStatusElement.moActivated))
            {
                UpdateTransitions();
            }
        }

        // MAPOBJ.PAS:733
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            TMapField MapField = default;
            THexagonSprite HS = default;
            if (Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < MapContainer.FWidth && Yhx < MapContainer.FHeight && Lhx < MapContainer.FLevelCount)
            {
                MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
                HS = MapField.GetMaxLevelNoneTransparentHS();
                bool bool__382 = HS == null;
                if (!bool__382)
                {
                    var call__383 = HS.GetLevel();
                    bool bool__384 = call__383 < 10;
                    if (!bool__384)
                    {
                        var call__385 = GetOverlay(0);
                        var call__386 = GetTerrain(0);
                        var call__387 = MapField.CanChangeTerrain(this, call__386, call__385);
                        bool__384 = call__387;
                    }

                    bool__382 = bool__384 && !(HS is TAbstractBorderHexagon);
                }

                Result = bool__382;
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
            bool Result = default;
            THexagonSprite HS = default;
            var call__388 = MapContainer.GetField(Xhx, Yhx, Lhx);
            var call__389 = call__388.FindOwnedHS(typeof(TAbstractHexagon));
            HS = call__389;
            if (HS != null)
            {
                HS.Destroy();
            }

            Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
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

    public partial class TAbstractHexagonResource : TMapObjectResource
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
                    var call__390 = GetHexagonControl();
                    call__390.RegisterResource(this);
                }
            }
        }

        // MAPOBJ.PAS:776
        public virtual THexagonControl GetHexagonControl()
        {
            THexagonControl Result = default;
            Result = FEngine.FHSSet.FHexagonControl;
            return Result;
        }

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
        {}


    }

    // the base of roads and bridges
    public partial class TAbstractRoad : TMapObject
    {
        public new const int ClassID = 65906;

        public TAbstractRoad(TECustomNode owner) : base(owner)
        { }


        // MAPOBJ.PAS:803
        public override byte GetLevel()
        {
            byte Result = default;
            Result = 10;
            return Result;
        }

        // MAPOBJ.PAS:808
        public override int GetShowPriority()
        {
            int Result = default;
            int Xhp = default;
            int Yhp = default;
            Xhp = ((TMapField)Owner).FXhx * 32 + 8;
            Yhp = (((TMapField)Owner).FXhx & 1) * 16 + ((TMapField)Owner).FYhx * 32;
            Result = Xhp + ((Yhp << 13) + 671088640);
            return Result;
        }

        // MAPOBJ.PAS:848
        public override void ChangeTerrain(in THSTerrainInfo Msg)
        {
            TMapObjectResource R = default;
            bool bool__391 = Msg.HN == 0;
            if (bool__391)
            {
                var call__392 = GetTerrain(0);
                bool__391 = call__392 != Msg.TerrainType;
            }

            if (bool__391)
            {
                var call__393 = FResource.EClass.ClassID;
                var call__394 = ((TAbstractRoadResource)FResource).GetRoadControl();
                var call__395 = call__394.GetRndResource(Msg.TerrainType, call__393);
                R = call__395;
                if (R != null)
                {
                    SetResource(R);
                    UpdateTransitions();
                    var call__396 = GetLhx();
                    var call__397 = GetYhx();
                    var call__398 = GetXhx();
                    FResource.FEngine.FHSMap.FMapContainer.FMap.InvalidateStaticScene(call__398, call__397, call__396);
                }
                else
                {
                    var call__399 = GetLhx();
                    var call__400 = GetYhx();
                    var call__401 = GetXhx();
                    var call__402 = ((TAbstractRoadResource)FResource).GetRoadControl();
                    var call__403 = call__402.Place((TMapContainer)Owner.Owner.Owner, call__401, call__400, call__399, Msg.TerrainType);
                    if (call__403 == null)
                    {
                        Destroy();
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
            bool Result = default;
            THexagonSprite HS = default;
            bool bool__404 = Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < MapContainer.FWidth && Yhx < MapContainer.FHeight && Lhx < MapContainer.FLevelCount;
            if (bool__404)
            {
                var call__405 = MapContainer.GetField(Xhx, Yhx, Lhx);
                var call__406 = FResource.FTerrainList.GetTerrain(0);
                bool__404 = call__405.FTerrainType == call__406;
            }

            if (bool__404)
            {
                var call__407 = MapContainer.GetField(Xhx, Yhx, Lhx);
                var call__408 = call__407.GetMaxLevelNoneTransparentHS();
                HS = call__408;
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

    public partial class TAbstractRoadResource : TMapObjectResource
    {
        public TAbstractRoadResource(TECustomNode owner) : base(owner)
        {}


        // MAPOBJ.PAS:874
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 65556)
            {
                var call__409 = GetRoadControl();
                call__409.RegisterResource(this);
            }

            if (Msg.MsgID == 65621)
            {
                Msg.Result = 6;
            }
        }

        // MAPOBJ.PAS:883
        public override TMapObject PlaceResourceUser(THSMap HSMap, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            TMapObject Result = default;
            int Xhp = default;
            int Yhp = default;
            Result = (TMapObject)CreateResourceUser();
            HSEngine.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
            Xhp += 16;
            Yhp += 16;
            if (!HSMap.Place(Result, Xhp, Yhp, Lhx))
            {
                Result.Free();
                Result = null;
            }

            return Result;
        }

        // MAPOBJ.PAS:869
        public virtual TRoadControl GetRoadControl()
        {
            TRoadControl Result = default;
            Result = FEngine.FHSSet.FRoadControl;
            return Result;
        }
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
            TXYL Result = default;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if (NewCapacity < FCount || NewCapacity > MaxListSize)
            {
                Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
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
            if (NewCount < 0 || NewCount > MaxListSize)
            {
                Error((AnsiString)"List count out of bounds (%d)", NewCount);
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
            int Delta = default;
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
        public override void Destroy()
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
            int Result = default;
            Result = FCount;
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
            int Result = default;
            Result = FCount;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                Rtl.Rtl.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), (FCount - Index) * 4);
            }
        }

        // HSEngine.pas:1651
        public void Exchange(int Index1, int Index2)
        {
            TXYL Item = default;
            if (Index1 < 0 || Index1 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if (Index2 < 0 || Index2 >= FCount)
            {
                Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // HSEngine.pas:1662
        public TXYLList Expand()
        {
            TXYLList Result = default;
            if (FCount == FCapacity)
            {
                Grow();
            }

            Result = this;
            return Result;
        }

        // HSEngine.pas:1668
        public TXYL First()
        {
            TXYL Result = default;
            Result = Get(0);
            return Result;
        }

        // HSEngine.pas:1691
        public int IndexOf(TXYL Item)
        {
            int Result = default;
            Result = 0;
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
            int Result = default;
            TXYL Item = default;
            Item.X = X;
            Item.Y = Y;
            Result = 0;
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
            int Result = default;
            Result = 0;
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
                Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            if (Index < FCount)
            {
                Rtl.Rtl.Move(MemoryMarshal.AsBytes(FList.AsSpan(Index, FCount - Index)), MemoryMarshal.AsBytes(FList.AsSpan(Index + 1, FCount - Index)), (FCount - Index) * 4);
            }

            FList[Index] = Item;
            FCount += 1;
        }

        // HSEngine.pas:1728
        public TXYL Last()
        {
            TXYL Result = default;
            var call__411 = Get(FCount - 1);
            Result = call__411;
            return Result;
        }

        // HSEngine.pas:1733
        public void Move(int CurIndex, int NewIndex)
        {
            TXYL Item = default;
            if (CurIndex != NewIndex)
            {
                if (NewIndex < 0 || NewIndex >= FCount)
                {
                    Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // HSEngine.pas:1753
        public int Remove(TXYL Item)
        {
            int Result = default;
            Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // HSEngine.pas:1759
        public void Pack()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__412 = 0;
                for (; I >= end__412; I -= 1)
                {
                    var call__413 = this[I];
                    if (Unsafe.BitCast<TXYL, int>(call__413) == Unsafe.As<TXYL, int>(ref FDefault))
                    {
                        Delete(I);
                    }
                }
            }
        }

        // HSEngine.pas:1815
        public void Sort(TXYLSortCompare Compare)
        {
            if (FList.Length != 0 && Count > 0)
            {
                HSEngine.QuickSortXYL(FList, 0, Count - 1, Compare);
            }
        }

        // HSEngine.pas:1821
        public bool Compare(TXYLList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__414 = CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 4);
                Result = call__414;
            }

            return Result;
        }

        // HSEngine.pas:1828
        public void LoadFromStream(TStream Stream)
        {
            int Count = default;
            int Run = default;
            TXYL Item = default;
            Clear();
            Stream.Read(Bytes.Of(ref FDefault, 4));
            Stream.Read(Bytes.Of(ref Count, 4));
            SetCapacity(Count);
            while (Count != FCount)
            {
                Stream.Read(Bytes.Of(ref Item, 4));
                if (Unsafe.As<TXYL, int>(ref Item) == Unsafe.As<TXYL, int>(ref FDefault))
                {
                    Stream.Read(Bytes.Of(ref Run, 4));
                    SetCount(FCount + Run);
                }
                else
                {
                    Add(Item);
                }
            }
        }

        // HSEngine.pas:1850
        public void SaveToStream(TStream Stream)
        {
            int I = default;
            int Run = default;
            int DefaultRun()
            {
                int Result = default;
                int J = default;
                Result = 0;
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
        protected internal static void Error(AnsiString Msg, int Data)
        {
            throw EListError.CreateFmt(Msg, [Data]);
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
        public AnsiString ID { get => ReadShortString(ID__image); set => WriteShortString(ID__image, value); }

        [FieldOffset(9)]
        public FixedArray_byte_41 Name__image;
        public AnsiString Name { get => ReadShortString(Name__image); set => WriteShortString(Name__image, value); }
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

    public static partial class HSEngine
    {
        public static readonly AnsiString TerrainStr = (AnsiString)"Terrain";
        public static readonly AnsiString BridgeStr = (AnsiString)"Bridge";
        public static readonly AnsiString UpperIsometricHexagonStr = (AnsiString)"Upper Isometric Hexagon";
        public static readonly AnsiString LowerIsometricHexagonStr = (AnsiString)"Lower Isometric Hexagon";
        public static readonly AnsiString InvalidPlaceMentStr = (AnsiString)"Invalid Placement";
        public static readonly AnsiString HexagonStr = (AnsiString)"Hexagon";
        public static readonly AnsiString SelectedStr = (AnsiString)"selected";
        public static readonly AnsiString TransparentStr = (AnsiString)"Transparent";
        public static readonly AnsiString LoadingStr = (AnsiString)"Loading";
        public static readonly AnsiString ClosingStr = (AnsiString)"Closing";
        public static readonly AnsiString SaveChangesToXStr = (AnsiString)"Save changes to %s";
        public static readonly AnsiString OneMomentPleaseStr = (AnsiString)"One moment please...";
        public static readonly AnsiString CutStr = (AnsiString)"Cu&t";
        public static readonly AnsiString PasteStr = (AnsiString)"&Paste";
        public static readonly AnsiString DeleteStr = (AnsiString)"&Delete";
        public static readonly AnsiString CopyStr = (AnsiString)"&Copy";
        public static readonly AnsiString EditStr = (AnsiString)"&Edit";
        // HSEngine.pas:3889
        // Group A, third part: the unit routines (code.). HXtoV, HXtoH, HVtoHX, HPtoHX, HPtoHXs and
        // WalkToNextHN are assembler in the original (hand-scheduled code, explicit RETs)
        // { Hexagon coordinates }
        // { Hexagon coordinates (hx: column X, row Y; the odd columns are half a hexagon lower), hexagon
//   numbers (HN: 0 the centre, then the rings, 6 * R numbers on ring R, clockwise from the top),
//   hexagon directions (HT: 1..6 like the HN of ring 1), pixel coordinates (hp: 32 pixels per column
//   and row) and the two diagonal axes (H, V). }
        public static int dHXtoRad(int X1, int Y1, int X2, int Y2)
        {
            int Result = default;
            int dX = default;
            int dY = default;
            int Half = default;
            bool Up = default;
            dX = Math.Abs(X1 - X2);
            dY = Math.Abs(Y1 - Y2);
            if (dY >= dX)
            {
                Up = (dX & 1) != 0 && (!((X1 & 1) != 0) && Y1 < Y2 || !((X2 & 1) != 0) && Y2 < Y1);
                if (Up)
                {
                    Result = (int)(((uint)(dX + 1)) >> 1) + dY;
                }
                else
                {
                    Result = (int)(((uint)dX) >> 1) + dY;
                }
            }
            else
            {
                Result = dX;
                Half = (int)(((uint)(dX + 1)) >> 1);
                if ((dX & 1) != 0 && (!((X1 & 1) != 0) && Y1 < Y2 || !((X2 & 1) != 0) && Y2 < Y1))
                {
                    dY += 1;
                }

                if (dY > Half)
                {
                    Result += dY - Half;
                }
            }

            return Result;
        }

        // HSEngine.pas:3913
        public static bool ConnectingHX(int X1, int Y1, int X2, int Y2)
        {
            bool Result = default;
            if (X1 == X2)
            {
                Result = Y1 == Y2 + 1 || Y1 == Y2 - 1;
            }
            else
            {
                if (X1 == X2 - 1 || X1 == X2 + 1)
                {
                    if (!((X1 & 1) != 0))
                    {
                        Result = Y1 == Y2 || Y1 == Y2 + 1;
                    }
                    else
                    {
                        Result = Y1 == Y2 || Y1 == Y2 - 1;
                    }
                }
                else
                {
                    Result = false;
                }
            }

            return Result;
        }

        // HSEngine.pas:3928
        public static int HXtoRad(int Xhx, int Yhx)
        {
            int Result = default;
            int dX = default;
            int dY = default;
            int Half = default;
            bool Up = default;
            dX = Math.Abs(Xhx);
            dY = Math.Abs(Yhx);
            if (dY >= dX)
            {
                Up = (dX & 1) != 0 && (Yhx > 0 || !((Xhx & 1) != 0) && Yhx < 0);
                if (Up)
                {
                    Result = (int)(((uint)(dX + 1)) >> 1) + dY;
                }
                else
                {
                    Result = (int)(((uint)dX) >> 1) + dY;
                }
            }
            else
            {
                Result = dX;
                Half = (int)(((uint)(dX + 1)) >> 1);
                if ((dX & 1) != 0 && (Yhx > 0 || !((Xhx & 1) != 0) && Yhx < 0))
                {
                    dY += 1;
                }

                if (dY > Half)
                {
                    Result += dY - Half;
                }
            }

            return Result;
        }

        // HSEngine.pas:3996
        public static void HXtoHV(int Xhx, int Yhx, ref int H, ref int V)
        {
            H = HXtoH(Xhx, Yhx);
            V = HXtoV(Xhx, Yhx);
        }

        // HSEngine.pas:4004
        // { Hexagon numbers }
        public static int HNtoRad(int HN)
        {
            int Result = default;
            if (HN < 37)
            {
                Result = HNtoRadTable[HN];
            }
            else
            {
                // { the original lacks the "div 6" of InvHN and HNtoHX here }
                Result = (int)Math.Sqrt((HN - 1) * 12 + 9) + 3;
            }

            return Result;
        }

        // HSEngine.pas:4013
        public static int RadToHN(int Rad)
        {
            int Result = default;
            if (Rad <= 10)
            {
                Result = RadToHNTable[Rad];
            }
            else
            {
                Result = (Rad - 1) * Rad * 3 + 1;
            }

            return Result;
        }

        // HSEngine.pas:4021
        public static int InvHN(int HN)
        {
            int Result = default;
            int R = default;
            int N = default;
            if (HN < 37)
            {
                Result = InvHNTable[HN];
            }
            else
            {
                R = ((int)Math.Sqrt((HN - 1) * 12 + 9) + 3) / 6;
                N = R * 6;
                Result = (HN - (R - 1) * R * 3 - 1 + (int)(((uint)N) >> 1)) % N + ((R - 1) * R * 3 + 1);
            }

            return Result;
        }

        // HSEngine.pas:4035
        public static int HXtoHN(int Xhx, int Yhx)
        {
            int Result = default;
            int R = default;
            int Side = default;
            int Pos = default;
            int H = default;
            int V = default;
            R = HXtoRad(Xhx, Yhx);
            if (R != 0)
            {
                HXtoHV(Xhx, Yhx, ref H, ref V);
                if (R == H)
                {
                    Side = 0;
                    Pos = V + R;
                }
                else
                {
                    if (R == V)
                    {
                        Side = 2;
                        Pos = -H;
                    }
                    else
                    {
                        if (-R == H)
                        {
                            Side = 3;
                            Pos = -V + R;
                        }
                        else
                        {
                            if (-R == V)
                            {
                                Side = 5;
                                Pos = H;
                            }
                            else
                            {
                                if (R == Xhx)
                                {
                                    Side = 1;
                                    Pos = (Xhx + 1) / 2 + Yhx;
                                }
                                else
                                {
                                    if (R == -Xhx)
                                    {
                                        Side = 4;
                                        Pos = -Yhx - Xhx / 2;
                                    }
                                    else
                                    {
                                        throw Exception.Create((AnsiString)"arithemetic error!");
                                    }
                                }
                            }
                        }
                    }
                }

                Result = Pos + ((R - 1) * R * 3 + 1 + Side * R);
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // HSEngine.pas:4082
        public static void HNtoHX(int HN, ref int Xhx, ref int Yhx)
        {
            int R = default;
            int Side = default;
            int Pos = default;
            if (HN < 37)
            {
                Xhx = HNtoHXTable[HN][0];
                Yhx = HNtoHXTable[HN][1];
            }
            else
            {
                R = ((int)Math.Sqrt((HN - 1) * 12 + 9) + 3) / 6;
                HN = HN - (R - 1) * R * 3 - 1;
                Side = HN / R;
                Pos = HN % R;
                switch (Side)
                {
                    case 0:
                        HVtoHX(R, Pos - R, ref Xhx, ref Yhx);
                        break;
                    case 1:
                        Xhx = R;
                        Yhx = Pos - (R + 1) / 2;
                        break;
                    case 2:
                        HVtoHX(-Pos, R, ref Xhx, ref Yhx);
                        break;
                    case 3:
                        HVtoHX(-R, R - Pos, ref Xhx, ref Yhx);
                        break;
                    case 4:
                        Xhx = -R;
                        Yhx = -Pos + R / 2;
                        break;
                    case 5:
                        HVtoHX(Pos, -R, ref Xhx, ref Yhx);
                        break;
                }
            }
        }

        // HSEngine.pas:4116
        public static void CenterHNtoHX(int Xhx, int Yhx, int HN, ref int NewXhx, ref int NewYhx)
        {
            int R = default;
            int Side = default;
            int Pos = default;
            if (HN != 0)
            {
                R = ((int)Math.Sqrt((HN - 1) * 12 + 9) + 3) / 6;
                HN = HN - (R - 1) * R * 3 - 1;
                Side = HN / R;
                Pos = HN % R;
                switch (Side)
                {
                    case 0:
                        HVtoHX(R, Pos - R, ref NewXhx, ref NewYhx);
                        break;
                    case 1:
                        NewXhx = R;
                        NewYhx = Pos - (R + 1) / 2;
                        break;
                    case 2:
                        HVtoHX(-Pos, R, ref NewXhx, ref NewYhx);
                        break;
                    case 3:
                        HVtoHX(-R, R - Pos, ref NewXhx, ref NewYhx);
                        break;
                    case 4:
                        NewXhx = -R;
                        NewYhx = -Pos + R / 2;
                        break;
                    case 5:
                        HVtoHX(Pos, -R, ref NewXhx, ref NewYhx);
                        break;
                }
            }
            else
            {
                NewXhx = 0;
                NewYhx = 0;
            }

            if ((Xhx & 1) != 0 && (NewXhx & 1) != 0)
            {
                NewYhx = NewYhx + Yhx + 1;
            }
            else
            {
                NewYhx = NewYhx + Yhx;
            }

            NewXhx = NewXhx + Xhx;
        }

        // HSEngine.pas:4155
        public static void TranslateHX(int FromXhx, int FromYhx, int ToXhx, int ToYhx, int Xhx, int Yhx, ref int NewXhx, ref int NewYhx)
        {
            int dX = default;
            int dY = default;
            dX = ToXhx - FromXhx;
            dY = ToYhx - FromYhx;
            if ((dX & 1) != 0)
            {
                if ((FromXhx & 1) != 0)
                {
                    if (!((Xhx & 1) != 0))
                    {
                        dY -= 1;
                    }
                }
                else
                {
                    if ((Xhx & 1) != 0)
                    {
                        dY += 1;
                    }
                }
            }

            NewXhx = Xhx + dX;
            NewYhx = Yhx + dY;
        }

        // HSEngine.pas:4171
        public static void TranslateZeroHX(int Xhx, int Yhx, int dXhx, int dYhx, ref int NewXhx, ref int NewYhx)
        {
            if ((Xhx & 1) != 0 && (dXhx & 1) != 0)
            {
                NewYhx = dYhx + Yhx + 1;
            }
            else
            {
                NewYhx = dYhx + Yhx;
            }

            NewXhx = dXhx + Xhx;
        }

        // HSEngine.pas:4180
        public static int dHXtoHN(int X1, int Y1, int X2, int Y2)
        {
            int Result = default;
            TranslateHX(X1, Y1, 0, 0, X2, Y2, ref X2, ref Y2);
            Result = HXtoHN(X2, Y2);
            return Result;
        }

        // HSEngine.pas:4186
        public static int dHXtoHNfast(int X1, int Y1, int X2, int Y2)
        {
            int Result = default;
            if (X1 == X2)
            {
                if (Y2 < Y1)
                {
                    Result = 1;
                }
                else
                {
                    if (Y2 > Y1)
                    {
                        Result = 4;
                    }
                    else
                    {
                        Result = 0;
                    }
                }
            }
            else
            {
                if (!((X1 & 1) != 0))
                {
                    if (X2 < X1)
                    {
                        if (Y2 < Y1)
                        {
                            Result = 6;
                        }
                        else
                        {
                            Result = 5;
                        }
                    }
                    else
                    {
                        if (Y2 < Y1)
                        {
                            Result = 2;
                        }
                        else
                        {
                            Result = 3;
                        }
                    }
                }
                else
                {
                    if (X2 < X1)
                    {
                        if (Y2 > Y1)
                        {
                            Result = 5;
                        }
                        else
                        {
                            Result = 6;
                        }
                    }
                    else
                    {
                        if (Y2 > Y1)
                        {
                            Result = 3;
                        }
                        else
                        {
                            Result = 2;
                        }
                    }
                }
            }

            return Result;
        }

        // HSEngine.pas:4219
        // { Pixel coordinates }
        public static void HXtoHP(int Xhx, int Yhx, ref int Xhp, ref int Yhp)
        {
            Xhp = Xhx * 32 + 8;
            Yhp = Yhx * 32 + (Xhx & 1) * 16;
        }

        // HSEngine.pas:4252
        public static int HPtoHN(int Xhp, int Yhp)
        {
            int Result = default;
            HPtoHX(Xhp, Yhp, ref Xhp, ref Yhp);
            Result = HXtoHN(Xhp, Yhp);
            return Result;
        }

        // HSEngine.pas:4258
        public static void HNtoHP(int HN, ref int Xhp, ref int Yhp)
        {
            HNtoHX(HN, ref Xhp, ref Yhp);
            HXtoHP(Xhp, Yhp, ref Xhp, ref Yhp);
            Xhp -= 8;
        }

        // HSEngine.pas:4267
        // { Hexagon directions }
        public static int HXtoHT(int Xhx, int Yhx)
        {
            int Result = default;
            int H = default;
            H = HXtoH(Xhx, Yhx);
            if (H > 0)
            {
                if (Xhx < 0)
                {
                    Result = 6;
                }
                else
                {
                    if (HXtoV(Xhx, Yhx) < 0)
                    {
                        Result = 1;
                    }
                    else
                    {
                        Result = 2;
                    }
                }
            }
            else
            {
                if (H < 0)
                {
                    if (Xhx > 0)
                    {
                        Result = 3;
                    }
                    else
                    {
                        if (HXtoV(Xhx, Yhx) > 0)
                        {
                            Result = 4;
                        }
                        else
                        {
                            Result = 5;
                        }
                    }
                }
                else
                {
                    if (Xhx < 0)
                    {
                        Result = 6;
                    }
                    else
                    {
                        if (Xhx > 0)
                        {
                            Result = 3;
                        }
                        else
                        {
                            Result = 0;
                        }
                    }
                }
            }

            return Result;
        }

        // HSEngine.pas:4298
        public static int dHXtoHT(int X1, int Y1, int X2, int Y2)
        {
            int Result = default;
            int dX = default;
            int dY = default;
            X2 = X2 - X1;
            Y2 = Y2 - Y1;
            if ((X2 & 1) != 0 && (X1 & 1) != 0)
            {
                Y2 -= 1;
            }

            Result = HXtoHT(X2, Y2);
            return Result;
        }

        // HSEngine.pas:4310
        // { Walks }
        public static void GenerateWalkAroundRoute(int Xhx, int Yhx, TWalkAroundRouteCallBack CallBack)
        {
            int NXhx = default;
            int NYhx = default;
            int TXhx = default;
            int TYhx = default;
            int HN = default;
            bool Found = default;
            bool Cancel = default;
            Cancel = false;
            NXhx = 65535;
            Found = false;
            HN = 0;
            while (!Found && HN != 6 && !Cancel)
            {
                HN += 1;
                CenterHNtoHX(Xhx, Yhx, HN, ref NXhx, ref NYhx);
                Found = CallBack(NXhx, NYhx, ref Cancel);
            }

            if (Found && !Cancel)
            {
                Xhx = NXhx;
                Yhx = NYhx;
                while (!Cancel)
                {
                    HN += 2;
                    if (HN > 6)
                    {
                        HN -= 6;
                    }

                    do
                    {
                        CenterHNtoHX(Xhx, Yhx, HN, ref TXhx, ref TYhx);
                        HN -= 1;
                        // { the original asks for NXhx, NYhx here, not for the hexagon it just stepped to }
                        Found = CallBack(NXhx, NYhx, ref Cancel);
                    }
                    while (!(Found && !Cancel));
                    Xhx = TXhx;
                    Yhx = TYhx;
                }
            }
        }

        // HSEngine.pas:4346
        public static void InitHNWalk(ref THNWalkData Walk, int Xhx, int Yhx, int Radius)
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

        // HSEngine.pas:4431
        // { next ring }
        // { next side }
        // { next hexagon on the side }
        // { Locations }
        public static TXYL PackXYL(sbyte X, sbyte Y, sbyte L)
        {
            TXYL Result = default;
            Result.X = X;
            Result.Y = Y;
            Result.L = L;
            Result.Flag = 0;
            return Result;
        }

        // HSEngine.pas:4439
        public static bool LocationsConnected(TXYLList List)
        {
            bool Result = default;
            TXYLList Locations = default;
            int I = default;
            TXYL XYL = default;
            void Connect(TXYL XYL)
            {
                THNWalkData Walk = default;
                int I = default;
                InitHNWalk(ref Walk, XYL.X, XYL.Y, 1);
                while (Walk.Radius <= 1)
                {
                    {
                        I = 0;
                        int end__415 = Locations.Count - 1;
                        for (; I <= end__415; I += 1)
                        {
                            var call__416 = Locations[I];
                            if (call__416.Flag == 0)
                            {
                                var call__417 = Locations[I];
                                var call__418 = PackXYL((sbyte)Walk.X, (sbyte)Walk.Y, XYL.L);
                                if (Unsafe.BitCast<TXYL, int>(call__417) == Unsafe.BitCast<TXYL, int>(call__418))
                                {
                                    Locations.FList[I].Flag = 1;
                                    var call__419 = Locations[I];
                                    Connect(call__419);
                                }
                            }
                        }
                    }

                    WalkToNextHN(ref Walk);
                }
            }

            Locations = new TXYLList();
            {
                I = 0;
                int end__420 = List.Count - 1;
                for (; I <= end__420; I += 1)
                {
                    var call__421 = List[I];
                    XYL = call__421;
                    XYL.Flag = 0;
                    if (Locations.IndexOf(XYL) == -1)
                    {
                        Locations.Add(XYL);
                    }
                }
            }

            if (Locations.Count > 0)
            {
                if (Locations.Count > 1)
                {
                    Locations.FList[0].Flag = 1;
                    var call__422 = Locations[0];
                    Connect(call__422);
                    Result = true;
                    {
                        I = 0;
                        int end__423 = Locations.Count - 1;
                        for (; I <= end__423; I += 1)
                        {
                            var call__424 = Locations[I];
                            if (call__424.Flag != 1)
                            {
                                Result = false;
                            }
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

            Locations.Free();
            return Result;
        }

        // { the signature of a map file (THSEngine.SaveHSM, LoadPreviewHSM) }
        public static FixedArray_byte_4 HSMID = FixedArray_byte_4.From([72, 83, 77, 0]);
        // { offsets of the neighbours (HN 1..6) by column parity; 7: none }
        public static FixedArray_Aow_Units_FixedArray_Aow_Units_HSEngine_XYTableEntry_8_2 HNtoXYTable = FixedArray_Aow_Units_FixedArray_Aow_Units_HSEngine_XYTableEntry_8_2.From([FixedArray_Aow_Units_HSEngine_XYTableEntry_8.From([new XYTableEntry { X = 0, Y = 0 }, new XYTableEntry { X = 0, Y = -1 }, new XYTableEntry { X = 1, Y = -1 }, new XYTableEntry { X = 1, Y = 0 }, new XYTableEntry { X = 0, Y = 1 }, new XYTableEntry { X = -1, Y = 0 }, new XYTableEntry { X = -1, Y = -1 }, new XYTableEntry { X = 0, Y = 0 }]), FixedArray_Aow_Units_HSEngine_XYTableEntry_8.From([new XYTableEntry { X = 0, Y = 0 }, new XYTableEntry { X = 0, Y = -1 }, new XYTableEntry { X = 1, Y = 0 }, new XYTableEntry { X = 1, Y = 1 }, new XYTableEntry { X = 0, Y = 1 }, new XYTableEntry { X = -1, Y = 1 }, new XYTableEntry { X = -1, Y = 0 }, new XYTableEntry { X = 0, Y = 0 }])]);
        public static FixedArray_byte_37 HNtoRadTable = FixedArray_byte_37.From([0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3]);
        // { the hexagon number on the opposite side of the centre }
        public static FixedArray_byte_37 InvHNTable = FixedArray_byte_37.From([0, 4, 5, 6, 1, 2, 3, 13, 14, 15, 16, 17, 18, 7, 8, 9, 10, 11, 12, 28, 29, 30, 31, 32, 33, 34, 35, 36, 19, 20, 21, 22, 23, 24, 25, 26, 27]);
        // { the offsets of the hexagon numbers 0..36 (rings 0..3) from an even column }
        public static FixedArray_Aow_Units_FixedArray_int_2_37 HNtoHXTable = FixedArray_Aow_Units_FixedArray_int_2_37.From([FixedArray_int_2.From([0, 0]), FixedArray_int_2.From([0, -1]), FixedArray_int_2.From([1, -1]), FixedArray_int_2.From([1, 0]), FixedArray_int_2.From([0, 1]), FixedArray_int_2.From([-1, 0]), FixedArray_int_2.From([-1, -1]), FixedArray_int_2.From([0, -2]), FixedArray_int_2.From([1, -2]), FixedArray_int_2.From([2, -1]), FixedArray_int_2.From([2, 0]), FixedArray_int_2.From([2, 1]), FixedArray_int_2.From([1, 1]), FixedArray_int_2.From([0, 2]), FixedArray_int_2.From([-1, 1]), FixedArray_int_2.From([-2, 1]), FixedArray_int_2.From([-2, 0]), FixedArray_int_2.From([-2, -1]), FixedArray_int_2.From([-1, -2]), FixedArray_int_2.From([0, -3]), FixedArray_int_2.From([1, -3]), FixedArray_int_2.From([2, -2]), FixedArray_int_2.From([3, -2]), FixedArray_int_2.From([3, -1]), FixedArray_int_2.From([3, 0]), FixedArray_int_2.From([3, 1]), FixedArray_int_2.From([2, 2]), FixedArray_int_2.From([1, 2]), FixedArray_int_2.From([0, 3]), FixedArray_int_2.From([-1, 2]), FixedArray_int_2.From([-2, 2]), FixedArray_int_2.From([-3, 1]), FixedArray_int_2.From([-3, 0]), FixedArray_int_2.From([-3, -1]), FixedArray_int_2.From([-3, -2]), FixedArray_int_2.From([-2, -2]), FixedArray_int_2.From([-1, -3])]);
        // { the number of hexagons inside the rings 0..10 (the first hexagon number of each ring) }
        public static FixedArray_int_11 RadToHNTable = FixedArray_int_11.From([0, 1, 7, 19, 37, 61, 91, 127, 169, 217, 271]);
        public static int HSEngineDID;
        // HSEngine.pas:5411
        public static void Register()
        {
            RegisterComponents((AnsiString)"HS-Engine", [typeof(THexagonSpriteEngine), typeof(THSMapViewer)]);
        }

        // HSEngine.pas:1390
        public static void OriginalAssert(AnsiString Message, AnsiString Filename, int LineNumber)
        {
            Assert(false, Message, Filename.ToString(), LineNumber);
        }

        // { the hexagon direction opposite to the directions 1..6 }
        public static FixedArray_byte_6 OppositeHTTable = FixedArray_byte_6.From([4, 5, 6, 1, 2, 3]);
        // { the step from side to side of a ring around a centre (WalkToNextHN), by column parity }
        public static FixedArray_Aow_Units_FixedArray_Aow_Units_HSEngine_XYTableEntry_8_2 HNWalkTable = FixedArray_Aow_Units_FixedArray_Aow_Units_HSEngine_XYTableEntry_8_2.From([FixedArray_Aow_Units_HSEngine_XYTableEntry_8.From([new XYTableEntry { X = 1, Y = 0 }, new XYTableEntry { X = 0, Y = 1 }, new XYTableEntry { X = -1, Y = 0 }, new XYTableEntry { X = -1, Y = -1 }, new XYTableEntry { X = 0, Y = -1 }, new XYTableEntry { X = 1, Y = -1 }, new XYTableEntry { X = 0, Y = 0 }, new XYTableEntry { X = 0, Y = 0 }]), FixedArray_Aow_Units_HSEngine_XYTableEntry_8.From([new XYTableEntry { X = 1, Y = 1 }, new XYTableEntry { X = 0, Y = 1 }, new XYTableEntry { X = -1, Y = 1 }, new XYTableEntry { X = -1, Y = 0 }, new XYTableEntry { X = 0, Y = -1 }, new XYTableEntry { X = 1, Y = 0 }, new XYTableEntry { X = 0, Y = 0 }, new XYTableEntry { X = 0, Y = 0 }])]);
        // HSEngine.pas:1788
        public static void QuickSortXYL(Span<TXYL> SortList, int L, int R, TXYLSortCompare SCompare)
        {
            int I = default;
            int J = default;
            TXYL P = default;
            TXYL T = default;
            do
            {
                I = L;
                J = R;
                P = SortList[(int)(((uint)(L + R)) >> 1)];
                do
                {
                    while (true)
                    {
                        var call__425 = SCompare(SortList[I], P);
                        if (!(call__425 < 0))
                            break;
                        {
                            I += 1;
                        }
                    }

                    while (true)
                    {
                        var call__426 = SCompare(SortList[J], P);
                        if (!(call__426 > 0))
                            break;
                        {
                            J -= 1;
                        }
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

        // { copies of a field's sprite list: SendMsg and Loaded call methods that may remove sprites }
        public static FixedArray_Aow_Units_HSEngine_THexagonSprite_256 SendMsgList;
        public static FixedArray_Aow_Units_HSEngine_THexagonSprite_256 LoadedList;
    }
}
