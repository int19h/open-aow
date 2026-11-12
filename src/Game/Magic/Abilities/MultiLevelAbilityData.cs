// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Level, 10);
        stream.rwWord(ref LevelAbilityId, 11);
    }

    // AoWAb.PAS:243
    public override int GetAbilityId() => LevelAbilityId;

    // Engine.pas:524

}
