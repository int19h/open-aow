// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// { Interface declaration run owned by P2. }
// FFinished, Frame, FX, FY, Layer, Path, EffectType
public partial class WorldEffectHexSprite : FixedVisibleHexSprite
{
    public bool Done;
    public int Frame;
    public int X;
    public int Y;
    public int Layer;
    public Point3DList Path;
    public WorldEffectType EffectType;

    // Army.PAS:5334
    public override void Dispose()
    {
        Path?.Dispose();
        base.Dispose();
    }

    // Army.PAS:5327
    public WorldEffectHexSprite(ECustomNode owner) : base(owner)
    {
        Path = new Point3DList();
        Frame = -1;
    }

    // Army.PAS:5353
    public override int GetShowPriority() => 1811939328;

    // Army.PAS:5524
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        LockedSurface lockedSurfaceValue = lockedSurface;
        ImageSequence sequence;
        int frame;
        void ShowGlobal(bool shadow, bool inverse)
        {
            int brightness = default;
            var count = Path.Count;
            brightness = 100 - Frame * 100 / count - FrameFixtureInputs.ShimmerOffset(this, Frame);
            if (brightness < 0)
            {
                brightness = 0;
            }

            if (shadow)
            {
                brightness += 20;
            }

            if (brightness > 100)
            {
                brightness = 100;
            }

            while (sequence != null)
            {
                // { Original uses the instance address as the animation phase. }
                frame = sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, 3));
                if (frame != -1)
                {
                    if (shadow)
                    {
                        if (sequence.ImageLibrary.ImageExist(frame))
                        {
                            var point = Path[Frame];
                            var fPathItem = Path[Frame];
                            var xyz = Path[Frame];
                            sequence.ImageLibrary[frame].ShowBlended(xyz.X + (lockedSurfaceValue.Width * X / 100 + sequence.OffsetX), point.Y + (lockedSurfaceValue.Height * Y / 100 + sequence.OffsetY) - fPathItem.Z, lockedSurfaceValue, GraphicsState.ShadowBlendTable.LookupTables[brightness]);
                        }
                    }
                    else
                    {
                        if (inverse)
                        {
                            if (sequence.ImageLibrary.ImageExist(frame))
                            {
                                var item = Path[Frame];
                                var fPathItem = Path[Frame];
                                var item2 = Path[Frame];
                                sequence.ImageLibrary[frame].ShowBlended(item2.X + (lockedSurfaceValue.Width * X / 100 + sequence.OffsetX), item.Y + (lockedSurfaceValue.Height * Y / 100 + sequence.OffsetY) - fPathItem.Z, lockedSurfaceValue, GraphicsState.IntensityBlendTable.LookupTables[100 - brightness]);
                            }
                        }
                        else
                        {
                            if (sequence.ImageLibrary.ImageExist(frame))
                            {
                                var item = Path[Frame];
                                var fPathItem = Path[Frame];
                                var item2 = Path[Frame];
                                sequence.ImageLibrary[frame].ShowBlended(item2.X + (lockedSurfaceValue.Width * X / 100 + sequence.OffsetX), item.Y + (lockedSurfaceValue.Height * Y / 100 + sequence.OffsetY) - fPathItem.Z, lockedSurfaceValue, GraphicsState.IntensityBlendTable.LookupTables[brightness]);
                            }
                        }
                    }
                }

                sequence = sequence.Next;
            }
        }

        void ShowLocal(bool shadow)
        {
            int brightness = default;
            var count = Path.Count;
            brightness = 100 - Frame * 100 / count - FrameFixtureInputs.ShimmerOffset(this, Frame);
            if (brightness < 0)
            {
                brightness = 0;
            }

            if (shadow)
            {
                brightness += 20;
            }

            if (brightness > 100)
            {
                brightness = 100;
            }

            while (sequence != null)
            {
                frame = sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, 3));
                if (frame != -1)
                {
                    if (shadow)
                    {
                        if (sequence.ImageLibrary.ImageExist(frame))
                        {
                            var item = Path[Frame];
                            var fPathItem = Path[Frame];
                            var item2 = Path[Frame];
                            sequence.ImageLibrary[frame].ShowBlended(item2.X + sequence.OffsetX - pixelXSurface, item.Y + sequence.OffsetY - fPathItem.Z - pixelYSurface, lockedSurfaceValue, GraphicsState.ShadowBlendTable.LookupTables[brightness]);
                        }
                    }
                    else
                    {
                        if (sequence.ImageLibrary.ImageExist(frame))
                        {
                            var item = Path[Frame];
                            var fPathItem = Path[Frame];
                            var item2 = Path[Frame];
                            sequence.ImageLibrary[frame].ShowBlended(item2.X + sequence.OffsetX - pixelXSurface, item.Y + sequence.OffsetY - fPathItem.Z - pixelYSurface, lockedSurfaceValue, GraphicsState.IntensityBlendTable.LookupTables[brightness]);
                        }
                    }
                }

                sequence = sequence.Next;
            }
        }

        try
        {
            if (Frame >= 0)
            {
                switch (EffectType)
                {
                    case WorldEffectType.EMParticle:
                        sequence = AoWHSSet.Current.SpellControl.GetSpell(64).CastImages.GetImageSequence(0);
                        ShowGlobal(false, false);
                        break;
                    case WorldEffectType.WMParticle:
                        sequence = AoWHSSet.Current.SpellControl.GetSpell(45).CastImages.GetImageSequence(0);
                        ShowGlobal(false, false);
                        break;
                    case WorldEffectType.WMLParticle:
                        if (AoWHSMap.Current.ViewL == Layer)
                        {
                            sequence = AoWHSSet.Current.SpellControl.GetSpell(75).CastImages.GetImageSequence(1);
                            ShowLocal(false);
                        }

                        break;
                    case WorldEffectType.HatredParticle:
                        sequence = AoWHSSet.Current.SpellControl.GetSpell(46).CastImages.GetImageSequence(0);
                        ShowGlobal(true, false);
                        break;
                    case WorldEffectType.TranqParticle:
                        sequence = AoWHSSet.Current.SpellControl.GetSpell(47).CastImages.GetImageSequence(0);
                        ShowGlobal(false, false);
                        break;
                    case WorldEffectType.SWParticle:
                        sequence = AoWHSSet.Current.SpellControl.GetSpell(65).CastImages.GetImageSequence(1);
                        ShowGlobal(false, true);
                        break;
                    case WorldEffectType.SWBeamParticle:
                        sequence = AoWHSSet.Current.SpellControl.GetSpell(65).CastImages.GetImageSequence(1);
                        ShowGlobal(false, false);
                        break;
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Show", "Error - WorldfxHS " + IntToStr((int)EffectType), 0);
        }
    }

    // Army.PAS:5340
    public override void Activate()
    {
        base.Activate();
        AoWHSMap.Current.NewFrameEvents += NewFrame;
        Frame = -1;
    }

    // Army.PAS:5347
    public override void Deactivate()
    {
        AoWHSMap.Current.NewFrameEvents -= NewFrame;
        base.Deactivate();
    }

    // Army.PAS:5358
    public void NewFrame(object sender)
    {
        WorldEffectHexSprite effect;
        int Count;
        int radiusX;
        int radiusY;
        float gravity = default;
        float vz = default;
        float speed = default;
        float Z = default;
        try
        {
            Frame += 1;
            switch (EffectType)
            {
                case WorldEffectType.Hatred:
                    switch (Frame)
                    {
                        case >= 0 and <= 89:
                            if (Frame % 3 == 0)
                            {
                                radiusX = 200;
                                radiusY = 200;
                                for (int I = 0; I <= 29; I++)
                                {
                                    effect = AoWHSMap.Current.AddEffect(WorldEffectType.HatredParticle);
                                    effect.X = 50;
                                    effect.Y = 50;
                                    radiusX = -radiusX;
                                    var pi = Math.PI;
                                    var sin = Math.Sin((double)((double)(I * 6 + Frame * 4) * pi) / 180);
                                    var random = Random(10);
                                    var random2 = Random(12);
                                    var mathPI = Math.PI;
                                    EffectPaths.MakeLinePath(0, 0, Round((double)Math.Cos((double)((double)(random2 + (I * 12 + Frame * 4)) * mathPI) / 180) * (radiusX + Random(10))), Round((double)sin * (radiusY + random)) / 2, 75, ref effect.Path);
                                }
                            }

                            break;
                        case 90:
                            Frame = -1;
                            break;
                    }

                    break;
                case WorldEffectType.Tranquility:
                    switch (Frame)
                    {
                        case >= 0 and <= 89:
                            if (Frame % 3 == 0)
                            {
                                radiusX = 200;
                                radiusY = 200;
                                for (int I = 0; I <= 29; I++)
                                {
                                    effect = AoWHSMap.Current.AddEffect(WorldEffectType.TranqParticle);
                                    effect.X = 50;
                                    effect.Y = 50;
                                    radiusX = -radiusX;
                                    var pi = Math.PI;
                                    var sin = Math.Sin((double)((double)(I * 6 + Frame * 4) * pi) / 180);
                                    var random = Random(10);
                                    var random2 = Random(12);
                                    var mathPI = Math.PI;
                                    EffectPaths.MakeLinePath(0, 0, Round((double)Math.Cos((double)((double)(random2 + (I * 12 + Frame * 4)) * mathPI) / 180) * (radiusX + Random(10))), Round((double)sin * (radiusY + random)) / 2, 75, ref effect.Path);
                                }
                            }

                            break;
                        case 90:
                            Frame = -1;
                            break;
                    }

                    break;
                case WorldEffectType.Crusade:
                    switch (Frame)
                    {
                        case >= 0 and <= 74:
                            if (Frame % 3 == 0)
                            {
                                radiusX = 150;
                                radiusY = 150;
                                for (int I = 0; I <= 59; I++)
                                {
                                    effect = AoWHSMap.Current.AddEffect(WorldEffectType.WMLParticle);
                                    effect.Layer = Layer;
                                    radiusX = -radiusX;
                                    var pi = Math.PI;
                                    var sin = Math.Sin((double)((double)(I * 6 + Frame * 4) * pi) / 180);
                                    var random = Random(10);
                                    var mathPI = Math.PI;
                                    var cos = Math.Cos((double)((double)(I * 6 + Frame * 4) * mathPI) / 180);
                                    var random2 = Random(10);
                                    EffectPaths.MakeLinePath(X, Y, X + Round((double)cos * (radiusX + random2)), Y + Round((double)sin * (radiusY + random)) / 2, 50, ref effect.Path);
                                }
                            }

                            break;
                        case 75:
                            Frame = -1;
                            break;
                    }

                    break;
                case WorldEffectType.WaterMastery:
                    switch (Frame)
                    {
                        case >= 0 and <= 199:
                            if (Frame % 5 == 0)
                            {
                                AoWHSSet.Current.SpellControl.GetSpell(45).CastSounds.Play(1);
                                Count = Random(10) + 100;
                                for (int J = 0; J <= Count; J++)
                                {
                                    effect = AoWHSMap.Current.AddEffect(WorldEffectType.WMParticle);
                                    effect.X = 50;
                                    effect.Y = 50;
                                    speed = (float)((double)(Random(50) + 50) * 0.1);
                                    gravity = (float)0.1;
                                    vz = (float)((double)(Random(5) + 5) * 0.1);
                                    Z = 0;
                                    EffectPaths.MakeArcPath(0, 0, Random(360), ref Z, ref speed, ref vz, ref gravity, ref effect.Path);
                                }
                            }

                            {
                                for (int J = 0; J <= 1; J++)
                                {
                                    Count = Random(3) + 7;
                                    for (int I = 1; I <= Count - 1; I++)
                                    {
                                        effect = AoWHSMap.Current.AddEffect(WorldEffectType.WMParticle);
                                        var pi = Math.PI;
                                        effect.X = 50 + Round((double)Math.Cos((double)((double)((double)(Frame * 360) * 0.005) * pi) / 180) * Frame) / 4;
                                        var mathPI = Math.PI;
                                        effect.Y = 50 + Round((double)Math.Sin((double)((double)((double)(Frame * 360) * 0.005) * mathPI) / 180) * Frame) / 8;
                                        if (J == 1)
                                        {
                                            effect.X = 100 - effect.X;
                                            effect.Y = 100 - effect.Y;
                                        }

                                        speed = (float)((double)(Random(10) + 30) * 0.1);
                                        gravity = (float)0.5;
                                        vz = (float)((double)(Random(50) + Frame / 2) * 0.1);
                                        Z = 0;
                                        EffectPaths.MakeArcPath(0, 0, I * 360 / Count, ref Z, ref speed, ref vz, ref gravity, ref effect.Path);
                                    }
                                }
                            }

                            break;
                        case 200:
                            Frame = -1;
                            break;
                    }

                    break;
                case WorldEffectType.SpellWard:
                    switch (Frame)
                    {
                        case >= 0 and <= 14:
                            radiusX = 200;
                            radiusY = 200;
                            {
                                for (int I = 0; I <= 29; I++)
                                {
                                    effect = AoWHSMap.Current.AddEffect(WorldEffectType.SWParticle);
                                    effect.X = 50;
                                    effect.Y = 50;
                                    radiusX = -radiusX;
                                    var pi = Math.PI;
                                    var sin = Math.Sin((double)((double)(I * 12 + Frame * 5) * pi) / 180);
                                    var random = Random(10);
                                    var mathPI = Math.PI;
                                    EffectPaths.MakeArcPathXY((short)Round((double)Math.Cos((double)((double)(I * 12 + Frame * 5) * mathPI) / 180) * (radiusX + Random(10))), (short)(Round((double)sin * (radiusY + random)) / 2), 0, -150, 100, 10, ref effect.Path);
                                    for (int J = 0, last = effect.Path.Count - 1; J <= last; J++)
                                    {
                                        var point = effect.Path[J];
                                        effect.Path[J].Z = -point.Z;
                                    }
                                }
                            }

                            break;
                    }

                    switch (Frame)
                    {
                        case >= 15 and <= 44:
                            if (Frame % 5 == 0)
                            {
                                Count = Random(10) + 100;
                                for (int J = 0; J <= Count; J++)
                                {
                                    effect = AoWHSMap.Current.AddEffect(WorldEffectType.SWBeamParticle);
                                    effect.X = 50;
                                    effect.Y = 50;
                                    speed = (float)((double)(Random(50) + 50) * 0.1);
                                    gravity = (float)0.1;
                                    vz = (float)((double)(Random(5) + 5) * 0.1);
                                    Z = 0;
                                    EffectPaths.MakeArcPath(0, -150, Random(360), ref Z, ref speed, ref vz, ref gravity, ref effect.Path);
                                }
                            }

                            break;
                        case 120:
                            Frame = -1;
                            break;
                    }

                    break;
                case WorldEffectType.EarthMastery:
                    switch (Frame)
                    {
                        case >= 0 and <= 49:
                            {
                                for (int I = 0; I <= 29; I++)
                                {
                                    effect = AoWHSMap.Current.AddEffect(WorldEffectType.EMParticle);
                                    radiusX = Random(12);
                                    var pi = Math.PI;
                                    effect.X = Round((double)Math.Cos((double)((double)(I * 12 + radiusX) * pi) / 180) * Frame) + 50;
                                    var mathPI = Math.PI;
                                    effect.Y = Round((double)Math.Sin((double)((double)(I * 12 + radiusX) * mathPI) / 180) * Frame) / 2 + 50;
                                    EffectPaths.MakeLinePath(0, 0, 0, Frame * 3 - 200, 30, ref effect.Path);
                                }
                            }

                            break;
                        case 200:
                            Frame = -1;
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
                    var count = Path.Count;
                    if (Frame >= count - 1)
                    {
                        Frame = Path.Count - 1;
                        Done = true;
                    }

                    break;
            }

            if (Frame == -1)
            {
                Done = true;
            }
        }
        catch
        {
            MessageBox(0, "Error - WorldfxHS " + IntToStr((int)EffectType), "Error during NewFrame", 0);
        }
    }

    // intf_P2_01.inc:17

}
