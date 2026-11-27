#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from Farm.pas.
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiFormat;

namespace Aow.Game.World.Structures;

// { ot=1 }
// Mode
public partial class Farm(CustomNode owner) : PlayerCropStructure(owner)
{

    // Farm.pas:54
    public new const int ClassId = 131_950;

    public Modes Mode { get; set; }

    // Farm.pas:81
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Mode = persister.Set("Mode", value: Mode);
    }

    // Farm.pas:130
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        base.TerrainChanged(info);
        if (info.HexNumber == 0)
        {
            UpdateCrops();
        }
    }

    // Farm.pas:64
    public override void SetRazed(bool value)
    {
        if (value)
        {
            SetPlayer(null);
            Mode |= Modes.Razed;
        }
        else
        {
            Mode &= ~Modes.Razed;
        }

        UpdateCrops();
        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Farm.pas:59
    public override bool GetRazed() => Mode.Contains(Modes.Razed);

    // Farm.pas:76
    public override string GetCurrentActivityText()
    {
        int income = GetIncome();
        return Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.ProducingIncomeFormat), args: [income.ToString(CultureInfo.InvariantCulture)]);
    }

    // Farm.pas:111
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        if (!GetRazed() || Building)
        {
            switch (info.Mode)
            {
                case AITargetMode.Attack:
                    if (info.Player != Player && !GetRazed())
                    {
                        int priority = data.Priority;
                        if (Player == PlayerId.Independent)
                        {
                            priority = 50;
                        }
                        else
                        {
                            if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, targetPlayer: Player, targetRace: 0, engine: GetEngine()!))
                            {
                                priority = 50 - (StrategicMap.Current.Players.GetPlayers(info.Player).Income / 2);
                                if (priority < 25)
                                {
                                    priority = 25;
                                }

                                if (priority > 75)
                                {
                                    priority = 75;
                                }
                            }
                        }

                        if (priority > data.Priority)
                        {
                            data.Priority = priority;
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
    }

    // Farm.pas:92
    public override bool CanPlaceCrop(MapField field) => (!field.TryGetTerrain(out TerrainType terrain) || terrain != Map.TerrainType.Wasteland) && base.CanPlaceCrop(field);

    // Farm.pas:87
    public override bool CropsEnabled() => base.CropsEnabled() && TerrainType != 5;

    // Farm.pas:97
    public override void UpdateCrops()
    {
        if (!EditMode && Status.Contains(MapObjectStatuses.Activated))
        {
            int count = GetRazed() || TerrainType == 5
                ? 0
                : StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting)
                    ? base.Resource.CropLocations.Count
                    : -1;

            if (count != -1)
            {
                SetCropCount(count);
            }

            UpdateCropSettings();
            UpdateIncome();
        }
    }

    [Flags]
    public enum Modes : byte
    {
        None = 0,
        Razed = 1 << 0,
    }

    public new class Resource : PlayerCropStructureResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            Claimed = false;
            Razeable = true;
            RazeDefenderBudget = 50;
            RebuildGold = 25;
            RebuildDays = 2;
        }

        // Farm.pas:151
        public new const int ClassId = 131_951;

        // Farm.pas:156
        public override int ResourceUserClassId() => Farm.ClassId;

        // Farm.pas:146
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_farmText);

        private static readonly string s_farmText = "Farm";

    }
}
