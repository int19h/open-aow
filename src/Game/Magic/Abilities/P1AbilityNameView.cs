// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class P1AbilityNameView : EObject
{
    public P1AbilityNameView(ECustomNode owner) : base(owner)
    { }

    // Combat.pas:94
    public virtual void Execute()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:95
    public virtual void GetHidden()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:96
    public virtual void GetAbilityType()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:97
    public virtual string GetName(object Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}
