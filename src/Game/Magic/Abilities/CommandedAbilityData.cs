// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

namespace Aow.Game.Magic.Abilities;

// (ability ID, commander ID, command ability ID, original combat player)
public partial class CommandedAbilityData : AbilityData
{
    public new const int ClassId = 131757;

    public CommandedAbilityData(ECustomNode owner) : base(owner)
    { }

    public int AbilityId { get; set; }
    public byte SourceId { get; set; }
    public int CommandAbilityId { get; set; }
    public byte OriginalCombatPlayer { get; set; }

    // AoWAb.PAS:5005
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
