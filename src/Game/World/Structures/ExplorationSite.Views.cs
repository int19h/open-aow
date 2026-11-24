using System.Globalization;
using Aow.Game.Combat;
using Aow.Game.Session;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Structures;

public partial class ExplorationSite
{
    public override StructureView InfoPanel(PlayerId? viewer) => StatePanel() ?? StructureView.ExplorationSite;

    public override StructureView InfoButton(PlayerId? viewer)
    {
        _ = Search();
        return StructureView.None;
    }

    // A site fight takes one of Variants maps and fixes every neighbor tag. Another fight at the
    // site takes the blank map.
    protected static string SiteCombatMapName(CombatMapSite site, string prefix, int variants, bool takesItems)
    {
        if (!site.SiteCombat)
        {
            return "Blank000";
        }

        site.TakesCombatItems = takesItems;
        string stem = prefix + Random(variants).ToString(CultureInfo.InvariantCulture);
        for (int i = 1; i <= 6; i += 1)
        {
            site.FixTag(i);
        }

        return stem;
    }
}
