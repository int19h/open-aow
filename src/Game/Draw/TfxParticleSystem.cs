
using Aow.Rtl;

namespace Aow.Game.Draw;

// FParticles, FEffects (never created by this unit)
public partial class TfxParticleSystem : TfxParticleObject
{
    internal DelphiList FParticles;
    internal DelphiList FEffects;

    // aowFX.pas:143
    public TfxParticle GetParticle(int Index)
    {
        object P;
        P = FParticles[Index];
        return (TfxParticle)P;
    }

    // aowFX.pas:151
    public TfxParticleEffect GetEffect(int Index)
    {
        object P;
        P = FEffects[Index];
        return (TfxParticleEffect)P;
    }

    public sealed class ParticlesAccessor(TfxParticleSystem target)
    {
        public TfxParticle this[int Index] { get => target.GetParticle(Index); }
    }

    public ParticlesAccessor Particles => new ParticlesAccessor(this);

    public sealed class EffectsAccessor(TfxParticleSystem target)
    {
        public TfxParticleEffect this[int Index] { get => target.GetEffect(Index); }
    }

    public EffectsAccessor Effects => new EffectsAccessor(this);
}
