// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

namespace Aow.Game.Magic.Abilities;

// (duration, player index, ability ID, unresolved Integer)
public partial class DurationAbilityData : AbilityData
{
    public new const int ClassId = 139266;

    public DurationAbilityData(ECustomNode owner) : base(owner)
    { }

    public byte Duration;
    public byte PlayerIndex;
    public ushort AbilityId;
    public int Source;

    // AoWAb.PAS:16
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Duration, 11);
        stream.UInt16(ref AbilityId, 12);
        stream.Byte(ref PlayerIndex, 13);
        stream.Int32(ref Source, 14);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Duration = persister.Byte("Duration", value: Duration);
        AbilityId = persister.UInt16("AbilityId", value: AbilityId);
        PlayerIndex = persister.Byte("PlayerIndex", value: PlayerIndex);
        Source = persister.Int32("Source", value: Source);
    }

    // AoWAb.PAS:11
    public override int GetAbilityId() => AbilityId;

    // Engine.pas:524

}
