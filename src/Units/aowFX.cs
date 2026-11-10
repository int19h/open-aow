// Translated from aowFX.pas.
using Aow.Game.Engine.Buffers;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public enum TSwarmDirection : byte
{
    sdForward = 0,
    sdBackward = 1
}

[LegacyObject]
// one point, X, Y, Z
public partial class TXYZ
{
    public int X;
    public int Y;
    public int Z;

    public TXYZ()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
// FList, the owned points
public partial class TXYZList : IDisposable
{

    internal TList FList;

    // aowFX.pas:93
    public TXYZList()
    {
        CaptureAddress.Assign(this);
        FList = new TList();
    }

    // aowFX.pas:98
    public virtual void Dispose()
    {
        for (int I = 0, last = FList.Count - 1; I <= last; I++)
        {
            _ = GetXYZ(I);
        }

        FList?.Dispose();
    }

    // aowFX.pas:108
    public void Clear()
    {
        for (int I = 0, last = FList.Count - 1; I <= last; I++)
        {
            _ = GetXYZ(I);
        }

        FList.Clear();
    }

    // aowFX.pas:117
    public void Add(TXYZ XYZ)
    {
        FList.Add(XYZ);
    }

    // aowFX.pas:122
    public void Delete(int Index)
    {
        FList.Delete(Index);
    }

    // aowFX.pas:128
    public TXYZ GetXYZ(int Index)
    {
        TXYZ Result;
        if (Index >= 0 && Index < FList.Count)
        {
            Result = (TXYZ)FList[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // aowFX.pas:136
    public int GetCount() => FList.Count;

    public int Count { get => GetCount(); }

    public TXYZ this[int Index] { get => GetXYZ(Index); }

    internal static void ScatterPath(ref TXYZList List)
    {
        int RX;
        int RY;
        int DX;
        int DY;
        TXYZ XYZ;
        DX = 0;
        DY = 0;
        for (int I = 1, last = List.Count - 2; I <= last; I++)
        {
            XYZ = List[I];
            RX = Random(3);
            RY = Random(3);
            if (RX - DX > 1)
            {
                RX = DX + 1;
            }

            if (DX - RX > 1)
            {
                RX = DX - 1;
            }

            if (RY - DY > 1)
            {
                RY = DY + 1;
            }

            if (DY - RY > 1)
            {
                RY = DY - 1;
            }

            XYZ.X = XYZ.X + RX - 1;
            XYZ.Y = XYZ.Y + RY - 1;
            DX = RX;
            DY = RY;
        }
    }

}

[LegacyObject]
// no method of the package touches its fields, so only their size is known
public partial class TfxParticleObject
{
    internal FixedArray_int_10 FData;

    public TfxParticleObject()
    {
        CaptureAddress.Assign(this);
    }
}

// no fields of its own
public partial class TfxParticle : TfxParticleObject
{
}

// no fields of its own
public partial class TfxParticleEffect : TfxParticleObject
{
}

// FParticles, FEffects (never created by this unit)
public partial class TfxParticleSystem : TfxParticleObject
{
    internal TList FParticles;
    internal TList FEffects;

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
