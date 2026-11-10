// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Magic.Abilities;

// { Interface declaration run owned by P1. }
// (FNext,)
public partial class AbilityData : EObject
{

    public AbilityData(ECustomNode owner) : base(owner)
    { }

    public AbilityData FNext;

    // Ability.PAS:1234
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
    }

    // Ability.PAS:1230
    public virtual int GetAbilityID() => -1;
}
