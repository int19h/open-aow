// Translated from Hill.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.Terrain;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Hill
{
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
            TEObject Result = default;
            THillMO MO = default;
            MO = new THillMO(null);
            MO.SetResource(this);
            MO.SetResourceLinked(true);
            Result = MO;
            return Result;
        }

        // Hill.pas:45
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            var call__1 = HillRStr;
            var call__2 = TranslateRStr(call__1);
            Result = call__2;
            return Result;
        }

    }

    public static partial class Hill
    {
        // Hill.pas:58
        public static void RegisterHill(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<THillMO>()]);
        }

        public static readonly AnsiString HillNameRStr = (AnsiString)"Hill";
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
