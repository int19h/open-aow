// Translated from CombatHex.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public class WaterCombatHex(CustomNode owner) : WaterHex(owner)
{

    // CombatHex.pas:219
    public new const int ClassId = 2_229_655;

    // CombatHex.pas:216
    public override MapObjectResourceList GetResourceList(GameEngine engine) => GamePack.Current.CombatResources;

    public new class Resource(CustomNode owner) : WaterHex.Resource(owner)
    {

        // CombatHex.pas:222
        public new const int ClassId = 2_229_656;

        // CombatHex.pas:228
        public override int ResourceUserClassId() => WaterCombatHex.ClassId;

        // CombatHex.pas:231
        public override HexControl HexControl => Engine.HexSpriteSet.CombatHexes;

        // CombatHex.pas:225
        public override string ResourceEditName() => "Combat Water/Ice hex";

    }
}
