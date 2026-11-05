// Translated from Tower.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Tower
{
    public enum TTowerStateElement : byte
    {
        tsRazed = 0
    }

    // FState, bit 0 razed
    public partial class TTower : TPlayerStructure
    {
        public TTower(TECustomNode owner) : base(owner)
        {}

        // Tower.pas:55
        public new const int ClassID = 132158;

        public TSet<TTowerStateElement> FState;
        // Tower.pas:96
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            RwSet(ref FState, 1, 20, Stream.rwData, 0);
        }

        // Tower.pas:102
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            var call__1 = FResource.TerrainList.Terrain[Msg.HN];
            bool bool__2 = call__1 >= 0 && Msg.MsgID == 4387;
            if (bool__2)
            {
                var call__3 = Msg.Param[0].As<TMapField>().FindNoneTransparentHS(typeof(TTower));
                bool__2 = call__3 == this;
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
                SetPlayer(-1);
                FState = FState + TSet<TTowerStateElement>.Of(TTowerStateElement.tsRazed);
            }
            else
            {
                FState = FState - TSet<TTowerStateElement>.Of(TTowerStateElement.tsRazed);
            }

            var call__8 = GetTerrainTypeImage(FTerrainType);
            FImageIndex = call__8;
        }

        // Tower.pas:82
        public override bool GetRazed()
        {
            bool Result = default;
            Result = FState.Contains(TTowerStateElement.tsRazed);
            return Result;
        }

        // Tower.pas:73
        public override void UpdateAITarget(in TAITargetInfo Info, ref TAITargetData Data)
        {
            base.UpdateAITarget(Info, ref Data);
            if (!FState.Contains(TTowerStateElement.tsRazed))
            {
                switch (Info.Mode)
                {
                    case TAITargetMode.tmFortify:
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
                bool bool__10 = call__9 != 0;
                if (!bool__10)
                {
                    bool bool__11 = FPlayer > 0;
                    if (bool__11)
                    {
                        var call__12 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                        bool__11 = (((byte)call__12.FMagicControl.FEnemyEnchantments.Image) & 8) != 0;
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

    }

    public partial class TTowerResource : TPlayerStructureResource
    {
        public TTowerResource(TECustomNode owner) : base(owner)
        {
            FTerrainBound = true;
            FClaimed = false;
            FRazeable = true;
            FRazeDefenderBudget = 50;
            FRebuildGold = 25;
            FRebuildDays = 2;
        }

        // Tower.pas:124
        public new const int ClassID = 132159;

        // Tower.pas:110


        // Tower.pas:127
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132158;
            return Result;
        }

        // Tower.pas:35


        // Tower.pas:121
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            var call__13 = TowerRStr;
            var call__14 = TranslateRStr(call__13);
            Result = call__14;
            return Result;
        }

    }

    public partial class TTowerConstructionControl : TStructureConstructionControl
    {
        public TTowerConstructionControl(TECustomNode owner) : base(owner)
        { }

        // Tower.pas:139
        public override void GetInfo(TAbstractUnit AUnit, ref AnsiString Text, ref int Gold, ref int Days)
        {
            var call__15 = TowerRStr;
            var call__16 = TranslateRStr(call__15);
            var call__17 = BuildXRStr;
            var call__18 = TranslateRStr(call__17);
            var call__19 = Rtl.Rtl.Format(call__18, [call__16]);
            Text = call__19;
            Gold = 50;
            Days = 2;
        }

        // Tower.pas:146
        public override bool CanConstruct(TAbstractUnit A1, ref AnsiString A2)
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
                var call__20 = TowerRStr;
                var call__21 = TranslateRStr(call__20);
                var call__22 = CannotBuildXInUndergroundRStr;
                var call__23 = TranslateRStr(call__22);
                var call__24 = Rtl.Rtl.Format(call__23, [call__21]);
                A2 = call__24;
            }

            return Result;
        }

        // Tower.pas:133
        public override TStructureResource StructureResource(TAbstractUnit AUnit)
        {
            TStructureResource Result = default;
            var call__25 = AoWHSSet.MapObjectResourceList.FindChild(132159);
            Result = (TStructureResource)call__25;
            return Result;
        }

        // Tower.pas:130
        public override int ID()
        {
            int Result = default;
            Result = 3;
            return Result;
        }

    }

    public static partial class Tower
    {
        // Tower.pas:159
        public static void RegisterTower(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TTower>(), EClass.Of<TTowerResource>()]);
            var call__26 = new TTowerConstructionControl(null);
            var call__27 = AoWHSSet.FAbilityControl.GetAbility(125);
            ((TConstructAbility)call__27).RegisterConstructionControl(call__26);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
