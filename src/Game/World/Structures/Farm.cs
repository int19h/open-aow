// Translated from Farm.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// { ot=1 }
// Mode
public partial class Farm : PlayerCropStructure
{
    public Farm(ECustomNode owner) : base(owner)
    { }

    // Farm.pas:54
    public new const int ClassId = 131950;

    public Set<ModeElement> Mode { get; set; }

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
            SetPlayer(-1);
            Mode = Mode + Set<ModeElement>.Of(ModeElement.Razed);
        }
        else
        {
            Mode = Mode - Set<ModeElement>.Of(ModeElement.Razed);
        }

        UpdateCrops();
        ImageIndex = GetTerrainTypeImage(TerrainType);
    }

    // Farm.pas:59
    public override bool GetRazed() => Mode.Contains(ModeElement.Razed);

    // Farm.pas:76
    public override string GetCurrentActivityText()
    {
        var income = GetIncome();
        return RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.ProducingIncomeFormat), [IntToStr(income)]);
    }

    // Farm.pas:111
    public override void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        int priority;
        if (!GetRazed() || GetBuilding())
        {
            switch (info.Mode)
            {
                case AITargetMode.Attack:
                    if (info.Player != Player && !GetRazed())
                    {
                        priority = data.Priority;
                        if (Player == 0)
                        {
                            priority = 50;
                        }
                        else
                        {
                            if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, Player, 0))
                            {
                                priority = 50 - StrategicMap.Current.Players.GetPlayers(info.Player).Income / 2;
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
            }
        }
    }

    // Farm.pas:92
    public override bool CanPlaceCrop(MapField field) => field.TerrainType != 5 && base.CanPlaceCrop(field);

    // Farm.pas:87
    public override bool CropsEnabled() => base.CropsEnabled() && TerrainType != 5;

    // Farm.pas:97
    public override void UpdateCrops()
    {
        int count;
        if (!GetEditMode() && Status.Contains(MapObjectStatusElement.Activated))
        {
            if (GetRazed() || TerrainType == 5)
            {
                count = 0;
            }
            else
            {
                if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                {
                    count = ((PlayerCropStructureResource)_resource).CropLocations.Count;
                }
                else
                {
                    count = -1;
                }
            }

            if (count != -1)
            {
                SetCropCount(count);
            }

            UpdateCropSettings();
            UpdateIncome();
        }
    }

    public enum ModeElement : byte
    {
        Razed = 0
    }

    public new partial class Resource : PlayerCropStructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            Claimed = false;
            Razeable = true;
            RazeDefenderBudget = 50;
            RebuildGold = 25;
            RebuildDays = 2;
        }

        // Farm.pas:151
        public new const int ClassId = 131951;

        // Farm.pas:136

        // Farm.pas:156
        public override int ResourceUserClassId() => 131950;

        // Farm.pas:36

        // Farm.pas:146
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_farmText);

        private static readonly string s_farmText = "Farm";

    }

}
