// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

namespace Aow.Game.Magic.Abilities;

// (enabled, level)
public partial class DispelMagicAbilityData : AbilityData
{
    public new const int ClassId = 131789;

    public DispelMagicAbilityData(ECustomNode owner) : base(owner)
    { }

    public bool Enabled;
    public byte Level;

    // AoWAb.PAS:4034
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Boolean(ref Enabled, 10);
        stream.Byte(ref Level, 11);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Enabled = persister.Boolean("Enabled", Enabled);
        Level = persister.Byte("Level", Level);
    }

    // AoWAb.PAS:4030
    public override int GetAbilityId() => 60;

    // Engine.pas:524

}
