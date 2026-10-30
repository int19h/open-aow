// Translated from Farm.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.Crops;
using static Aow.Units.Crops.Crops;
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
    public partial class TFarm : Aow.Units.Crops.TPlayerCropStructure
    {
        public TFarm(TECustomNode owner) : base(owner)
        {}

        // Farm.pas:54
        public new const int ClassID = 131950;

        public TSet<Aow.Units.Farm.TFarmModeElement> FMode;
        // Farm.pas:81
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Aow.Rtl.Rtl.RwSet(ref FMode, 1, 20, Stream.rwData, 0);
        }

        // Farm.pas:130
        public override void TerrainChanged(in Aow.Units.HSEngine.THSTerrainInfo Info)
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
                SetPlayer(unchecked((sbyte)-1));
                FMode = FMode + TSet<Aow.Units.Farm.TFarmModeElement>.Of(Aow.Units.Farm.TFarmModeElement.fmRazed);
            }
            else
            {
                FMode = FMode - TSet<Aow.Units.Farm.TFarmModeElement>.Of(Aow.Units.Farm.TFarmModeElement.fmRazed);
            }

            UpdateCrops();
            var call__1 = GetTerrainTypeImage(FTerrainType);
            FImageIndex = call__1;
        }

        // Farm.pas:59
        public override bool GetRazed()
        {
            bool Result = default;
            Result = FMode.Contains(Aow.Units.Farm.TFarmModeElement.fmRazed);
            return Result;
        }

        // Farm.pas:76
        public override Aow.Rtl.AnsiString GetCurrentActivityText()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__2 = GetIncome();
            var call__3 = Aow.Units.AoWE.AoWE.ProducingXIncomeRStr;
            var call__4 = Aow.Units.AoWE.AoWE.TranslateRStr(call__3);
            var call__5 = Aow.Rtl.Rtl.Format(call__4, [Aow.Rtl.Rtl.IntToStr(call__2)]);
            Result = call__5;
            return Result;
        }

        // Farm.pas:111
        public override void UpdateAITarget(in Aow.Units.AoWE.TAITargetInfo Info, ref Aow.Units.AoWE.TAITargetData Data)
        {
            int Priority = default;
            var call__6 = GetRazed();
            bool bool__7 = !call__6;
            if (!bool__7)
            {
                var call__8 = GetBuilding();
                bool__7 = call__8;
            }

            if (bool__7)
            {
                switch (Info.Mode)
                {
                    case Aow.Units.AoWE.TAITargetMode.tmAttack:
                        bool bool__9 = (Info.Player != FPlayer);
                        if (bool__9)
                        {
                            var call__10 = GetRazed();
                            bool__9 = !call__10;
                        }

                        if (bool__9)
                        {
                            Priority = Data.Priority;
                            if (FPlayer == 0)
                            {
                                Priority = 50;
                            }
                            else
                            {
                                var call__11 = Aow.Units.AoWE.AoWE.ValidAttackRelation(unchecked((sbyte)Info.Player), unchecked((sbyte)Info.IndependentRelation), unchecked((sbyte)FPlayer), 0);
                                if (call__11)
                                {
                                    var call__12 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Info.Player);
                                    Priority = 50 - (call__12.FIncome / 2);
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
        public override bool CanPlaceCrop(Aow.Units.HSEngine.TMapField Field)
        {
            bool Result = default;
            bool bool__13 = (unchecked((short)Field.TerrainType) != 5);
            if (bool__13)
            {
                var call__14 = base.CanPlaceCrop(Field);
                bool__13 = call__14;
            }

            Result = bool__13;
            return Result;
        }

        // Farm.pas:87
        public override bool CropsEnabled()
        {
            bool Result = default;
            Result = base.CropsEnabled() && (unchecked((short)FTerrainType) != 5);
            return Result;
        }

        // Farm.pas:97
        public override void UpdateCrops()
        {
            int Count = default;
            var call__15 = GetEditMode();
            if (!call__15 && FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moActivated))
            {
                var call__16 = GetRazed();
                if (call__16 || (unchecked((short)FTerrainType) == 5))
                {
                    Count = 0;
                }
                else
                {
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
                    {
                        Count = ((Aow.Units.Farm.TFarmResource)FResource).FCropLocations.Count;
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

    public partial class TFarmResource : Aow.Units.Crops.TPlayerCropStructureResource
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
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131950;
            return Result;
        }

        // Farm.pas:36


        // Farm.pas:146
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__17 = Aow.Units.AoWE.AoWE.FarmRStr;
            var call__18 = Aow.Units.AoWE.AoWE.TranslateRStr(call__17);
            Result = call__18;
            return Result;
        }

    }

    public static partial class Farm
    {
        // Farm.pas:161
        public static int FarmIncome(sbyte Player)
        {
            int Result = default;
            Aow.Units.AoWE.TPlayer P = default;
            int I = default;
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            if (((Aow.Units.AoWE.TAoWHSMap)Aow.Units.AoWE.AoWE.AoWHSMap) != null)
            {
                var call__19 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                P = call__19;
                Result = 0;
                {
                    I = 0;
                    int end__20 = P.FStructures.Count - 1;
                    for (; I <= end__20; I += 1)
                    {
                        var call__21 = P.FStructures.GetStructure(I);
                        Structure = call__21;
                        var call__22 = Structure.EClass.ClassID;
                        if ((call__22 == 131950) && (Structure.FPlayer == Player))
                        {
                            Result += ((Aow.Units.Farm.TFarm)Structure).FIncome;
                        }
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
        public static void RegisterFarm(Aow.Units.Engine.TEngine Engine)
        {
            Engine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Farm.TFarm>(), Aow.Units.Engine.EClass.Of<Aow.Units.Farm.TFarmResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
