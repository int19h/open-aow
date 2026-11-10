using Aow.Game.Combat;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

public partial class City
{
    // War mode shows the war panel to every player.
    public override StructureView InfoPanel(sbyte Viewer)
    {
        var state = StatePanel();
        if (state != null)
        {
            return state;
        }

        if (ShownToOwner(Viewer))
        {
            return FMode.Contains(ModeElement.Warmode) ? StructureView.WarCity : StructureView.City;
        }

        return FMode.Contains(ModeElement.Warmode) ? StructureView.WarCity : StructureView.OtherPlayer;
    }

    public override StructureView InfoButton(sbyte Viewer) => StructureView.CityScreen;

    public override StructureView DoubleClick(sbyte Viewer) => FPlayer == Viewer ? StructureView.CityScreen : StructureView.None;

    // "0", the directions of the neighbors that hold the city, "x" up to four characters, "C000".
    public override string CombatMapName(CombatMapSite Site)
    {
        string Stem = "0";
        for (int I = 1; I <= 6; I += 1)
        {
            if (Site.NeighborHolds(I, typeof(City)))
            {
                Stem = Stem + IntToStr(I);
                Site.FixTag(I);
            }
        }

        while (Stem.Length < 4)
        {
            Stem = Stem + "x";
        }

        return Stem + "C000";
    }
}
