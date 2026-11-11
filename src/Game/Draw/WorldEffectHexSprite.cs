// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// { Interface declaration run owned by P2. }
// FFinished, FFrame, FX, FY, FLayer, FPath, FEffectType
public partial class WorldEffectHexSprite : FixedVisibleHexSprite
{
    public bool FDone;
    public int FFrame;
    public int FX;
    public int FY;
    public int FLayer;
    public Point3DList FPath;
    public WorldEffectType FEffectType;

    // Army.PAS:5334
    public override void Dispose()
    {
        FPath?.Dispose();
        base.Dispose();
    }

    // Army.PAS:5327
    public WorldEffectHexSprite(ECustomNode owner) : base(owner)
    {
        FPath = new Point3DList();
        FFrame = -1;
    }

    // Army.PAS:5353
    public override int GetShowPriority() => 1811939328;

    // Army.PAS:5524
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        LockedSurface SurfaceDesc__value = SurfaceDesc;
        ImageSequence Sequence;
        int Frame;
        void ShowGlobal(bool Shadow, bool Inverse)
        {
            int Brightness = default;
            var count = FPath.Count;
            Brightness = 100 - FFrame * 100 / count - FrameFixtureInputs.ShimmerOffset(this, FFrame);
            if (Brightness < 0)
            {
                Brightness = 0;
            }

            if (Shadow)
            {
                Brightness += 20;
            }

            if (Brightness > 100)
            {
                Brightness = 100;
            }

            while (Sequence != null)
            {
                // { Original uses the instance address as the animation phase. }
                Frame = Sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, 3));
                if (Frame != -1)
                {
                    if (Shadow)
                    {
                        if (Sequence.ImageLibrary.ImageExist(Frame))
                        {
                            var point = FPath[FFrame];
                            var fPathItem = FPath[FFrame];
                            var xyz = FPath[FFrame];
                            Sequence.ImageLibrary[Frame].ShowBlended(xyz.X + (SurfaceDesc__value.Width * FX / 100 + Sequence.dXshift), point.Y + (SurfaceDesc__value.Height * FY / 100 + Sequence.dYshift) - fPathItem.Z, SurfaceDesc__value, GraphicsState.ShadowBlendTable.LookupTables[Brightness]);
                        }
                    }
                    else
                    {
                        if (Inverse)
                        {
                            if (Sequence.ImageLibrary.ImageExist(Frame))
                            {
                                var item = FPath[FFrame];
                                var fPathItem = FPath[FFrame];
                                var item2 = FPath[FFrame];
                                Sequence.ImageLibrary[Frame].ShowBlended(item2.X + (SurfaceDesc__value.Width * FX / 100 + Sequence.dXshift), item.Y + (SurfaceDesc__value.Height * FY / 100 + Sequence.dYshift) - fPathItem.Z, SurfaceDesc__value, GraphicsState.IntensityBlendTable.LookupTables[100 - Brightness]);
                            }
                        }
                        else
                        {
                            if (Sequence.ImageLibrary.ImageExist(Frame))
                            {
                                var item = FPath[FFrame];
                                var fPathItem = FPath[FFrame];
                                var item2 = FPath[FFrame];
                                Sequence.ImageLibrary[Frame].ShowBlended(item2.X + (SurfaceDesc__value.Width * FX / 100 + Sequence.dXshift), item.Y + (SurfaceDesc__value.Height * FY / 100 + Sequence.dYshift) - fPathItem.Z, SurfaceDesc__value, GraphicsState.IntensityBlendTable.LookupTables[Brightness]);
                            }
                        }
                    }
                }

                Sequence = Sequence.Next;
            }
        }

        void ShowLocal(bool Shadow)
        {
            int Brightness = default;
            var count = FPath.Count;
            Brightness = 100 - FFrame * 100 / count - FrameFixtureInputs.ShimmerOffset(this, FFrame);
            if (Brightness < 0)
            {
                Brightness = 0;
            }

            if (Shadow)
            {
                Brightness += 20;
            }

            if (Brightness > 100)
            {
                Brightness = 100;
            }

            while (Sequence != null)
            {
                Frame = Sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, 3));
                if (Frame != -1)
                {
                    if (Shadow)
                    {
                        if (Sequence.ImageLibrary.ImageExist(Frame))
                        {
                            var item = FPath[FFrame];
                            var fPathItem = FPath[FFrame];
                            var item2 = FPath[FFrame];
                            Sequence.ImageLibrary[Frame].ShowBlended(item2.X + Sequence.dXshift - XhpSurf, item.Y + Sequence.dYshift - fPathItem.Z - YhpSurf, SurfaceDesc__value, GraphicsState.ShadowBlendTable.LookupTables[Brightness]);
                        }
                    }
                    else
                    {
                        if (Sequence.ImageLibrary.ImageExist(Frame))
                        {
                            var item = FPath[FFrame];
                            var fPathItem = FPath[FFrame];
                            var item2 = FPath[FFrame];
                            Sequence.ImageLibrary[Frame].ShowBlended(item2.X + Sequence.dXshift - XhpSurf, item.Y + Sequence.dYshift - fPathItem.Z - YhpSurf, SurfaceDesc__value, GraphicsState.IntensityBlendTable.LookupTables[Brightness]);
                        }
                    }
                }

                Sequence = Sequence.Next;
            }
        }

        try
        {
            if (FFrame >= 0)
            {
                switch (FEffectType)
                {
                    case WorldEffectType.EMParticle:
                        Sequence = AoWHSSet.Current.FSpellControl.GetSpell(64).FCastImages.GetImageSequence(0);
                        ShowGlobal(false, false);
                        break;
                    case WorldEffectType.WMParticle:
                        Sequence = AoWHSSet.Current.FSpellControl.GetSpell(45).FCastImages.GetImageSequence(0);
                        ShowGlobal(false, false);
                        break;
                    case WorldEffectType.WMLParticle:
                        if (AoWHSMap.Current.ViewL == FLayer)
                        {
                            Sequence = AoWHSSet.Current.FSpellControl.GetSpell(75).FCastImages.GetImageSequence(1);
                            ShowLocal(false);
                        }

                        break;
                    case WorldEffectType.HatredParticle:
                        Sequence = AoWHSSet.Current.FSpellControl.GetSpell(46).FCastImages.GetImageSequence(0);
                        ShowGlobal(true, false);
                        break;
                    case WorldEffectType.TranqParticle:
                        Sequence = AoWHSSet.Current.FSpellControl.GetSpell(47).FCastImages.GetImageSequence(0);
                        ShowGlobal(false, false);
                        break;
                    case WorldEffectType.SWParticle:
                        Sequence = AoWHSSet.Current.FSpellControl.GetSpell(65).FCastImages.GetImageSequence(1);
                        ShowGlobal(false, true);
                        break;
                    case WorldEffectType.SWBeamParticle:
                        Sequence = AoWHSSet.Current.FSpellControl.GetSpell(65).FCastImages.GetImageSequence(1);
                        ShowGlobal(false, false);
                        break;
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Show", "Error - WorldfxHS " + IntToStr((int)FEffectType), 0);
        }
    }

    // Army.PAS:5340
    public override void Activate()
    {
        base.Activate();
        AoWHSMap.Current.NewFrameEvents += NewFrame;
        FFrame = -1;
    }

    // Army.PAS:5347
    public override void Deactivate()
    {
        AoWHSMap.Current.NewFrameEvents -= NewFrame;
        base.Deactivate();
    }

    // Army.PAS:5358
    public void NewFrame(object Sender)
    {
        WorldEffectHexSprite Effect;
        int Count;
        int RadiusX;
        int RadiusY;
        float Gravity = default;
        float VZ = default;
        float Speed = default;
        float Z = default;
        try
        {
            FFrame += 1;
            switch (FEffectType)
            {
                case WorldEffectType.Hatred:
                    switch (FFrame)
                    {
                        case >= 0 and <= 89:
                            if (FFrame % 3 == 0)
                            {
                                RadiusX = 200;
                                RadiusY = 200;
                                for (int I = 0; I <= 29; I++)
                                {
                                    Effect = AoWHSMap.Current.AddEffect(WorldEffectType.HatredParticle);
                                    Effect.FX = 50;
                                    Effect.FY = 50;
                                    RadiusX = -RadiusX;
                                    var pi = Math.PI;
                                    var sin = Math.Sin((double)((double)(I * 6 + FFrame * 4) * pi) / 180);
                                    var random = Random(10);
                                    var random2 = Random(12);
                                    var mathPI = Math.PI;
                                    EffectPaths.MakeLinePath(0, 0, Round((double)Math.Cos((double)((double)(random2 + (I * 12 + FFrame * 4)) * mathPI) / 180) * (RadiusX + Random(10))), Round((double)sin * (RadiusY + random)) / 2, 75, ref Effect.FPath);
                                }
                            }

                            break;
                        case 90:
                            FFrame = -1;
                            break;
                    }

                    break;
                case WorldEffectType.Tranquility:
                    switch (FFrame)
                    {
                        case >= 0 and <= 89:
                            if (FFrame % 3 == 0)
                            {
                                RadiusX = 200;
                                RadiusY = 200;
                                for (int I = 0; I <= 29; I++)
                                {
                                    Effect = AoWHSMap.Current.AddEffect(WorldEffectType.TranqParticle);
                                    Effect.FX = 50;
                                    Effect.FY = 50;
                                    RadiusX = -RadiusX;
                                    var pi = Math.PI;
                                    var sin = Math.Sin((double)((double)(I * 6 + FFrame * 4) * pi) / 180);
                                    var random = Random(10);
                                    var random2 = Random(12);
                                    var mathPI = Math.PI;
                                    EffectPaths.MakeLinePath(0, 0, Round((double)Math.Cos((double)((double)(random2 + (I * 12 + FFrame * 4)) * mathPI) / 180) * (RadiusX + Random(10))), Round((double)sin * (RadiusY + random)) / 2, 75, ref Effect.FPath);
                                }
                            }

                            break;
                        case 90:
                            FFrame = -1;
                            break;
                    }

                    break;
                case WorldEffectType.Crusade:
                    switch (FFrame)
                    {
                        case >= 0 and <= 74:
                            if (FFrame % 3 == 0)
                            {
                                RadiusX = 150;
                                RadiusY = 150;
                                for (int I = 0; I <= 59; I++)
                                {
                                    Effect = AoWHSMap.Current.AddEffect(WorldEffectType.WMLParticle);
                                    Effect.FLayer = FLayer;
                                    RadiusX = -RadiusX;
                                    var pi = Math.PI;
                                    var sin = Math.Sin((double)((double)(I * 6 + FFrame * 4) * pi) / 180);
                                    var random = Random(10);
                                    var mathPI = Math.PI;
                                    var cos = Math.Cos((double)((double)(I * 6 + FFrame * 4) * mathPI) / 180);
                                    var random2 = Random(10);
                                    EffectPaths.MakeLinePath(FX, FY, FX + Round((double)cos * (RadiusX + random2)), FY + Round((double)sin * (RadiusY + random)) / 2, 50, ref Effect.FPath);
                                }
                            }

                            break;
                        case 75:
                            FFrame = -1;
                            break;
                    }

                    break;
                case WorldEffectType.WaterMastery:
                    switch (FFrame)
                    {
                        case >= 0 and <= 199:
                            if (FFrame % 5 == 0)
                            {
                                AoWHSSet.Current.FSpellControl.GetSpell(45).FCastSounds.Play(1);
                                Count = Random(10) + 100;
                                for (int J = 0; J <= Count; J++)
                                {
                                    Effect = AoWHSMap.Current.AddEffect(WorldEffectType.WMParticle);
                                    Effect.FX = 50;
                                    Effect.FY = 50;
                                    Speed = (float)((double)(Random(50) + 50) * 0.1);
                                    Gravity = (float)0.1;
                                    VZ = (float)((double)(Random(5) + 5) * 0.1);
                                    Z = 0;
                                    EffectPaths.MakeArcPath(0, 0, Random(360), ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                }
                            }

                            {
                                for (int J = 0; J <= 1; J++)
                                {
                                    Count = Random(3) + 7;
                                    for (int I = 1; I <= Count - 1; I++)
                                    {
                                        Effect = AoWHSMap.Current.AddEffect(WorldEffectType.WMParticle);
                                        var pi = Math.PI;
                                        Effect.FX = 50 + Round((double)Math.Cos((double)((double)((double)(FFrame * 360) * 0.005) * pi) / 180) * FFrame) / 4;
                                        var mathPI = Math.PI;
                                        Effect.FY = 50 + Round((double)Math.Sin((double)((double)((double)(FFrame * 360) * 0.005) * mathPI) / 180) * FFrame) / 8;
                                        if (J == 1)
                                        {
                                            Effect.FX = 100 - Effect.FX;
                                            Effect.FY = 100 - Effect.FY;
                                        }

                                        Speed = (float)((double)(Random(10) + 30) * 0.1);
                                        Gravity = (float)0.5;
                                        VZ = (float)((double)(Random(50) + FFrame / 2) * 0.1);
                                        Z = 0;
                                        EffectPaths.MakeArcPath(0, 0, I * 360 / Count, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }
                            }

                            break;
                        case 200:
                            FFrame = -1;
                            break;
                    }

                    break;
                case WorldEffectType.SpellWard:
                    switch (FFrame)
                    {
                        case >= 0 and <= 14:
                            RadiusX = 200;
                            RadiusY = 200;
                            {
                                for (int I = 0; I <= 29; I++)
                                {
                                    Effect = AoWHSMap.Current.AddEffect(WorldEffectType.SWParticle);
                                    Effect.FX = 50;
                                    Effect.FY = 50;
                                    RadiusX = -RadiusX;
                                    var pi = Math.PI;
                                    var sin = Math.Sin((double)((double)(I * 12 + FFrame * 5) * pi) / 180);
                                    var random = Random(10);
                                    var mathPI = Math.PI;
                                    EffectPaths.MakeArcPathXY((short)Round((double)Math.Cos((double)((double)(I * 12 + FFrame * 5) * mathPI) / 180) * (RadiusX + Random(10))), (short)(Round((double)sin * (RadiusY + random)) / 2), 0, -150, 100, 10, ref Effect.FPath);
                                    for (int J = 0, last = Effect.FPath.Count - 1; J <= last; J++)
                                    {
                                        var point = Effect.FPath[J];
                                        Effect.FPath[J].Z = -point.Z;
                                    }
                                }
                            }

                            break;
                    }

                    switch (FFrame)
                    {
                        case >= 15 and <= 44:
                            if (FFrame % 5 == 0)
                            {
                                Count = Random(10) + 100;
                                for (int J = 0; J <= Count; J++)
                                {
                                    Effect = AoWHSMap.Current.AddEffect(WorldEffectType.SWBeamParticle);
                                    Effect.FX = 50;
                                    Effect.FY = 50;
                                    Speed = (float)((double)(Random(50) + 50) * 0.1);
                                    Gravity = (float)0.1;
                                    VZ = (float)((double)(Random(5) + 5) * 0.1);
                                    Z = 0;
                                    EffectPaths.MakeArcPath(0, -150, Random(360), ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                }
                            }

                            break;
                        case 120:
                            FFrame = -1;
                            break;
                    }

                    break;
                case WorldEffectType.EarthMastery:
                    switch (FFrame)
                    {
                        case >= 0 and <= 49:
                            {
                                for (int I = 0; I <= 29; I++)
                                {
                                    Effect = AoWHSMap.Current.AddEffect(WorldEffectType.EMParticle);
                                    RadiusX = Random(12);
                                    var pi = Math.PI;
                                    Effect.FX = Round((double)Math.Cos((double)((double)(I * 12 + RadiusX) * pi) / 180) * FFrame) + 50;
                                    var mathPI = Math.PI;
                                    Effect.FY = Round((double)Math.Sin((double)((double)(I * 12 + RadiusX) * mathPI) / 180) * FFrame) / 2 + 50;
                                    EffectPaths.MakeLinePath(0, 0, 0, FFrame * 3 - 200, 30, ref Effect.FPath);
                                }
                            }

                            break;
                        case 200:
                            FFrame = -1;
                            break;
                    }

                    break;
                case WorldEffectType.WMParticle:
                case WorldEffectType.WMLParticle:
                case WorldEffectType.HatredParticle:
                case WorldEffectType.SWParticle:
                case WorldEffectType.SWBeamParticle:
                case WorldEffectType.EMParticle:
                case WorldEffectType.TranqParticle:
                    var count = FPath.Count;
                    if (FFrame >= count - 1)
                    {
                        FFrame = FPath.Count - 1;
                        FDone = true;
                    }

                    break;
            }

            if (FFrame == -1)
            {
                FDone = true;
            }
        }
        catch
        {
            MessageBox(0, "Error - WorldfxHS " + IntToStr((int)FEffectType), "Error during NewFrame", 0);
        }
    }

    // intf_P2_01.inc:17

}
