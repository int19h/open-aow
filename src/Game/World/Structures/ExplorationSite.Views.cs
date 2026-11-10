using Aow.Game.Combat;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

public partial class ExplorationSite
{
    public override StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? StructureView.ExplorationSite;

    public override StructureView InfoButton(sbyte Viewer)
    {
        Search();
        return StructureView.None;
    }

    // A site fight takes one of Variants maps and fixes every neighbor tag. Another fight at the
    // site takes the blank map.
    protected static string SiteCombatMapName(CombatMapSite Site, string Prefix, int Variants, bool TakesItems)
    {
        if (!Site.SiteCombat)
        {
            return "Blank000";
        }

        Site.TakesCombatItems = TakesItems;
        string Stem = Prefix + IntToStr(Random(Variants));
        for (int I = 1; I <= 6; I += 1)
        {
            Site.FixTag(I);
        }

        return Stem;
    }
}
