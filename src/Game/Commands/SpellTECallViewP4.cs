// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Commands;

public partial class SpellTECallViewP4 : TokenEvent
{
    public SpellTECallViewP4(ECustomNode owner) : base(owner)
    { }

    // Combat.pas:828
    public virtual bool ValidateValue(AbstractUnit Source)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:829
    public virtual bool ProcessValue(AbstractUnit Source, int Pass)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}
