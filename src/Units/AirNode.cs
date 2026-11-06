// Translated from AirNode.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.AirNode;

public partial class TAirNode : TProductionPlace
{
    public TAirNode(TECustomNode owner) : base(owner)
    {}

    // AirNode.pas:37
    public new const int ClassID = 133454;

    // AirNode.pas:40
    public override TMagicSphere GetMagicSphere() => TMagicSphere.msAir;

}

public partial class TAirNodeResource : TProductionPlaceResource
{
    public TAirNodeResource(TECustomNode owner) : base(owner)
    {
        FRazeDefenderBudget = 200;
        FRebuildGold = 50;
        FRebuildDays = 2;
        FTerrainBound = false;
    }

    // AirNode.pas:55
    public new const int ClassID = 133455;

    // AirNode.pas:43


    // AirNode.pas:58
    public override int ResourceUserClassID() => 133454;

    // AirNode.pas:27


    // AirNode.pas:52
    public override string ResourceEditName() => TranslateRStr(AirNodeRStr);

}

public static partial class AirNodeUnit
{
    // AirNode.pas:61
    public static void RegisterAirNode(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TAirNode>(), EClass.Of<TAirNodeResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
