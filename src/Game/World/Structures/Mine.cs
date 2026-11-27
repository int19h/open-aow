#nullable disable warnings
// Translated from Mine.pas.
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

// State (bit 0 razed), IncomeSource
public partial class Mine : PlayerStructure
{
    public Mine(CustomNode? owner) : base(owner)
    {
        _incomeSource = new PlayerStructureIncomeSource(null) { Holder = CreationHolder() };
        _incomeSource.Setup(this);
    }

    // Mine.pas:82
    public new const int ClassId = 131_962;

    public int State { get; set; }

    private readonly PlayerStructureIncomeSource _incomeSource;

    // Mine.pas:76
    public override void Dispose()
    {
        base.Dispose();
        _incomeSource?.Dispose();
    }

    // Mine.pas:172
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        State = persister.RawByte("State", value: State.TruncateToByte());
    }

    // Mine.pas:142
    public override void Activate()
    {
        base.Activate();
        if (Player is not null)
        {
            StrategicMap.Current.Players.GetPlayers((sbyte)(byte)PlayerIdExtensions.ToOriginal(Player)).RegisterIncomeSource(_incomeSource);
        }
    }

    // Mine.pas:150
    public override void Deactivate()
    {
        if (Player is not null)
        {
            StrategicMap.Current.Players.GetPlayers((sbyte)(byte)PlayerIdExtensions.ToOriginal(Player)).UnregisterIncomeSource(_incomeSource);
        }

        base.Deactivate();
    }

    // Mine.pas:161
    public override void SetRazed(bool value)
    {
        if (value)
        {
            SetPlayer(null);
            State |= 1;
        }
        else
        {
            State &= ~1;
        }

        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Mine.pas:93
    public override string GetCurrentActivityText()
    {
        int income = GetIncome();
        return Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.ProducingIncomeFormat), args: [income.ToString(CultureInfo.InvariantCulture)]);
    }

    // Mine.pas:99
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        if (!GetRazed() || Building)
        {
            int candidatePriority = data.Priority;
            switch (info.Mode)
            {
                case AITargetMode.Attack:
                    if ((byte)PlayerIdExtensions.ToOriginal(info.Player) != (byte)PlayerIdExtensions.ToOriginal(Player) && !GetRazed())
                    {
                        if ((byte)PlayerIdExtensions.ToOriginal(Player) == 0)
                        {
                            candidatePriority = 50;
                        }
                        else
                        {
                            if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, targetPlayer: PlayerId.FromOriginal((sbyte)(byte)PlayerIdExtensions.ToOriginal(Player)), targetRace: 0, engine: GetEngine()!))
                            {
                                candidatePriority = 50 - (StrategicMap.Current.Players.GetPlayers(info.Player).Income / 2);
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
                    if ((byte)PlayerIdExtensions.ToOriginal(info.Player) == (byte)PlayerIdExtensions.ToOriginal(Player))
                    {
                        data.Priority = 20;
                    }

                    break;
                case AITargetMode.Raid:
                case AITargetMode.Recruit:
                case AITargetMode.Produce:
                case AITargetMode.Hunt:
                case AITargetMode.Fortify:
                case AITargetMode.GuardArea:
                case AITargetMode.AttackArmies:
                default:
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
    public override void SetPlayer(PlayerId? player)
    {
        if (player != Player)
        {
            if (Status.Contains(MapObjectStatuses.Activated))
            {
                if (Player is not null)
                {
                    StrategicMap.Current.Players.GetPlayers((sbyte)(byte)PlayerIdExtensions.ToOriginal(Player)).UnregisterIncomeSource(_incomeSource);
                }

                base.SetPlayer(player);
                if (Player is not null)
                {
                    StrategicMap.Current.Players.GetPlayers((sbyte)(byte)PlayerIdExtensions.ToOriginal(Player)).RegisterIncomeSource(_incomeSource);
                }
            }
            else
            {
                base.SetPlayer(player);
            }
        }
    }

    // Mine.pas:85
    public override int GetIncome() => !GetRazed() && !Building ? 10 : 0;

    // Mine.pas:178
    public override string GetCombatEventText(Battle? combat)
    {
        return combat.CombatResult is CombatResult.Victory or CombatResult.NoDefenders
            ? AbilityRules.TranslateResourceString(GetEngine(), S: s_mineBattleWonText)
            : "";
    }

    // Mine.pas:25
    public override bool GetRazed() => (State & 1) != 0;

    private static readonly string s_mineBattleWonText = "Mine captured";

    public new class Resource : PlayerStructureResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            TerrainBound = false;
            Claimed = false;
            Razeable = true;
            RazeDefenderBudget = 75;
            RebuildGold = 25;
            RebuildDays = 2;
        }

        // Mine.pas:200
        public new const int ClassId = 131_963;

        // Mine.pas:203
        public override int ResourceUserClassId() => Mine.ClassId;

        // Mine.pas:197
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_mineText);

        private static readonly string s_mineText = "Mine";

    }
}
