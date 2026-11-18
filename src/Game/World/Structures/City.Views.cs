using Aow.Game.Combat;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.World.Structures;

public partial class City
{
    // War mode shows the war panel to every player.
    public override StructureView InfoPanel(sbyte viewer)
    {
        StructureView state = StatePanel();
        return state ?? (ShownToOwner(viewer)
            ? Mode.Contains(ModeElement.WarMode) ? StructureView.WarCity : StructureView.City
            : Mode.Contains(ModeElement.WarMode) ? StructureView.WarCity : StructureView.OtherPlayer);
    }

    public override StructureView InfoButton(sbyte viewer) => StructureView.CityScreen;

    public override StructureView DoubleClick(sbyte viewer) => Player == viewer ? StructureView.CityScreen : StructureView.None;

    // "0", the directions of the neighbors that hold the city, "x" up to four characters, "C000".
    public override string CombatMapName(CombatMapSite site)
    {
        string stem = "0";
        for (int i = 1; i <= 6; i += 1)
        {
            if (site.NeighborHolds(i, typeof(City)))
            {
                stem += IntToStr(i);
                site.FixTag(i);
            }
        }

        while (stem.Length < 4)
        {
            stem += "x";
        }

        return stem + "C000";
    }
}
