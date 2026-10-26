// Generated from aowFX.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aow.Units.aowFX
{
    public enum TSwarmDirection : byte
    {
        sdForward = 0,
        sdBackward = 1
    }

    // { aowFX.TXYZ (instance size 16): one point, +4 X, +8 Y, +$C Z. }
    public partial class TXYZ : Aow.Rtl.TObject
    {
        public int X;
        public int Y;
        public int Z;
        // System.pas:111
        public static Aow.Units.aowFX.TXYZ Create()
        {
            return new Aow.Units.aowFX.TXYZ();
        }
    }

    // { aowFX.TXYZList (instance size 8): +4 FList, the owned points. }
    public partial class TXYZList : Aow.Rtl.TObject
    {
        internal Aow.Rtl.TList FList;
        // aowFX.pas:93
        // { TXYZList }
        public void Create__init_TXYZList()
        {
            var call__1 = new Aow.Rtl.TList();
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
        public void Add(Aow.Units.aowFX.TXYZ XYZ)
        {
            FList.Add(XYZ);
        }

        // aowFX.pas:122
        public void Delete(int Index)
        {
            var call__6 = FList[Index];
            ((Aow.Units.aowFX.TXYZ)call__6).Free();
            FList.Delete(Index);
        }

        // aowFX.pas:128
        public Aow.Units.aowFX.TXYZ GetXYZ(int Index)
        {
            Aow.Units.aowFX.TXYZ Result = default;
            if ((Index >= 0) && (Index < FList.Count))
            {
                var call__7 = FList[Index];
                Result = (Aow.Units.aowFX.TXYZ)call__7;
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

        public Aow.Units.aowFX.TXYZ this[int Index] { get => GetXYZ(Index); }

        // aowFX.pas:41
        public static Aow.Units.aowFX.TXYZList Create()
        {
            Aow.Units.aowFX.TXYZList o = new Aow.Units.aowFX.TXYZList();
            try
            {
                o.Create__init_TXYZList();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { aowFX.TfxParticleObject (instance size 44): no method of the package touches its fields, so
//     only their size (+4..+$2B) is known. }
    public partial class TfxParticleObject : Aow.Rtl.TObject
    {
        internal Aow.Units.FixedArray_int_10 FData;
    }

    // { aowFX.TfxParticle (instance size 44), no fields of its own. }
    public partial class TfxParticle : Aow.Units.aowFX.TfxParticleObject
    {
    }

    // { aowFX.TfxParticleEffect (instance size 44), no fields of its own. }
    public partial class TfxParticleEffect : Aow.Units.aowFX.TfxParticleObject
    {
    }

    // { aowFX.TfxParticleSystem (instance size 52): +$2C FParticles, +$30 FEffects (never created by
//     this unit). }
    public partial class TfxParticleSystem : Aow.Units.aowFX.TfxParticleObject
    {
        internal Aow.Rtl.TList FParticles;
        internal Aow.Rtl.TList FEffects;
        // aowFX.pas:143
        // { TfxParticleSystem }
        public Aow.Units.aowFX.TfxParticle GetParticle(int Index)
        {
            Aow.Units.aowFX.TfxParticle Result = default;
            object P = default;
            var call__8 = FParticles[Index];
            P = call__8;
            Result = (Aow.Units.aowFX.TfxParticle)P;
            return Result;
        }

        // aowFX.pas:151
        public Aow.Units.aowFX.TfxParticleEffect GetEffect(int Index)
        {
            Aow.Units.aowFX.TfxParticleEffect Result = default;
            object P = default;
            var call__9 = FEffects[Index];
            P = call__9;
            Result = (Aow.Units.aowFX.TfxParticleEffect)P;
            return Result;
        }

        public sealed class ParticlesAccessor(Aow.Units.aowFX.TfxParticleSystem target)
        {
            public Aow.Units.aowFX.TfxParticle this[int Index] { get => target.GetParticle(Index); }
        }

        public ParticlesAccessor Particles => new ParticlesAccessor(this);

        public sealed class EffectsAccessor(Aow.Units.aowFX.TfxParticleSystem target)
        {
            public Aow.Units.aowFX.TfxParticleEffect this[int Index] { get => target.GetEffect(Index); }
        }

        public EffectsAccessor Effects => new EffectsAccessor(this);
    }

    public static partial class aowFX
    {
        // aowFX.pas:161
        // { paths }
        public static void ScatterPath(ref Aow.Units.aowFX.TXYZList List)
        {
            int I = default;
            int RX = default;
            int RY = default;
            int DX = default;
            int DY = default;
            Aow.Units.aowFX.TXYZ XYZ = default;
            DX = 0;
            DY = 0;
            {
                I = 1;
                int end__10 = List.Count - 2;
                for (; I <= end__10; I += 1)
                {
                    var call__11 = List[I];
                    XYZ = call__11;
                    RX = Aow.Rtl.Rtl.Random(3);
                    RY = Aow.Rtl.Rtl.Random(3);
                    if ((RX - DX) > 1)
                    {
                        RX = DX + 1;
                    }

                    if ((DX - RX) > 1)
                    {
                        RX = DX - 1;
                    }

                    if ((RY - DY) > 1)
                    {
                        RY = DY + 1;
                    }

                    if ((DY - RY) > 1)
                    {
                        RY = DY - 1;
                    }

                    XYZ.X = (XYZ.X + RX) - 1;
                    XYZ.Y = (XYZ.Y + RY) - 1;
                    DX = RX;
                    DY = RY;
                }
            }
        }

        // aowFX.pas:188
        public static void MakeLinePath(int X1, int Y1, int X2, int Y2, int Steps, ref Aow.Units.aowFX.TXYZList List)
        {
            int I = default;
            Aow.Units.aowFX.TXYZ XYZ = default;
            if (Steps < 2)
            {
                Steps = 2;
            }

            {
                I = 0;
                int end__12 = Steps - 1;
                for (; I <= end__12; I += 1)
                {
                    XYZ = Aow.Units.aowFX.TXYZ.Create();
                    List.Add(XYZ);
                    XYZ.X = X1 + (((X2 - X1) * I) / (Steps - 1));
                    XYZ.Y = Y1 + (((Y2 - Y1) * I) / (Steps - 1));
                }
            }
        }

        // aowFX.pas:204
        public static void MakeSwarmPath(int Step, int Radius, int Phase, int Angle, int X, int Y, Aow.Units.aowFX.TSwarmDirection Direction, ref Aow.Units.aowFX.TXYZList List)
        {
            int Total = default;
            int B = default;
            float Rad = default;
            float R = default;
            Aow.Units.aowFX.TXYZ XYZ = default;
            Total = 0;
            do
            {
                Rad = (float)(((double)(Angle * System.Math.PI)) / 180);
                B = (Phase + Angle) % 180;
                switch (B)
                {
                    case >= 0 and <= 89:
                        R = (float)((((((double)B) / 90) * (((double)B) / 90)) * Radius) + Radius);
                        break;
                    default:
                        R = (float)((((((double)(180 - B)) / 90) * (((double)(180 - B)) / 90)) * Radius) + Radius);
                        break;
                }

                XYZ = Aow.Units.aowFX.TXYZ.Create();
                var call__13 = System.Math.Sin(Rad);
                XYZ.X = Aow.Rtl.Rtl.Round(call__13 * R) + X;
                var call__14 = System.Math.Cos(Rad);
                XYZ.Y = Aow.Rtl.Rtl.Round(call__14 * R) + Y;
                List.Add(XYZ);
                switch (Direction)
                {
                    case Aow.Units.aowFX.TSwarmDirection.sdForward:
                        Angle = Angle + Step;
                        if (Angle >= 360)
                        {
                            Angle = Angle - 360;
                        }

                        break;
                    case Aow.Units.aowFX.TSwarmDirection.sdBackward:
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
        public static void MakeLightningPath(int X1, int Y1, int X2, int Y2, ref Aow.Units.aowFX.TXYZList List)
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
            Angle = Aow.Units.aowFX.aowFX.GetAngle(X1, Y1, X2, Y2);
            A2 = Angle;
            Len = Aow.Rtl.Rtl.Round(System.Math.Sqrt(((X2 - X1) * (X2 - X1)) + ((Y2 - Y1) * (Y2 - Y1))));
            Dist = 0;
            Seg = Aow.Rtl.Rtl.Random(15) + 5;
            while ((Dist + Seg) < Len)
            {
                if (A2 > Angle)
                {
                    var call__15 = Aow.Rtl.Rtl.Random(40);
                    var call__16 = Aow.Rtl.Rtl.DegToRad(call__15 + 40);
                    A2 = (float)(Angle - call__16);
                }
                else
                {
                    var call__17 = Aow.Rtl.Rtl.Random(40);
                    var call__18 = Aow.Rtl.Rtl.DegToRad(call__17 + 40);
                    A2 = (float)(Angle + call__18);
                }

                var call__19 = System.Math.Cos(Angle);
                var call__20 = System.Math.Cos(A2);
                NX = (X1 - Aow.Rtl.Rtl.Round(call__19 * Dist)) - Aow.Rtl.Rtl.Round(call__20 * Seg);
                var call__21 = System.Math.Sin(Angle);
                var call__22 = System.Math.Sin(A2);
                NY = (Y1 - Aow.Rtl.Rtl.Round(call__21 * Dist)) - Aow.Rtl.Rtl.Round(call__22 * Seg);
                Aow.Units.aowFX.aowFX.MakeLinePath(CX, CY, NX, NY, Seg, ref List);
                CX = NX;
                CY = NY;
                Seg = System.Math.Abs(Aow.Rtl.Rtl.Round(System.Math.Cos(A2 - Angle) * Seg));
                Dist = Dist + Seg;
                Seg = Aow.Rtl.Rtl.Random(15) + 5;
            }

            Aow.Units.aowFX.aowFX.MakeLinePath(CX, CY, X2, Y2, Len - Dist, ref List);
            var call__23 = List.Count;
            var call__24 = List[call__23 - 1];
            call__24.Z = 1;
        }

        // aowFX.pas:290
        // { sic: Angle is used as radians (Cos(Angle)), but the game passes degrees (Random(360)). }
        public static void MakeArcPath(int X, int Y, int Angle, ref float Z, ref float Speed, ref float VZ, ref float Gravity, ref Aow.Units.aowFX.TXYZList List)
        {
            float FX = default;
            float FY = default;
            float DX = default;
            float DY = default;
            Aow.Units.aowFX.TXYZ XYZ = default;
            FX = X;
            FY = Y;
            DX = (float)(System.Math.Cos(Angle) * Speed);
            DY = (float)(System.Math.Sin(Angle) * Speed);
            if (Gravity > 0)
            {
                while (Z >= 0)
                {
                    XYZ = Aow.Units.aowFX.TXYZ.Create();
                    XYZ.X = Aow.Rtl.Rtl.Round(FX);
                    XYZ.Y = Aow.Rtl.Rtl.Round(FY);
                    XYZ.Z = Aow.Rtl.Rtl.Round(Z);
                    List.Add(XYZ);
                    FX = (float)(FX + DX);
                    FY = (float)(FY + DY);
                    VZ = (float)(VZ - Gravity);
                    Z = (float)(Z + VZ);
                }
            }
        }

        // aowFX.pas:315
        public static void MakeArcPathXY(short X1, short Y1, short X2, short Y2, short StepSize, short Height, ref Aow.Units.aowFX.TXYZList List)
        {
            int I = default;
            int Steps = default;
            int DX = default;
            int DY = default;
            Aow.Units.aowFX.TXYZ XYZ = default;
            I = 0;
            DX = System.Math.Abs(X2 - X1);
            DY = System.Math.Abs(Y2 - Y1);
            Steps = (DX + DY) / StepSize;
            if (Steps < 2)
            {
                Steps = 2;
            }

            while (I <= Steps)
            {
                XYZ = Aow.Units.aowFX.TXYZ.Create();
                List.Add(XYZ);
                XYZ.X = X1 + (((X2 - X1) * I) / Steps);
                XYZ.Y = Y1 + (((Y2 - Y1) * I) / Steps);
                var call__25 = System.Math.PI;
                var call__26 = System.Math.Sin(((double)(I * call__25)) / Steps);
                XYZ.Z = Aow.Rtl.Rtl.Round(call__26 * Height);
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
                Result = (float)(((double)System.Math.PI) / 2);
            }
            else
            {
                Result = (float)System.Math.Atan(((double)(Y2 - Y1)) / (X2 - X1));
            }

            if (X2 > X1)
            {
                Result = (float)(Result + System.Math.PI);
            }
            else
            {
                if ((Y1 < Y2) && (X2 == X1))
                {
                    Result = (float)(Result + System.Math.PI);
                }
            }

            return Result;
        }
    }
}
