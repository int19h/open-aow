using System.Runtime.InteropServices;
using Aow.Rtl;

namespace Aow.Game.Session;

/// <summary>
/// The stored record of a <see cref="SetupPlayerBehavior"/> (Delphi <c>TSetupPlayerBehavior</c>,
/// 8 bytes): the player type, three padding bytes (ignored on read, written as 0) and the flags.
/// </summary>
[StoredLayout, StoredRecord("SetupPlayerBehavior"), StoredPadding(offset: 1, length: 3)]
[StructLayout(LayoutKind.Explicit, Size = 8)]
public readonly struct SetupPlayerBehaviorImage : IRecordImage<SetupPlayerBehaviorImage, SetupPlayerBehavior>
{
    [FieldOffset(0)] public readonly byte PlayerType;
    [FieldOffset(4)] public readonly int Flags;

    private SetupPlayerBehaviorImage(in SetupPlayerBehavior value)
    {
        PlayerType = value.PlayerType;
        Flags = value.Flags;
    }

    public static SetupPlayerBehaviorImage From(SetupPlayerBehavior value) => new(value);

    public SetupPlayerBehavior ToValue() => new(PlayerType: PlayerType, Flags: Flags);
}
