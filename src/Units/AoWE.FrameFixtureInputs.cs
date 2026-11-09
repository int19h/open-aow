#nullable enable
using System.Runtime.CompilerServices;
using Aow.Rtl;

namespace Aow.Units.AoWE;

/// <summary>
/// Explicit capture inputs. Fixture and smoke processes pin construction order for the reads that follow the old counter.
/// The looped frame offset stays on the identity hash unless the capture replaces it.
/// </summary>
public static class FrameFixtureInputs
{
    public static int? AddressAnimationPhase { get; set; }
    // Water and adjacent raised terrain share the map texture capture phase.
    public static int? WaterVariantPhase { get; set; }
    public static bool Tactical { get; set; }
    public static int? MeasuredFrameRate { get; set; }
    public static Action? TacticalInitialized { get; set; }

    // Unit tests pin one address here. A capture leaves this empty and uses construction order.
    internal static int? PinnedAddress { get; set; }

    // The value the original read from the object address. Null reads as 0.
    public static int Address(TObject? instance) => instance is null ? 0 : PinnedAddress ?? CaptureAddress.Of(instance);

    // The capture replaces the shifted offset. Ordinary play uses the identity hash, which stays fixed for one object.
    public static int AnimationOffset(TObject instance) => AddressAnimationPhase ?? ((int)((uint)RuntimeHelpers.GetHashCode(instance) >> 2) & 65535);

    public static int WaterBasePhase(TObject? owner) => WaterVariantPhase ?? (int)((uint)Address(owner) >> 3);

    public static int TextureVariant(TObject instance) => WaterVariantPhase ?? ((int)((uint)Address(instance) >> 8) & 3);

    // Combat edges stay on the address. The water capture does not cover them.
    public static int CombatTextureVariant(TObject instance) => (int)((uint)Address(instance) >> 8) & 3;

    public static int PhaseModulo(TObject instance, int divisor) => Address(instance) % divisor;

    public static int ShimmerOffset(TObject instance, int frame) => (frame % 10 * 2 + Address(instance)) % 20;

    public static int ArmyStagger(TObject? unit) => (int)((uint)Address(unit) >> 3) & 7;

    public static int FieldFrameOffset(TObject field) => (int)((uint)Address(field) >> 8) & 15;
}
