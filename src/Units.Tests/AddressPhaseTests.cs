using System.Runtime.CompilerServices;
using Aow.Units.AoWE;

namespace Aow.Units.Tests;

public sealed class AddressPhaseTests
{
    [Fact]
    public void PinnedPhaseReachesEveryAddressRead()
    {
        const int address = 677;
        var instance = new object();
        try
        {
            FrameFixtureInputs.PinnedAddress = address;
            FrameFixtureInputs.AddressAnimationPhase = 0;
            FrameFixtureInputs.WaterVariantPhase = 0;
            Assert.Equal(address, FrameFixtureInputs.Address(instance));
            Assert.Equal(0, FrameFixtureInputs.Address(null));
            // Looped frames take the capture phase and ignore the address.
            Assert.Equal(0, FrameFixtureInputs.AnimationOffset(instance));
            // Water and raised-terrain variants take the shared capture phase.
            Assert.Equal(0, FrameFixtureInputs.WaterBasePhase(instance));
            Assert.Equal(0, FrameFixtureInputs.TextureVariant(instance));
            // Combat edges stay on the address when the water phase is pinned.
            Assert.Equal(2, FrameFixtureInputs.CombatTextureVariant(instance));

            FrameFixtureInputs.AddressAnimationPhase = null;
            FrameFixtureInputs.WaterVariantPhase = null;
            Assert.Equal(((int)((uint)RuntimeHelpers.GetHashCode(instance) >> 2)) & 65535, FrameFixtureInputs.AnimationOffset(instance));
            Assert.Equal(84, FrameFixtureInputs.WaterBasePhase(instance));
            Assert.Equal(0, FrameFixtureInputs.WaterBasePhase(null));
            Assert.Equal(2, FrameFixtureInputs.TextureVariant(instance));
            Assert.Equal(2, FrameFixtureInputs.PhaseModulo(instance, 3));
            Assert.Equal(5, FrameFixtureInputs.PhaseModulo(instance, 7));
            Assert.Equal(5, FrameFixtureInputs.ShimmerOffset(instance, 4));
            Assert.Equal(4, FrameFixtureInputs.ArmyStagger(instance));
            Assert.Equal(0, FrameFixtureInputs.ArmyStagger(null));
            Assert.Equal(2, FrameFixtureInputs.FieldFrameOffset(instance));
        }
        finally
        {
            FrameFixtureInputs.PinnedAddress = null;
            FrameFixtureInputs.AddressAnimationPhase = null;
            FrameFixtureInputs.WaterVariantPhase = null;
        }
    }
}
