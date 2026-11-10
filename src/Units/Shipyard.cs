// Translated from Shipyard.pas.
using Aow.Game.Ai;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.World.Structures;

public partial class TShipyard : TProductionPlace
{
    public TShipyard(TECustomNode owner) : base(owner)
    { }

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
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ShipyardRStr);

}

public partial class TShipyardConstructionControl : TStructureConstructionControl
{
    public TShipyardConstructionControl(TECustomNode owner) : base(owner)
    { }

    // Shipyard.pas:93
    public override void GetInfo(TAbstractUnit AUnit, ref string Text, ref int Gold, ref int Days)
    {
        var shipyard = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ShipyardRStr);
        Text = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.BuildXRStr), [shipyard]);
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
            MapLevel = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetMapLevel(L);
            Aow.Game.World.Movement.THNWalkData.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.HN < 7 && !Result)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    var fieldValue = MapLevel.GetField(Walk.X, Walk.Y);
                    Result = TSet<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)fieldValue.TerrainType);
                }

                Aow.Game.World.Movement.THNWalkData.WalkToNextHN(ref Walk);
            }

            if (!Result)
            {
                Reason = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TShipyardConstructionControl.CanOnlyBuildNextToWaterRStr);
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
            Result = (TStructureResource)Aow.Game.Engine.TAoWHSSet.AoWHSSet.MapObjectResourceList.FindResource(60);
        }
        else
        {
            Result = (TStructureResource)Aow.Game.Engine.TAoWHSSet.AoWHSSet.MapObjectResourceList.FindResource(107);
        }

        return Result;
    }

    // Shipyard.pas:78
    public override int ID() => 4;

    private static readonly string CanOnlyBuildNextToWaterRStr = "Can only be built next to water";

}
