// Translated from aowFX.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;

namespace Aow.Units.aowFX;

public enum TSwarmDirection : byte
{
    sdForward = 0,
    sdBackward = 1
}

// one point, X, Y, Z
public partial class TXYZ : TObject
{
    public int X;
    public int Y;
    public int Z;
}

// FList, the owned points
public partial class TXYZList : TObject
{

    internal TList FList;
    // aowFX.pas:93
    public TXYZList()
    {
        FList = new TList();
    }

    // aowFX.pas:98
    public override void Destroy()
    {
        for (int I = 0, last = FList.Count - 1; I <= last; I++)
        {
            GetXYZ(I).Free();
        }

        FList.Free();
        base.Destroy();
    }

    // aowFX.pas:108
    public void Clear()
    {
        for (int I = 0, last = FList.Count - 1; I <= last; I++)
        {
            GetXYZ(I).Free();
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
        ((TXYZ)FList[Index]).Free();
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

}

// no method of the package touches its fields, so only their size is known
public partial class TfxParticleObject : TObject
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

public static partial class aowFX
{
    // aowFX.pas:161
    // { paths }
    public static void ScatterPath(ref TXYZList List)
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

    // aowFX.pas:188
    public static void MakeLinePath(int X1, int Y1, int X2, int Y2, int Steps, ref TXYZList List)
    {
        TXYZ XYZ;
        if (Steps < 2)
        {
            Steps = 2;
        }

        for (int I = 0; I <= Steps - 1; I++)
        {
            XYZ = new TXYZ();
            List.Add(XYZ);
            XYZ.X = X1 + (X2 - X1) * I / (Steps - 1);
            XYZ.Y = Y1 + (Y2 - Y1) * I / (Steps - 1);
        }
    }

    // aowFX.pas:204
    public static void MakeSwarmPath(int Step, int Radius, int Phase, int Angle, int X, int Y, TSwarmDirection Direction, ref TXYZList List)
    {
        int Total;
        int B;
        float Rad;
        float R;
        TXYZ XYZ;
        Total = 0;
        do
        {
            Rad = (float)((double)((double)Angle * Math.PI) / 180);
            B = (Phase + Angle) % 180;
            switch (B)
            {
                case >= 0 and <= 89:
                    R = (float)((double)((double)((double)B / 90 * ((double)B / 90)) * Radius) + Radius);
                    break;
                default:
                    R = (float)((double)((double)((double)(180 - B) / 90 * ((double)(180 - B) / 90)) * Radius) + Radius);
                    break;
            }

            XYZ = new TXYZ();
            XYZ.X = Round((double)Math.Sin(Rad) * R) + X;
            XYZ.Y = Round((double)Math.Cos(Rad) * R) + Y;
            List.Add(XYZ);
            switch (Direction)
            {
                case TSwarmDirection.sdForward:
                    Angle = Angle + Step;
                    if (Angle >= 360)
                    {
                        Angle = Angle - 360;
                    }

                    break;
                case TSwarmDirection.sdBackward:
                    Angle = Angle - Step;
                    // { sic: tests 360 instead of 0, so the angle is never wrapped back }
                    if (Angle < 360)
                    {
                        Angle = Angle + 360;
                    }

                    break;
            }

            Total = Total + Step;
        }
        while (!(Total >= 360));
    }

    // aowFX.pas:257
    public static void MakeLightningPath(int X1, int Y1, int X2, int Y2, ref TXYZList List)
    {
        int NX;
        int NY;
        int CX;
        int CY;
        int Len;
        float Angle;
        float A2;
        int Dist;
        int Seg;
        CX = X1;
        CY = Y1;
        Angle = GetAngle(X1, Y1, X2, Y2);
        A2 = Angle;
        Len = Round(Math.Sqrt((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1)));
        Dist = 0;
        Seg = Random(15) + 5;
        while (Dist + Seg < Len)
        {
            if ((double)A2 > Angle)
            {
                A2 = (float)((double)Angle - DegToRad(Random(40) + 40));
            }
            else
            {
                A2 = (float)((double)Angle + DegToRad(Random(40) + 40));
            }

            var cos = Math.Cos(Angle);
            var mathCos = Math.Cos(A2);
            NX = X1 - Round((double)cos * Dist) - Round((double)mathCos * Seg);
            var sin = Math.Sin(Angle);
            var mathSin = Math.Sin(A2);
            NY = Y1 - Round((double)sin * Dist) - Round((double)mathSin * Seg);
            MakeLinePath(CX, CY, NX, NY, Seg, ref List);
            CX = NX;
            CY = NY;
            Seg = Math.Abs(Round((double)Math.Cos((double)A2 - Angle) * Seg));
            Dist = Dist + Seg;
            Seg = Random(15) + 5;
        }

        MakeLinePath(CX, CY, X2, Y2, Len - Dist, ref List);
        var count = List.Count;
        List[count - 1].Z = 1;
    }

    // aowFX.pas:290
    // { sic: Angle is used as radians (Cos(Angle)), but the game passes degrees (Random(360)). }
    public static void MakeArcPath(int X, int Y, int Angle, ref float Z, ref float Speed, ref float VZ, ref float Gravity, ref TXYZList List)
    {
        float FX;
        float FY;
        float DX;
        float DY;
        TXYZ XYZ;
        FX = X;
        FY = Y;
        DX = (float)((double)Math.Cos(Angle) * Speed);
        DY = (float)((double)Math.Sin(Angle) * Speed);
        if ((double)Gravity > 0)
        {
            while ((double)Z >= 0)
            {
                XYZ = new TXYZ();
                XYZ.X = Round(FX);
                XYZ.Y = Round(FY);
                XYZ.Z = Round(Z);
                List.Add(XYZ);
                FX = (float)((double)FX + DX);
                FY = (float)((double)FY + DY);
                VZ = (float)((double)VZ - Gravity);
                Z = (float)((double)Z + VZ);
            }
        }
    }

    // aowFX.pas:315
    public static void MakeArcPathXY(short X1, short Y1, short X2, short Y2, short StepSize, short Height, ref TXYZList List)
    {
        int I;
        int Steps;
        int DX;
        int DY;
        TXYZ XYZ;
        I = 0;
        DX = Math.Abs(X2 - X1);
        DY = Math.Abs(Y2 - Y1);
        Steps = (DX + DY) / StepSize;
        if (Steps < 2)
        {
            Steps = 2;
        }

        while (I <= Steps)
        {
            XYZ = new TXYZ();
            List.Add(XYZ);
            XYZ.X = X1 + (X2 - X1) * I / Steps;
            XYZ.Y = Y1 + (Y2 - Y1) * I / Steps;
            var pi = Math.PI;
            XYZ.Z = Round((double)Math.Sin((double)((double)I * pi) / Steps) * Height);
            I += 1;
        }
    }

    // aowFX.pas:245
    // { The direction of the line from (X1, Y1) to (X2, Y2) in radians, pointing backwards (the
//   lightning path subtracts it). }
    public static float GetAngle(int X1, int Y1, int X2, int Y2)
    {
        float Result;
        if (X1 == X2)
        {
            Result = (float)((double)Math.PI / 2);
        }
        else
        {
            Result = (float)Math.Atan((double)(Y2 - Y1) / (X2 - X1));
        }

        if (X2 > X1)
        {
            Result = (float)((double)Result + Math.PI);
        }
        else
        {
            if (Y1 < Y2 && X2 == X1)
            {
                Result = (float)((double)Result + Math.PI);
            }
        }

        return Result;
    }
}
