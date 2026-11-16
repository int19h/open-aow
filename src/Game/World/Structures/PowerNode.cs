#nullable disable
// Translated from PowerNode.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

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
    public new const int ClassId = 131482;

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

    // PowerNode.pas:51

    // PowerNode.pas:131
    public override void Activate()
    {
        base.Activate();
        if (Player != -1)
        {
            StrategicMap.Current.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(_powerSource);
        }
    }

    // PowerNode.pas:138
    public override void Deactivate()
    {
        if (Player != -1)
        {
            StrategicMap.Current.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(_powerSource);
        }

        base.Deactivate();
    }

    // PowerNode.pas:76
    public override void SetRazed(bool value)
    {
        if (value)
        {
            SetPlayer(-1);
            State = State + Set<StateElement>.Of(StateElement.Razed);
        }
        else
        {
            State = State - Set<StateElement>.Of(StateElement.Razed);
        }

        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // PowerNode.pas:73
    public override bool GetRazed() => State.Contains(StateElement.Razed);

    // PowerNode.pas:93
    public override string GetCurrentActivityText()
    {
        int power = GetPower();
        return Format(AbilityRules.TranslateResourceString(GetEngine(), s_generatingPowerFormat), [IntToStr(power)]);
    }

    // PowerNode.pas:96
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        int p;
        switch (info.Mode)
        {
            case AITargetMode.Attack:
                if (info.Player != Player && !GetRazed())
                {
                    p = data.Priority;
                    if (Player == 0)
                    {
                        p = 50;
                    }
                    else
                    {
                        if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, Player, 0))
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
                    StrategicMap.Current.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(_powerSource);
                }

                base.SetPlayer(player);
                if (Player != -1)
                {
                    StrategicMap.Current.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(_powerSource);
                }
            }
            else
            {
                base.SetPlayer(player);
            }
        }
    }

    // PowerNode.pas:67
    public override int GetPower()
    {
        int result = !GetRazed() && !GetBuilding() ? 10 : 0;
        return result;
    }

    // PowerNode.pas:145
    public override string GetCombatEventText(Battle combat)
    {
        string result = combat.CombatResult == CombatResult.Victory || combat.CombatResult == CombatResult.NoDefenders
            ? AbilityRules.TranslateResourceString(GetEngine(), s_powerNodeBattleWonText)
            : "";
        return result;
    }

    private static readonly string s_generatingPowerFormat = "Generating %s power";
    private static readonly string s_powerNodeBattleWonText = "Power Node captured";

    public enum StateElement : byte
    {
        Razed = 0
    }

    public new partial class Resource : PlayerStructureResource
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
        public new const int ClassId = 131483;

        // PowerNode.pas:152

        // PowerNode.pas:169
        public override int ResourceUserClassId() => 131482;

        // PowerNode.pas:41

        // PowerNode.pas:163
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_powerNodeText);

        private static readonly string s_powerNodeText = "Power Node";

    }

    // PowerNode.pas:25

    // PowerNode.pas:25

}
