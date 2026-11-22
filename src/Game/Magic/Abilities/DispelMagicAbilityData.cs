// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// (enabled, level)
public class DispelMagicAbilityData(CustomNode owner) : AbilityData(owner)
{
    public new const int ClassId = 131_789;

    public bool Enabled { get; set; }
    public int Level { get; set; }

    // AoWAb.PAS:4034
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Enabled = persister.Boolean("Enabled", value: Enabled);
        Level = persister.Byte("Level", value: Level.TruncateToByte());
    }

    // AoWAb.PAS:4030
    public override int GetAbilityId() => 60;
}
