// Translated from Farm.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.Crops;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Farm
{
    public enum TFarmModeElement : byte
    {
        fmRazed = 0
    }

    // { ot=1 }
    // FMode
    public partial class TFarm : TPlayerCropStructure
    {
        public TFarm(TECustomNode owner) : base(owner)
        {}

        // Farm.pas:54
        public new const int ClassID = 131950;

        public TSet<TFarmModeElement> FMode;
        // Farm.pas:81
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            RwSet(ref FMode, 1, 20, Stream.rwData, 0);
        }

        // Farm.pas:130
        public override void TerrainChanged(in THSTerrainInfo Info)
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
                FMode = FMode + TSet<TFarmModeElement>.Of(TFarmModeElement.fmRazed);
            }
            else
            {
                FMode = FMode - TSet<TFarmModeElement>.Of(TFarmModeElement.fmRazed);
            }

            UpdateCrops();
            FImageIndex = GetTerrainTypeImage(FTerrainType);
        }

        // Farm.pas:59
        public override bool GetRazed() => FMode.Contains(TFarmModeElement.fmRazed);

        // Farm.pas:76
        public override AnsiString GetCurrentActivityText()
        {
            var income = GetIncome();
            return Rtl.Rtl.Format(TranslateRStr(ProducingXIncomeRStr), [IntToStr(income)]);
        }

        // Farm.pas:111
        public override void UpdateAITarget(in TAITargetInfo Info, ref TAITargetData Data)
        {
            int Priority;
            if (!GetRazed() || GetBuilding())
            {
                switch (Info.Mode)
                {
                    case TAITargetMode.tmAttack:
                        if (Info.Player != FPlayer && !GetRazed())
                        {
                            Priority = Data.Priority;
                            if (FPlayer == 0)
                            {
                                Priority = 50;
                            }
                            else
                            {
                                if (ValidAttackRelation(Info.Player, Info.IndependentRelation, FPlayer, 0))
                                {
                                    Priority = 50 - AoWHSMap.FPlayers.GetPlayers(Info.Player).FIncome / 2;
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
        public override bool CanPlaceCrop(TMapField Field) => Field.TerrainType != 5 && base.CanPlaceCrop(Field);

        // Farm.pas:87
        public override bool CropsEnabled() => base.CropsEnabled() && FTerrainType != 5;

        // Farm.pas:97
        public override void UpdateCrops()
        {
            int Count;
            if (!GetEditMode() && FStatus.Contains(TMapObjectStatusElement.moActivated))
            {
                if (GetRazed() || FTerrainType == 5)
                {
                    Count = 0;
                }
                else
                {
                    if (AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
                    {
                        Count = ((TFarmResource)FResource).FCropLocations.Count;
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

    }

    public partial class TFarmResource : TPlayerCropStructureResource
    {
        public TFarmResource(TECustomNode owner) : base(owner)
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
        public override AnsiString ResourceEditName() => TranslateRStr(FarmRStr);

    }

    public static partial class Farm
    {
        // Farm.pas:161
        public static int FarmIncome(sbyte Player)
        {
            int Result;
            TPlayer P;
            TPlayerStructure Structure;
            if (AoWHSMap != null)
            {
                P = AoWHSMap.FPlayers.GetPlayers(Player);
                Result = 0;
                for (int I = 0, last = P.FStructures.Count - 1; I <= last; I++)
                {
                    Structure = P.FStructures.GetStructure(I);
                    if (Structure.EClass.ClassID == 131950 && Structure.FPlayer == Player)
                    {
                        Result += ((TFarm)Structure).FIncome;
                    }
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // Farm.pas:177
        public static void RegisterFarm(TEngine Engine)
        {
            Engine.RegisterEClasses([EClass.Of<TFarm>(), EClass.Of<TFarmResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
