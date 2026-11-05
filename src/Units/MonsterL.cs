// Translated from MonsterL.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.ItemExpS;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.MonsterL
{
    public partial class TMonsterlair : TItemExplorationSite
    {
        public TMonsterlair(TECustomNode owner) : base(owner)
        {}

        // MonsterL.pas:34
        public new const int ClassID = 131886;

    }

    public partial class TMonsterlairResource : TItemExplorationSiteResource
    {
        public TMonsterlairResource(TECustomNode owner) : base(owner)
        {}

        // MonsterL.pas:40
        public new const int ClassID = 131887;

        // MonsterL.pas:43
        public override int ResourceUserClassID() => 131886;

        // MonsterL.pas:37
        public override AnsiString ResourceEditName() => TranslateRStr(MonsterLairRStr);

    }

    public static partial class MonsterL
    {
        // MonsterL.pas:46
        public static void RegisterMonsterlair(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TMonsterlair>(), EClass.Of<TMonsterlairResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
