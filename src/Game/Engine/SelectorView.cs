// Translated from AoWE.pas.
using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Engine;

public partial class SelectorView
{
    // Combat.pas:102
    public virtual void SetSelection()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:103
    public virtual bool CanSelectUnit(Unit Unit_)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}
