// Translated from CombatHex.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class WaterCombatHexagon : AoWWaterHexagon
{
    public WaterCombatHexagon(ECustomNode owner) : base(owner)
    { }

    // CombatHex.pas:219
    public new const int ClassID = 2229655;

    // CombatHex.pas:216
    public override MapObjectResourceList GetResourceList(GameEngine AEngine) => AoWHSSet.Current.FCombatResources;

    public new partial class Resource : AoWWaterHexagon.Resource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // CombatHex.pas:222
        public new const int ClassID = 2229656;

        // CombatHex.pas:228
        public override int ResourceUserClassID() => 2229655;

        // CombatHex.pas:231
        public override HexagonControl GetHexagonControl() => ((AoWHSSet)Engine.HSSet).FCombatHexagons;

        // CombatHex.pas:225
        public override string ResourceEditName() => "Combat Water/Ice hex";

    }

}
