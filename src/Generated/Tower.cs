// Generated from Tower.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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

namespace Aow.Units.Tower
{
    public enum TTowerStateElement : byte
    {
        tsRazed = 0
    }

    // { TTower (instance size 64, VMT 557C31A8): $3C FState, bit 0 razed. }
    public partial class TTower : Aow.Units.AoWE.TPlayerStructure
    {
        public TSet<Aow.Units.Tower.TTowerStateElement> FState;
        public Aow.Units.FixedArray_byte_3 F_Tower_3D;
        // Tower.pas:96
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Aow.Rtl.Rtl.RwSet(ref FState, 1, 20, Stream.rwData, 0);
        }

        // Tower.pas:102
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            var call__1 = FResource.TerrainList.Terrain[Msg.HN];
            bool bool__2 = (call__1 >= 0) && (Msg.MsgID == 4387);
            if (bool__2)
            {
                var call__3 = Msg.Param[0].As<Aow.Units.HSEngine.TMapField>().FindNoneTransparentHS(typeof(Aow.Units.Tower.TTower));
                bool__2 = (call__3 == this);
            }

            bool bool__4 = bool__2;
            if (bool__4)
            {
                var call__5 = GetBuilding();
                bool__4 = !call__5;
            }

            bool bool__6 = bool__4;
            if (bool__6)
            {
                var call__7 = GetRazed();
                bool__6 = !call__7;
            }

            if (bool__6)
            {
                Msg.Result = 2;
            }
        }

        // Tower.pas:85
        public override void SetRazed(bool Value)
        {
            if (Value)
            {
                SetPlayer(unchecked((sbyte)-1));
                FState = FState + TSet<Aow.Units.Tower.TTowerStateElement>.Of(Aow.Units.Tower.TTowerStateElement.tsRazed);
            }
            else
            {
                FState = FState - TSet<Aow.Units.Tower.TTowerStateElement>.Of(Aow.Units.Tower.TTowerStateElement.tsRazed);
            }

            var call__8 = GetTerrainTypeImage(FTerrainType);
            FImageIndex = call__8;
        }

        // Tower.pas:82
        public override bool GetRazed()
        {
            bool Result = default;
            Result = FState.Contains(Aow.Units.Tower.TTowerStateElement.tsRazed);
            return Result;
        }

        // Tower.pas:73
        public override void UpdateAITarget(in Aow.Units.AoWE.TAITargetInfo Info, ref Aow.Units.AoWE.TAITargetData Data)
        {
            base.UpdateAITarget(Info, ref Data);
            if (!FState.Contains(Aow.Units.Tower.TTowerStateElement.tsRazed))
            {
                switch (Info.Mode)
                {
                    case Aow.Units.AoWE.TAITargetMode.tmFortify:
                        if (Info.Player == FPlayer)
                        {
                            Data.Priority = 50;
                        }

                        break;
                }
            }
        }

        // Tower.pas:58
        public override int VisibilityRange()
        {
            int Result = default;
            if (!GetBuilding())
            {
                Result = 12;
                var call__9 = GetLhx();
                bool bool__10 = (call__9 != 0);
                if (!bool__10)
                {
                    bool bool__11 = (FPlayer > 0);
                    if (bool__11)
                    {
                        var call__12 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                        bool__11 = ((((byte)call__12.FMagicControl.FEnemyEnchantments.Image) & 8) != 0);
                    }

                    bool__10 = bool__11;
                }

                if (bool__10)
                {
                    Result = (int)(((uint)(Result + 1)) >> 1);
                }
            }
            else
            {
                Result = 4;
            }

            return Result;
        }

        // Tower.pas:70
        public override int GetDefaultAITargetPriority()
        {
            int Result = default;
            Result = 1;
            return Result;
        }

        public new partial class Meta : Aow.Units.AoWE.TPlayerStructure.Meta
        {
            // Tower.pas:55
            public override int ClassID()
            {
                int Result = default;
                Result = 132158;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Tower.TTower => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P1_09.inc:249
            public new Aow.Units.Tower.TTower Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Tower.TTower o = (Aow.Units.Tower.TTower)NewInstance();
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
        }
    }

    // { TTowerResource (instance size 116, VMT 557C3434): no additional fields. }
    public partial class TTowerResource : Aow.Units.AoWE.TPlayerStructureResource
    {
        // Tower.pas:110
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FTerrainBound = true;
            FClaimed = false;
            FRazeable = true;
            FRazeDefenderBudget = 50;
            FRebuildGold = 25;
            FRebuildDays = 2;
        }

        // Tower.pas:127
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132158;
            return Result;
        }

        // Tower.pas:35
        public static new Aow.Units.Tower.TTowerResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Tower.TTowerResource o = new Aow.Units.Tower.TTowerResource();
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
            // Tower.pas:124
            public override int ClassID()
            {
                int Result = default;
                Result = 132159;
                return Result;
            }

            // Tower.pas:121
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                var call__13 = Aow.Units.AoWE.AoWE.TowerRStr;
                var call__14 = Aow.Units.AoWE.AoWE.TranslateRStr(call__13);
                Result = call__14;
                return Result;
            }

            // Tower.pas:35
            public new Aow.Units.Tower.TTowerResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Tower.TTowerResource o = (Aow.Units.Tower.TTowerResource)NewInstance();
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
                where T : Aow.Units.Tower.TTowerResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TTowerConstructionControl (instance size 8, VMT 557C352C) }
    public partial class TTowerConstructionControl : Aow.Units.AoWE.TStructureConstructionControl
    {
        // Tower.pas:139
        public override void GetInfo(Aow.Units.AoWE.TAbstractUnit AUnit, ref Aow.Rtl.AnsiString Text, ref int Gold, ref int Days)
        {
            var call__15 = Aow.Units.AoWE.AoWE.TowerRStr;
            var call__16 = Aow.Units.AoWE.AoWE.TranslateRStr(call__15);
            var call__17 = Aow.Units.AoWE.AoWE.BuildXRStr;
            var call__18 = Aow.Units.AoWE.AoWE.TranslateRStr(call__17);
            var call__19 = Aow.Rtl.Rtl.Format(call__18, [call__16]);
            Text = call__19;
            Gold = 50;
            Days = 2;
        }

        // Tower.pas:146
        public override bool CanConstruct(Aow.Units.AoWE.TAbstractUnit A1, ref Aow.Rtl.AnsiString A2)
        {
            bool Result = default;
            sbyte Xhx = default;
            sbyte Yhx = default;
            sbyte Lhx = default;
            A1.GetLocation(ref Xhx, ref Yhx, ref Lhx);
            if (Lhx == 0)
            {
                Result = base.CanConstruct(A1, ref A2);
            }
            else
            {
                Result = false;
                var call__20 = Aow.Units.AoWE.AoWE.TowerRStr;
                var call__21 = Aow.Units.AoWE.AoWE.TranslateRStr(call__20);
                var call__22 = Aow.Units.AoWE.AoWE.CannotBuildXInUndergroundRStr;
                var call__23 = Aow.Units.AoWE.AoWE.TranslateRStr(call__22);
                var call__24 = Aow.Rtl.Rtl.Format(call__23, [call__21]);
                A2 = call__24;
            }

            return Result;
        }

        // Tower.pas:133
        public override Aow.Units.AoWE.TStructureResource StructureResource(Aow.Units.AoWE.TAbstractUnit AUnit)
        {
            Aow.Units.AoWE.TStructureResource Result = default;
            // { conflict: function TTowerConstructionControl.StructureResource: TStructureResource; }
            var call__25 = Aow.Units.AoWE.AoWE.AoWHSSet.MapObjectResourceList.FindChild(132159);
            Result = (Aow.Units.AoWE.TStructureResource)call__25;
            return Result;
        }

        // Engine.pas:524
        public static new Aow.Units.Tower.TTowerConstructionControl Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Tower.TTowerConstructionControl o = new Aow.Units.Tower.TTowerConstructionControl();
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

        public new partial class Meta : Aow.Units.AoWE.TConstructionControl.Meta
        {
            // Tower.pas:130
            public override int ID()
            {
                int Result = default;
                Result = 3;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Tower.TTowerConstructionControl => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.Tower.TTowerConstructionControl Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Tower.TTowerConstructionControl o = (Aow.Units.Tower.TTowerConstructionControl)NewInstance();
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
        }
    }

    public static partial class Tower
    {
        // Tower.pas:159
        // { conflict: function StructureResource: TStructureResource; override; }
        public static void RegisterTower(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Tower.TTower>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Tower.TTowerResource>()]);
            var call__26 = Aow.Units.Tower.TTowerConstructionControl.Create(null);
            var call__27 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(125);
            ((Aow.Units.AoWE.TConstructAbility)call__27).RegisterConstructionControl(call__26);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
