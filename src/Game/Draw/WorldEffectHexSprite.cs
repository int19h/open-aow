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
    public bool Done { get; private set; }
    public int Frame { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Layer { get; set; }

    public Point3DList Path { get; private set; }

    public WorldEffectType EffectType { get; set; }

    // Army.PAS:5334
    public override void Dispose()
    {
        Path?.Dispose();
        base.Dispose();
    }

    // Army.PAS:5327
    public WorldEffectHexSprite(CustomNode owner) : base(owner)
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
            int count = Path.Count;
            brightness = 100 - (Frame * 100 / count) - FrameFixtureInputs.ShimmerOffset(this, Frame);
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
                            Point3D point = Path[Frame];
                            Point3D fPathItem = Path[Frame];
                            Point3D xyz = Path[Frame];
                            sequence.ImageLibrary[frame].ShowBlended(xyz.X + (lockedSurfaceValue.Width * X / 100) + sequence.OffsetX, point.Y + (lockedSurfaceValue.Height * Y / 100) + sequence.OffsetY - fPathItem.Z, lockedSurfaceValue, GraphicsState.ShadowBlendTable.LookupTables[brightness]);
                        }
                    }
                    else
                    {
                        if (inverse)
                        {
                            if (sequence.ImageLibrary.ImageExist(frame))
                            {
                                Point3D item = Path[Frame];
                                Point3D fPathItem = Path[Frame];
                                Point3D item2 = Path[Frame];
                                sequence.ImageLibrary[frame].ShowBlended(item2.X + (lockedSurfaceValue.Width * X / 100) + sequence.OffsetX, item.Y + (lockedSurfaceValue.Height * Y / 100) + sequence.OffsetY - fPathItem.Z, lockedSurfaceValue, GraphicsState.IntensityBlendTable.LookupTables[100 - brightness]);
                            }
                        }
                        else
                        {
                            if (sequence.ImageLibrary.ImageExist(frame))
                            {
                                Point3D item = Path[Frame];
                                Point3D fPathItem = Path[Frame];
                                Point3D item2 = Path[Frame];
                                sequence.ImageLibrary[frame].ShowBlended(item2.X + (lockedSurfaceValue.Width * X / 100) + sequence.OffsetX, item.Y + (lockedSurfaceValue.Height * Y / 100) + sequence.OffsetY - fPathItem.Z, lockedSurfaceValue, GraphicsState.IntensityBlendTable.LookupTables[brightness]);
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
            int count = Path.Count;
            brightness = 100 - (Frame * 100 / count) - FrameFixtureInputs.ShimmerOffset(this, Frame);
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
                            Point3D item = Path[Frame];
                            Point3D fPathItem = Path[Frame];
                            Point3D item2 = Path[Frame];
                            sequence.ImageLibrary[frame].ShowBlended(item2.X + sequence.OffsetX - pixelXSurface, item.Y + sequence.OffsetY - fPathItem.Z - pixelYSurface, lockedSurfaceValue, GraphicsState.ShadowBlendTable.LookupTables[brightness]);
                        }
                    }
                    else
                    {
                        if (sequence.ImageLibrary.ImageExist(frame))
                        {
                            Point3D item = Path[Frame];
                            Point3D fPathItem = Path[Frame];
                            Point3D item2 = Path[Frame];
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
                        sequence = GamePack.Current.SpellControl.GetSpell(64).CastImages.GetImageSequence(0);
                        ShowGlobal(false, false);
                        break;
                    case WorldEffectType.WMParticle:
                        sequence = GamePack.Current.SpellControl.GetSpell(45).CastImages.GetImageSequence(0);
                        ShowGlobal(false, false);
                        break;
                    case WorldEffectType.WMLParticle:
                        if (StrategicMap.Current.ViewMapLevel == Layer)
                        {
                            sequence = GamePack.Current.SpellControl.GetSpell(75).CastImages.GetImageSequence(1);
                            ShowLocal(false);
                        }

                        break;
                    case WorldEffectType.HatredParticle:
                        sequence = GamePack.Current.SpellControl.GetSpell(46).CastImages.GetImageSequence(0);
                        ShowGlobal(true, false);
                        break;
                    case WorldEffectType.TranqParticle:
                        sequence = GamePack.Current.SpellControl.GetSpell(47).CastImages.GetImageSequence(0);
                        ShowGlobal(false, false);
                        break;
                    case WorldEffectType.SWParticle:
                        sequence = GamePack.Current.SpellControl.GetSpell(65).CastImages.GetImageSequence(1);
                        ShowGlobal(false, true);
                        break;
                    case WorldEffectType.SWBeamParticle:
                        sequence = GamePack.Current.SpellControl.GetSpell(65).CastImages.GetImageSequence(1);
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
        StrategicMap.Current.NewFrameEvents += NewFrame;
        Frame = -1;
    }

    // Army.PAS:5347
    public override void Deactivate()
    {
        StrategicMap.Current.NewFrameEvents -= NewFrame;
        base.Deactivate();
    }

    // Army.PAS:5358
    public void NewFrame(object sender)
    {
        WorldEffectHexSprite effect;
        int count2;
        int radiusX;
        int radiusY;
        float gravity = default;
        float vz = default;
        float speed = default;
        float z = default;
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
                                for (int i = 0; i <= 29; i++)
                                {
                                    effect = StrategicMap.Current.AddEffect(WorldEffectType.HatredParticle);
                                    effect.X = 50;
                                    effect.Y = 50;
                                    radiusX = -radiusX;
                                    double pi = Math.PI;
                                    double sin = Math.Sin((double)(((i * 6) + (Frame * 4)) * pi) / 180);
                                    int random = Random(10);
                                    int random2 = Random(12);
                                    double mathPI = Math.PI;
                                    Point3DList path = effect.Path;
                                    EffectPaths.MakeLinePath(0, 0, Round((double)Math.Cos((double)((random2 + ((i * 12) + (Frame * 4))) * mathPI) / 180) * (radiusX + Random(10))), Round((double)sin * (radiusY + random)) / 2, 75, ref path);
                                    effect.Path = path;
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
                                for (int i = 0; i <= 29; i++)
                                {
                                    effect = StrategicMap.Current.AddEffect(WorldEffectType.TranqParticle);
                                    effect.X = 50;
                                    effect.Y = 50;
                                    radiusX = -radiusX;
                                    double pi = Math.PI;
                                    double sin = Math.Sin((double)(((i * 6) + (Frame * 4)) * pi) / 180);
                                    int random = Random(10);
                                    int random2 = Random(12);
                                    double mathPI = Math.PI;
                                    Point3DList path2 = effect.Path;
                                    EffectPaths.MakeLinePath(0, 0, Round((double)Math.Cos((double)((random2 + ((i * 12) + (Frame * 4))) * mathPI) / 180) * (radiusX + Random(10))), Round((double)sin * (radiusY + random)) / 2, 75, ref path2);
                                    effect.Path = path2;
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
                                for (int i = 0; i <= 59; i++)
                                {
                                    effect = StrategicMap.Current.AddEffect(WorldEffectType.WMLParticle);
                                    effect.Layer = Layer;
                                    radiusX = -radiusX;
                                    double pi = Math.PI;
                                    double sin = Math.Sin((double)(((i * 6) + (Frame * 4)) * pi) / 180);
                                    int random = Random(10);
                                    double mathPI = Math.PI;
                                    double cos = Math.Cos((double)(((i * 6) + (Frame * 4)) * mathPI) / 180);
                                    int random2 = Random(10);
                                    Point3DList path3 = effect.Path;
                                    EffectPaths.MakeLinePath(X, Y, X + Round((double)cos * (radiusX + random2)), Y + (Round((double)sin * (radiusY + random)) / 2), 50, ref path3);
                                    effect.Path = path3;
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
                                GamePack.Current.SpellControl.GetSpell(45).CastSounds.Play(1);
                                count2 = Random(10) + 100;
                                for (int j = 0; j <= count2; j++)
                                {
                                    effect = StrategicMap.Current.AddEffect(WorldEffectType.WMParticle);
                                    effect.X = 50;
                                    effect.Y = 50;
                                    speed = (float)((Random(50) + 50) * 0.1);
                                    gravity = (float)0.1;
                                    vz = (float)((Random(5) + 5) * 0.1);
                                    z = 0;
                                    Point3DList path4 = effect.Path;
                                    EffectPaths.MakeArcPath(0, 0, Random(360), ref z, ref speed, ref vz, ref gravity, ref path4);
                                    effect.Path = path4;
                                }
                            }

                            {
                                for (int j = 0; j <= 1; j++)
                                {
                                    count2 = Random(3) + 7;
                                    for (int i = 1; i <= count2 - 1; i++)
                                    {
                                        effect = StrategicMap.Current.AddEffect(WorldEffectType.WMParticle);
                                        double pi = Math.PI;
                                        effect.X = 50 + (Round((double)Math.Cos((double)((double)(Frame * 360 * 0.005) * pi) / 180) * Frame) / 4);
                                        double mathPI = Math.PI;
                                        effect.Y = 50 + (Round((double)Math.Sin((double)((double)(Frame * 360 * 0.005) * mathPI) / 180) * Frame) / 8);
                                        if (j == 1)
                                        {
                                            effect.X = 100 - effect.X;
                                            effect.Y = 100 - effect.Y;
                                        }

                                        speed = (float)((Random(10) + 30) * 0.1);
                                        gravity = (float)0.5;
                                        vz = (float)((Random(50) + (Frame / 2)) * 0.1);
                                        z = 0;
                                        Point3DList path5 = effect.Path;
                                        EffectPaths.MakeArcPath(0, 0, i * 360 / count2, ref z, ref speed, ref vz, ref gravity, ref path5);
                                        effect.Path = path5;
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
                                for (int i = 0; i <= 29; i++)
                                {
                                    effect = StrategicMap.Current.AddEffect(WorldEffectType.SWParticle);
                                    effect.X = 50;
                                    effect.Y = 50;
                                    radiusX = -radiusX;
                                    double pi = Math.PI;
                                    double sin = Math.Sin((double)(((i * 12) + (Frame * 5)) * pi) / 180);
                                    int random = Random(10);
                                    double mathPI = Math.PI;
                                    Point3DList path6 = effect.Path;
                                    EffectPaths.MakeArcPathXY((short)Round((double)Math.Cos((double)(((i * 12) + (Frame * 5)) * mathPI) / 180) * (radiusX + Random(10))), (short)(Round((double)sin * (radiusY + random)) / 2), 0, -150, 100, 10, ref path6);
                                    effect.Path = path6;
                                    for (int j = 0, last = effect.Path.Count - 1; j <= last; j++)
                                    {
                                        Point3D point = effect.Path[j];
                                        Point3DList path7 = effect.Path;
                                        path7[j].Z = -point.Z;
                                        effect.Path = path7;
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
                                count2 = Random(10) + 100;
                                for (int j = 0; j <= count2; j++)
                                {
                                    effect = StrategicMap.Current.AddEffect(WorldEffectType.SWBeamParticle);
                                    effect.X = 50;
                                    effect.Y = 50;
                                    speed = (float)((Random(50) + 50) * 0.1);
                                    gravity = (float)0.1;
                                    vz = (float)((Random(5) + 5) * 0.1);
                                    z = 0;
                                    Point3DList path8 = effect.Path;
                                    EffectPaths.MakeArcPath(0, -150, Random(360), ref z, ref speed, ref vz, ref gravity, ref path8);
                                    effect.Path = path8;
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
                                for (int i = 0; i <= 29; i++)
                                {
                                    effect = StrategicMap.Current.AddEffect(WorldEffectType.EMParticle);
                                    radiusX = Random(12);
                                    double pi = Math.PI;
                                    effect.X = Round((double)Math.Cos((double)(((i * 12) + radiusX) * pi) / 180) * Frame) + 50;
                                    double mathPI = Math.PI;
                                    effect.Y = (Round((double)Math.Sin((double)(((i * 12) + radiusX) * mathPI) / 180) * Frame) / 2) + 50;
                                    Point3DList path9 = effect.Path;
                                    EffectPaths.MakeLinePath(0, 0, 0, (Frame * 3) - 200, 30, ref path9);
                                    effect.Path = path9;
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
                    int count = Path.Count;
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
