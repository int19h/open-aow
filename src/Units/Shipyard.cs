// Translated from Shipyard.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Shipyard
{
    public partial class TShipyard : TProductionPlace
    {
        public TShipyard(TECustomNode owner) : base(owner)
        {}

        // Shipyard.pas:49
        public new const int ClassID = 132286;

        // Shipyard.pas:55
        public override void UpdateAITarget(in TAITargetInfo Info, ref TAITargetData Data)
        {
            base.UpdateAITarget(Info, ref Data);
        }

        // Shipyard.pas:52
        public override int GetBaseIncome()
        {
            int Result = default;
            Result = 0;
            return Result;
        }

    }

    public partial class TShipyardResource : TProductionPlaceResource
    {
        public TShipyardResource(TECustomNode owner) : base(owner)
        {
            FTerrainBound = false;
            FRazeDefenderBudget = 150;
            FRebuildGold = 25;
            FRebuildDays = 2;
        }

        // Shipyard.pas:72
        public new const int ClassID = 132287;

        // Shipyard.pas:60


        // Shipyard.pas:75
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132286;
            return Result;
        }

        // Shipyard.pas:29


        // Shipyard.pas:69
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(ShipyardRStr);
            return Result;
        }

    }

    public partial class TShipyardConstructionControl : TStructureConstructionControl
    {
        public TShipyardConstructionControl(TECustomNode owner) : base(owner)
        { }

        // Shipyard.pas:93
        public override void GetInfo(TAbstractUnit AUnit, ref AnsiString Text, ref int Gold, ref int Days)
        {
            var call__1 = TranslateRStr(ShipyardRStr);
            var call__2 = TranslateRStr(BuildXRStr);
            var call__3 = Rtl.Rtl.Format(call__2, [call__1]);
            Text = call__3;
            Gold = 50;
            Days = 2;
        }

        // Shipyard.pas:100
        public override bool ValidateLocation(TAbstractUnit AUnit, ref AnsiString Reason)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            TMapLevel MapLevel = default;
            THNWalkData Walk = default;
            Result = base.ValidateLocation(AUnit, ref Reason);
            if (Result)
            {
                Result = false;
                AUnit.GetLocation(ref X, ref Y, ref L);
                var call__4 = AoWHSMap.MapContainer.GetMapLevel(L);
                MapLevel = call__4;
                InitHNWalk(ref Walk, X, Y, 0);
                while (Walk.HN < 7 && !Result)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                    {
                        var call__5 = MapLevel.GetField(Walk.X, Walk.Y);
                        Result = TSet<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)call__5.TerrainType);
                    }

                    WalkToNextHN(ref Walk);
                }

                if (!Result)
                {
                    Reason = TranslateRStr(CanOnlyBuildNextToWaterRStr);
                }
            }

            return Result;
        }

        // Shipyard.pas:81
        public override TStructureResource StructureResource(TAbstractUnit AUnit)
        {
            TStructureResource Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            AUnit.GetLocation(ref X, ref Y, ref L);
            if (L == 0)
            {
                var call__6 = AoWHSSet.MapObjectResourceList.FindResource(60);
                Result = (TStructureResource)call__6;
            }
            else
            {
                var call__7 = AoWHSSet.MapObjectResourceList.FindResource(107);
                Result = (TStructureResource)call__7;
            }

            return Result;
        }

        // Shipyard.pas:78
        public override int ID()
        {
            int Result = default;
            Result = 4;
            return Result;
        }

    }

    public static partial class Shipyard
    {
        // Shipyard.pas:125
        public static void RegisterShipyard(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TShipyard>(), EClass.Of<TShipyardResource>()]);
            var call__8 = new TShipyardConstructionControl(null);
            var call__9 = AoWHSSet.FAbilityControl.GetAbility(125);
            ((TConstructAbility)call__9).RegisterConstructionControl(call__8);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
