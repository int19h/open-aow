// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Magic.Abilities;

// (ability ID, commander ID, command ability ID, original combat player)
public partial class CommandedAbilityData : AbilityData
{
    public new const int ClassId = 131757;

    public CommandedAbilityData(ECustomNode owner) : base(owner)
    { }

    public int AbilityId;
    public byte SourceId;
    public int CommandAbilityId;
    public byte OriginalCombatPlayer;

    // AoWAb.PAS:5005
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref AbilityId, 20);
        stream.rwInteger(ref CommandAbilityId, 21);
        stream.rwByte(ref SourceId, 22);
        stream.rwByte(ref OriginalCombatPlayer, 23);
    }

    // AoWAb.PAS:5001
    public override int GetAbilityId() => AbilityId;

}
