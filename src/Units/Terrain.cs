// Translated from Terrain.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;

namespace Aow.Units.Terrain
{
    // FDecorationType (-1 none), FTerrainType. persisted as ids $12 and $11
    public partial class TTerrainMOR : TMultiHexMOR
    {
        public TTerrainMOR(TECustomNode owner) : base(owner)
        {
            FDecorationType = -1;
        }

        // Terrain.pas:327
        public new const int ClassID = 131431;

        internal sbyte FDecorationType;
        internal byte FTerrainType;
        // Terrain.pas:305
        public override void Destroy()
        {
            base.Destroy();
        }

        // Terrain.pas:310
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FTerrainType, 17);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDecorationType), 18);
        }

        // Terrain.pas:299


        // Terrain.pas:322
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131430;
            return Result;
        }

        public sbyte DecorationType { get => FDecorationType; set => FDecorationType = value; }
        public byte TerrainType { get => FTerrainType; set => FTerrainType = value; }

        // Terrain.pas:42


        // Terrain.pas:317
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TerrainStr;
            return Result;
        }

    }

    public partial class TTerrainMO : TMultiHexMO
    {
        public TTerrainMO(TECustomNode owner) : base(owner)
        {
        }

        // Terrain.pas:65
        public new const int ClassID = 131430;

        // Terrain.pas:60


        // Terrain.pas:179
        public override bool CanChangeTerrain(in THSSenderInfo Info)
        {
            bool Result = default;
            if (this != Info.Sender)
            {
                var call__1 = FResource.TerrainList.GetTerrain(Info.HN);
                if (call__1 >= 0)
                {
                    var call__2 = FResource.TerrainList.GetTerrain(Info.HN);
                    var call__3 = GetMostFrequentMapFieldTerrainType((TMapLevel)Owner.Owner, FXhx, FYhx, call__2);
                    if (call__3 == Info.MapField.TerrainType)
                    {
                        var call__4 = CanChangeOwnTerrain(Info.HN);
                        Result = call__4;
                    }
                    else
                    {
                        Result = false;
                    }
                }
                else
                {
                    Result = true;
                }
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // Terrain.pas:195
        public override void ChangeTerrain(in THSTerrainInfo Info)
        {
            sbyte TerrainType = default;
            sbyte NewTerrainType = default;
            TTerrainMOR R = default;
            TECustomNode OldOwner = default;
            var call__5 = FResource.TerrainList.GetTerrain(Info.HN);
            TerrainType = call__5;
            if (TerrainType >= 0 && TerrainType != Info.TerrainType)
            {
                var call__6 = FResource.TerrainList.GetTerrain(Info.HN);
                var call__7 = GetMostFrequentMapFieldTerrainType((TMapLevel)Owner.Owner, FXhx, FYhx, call__6);
                NewTerrainType = call__7;
                if (NewTerrainType != TerrainType)
                {
                    R = FindTerrainResource((byte)NewTerrainType);
                    if (R != null)
                    {
                        OldOwner = Owner;
                        RemoveFromMap();
                        SetResource(R);
                        Owner = OldOwner;
                        PlaceOnMap();
                        if (FResource.Level <= 15)
                        {
                            FResource.Engine.HSMap.StaticSceneInvalid = true;
                        }

                        OverrideTerrain();
                    }
                    else
                    {
                        Destroy();
                    }
                }
            }
        }

        // Terrain.pas:275
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            if (Msg.MsgID == 16)
            {
                var call__8 = FResource.TerrainList.GetTerrain(Msg.HN);
                if (call__8 >= 0)
                {
                    ClearTerrain(Msg.MapField, Msg.HN);
                }
                else
                {
                    if (Msg.HN == 0)
                    {
                        Destroy();
                    }
                }
            }
            else
            {
                base.MapFieldMsgProc(ref Msg);
            }

            if (Msg.MsgID == 17)
            {
                var call__9 = FResource.TerrainList.GetTerrain(Msg.HN);
                if (call__9 >= 0)
                {
                    Msg.Result = ((TTerrainMOR)FResource).FDecorationType;
                }
            }
        }

        // Terrain.pas:291
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
            if (Result && !GetEditMode())
            {
                OverrideTerrain();
            }

            return Result;
        }

        // Terrain.pas:226
        public virtual void ClearTerrain(TMapField MapField, int HN)
        {
            int Xhx = default;
            int Yhx = default;
            int Lhx = default;
            int X = default;
            int Y = default;
            TTerrainList TerrainList = default;
            THSMap Map = default;
            TTerrainMOR R = default;
            TEObjectList Resources = default;
            int I = default;
            bool Found = default;
            var call__10 = FResource.TerrainList.UsedTerrain();
            if (call__10 > 1)
            {
                R = null;
                var call__11 = GetResourceList(FResource.Engine);
                Resources = call__11.FChildren;
                I = Resources.Count;
                Found = false;
                while (!Found && I != 0)
                {
                    I -= 1;
                    R = (TTerrainMOR)Resources.List[I];
                    var call__12 = R.EClass.ClassID;
                    var call__13 = FResource.EClass.ClassID;
                    bool bool__14 = call__12 == call__13 && R.FDecorationType == ((TTerrainMOR)FResource).FDecorationType && R.FTerrainType == ((TTerrainMOR)FResource).FTerrainType;
                    if (bool__14)
                    {
                        var call__15 = R.TerrainList.UsedTerrain();
                        bool__14 = call__15 == 1;
                    }

                    bool bool__16 = bool__14;
                    if (bool__16)
                    {
                        var call__17 = R.TerrainList.GetTerrain(0);
                        bool__16 = call__17 >= 0;
                    }

                    bool bool__18 = bool__16;
                    if (bool__18)
                    {
                        var call__19 = GetOverlay(0);
                        var call__20 = R.TerrainList.GetOverlay(0);
                        bool__18 = call__19 == call__20;
                    }

                    Found = bool__18;
                }

                if (Found)
                {
                    Xhx = GetXhx();
                    Yhx = GetYhx();
                    Lhx = GetLhx();
                    Map = ((TMapLevel)Owner.Owner).Map;
                    TerrainList = FResource.TerrainList;
                    Destroy();
                    {
                        I = 0;
                        int end__21 = TerrainList.Count - 1;
                        for (; I <= end__21; I += 1)
                        {
                            if (TerrainList.GetTerrain(I) >= 0 && I != HN)
                            {
                                CenterHNtoHX(Xhx, Yhx, I, ref X, ref Y);
                                R.PlaceResourceUser(Map, (sbyte)X, (sbyte)Y, (sbyte)Lhx);
                            }
                        }
                    }
                }
                else
                {
                    Destroy();
                }
            }
            else
            {
                Destroy();
            }
        }

        // Terrain.pas:70
        public virtual TTerrainMOR FindDecorationResource(byte DecorationType)
        {
            TTerrainMOR Result = default;
            TList Candidates = default;
            TEObjectList Resources = default;
            int I = default;
            TTerrainMOR R = default;
            Candidates = new TList();
            var call__22 = GetResourceList(FResource.Engine);
            Resources = call__22.FChildren;
            {
                I = 0;
                int end__23 = Resources.Count - 1;
                for (; I <= end__23; I += 1)
                {
                    R = (TTerrainMOR)Resources.List[I];
                    var call__24 = R.EClass.ClassID;
                    var call__25 = FResource.EClass.ClassID;
                    bool bool__26 = call__24 == call__25 && R.FTerrainType == ((TTerrainMOR)FResource).FTerrainType && R.FDecorationType == DecorationType && R != FResource;
                    if (bool__26)
                    {
                        var call__27 = R.TerrainList.CompareTerrain(FResource.TerrainList, TSet<TTerrainCompareModeElement>.Of(TTerrainCompareModeElement.cmSameOverlayType));
                        bool__26 = call__27;
                    }

                    if (bool__26)
                    {
                        Candidates.Add(R);
                    }
                }
            }

            if (Candidates.Count != 0)
            {
                var call__28 = Random(Candidates.Count);
                Result = (TTerrainMOR)Candidates.List[call__28];
            }
            else
            {
                Result = null;
            }

            Candidates.Free();
            return Result;
        }

        // Terrain.pas:160
        public bool CanChangeOwnTerrain(int HN)
        {
            bool Result = default;
            TMapLevel Level = default;
            int Count = default;
            THNWalkData Walk = default;
            Result = true;
            Level = (TMapLevel)Owner.Owner;
            Count = GetTerrainCount();
            InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (Walk.HN < Count && Result)
            {
                bool bool__29 = Walk.HN != HN;
                if (bool__29)
                {
                    var call__30 = GetTerrain(Walk.HN);
                    bool__29 = call__30 >= 0;
                }

                if (bool__29 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var call__31 = Level.GetField(Walk.X, Walk.Y);
                    var call__32 = call__31.CanChangeTerrainEx(this);
                    Result = call__32;
                }

                WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // Terrain.pas:19


    }

    public static partial class Terrain
    {
        // Terrain.pas:332
        public static void RegisterTerrain(TEngine Engine)
        {
            Engine.RegisterEClasses([EClass.Of<TTerrainMO>(), EClass.Of<TTerrainMOR>()]);
        }
    }
}
