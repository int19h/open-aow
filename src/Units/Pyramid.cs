// Translated from Pyramid.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.ItemExpS;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Pyramid;

public partial class TPyramid : TItemExplorationSite
{
    public TPyramid(TECustomNode owner) : base(owner)
    {}

    // Pyramid.pas:34
    public new const int ClassID = 131982;

}

public partial class TPyramidResource : TItemExplorationSiteResource
{
    public TPyramidResource(TECustomNode owner) : base(owner)
    {}

    // Pyramid.pas:40
    public new const int ClassID = 131983;

    // Pyramid.pas:43
    public override int ResourceUserClassID() => 131982;

    // Pyramid.pas:37
    public override string ResourceEditName() => TranslateRStr(PyramidRStr);

}

public static partial class PyramidUnit
{
    // Pyramid.pas:46
    public static void RegisterPyramid(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TPyramid>(), EClass.Of<TPyramidResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
