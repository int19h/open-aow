// Translated from WaterNode.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic;
using Aow.Game.World.Structures;
using static Aow.Game.Magic.Abilities.AoWEUnit;

namespace Aow.Game.World.Structures;

public partial class TWaterNode : TProductionPlace
{
    public TWaterNode(TECustomNode owner) : base(owner)
    { }

    // WaterNode.pas:37
    public new const int ClassID = 132275;

    // WaterNode.pas:40
    public override TMagicSphere GetMagicSphere() => TMagicSphere.msWater;

}

public partial class TWaterNodeResource : TProductionPlaceResource
{
    public TWaterNodeResource(TECustomNode owner) : base(owner)
    {
        FRazeDefenderBudget = 200;
        FTerrainBound = false;
        FRebuildGold = 50;
        FRebuildDays = 2;
    }

    // WaterNode.pas:55
    public new const int ClassID = 132276;

    // WaterNode.pas:43

    // WaterNode.pas:58
    public override int ResourceUserClassID() => 132275;

    // WaterNode.pas:27

    // WaterNode.pas:52
    public override string ResourceEditName() => TranslateRStr(WaterNodeRStr);

}

public static partial class WaterNodeUnit
{
    // WaterNode.pas:61
    public static void RegisterWaterNode(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TWaterNode>(), EClass.Of<TWaterNodeResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
