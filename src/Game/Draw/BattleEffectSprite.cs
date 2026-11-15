// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// Done. Cancelled. Frame. AbilityIndex. SpellIndex. Path. Variant. TargetX. TargetY.
// Target. EffectType
public partial class BattleEffectSprite : FixedVisibleHexSprite
{

    public BattleEffectSprite(ECustomNode owner) : base(owner)
    {
        Path = new Point3DList();
        Frame = -1;
    }

    // AoWTC.pas:5227
    public new const int ClassId = 2228486;

    public bool Done { get; set; }
    public bool Cancelled { get; set; }
    public int Frame { get; private set; }
    public int AbilityIndex { get; set; }
    public int SpellIndex { get; set; }

    public Point3DList Path;

    public int Variant { get; set; }

    public int TargetX;
    public int TargetY;

    public CombatObject Target { get; set; }
    public CombatAction CombatAction { get; set; }
    public BattleEffectType EffectType { get; set; }

    // AoWTC.pas:5221
    public override void Dispose()
    {
        Path?.Dispose();
        base.Dispose();
    }

    // AoWTC.pas:5214

    // AoWTC.pas:5239
    public override int GetShowPriority() => 1811939328;

    // AoWTC.pas:5232
    public override void Activate()
    {
        base.Activate();
        CombatMap.Current.NewFrameEvents += NewFrame;
        Frame = -1;
    }

    // AoWTC.pas:5208
    public override void Deactivate()
    {
        CombatMap.Current.NewFrameEvents -= NewFrame;
        base.Deactivate();
    }

    // AoWTC.pas:5244
    public void NewFrame(object sender)
    {
        BattleEffectSprite effect;
        int j = default;
        int i = default;
        ImageSequence sequence;
        Point3DList newPath;
        int k = default;
        int x = default;
        int y = default;
        float z = default;
        float speed = default;
        float vz = default;
        float gravity = default;
        BattleEffectSprite effect2;
        TacticalBattleUnitHexSprite unitHexSprite;
        MapFieldMessage message = default;
        void MakeCloud(int count, int radius)
        {
            for (int n = 1; n <= count - 1; n++)
            {
                effect = CombatMap.Current.AddEffect(BattleEffectType.CloudPuff);
                effect.AbilityIndex = AbilityIndex;
                effect.SpellIndex = SpellIndex;
                var xyz = new Point3D();
                effect.Path.Add(xyz);
                j = Random(70) + 10;
                var pi = Math.PI;
                var cos = Math.Cos((double)((double)j * pi) / 180);
                var random = Random(radius);
                var pathX = Path[0];
                effect.Path[0].X = pathX.X + Round((double)cos * (random - radius / 2));
                var mathPI = Math.PI;
                var sin = Math.Sin((double)((double)j * mathPI) / 180);
                var random2 = Random(radius);
                var pathY = Path[0];
                effect.Path[0].Y = pathY.Y + Round((double)sin * (random2 - radius / 2)) / 2;
            }
        }

        void MakeImpact()
        {
            effect = null;
            var pathY = Path[Frame];
            HexGeometry.PixelToHex(Path[Frame].X, pathY.Y, out i, out j);
            if (CombatMap.Current.MapContainer.MapLevels[0].Field[i, j].TerrainType == 0)
            {
                effect = CombatMap.Current.AddEffect(BattleEffectType.HitWater);
            }
            else
            {
                if (CombatMap.Current.MapContainer.MapLevels[0].Field[i, j].OverlayType != 8 && CombatMap.Current.MapContainer.MapLevels[0].Field[i, j].FindOwnedChild(2229508) == null && CombatMap.Current.MapContainer.MapLevels[0].Field[i, j].FindOwnedChild(2229510) == null)
                {
                    effect = CombatMap.Current.AddEffect(BattleEffectType.HitGround);
                }
            }

            if (effect != null)
            {
                var xyz = new Point3D();
                effect.Path.Add(xyz);
                var pathX = Path[Frame];
                effect.Path[0].X = pathX.X;
                var path = Path[Frame];
                effect.Path[0].Y = path.Y;
            }
        }

        try
        {
            if (Cancelled)
            {
                Frame = -1;
            }
            else
            {
                Frame += 1;
                switch (EffectType)
                {
                    case BattleEffectType.Arrow:
                    case BattleEffectType.Rock:
                    case BattleEffectType.Comet:
                    case BattleEffectType.Breath:
                    case BattleEffectType.CometBall:
                    case BattleEffectType.Swarm:
                        switch (AbilityIndex)
                        {
                            case 44:
                            case 45:
                            case 57:
                            case 58:
                            case 123:
                                Frame += 1;
                                break;
                            case 52:
                                switch (SpellIndex)
                                {
                                    case 117:
                                        Frame += 1;
                                        break;
                                }

                                break;
                        }

                        if (Frame == 0 && CombatMap.Current.PlaySounds && AbilityIndex == 52 && SpellIndex == 122)
                        {
                            GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(0);
                        }

                        var count = Path.Count;
                        if (Frame >= count - 1)
                        {
                            if (CombatMap.Current.PlaySounds)
                            {
                                if (AbilityIndex == 52)
                                {
                                    GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                }
                                else
                                {
                                    GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                }
                            }

                            Frame = Path.Count - 1;
                            Done = true;
                            if (Target == null)
                            {
                                MakeImpact();
                            }

                            var pathY = Path[Frame];
                            var pathZ = Path[Frame];
                            HexGeometry.PixelToHex(Path[Frame].X, pathY.Y - pathZ.Z, out i, out j);
                            CombatMap.Current.MakeVisible(i, j, 0, 2);
                        }

                        break;
                    case BattleEffectType.DecayComet:
                        if (Frame == 0 && CombatMap.Current.PlaySounds && AbilityIndex == 52)
                        {
                            GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(0);
                        }

                        var fPathCount = Path.Count;
                        if (Frame >= fPathCount - 1)
                        {
                            if (CombatMap.Current.PlaySounds)
                            {
                                if (AbilityIndex == 52)
                                {
                                    GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                }
                                else
                                {
                                    GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                }
                            }

                            Frame = Path.Count - 1;
                            Done = true;
                        }

                        break;
                    case BattleEffectType.AltArrow:
                        if (Frame == 0 && CombatMap.Current.PlaySounds && AbilityIndex == 52)
                        {
                            GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(0);
                        }

                        Frame += 3;
                        var count2 = Path.Count;
                        if (Frame >= count2 - 1)
                        {
                            if (CombatMap.Current.PlaySounds)
                            {
                                if (AbilityIndex == 52)
                                {
                                    GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                }
                                else
                                {
                                    GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                }
                            }

                            Frame = Path.Count - 1;
                            Done = true;
                            if (Target == null)
                            {
                                MakeImpact();
                            }

                            var pathY = Path[Frame];
                            var pathZ = Path[Frame];
                            HexGeometry.PixelToHex(Path[Frame].X, pathY.Y - pathZ.Z, out i, out j);
                            CombatMap.Current.MakeVisible(i, j, 0, 2);
                        }

                        break;
                    case BattleEffectType.ScreenNS:
                        Frame += 1;
                        var count3 = Path.Count;
                        if (Frame >= count3 - 1)
                        {
                            if (CombatMap.Current.PlaySounds)
                            {
                                if (AbilityIndex == 52)
                                {
                                    GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                }
                                else
                                {
                                    GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                }
                            }

                            Frame = Path.Count - 1;
                            Done = true;
                            effect = CombatMap.Current.AddEffect(BattleEffectType.HailImpact);
                            effect.AbilityIndex = AbilityIndex;
                            effect.SpellIndex = SpellIndex;
                            var xyz = new Point3D();
                            effect.Path.Add(xyz);
                            var pathX = Path[Frame];
                            effect.Path[0].X = pathX.X;
                            var pathY = Path[Frame];
                            effect.Path[0].Y = pathY.Y;
                        }

                        break;
                    case BattleEffectType.HailImpact:
                        if (AbilityIndex == 52)
                        {
                            sequence = GamePack.Current.SpellControl.GetSpell(SpellIndex).CastImages.ImageSequences[2];
                        }
                        else
                        {
                            sequence = GamePack.Current.AbilityControl.GetAbility(AbilityIndex).ImageSequences.ImageSequences[2];
                        }

                        if (sequence != null)
                        {
                            var totalSequenceFrames = sequence.GetTotalSequenceFrames();
                            if (Frame >= totalSequenceFrames - 1)
                            {
                                Frame = sequence.GetTotalSequenceFrames() - 1;
                                Done = true;
                            }
                        }
                        else
                        {
                            Frame = -1;
                        }

                        break;
                    case BattleEffectType.PrayerGlow:
                        if (AbilityIndex == 52)
                        {
                            sequence = GamePack.Current.SpellControl.GetSpell(SpellIndex).CastImages.ImageSequences[1];
                        }
                        else
                        {
                            sequence = GamePack.Current.AbilityControl.GetAbility(AbilityIndex).ImageSequences.ImageSequences[1];
                        }

                        if (sequence != null)
                        {
                            var totalSequenceFrames = sequence.GetTotalSequenceFrames();
                            if (Frame >= totalSequenceFrames - 1)
                            {
                                Frame = sequence.GetTotalSequenceFrames() - 1;
                                Done = true;
                            }
                        }
                        else
                        {
                            Frame = -1;
                        }

                        break;
                    case BattleEffectType.SFOverlay:
                        var count4 = Path.Count;
                        if (Frame >= count4 - 1)
                        {
                            Frame = Path.Count - 1;
                            Done = true;
                        }

                        break;
                    case BattleEffectType.ScreenSwarm:
                        if (Frame == 0)
                        {
                            Frame = 1;
                        }

                        var count5 = Path.Count;
                        if (Frame >= count5 - 1)
                        {
                            if (CombatMap.Current.PlaySounds)
                            {
                                if (Target != null && AbilityIndex == 52)
                                {
                                    GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                }
                                else
                                {
                                    GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                }
                            }

                            Frame = Path.Count - 1;
                            Done = true;
                        }

                        break;
                    case BattleEffectType.Ray:
                        Frame += 12;
                        var count6 = Path.Count;
                        if (Frame >= count6 - 1)
                        {
                            if (CombatMap.Current.PlaySounds)
                            {
                                if (AbilityIndex == 52)
                                {
                                    GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                }
                                else
                                {
                                    GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                }
                            }

                            Frame = Path.Count - 1;
                            Done = true;
                        }

                        break;
                    case BattleEffectType.FrostBeam:
                        switch (Frame)
                        {
                            case >= 0 and <= 999:
                                Frame += 1;
                                var count7 = Path.Count;
                                if (Frame >= count7 - 1)
                                {
                                    Frame = 999;
                                }

                                j = Random(5) + 5;
                                HexGeometry.HexToPixel(TargetX, TargetY, out k, out x);
                                {
                                    i = 0;
                                    int last = j;
                                    for (; i <= last; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        var random = Random(100);
                                        var random2 = Random(5);
                                        var random3 = Random(100);
                                        EffectPaths.MakeArcPathXY((short)(k - 200), (short)(x - 200), (short)(random3 + k - 50), (short)(random + x - 50), (short)(random2 + 5), 100, ref effect.Path);
                                    }
                                }

                                j = Random(10) + 10;
                                {
                                    i = 0;
                                    int last = j;
                                    for (; i <= last; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        speed = (float)((double)(Random(50) + 20) / 10);
                                        gravity = (float)0.5;
                                        vz = (float)((double)Random(10) / 10);
                                        z = 100;
                                        EffectPaths.MakeArcPath(angle: Random(360), x: k - 200, y: x - 100, z: ref z, speed: ref speed, vz: ref vz, gravity: ref gravity, list: ref effect.Path);
                                    }
                                }

                                break;
                            case >= 1000 and <= 1029:
                                HexGeometry.HexToPixel(TargetX, TargetY, out k, out x);
                                j = Random(5) + 5;
                                {
                                    i = 0;
                                    int last = j;
                                    for (; i <= last; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        var random = Random(100);
                                        var random2 = Random(5);
                                        var random3 = Random(100);
                                        EffectPaths.MakeArcPathXY((short)(k - 200), (short)(x - 200), (short)(random3 + k - 50), (short)(random + x - 50), (short)(random2 + 5), 100, ref effect.Path);
                                    }
                                }

                                j = Random(10) + 10;
                                {
                                    i = 0;
                                    int last = j;
                                    for (; i <= last; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        speed = (float)((double)(Random(50) + 20) / 10);
                                        gravity = (float)0.5;
                                        vz = (float)((double)Random(10) / 10);
                                        z = 100;
                                        EffectPaths.MakeArcPath(angle: Random(360), x: k - 200, y: x - 100, z: ref z, speed: ref speed, vz: ref vz, gravity: ref gravity, list: ref effect.Path);
                                    }
                                }

                                j = Random(5) + 5;
                                {
                                    i = 0;
                                    int last = j;
                                    for (; i <= last; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        speed = (float)((double)(Random(20) + 20) / 10);
                                        gravity = (float)0.5;
                                        vz = (float)((double)(Random(50) + 20) / 10);
                                        z = 0;
                                        var count8 = Path.Count;
                                        var path = Path[count8 - 1];
                                        var count9 = Path.Count;
                                        var pathZ = Path[count9 - 1];
                                        var count10 = Path.Count;
                                        var pathX = Path[count10 - 1];
                                        EffectPaths.MakeArcPath(angle: Random(360), x: pathX.X, y: path.Y - pathZ.Z, z: ref z, speed: ref speed, vz: ref vz, gravity: ref gravity, list: ref effect.Path);
                                    }
                                }

                                break;
                            case 1030:
                                Frame = 1999;
                                break;
                            case >= 2000 and <= 2999:
                                Frame += 1;
                                var count11 = Path.Count;
                                if (Frame >= count11 + 2000 - 1)
                                {
                                    Frame = 2999;
                                }

                                j = Random(5) + 5;
                                {
                                    i = 0;
                                    int last = j;
                                    for (; i <= last; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        speed = (float)((double)(Random(20) + 20) / 10);
                                        gravity = (float)0.5;
                                        vz = (float)((double)(Random(50) + 20) / 10);
                                        z = 0;
                                        var count8 = Path.Count;
                                        var path = Path[count8 - 1];
                                        var count9 = Path.Count;
                                        var pathZ = Path[count9 - 1];
                                        var count10 = Path.Count;
                                        var pathX = Path[count10 - 1];
                                        EffectPaths.MakeArcPath(angle: Random(360), x: pathX.X, y: path.Y - pathZ.Z, z: ref z, speed: ref speed, vz: ref vz, gravity: ref gravity, list: ref effect.Path);
                                    }
                                }

                                break;
                            case 3000:
                                if (CombatMap.Current.PlaySounds)
                                {
                                    if (AbilityIndex == 52)
                                    {
                                        GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                    }
                                    else
                                    {
                                        GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                    }
                                }

                                {
                                    i = 0;
                                    int last = Path.Count - 1;
                                    for (; i <= last; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.Frost);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        speed = (float)((double)(Random(25) + 25) / 10);
                                        gravity = (float)0.5;
                                        vz = (float)((double)(Random(50) + 40) / 10);
                                        z = 0;
                                        var pathY = Path[i];
                                        var pathZ = Path[i];
                                        EffectPaths.MakeArcPath(angle: Random(360), x: Path[i].X, y: pathY.Y - pathZ.Z, z: ref z, speed: ref speed, vz: ref vz, gravity: ref gravity, list: ref effect.Path);
                                    }
                                }

                                j = Random(10) + 100;
                                {
                                    i = 0;
                                    int last = j;
                                    for (; i <= last; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.Frost);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        speed = (float)((double)(Random(50) + 50) / 10);
                                        gravity = (float)0.1;
                                        vz = (float)((double)(Random(5) + 5) / 10);
                                        z = 0;
                                        var count8 = Path.Count;
                                        var path = Path[count8 - 1];
                                        var count9 = Path.Count;
                                        var pathZ = Path[count9 - 1];
                                        var count10 = Path.Count;
                                        var pathX = Path[count10 - 1];
                                        EffectPaths.MakeArcPath(angle: Random(360), x: pathX.X, y: path.Y - pathZ.Z, z: ref z, speed: ref speed, vz: ref vz, gravity: ref gravity, list: ref effect.Path);
                                    }
                                }

                                Done = true;
                                Target = null;
                                break;
                        }

                        break;
                    case BattleEffectType.ShockWave:
                        switch (Frame)
                        {
                            case >= 0 and <= 29:
                                if (Frame == 0 && CombatMap.Current.PlaySounds)
                                {
                                    if (AbilityIndex == 52)
                                    {
                                        GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(0);
                                    }
                                    else
                                    {
                                        GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(0);
                                    }
                                }

                                {
                                    i = 1;
                                    for (; i <= 20; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        var random = Random(360);
                                        var pathX = Path[0];
                                        var pathY = Path[0];
                                        var random2 = Random(2);
                                        var random3 = Random(10);
                                        EffectPaths.MakeSwarmPath(Random(10) + 10, random3 + Frame / 4, i % 6 * 60 + Frame, random, pathX.X, pathY.Y, (SwarmDirection)random2, ref effect.Path);
                                    }
                                }

                                break;
                            case >= 30 and <= 59:
                                {
                                    i = 1;
                                    for (; i <= 20; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        var random = Random(360);
                                        var pathX = Path[0];
                                        var pathY = Path[0];
                                        var random2 = Random(2);
                                        var random3 = Random(10);
                                        EffectPaths.MakeSwarmPath(Random(10) + 10, random3 + (60 - Frame) / 4, i % 6 * 60 + Frame, random, pathX.X, pathY.Y, (SwarmDirection)random2, ref effect.Path);
                                    }
                                }

                                break;
                            case >= 60 and <= 89:
                                {
                                    i = 1;
                                    for (; i <= 20; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        var random = Random(360);
                                        var pathX = Path[0];
                                        var pathY = Path[0];
                                        var random2 = Random(2);
                                        var random3 = Random(10);
                                        EffectPaths.MakeSwarmPath(Random(10) + 10, random3 + (Frame - 60) / 2, i % 6 * 60 + Frame, random, pathX.X, pathY.Y, (SwarmDirection)random2, ref effect.Path);
                                    }
                                }

                                break;
                            case >= 90 and <= 119:
                                {
                                    i = 1;
                                    for (; i <= 20; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        var random = Random(360);
                                        var pathX = Path[0];
                                        var pathY = Path[0];
                                        var random2 = Random(2);
                                        var random3 = Random(10);
                                        EffectPaths.MakeSwarmPath(Random(10) + 10, random3 + (120 - Frame) / 2, i % 6 * 60 + Frame, random, pathX.X, pathY.Y, (SwarmDirection)random2, ref effect.Path);
                                    }
                                }

                                break;
                            case 150:
                                if (CombatMap.Current.PlaySounds)
                                {
                                    if (AbilityIndex == 52)
                                    {
                                        GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                    }
                                    else
                                    {
                                        GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                    }
                                }

                                x = 200;
                                y = 200;
                                j = Random(5);
                                {
                                    i = 0;
                                    for (; i <= 359; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        x = -x;
                                        var pi = Math.PI;
                                        var sin = Math.Sin((double)((double)(i + j) * pi) / 180);
                                        var random = Random(10);
                                        var pathY = Path[0];
                                        var mathPI = Math.PI;
                                        var cos = Math.Cos((double)((double)(i + j) * mathPI) / 180);
                                        var random2 = Random(10);
                                        var pathX = Path[0];
                                        var path = Path[0];
                                        EffectPaths.MakeLinePath(Path[0].X, path.Y, pathX.X + Round((double)cos * (random2 + x)), pathY.Y + Round((double)sin * (random + y)) / 2, 75, ref effect.Path);
                                        if (i % 10 == 0)
                                        {
                                            k = 1;
                                            for (; k <= 10; k += 1)
                                            {
                                                effect2 = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                                effect2.AbilityIndex = AbilityIndex;
                                                effect2.SpellIndex = SpellIndex;
                                                var count7 = effect.Path.Count;
                                                var path2 = effect.Path[count7 - 1];
                                                var count8 = effect.Path.Count;
                                                var path3 = effect.Path[count8 - 1];
                                                var path4 = Path[0];
                                                EffectPaths.MakeLinePath(Path[0].X, path4.Y, path3.X, path2.Y, k * 5 + 75, ref effect2.Path);
                                            }
                                        }
                                    }
                                }

                                break;
                            case >= 151 and <= 169:
                            case >= 210 and <= 229:
                                var path5 = Path[0];
                                HexGeometry.PixelToHex(Path[0].X, path5.Y, out x, out y);
                                message.MessageId = 200720;
                                message.Param[0] = 1;
                                message.Result = -1;
                                {
                                    i = 1;
                                    for (; i <= 36; i += 1)
                                    {
                                        HexGeometry.CenterHexNumberToHex(x, y, i, ref j, ref k);
                                        CombatMap.Current.MapContainer.MapLevels[0].Field[j, k].SendMapFieldMessage(ref message);
                                    }
                                }

                                break;
                            case >= 170 and <= 184:
                            case >= 195 and <= 209:
                                var path6 = Path[0];
                                HexGeometry.PixelToHex(Path[0].X, path6.Y, out x, out y);
                                message.MessageId = 200720;
                                message.Param[0] = 2;
                                message.Result = -1;
                                {
                                    i = 1;
                                    for (; i <= 36; i += 1)
                                    {
                                        HexGeometry.CenterHexNumberToHex(x, y, i, ref j, ref k);
                                        CombatMap.Current.MapContainer.MapLevels[0].Field[j, k].SendMapFieldMessage(ref message);
                                    }
                                }

                                break;
                            case >= 185 and <= 194:
                                var path7 = Path[0];
                                HexGeometry.PixelToHex(Path[0].X, path7.Y, out x, out y);
                                message.MessageId = 200720;
                                message.Param[0] = 3;
                                message.Result = -1;
                                {
                                    i = 1;
                                    for (; i <= 36; i += 1)
                                    {
                                        HexGeometry.CenterHexNumberToHex(x, y, i, ref j, ref k);
                                        CombatMap.Current.MapContainer.MapLevels[0].Field[j, k].SendMapFieldMessage(ref message);
                                    }
                                }

                                break;
                            case 230:
                                var path8 = Path[0];
                                HexGeometry.PixelToHex(Path[0].X, path8.Y, out x, out y);
                                message.MessageId = 200721;
                                message.Result = -1;
                                {
                                    i = 1;
                                    for (; i <= 36; i += 1)
                                    {
                                        HexGeometry.CenterHexNumberToHex(x, y, i, ref j, ref k);
                                        CombatMap.Current.MapContainer.MapLevels[0].Field[j, k].SendMapFieldMessage(ref message);
                                    }
                                }

                                Frame = -1;
                                break;
                        }

                        break;
                    case BattleEffectType.Winds:
                        switch (Frame)
                        {
                            case >= 0 and <= 1999:
                                var count7 = Path.Count;
                                if (Frame >= count7 + 20)
                                {
                                    if (Target != null && Target.GetEnabled())
                                    {
                                        if (CombatMap.Current.PlaySounds)
                                        {
                                            if (AbilityIndex == 52)
                                            {
                                                GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                            }
                                            else
                                            {
                                                GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                            }
                                        }

                                        var count8 = Path.Count;
                                        var path = Path[count8 - 1];
                                        var count9 = Path.Count;
                                        HexGeometry.PixelToHex(Path[count9 - 1].X, path.Y, out j, out k);
                                        var mapField = CombatMap.Current.MapContainer.MapLevels[0].Field[j, k];
                                        ((TacticalBattleUnit)Target).UnitHexSprite.MoveTo(mapField, 0);
                                        ((TacticalBattleUnit)Target).UnitHexSprite.OffsetX = 0;
                                        ((TacticalBattleUnit)Target).UnitHexSprite.OffsetY = 0;
                                        ((TacticalBattleUnit)Target).UnitHexSprite.Busy = false;
                                    }

                                    Frame = -1;
                                }
                                else
                                {
                                    if (CombatMap.Current.PlaySounds && (Frame == TargetX || Frame == TargetY))
                                    {
                                        if (AbilityIndex == 52)
                                        {
                                            GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(0);
                                        }
                                        else
                                        {
                                            GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(0);
                                        }
                                    }

                                    if (Target != null && Target.GetEnabled())
                                    {
                                        var count8 = Path.Count;
                                        if (Frame - 20 < count8 && Frame >= 20)
                                        {
                                            var pathX = Path[Frame - 20];
                                            var path = Path[0];
                                            ((TacticalBattleUnit)Target).UnitHexSprite.OffsetX = pathX.X - path.X;
                                            var pathY = Path[Frame - 20];
                                            var pathZ = Path[Frame - 20];
                                            var path2 = Path[0];
                                            ((TacticalBattleUnit)Target).UnitHexSprite.OffsetY = pathY.Y - pathZ.Z - path2.Y;
                                            ((TacticalBattleUnit)Target).UnitHexSprite.SetDirection((byte)(((TacticalBattleUnit)Target).UnitHexSprite.Direction + 1));
                                            if (((TacticalBattleUnit)Target).UnitHexSprite.Direction > 6)
                                            {
                                                ((TacticalBattleUnit)Target).UnitHexSprite.SetDirection(1);
                                            }

                                            ((TacticalBattleUnit)Target).UnitHexSprite.Busy = true;
                                        }
                                    }

                                    i = Frame - 20;
                                    int last = Frame;
                                    for (; i <= last; i += 1)
                                    {
                                        bool condition = i >= 0;
                                        if (condition)
                                        {
                                            var count8 = Path.Count;
                                            condition = i < count8;
                                        }

                                        if (condition)
                                        {
                                            effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                            effect.AbilityIndex = AbilityIndex;
                                            effect.SpellIndex = SpellIndex;
                                            var random = Random(360);
                                            var pathX = Path[i];
                                            var random2 = Random(10);
                                            var pathY = Path[i];
                                            var pathZ = Path[i];
                                            var random3 = Random(10);
                                            var random4 = Random(2);
                                            var count8 = Path.Count;
                                            EffectPaths.MakeSwarmPath(Random(5) + 20, (count8 - i) / 5, 0, random, pathX.X + random2 - 5, pathY.Y - pathZ.Z + random3 - 5, (SwarmDirection)random4, ref effect.Path);
                                        }
                                    }
                                }

                                break;
                        }

                        break;
                    case BattleEffectType.SacFlame:
                        switch (Frame)
                        {
                            case >= 0 and <= 99:
                                if (Frame == 0 && CombatMap.Current.PlaySounds)
                                {
                                    if (AbilityIndex == 52)
                                    {
                                        GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(0);
                                    }
                                    else
                                    {
                                        GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(0);
                                    }
                                }

                                j = Random(6) + 10;
                                var hexY = ((TacticalBattleUnit)Target).UnitHexSprite.GetHexY();
                                HexGeometry.HexToPixel(((TacticalBattleUnit)Target).UnitHexSprite.GetHexX(), hexY, out k, out x);
                                {
                                    i = 1;
                                    int last = j - 1;
                                    for (; i <= last; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.Frost);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        speed = (float)((double)(Random(25) + 25) / 10);
                                        gravity = (float)0.5;
                                        vz = (float)((double)(Random(50) + Frame) / 10);
                                        z = 0;
                                        EffectPaths.MakeArcPath(k + 16, x + 16, i * 360 / j, ref z, ref speed, ref vz, ref gravity, ref effect.Path);
                                    }
                                }

                                switch (Frame)
                                {
                                    case 0:
                                    case 20:
                                    case 40:
                                    case 60:
                                    case 75:
                                    case 85:
                                    case 90:
                                    case 94:
                                    case 97:
                                    case 99:
                                        if (CombatMap.Current.PlaySounds)
                                        {
                                            if (AbilityIndex == 52)
                                            {
                                                GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                            }
                                            else
                                            {
                                                GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                            }
                                        }

                                        j = Random(10) + 100;
                                        {
                                            i = 0;
                                            int last = j;
                                            for (; i <= last; i += 1)
                                            {
                                                effect = CombatMap.Current.AddEffect(BattleEffectType.Frost);
                                                effect.AbilityIndex = AbilityIndex;
                                                effect.SpellIndex = SpellIndex;
                                                speed = (float)((double)(Random(50) + 50) / 10);
                                                gravity = (float)0.1;
                                                vz = (float)((double)(Random(5) + 5) / 10);
                                                z = 0;
                                                EffectPaths.MakeArcPath(angle: Random(360), x: k + 16, y: x + 16, z: ref z, speed: ref speed, vz: ref vz, gravity: ref gravity, list: ref effect.Path);
                                            }
                                        }

                                        break;
                                }

                                break;
                            case >= 125 and <= 199:
                                if (Frame == 125)
                                {
                                    var path = Path[0];
                                    HexGeometry.PixelToHex(Path[0].X, path.Y, out i, out j);
                                    CombatMap.Current.CenterViewTo(i, j - 3, 0);
                                }

                                if (CombatMap.Current.PlaySounds)
                                {
                                    if (AbilityIndex == 52)
                                    {
                                        GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                    }
                                    else
                                    {
                                        GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                    }
                                }

                                j = Random(6) + 10;
                                var pathX = Path[0];
                                var random2 = Random(160);
                                k = pathX.X + random2 - 80;
                                var pathY = Path[0];
                                var random3 = Random(80);
                                x = pathY.Y + random3 - 40;
                                {
                                    i = 1;
                                    int last = j - 1;
                                    for (; i <= last; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.Frost);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        speed = (float)((double)(Random(50) + 25) / 10);
                                        gravity = (float)0.5;
                                        vz = (float)((double)(Random(50) + 100) / 10);
                                        z = 0;
                                        EffectPaths.MakeArcPath(k, x, i * 360 / j, ref z, ref speed, ref vz, ref gravity, ref effect.Path);
                                    }
                                }

                                j = Random(10) + 50;
                                {
                                    i = 0;
                                    int last = j;
                                    for (; i <= last; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.Frost);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        speed = (float)((double)(Random(50) + 50) / 10);
                                        gravity = (float)0.1;
                                        vz = (float)((double)(Random(5) + 5) / 10);
                                        z = 0;
                                        EffectPaths.MakeArcPath(angle: Random(360), x: k, y: x, z: ref z, speed: ref speed, vz: ref vz, gravity: ref gravity, list: ref effect.Path);
                                    }
                                }

                                break;
                            case 200:
                                Frame = -1;
                                break;
                        }

                        break;
                    case BattleEffectType.MindDecay:
                        switch (Frame)
                        {
                            case >= 0 and <= 49:
                                if (Frame % 2 > 0)
                                {
                                    i = 0;
                                    int last = Frame;
                                    for (; i <= last; i += 1)
                                    {
                                        j = 0;
                                        int limit = i / 25;
                                        for (; j <= limit; j += 1)
                                        {
                                            effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                            effect.AbilityIndex = AbilityIndex;
                                            effect.SpellIndex = SpellIndex;
                                            var random = Random(360);
                                            var pathX = Path[0];
                                            var pathY = Path[0];
                                            var random2 = Random(2);
                                            EffectPaths.MakeSwarmPath(Random(10) + 10, i / 2, 45, random, pathX.X + 100 - i, pathY.Y - 200 - 50 + i, (SwarmDirection)random2, ref effect.Path);
                                        }
                                    }
                                }

                                break;
                            case >= 50 and <= 149:
                                if (Frame % 2 > 0)
                                {
                                    i = 0;
                                    for (; i <= 50; i += 1)
                                    {
                                        j = 0;
                                        int last = i / 25;
                                        for (; j <= last; j += 1)
                                        {
                                            effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                            effect.AbilityIndex = AbilityIndex;
                                            effect.SpellIndex = SpellIndex;
                                            var random = Random(360);
                                            var pathX = Path[0];
                                            var pathY = Path[0];
                                            var random2 = Random(2);
                                            EffectPaths.MakeSwarmPath(Random(10) + 10, i / 2, 45, random, pathX.X + 100 - i, pathY.Y - 200 - 50 + i, (SwarmDirection)random2, ref effect.Path);
                                        }
                                    }
                                }

                                if (Frame % 5 == 0 && Frame < 85)
                                {
                                    effect = CombatMap.Current.AddEffect(BattleEffectType.DecayComet);
                                    effect.AbilityIndex = AbilityIndex;
                                    effect.SpellIndex = SpellIndex;
                                    var pathY = Path[0];
                                    var random = Random(50);
                                    var random2 = Random(5);
                                    var pathX = Path[0];
                                    var random3 = Random(50);
                                    var path = Path[0];
                                    var random4 = Random(50);
                                    var path2 = Path[0];
                                    var random5 = Random(50);
                                    EffectPaths.MakeArcPathXY((short)(path2.X + random5 + 25), (short)(path.Y + random4 - 225), (short)(pathX.X + random3 - 25), (short)(pathY.Y + random - 25), (short)(random2 + 5), 100, ref effect.Path);
                                    i = 0;
                                    for (; i <= 10; i += 1)
                                    {
                                        speed = (float)((double)(Random(50) + 50) / 10);
                                        gravity = (float)1.5;
                                        vz = (float)((double)(Random(25) + 25) / 10);
                                        z = 0;
                                        var count7 = effect.Path.Count;
                                        var path3 = effect.Path[count7 - 1];
                                        var count8 = effect.Path.Count;
                                        var pathZ = effect.Path[count8 - 1];
                                        var count9 = effect.Path.Count;
                                        var path4 = effect.Path[count9 - 1];
                                        EffectPaths.MakeArcPath(angle: Random(360), x: path4.X, y: path3.Y - pathZ.Z, z: ref z, speed: ref speed, vz: ref vz, gravity: ref gravity, list: ref effect.Path);
                                    }

                                    HexGeometry.CenterHexNumberToHex(TargetX, TargetY, (Frame - 50) / 5, ref j, ref k);
                                    // { Original swaps the converted hex coordinates. }
                                    HexGeometry.HexToPixel(j, k, out x, out y);
                                    unitHexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].Field[j, k].FindOwnedChild(2228484);
                                    if (unitHexSprite != null && unitHexSprite.Unit.GetSide() != Target.GetSide())
                                    {
                                        effect.Target = unitHexSprite.Unit;
                                    }
                                    else
                                    {
                                        effect.Target = null;
                                    }

                                    if (effect.Target != null)
                                    {
                                        var random6 = Random(3);
                                        var count7 = effect.Path.Count;
                                        var path3 = effect.Path[count7 - 1];
                                        var count8 = effect.Path.Count;
                                        var pathZ = effect.Path[count8 - 1];
                                        var count9 = effect.Path.Count;
                                        EffectPaths.MakeArcPathXY((short)effect.Path[count9 - 1].X, (short)(path3.Y - pathZ.Z), (short)(x + 16), (short)(y - 8), (short)(random6 + 6), 100, ref effect.Path);
                                        i = 0;
                                        int last = Random(3) + 5;
                                        for (; i <= last; i += 1)
                                        {
                                            var count10 = effect.Path.Count;
                                            var path4 = effect.Path[count10 - 1];
                                            var count11 = effect.Path.Count;
                                            var path5 = effect.Path[count11 - 1];
                                            var count12 = effect.Path.Count;
                                            var path6 = effect.Path[count12 - 1];
                                            var count13 = effect.Path.Count;
                                            var path7 = effect.Path[count13 - 1];
                                            var count14 = effect.Path.Count;
                                            var path8 = effect.Path[count14 - 1];
                                            var count15 = effect.Path.Count;
                                            EffectPaths.MakeLinePath(effect.Path[count15 - 1].X, path6.Y - path7.Z, path8.X, path4.Y - path5.Z - 6, 3, ref effect.Path);
                                            var count16 = effect.Path.Count;
                                            var path9 = effect.Path[count16 - 1];
                                            var count17 = effect.Path.Count;
                                            var item = effect.Path[count17 - 1];
                                            var count18 = effect.Path.Count;
                                            var fPathItem = effect.Path[count18 - 1];
                                            var count19 = effect.Path.Count;
                                            var path10 = effect.Path[count19 - 1];
                                            var count20 = effect.Path.Count;
                                            var path11 = effect.Path[count20 - 1];
                                            var count21 = effect.Path.Count;
                                            EffectPaths.MakeLinePath(effect.Path[count21 - 1].X, fPathItem.Y - path10.Z, path11.X, path9.Y - item.Z + 6, 3, ref effect.Path);
                                        }
                                    }
                                }

                                break;
                            case 200:
                                Frame = -1;
                                Target = null;
                                break;
                        }

                        break;
                    case BattleEffectType.Chain:
                        newPath = new Point3DList();
                        i = 0;
                        j = 0;
                        k = 0;
                        while (true)
                        {
                            var count7 = Path.Count;
                            if (!(i < count7))
                                break;
                            if (Path[i].Z > 0)
                            {
                                if (Path[i].Z > 1)
                                {
                                    k = Path[i].Z;
                                }

                                var pathY = Path[i];
                                var pathX = Path[i];
                                var path = Path[j];
                                EffectPaths.MakeLightningPath(Path[j].X, path.Y, pathX.X, pathY.Y, ref newPath);
                                switch (k)
                                {
                                    case -1:
                                        var newPathCount = newPath.Count;
                                        newPath[newPathCount - 1].Z = 16;
                                        Frame = newPath.Count - 1;
                                        k = 0;
                                        break;
                                    case >= 2 and <= 16:
                                        var count8 = newPath.Count;
                                        newPath[count8 - 1].Z = k - 1;
                                        Frame = newPath.Count - 1;
                                        if (k == 2)
                                        {
                                            k = -1;
                                        }
                                        else
                                        {
                                            k = 0;
                                        }

                                        break;
                                }

                                j = i;
                            }

                            i += 1;
                        }

                        Path?.Dispose();
                        Path = newPath;
                        var count22 = Path.Count;
                        if (Frame >= count22 - 1)
                        {
                            Frame = Path.Count - 1;
                            if (Path[Frame].Z <= 1)
                            {
                                Done = true;
                            }
                        }

                        if (Path[Frame].Z == 15)
                        {
                            if (CombatMap.Current.PlaySounds)
                            {
                                if (AbilityIndex == 52)
                                {
                                    GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                }
                                else
                                {
                                    GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                }
                            }

                            var pathY = Path[Frame];
                            var pathZ = Path[Frame];
                            HexGeometry.PixelToHex(Path[Frame].X, pathY.Y - pathZ.Z, out i, out j);
                            CombatMap.Current.MakeVisible(i, j, 0, 3);
                        }

                        break;
                    case BattleEffectType.Touch:
                    case BattleEffectType.Effect:
                        if (Frame == 0 && CombatMap.Current.PlaySounds && AbilityIndex == 65)
                        {
                            GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(0);
                        }

                        if (AbilityIndex == 52)
                        {
                            sequence = GamePack.Current.SpellControl.GetSpell(SpellIndex).CastImages.ImageSequences[0];
                        }
                        else
                        {
                            sequence = GamePack.Current.AbilityControl.GetAbility(AbilityIndex).ImageSequences.ImageSequences[0];
                        }

                        if (sequence != null)
                        {
                            var totalSequenceFrames = sequence.GetTotalSequenceFrames();
                            if (Frame >= totalSequenceFrames - 1)
                            {
                                if (CombatMap.Current.PlaySounds)
                                {
                                    if (AbilityIndex == 52)
                                    {
                                        GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                    }
                                    else
                                    {
                                        GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                    }
                                }

                                Frame = sequence.GetTotalSequenceFrames() - 1;
                                Done = true;
                            }
                        }
                        else
                        {
                            Frame = -1;
                        }

                        break;
                    case BattleEffectType.SacredWrath:
                        if (AbilityIndex == 52)
                        {
                            sequence = GamePack.Current.SpellControl.GetSpell(SpellIndex).CastImages.ImageSequences[1];
                        }
                        else
                        {
                            sequence = GamePack.Current.AbilityControl.GetAbility(AbilityIndex).ImageSequences.ImageSequences[1];
                        }

                        if (sequence == null)
                        {
                            Frame = -1;
                        }

                        break;
                    case BattleEffectType.FBExplosion:
                        sequence = GamePack.Current.SpellControl.GetSpell(SpellIndex).CastImages.ImageSequences[1];
                        if (sequence != null)
                        {
                            var totalSequenceFrames = sequence.GetTotalSequenceFrames();
                            if (Frame >= totalSequenceFrames - 1)
                            {
                                Frame = sequence.GetTotalSequenceFrames() - 1;
                                Done = true;
                            }
                        }
                        else
                        {
                            Frame = -1;
                        }

                        break;
                    case BattleEffectType.ScreenNESW:
                        if (Frame == 0)
                        {
                            if (Path[0].X == 0)
                            {
                                Path[0].X = 100;
                                var random = Random(50);
                                Path[0].Y = random;
                            }
                            else
                            {
                                var random = Random(50);
                                Path[0].X = random + 50;
                                Path[0].Y = 0;
                            }
                        }
                        else
                        {
                            var pathX = Path[0];
                            pathX.X -= 3;
                            var pathY = Path[0];
                            pathY.Y += 3;
                            if (Path[0].X < 0)
                            {
                                Frame = -1;
                            }
                            else
                            {
                                if (Path[0].Y > 100)
                                {
                                    Frame = -1;
                                }
                            }
                        }

                        break;
                    case BattleEffectType.DamLightning:
                    case BattleEffectType.DamFire:
                    case BattleEffectType.DamPoison:
                    case BattleEffectType.DamMagic:
                    case BattleEffectType.DamHoly:
                    case BattleEffectType.DamDeath:
                    case BattleEffectType.DamCold:
                    case BattleEffectType.GeyserWater:
                    case BattleEffectType.Frost:
                    case BattleEffectType.SacFire:
                        var count23 = Path.Count;
                        if (Frame >= count23 - 1)
                        {
                            Frame = Path.Count - 1;
                            Done = true;
                        }

                        break;
                    case BattleEffectType.WallSmoke:
                        sequence = GamePack.Current.ImageSequences.ImageSequences[5];
                        if (sequence == null)
                        {
                            Frame = -1;
                        }
                        else
                        {
                            var totalSequenceFrames = sequence.GetTotalSequenceFrames();
                            if (Frame >= totalSequenceFrames)
                            {
                                Frame = sequence.GetTotalSequenceFrames() - 1;
                                Done = true;
                            }
                        }

                        break;
                    case BattleEffectType.HitGround:
                        sequence = GamePack.Current.ImageSequences.ImageSequences[9];
                        if (sequence == null)
                        {
                            Frame = -1;
                        }
                        else
                        {
                            var totalSequenceFrames = sequence.GetTotalSequenceFrames();
                            if (Frame >= totalSequenceFrames)
                            {
                                Frame = sequence.GetTotalSequenceFrames() - 1;
                                Done = true;
                            }
                        }

                        break;
                    case BattleEffectType.HitWater:
                        sequence = GamePack.Current.ImageSequences.ImageSequences[10];
                        if (sequence == null)
                        {
                            Frame = -1;
                        }
                        else
                        {
                            var totalSequenceFrames = sequence.GetTotalSequenceFrames();
                            if (Frame >= totalSequenceFrames)
                            {
                                Frame = sequence.GetTotalSequenceFrames() - 1;
                                Done = true;
                            }
                        }

                        break;
                    case BattleEffectType.Blood:
                        var pathZ2 = Path[0];
                        sequence = GamePack.Current.ImageSequences.ImageSequences[pathZ2.Z];
                        if (sequence == null)
                        {
                            Frame = -1;
                        }
                        else
                        {
                            var totalSequenceFrames = sequence.GetTotalSequenceFrames();
                            if (Frame >= totalSequenceFrames - 1)
                            {
                                Frame = sequence.GetTotalSequenceFrames() - 1;
                                Done = true;
                            }
                        }

                        break;
                    case BattleEffectType.Tremors:
                        switch (Frame)
                        {
                            case 0:
                                var xyz = new Point3D();
                                Path.Add(xyz);
                                Path[0].X = CombatMap.Current.ViewPixelX;
                                Path[0].Y = CombatMap.Current.ViewPixelY;
                                break;
                            case >= 1 and <= 29:
                                var pathY = Path[0];
                                var random = Random(Frame / 3);
                                var pathX = Path[0];
                                var random2 = Random(Frame / 3);
                                CombatMap.Current.ChangeViewTo(pathX.X + random2 - Frame / 6, pathY.Y + random - Frame / 6, CombatMap.Current.ViewMapLevel);
                                break;
                            case >= 30 and <= 89:
                                var path = Path[0];
                                var random3 = Random(10);
                                var path2 = Path[0];
                                var random4 = Random(10);
                                CombatMap.Current.ChangeViewTo(path2.X + random4 - 5, path.Y + random3 - 5, CombatMap.Current.ViewMapLevel);
                                break;
                            case >= 90 and <= 119:
                                var path3 = Path[0];
                                var random5 = Random((119 - Frame) / 3);
                                var path4 = Path[0];
                                var random6 = Random((119 - Frame) / 3);
                                CombatMap.Current.ChangeViewTo(path4.X + random6 - (119 - Frame) / 6, path3.Y + random5 - (119 - Frame) / 6, CombatMap.Current.ViewMapLevel);
                                break;
                            case 120:
                                Frame = -1;
                                break;
                        }

                        break;
                    case BattleEffectType.Cloud:
                        switch (Frame)
                        {
                            case 250:
                                Frame = -1;
                                break;
                            case >= 1 and <= 149:
                                MakeCloud(Frame, Frame);
                                break;
                            case >= 150 and <= 199:
                                MakeCloud(150, 150);
                                break;
                            case >= 200 and <= 249:
                                MakeCloud((250 - Frame) * 3, 150);
                                break;
                        }

                        break;
                    case BattleEffectType.CloudPuff:
                        if (AbilityIndex == 52)
                        {
                            sequence = GamePack.Current.SpellControl.GetSpell(SpellIndex).CastImages.ImageSequences[0];
                        }
                        else
                        {
                            sequence = GamePack.Current.AbilityControl.GetAbility(AbilityIndex).ImageSequences.ImageSequences[0];
                        }

                        if (sequence != null)
                        {
                            var totalSequenceFrames = sequence.GetTotalSequenceFrames();
                            if (Frame >= totalSequenceFrames - 1)
                            {
                                Frame = sequence.GetTotalSequenceFrames() - 1;
                                Done = true;
                            }
                        }
                        else
                        {
                            Frame = -1;
                        }

                        break;
                    case BattleEffectType.HighPrayer:
                        switch (Frame)
                        {
                            case 0:
                            case 5:
                            case 10:
                            case 15:
                            case 20:
                            case 25:
                            case 30:
                            case 35:
                            case 40:
                            case 45:
                            case 50:
                            case 55:
                            case 60:
                            case 65:
                            case 70:
                            case 75:
                            case 80:
                            case 85:
                            case 90:
                            case 95:
                                {
                                    i = 0;
                                    int last = CombatMap.Current.Combat.CombatData.GetCount() - 1;
                                    for (; i <= last; i += 1)
                                    {
                                        if (CombatMap.Current.Combat.CombatData.GetObjects(i) is CombatUnit && CombatMap.Current.Combat.CombatData.GetObjects(i).GetEnabled() && CombatMap.Current.Combat.CombatData.GetObjects(i).CombatPlayer == Target.CombatPlayer)
                                        {
                                            effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                            var hexY = ((TacticalBattleUnit)CombatMap.Current.Combat.CombatData.GetObjects(i)).UnitHexSprite.GetHexY();
                                            HexGeometry.HexToPixel(((TacticalBattleUnit)CombatMap.Current.Combat.CombatData.GetObjects(i)).UnitHexSprite.GetHexX(), hexY, out j, out k);
                                            j = Random(20) + (j + 6);
                                            k = Random(20) + (k + 6);
                                            EffectPaths.MakeLinePath(j, k - 100, j, k, 50, ref effect.Path);
                                            effect.AbilityIndex = AbilityIndex;
                                            effect.SpellIndex = SpellIndex;
                                        }
                                    }
                                }

                                break;
                            case 100:
                                {
                                    i = 0;
                                    int last = CombatMap.Current.Combat.CombatData.GetCount() - 1;
                                    for (; i <= last; i += 1)
                                    {
                                        if (CombatMap.Current.Combat.CombatData.GetObjects(i) is CombatUnit && CombatMap.Current.Combat.CombatData.GetObjects(i).GetEnabled() && CombatMap.Current.Combat.CombatData.GetObjects(i).CombatPlayer == Target.CombatPlayer)
                                        {
                                            effect = CombatMap.Current.AddEffect(BattleEffectType.PrayerGlow);
                                            var hexY = ((TacticalBattleUnit)CombatMap.Current.Combat.CombatData.GetObjects(i)).UnitHexSprite.GetHexY();
                                            HexGeometry.HexToPixel(((TacticalBattleUnit)CombatMap.Current.Combat.CombatData.GetObjects(i)).UnitHexSprite.GetHexX(), hexY, out j, out k);
                                            var xyz = new Point3D();
                                            effect.Path.Add(xyz);
                                            effect.Path[0].X = j + 16;
                                            effect.Path[0].Y = k + 16;
                                            effect.AbilityIndex = AbilityIndex;
                                            effect.SpellIndex = SpellIndex;
                                            var combatObject = CombatMap.Current.Combat.CombatData.GetObjects(i);
                                            effect.Target = combatObject;
                                        }
                                    }
                                }

                                Frame = -1;
                                Target = null;
                                break;
                        }

                        break;
                    case BattleEffectType.Resurrect:
                        switch (Frame)
                        {
                            case 100:
                                Frame = -1;
                                break;
                        }

                        break;
                    case BattleEffectType.DecayLeg:
                        var count24 = Path.Count;
                        if (Frame >= count24)
                        {
                            Frame = -1;
                        }

                        break;
                    case BattleEffectType.Geyser:
                        switch (Frame)
                        {
                            case 150:
                                Frame = -1;
                                if (Target != null && Target.GetEnabled())
                                {
                                    if (CombatMap.Current.PlaySounds)
                                    {
                                        if (AbilityIndex == 52)
                                        {
                                            GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(1);
                                        }
                                        else
                                        {
                                            GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(1);
                                        }
                                    }

                                    ((TacticalBattleUnit)Target).UnitHexSprite.OffsetY = 0;
                                    ((TacticalBattleUnit)Target).UnitHexSprite.Busy = false;
                                }

                                if (Target != null && ((CombatUnit)Target).Troop.GetMoveTypes().Contains(MoveType.Flying))
                                {
                                    Target = null;
                                }

                                break;
                            case >= 0 and <= 99:
                                if (CombatMap.Current.PlaySounds && Frame % 30 == 29)
                                {
                                    if (AbilityIndex == 52)
                                    {
                                        GamePack.Current.SpellControl.GetSpell(SpellIndex).CastSounds.Play(0);
                                    }
                                    else
                                    {
                                        GamePack.Current.AbilityControl.GetAbility(AbilityIndex).SfxLibrary.Play(0);
                                    }
                                }

                                if (Target != null && Target.GetEnabled())
                                {
                                    var random = Random(2);
                                    ((TacticalBattleUnit)Target).UnitHexSprite.OffsetY = random + -(Frame * 3 / 2) - 1;
                                    ((TacticalBattleUnit)Target).UnitHexSprite.Busy = true;
                                }

                                j = Random(6) + 20;
                                {
                                    i = 1;
                                    int last = j - 1;
                                    for (; i <= last; i += 1)
                                    {
                                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        effect.AbilityIndex = AbilityIndex;
                                        effect.SpellIndex = SpellIndex;
                                        speed = (float)((double)(Random(10) + 10) / 10);
                                        gravity = (float)0.5;
                                        vz = (float)((double)((double)Random(50) + (double)(Frame * 3) * 0.5) / 10);
                                        z = 0;
                                        var path = Path[0];
                                        EffectPaths.MakeArcPath(Path[0].X, path.Y, i * 360 / j, ref z, ref speed, ref vz, ref gravity, ref effect.Path);
                                    }
                                }

                                break;
                            case >= 125 and <= 149:
                                if (Target != null && Target.GetEnabled())
                                {
                                    ((TacticalBattleUnit)Target).UnitHexSprite.OffsetY = (Frame - 150) * 6;
                                    ((TacticalBattleUnit)Target).UnitHexSprite.SetDirection((byte)(Frame % 6 + 1));
                                }

                                break;
                        }

                        break;
                    case BattleEffectType.SWComet:
                        break;
                    default:
                        Frame = -1;
                        break;
                }
            }

            if (Frame == -1)
            {
                Done = true;
            }
        }
        catch
        {
            // { Original puts the diagnostic in the message-box caption. }
            MessageBox(0, "Error during NewFrame", NullTerminatedText("Error - fxHS " + IntToStr(AbilityIndex) + " " + IntToStr(SpellIndex)), 0);
        }
    }

    // AoWTC.pas:196

}
