// Translated from Mine.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// State (bit 0 razed), IncomeSource
public partial class Mine : PlayerStructure
{
    public Mine(ECustomNode owner) : base(owner)
    {
        _incomeSource = new PlayerStructureIncomeSource(null) { Holder = CreationHolder() };
        _incomeSource.Setup(this);
    }

    // Mine.pas:82
    public new const int ClassId = 131962;

    public byte State;
    private readonly PlayerStructureIncomeSource _incomeSource;

    // Mine.pas:76
    public override void Dispose()
    {
        base.Dispose();
        _incomeSource?.Dispose();
    }

    // Mine.pas:172
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Bytes(Bytes.Of(ref State, 1), 1, 20);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        State = persister.RawByte("State", value: State);
    }

    // Mine.pas:69

    // Mine.pas:142
    public override void Activate()
    {
        base.Activate();
        if (Player != -1)
        {
            StrategicMap.Current.Players.GetPlayers((sbyte)(byte)Player).RegisterIncomeSource(_incomeSource);
        }
    }

    // Mine.pas:150
    public override void Deactivate()
    {
        if (Player != -1)
        {
            StrategicMap.Current.Players.GetPlayers((sbyte)(byte)Player).UnregisterIncomeSource(_incomeSource);
        }

        base.Deactivate();
    }

    // Mine.pas:161
    public override void SetRazed(bool value)
    {
        if (value)
        {
            SetPlayer(-1);
            State = (byte)(Set<byte>.FromMask(State) + Set<byte>.WithBounds(0, 7).With(0)).Image;
        }
        else
        {
            State = (byte)(Set<byte>.FromMask(State) - Set<byte>.WithBounds(0, 7).With(0)).Image;
        }

        this.ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Mine.pas:93
    public override string GetCurrentActivityText()
    {
        var income = GetIncome();
        return RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.ProducingIncomeFormat), [IntToStr(income)]);
    }

    // Mine.pas:99
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        int candidatePriority;
        if (!GetRazed() || GetBuilding())
        {
            candidatePriority = data.Priority;
            switch (info.Mode)
            {
                case AITargetMode.Attack:
                    if ((byte)info.Player != (byte)Player && !GetRazed())
                    {
                        if ((byte)Player == 0)
                        {
                            candidatePriority = 50;
                        }
                        else
                        {
                            if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, (sbyte)(byte)Player, 0))
                            {
                                candidatePriority = 50 - StrategicMap.Current.Players.GetPlayers(info.Player).Income / 2;
                                if (candidatePriority < 25)
                                {
                                    candidatePriority = 25;
                                }

                                if (candidatePriority > 75)
                                {
                                    candidatePriority = 75;
                                }
                            }
                        }
                    }

                    break;
                case AITargetMode.Defend:
                    if ((byte)info.Player == (byte)Player)
                    {
                        data.Priority = 20;
                    }

                    break;
            }

            // { Original can overwrite the special kind-6 priority with the previous higher value. }
            if (data.Priority < candidatePriority)
            {
                data.Priority = candidatePriority;
            }
        }
    }

    // Mine.pas:126
    public override void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            if (Status.Contains(MapObjectStatusElement.Activated))
            {
                if (Player != -1)
                {
                    StrategicMap.Current.Players.GetPlayers((sbyte)(byte)Player).UnregisterIncomeSource(_incomeSource);
                }

                base.SetPlayer(player);
                if (Player != -1)
                {
                    StrategicMap.Current.Players.GetPlayers((sbyte)(byte)Player).RegisterIncomeSource(_incomeSource);
                }
            }
            else
            {
                base.SetPlayer(player);
            }
        }
    }

    // Mine.pas:85
    public override int GetIncome()
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

    // Mine.pas:178
    public override string GetCombatEventText(Battle combat)
    {
        string result;
        if (combat.CombatResult == CombatResult.Victory || combat.CombatResult == CombatResult.NoDefenders)
        {
            result = AbilityRules.TranslateResourceString(GetEngine(), s_mineBattleWonText);
        }
        else
        {
            result = "";
        }

        return result;
    }

    // Mine.pas:25
    public override bool GetRazed() => (State & 1) != 0;

    private static readonly string s_mineBattleWonText = "Mine captured";

    public new partial class Resource : PlayerStructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            TerrainBound = false;
            Claimed = false;
            Razeable = true;
            RazeDefenderBudget = 75;
            RebuildGold = 25;
            RebuildDays = 2;
        }

        // Mine.pas:200
        public new const int ClassId = 131963;

        // Mine.pas:186

        // Mine.pas:203
        public override int ResourceUserClassId() => 131962;

        // Mine.pas:41

        // Mine.pas:197
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_mineText);

        private static readonly string s_mineText = "Mine";

    }
}
