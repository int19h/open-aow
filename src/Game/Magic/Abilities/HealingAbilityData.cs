// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

namespace Aow.Game.Magic.Abilities;

// (enabled)
public partial class HealingAbilityData : AbilityData
{
    public new const int ClassId = 131758;

    public HealingAbilityData(ECustomNode owner) : base(owner)
    { }

    public bool Enabled;

    // AoWAb.PAS:3611
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Boolean(ref Enabled, 10);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Enabled = persister.Boolean("Enabled", value: Enabled);
    }

    // AoWAb.PAS:3607
    public override int GetAbilityId() => 47;

    // Engine.pas:524

}
