// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Magic.Abilities;

// (duration, player index, ability ID, unresolved Integer)
public partial class DurationAbilityData : AbilityData
{
    public new const int ClassID = 139266;

    public DurationAbilityData(ECustomNode owner) : base(owner)
    { }

    public byte FDuration;
    public byte FPlayerIndex;
    public ushort FAbilityID;
    public int FSource;

    // AoWAb.PAS:16
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FDuration, 11);
        Stream.rwWord(ref FAbilityID, 12);
        Stream.rwByte(ref FPlayerIndex, 13);
        Stream.rwInteger(ref FSource, 14);
    }

    // AoWAb.PAS:11
    public override int GetAbilityID() => FAbilityID;

    // Engine.pas:524

}
