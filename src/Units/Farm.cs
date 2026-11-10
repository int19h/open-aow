// Translated from Farm.pas.
using Aow.Game.Ai;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Session;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// { ot=1 }
// FMode
public partial class Farm : PlayerCropStructure
{
    public Farm(ECustomNode owner) : base(owner)
    { }

    // Farm.pas:54
    public new const int ClassID = 131950;

    public Set<ModeElement> FMode;

    // Farm.pas:81
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        RwSet(ref FMode, 1, 20, Stream.rwData, 0);
    }

    // Farm.pas:130
    public override void TerrainChanged(in HSTerrainInfo Info)
    {
        base.TerrainChanged(Info);
        if (Info.HN == 0)
        {
            UpdateCrops();
        }
    }

    // Farm.pas:64
    public override void SetRazed(bool Value)
    {
        if (Value)
        {
            SetPlayer(-1);
            FMode = FMode + Set<ModeElement>.Of(ModeElement.Razed);
        }
        else
        {
            FMode = FMode - Set<ModeElement>.Of(ModeElement.Razed);
        }

        UpdateCrops();
        FImageIndex = GetTerrainTypeImage(FTerrainType);
    }

    // Farm.pas:59
    public override bool GetRazed() => FMode.Contains(ModeElement.Razed);

    // Farm.pas:76
    public override string GetCurrentActivityText()
    {
        var income = GetIncome();
        return RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ProducingXIncomeRStr), [IntToStr(income)]);
    }

    // Farm.pas:111
    public override void UpdateAITarget(in AITargetInfo Info, ref AITargetData Data)
    {
        int Priority;
        if (!GetRazed() || GetBuilding())
        {
            switch (Info.Mode)
            {
                case AITargetMode.Attack:
                    if (Info.Player != FPlayer && !GetRazed())
                    {
                        Priority = Data.Priority;
                        if (FPlayer == 0)
                        {
                            Priority = 50;
                        }
                        else
                        {
                            if (Aow.Game.Magic.Abilities.AbilityRules.ValidAttackRelation(Info.Player, Info.IndependentRelation, FPlayer, 0))
                            {
                                Priority = 50 - Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Info.Player).FIncome / 2;
                                if (Priority < 25)
                                {
                                    Priority = 25;
                                }

                                if (Priority > 75)
                                {
                                    Priority = 75;
                                }
                            }
                        }

                        if (Priority > Data.Priority)
                        {
                            Data.Priority = Priority;
                        }
                    }

                    break;
            }
        }
    }

    // Farm.pas:92
    public override bool CanPlaceCrop(MapField Field) => Field.TerrainType != 5 && base.CanPlaceCrop(Field);

    // Farm.pas:87
    public override bool CropsEnabled() => base.CropsEnabled() && FTerrainType != 5;

    // Farm.pas:97
    public override void UpdateCrops()
    {
        int Count;
        if (!GetEditMode() && FStatus.Contains(Aow.Game.World.MapObjectStatusElement.Activated))
        {
            if (GetRazed() || FTerrainType == 5)
            {
                Count = 0;
            }
            else
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
                {
                    Count = ((PlayerCropStructureResource)FResource).FCropLocations.Count;
                }
                else
                {
                    Count = -1;
                }
            }

            if (Count != -1)
            {
                SetCropCount(Count);
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
            FClaimed = false;
            FRazeable = true;
            FRazeDefenderBudget = 50;
            FRebuildGold = 25;
            FRebuildDays = 2;
        }

        // Farm.pas:151
        public new const int ClassID = 131951;

        // Farm.pas:136

        // Farm.pas:156
        public override int ResourceUserClassID() => 131950;

        // Farm.pas:36

        // Farm.pas:146
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.Farm.Resource.FarmRStr);

        private static readonly string FarmRStr = "Farm";

    }

}
