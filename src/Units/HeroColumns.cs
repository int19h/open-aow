using System.Runtime.InteropServices;
using Aow.Game.Draw.Images;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Grids;

internal static class HeroColumns
{
    internal static readonly string GridUsedRStr = "Used";

    internal static int HeroControlCompareName(int A, int B)
    {
        var hero = AoWHSMap.Current.FHeroControl.GetHero(B);
        return CompareText(AoWHSMap.Current.FHeroControl.GetHero(A).FHeroName, hero.FHeroName);
    }

    internal static int HeroControlCompareLevel(int A, int B)
    {
        int Result = AoWHSMap.Current.FHeroControl.GetHero(A).GetLevel() - AoWHSMap.Current.FHeroControl.GetHero(B).GetLevel();
        if (Result == 0)
        {
            Result = HeroControlCompareName(A, B);
        }

        return Result;
    }

}
