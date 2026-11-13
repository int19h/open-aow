// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref AbilityId, 20);
        stream.Int32(ref CommandAbilityId, 21);
        stream.Byte(ref SourceId, 22);
        stream.Byte(ref OriginalCombatPlayer, 23);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        AbilityId = persister.Int32("AbilityId", value: AbilityId);
        CommandAbilityId = persister.Int32("CommandAbilityId", value: CommandAbilityId);
        SourceId = persister.Byte("SourceId", value: SourceId);
        OriginalCombatPlayer = persister.Byte("OriginalCombatPlayer", value: OriginalCombatPlayer);
    }

    // AoWAb.PAS:5001
    public override int GetAbilityId() => AbilityId;

}
