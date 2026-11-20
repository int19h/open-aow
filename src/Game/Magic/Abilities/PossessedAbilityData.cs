// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

namespace Aow.Game.Magic.Abilities;

// (source experience)
public class PossessedAbilityData(CustomNode owner) : AbilityData(owner)
{
    public new const int ClassId = 131797;

    public int SourceExperience { get; set; }

    // AoWAb.PAS:2556
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SourceExperience = persister.Int32("SourceExperience", value: SourceExperience);
    }

    // AoWAb.PAS:2551
    public override int GetAbilityId() => 107;
}
