// Translated from AoWSetup.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref PlayerId, 10);
        stream.rwData(Bytes.Of(ref Behavior, 8), 8, 11);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Race), 12);
        stream.rwData(Bytes.Of(ref BehaviorMode, 1), 1, 13);
        stream.rwEObject(Leader, 14);
        RwSet(ref Status, 1, 15, stream.rwData, 0);
        stream.rwData(Bytes.Of(ref OriginalBehavior, 8), 8, 16);
    }

    // AoWSetup.pas:296
    public PlayerSetupSettings(ECustomNode owner) : base(owner)
    {
        Status = Set<PlayerSetupStatusElement>.Of();
        Behavior = s_defaultBehavior;
        OriginalBehavior = s_defaultOriginalBehavior;
        Leader = new Leader(null);
    }

    private static SetupPlayerBehavior s_defaultBehavior = new SetupPlayerBehavior
    {
        PlayerType = 0,
        Reserved = ByteArray3.From((ReadOnlySpan<byte>)new byte[3] { 0, 0, 0 }),
        Flags = 0
    };

    private static SetupPlayerBehavior s_defaultOriginalBehavior = new SetupPlayerBehavior
    {
        PlayerType = 5,
        Reserved = ByteArray3.From((ReadOnlySpan<byte>)new byte[3] { 0, 0, 0 }),
        Flags = 0
    };

}
