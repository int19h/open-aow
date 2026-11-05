// Translated from aowFX.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;

namespace Aow.Units.aowFX
{
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
            var call__1 = new TList();
            FList = call__1;
        }

        // aowFX.pas:98
        public override void Destroy()
        {
            int I = default;
            {
                I = 0;
                int end__2 = FList.Count - 1;
                for (; I <= end__2; I += 1)
                {
                    var call__3 = GetXYZ(I);
                    call__3.Free();
                }
            }

            FList.Free();
            base.Destroy();
        }

        // aowFX.pas:108
        public void Clear()
        {
            int I = default;
            {
                I = 0;
                int end__4 = FList.Count - 1;
                for (; I <= end__4; I += 1)
                {
                    var call__5 = GetXYZ(I);
                    call__5.Free();
                }
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
            var call__6 = FList[Index];
            ((TXYZ)call__6).Free();
            FList.Delete(Index);
        }

        // aowFX.pas:128
        public TXYZ GetXYZ(int Index)
        {
            TXYZ Result = default;
            if (Index >= 0 && Index < FList.Count)
            {
                var call__7 = FList[Index];
                Result = (TXYZ)call__7;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // aowFX.pas:136
        public int GetCount()
        {
            int Result = default;
            Result = FList.Count;
            return Result;
        }

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
            TfxParticle Result = default;
            object P = default;
            var call__8 = FParticles[Index];
            P = call__8;
            Result = (TfxParticle)P;
            return Result;
        }

        // aowFX.pas:151
        public TfxParticleEffect GetEffect(int Index)
        {
            TfxParticleEffect Result = default;
            object P = default;
            var call__9 = FEffects[Index];
            P = call__9;
            Result = (TfxParticleEffect)P;
            return Result;
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
            int I = default;
            int RX = default;
            int RY = default;
            int DX = default;
            int DY = default;
            TXYZ XYZ = default;
            DX = 0;
            DY = 0;
            {
                I = 1;
                int end__10 = List.Count - 2;
                for (; I <= end__10; I += 1)
                {
                    var call__11 = List[I];
                    XYZ = call__11;
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

        // aowFX.pas:188
        public static void MakeLinePath(int X1, int Y1, int X2, int Y2, int Steps, ref TXYZList List)
        {
            int I = default;
            TXYZ XYZ = default;
            if (Steps < 2)
            {
                Steps = 2;
            }

            {
                I = 0;
                int end__12 = Steps - 1;
                for (; I <= end__12; I += 1)
                {
                    XYZ = new TXYZ();
                    List.Add(XYZ);
                    XYZ.X = X1 + (X2 - X1) * I / (Steps - 1);
                    XYZ.Y = Y1 + (Y2 - Y1) * I / (Steps - 1);
                }
            }
        }

        // aowFX.pas:204
        public static void MakeSwarmPath(int Step, int Radius, int Phase, int Angle, int X, int Y, TSwarmDirection Direction, ref TXYZList List)
        {
            int Total = default;
            int B = default;
            float Rad = default;
            float R = default;
            TXYZ XYZ = default;
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
                var call__13 = Math.Sin(Rad);
                XYZ.X = Round((double)call__13 * R) + X;
                var call__14 = Math.Cos(Rad);
                XYZ.Y = Round((double)call__14 * R) + Y;
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
            int NX = default;
            int NY = default;
            int CX = default;
            int CY = default;
            int Len = default;
            float Angle = default;
            float A2 = default;
            int Dist = default;
            int Seg = default;
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
                    var call__15 = Random(40);
                    var call__16 = DegToRad(call__15 + 40);
                    A2 = (float)((double)Angle - call__16);
                }
                else
                {
                    var call__17 = Random(40);
                    var call__18 = DegToRad(call__17 + 40);
                    A2 = (float)((double)Angle + call__18);
                }

                var call__19 = Math.Cos(Angle);
                var call__20 = Math.Cos(A2);
                NX = X1 - Round((double)call__19 * Dist) - Round((double)call__20 * Seg);
                var call__21 = Math.Sin(Angle);
                var call__22 = Math.Sin(A2);
                NY = Y1 - Round((double)call__21 * Dist) - Round((double)call__22 * Seg);
                MakeLinePath(CX, CY, NX, NY, Seg, ref List);
                CX = NX;
                CY = NY;
                Seg = Math.Abs(Round((double)Math.Cos((double)A2 - Angle) * Seg));
                Dist = Dist + Seg;
                Seg = Random(15) + 5;
            }

            MakeLinePath(CX, CY, X2, Y2, Len - Dist, ref List);
            var call__23 = List.Count;
            var call__24 = List[call__23 - 1];
            call__24.Z = 1;
        }

        // aowFX.pas:290
        // { sic: Angle is used as radians (Cos(Angle)), but the game passes degrees (Random(360)). }
        public static void MakeArcPath(int X, int Y, int Angle, ref float Z, ref float Speed, ref float VZ, ref float Gravity, ref TXYZList List)
        {
            float FX = default;
            float FY = default;
            float DX = default;
            float DY = default;
            TXYZ XYZ = default;
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
            int I = default;
            int Steps = default;
            int DX = default;
            int DY = default;
            TXYZ XYZ = default;
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
                var call__25 = Math.PI;
                var call__26 = Math.Sin((double)((double)I * call__25) / Steps);
                XYZ.Z = Round((double)call__26 * Height);
                I += 1;
            }
        }

        // aowFX.pas:245
        // { The direction of the line from (X1, Y1) to (X2, Y2) in radians, pointing backwards (the
//   lightning path subtracts it). }
        public static float GetAngle(int X1, int Y1, int X2, int Y2)
        {
            float Result = default;
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
}
