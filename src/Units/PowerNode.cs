// Translated from PowerNode.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Ai;
using Aow.Game.Combat;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// FState, FPowerSource
public partial class PowerNode : PlayerStructure
{
    public PowerNode(ECustomNode owner) : base(owner)
    {
        FPowerSource = new PlayerStructurePowerSource(null);
        FPowerSource.Setup(this);
    }

    // PowerNode.pas:64
    public new const int ClassID = 131482;

    public Set<StateElement> FState;
    public PlayerStructurePowerSource FPowerSource;

    // PowerNode.pas:58
    public override void Dispose()
    {
        base.Dispose();
        FPowerSource?.Dispose();
    }

    // PowerNode.pas:87
    public override void ReadWrite(EStorageStream Stream)
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
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.RegisterPowerSource(FPowerSource);
        }
    }

    // PowerNode.pas:138
    public override void Deactivate()
    {
        if (FPlayer != -1)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.UnregisterPowerSource(FPowerSource);
        }

        base.Deactivate();
    }

    // PowerNode.pas:76
    public override void SetRazed(bool Value)
    {
        if (Value)
        {
            SetPlayer(-1);
            FState = FState + Set<StateElement>.Of(StateElement.Razed);
        }
        else
        {
            FState = FState - Set<StateElement>.Of(StateElement.Razed);
        }

        FImageIndex = GetTerrainTypeImage(FTerrainType);
    }

    // PowerNode.pas:73
    public override bool GetRazed() => FState.Contains(StateElement.Razed);

    // PowerNode.pas:93
    public override string GetCurrentActivityText()
    {
        var power = GetPower();
        return RtlUnit.Format(AbilityRules.TranslateRStr(GeneratingXPowerRStr), [IntToStr(power)]);
    }

    // PowerNode.pas:96
    public override void UpdateAITarget(in AITargetInfo Info, ref AITargetData Data)
    {
        int P;
        switch (Info.Mode)
        {
            case AITargetMode.Attack:
                if (Info.Player != FPlayer && !GetRazed())
                {
                    P = Data.Priority;
                    if (FPlayer == 0)
                    {
                        P = 50;
                    }
                    else
                    {
                        if (AbilityRules.ValidAttackRelation(Info.Player, Info.IndependentRelation, FPlayer, 0))
                        {
                            P = 50 - AoWHSMap.Current.FPlayers.GetPlayers(Info.Player).FIncome / 2;
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
            if (FStatus.Contains(MapObjectStatusElement.Activated))
            {
                if (FPlayer != -1)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.UnregisterPowerSource(FPowerSource);
                }

                base.SetPlayer(Player);
                if (FPlayer != -1)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FMagicControl.RegisterPowerSource(FPowerSource);
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
        int Result;
        if (!GetRazed() && !GetBuilding())
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
    public override string GetCombatEventText(Combat.Battle Combat)
    {
        string Result;
        if (Combat.FCombatResult == CombatResult.Victory || Combat.FCombatResult == CombatResult.NoDefenders)
        {
            Result = AbilityRules.TranslateRStr(PowerNodeBattleWonRStr);
        }
        else
        {
            Result = "";
        }

        return Result;
    }

    private static readonly string GeneratingXPowerRStr = "Generating %s power";
    private static readonly string PowerNodeBattleWonRStr = "Power Node captured";

    public enum StateElement : byte
    {
        Razed = 0
    }

    public new partial class Resource : PlayerStructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
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
        public override int ResourceUserClassID() => 131482;

        // PowerNode.pas:41

        // PowerNode.pas:163
        public override string ResourceEditName() => AbilityRules.TranslateRStr(PowerNodeRStr);

        private static readonly string PowerNodeRStr = "Power Node";

    }

    // PowerNode.pas:25

    // PowerNode.pas:25

}
