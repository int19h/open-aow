// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// FDone. FCancelled. FFrame. FAbilityIndex. FSpellIndex. FPath. FVariant. FTargetX. FTargetY.
// FTarget. FEffectType
public partial class BattleEffectSprite : FixedVisibleHexSprite
{

    public BattleEffectSprite(ECustomNode owner) : base(owner)
    {
        FPath = new Point3DList();
        FFrame = -1;
    }

    // AoWTC.pas:5227
    public new const int ClassID = 2228486;

    public bool FDone;
    public bool FCancelled;
    public int FFrame;
    public int FAbilityIndex;
    public int FSpellIndex;
    public Point3DList FPath;
    public int FVariant;
    public int FTargetX;
    public int FTargetY;
    public CombatObject FTarget;
    public CombatAction FCombatAction;
    public BattleEffectType FEffectType;

    // AoWTC.pas:5221
    public override void Dispose()
    {
        FPath?.Dispose();
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
        FFrame = -1;
    }

    // AoWTC.pas:5208
    public override void Deactivate()
    {
        CombatMap.Current.NewFrameEvents -= NewFrame;
        base.Deactivate();
    }

    // AoWTC.pas:5244
    public void NewFrame(object Sender)
    {
        BattleEffectSprite Effect;
        int J = default;
        int I = default;
        ImageSequence Sequence;
        Point3DList NewPath;
        int K = default;
        int X = default;
        int Y = default;
        float Z = default;
        float Speed = default;
        float VZ = default;
        float Gravity = default;
        BattleEffectSprite Effect2;
        TacticalBattleUnitHexSprite UnitHS;
        MapFieldMessage Msg = default;
        void MakeCloud(int Count, int Radius)
        {
            for (int N = 1; N <= Count - 1; N++)
            {
                Effect = CombatMap.Current.AddEffect(BattleEffectType.CloudPuff);
                Effect.FAbilityIndex = FAbilityIndex;
                Effect.FSpellIndex = FSpellIndex;
                var xyz = new Point3D();
                Effect.FPath.Add(xyz);
                J = Random(70) + 10;
                var pi = Math.PI;
                var cos = Math.Cos((double)((double)J * pi) / 180);
                var random = Random(Radius);
                var pathX = FPath[0];
                Effect.FPath[0].X = pathX.X + Round((double)cos * (random - Radius / 2));
                var mathPI = Math.PI;
                var sin = Math.Sin((double)((double)J * mathPI) / 180);
                var random2 = Random(Radius);
                var pathY = FPath[0];
                Effect.FPath[0].Y = pathY.Y + Round((double)sin * (random2 - Radius / 2)) / 2;
            }
        }

        void MakeImpact()
        {
            Effect = null;
            var pathY = FPath[FFrame];
            HexGeometry.HPtoHX(FPath[FFrame].X, pathY.Y, ref I, ref J);
            if (CombatMap.Current.MapContainer.MapLevels[0].Field[I, J].TerrainType == 0)
            {
                Effect = CombatMap.Current.AddEffect(BattleEffectType.HitWater);
            }
            else
            {
                if (CombatMap.Current.MapContainer.MapLevels[0].Field[I, J].OverlayType != 8 && CombatMap.Current.MapContainer.MapLevels[0].Field[I, J].FindOwnedChild(2229508) == null && CombatMap.Current.MapContainer.MapLevels[0].Field[I, J].FindOwnedChild(2229510) == null)
                {
                    Effect = CombatMap.Current.AddEffect(BattleEffectType.HitGround);
                }
            }

            if (Effect != null)
            {
                var xyz = new Point3D();
                Effect.FPath.Add(xyz);
                var pathX = FPath[FFrame];
                Effect.FPath[0].X = pathX.X;
                var path = FPath[FFrame];
                Effect.FPath[0].Y = path.Y;
            }
        }

        try
        {
            if (FCancelled)
            {
                FFrame = -1;
            }
            else
            {
                FFrame += 1;
                switch (FEffectType)
                {
                    case BattleEffectType.Arrow:
                    case BattleEffectType.Rock:
                    case BattleEffectType.Comet:
                    case BattleEffectType.Breath:
                    case BattleEffectType.CometBall:
                    case BattleEffectType.Swarm:
                        switch (FAbilityIndex)
                        {
                            case 44:
                            case 45:
                            case 57:
                            case 58:
                            case 123:
                                FFrame += 1;
                                break;
                            case 52:
                                switch (FSpellIndex)
                                {
                                    case 117:
                                        FFrame += 1;
                                        break;
                                }

                                break;
                        }

                        if (FFrame == 0 && CombatMap.Current.FPlaySounds && FAbilityIndex == 52 && FSpellIndex == 122)
                        {
                            AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                        }

                        var count = FPath.Count;
                        if (FFrame >= count - 1)
                        {
                            if (CombatMap.Current.FPlaySounds)
                            {
                                if (FAbilityIndex == 52)
                                {
                                    AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            FFrame = FPath.Count - 1;
                            FDone = true;
                            if (FTarget == null)
                            {
                                MakeImpact();
                            }

                            var pathY = FPath[FFrame];
                            var pathZ = FPath[FFrame];
                            HexGeometry.HPtoHX(FPath[FFrame].X, pathY.Y - pathZ.Z, ref I, ref J);
                            CombatMap.Current.MakeVisible(I, J, 0, 2);
                        }

                        break;
                    case BattleEffectType.DecayComet:
                        if (FFrame == 0 && CombatMap.Current.FPlaySounds && FAbilityIndex == 52)
                        {
                            AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                        }

                        var fPathCount = FPath.Count;
                        if (FFrame >= fPathCount - 1)
                        {
                            if (CombatMap.Current.FPlaySounds)
                            {
                                if (FAbilityIndex == 52)
                                {
                                    AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            FFrame = FPath.Count - 1;
                            FDone = true;
                        }

                        break;
                    case BattleEffectType.AltArrow:
                        if (FFrame == 0 && CombatMap.Current.FPlaySounds && FAbilityIndex == 52)
                        {
                            AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                        }

                        FFrame += 3;
                        var count2 = FPath.Count;
                        if (FFrame >= count2 - 1)
                        {
                            if (CombatMap.Current.FPlaySounds)
                            {
                                if (FAbilityIndex == 52)
                                {
                                    AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            FFrame = FPath.Count - 1;
                            FDone = true;
                            if (FTarget == null)
                            {
                                MakeImpact();
                            }

                            var pathY = FPath[FFrame];
                            var pathZ = FPath[FFrame];
                            HexGeometry.HPtoHX(FPath[FFrame].X, pathY.Y - pathZ.Z, ref I, ref J);
                            CombatMap.Current.MakeVisible(I, J, 0, 2);
                        }

                        break;
                    case BattleEffectType.ScreenNS:
                        FFrame += 1;
                        var count3 = FPath.Count;
                        if (FFrame >= count3 - 1)
                        {
                            if (CombatMap.Current.FPlaySounds)
                            {
                                if (FAbilityIndex == 52)
                                {
                                    AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            FFrame = FPath.Count - 1;
                            FDone = true;
                            Effect = CombatMap.Current.AddEffect(BattleEffectType.HailImpact);
                            Effect.FAbilityIndex = FAbilityIndex;
                            Effect.FSpellIndex = FSpellIndex;
                            var xyz = new Point3D();
                            Effect.FPath.Add(xyz);
                            var pathX = FPath[FFrame];
                            Effect.FPath[0].X = pathX.X;
                            var pathY = FPath[FFrame];
                            Effect.FPath[0].Y = pathY.Y;
                        }

                        break;
                    case BattleEffectType.HailImpact:
                        if (FAbilityIndex == 52)
                        {
                            Sequence = AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[2];
                        }
                        else
                        {
                            Sequence = AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[2];
                        }

                        if (Sequence != null)
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames - 1)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }
                        else
                        {
                            FFrame = -1;
                        }

                        break;
                    case BattleEffectType.PrayerGlow:
                        if (FAbilityIndex == 52)
                        {
                            Sequence = AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[1];
                        }
                        else
                        {
                            Sequence = AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[1];
                        }

                        if (Sequence != null)
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames - 1)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }
                        else
                        {
                            FFrame = -1;
                        }

                        break;
                    case BattleEffectType.SFOverlay:
                        var count4 = FPath.Count;
                        if (FFrame >= count4 - 1)
                        {
                            FFrame = FPath.Count - 1;
                            FDone = true;
                        }

                        break;
                    case BattleEffectType.ScreenSwarm:
                        if (FFrame == 0)
                        {
                            FFrame = 1;
                        }

                        var count5 = FPath.Count;
                        if (FFrame >= count5 - 1)
                        {
                            if (CombatMap.Current.FPlaySounds)
                            {
                                if (FTarget != null && FAbilityIndex == 52)
                                {
                                    AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            FFrame = FPath.Count - 1;
                            FDone = true;
                        }

                        break;
                    case BattleEffectType.Ray:
                        FFrame += 12;
                        var count6 = FPath.Count;
                        if (FFrame >= count6 - 1)
                        {
                            if (CombatMap.Current.FPlaySounds)
                            {
                                if (FAbilityIndex == 52)
                                {
                                    AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            FFrame = FPath.Count - 1;
                            FDone = true;
                        }

                        break;
                    case BattleEffectType.FrostBeam:
                        switch (FFrame)
                        {
                            case >= 0 and <= 999:
                                FFrame += 1;
                                var count7 = FPath.Count;
                                if (FFrame >= count7 - 1)
                                {
                                    FFrame = 999;
                                }

                                J = Random(5) + 5;
                                HexGeometry.HXtoHP(FTargetX, FTargetY, ref K, ref X);
                                {
                                    I = 0;
                                    int last = J;
                                    for (; I <= last; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var random = Random(100);
                                        var random2 = Random(5);
                                        var random3 = Random(100);
                                        EffectPaths.MakeArcPathXY((short)(K - 200), (short)(X - 200), (short)(random3 + K - 50), (short)(random + X - 50), (short)(random2 + 5), 100, ref Effect.FPath);
                                    }
                                }

                                J = Random(10) + 10;
                                {
                                    I = 0;
                                    int last = J;
                                    for (; I <= last; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)((double)(Random(50) + 20) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)((double)Random(10) / 10);
                                        Z = 100;
                                        var random = Random(360);
                                        EffectPaths.MakeArcPath(K - 200, X - 100, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                break;
                            case >= 1000 and <= 1029:
                                HexGeometry.HXtoHP(FTargetX, FTargetY, ref K, ref X);
                                J = Random(5) + 5;
                                {
                                    I = 0;
                                    int last = J;
                                    for (; I <= last; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var random = Random(100);
                                        var random2 = Random(5);
                                        var random3 = Random(100);
                                        EffectPaths.MakeArcPathXY((short)(K - 200), (short)(X - 200), (short)(random3 + K - 50), (short)(random + X - 50), (short)(random2 + 5), 100, ref Effect.FPath);
                                    }
                                }

                                J = Random(10) + 10;
                                {
                                    I = 0;
                                    int last = J;
                                    for (; I <= last; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)((double)(Random(50) + 20) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)((double)Random(10) / 10);
                                        Z = 100;
                                        var random = Random(360);
                                        EffectPaths.MakeArcPath(K - 200, X - 100, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                J = Random(5) + 5;
                                {
                                    I = 0;
                                    int last = J;
                                    for (; I <= last; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)((double)(Random(20) + 20) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)((double)(Random(50) + 20) / 10);
                                        Z = 0;
                                        var count8 = FPath.Count;
                                        var path = FPath[count8 - 1];
                                        var count9 = FPath.Count;
                                        var pathZ = FPath[count9 - 1];
                                        var count10 = FPath.Count;
                                        var pathX = FPath[count10 - 1];
                                        var random = Random(360);
                                        EffectPaths.MakeArcPath(pathX.X, path.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                break;
                            case 1030:
                                FFrame = 1999;
                                break;
                            case >= 2000 and <= 2999:
                                FFrame += 1;
                                var count11 = FPath.Count;
                                if (FFrame >= count11 + 2000 - 1)
                                {
                                    FFrame = 2999;
                                }

                                J = Random(5) + 5;
                                {
                                    I = 0;
                                    int last = J;
                                    for (; I <= last; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)((double)(Random(20) + 20) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)((double)(Random(50) + 20) / 10);
                                        Z = 0;
                                        var count8 = FPath.Count;
                                        var path = FPath[count8 - 1];
                                        var count9 = FPath.Count;
                                        var pathZ = FPath[count9 - 1];
                                        var count10 = FPath.Count;
                                        var pathX = FPath[count10 - 1];
                                        var random = Random(360);
                                        EffectPaths.MakeArcPath(pathX.X, path.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                break;
                            case 3000:
                                if (CombatMap.Current.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                    }
                                }

                                {
                                    I = 0;
                                    int last = FPath.Count - 1;
                                    for (; I <= last; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.Frost);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)((double)(Random(25) + 25) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)((double)(Random(50) + 40) / 10);
                                        Z = 0;
                                        var pathY = FPath[I];
                                        var pathZ = FPath[I];
                                        var random = Random(360);
                                        EffectPaths.MakeArcPath(FPath[I].X, pathY.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                J = Random(10) + 100;
                                {
                                    I = 0;
                                    int last = J;
                                    for (; I <= last; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.Frost);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)((double)(Random(50) + 50) / 10);
                                        Gravity = (float)0.1;
                                        VZ = (float)((double)(Random(5) + 5) / 10);
                                        Z = 0;
                                        var count8 = FPath.Count;
                                        var path = FPath[count8 - 1];
                                        var count9 = FPath.Count;
                                        var pathZ = FPath[count9 - 1];
                                        var count10 = FPath.Count;
                                        var pathX = FPath[count10 - 1];
                                        var random = Random(360);
                                        EffectPaths.MakeArcPath(pathX.X, path.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                FDone = true;
                                FTarget = null;
                                break;
                        }

                        break;
                    case BattleEffectType.ShockWave:
                        switch (FFrame)
                        {
                            case >= 0 and <= 29:
                                if (FFrame == 0 && CombatMap.Current.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                                    }
                                }

                                {
                                    I = 1;
                                    for (; I <= 20; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var random = Random(360);
                                        var pathX = FPath[0];
                                        var pathY = FPath[0];
                                        var random2 = Random(2);
                                        var random3 = Random(10);
                                        EffectPaths.MakeSwarmPath(Random(10) + 10, random3 + FFrame / 4, I % 6 * 60 + FFrame, random, pathX.X, pathY.Y, (SwarmDirection)random2, ref Effect.FPath);
                                    }
                                }

                                break;
                            case >= 30 and <= 59:
                                {
                                    I = 1;
                                    for (; I <= 20; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var random = Random(360);
                                        var pathX = FPath[0];
                                        var pathY = FPath[0];
                                        var random2 = Random(2);
                                        var random3 = Random(10);
                                        EffectPaths.MakeSwarmPath(Random(10) + 10, random3 + (60 - FFrame) / 4, I % 6 * 60 + FFrame, random, pathX.X, pathY.Y, (SwarmDirection)random2, ref Effect.FPath);
                                    }
                                }

                                break;
                            case >= 60 and <= 89:
                                {
                                    I = 1;
                                    for (; I <= 20; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var random = Random(360);
                                        var pathX = FPath[0];
                                        var pathY = FPath[0];
                                        var random2 = Random(2);
                                        var random3 = Random(10);
                                        EffectPaths.MakeSwarmPath(Random(10) + 10, random3 + (FFrame - 60) / 2, I % 6 * 60 + FFrame, random, pathX.X, pathY.Y, (SwarmDirection)random2, ref Effect.FPath);
                                    }
                                }

                                break;
                            case >= 90 and <= 119:
                                {
                                    I = 1;
                                    for (; I <= 20; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var random = Random(360);
                                        var pathX = FPath[0];
                                        var pathY = FPath[0];
                                        var random2 = Random(2);
                                        var random3 = Random(10);
                                        EffectPaths.MakeSwarmPath(Random(10) + 10, random3 + (120 - FFrame) / 2, I % 6 * 60 + FFrame, random, pathX.X, pathY.Y, (SwarmDirection)random2, ref Effect.FPath);
                                    }
                                }

                                break;
                            case 150:
                                if (CombatMap.Current.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                    }
                                }

                                X = 200;
                                Y = 200;
                                J = Random(5);
                                {
                                    I = 0;
                                    for (; I <= 359; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        X = -X;
                                        var pi = Math.PI;
                                        var sin = Math.Sin((double)((double)(I + J) * pi) / 180);
                                        var random = Random(10);
                                        var pathY = FPath[0];
                                        var mathPI = Math.PI;
                                        var cos = Math.Cos((double)((double)(I + J) * mathPI) / 180);
                                        var random2 = Random(10);
                                        var pathX = FPath[0];
                                        var path = FPath[0];
                                        EffectPaths.MakeLinePath(FPath[0].X, path.Y, pathX.X + Round((double)cos * (random2 + X)), pathY.Y + Round((double)sin * (random + Y)) / 2, 75, ref Effect.FPath);
                                        if (I % 10 == 0)
                                        {
                                            K = 1;
                                            for (; K <= 10; K += 1)
                                            {
                                                Effect2 = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                                Effect2.FAbilityIndex = FAbilityIndex;
                                                Effect2.FSpellIndex = FSpellIndex;
                                                var count7 = Effect.FPath.Count;
                                                var path2 = Effect.FPath[count7 - 1];
                                                var count8 = Effect.FPath.Count;
                                                var path3 = Effect.FPath[count8 - 1];
                                                var path4 = FPath[0];
                                                EffectPaths.MakeLinePath(FPath[0].X, path4.Y, path3.X, path2.Y, K * 5 + 75, ref Effect2.FPath);
                                            }
                                        }
                                    }
                                }

                                break;
                            case >= 151 and <= 169:
                            case >= 210 and <= 229:
                                var path5 = FPath[0];
                                HexGeometry.HPtoHX(FPath[0].X, path5.Y, ref X, ref Y);
                                Msg.MsgID = 200720;
                                Msg.Param[0] = 1;
                                Msg.Result = -1;
                                {
                                    I = 1;
                                    for (; I <= 36; I += 1)
                                    {
                                        HexGeometry.CenterHNtoHX(X, Y, I, ref J, ref K);
                                        CombatMap.Current.MapContainer.MapLevels[0].Field[J, K].SendMapFieldMsg(ref Msg);
                                    }
                                }

                                break;
                            case >= 170 and <= 184:
                            case >= 195 and <= 209:
                                var path6 = FPath[0];
                                HexGeometry.HPtoHX(FPath[0].X, path6.Y, ref X, ref Y);
                                Msg.MsgID = 200720;
                                Msg.Param[0] = 2;
                                Msg.Result = -1;
                                {
                                    I = 1;
                                    for (; I <= 36; I += 1)
                                    {
                                        HexGeometry.CenterHNtoHX(X, Y, I, ref J, ref K);
                                        CombatMap.Current.MapContainer.MapLevels[0].Field[J, K].SendMapFieldMsg(ref Msg);
                                    }
                                }

                                break;
                            case >= 185 and <= 194:
                                var path7 = FPath[0];
                                HexGeometry.HPtoHX(FPath[0].X, path7.Y, ref X, ref Y);
                                Msg.MsgID = 200720;
                                Msg.Param[0] = 3;
                                Msg.Result = -1;
                                {
                                    I = 1;
                                    for (; I <= 36; I += 1)
                                    {
                                        HexGeometry.CenterHNtoHX(X, Y, I, ref J, ref K);
                                        CombatMap.Current.MapContainer.MapLevels[0].Field[J, K].SendMapFieldMsg(ref Msg);
                                    }
                                }

                                break;
                            case 230:
                                var path8 = FPath[0];
                                HexGeometry.HPtoHX(FPath[0].X, path8.Y, ref X, ref Y);
                                Msg.MsgID = 200721;
                                Msg.Result = -1;
                                {
                                    I = 1;
                                    for (; I <= 36; I += 1)
                                    {
                                        HexGeometry.CenterHNtoHX(X, Y, I, ref J, ref K);
                                        CombatMap.Current.MapContainer.MapLevels[0].Field[J, K].SendMapFieldMsg(ref Msg);
                                    }
                                }

                                FFrame = -1;
                                break;
                        }

                        break;
                    case BattleEffectType.Winds:
                        switch (FFrame)
                        {
                            case >= 0 and <= 1999:
                                var count7 = FPath.Count;
                                if (FFrame >= count7 + 20)
                                {
                                    if (FTarget != null && FTarget.GetEnabled())
                                    {
                                        if (CombatMap.Current.FPlaySounds)
                                        {
                                            if (FAbilityIndex == 52)
                                            {
                                                AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                            }
                                            else
                                            {
                                                AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                            }
                                        }

                                        var count8 = FPath.Count;
                                        var path = FPath[count8 - 1];
                                        var count9 = FPath.Count;
                                        HexGeometry.HPtoHX(FPath[count9 - 1].X, path.Y, ref J, ref K);
                                        var mapField = CombatMap.Current.MapContainer.MapLevels[0].Field[J, K];
                                        ((TacticalBattleUnit)FTarget).FUnitHS.MoveTo(mapField, 0);
                                        ((TacticalBattleUnit)FTarget).FUnitHS.FOffsetX = 0;
                                        ((TacticalBattleUnit)FTarget).FUnitHS.FOffsetY = 0;
                                        ((TacticalBattleUnit)FTarget).FUnitHS.FBusy = false;
                                    }

                                    FFrame = -1;
                                }
                                else
                                {
                                    if (CombatMap.Current.FPlaySounds && (FFrame == FTargetX || FFrame == FTargetY))
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                                        }
                                        else
                                        {
                                            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                                        }
                                    }

                                    if (FTarget != null && FTarget.GetEnabled())
                                    {
                                        var count8 = FPath.Count;
                                        if (FFrame - 20 < count8 && FFrame >= 20)
                                        {
                                            var pathX = FPath[FFrame - 20];
                                            var path = FPath[0];
                                            ((TacticalBattleUnit)FTarget).FUnitHS.FOffsetX = pathX.X - path.X;
                                            var pathY = FPath[FFrame - 20];
                                            var pathZ = FPath[FFrame - 20];
                                            var path2 = FPath[0];
                                            ((TacticalBattleUnit)FTarget).FUnitHS.FOffsetY = pathY.Y - pathZ.Z - path2.Y;
                                            ((TacticalBattleUnit)FTarget).FUnitHS.SetDirection((byte)(((TacticalBattleUnit)FTarget).FUnitHS.FDirection + 1));
                                            if (((TacticalBattleUnit)FTarget).FUnitHS.FDirection > 6)
                                            {
                                                ((TacticalBattleUnit)FTarget).FUnitHS.SetDirection(1);
                                            }

                                            ((TacticalBattleUnit)FTarget).FUnitHS.FBusy = true;
                                        }
                                    }

                                    I = FFrame - 20;
                                    int last = FFrame;
                                    for (; I <= last; I += 1)
                                    {
                                        bool condition = I >= 0;
                                        if (condition)
                                        {
                                            var count8 = FPath.Count;
                                            condition = I < count8;
                                        }

                                        if (condition)
                                        {
                                            Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var random = Random(360);
                                            var pathX = FPath[I];
                                            var random2 = Random(10);
                                            var pathY = FPath[I];
                                            var pathZ = FPath[I];
                                            var random3 = Random(10);
                                            var random4 = Random(2);
                                            var count8 = FPath.Count;
                                            EffectPaths.MakeSwarmPath(Random(5) + 20, (count8 - I) / 5, 0, random, pathX.X + random2 - 5, pathY.Y - pathZ.Z + random3 - 5, (SwarmDirection)random4, ref Effect.FPath);
                                        }
                                    }
                                }

                                break;
                        }

                        break;
                    case BattleEffectType.SacFlame:
                        switch (FFrame)
                        {
                            case >= 0 and <= 99:
                                if (FFrame == 0 && CombatMap.Current.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                                    }
                                }

                                J = Random(6) + 10;
                                var yhx = ((TacticalBattleUnit)FTarget).FUnitHS.GetYhx();
                                HexGeometry.HXtoHP(((TacticalBattleUnit)FTarget).FUnitHS.GetXhx(), yhx, ref K, ref X);
                                {
                                    I = 1;
                                    int last = J - 1;
                                    for (; I <= last; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.Frost);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)((double)(Random(25) + 25) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)((double)(Random(50) + FFrame) / 10);
                                        Z = 0;
                                        EffectPaths.MakeArcPath(K + 16, X + 16, I * 360 / J, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                switch (FFrame)
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
                                        if (CombatMap.Current.FPlaySounds)
                                        {
                                            if (FAbilityIndex == 52)
                                            {
                                                AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                            }
                                            else
                                            {
                                                AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                            }
                                        }

                                        J = Random(10) + 100;
                                        {
                                            I = 0;
                                            int last = J;
                                            for (; I <= last; I += 1)
                                            {
                                                Effect = CombatMap.Current.AddEffect(BattleEffectType.Frost);
                                                Effect.FAbilityIndex = FAbilityIndex;
                                                Effect.FSpellIndex = FSpellIndex;
                                                Speed = (float)((double)(Random(50) + 50) / 10);
                                                Gravity = (float)0.1;
                                                VZ = (float)((double)(Random(5) + 5) / 10);
                                                Z = 0;
                                                var random = Random(360);
                                                EffectPaths.MakeArcPath(K + 16, X + 16, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                            }
                                        }

                                        break;
                                }

                                break;
                            case >= 125 and <= 199:
                                if (FFrame == 125)
                                {
                                    var path = FPath[0];
                                    HexGeometry.HPtoHX(FPath[0].X, path.Y, ref I, ref J);
                                    CombatMap.Current.CenterViewTo(I, J - 3, 0);
                                }

                                if (CombatMap.Current.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                    }
                                }

                                J = Random(6) + 10;
                                var pathX = FPath[0];
                                var random2 = Random(160);
                                K = pathX.X + random2 - 80;
                                var pathY = FPath[0];
                                var random3 = Random(80);
                                X = pathY.Y + random3 - 40;
                                {
                                    I = 1;
                                    int last = J - 1;
                                    for (; I <= last; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.Frost);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)((double)(Random(50) + 25) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)((double)(Random(50) + 100) / 10);
                                        Z = 0;
                                        EffectPaths.MakeArcPath(K, X, I * 360 / J, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                J = Random(10) + 50;
                                {
                                    I = 0;
                                    int last = J;
                                    for (; I <= last; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.Frost);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)((double)(Random(50) + 50) / 10);
                                        Gravity = (float)0.1;
                                        VZ = (float)((double)(Random(5) + 5) / 10);
                                        Z = 0;
                                        var random = Random(360);
                                        EffectPaths.MakeArcPath(K, X, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                break;
                            case 200:
                                FFrame = -1;
                                break;
                        }

                        break;
                    case BattleEffectType.MindDecay:
                        switch (FFrame)
                        {
                            case >= 0 and <= 49:
                                if (FFrame % 2 > 0)
                                {
                                    I = 0;
                                    int last = FFrame;
                                    for (; I <= last; I += 1)
                                    {
                                        J = 0;
                                        int limit = I / 25;
                                        for (; J <= limit; J += 1)
                                        {
                                            Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var random = Random(360);
                                            var pathX = FPath[0];
                                            var pathY = FPath[0];
                                            var random2 = Random(2);
                                            EffectPaths.MakeSwarmPath(Random(10) + 10, I / 2, 45, random, pathX.X + 100 - I, pathY.Y - 200 - 50 + I, (SwarmDirection)random2, ref Effect.FPath);
                                        }
                                    }
                                }

                                break;
                            case >= 50 and <= 149:
                                if (FFrame % 2 > 0)
                                {
                                    I = 0;
                                    for (; I <= 50; I += 1)
                                    {
                                        J = 0;
                                        int last = I / 25;
                                        for (; J <= last; J += 1)
                                        {
                                            Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var random = Random(360);
                                            var pathX = FPath[0];
                                            var pathY = FPath[0];
                                            var random2 = Random(2);
                                            EffectPaths.MakeSwarmPath(Random(10) + 10, I / 2, 45, random, pathX.X + 100 - I, pathY.Y - 200 - 50 + I, (SwarmDirection)random2, ref Effect.FPath);
                                        }
                                    }
                                }

                                if (FFrame % 5 == 0 && FFrame < 85)
                                {
                                    Effect = CombatMap.Current.AddEffect(BattleEffectType.DecayComet);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    var pathY = FPath[0];
                                    var random = Random(50);
                                    var random2 = Random(5);
                                    var pathX = FPath[0];
                                    var random3 = Random(50);
                                    var path = FPath[0];
                                    var random4 = Random(50);
                                    var path2 = FPath[0];
                                    var random5 = Random(50);
                                    EffectPaths.MakeArcPathXY((short)(path2.X + random5 + 25), (short)(path.Y + random4 - 225), (short)(pathX.X + random3 - 25), (short)(pathY.Y + random - 25), (short)(random2 + 5), 100, ref Effect.FPath);
                                    I = 0;
                                    for (; I <= 10; I += 1)
                                    {
                                        Speed = (float)((double)(Random(50) + 50) / 10);
                                        Gravity = (float)1.5;
                                        VZ = (float)((double)(Random(25) + 25) / 10);
                                        Z = 0;
                                        var count7 = Effect.FPath.Count;
                                        var path3 = Effect.FPath[count7 - 1];
                                        var count8 = Effect.FPath.Count;
                                        var pathZ = Effect.FPath[count8 - 1];
                                        var count9 = Effect.FPath.Count;
                                        var path4 = Effect.FPath[count9 - 1];
                                        var random6 = Random(360);
                                        EffectPaths.MakeArcPath(path4.X, path3.Y - pathZ.Z, random6, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }

                                    HexGeometry.CenterHNtoHX(FTargetX, FTargetY, (FFrame - 50) / 5, ref J, ref K);
                                    // { Original swaps the converted hex coordinates. }
                                    HexGeometry.HXtoHP(J, K, ref X, ref Y);
                                    UnitHS = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].Field[J, K].FindOwnedChild(2228484);
                                    if (UnitHS != null && UnitHS.FUnit.GetSide() != FTarget.GetSide())
                                    {
                                        Effect.FTarget = UnitHS.FUnit;
                                    }
                                    else
                                    {
                                        Effect.FTarget = null;
                                    }

                                    if (Effect.FTarget != null)
                                    {
                                        var random6 = Random(3);
                                        var count7 = Effect.FPath.Count;
                                        var path3 = Effect.FPath[count7 - 1];
                                        var count8 = Effect.FPath.Count;
                                        var pathZ = Effect.FPath[count8 - 1];
                                        var count9 = Effect.FPath.Count;
                                        EffectPaths.MakeArcPathXY((short)Effect.FPath[count9 - 1].X, (short)(path3.Y - pathZ.Z), (short)(X + 16), (short)(Y - 8), (short)(random6 + 6), 100, ref Effect.FPath);
                                        I = 0;
                                        int last = Random(3) + 5;
                                        for (; I <= last; I += 1)
                                        {
                                            var count10 = Effect.FPath.Count;
                                            var path4 = Effect.FPath[count10 - 1];
                                            var count11 = Effect.FPath.Count;
                                            var path5 = Effect.FPath[count11 - 1];
                                            var count12 = Effect.FPath.Count;
                                            var path6 = Effect.FPath[count12 - 1];
                                            var count13 = Effect.FPath.Count;
                                            var path7 = Effect.FPath[count13 - 1];
                                            var count14 = Effect.FPath.Count;
                                            var path8 = Effect.FPath[count14 - 1];
                                            var count15 = Effect.FPath.Count;
                                            EffectPaths.MakeLinePath(Effect.FPath[count15 - 1].X, path6.Y - path7.Z, path8.X, path4.Y - path5.Z - 6, 3, ref Effect.FPath);
                                            var count16 = Effect.FPath.Count;
                                            var path9 = Effect.FPath[count16 - 1];
                                            var count17 = Effect.FPath.Count;
                                            var item = Effect.FPath[count17 - 1];
                                            var count18 = Effect.FPath.Count;
                                            var fPathItem = Effect.FPath[count18 - 1];
                                            var count19 = Effect.FPath.Count;
                                            var path10 = Effect.FPath[count19 - 1];
                                            var count20 = Effect.FPath.Count;
                                            var path11 = Effect.FPath[count20 - 1];
                                            var count21 = Effect.FPath.Count;
                                            EffectPaths.MakeLinePath(Effect.FPath[count21 - 1].X, fPathItem.Y - path10.Z, path11.X, path9.Y - item.Z + 6, 3, ref Effect.FPath);
                                        }
                                    }
                                }

                                break;
                            case 200:
                                FFrame = -1;
                                FTarget = null;
                                break;
                        }

                        break;
                    case BattleEffectType.Chain:
                        NewPath = new Point3DList();
                        I = 0;
                        J = 0;
                        K = 0;
                        while (true)
                        {
                            var count7 = FPath.Count;
                            if (!(I < count7))
                                break;
                            if (FPath[I].Z > 0)
                            {
                                if (FPath[I].Z > 1)
                                {
                                    K = FPath[I].Z;
                                }

                                var pathY = FPath[I];
                                var pathX = FPath[I];
                                var path = FPath[J];
                                EffectPaths.MakeLightningPath(FPath[J].X, path.Y, pathX.X, pathY.Y, ref NewPath);
                                switch (K)
                                {
                                    case -1:
                                        var newPathCount = NewPath.Count;
                                        NewPath[newPathCount - 1].Z = 16;
                                        FFrame = NewPath.Count - 1;
                                        K = 0;
                                        break;
                                    case >= 2 and <= 16:
                                        var count8 = NewPath.Count;
                                        NewPath[count8 - 1].Z = K - 1;
                                        FFrame = NewPath.Count - 1;
                                        if (K == 2)
                                        {
                                            K = -1;
                                        }
                                        else
                                        {
                                            K = 0;
                                        }

                                        break;
                                }

                                J = I;
                            }

                            I += 1;
                        }

                        FPath?.Dispose();
                        FPath = NewPath;
                        var count22 = FPath.Count;
                        if (FFrame >= count22 - 1)
                        {
                            FFrame = FPath.Count - 1;
                            if (FPath[FFrame].Z <= 1)
                            {
                                FDone = true;
                            }
                        }

                        if (FPath[FFrame].Z == 15)
                        {
                            if (CombatMap.Current.FPlaySounds)
                            {
                                if (FAbilityIndex == 52)
                                {
                                    AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            var pathY = FPath[FFrame];
                            var pathZ = FPath[FFrame];
                            HexGeometry.HPtoHX(FPath[FFrame].X, pathY.Y - pathZ.Z, ref I, ref J);
                            CombatMap.Current.MakeVisible(I, J, 0, 3);
                        }

                        break;
                    case BattleEffectType.Touch:
                    case BattleEffectType.Effect:
                        if (FFrame == 0 && CombatMap.Current.FPlaySounds && FAbilityIndex == 65)
                        {
                            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                        }

                        if (FAbilityIndex == 52)
                        {
                            Sequence = AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[0];
                        }
                        else
                        {
                            Sequence = AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[0];
                        }

                        if (Sequence != null)
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames - 1)
                            {
                                if (CombatMap.Current.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                    }
                                }

                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }
                        else
                        {
                            FFrame = -1;
                        }

                        break;
                    case BattleEffectType.SacredWrath:
                        if (FAbilityIndex == 52)
                        {
                            Sequence = AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[1];
                        }
                        else
                        {
                            Sequence = AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[1];
                        }

                        if (Sequence == null)
                        {
                            FFrame = -1;
                        }

                        break;
                    case BattleEffectType.FBExplosion:
                        Sequence = AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[1];
                        if (Sequence != null)
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames - 1)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }
                        else
                        {
                            FFrame = -1;
                        }

                        break;
                    case BattleEffectType.ScreenNESW:
                        if (FFrame == 0)
                        {
                            if (FPath[0].X == 0)
                            {
                                FPath[0].X = 100;
                                var random = Random(50);
                                FPath[0].Y = random;
                            }
                            else
                            {
                                var random = Random(50);
                                FPath[0].X = random + 50;
                                FPath[0].Y = 0;
                            }
                        }
                        else
                        {
                            var pathX = FPath[0];
                            pathX.X -= 3;
                            var pathY = FPath[0];
                            pathY.Y += 3;
                            if (FPath[0].X < 0)
                            {
                                FFrame = -1;
                            }
                            else
                            {
                                if (FPath[0].Y > 100)
                                {
                                    FFrame = -1;
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
                        var count23 = FPath.Count;
                        if (FFrame >= count23 - 1)
                        {
                            FFrame = FPath.Count - 1;
                            FDone = true;
                        }

                        break;
                    case BattleEffectType.WallSmoke:
                        Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[5];
                        if (Sequence == null)
                        {
                            FFrame = -1;
                        }
                        else
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }

                        break;
                    case BattleEffectType.HitGround:
                        Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[9];
                        if (Sequence == null)
                        {
                            FFrame = -1;
                        }
                        else
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }

                        break;
                    case BattleEffectType.HitWater:
                        Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[10];
                        if (Sequence == null)
                        {
                            FFrame = -1;
                        }
                        else
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }

                        break;
                    case BattleEffectType.Blood:
                        var pathZ2 = FPath[0];
                        Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[pathZ2.Z];
                        if (Sequence == null)
                        {
                            FFrame = -1;
                        }
                        else
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames - 1)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }

                        break;
                    case BattleEffectType.Tremors:
                        switch (FFrame)
                        {
                            case 0:
                                var xyz = new Point3D();
                                FPath.Add(xyz);
                                FPath[0].X = CombatMap.Current.ViewXhp;
                                FPath[0].Y = CombatMap.Current.ViewYhp;
                                break;
                            case >= 1 and <= 29:
                                var pathY = FPath[0];
                                var random = Random(FFrame / 3);
                                var pathX = FPath[0];
                                var random2 = Random(FFrame / 3);
                                CombatMap.Current.ChangeViewTo(pathX.X + random2 - FFrame / 6, pathY.Y + random - FFrame / 6, CombatMap.Current.ViewL);
                                break;
                            case >= 30 and <= 89:
                                var path = FPath[0];
                                var random3 = Random(10);
                                var path2 = FPath[0];
                                var random4 = Random(10);
                                CombatMap.Current.ChangeViewTo(path2.X + random4 - 5, path.Y + random3 - 5, CombatMap.Current.ViewL);
                                break;
                            case >= 90 and <= 119:
                                var path3 = FPath[0];
                                var random5 = Random((119 - FFrame) / 3);
                                var path4 = FPath[0];
                                var random6 = Random((119 - FFrame) / 3);
                                CombatMap.Current.ChangeViewTo(path4.X + random6 - (119 - FFrame) / 6, path3.Y + random5 - (119 - FFrame) / 6, CombatMap.Current.ViewL);
                                break;
                            case 120:
                                FFrame = -1;
                                break;
                        }

                        break;
                    case BattleEffectType.Cloud:
                        switch (FFrame)
                        {
                            case 250:
                                FFrame = -1;
                                break;
                            case >= 1 and <= 149:
                                MakeCloud(FFrame, FFrame);
                                break;
                            case >= 150 and <= 199:
                                MakeCloud(150, 150);
                                break;
                            case >= 200 and <= 249:
                                MakeCloud((250 - FFrame) * 3, 150);
                                break;
                        }

                        break;
                    case BattleEffectType.CloudPuff:
                        if (FAbilityIndex == 52)
                        {
                            Sequence = AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[0];
                        }
                        else
                        {
                            Sequence = AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[0];
                        }

                        if (Sequence != null)
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames - 1)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }
                        else
                        {
                            FFrame = -1;
                        }

                        break;
                    case BattleEffectType.HighPrayer:
                        switch (FFrame)
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
                                    I = 0;
                                    int last = CombatMap.Current.FCombat.FCombatData.GetCount() - 1;
                                    for (; I <= last; I += 1)
                                    {
                                        if (CombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && CombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled() && CombatMap.Current.FCombat.FCombatData.GetObjects(I).FCombatPlayer == FTarget.FCombatPlayer)
                                        {
                                            Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                            var yhx = ((TacticalBattleUnit)CombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
                                            HexGeometry.HXtoHP(((TacticalBattleUnit)CombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx(), yhx, ref J, ref K);
                                            J = Random(20) + (J + 6);
                                            K = Random(20) + (K + 6);
                                            EffectPaths.MakeLinePath(J, K - 100, J, K, 50, ref Effect.FPath);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                        }
                                    }
                                }

                                break;
                            case 100:
                                {
                                    I = 0;
                                    int last = CombatMap.Current.FCombat.FCombatData.GetCount() - 1;
                                    for (; I <= last; I += 1)
                                    {
                                        if (CombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && CombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled() && CombatMap.Current.FCombat.FCombatData.GetObjects(I).FCombatPlayer == FTarget.FCombatPlayer)
                                        {
                                            Effect = CombatMap.Current.AddEffect(BattleEffectType.PrayerGlow);
                                            var yhx = ((TacticalBattleUnit)CombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
                                            HexGeometry.HXtoHP(((TacticalBattleUnit)CombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx(), yhx, ref J, ref K);
                                            var xyz = new Point3D();
                                            Effect.FPath.Add(xyz);
                                            Effect.FPath[0].X = J + 16;
                                            Effect.FPath[0].Y = K + 16;
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var combatObject = CombatMap.Current.FCombat.FCombatData.GetObjects(I);
                                            Effect.FTarget = combatObject;
                                        }
                                    }
                                }

                                FFrame = -1;
                                FTarget = null;
                                break;
                        }

                        break;
                    case BattleEffectType.Resurrect:
                        switch (FFrame)
                        {
                            case 100:
                                FFrame = -1;
                                break;
                        }

                        break;
                    case BattleEffectType.DecayLeg:
                        var count24 = FPath.Count;
                        if (FFrame >= count24)
                        {
                            FFrame = -1;
                        }

                        break;
                    case BattleEffectType.Geyser:
                        switch (FFrame)
                        {
                            case 150:
                                FFrame = -1;
                                if (FTarget != null && FTarget.GetEnabled())
                                {
                                    if (CombatMap.Current.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                        }
                                    }

                                    ((TacticalBattleUnit)FTarget).FUnitHS.FOffsetY = 0;
                                    ((TacticalBattleUnit)FTarget).FUnitHS.FBusy = false;
                                }

                                if (FTarget != null && ((CombatUnit)FTarget).FUnit.GetMoveTypes().Contains(MoveType.Flying))
                                {
                                    FTarget = null;
                                }

                                break;
                            case >= 0 and <= 99:
                                if (CombatMap.Current.FPlaySounds && FFrame % 30 == 29)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                                    }
                                }

                                if (FTarget != null && FTarget.GetEnabled())
                                {
                                    var random = Random(2);
                                    ((TacticalBattleUnit)FTarget).FUnitHS.FOffsetY = random + -(FFrame * 3 / 2) - 1;
                                    ((TacticalBattleUnit)FTarget).FUnitHS.FBusy = true;
                                }

                                J = Random(6) + 20;
                                {
                                    I = 1;
                                    int last = J - 1;
                                    for (; I <= last; I += 1)
                                    {
                                        Effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)((double)(Random(10) + 10) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)((double)((double)Random(50) + (double)(FFrame * 3) * 0.5) / 10);
                                        Z = 0;
                                        var path = FPath[0];
                                        EffectPaths.MakeArcPath(FPath[0].X, path.Y, I * 360 / J, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                break;
                            case >= 125 and <= 149:
                                if (FTarget != null && FTarget.GetEnabled())
                                {
                                    ((TacticalBattleUnit)FTarget).FUnitHS.FOffsetY = (FFrame - 150) * 6;
                                    ((TacticalBattleUnit)FTarget).FUnitHS.SetDirection((byte)(FFrame % 6 + 1));
                                }

                                break;
                        }

                        break;
                    case BattleEffectType.SWComet:
                        break;
                    default:
                        FFrame = -1;
                        break;
                }
            }

            if (FFrame == -1)
            {
                FDone = true;
            }
        }
        catch
        {
            // { Original puts the diagnostic in the message-box caption. }
            MessageBox(0, "Error during NewFrame", NullTerminatedText("Error - fxHS " + IntToStr(FAbilityIndex) + " " + IntToStr(FSpellIndex)), 0);
        }
    }

    // AoWTC.pas:196

}
