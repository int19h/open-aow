// Translated from AoWSetup.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

// behaviors, race, status, leader, player, behavior, email
public partial class PlayerSetupSettings : EngineObject
{
    // AoWSetup.pas:312
    public new const int ClassId = 131683;

    public SetupPlayerBehavior Behavior;
    public SetupPlayerBehavior OriginalBehavior;
    public sbyte Race;
    // AoW.exe
    public Set<PlayerSetupStatusElement> Status;

    public Leader Leader { get; private set; }

    public byte PlayerId;
    public byte BehaviorMode;

    public string EmailAddress { get; set; } = "";

    // AoWSetup.pas:305
    public override void Dispose()
    {
        Leader?.Dispose();
        Leader = null;
        base.Dispose();
    }

    // AoWSetup.pas:315
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref PlayerId, 10);
        TransferRecord<SetupPlayerBehaviorImage, SetupPlayerBehavior>(ref Behavior, 8, 11, stream.Bytes);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Race), 12);
        stream.Bytes(Bytes.Of(ref BehaviorMode, 1), 1, 13);
        stream.Object(Leader, 14);
        TransferSet(ref Status, 1, 15, stream.Bytes, 0);
        TransferRecord<SetupPlayerBehaviorImage, SetupPlayerBehavior>(ref OriginalBehavior, 8, 16, stream.Bytes);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PlayerId = persister.Byte("PlayerId", value: PlayerId);
        Behavior = PersistSupport.Record<SetupPlayerBehaviorImage, SetupPlayerBehavior>(persister, name: "Behavior", value: Behavior);
        Race = PersistSupport.UnsignedByte(persister, name: "Race", value: Race);
        BehaviorMode = persister.RawByte("BehaviorMode", value: BehaviorMode);
        persister.Object("Leader", objectValue: Leader);
        Status = persister.Set("Status", value: Status);
        OriginalBehavior = PersistSupport.Record<SetupPlayerBehaviorImage, SetupPlayerBehavior>(persister, name: "OriginalBehavior", value: OriginalBehavior);
    }

    // AoWSetup.pas:296
    public PlayerSetupSettings(ECustomNode owner) : base(owner)
    {
        Status = Set<PlayerSetupStatusElement>.Of();
        Behavior = s_defaultBehavior;
        OriginalBehavior = s_defaultOriginalBehavior;
        Leader = new Leader(null) { Holder = CreationHolder() };
    }

    private static readonly SetupPlayerBehavior s_defaultBehavior = new(PlayerType: 0, Flags: 0);

    private static readonly SetupPlayerBehavior s_defaultOriginalBehavior = new(PlayerType: 5, Flags: 0);

}
