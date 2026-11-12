// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

namespace Aow.Game.Magic.Abilities;

// (source experience)
public partial class PossessedAbilityData : AbilityData
{
    public new const int ClassId = 131797;

    public PossessedAbilityData(ECustomNode owner) : base(owner)
    { }

    public int SourceExperience;

    // AoWAb.PAS:2556
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref SourceExperience, 30);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SourceExperience = persister.Int32("SourceExperience", SourceExperience);
    }

    // AoWAb.PAS:2551
    public override int GetAbilityId() => 107;

    // Engine.pas:524

}
