using Aow.Game.Combat;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

public partial class City
{
    // War mode shows the war panel to every player.
    public override StructureView InfoPanel(sbyte viewer)
    {
        var state = StatePanel();
        if (state != null)
        {
            return state;
        }

        if (ShownToOwner(viewer))
        {
            return Mode.Contains(ModeElement.WarMode) ? StructureView.WarCity : StructureView.City;
        }

        return Mode.Contains(ModeElement.WarMode) ? StructureView.WarCity : StructureView.OtherPlayer;
    }

    public override StructureView InfoButton(sbyte viewer) => StructureView.CityScreen;

    public override StructureView DoubleClick(sbyte viewer) => Player == viewer ? StructureView.CityScreen : StructureView.None;

    // "0", the directions of the neighbors that hold the city, "x" up to four characters, "C000".
    public override string CombatMapName(CombatMapSite site)
    {
        string stem = "0";
        for (int I = 1; I <= 6; I += 1)
        {
            if (site.NeighborHolds(I, typeof(City)))
            {
                stem = stem + IntToStr(I);
                site.FixTag(I);
            }
        }

        while (stem.Length < 4)
        {
            stem = stem + "x";
        }

        return stem + "C000";
    }
}
