// Translated from Hill.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.Terrain;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Hill;

public partial class THillMO : TTerrainMO
{
    public THillMO(TECustomNode owner) : base(owner)
    {}

    // Hill.pas:39
    public new const int ClassID = 131432;

    // Terrain.pas:19


}

public partial class THillMOR : TTerrainMOR
{
    public THillMOR(TECustomNode owner) : base(owner)
    {}

    // Hill.pas:42
    public new const int ClassID = 131433;

    // Hill.pas:48
    public override TEObject CreateResourceUser()
    {
        THillMO MO;
        MO = new THillMO(null);
        MO.SetResource(this);
        MO.SetResourceLinked(true);
        return MO;
    }

    // Hill.pas:45
    public override string ResourceEditName() => TranslateRStr(HillRStr);

}

public static partial class HillUnit
{
    // Hill.pas:58
    public static void RegisterHill(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<THillMO>()]);
    }

    public static readonly string HillNameRStr = "Hill";
    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
