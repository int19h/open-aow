// Translated from AoWE.pas.

namespace Aow.Game.World.Map;

// inherited map and level class IDs
public partial class AoWNewHSMapSettings : NewHSMapSettings
{
    // Legacy factory runs the split base body after raw allocation.
    public AoWNewHSMapSettings() : base()
    {
        HSMapClassID = 131089;
        MapLevelClassID = 131091;
    }

    // AoWHSMap.PAS:921

    // intf_P1_03.inc:346

}
