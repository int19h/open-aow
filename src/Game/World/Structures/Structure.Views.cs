using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Structure
{
    // The info window panel for the seated player Viewer (AoWInfoWin.pas:616).
    public virtual StructureView InfoPanel(sbyte viewer) => StatePanel() ?? StructureView.Generic;

    // The info window button (AoWInfoWin.pas:190): a screen to open, or None after the kind ran
    // its own action.
    public virtual StructureView InfoButton(sbyte viewer) => StructureView.None;

    // A double click on the selected structure on the map (aowMapEvents.pas:518).
    public virtual StructureView DoubleClick(sbyte viewer) => StructureView.None;

    // The tactical map file stem under TCMaps (AoWTC.pas:4156). The original matched exact class
    // ids, and every other structure fights on the blank map.
    public virtual string CombatMapName(CombatMapSite site) => "Blank000";

    // A structure under construction or razed shows that state before its kind.
    protected StructureView StatePanel()
    {
        if (GetBuilding())
        {
            return StructureView.Building;
        }

        if (GetRazed())
        {
            return StructureView.Razed;
        }

        return null;
    }
}
