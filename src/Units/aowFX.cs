using Aow.Engine.Buffers;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public enum SwarmDirection : byte
{
    Forward = 0,
    Backward = 1
}

// one point, X, Y, Z
public partial class XYZ
{
    public int X;
    public int Y;
    public int Z;
}

// FList, the owned points
public partial class XYZList : IDisposable
{

    internal DelphiList FList;

    // aowFX.pas:93
    public XYZList()
    {
        FList = new DelphiList();
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
    public void Add(XYZ XYZ)
    {
        FList.Add(XYZ);
    }

    // aowFX.pas:122
    public void Delete(int Index)
    {
        FList.Delete(Index);
    }

    // aowFX.pas:128
    public XYZ GetXYZ(int Index)
    {
        XYZ Result;
        if (Index >= 0 && Index < FList.Count)
        {
            Result = (XYZ)FList[Index];
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

    public XYZ this[int Index] { get => GetXYZ(Index); }

    internal static void ScatterPath(ref XYZList List)
    {
        int RX;
        int RY;
        int DX;
        int DY;
        XYZ XYZ;
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

// no method of the package touches its fields, so only their size is known
public partial class TfxParticleObject
{
    internal FixedArray_int_10 FData;
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
