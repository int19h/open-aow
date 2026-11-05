// Translated from Shipyard.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Shipyard;

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
    public override int GetBaseIncome() => 0;

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
    public override int ResourceUserClassID() => 132286;

    // Shipyard.pas:29


    // Shipyard.pas:69
    public override string ResourceEditName() => TranslateRStr(ShipyardRStr);

}

public partial class TShipyardConstructionControl : TStructureConstructionControl
{
    public TShipyardConstructionControl(TECustomNode owner) : base(owner)
    { }

    // Shipyard.pas:93
    public override void GetInfo(TAbstractUnit AUnit, ref string Text, ref int Gold, ref int Days)
    {
        var shipyard = TranslateRStr(ShipyardRStr);
        Text = Rtl.Rtl.Format(TranslateRStr(BuildXRStr), [shipyard]);
        Gold = 50;
        Days = 2;
    }

    // Shipyard.pas:100
    public override bool ValidateLocation(TAbstractUnit AUnit, ref string Reason)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TMapLevel MapLevel;
        THNWalkData Walk = default;
        bool Result = base.ValidateLocation(AUnit, ref Reason);
        if (Result)
        {
            Result = false;
            AUnit.GetLocation(ref X, ref Y, ref L);
            MapLevel = AoWHSMap.MapContainer.GetMapLevel(L);
            InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.HN < 7 && !Result)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    var fieldValue = MapLevel.GetField(Walk.X, Walk.Y);
                    Result = TSet<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)fieldValue.TerrainType);
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
        TStructureResource Result;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        AUnit.GetLocation(ref X, ref Y, ref L);
        if (L == 0)
        {
            Result = (TStructureResource)AoWHSSet.MapObjectResourceList.FindResource(60);
        }
        else
        {
            Result = (TStructureResource)AoWHSSet.MapObjectResourceList.FindResource(107);
        }

        return Result;
    }

    // Shipyard.pas:78
    public override int ID() => 4;

}

public static partial class Shipyard
{
    // Shipyard.pas:125
    public static void RegisterShipyard(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TShipyard>(), EClass.Of<TShipyardResource>()]);
        var shipyardConstructionControl = new TShipyardConstructionControl(null);
        ((TConstructAbility)AoWHSSet.FAbilityControl.GetAbility(125)).RegisterConstructionControl(shipyardConstructionControl);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
