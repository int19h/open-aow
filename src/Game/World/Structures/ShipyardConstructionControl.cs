// Translated from Shipyard.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

public partial class ShipyardConstructionControl : StructureConstructionControl
{
    public ShipyardConstructionControl(ECustomNode owner) : base(owner)
    { }

    // Shipyard.pas:93
    public override void GetInfo(Troop AUnit, ref string Text, ref int Gold, ref int Days)
    {
        var shipyard = AbilityRules.TranslateRStr(GameText.ShipyardRStr);
        Text = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.BuildXRStr), [shipyard]);
        Gold = 50;
        Days = 2;
    }

    // Shipyard.pas:100
    public override bool ValidateLocation(Troop AUnit, ref string Reason)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        MapLevel MapLevel;
        HexNumberWalkData Walk = default;
        bool Result = base.ValidateLocation(AUnit, ref Reason);
        if (Result)
        {
            Result = false;
            AUnit.GetLocation(ref X, ref Y, ref L);
            MapLevel = AoWHSMap.Current.MapContainer.GetMapLevel(L);
            HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 0);
            while (Walk.HN < 7 && !Result)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    var fieldValue = MapLevel.GetField(Walk.X, Walk.Y);
                    Result = Set<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)fieldValue.TerrainType);
                }

                HexNumberWalkData.WalkToNextHN(ref Walk);
            }

            if (!Result)
            {
                Reason = AbilityRules.TranslateRStr(CanOnlyBuildNextToWaterRStr);
            }
        }

        return Result;
    }

    // Shipyard.pas:81
    public override StructureResource StructureResource(Troop AUnit)
    {
        StructureResource Result;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        AUnit.GetLocation(ref X, ref Y, ref L);
        if (L == 0)
        {
            Result = (StructureResource)AoWHSSet.Current.MapObjectResourceList.FindResource(60);
        }
        else
        {
            Result = (StructureResource)AoWHSSet.Current.MapObjectResourceList.FindResource(107);
        }

        return Result;
    }

    // Shipyard.pas:78
    public override int ID() => 4;

    private static readonly string CanOnlyBuildNextToWaterRStr = "Can only be built next to water";

}
