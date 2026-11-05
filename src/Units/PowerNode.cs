// Translated from PowerNode.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.PowerNode
{
    public enum TPowerNodeStateElement : byte
    {
        pnsRazed = 0
    }

    // FState, FPowerSource
    public partial class TPowerNode : TPlayerStructure
    {
        public TPowerNode(TECustomNode owner) : base(owner)
        {
            var call__1 = new TPlayerStructurePowerSource(null);
            FPowerSource = call__1;
            FPowerSource.Setup(this);
        }

        // PowerNode.pas:64
        public new const int ClassID = 131482;

        public TSet<TPowerNodeStateElement> FState;
        public TPlayerStructurePowerSource FPowerSource;
        // PowerNode.pas:58
        public override void Destroy()
        {
            base.Destroy();
            FPowerSource.Free();
        }

        // PowerNode.pas:87
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            RwSet(ref FState, 1, 20, Stream.rwData, 0);
        }

        // PowerNode.pas:51


        // PowerNode.pas:131
        public override void Activate()
        {
            base.Activate();
            if (FPlayer != -1)
            {
                var call__2 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                call__2.FMagicControl.RegisterPowerSource(FPowerSource);
            }
        }

        // PowerNode.pas:138
        public override void Deactivate()
        {
            if (FPlayer != -1)
            {
                var call__3 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                call__3.FMagicControl.UnregisterPowerSource(FPowerSource);
            }

            base.Deactivate();
        }

        // PowerNode.pas:76
        public override void SetRazed(bool Value)
        {
            if (Value)
            {
                SetPlayer(-1);
                FState = FState + TSet<TPowerNodeStateElement>.Of(TPowerNodeStateElement.pnsRazed);
            }
            else
            {
                FState = FState - TSet<TPowerNodeStateElement>.Of(TPowerNodeStateElement.pnsRazed);
            }

            var call__4 = GetTerrainTypeImage(FTerrainType);
            FImageIndex = call__4;
        }

        // PowerNode.pas:73
        public override bool GetRazed()
        {
            bool Result = default;
            Result = FState.Contains(TPowerNodeStateElement.pnsRazed);
            return Result;
        }

        // PowerNode.pas:93
        public override AnsiString GetCurrentActivityText()
        {
            AnsiString Result = default;
            var call__5 = GetPower();
            var call__6 = TranslateRStr(GeneratingXPowerRStr);
            var call__7 = Rtl.Rtl.Format(call__6, [IntToStr(call__5)]);
            Result = call__7;
            return Result;
        }

        // PowerNode.pas:96
        public override void UpdateAITarget(in TAITargetInfo Info, ref TAITargetData Data)
        {
            int P = default;
            switch (Info.Mode)
            {
                case TAITargetMode.tmAttack:
                    bool bool__8 = Info.Player != FPlayer;
                    if (bool__8)
                    {
                        var call__9 = GetRazed();
                        bool__8 = !call__9;
                    }

                    if (bool__8)
                    {
                        P = Data.Priority;
                        if (FPlayer == 0)
                        {
                            P = 50;
                        }
                        else
                        {
                            var call__10 = ValidAttackRelation(Info.Player, Info.IndependentRelation, FPlayer, 0);
                            if (call__10)
                            {
                                var call__11 = AoWHSMap.FPlayers.GetPlayers(Info.Player);
                                P = 50 - call__11.FIncome / 2;
                                if (P < 25)
                                {
                                    P = 25;
                                }

                                if (P > 75)
                                {
                                    P = 75;
                                }
                            }
                        }

                        if (Data.Priority < P)
                        {
                            Data.Priority = P;
                        }
                    }

                    break;
            }
        }

        // PowerNode.pas:117
        public override void SetPlayer(sbyte Player)
        {
            if (Player != FPlayer)
            {
                if (FStatus.Contains(TMapObjectStatusElement.moActivated))
                {
                    if (FPlayer != -1)
                    {
                        var call__12 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                        call__12.FMagicControl.UnregisterPowerSource(FPowerSource);
                    }

                    base.SetPlayer(Player);
                    if (FPlayer != -1)
                    {
                        var call__13 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                        call__13.FMagicControl.RegisterPowerSource(FPowerSource);
                    }
                }
                else
                {
                    base.SetPlayer(Player);
                }
            }
        }

        // PowerNode.pas:67
        public override int GetPower()
        {
            int Result = default;
            var call__14 = GetRazed();
            bool bool__15 = !call__14;
            if (bool__15)
            {
                var call__16 = GetBuilding();
                bool__15 = !call__16;
            }

            if (bool__15)
            {
                Result = 10;
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // PowerNode.pas:145
        public override AnsiString GetCombatEventText(TCombat Combat)
        {
            AnsiString Result = default;
            if (Combat.FCombatResult == TCombatResult.crVictory || Combat.FCombatResult == TCombatResult.crNoDefenders)
            {
                Result = TranslateRStr(PowerNodeBattleWonRStr);
            }
            else
            {
                Result = (AnsiString)"";
            }

            return Result;
        }

        // PowerNode.pas:25


    }

    public partial class TPowerNodeResource : TPlayerStructureResource
    {
        public TPowerNodeResource(TECustomNode owner) : base(owner)
        {
            FTerrainBound = false;
            FClaimed = false;
            FRazeDefenderBudget = 200;
            FRazeable = true;
            FRebuildGold = 50;
            FRebuildDays = 2;
        }

        // PowerNode.pas:166
        public new const int ClassID = 131483;

        // PowerNode.pas:152


        // PowerNode.pas:169
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131482;
            return Result;
        }

        // PowerNode.pas:41


        // PowerNode.pas:163
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(PowerNodeRStr);
            return Result;
        }

    }

    public static partial class PowerNode
    {
        // PowerNode.pas:172
        public static void RegisterPowerNode(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TPowerNode>(), EClass.Of<TPowerNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
