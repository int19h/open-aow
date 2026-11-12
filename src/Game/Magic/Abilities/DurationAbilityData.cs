// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Duration, 11);
        stream.rwWord(ref AbilityId, 12);
        stream.rwByte(ref PlayerIndex, 13);
        stream.rwInteger(ref Source, 14);
    }

    // AoWAb.PAS:11
    public override int GetAbilityId() => AbilityId;

    // Engine.pas:524

}
