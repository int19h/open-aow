// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

namespace Aow.Game.Magic.Abilities;

// { Interface declaration run owned by P1. }
// (Next,)
public partial class AbilityData : EngineObject
{

    public AbilityData(ECustomNode owner) : base(owner)
    { }

    public AbilityData Next { get; set; }

    // Ability.PAS:1234
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
    }

    // Ability.PAS:1230
    public virtual int GetAbilityId() => -1;
}
