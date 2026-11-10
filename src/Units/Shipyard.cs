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

public partial class Shipyard : ProductionPlace
{
    public Shipyard(ECustomNode owner) : base(owner)
    { }

    // Shipyard.pas:49
    public new const int ClassID = 132286;

    // Shipyard.pas:55
    public override void UpdateAITarget(in AITargetInfo Info, ref AITargetData Data)
    {
        base.UpdateAITarget(Info, ref Data);
    }

    // Shipyard.pas:52
    public override int GetBaseIncome() => 0;

    public new partial class Resource : ProductionPlaceResource
    {
        public Resource(ECustomNode owner) : base(owner)
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
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.ShipyardRStr);

    }

}

public partial class ShipyardConstructionControl : StructureConstructionControl
{
    public ShipyardConstructionControl(ECustomNode owner) : base(owner)
    { }

    // Shipyard.pas:93
    public override void GetInfo(AbstractUnit AUnit, ref string Text, ref int Gold, ref int Days)
    {
        var shipyard = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.ShipyardRStr);
        Text = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.BuildXRStr), [shipyard]);
        Gold = 50;
        Days = 2;
    }

    // Shipyard.pas:100
    public override bool ValidateLocation(AbstractUnit AUnit, ref string Reason)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        MapLevel MapLevel;
        HNWalkData Walk = default;
        bool Result = base.ValidateLocation(AUnit, ref Reason);
        if (Result)
        {
            Result = false;
            AUnit.GetLocation(ref X, ref Y, ref L);
            MapLevel = Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(L);
            Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.HN < 7 && !Result)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    var fieldValue = MapLevel.GetField(Walk.X, Walk.Y);
                    Result = Set<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)fieldValue.TerrainType);
                }

                Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
            }

            if (!Result)
            {
                Reason = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(CanOnlyBuildNextToWaterRStr);
            }
        }

        return Result;
    }

    // Shipyard.pas:81
    public override StructureResource StructureResource(AbstractUnit AUnit)
    {
        StructureResource Result;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        AUnit.GetLocation(ref X, ref Y, ref L);
        if (L == 0)
        {
            Result = (StructureResource)Aow.Game.Engine.AoWHSSet.Current.MapObjectResourceList.FindResource(60);
        }
        else
        {
            Result = (StructureResource)Aow.Game.Engine.AoWHSSet.Current.MapObjectResourceList.FindResource(107);
        }

        return Result;
    }

    // Shipyard.pas:78
    public override int ID() => 4;

    private static readonly string CanOnlyBuildNextToWaterRStr = "Can only be built next to water";

}
