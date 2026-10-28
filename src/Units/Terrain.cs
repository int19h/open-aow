// Translated from Terrain.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;

namespace Aow.Units.Terrain
{
    // { Terrain.TTerrainMOR (instance size 72): +$44 FDecorationType (-1 none), +$45 FTerrainType;
//     persisted as ids $12 and $11 }
    public partial class TTerrainMOR : Aow.Units.HSEngine.TMultiHexMOR
    {
        internal sbyte FDecorationType;
        internal byte FTerrainType;
        // Terrain.pas:305
        public override void Destroy()
        {
            base.Destroy();
        }

        // Terrain.pas:310
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref FTerrainType, 17);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDecorationType), 18);
        }

        // Terrain.pas:299
        // { TTerrainMOR }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FDecorationType = unchecked((sbyte)-1);
        }

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
        public static new Aow.Units.Terrain.TTerrainMOR Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Terrain.TTerrainMOR o = new Aow.Units.Terrain.TTerrainMOR();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.HSEngine.TMultiHexMOR.Meta
        {
            // Terrain.pas:327
            public override int ClassID()
            {
                int Result = default;
                Result = 131431;
                return Result;
            }

            // Terrain.pas:317
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.HSEngine.HSEngine.TerrainStr;
                return Result;
            }

            // Terrain.pas:42
            public new Aow.Units.Terrain.TTerrainMOR Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Terrain.TTerrainMOR o = (Aow.Units.Terrain.TTerrainMOR)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Terrain.TTerrainMOR => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Terrain.TTerrainMO (instance size 20) }
    public partial class TTerrainMO : Aow.Units.HSEngine.TMultiHexMO
    {
        // Terrain.pas:60
        // { TTerrainMO }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
        }

        // Terrain.pas:179
        public override bool CanChangeTerrain(in Aow.Units.HSEngine.THSSenderInfo Info)
        {
            bool Result = default;
            if (this != Info.Sender)
            {
                var call__1 = FResource.TerrainList.GetTerrain(Info.HN);
                if (call__1 >= 0)
                {
                    var call__2 = FResource.TerrainList.GetTerrain(Info.HN);
                    var call__3 = GetMostFrequentMapFieldTerrainType((Aow.Units.HSEngine.TMapLevel)Owner.Owner, unchecked((sbyte)FXhx), unchecked((sbyte)FYhx), unchecked((sbyte)call__2));
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
        public override void ChangeTerrain(in Aow.Units.HSEngine.THSTerrainInfo Info)
        {
            sbyte TerrainType = default;
            sbyte NewTerrainType = default;
            Aow.Units.Terrain.TTerrainMOR R = default;
            Aow.Units.Engine.TECustomNode OldOwner = default;
            var call__5 = FResource.TerrainList.GetTerrain(Info.HN);
            TerrainType = unchecked((sbyte)call__5);
            if ((TerrainType >= 0) && (TerrainType != Info.TerrainType))
            {
                var call__6 = FResource.TerrainList.GetTerrain(Info.HN);
                var call__7 = GetMostFrequentMapFieldTerrainType((Aow.Units.HSEngine.TMapLevel)Owner.Owner, unchecked((sbyte)FXhx), unchecked((sbyte)FYhx), unchecked((sbyte)call__6));
                NewTerrainType = unchecked((sbyte)call__7);
                if (NewTerrainType != TerrainType)
                {
                    R = FindTerrainResource(unchecked((byte)NewTerrainType));
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
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
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
                    Msg.Result = ((Aow.Units.Terrain.TTerrainMOR)FResource).FDecorationType;
                }
            }
        }

        // Terrain.pas:291
        public override bool PlaceHX(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = base.PlaceHX(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)Lhx));
            if (Result && !GetEditMode())
            {
                OverrideTerrain();
            }

            return Result;
        }

        // Terrain.pas:226
        public virtual void ClearTerrain(Aow.Units.HSEngine.TMapField MapField, int HN)
        {
            int Xhx = default;
            int Yhx = default;
            int Lhx = default;
            int X = default;
            int Y = default;
            Aow.Units.HSEngine.TTerrainList TerrainList = default;
            Aow.Units.HSEngine.THSMap Map = default;
            Aow.Units.Terrain.TTerrainMOR R = default;
            Aow.Units.Engine.TEObjectList Resources = default;
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
                while (!Found && (I != 0))
                {
                    I -= 1;
                    R = (Aow.Units.Terrain.TTerrainMOR)Resources.List[I];
                    var call__12 = R.ClassType.ClassID();
                    var call__13 = FResource.ClassType.ClassID();
                    bool bool__14 = ((call__12 == call__13) && (R.FDecorationType == ((Aow.Units.Terrain.TTerrainMOR)FResource).FDecorationType)) && (R.FTerrainType == ((Aow.Units.Terrain.TTerrainMOR)FResource).FTerrainType);
                    if (bool__14)
                    {
                        var call__15 = R.TerrainList.UsedTerrain();
                        bool__14 = (call__15 == 1);
                    }

                    bool bool__16 = bool__14;
                    if (bool__16)
                    {
                        var call__17 = R.TerrainList.GetTerrain(0);
                        bool__16 = (call__17 >= 0);
                    }

                    bool bool__18 = bool__16;
                    if (bool__18)
                    {
                        var call__19 = GetOverlay(0);
                        var call__20 = R.TerrainList.GetOverlay(0);
                        bool__18 = (call__19 == call__20);
                    }

                    Found = bool__18;
                }

                if (Found)
                {
                    Xhx = GetXhx();
                    Yhx = GetYhx();
                    Lhx = GetLhx();
                    Map = ((Aow.Units.HSEngine.TMapLevel)Owner.Owner).Map;
                    TerrainList = FResource.TerrainList;
                    Destroy();
                    {
                        I = 0;
                        int end__21 = TerrainList.Count - 1;
                        for (; I <= end__21; I += 1)
                        {
                            if ((TerrainList.GetTerrain(I) >= 0) && (I != HN))
                            {
                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, I, ref X, ref Y);
                                R.PlaceResourceUser(Map, unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)Lhx));
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
        public virtual Aow.Units.Terrain.TTerrainMOR FindDecorationResource(byte DecorationType)
        {
            Aow.Units.Terrain.TTerrainMOR Result = default;
            Aow.Rtl.TList Candidates = default;
            Aow.Units.Engine.TEObjectList Resources = default;
            int I = default;
            Aow.Units.Terrain.TTerrainMOR R = default;
            Candidates = new Aow.Rtl.TList();
            var call__22 = GetResourceList(FResource.Engine);
            Resources = call__22.FChildren;
            {
                I = 0;
                int end__23 = Resources.Count - 1;
                for (; I <= end__23; I += 1)
                {
                    R = (Aow.Units.Terrain.TTerrainMOR)Resources.List[I];
                    var call__24 = R.ClassType.ClassID();
                    var call__25 = FResource.ClassType.ClassID();
                    bool bool__26 = (((call__24 == call__25) && (R.FTerrainType == ((Aow.Units.Terrain.TTerrainMOR)FResource).FTerrainType)) && (R.FDecorationType == DecorationType)) && (R != FResource);
                    if (bool__26)
                    {
                        var call__27 = R.TerrainList.CompareTerrain(FResource.TerrainList, TSet<Aow.Units.HSEngine.TTerrainCompareModeElement>.Of(Aow.Units.HSEngine.TTerrainCompareModeElement.cmSameOverlayType));
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
                var call__28 = Aow.Rtl.Rtl.Random(Candidates.Count);
                Result = (Aow.Units.Terrain.TTerrainMOR)Candidates.List[call__28];
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
            Aow.Units.HSEngine.TMapLevel Level = default;
            int Count = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Result = true;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            Count = GetTerrainCount();
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while ((Walk.HN < Count) && Result)
            {
                bool bool__29 = (Walk.HN != HN);
                if (bool__29)
                {
                    var call__30 = GetTerrain(Walk.HN);
                    bool__29 = (call__30 >= 0);
                }

                if ((((bool__29 && (Walk.X >= 0)) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__31 = Level.GetField(Walk.X, Walk.Y);
                    var call__32 = call__31.CanChangeTerrainEx(this);
                    Result = call__32;
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // Terrain.pas:19
        public static new Aow.Units.Terrain.TTerrainMO Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Terrain.TTerrainMO o = new Aow.Units.Terrain.TTerrainMO();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.HSEngine.TMapObject.Meta
        {
            // Terrain.pas:65
            public override int ClassID()
            {
                int Result = default;
                Result = 131430;
                return Result;
            }

            // Terrain.pas:19
            public new Aow.Units.Terrain.TTerrainMO Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Terrain.TTerrainMO o = (Aow.Units.Terrain.TTerrainMO)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Terrain.TTerrainMO => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class Terrain
    {
        // Terrain.pas:332
        public static void RegisterTerrain(Aow.Units.Engine.TEngine Engine)
        {
            Engine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Terrain.TTerrainMO>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Terrain.TTerrainMOR>()]);
        }
    }
}
