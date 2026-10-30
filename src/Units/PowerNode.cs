// Translated from PowerNode.pas.
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

namespace Aow.Units.PowerNode
{
    public enum TPowerNodeStateElement : byte
    {
        pnsRazed = 0
    }

    // { TPowerNode: size $44; $3C FState, $40 FPowerSource. }
    public partial class TPowerNode : Aow.Units.AoWE.TPlayerStructure
    {
        public TPowerNode(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__1 = new Aow.Units.AoWE.TPlayerStructurePowerSource(null);
            FPowerSource = call__1;
            FPowerSource.Setup(this);
        }

        // PowerNode.pas:64
        public new const int ClassID = 131482;

        public TSet<Aow.Units.PowerNode.TPowerNodeStateElement> FState;
        public Aow.Units.FixedArray_byte_3 FReserved3D;
        public Aow.Units.AoWE.TPlayerStructurePowerSource FPowerSource;
        // PowerNode.pas:58
        public override void Destroy()
        {
            base.Destroy();
            FPowerSource.Free();
        }

        // PowerNode.pas:87
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Aow.Rtl.Rtl.RwSet(ref FState, 1, 20, Stream.rwData, 0);
        }

        // PowerNode.pas:51


        // PowerNode.pas:131
        public override void Activate()
        {
            base.Activate();
            if (FPlayer != -1)
            {
                var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                call__2.FMagicControl.RegisterPowerSource(FPowerSource);
            }
        }

        // PowerNode.pas:138
        public override void Deactivate()
        {
            if (FPlayer != -1)
            {
                var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                call__3.FMagicControl.UnregisterPowerSource(FPowerSource);
            }

            base.Deactivate();
        }

        // PowerNode.pas:76
        public override void SetRazed(bool Value)
        {
            if (Value)
            {
                SetPlayer(unchecked((sbyte)-1));
                FState = FState + TSet<Aow.Units.PowerNode.TPowerNodeStateElement>.Of(Aow.Units.PowerNode.TPowerNodeStateElement.pnsRazed);
            }
            else
            {
                FState = FState - TSet<Aow.Units.PowerNode.TPowerNodeStateElement>.Of(Aow.Units.PowerNode.TPowerNodeStateElement.pnsRazed);
            }

            var call__4 = GetTerrainTypeImage(FTerrainType);
            FImageIndex = call__4;
        }

        // PowerNode.pas:73
        public override bool GetRazed()
        {
            bool Result = default;
            Result = FState.Contains(Aow.Units.PowerNode.TPowerNodeStateElement.pnsRazed);
            return Result;
        }

        // PowerNode.pas:93
        public override Aow.Rtl.AnsiString GetCurrentActivityText()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__5 = GetPower();
            var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GeneratingXPowerRStr);
            var call__7 = Aow.Rtl.Rtl.Format(call__6, [Aow.Rtl.Rtl.IntToStr(call__5)]);
            Result = call__7;
            return Result;
        }

        // PowerNode.pas:96
        public override void UpdateAITarget(in Aow.Units.AoWE.TAITargetInfo Info, ref Aow.Units.AoWE.TAITargetData Data)
        {
            int P = default;
            switch (Info.Mode)
            {
                case Aow.Units.AoWE.TAITargetMode.tmAttack:
                    bool bool__8 = (Info.Player != FPlayer);
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
                            var call__10 = Aow.Units.AoWE.AoWE.ValidAttackRelation(unchecked((sbyte)Info.Player), unchecked((sbyte)Info.IndependentRelation), unchecked((sbyte)FPlayer), 0);
                            if (call__10)
                            {
                                var call__11 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Info.Player);
                                P = 50 - (call__11.FIncome / 2);
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
                if (FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moActivated))
                {
                    if (FPlayer != -1)
                    {
                        var call__12 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                        call__12.FMagicControl.UnregisterPowerSource(FPowerSource);
                    }

                    base.SetPlayer(unchecked((sbyte)Player));
                    if (FPlayer != -1)
                    {
                        var call__13 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                        call__13.FMagicControl.RegisterPowerSource(FPowerSource);
                    }
                }
                else
                {
                    base.SetPlayer(unchecked((sbyte)Player));
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
        public override Aow.Rtl.AnsiString GetCombatEventText(Aow.Units.AoWE.TCombat Combat)
        {
            Aow.Rtl.AnsiString Result = default;
            if ((Combat.FCombatResult == Aow.Units.AoWE.TCombatResult.crVictory) || (Combat.FCombatResult == Aow.Units.AoWE.TCombatResult.crNoDefenders))
            {
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PowerNodeBattleWonRStr);
            }
            else
            {
                Result = (AnsiString)"";
            }

            return Result;
        }

        // PowerNode.pas:25


    }

    // { TPowerNodeResource (instance size 116, VMT 557D027C) }
    public partial class TPowerNodeResource : Aow.Units.AoWE.TPlayerStructureResource
    {
        public TPowerNodeResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
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
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PowerNodeRStr);
            return Result;
        }

    }

    public static partial class PowerNode
    {
        // PowerNode.pas:172
        public static void RegisterPowerNode(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.PowerNode.TPowerNode>(), Aow.Units.Engine.EClass.Of<Aow.Units.PowerNode.TPowerNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
