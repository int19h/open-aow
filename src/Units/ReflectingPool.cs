// Translated from ReflectingPool.pas.
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
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.ReflectingPool
{
    // { TReflectingPool: size $40; $3C FCenterFrame (-1 when inactive). }
    public partial class TReflectingPool : Aow.Units.AoWE.TPlayerStructure
    {
        public int FCenterFrame;
        // ReflectingPool.pas:80
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
        }

        // ReflectingPool.pas:52
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FCenterFrame = -1;
        }

        // ReflectingPool.pas:241
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            bool bool__1 = (Msg.MsgID == 4362);
            if (bool__1)
            {
                var call__2 = FResource.TerrainList.GetTerrain(Msg.HN);
                bool__1 = (call__2 >= 0);
            }

            if (bool__1)
            {
                var call__3 = VisibilityRange();
                bool bool__4 = (call__3 >= Msg.Param[0].Value);
                if (bool__4)
                {
                    var call__5 = PoolVisionPlayer(unchecked((sbyte)Msg.Param[1].Value));
                    bool__4 = call__5;
                }

                if (bool__4)
                {
                    Msg.Result = 1;
                }
            }
            else
            {
                bool bool__6 = (Msg.MsgID == 4365);
                if (bool__6)
                {
                    var call__7 = FResource.TerrainList.GetTerrain(Msg.HN);
                    bool__6 = (call__7 >= 0);
                }

                if (bool__6)
                {
                    var call__8 = VisibilityRange();
                    if (call__8 >= Msg.Param[0].Value)
                    {
                        NotifyPlayerChanged(Msg.MapField, Msg.Param[0].Value);
                    }
                }
                else
                {
                    base.MapFieldMsgProc(ref Msg);
                }
            }
        }

        // ReflectingPool.pas:83
        public override void Activate()
        {
            base.Activate();
            if (FPlayer != -1)
            {
                var call__9 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                call__9.FKnownStructures.AddChild(this);
            }
        }

        // ReflectingPool.pas:89
        public override void Deactivate()
        {
            base.Deactivate();
            DeactivatePoolCentering();
            if (FPlayer != -1)
            {
                var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                call__10.FKnownStructures.RemoveChild(this);
            }
        }

        // ReflectingPool.pas:143
        public override void SetPlayer(sbyte Player)
        {
            if (Player != FPlayer)
            {
                if (FPlayer != -1)
                {
                    var call__11 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__11.FKnownStructures.RemoveChild(this);
                }

                base.SetPlayer(unchecked((sbyte)Player));
                if (FPlayer != -1)
                {
                    var call__12 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__12.FKnownStructures.AddChild(this);
                }

                UpdateReflectingPoolsExploration();
                bool bool__13 = (FPlayer != -1) && (FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (bool__13)
                {
                    var call__14 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    var call__15 = call__14.FKnownStructures.Count;
                    bool__13 = (call__15 == 1);
                }

                if (bool__13)
                {
                    ActivatePoolCentering();
                }
            }
        }

        // ReflectingPool.pas:69
        public override int VisibilityRange()
        {
            int Result = default;
            Result = 8;
            var call__16 = GetLhx();
            bool bool__17 = (call__16 != 0);
            if (!bool__17)
            {
                bool bool__18 = (FPlayer > 0);
                if (bool__18)
                {
                    var call__19 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    bool__18 = ((((byte)call__19.FMagicControl.FEnemyEnchantments.Image) & 8) != 0);
                }

                bool__17 = bool__18;
            }

            if (bool__17)
            {
                Result = (int)(((uint)(Result + 1)) >> 1);
            }

            return Result;
        }

        // ReflectingPool.pas:185
        public override void UpdateUnfog()
        {
            int Range = default;
            int I = default;
            int X = default;
            int Y = default;
            if (FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moPlacedOnMap))
            {
                if (FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moActivated) && (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer != -1))
                {
                    var call__20 = PoolVisionPlayer(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    if (call__20)
                    {
                        Range = VisibilityRange();
                    }
                    else
                    {
                        Range = 0;
                    }
                }
                else
                {
                    Range = 0;
                }

                if (Range != FUnfogRange)
                {
                    if (FUnfogRange != 0)
                    {
                        {
                            I = 0;
                            int end__21 = FResource.TerrainList.Count - 1;
                            for (; I <= end__21; I += 1)
                            {
                                var call__22 = FResource.TerrainList.GetTerrain(I);
                                if (call__22 >= 0)
                                {
                                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FXhx, FYhx, I, ref X, ref Y);
                                    Aow.Units.AoWE.AoWE.AoWHSMap.FogArea(X, Y, ((Aow.Units.HSEngine.TMapField)Owner).Lhx, FUnfogRange);
                                }
                            }
                        }
                    }

                    FUnfogRange = unchecked((byte)Range);
                    if (FUnfogRange != 0)
                    {
                        {
                            I = 0;
                            int end__23 = FResource.TerrainList.Count - 1;
                            for (; I <= end__23; I += 1)
                            {
                                var call__24 = FResource.TerrainList.GetTerrain(I);
                                if (call__24 >= 0)
                                {
                                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FXhx, FYhx, I, ref X, ref Y);
                                    Aow.Units.AoWE.AoWE.AoWHSMap.UnFogArea(X, Y, ((Aow.Units.HSEngine.TMapField)Owner).Lhx, FUnfogRange);
                                }
                            }
                        }
                    }
                }
            }
        }

        // ReflectingPool.pas:157
        public override void UpdateExploration()
        {
            int P = default;
            int I = default;
            int X = default;
            int Y = default;
            if (FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moPlacedOnMap))
            {
                {
                    P = 1;
                    int end__25 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                    for (; P <= end__25; P += 1)
                    {
                        if (PoolVisionPlayer(unchecked((sbyte)P)))
                        {
                            {
                                I = 0;
                                int end__26 = FResource.TerrainList.Count - 1;
                                for (; I <= end__26; I += 1)
                                {
                                    var call__27 = FResource.TerrainList.GetTerrain(I);
                                    if (call__27 >= 0)
                                    {
                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FXhx, FYhx, I, ref X, ref Y);
                                        var call__28 = VisibilityRange();
                                        Aow.Units.AoWE.AoWE.AoWHSMap.ExploreArea(X, Y, ((Aow.Units.HSEngine.TMapField)Owner).Lhx, unchecked((sbyte)P), call__28);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // ReflectingPool.pas:77
        public override int GetDefaultAITargetPriority()
        {
            int Result = default;
            Result = 1;
            return Result;
        }

        // ReflectingPool.pas:61
        public void ListReflectingPools(ref Aow.Units.AoWE.TPlayerStructureList List)
        {
            int P = default;
            int I = default;
            {
                P = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                byte end__29 = 0;
                for (; P >= end__29; P -= 1)
                {
                    {
                        I = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(P).FKnownStructures.Count - 1;
                        byte end__30 = 0;
                        for (; I >= end__30; I -= 1)
                        {
                            var call__31 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(P);
                            var call__32 = call__31.FKnownStructures.GetStructure(I);
                            List.AddChild(call__32);
                        }
                    }
                }
            }
        }

        // ReflectingPool.pas:99
        public void NewFrame(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TPlayerStructureList List = default;
            Aow.Units.AoWE.TPlayerStructure Pool = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == FPlayer)
            {
                FCenterFrame += 1;
                if ((FCenterFrame % 8) == 0)
                {
                    List = new Aow.Units.AoWE.TPlayerStructureList(null);
                    ListReflectingPools(ref List);
                    List.RemoveChild(this);
                    List.Sort(Aow.Units.ReflectingPool.ReflectingPool.ComparePools);
                    var call__33 = List.Count;
                    if ((FCenterFrame / 8) < call__33)
                    {
                        var call__34 = List.GetStructure(FCenterFrame / 8);
                        Pool = call__34;
                        var call__35 = Pool.GetLhx();
                        var call__36 = Pool.GetYhx();
                        var call__37 = Pool.GetXhx();
                        Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(call__37, call__36, call__35);
                    }
                    else
                    {
                        var call__38 = GetLhx();
                        var call__39 = GetYhx();
                        var call__40 = GetXhx();
                        Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(call__40, call__39, call__38);
                        DeactivatePoolCentering();
                    }

                    List.Free();
                }
            }
            else
            {
                DeactivatePoolCentering();
            }
        }

        // ReflectingPool.pas:127
        public void ActivatePoolCentering()
        {
            DeactivatePoolCentering();
            FCenterFrame = 0;
            Aow.Units.AoWE.AoWE.AoWHSMap.NewFrameEvents.Add(NewFrame);
        }

        // ReflectingPool.pas:134
        public void DeactivatePoolCentering()
        {
            if (FCenterFrame != -1)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.NewFrameEvents.Remove(NewFrame);
                FCenterFrame = -1;
            }
        }

        // ReflectingPool.pas:171
        public void UpdateReflectingPoolsExploration()
        {
            int P = default;
            int I = default;
            Aow.Units.AoWE.TPlayer Player = default;
            {
                P = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                byte end__41 = 0;
                for (; P >= end__41; P -= 1)
                {
                    var call__42 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(P);
                    Player = call__42;
                    {
                        I = Player.FKnownStructures.Count - 1;
                        byte end__43 = 0;
                        for (; I >= end__43; I -= 1)
                        {
                            var call__44 = Player.FKnownStructures.GetStructure(I);
                            call__44.UpdateExploration();
                            var call__45 = Player.FKnownStructures.GetStructure(I);
                            call__45.UpdateUnfog();
                        }
                    }
                }
            }
        }

        // ReflectingPool.pas:217
        public bool PoolVisionPlayer(sbyte Player)
        {
            bool Result = default;
            int P = default;
            var call__46 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__47 = call__46.FKnownStructures.Count;
            Result = call__47 != 0;
            if (!Result)
            {
                var call__48 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count;
                P = call__48;
                while ((P > 1) && !Result)
                {
                    P -= 1;
                    var call__49 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(P);
                    var call__50 = call__49.FKnownStructures.Count;
                    bool bool__51 = (call__50 != 0);
                    if (bool__51)
                    {
                        var call__52 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(P);
                        var call__53 = call__52.FDiplomaticRelations.GetDiplomaticRelation(unchecked((sbyte)Player));
                        bool__51 = (call__53 == Aow.Units.AoWE.TDiplomaticRelation.drAlliance);
                    }

                    Result = bool__51;
                }
            }

            return Result;
        }

        // ReflectingPool.pas:233
        public void NotifyPlayerChanged(Aow.Units.HSEngine.TMapField MapField, int Distance)
        {
            int P = default;
            {
                P = 1;
                int end__54 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                for (; P <= end__54; P += 1)
                {
                    if (PoolVisionPlayer(unchecked((sbyte)P)))
                    {
                        var call__55 = TrueVisionRange();
                        Aow.Units.AoWE.AoWE.AoWHSMap.UpdatePlayerSeen(P, MapField.Xhx, MapField.Yhx, MapField.Lhx, Distance, call__55);
                    }
                }
            }
        }

        // ReflectingPool.pas:20
        public static new Aow.Units.ReflectingPool.TReflectingPool Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.ReflectingPool.TReflectingPool o = new Aow.Units.ReflectingPool.TReflectingPool();
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

        public new partial class Meta : Aow.Units.AoWE.TPlayerStructure.Meta
        {
            // ReflectingPool.pas:58
            public override int ClassID()
            {
                int Result = default;
                Result = 132670;
                return Result;
            }

            // ReflectingPool.pas:20
            public new Aow.Units.ReflectingPool.TReflectingPool Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.ReflectingPool.TReflectingPool o = (Aow.Units.ReflectingPool.TReflectingPool)NewInstance();
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
                where T : Aow.Units.ReflectingPool.TReflectingPool => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TReflectingPoolResource (instance size 116, VMT 557D4B6C) }
    public partial class TReflectingPoolResource : Aow.Units.AoWE.TPlayerStructureResource
    {
        // ReflectingPool.pas:254
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FTerrainBound = true;
            FConnectRoads = false;
            FClaimed = false;
            FRazeable = false;
        }

        // ReflectingPool.pas:269
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132670;
            return Result;
        }

        // ReflectingPool.pas:42
        public static new Aow.Units.ReflectingPool.TReflectingPoolResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.ReflectingPool.TReflectingPoolResource o = new Aow.Units.ReflectingPool.TReflectingPoolResource();
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

        public new partial class Meta : Aow.Units.AoWE.TPlayerStructureResource.Meta
        {
            // ReflectingPool.pas:266
            public override int ClassID()
            {
                int Result = default;
                Result = 132671;
                return Result;
            }

            // ReflectingPool.pas:263
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ReflectingPoolRStr);
                return Result;
            }

            // ReflectingPool.pas:42
            public new Aow.Units.ReflectingPool.TReflectingPoolResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.ReflectingPool.TReflectingPoolResource o = (Aow.Units.ReflectingPool.TReflectingPoolResource)NewInstance();
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
                where T : Aow.Units.ReflectingPool.TReflectingPoolResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class ReflectingPool
    {
        // ReflectingPool.pas:272
        public static void RegisterReflectingPool(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.ReflectingPool.TReflectingPool>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.ReflectingPool.TReflectingPoolResource>()]);
        }

        // ReflectingPool.pas:96
        public static int ComparePools(object Item1, object Item2)
        {
            int Result = default;
            Result = (((Aow.Rtl.TObject)Item1)?.InstanceId ?? 0) - (((Aow.Rtl.TObject)Item2)?.InstanceId ?? 0);
            return Result;
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
