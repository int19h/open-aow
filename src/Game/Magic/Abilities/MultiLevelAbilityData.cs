// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

namespace Aow.Game.Magic.Abilities;

// (level, ability ID)
public partial class MultiLevelAbilityData : AbilityData
{
    public new const int ClassId = 139265;

    public MultiLevelAbilityData(ECustomNode owner) : base(owner)
    { }

    public byte Level;
    public ushort LevelAbilityId;

    // AoWAb.PAS:231
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Level, 10);
        stream.UInt16(ref LevelAbilityId, 11);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Level = persister.Byte("Level", Level);
        LevelAbilityId = persister.UInt16("LevelAbilityID", LevelAbilityId);
    }

    // AoWAb.PAS:243
    public override int GetAbilityId() => LevelAbilityId;

    // Engine.pas:524

}
