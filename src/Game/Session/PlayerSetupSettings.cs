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
public partial class PlayerSetupSettings : EObject
{
    // AoWSetup.pas:312
    public new const int ClassID = 131683;

    public SetupPlayerBehavior FBehavior;
    public SetupPlayerBehavior FOriginalBehavior;
    public sbyte FRace;
    // AoW.exe
    public Set<PlayerSetupStatusElement> FStatus;
    public Leader FLeader;
    public byte FPlayerID;
    public byte FBehaviorMode;
    public string FEmailAddress = "";

    // AoWSetup.pas:305
    public override void Dispose()
    {
        FLeader?.Dispose();
        FLeader = null;
        base.Dispose();
    }

    // AoWSetup.pas:315
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FPlayerID, 10);
        Stream.rwData(Bytes.Of(ref FBehavior, 8), 8, 11);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FRace), 12);
        Stream.rwData(Bytes.Of(ref FBehaviorMode, 1), 1, 13);
        Stream.rwEObject(FLeader, 14);
        RwSet(ref FStatus, 1, 15, Stream.rwData, 0);
        Stream.rwData(Bytes.Of(ref FOriginalBehavior, 8), 8, 16);
    }

    // AoWSetup.pas:296
    public PlayerSetupSettings(ECustomNode owner) : base(owner)
    {
        FStatus = Set<PlayerSetupStatusElement>.Of();
        FBehavior = DefaultBehavior;
        FOriginalBehavior = DefaultOriginalBehavior;
        FLeader = new Leader(null);
    }

    private static SetupPlayerBehavior DefaultBehavior = new SetupPlayerBehavior
    {
        PlayerType = 0,
        Reserved = FixedArray_byte_3.From((ReadOnlySpan<byte>)new byte[3] { 0, 0, 0 }),
        Flags = 0
    };

    private static SetupPlayerBehavior DefaultOriginalBehavior = new SetupPlayerBehavior
    {
        PlayerType = 5,
        Reserved = FixedArray_byte_3.From((ReadOnlySpan<byte>)new byte[3] { 0, 0, 0 }),
        Flags = 0
    };

}
