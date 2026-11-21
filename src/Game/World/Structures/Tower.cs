#nullable disable warnings
// Translated from Tower.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

// State, bit 0 razed
public partial class Tower(CustomNode owner) : PlayerStructure(owner)
{

    // Tower.pas:55
    public new const int ClassId = 132_158;

    public States State { get; set; }

    // Tower.pas:96
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        State = persister.Set("State", value: State);
    }

    // Tower.pas:102
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (base.Resource.TerrainList.Terrain[message.HexNumber] >= 0 && message.MessageId == 4387 && message.Param[0].As<MapField>().FindNoneTransparentHexSprite(typeof(Tower)) == this && !Building && !GetRazed())
        {
            message.Result = 2;
        }
    }

    // Tower.pas:85
    public override void SetRazed(bool value)
    {
        if (value)
        {
            SetPlayer(-1);
            State |= States.Razed;
        }
        else
        {
            State &= ~States.Razed;
        }

        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Tower.pas:82
    public override bool GetRazed() => State.Contains(States.Razed);

    // Tower.pas:73
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        base.UpdateAITarget(info, ref data);
        if (!State.Contains(States.Razed))
        {
            switch (info.Mode)
            {
                case AITargetMode.Fortify:
                    if (info.Player == Player)
                    {
                        data.Priority = 50;
                    }

                    break;
                case AITargetMode.Attack:
                case AITargetMode.Raid:
                case AITargetMode.Recruit:
                case AITargetMode.Produce:
                case AITargetMode.Hunt:
                case AITargetMode.Defend:
                case AITargetMode.GuardArea:
                case AITargetMode.AttackArmies:
                default:
                    break;
            }
        }
    }

    // Tower.pas:58
    public override int VisibilityRange()
    {
        int result;
        if (!Building)
        {
            result = 12;
            if (HexLevel != 0 || (Player > 0 && (((byte)StrategicMap.Current.Players.GetPlayers(Player).MagicControl.EnemyEnchantments) & 8) != 0))
            {
                return (result + 1) >>> 1;
            }
        }
        else
        {
            return 4;
        }

        return result;
    }

    // Tower.pas:70
    public override int DefaultAITargetPriority => 1;

    [Flags]
    public enum States : byte
    {
        None = 0,
        Razed = 1 << 0,
    }

    public new class Resource : PlayerStructureResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            TerrainBound = true;
            Claimed = false;
            Razeable = true;
            RazeDefenderBudget = 50;
            RebuildGold = 25;
            RebuildDays = 2;
        }

        // Tower.pas:124
        public new const int ClassId = 132_159;

        // Tower.pas:127
        public override int ResourceUserClassId() => Tower.ClassId;

        // Tower.pas:121
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: GameText.TowerText);

    }
}
