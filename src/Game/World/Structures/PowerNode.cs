// Translated from PowerNode.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
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
    public PowerNode(ECustomNode owner) : base(owner)
    {
        _powerSource = new PlayerStructurePowerSource(null);
        _powerSource.Setup(this);
    }

    // PowerNode.pas:64
    public new const int ClassId = 131482;

    public Set<StateElement> State;
    private PlayerStructurePowerSource _powerSource;

    // PowerNode.pas:58
    public override void Dispose()
    {
        base.Dispose();
        _powerSource?.Dispose();
    }

    // PowerNode.pas:87
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        RwSet(ref State, 1, 20, stream.rwData, 0);
    }

    // PowerNode.pas:51

    // PowerNode.pas:131
    public override void Activate()
    {
        base.Activate();
        if (Player != -1)
        {
            AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(_powerSource);
        }
    }

    // PowerNode.pas:138
    public override void Deactivate()
    {
        if (Player != -1)
        {
            AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(_powerSource);
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
        var power = GetPower();
        return RtlUnit.Format(AbilityRules.TranslateResourceString(s_generatingPowerFormat), [IntToStr(power)]);
    }

    // PowerNode.pas:96
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        int P;
        switch (info.Mode)
        {
            case AITargetMode.Attack:
                if (info.Player != Player && !GetRazed())
                {
                    P = data.Priority;
                    if (Player == 0)
                    {
                        P = 50;
                    }
                    else
                    {
                        if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, Player, 0))
                        {
                            P = 50 - AoWHSMap.Current.Players.GetPlayers(info.Player).Income / 2;
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

                    if (data.Priority < P)
                    {
                        data.Priority = P;
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
                    AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.UnregisterPowerSource(_powerSource);
                }

                base.SetPlayer(player);
                if (Player != -1)
                {
                    AoWHSMap.Current.Players.GetPlayers(Player).MagicControl.RegisterPowerSource(_powerSource);
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
        int result;
        if (!GetRazed() && !GetBuilding())
        {
            result = 10;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // PowerNode.pas:145
    public override string GetCombatEventText(Battle combat)
    {
        string result;
        if (combat.CombatResult == CombatResult.Victory || combat.CombatResult == CombatResult.NoDefenders)
        {
            result = AbilityRules.TranslateResourceString(s_powerNodeBattleWonText);
        }
        else
        {
            result = "";
        }

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
        public Resource(ECustomNode owner) : base(owner)
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
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_powerNodeText);

        private static readonly string s_powerNodeText = "Power Node";

    }

    // PowerNode.pas:25

    // PowerNode.pas:25

}
