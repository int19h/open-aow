#nullable enable
using Aow.Rtl;

namespace Aow.Units.AoWE;

/// <summary>Explicit capture inputs. Ordinary games retain the InstanceId phase.</summary>
public static class FrameFixtureInputs
{
    public static int? AddressAnimationPhase { get; set; }
    public static bool Tactical { get; set; }
    public static int? MeasuredFrameRate { get; set; }
    public static Action? TacticalInitialized { get; set; }
    public static int AnimationOffset(TObject instance) => AddressAnimationPhase ?? (int)((uint)instance.InstanceId >> 2) & 65535;
}
