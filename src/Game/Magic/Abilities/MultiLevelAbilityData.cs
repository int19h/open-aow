// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Magic.Abilities;

// (level, ability ID)
public partial class MultiLevelAbilityData : AbilityData
{
    public new const int ClassID = 139265;

    public MultiLevelAbilityData(ECustomNode owner) : base(owner)
    { }

    public byte FLevel;
    public ushort FLevelAbilityID;

    // AoWAb.PAS:231
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FLevel, 10);
        Stream.rwWord(ref FLevelAbilityID, 11);
    }

    // AoWAb.PAS:243
    public override int GetAbilityID() => FLevelAbilityID;

    // Engine.pas:524

}
