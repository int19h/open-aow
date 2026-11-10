// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.World.Structures;

// inherited item array
public partial class ProductionControlList : DelphiList
{
    // Structur.PAS:929
    public ProductionControl GetPC(int Index) => (ProductionControl)List[Index];

    // System.pas:111

}
