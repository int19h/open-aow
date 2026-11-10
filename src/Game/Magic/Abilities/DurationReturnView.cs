// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// { Temporary declaration views: no instances are created. }
public partial class DurationReturnView : Ability
{

    public DurationReturnView(ECustomNode owner) : base(owner)
    { }

    // Combat.pas:853
    public virtual byte GetDuration(AbilityOwner AbilityOwner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:854
    public virtual int GetTotalDuration()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}
