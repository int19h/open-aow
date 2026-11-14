// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

namespace Aow.Game.Magic.Abilities;

// (enabled, level)
public partial class DispelMagicAbilityData : AbilityData
{
    public new const int ClassId = 131789;

    public DispelMagicAbilityData(ECustomNode owner) : base(owner)
    { }

    public bool Enabled { get; set; }
    public byte Level { get; set; }

    // AoWAb.PAS:4034
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Enabled = persister.Boolean("Enabled", value: Enabled);
        Level = persister.Byte("Level", value: Level);
    }

    // AoWAb.PAS:4030
    public override int GetAbilityId() => 60;

    // Engine.pas:524

}
