#nullable disable warnings
// Translated from AoWSetup.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Rtl;

namespace Aow.Game.Session;

// behaviors, race, status, leader, player, behavior, email
public sealed class PlayerSetupSettings : EngineObject
{
    // AoWSetup.pas:312
    public new const int ClassId = 131_683;

    public SetupPlayerBehavior Behavior { get; set; }
    public SetupPlayerBehavior OriginalBehavior { get; set; }
    public int Race { get; set; }
    // AoW.exe
    public PlayerSetupStatuses Status { get; set; }

    public Leader? Leader { get; private set; }

    public PlayerId? PlayerId { get; set; } = Session.PlayerId.Independent;
    public int BehaviorMode { get; set; }

    public string EmailAddress { get; set; } = "";

    // AoWSetup.pas:305
    public override void Dispose()
    {
        Leader?.Dispose();
        Leader = null;
        base.Dispose();
    }

    // AoWSetup.pas:315
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PlayerId = persister.OptionalPlayer("PlayerId", value: PlayerId, width: IdWidth.Byte, fallback: 0);
        Behavior = persister.Record<SetupPlayerBehaviorImage, SetupPlayerBehavior>(name: "Behavior", value: Behavior);
        Race = persister.UnsignedByte(name: "Race", value: Race.TruncateToSByte());
        BehaviorMode = persister.RawByte("BehaviorMode", value: BehaviorMode.TruncateToByte());
        persister.Object("Leader", objectValue: Leader);
        Status = persister.Set("Status", value: Status);
        OriginalBehavior = persister.Record<SetupPlayerBehaviorImage, SetupPlayerBehavior>(name: "OriginalBehavior", value: OriginalBehavior);
    }

    // AoWSetup.pas:296
    public PlayerSetupSettings(CustomNode? owner) : base(owner)
    {
        Status = PlayerSetupStatuses.None;
        Behavior = s_defaultBehavior;
        OriginalBehavior = s_defaultOriginalBehavior;
        Leader = new Leader(null) { Holder = CreationHolder() };
    }

    private static readonly SetupPlayerBehavior s_defaultBehavior = new(PlayerType: 0, Flags: 0);

    private static readonly SetupPlayerBehavior s_defaultOriginalBehavior = new(PlayerType: 5, Flags: 0);

}
