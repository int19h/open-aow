using System.Runtime.InteropServices;
using Aow.Engine.Buffers;
using Aow.Rtl;

namespace Aow.Game.Session;

/// <summary>
/// The stored record of a <see cref="SetupPlayerBehavior"/> (Delphi <c>TSetupPlayerBehavior</c>,
/// 8 bytes): the player type, three bytes of padding that own documents carry as
/// <c>$reserved</c>, and the flags.
/// </summary>
[StoredLayout, StoredRecord("SetupPlayerBehavior")]
[StructLayout(LayoutKind.Explicit, Size = 8)]
public readonly struct SetupPlayerBehaviorImage : IRecordImage<SetupPlayerBehaviorImage, SetupPlayerBehavior>
{
    [FieldOffset(0)] public readonly byte PlayerType;
    [FieldOffset(1)] public readonly Array3<byte> Reserved;
    [FieldOffset(4)] public readonly int Flags;

    private SetupPlayerBehaviorImage(in SetupPlayerBehavior value)
    {
        PlayerType = value.PlayerType;
        Reserved = Array3<byte>.From([(byte)value.UnknownBytes, (byte)(value.UnknownBytes >> 8), (byte)(value.UnknownBytes >> 16)]);
        Flags = value.Flags;
    }

    public static SetupPlayerBehaviorImage From(SetupPlayerBehavior value) => new(value);

    public SetupPlayerBehavior ToValue() =>
        new(PlayerType: PlayerType, Flags: Flags) { UnknownBytes = Reserved[0] | (Reserved[1] << 8) | (Reserved[2] << 16) };
}
