using Aow.Game.Combat;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

public partial class ExplorationSite
{
    public override StructureView InfoPanel(sbyte viewer) => StatePanel() ?? StructureView.ExplorationSite;

    public override StructureView InfoButton(sbyte viewer)
    {
        Search();
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
        string stem = prefix + IntToStr(Random(variants));
        for (int I = 1; I <= 6; I += 1)
        {
            site.FixTag(I);
        }

        return stem;
    }
}
