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
    public override void GetInfo(Troop troop, ref string text, ref int gold, ref int days)
    {
        var shipyard = AbilityRules.TranslateResourceString(GameText.ShipyardText);
        text = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.BuildFormat), [shipyard]);
        gold = 50;
        days = 2;
    }

    // Shipyard.pas:100
    public override bool ValidateLocation(Troop troop, ref string reason)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        MapLevel mapLevel;
        HexNumberWalkData walk = default;
        bool result = base.ValidateLocation(troop, ref reason);
        if (result)
        {
            result = false;
            troop.GetLocation(ref X, ref Y, ref L);
            mapLevel = AoWHSMap.Current.MapContainer.GetMapLevel(L);
            HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
            while (walk.HexNumber < 7 && !result)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
                {
                    var fieldValue = mapLevel.GetField(walk.X, walk.Y);
                    result = Set<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)fieldValue.TerrainType);
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }

            if (!result)
            {
                reason = AbilityRules.TranslateResourceString(s_canOnlyBuildNextToWaterText);
            }
        }

        return result;
    }

    // Shipyard.pas:81
    public override StructureResource StructureResource(Troop troop)
    {
        StructureResource result;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        troop.GetLocation(ref X, ref Y, ref L);
        if (L == 0)
        {
            result = (StructureResource)AoWHSSet.Current.MapObjectResourceList.FindResource(60);
        }
        else
        {
            result = (StructureResource)AoWHSSet.Current.MapObjectResourceList.FindResource(107);
        }

        return result;
    }

    // Shipyard.pas:78
    public override int Id() => 4;

    private static readonly string s_canOnlyBuildNextToWaterText = "Can only be built next to water";

}
