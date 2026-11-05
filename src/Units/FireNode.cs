// Translated from FireNode.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.FireNode;

public partial class TFireNode : TProductionPlace
{
    public TFireNode(TECustomNode owner) : base(owner)
    {}

    // FireNode.pas:36
    public new const int ClassID = 132062;

    // FireNode.pas:39
    public override TMagicSphere GetMagicSphere() => TMagicSphere.msFire;

}

public partial class TFireNodeResource : TProductionPlaceResource
{
    public TFireNodeResource(TECustomNode owner) : base(owner)
    {
        FRazeDefenderBudget = 200;
        FRebuildGold = 50;
        FRebuildDays = 3;
    }

    // FireNode.pas:53
    public new const int ClassID = 132063;

    // FireNode.pas:42


    // FireNode.pas:56
    public override int ResourceUserClassID() => 132062;

    // FireNode.pas:26


    // FireNode.pas:50
    public override string ResourceEditName() => TranslateRStr(FireNodeRStr);

}

public static partial class FireNode
{
    // FireNode.pas:59
    public static void RegisterFireNode(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TFireNode>(), EClass.Of<TFireNodeResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
