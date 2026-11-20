#nullable disable
// Translated from PowerNode.pas.
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.DelphiFormat;

namespace Aow.Game.World.Structures;

// State, PowerSource
public partial class PowerNode : PlayerStructure
{
    public PowerNode(CustomNode owner) : base(owner)
    {
        _powerSource = new PlayerStructurePowerSource(null) { Holder = CreationHolder() };
        _powerSource.Setup(this);
    }

    // PowerNode.pas:64
    public new const int ClassId = 131_482;

    public Set<StateElement> State { get; set; }

    private readonly PlayerStructurePowerSource _powerSource;

    // PowerNode.pas:58
    public override void Dispose()
    {
        base.Dispose();
        _powerSource?.Dispose();
    }

    // PowerNode.pas:87
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        State = persister.Set("State", value: State);
    }

    // PowerNode.pas:131
    public override void Activate()
    {
        base.Activate();
        if (Player != -1)
        {
            _ = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(_powerSource);
        }
    }

    // PowerNode.pas:138
    public override void Deactivate()
    {
        if (Player != -1)
        {
            _ = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(_powerSource);
        }

        base.Deactivate();
    }

    // PowerNode.pas:76
    public override void SetRazed(bool value)
    {
        if (value)
        {
            SetPlayer(-1);
            State += Set<StateElement>.Of(StateElement.Razed);
        }
        else
        {
            State -= Set<StateElement>.Of(StateElement.Razed);
        }

        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // PowerNode.pas:73
    public override bool GetRazed() => State.Contains(StateElement.Razed);

    // PowerNode.pas:93
    public override string GetCurrentActivityText()
    {
        int power = GetPower();
        return Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_generatingPowerFormat), args: [power.ToString(CultureInfo.InvariantCulture)]);
    }

    // PowerNode.pas:96
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        switch (info.Mode)
        {
            case AITargetMode.Attack:
                if (info.Player != Player && !GetRazed())
                {
                    int p = data.Priority;
                    if (Player == 0)
                    {
                        p = 50;
                    }
                    else
                    {
                        if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, targetPlayer: Player, targetRace: 0))
                        {
                            p = 50 - (StrategicMap.Current.Players.GetPlayers(info.Player).Income / 2);
                            if (p < 25)
                            {
                                p = 25;
                            }

                            if (p > 75)
                            {
                                p = 75;
                            }
                        }
                    }

                    if (data.Priority < p)
                    {
                        data.Priority = p;
                    }
                }

                break;
            case AITargetMode.Raid:
            case AITargetMode.Recruit:
            case AITargetMode.Produce:
            case AITargetMode.Hunt:
            case AITargetMode.Fortify:
            case AITargetMode.Defend:
            case AITargetMode.GuardArea:
            case AITargetMode.AttackArmies:
            default:
                break;
        }
    }

    // PowerNode.pas:117
    public override void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            if (Status.Contains(MapObjectStatusElement.Activated))
            {
                if (Player != -1)
                {
                    _ = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(_powerSource);
                }

                base.SetPlayer(player);
                if (Player != -1)
                {
                    _ = StrategicMap.Current.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(_powerSource);
                }
            }
            else
            {
                base.SetPlayer(player);
            }
        }
    }

    // PowerNode.pas:67
    public override int GetPower() => !GetRazed() && !Building ? 10 : 0;

    // PowerNode.pas:145
    public override string GetCombatEventText(Battle combat)
    {
        return combat.CombatResult is CombatResult.Victory or CombatResult.NoDefenders
            ? AbilityRules.TranslateResourceString(GetEngine(), S: s_powerNodeBattleWonText)
            : "";
    }

    private static readonly string s_generatingPowerFormat = "Generating %s power";
    private static readonly string s_powerNodeBattleWonText = "Power Node captured";

    public enum StateElement : byte
    {
        Razed = 0
    }

    public new class Resource : PlayerStructureResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            TerrainBound = false;
            Claimed = false;
            RazeDefenderBudget = 200;
            Razeable = true;
            RebuildGold = 50;
            RebuildDays = 2;
        }

        // PowerNode.pas:166
        public new const int ClassId = 131_483;

        // PowerNode.pas:169
        public override int ResourceUserClassId() => PowerNode.ClassId;

        // PowerNode.pas:163
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_powerNodeText);

        private static readonly string s_powerNodeText = "Power Node";

    }
}
