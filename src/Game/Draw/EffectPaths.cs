using Aow.Game.Combat;

using Aow.Game.World.Map;

using Aow.Game.World.Terrain;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

internal static class EffectPaths
{

    internal static void MakeLinePath(int X1, int Y1, int X2, int Y2, int Steps, ref Point3DList List)
    {
        Point3D XYZ;
        if (Steps < 2)
        {
            Steps = 2;
        }

        for (int I = 0; I <= Steps - 1; I++)
        {
            XYZ = new Point3D();
            List.Add(XYZ);
            XYZ.X = X1 + (X2 - X1) * I / (Steps - 1);
            XYZ.Y = Y1 + (Y2 - Y1) * I / (Steps - 1);
        }
    }

    internal static void MakeSwarmPath(int Step, int Radius, int Phase, int Angle, int X, int Y, SwarmDirection Direction, ref Point3DList List)
    {
        int Total;
        int B;
        float Rad;
        float R;
        Point3D XYZ;
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

            XYZ = new Point3D();
            XYZ.X = Round((double)Math.Sin(Rad) * R) + X;
            XYZ.Y = Round((double)Math.Cos(Rad) * R) + Y;
            List.Add(XYZ);
            switch (Direction)
            {
                case SwarmDirection.Forward:
                    Angle = Angle + Step;
                    if (Angle >= 360)
                    {
                        Angle = Angle - 360;
                    }

                    break;
                case SwarmDirection.Backward:
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

    internal static void MakeLightningPath(int X1, int Y1, int X2, int Y2, ref Point3DList List)
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

    internal static void MakeArcPath(int X, int Y, int Angle, ref float Z, ref float Speed, ref float VZ, ref float Gravity, ref Point3DList List)
    {
        float FX;
        float FY;
        float DX;
        float DY;
        Point3D XYZ;
        FX = X;
        FY = Y;
        DX = (float)((double)Math.Cos(Angle) * Speed);
        DY = (float)((double)Math.Sin(Angle) * Speed);
        if ((double)Gravity > 0)
        {
            while ((double)Z >= 0)
            {
                XYZ = new Point3D();
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

    internal static void MakeArcPathXY(short X1, short Y1, short X2, short Y2, short StepSize, short Height, ref Point3DList List)
    {
        int I;
        int Steps;
        int DX;
        int DY;
        Point3D XYZ;
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
            XYZ = new Point3D();
            List.Add(XYZ);
            XYZ.X = X1 + (X2 - X1) * I / Steps;
            XYZ.Y = Y1 + (Y2 - Y1) * I / Steps;
            var pi = Math.PI;
            XYZ.Z = Round((double)Math.Sin((double)((double)I * pi) / Steps) * Height);
            I += 1;
        }
    }

    internal static float GetAngle(int X1, int Y1, int X2, int Y2)
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

    internal static void CreateFire(bool Spread, int Chance, int Xhx, int Yhx)
    {
        CombatFire Fire;
        bool CanBurn;
        CombatImageLibraryTerrain Terrain;
        CityWall Wall;
        if (CombatMap.Current.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).TerrainType != 15)
        {
            if (CombatMap.Current.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).FindOwnedChild(2229578) == null)
            {
                if (AoWHSMap.Current.Random(100) < Chance)
                {
                    CanBurn = false;
                    if (CombatMap.Current.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).GetDecorationType() == 9 || CombatMap.Current.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).GetDecorationType() == 1 || CombatMap.Current.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).GetDecorationType() == 2 || CombatMap.Current.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).GetDecorationType() == 3)
                    {
                        CanBurn = true;
                    }
                    else
                    {
                        Terrain = (CombatImageLibraryTerrain)CombatMap.Current.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).FindOwnedChild(2228534);
                        if (Terrain != null)
                        {
                            if (Terrain.GetBurnValue() > 0)
                            {
                                CanBurn = true;
                            }
                        }

                        Wall = (CityWall)CombatMap.Current.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).FindOwnedChild(2229508);
                        if (Wall != null)
                        {
                            if (Wall.FWallType == 1)
                            {
                                if (Wall.FHitPoints > 0)
                                {
                                    CanBurn = true;
                                }
                            }
                        }
                    }

                    if (CanBurn)
                    {
                        Fire = new CombatFire(null);
                        if (!CombatMap.Current.PlaceHX(Fire, Xhx, Yhx, 0))
                        {
                            Fire?.Dispose();
                        }
                    }
                }
            }
        }
    }

}
