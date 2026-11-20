#nullable disable
// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.Draw;

// MiscellaneousImages. TargetImages. Frame. FrameCount. Selection. Range
public class RangedPathHexSprite(CustomNode owner) : FixedVisibleHexSprite(owner)
{

    // AoWTC.pas:4846
    public new const int ClassId = 2228485;

    private CustomImageLibrary _miscellaneousImages;
    private CustomImageLibrary _targetImages;
    private ushort _frame;
    private ushort _frameCount;

    public CombatUnitSelectionControl Selection { get; set; }
    public ushort Range { get; set; }

    // AoWTC.pas:4849
    public override int GetShowPriority() => 1811939328;

    // AoWTC.pas:4859
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (Selection is not null)
        {
            int pixelX;
            int pixelY;
            int hexX = default;
            int hexY = default;
            if (Range < 100)
            {
                int count8 = 6;
                int first = 1;
                for (int i = 0, last = Range - 2; i <= last; i++)
                {
                    first += count8;
                    count8 += 6;
                }

                int last2 = first + count8;
                for (int i = first; i <= last2 - 1; i++)
                {
                    sbyte yhx = Selection.TacticalBattleUnitHexSprite.HexY;
                    HexGeometry.CenterHexNumberToHex(Selection.TacticalBattleUnitHexSprite.HexX, yhx, i, ref hexX, ref hexY);
                    HexGeometry.HexToPixel(hexX, hexY, out pixelX, out pixelY);
                    _targetImages[2].Show(pixelX - pixelXSurface - 8, pixelY - pixelYSurface + 2, lockedSurface);
                }
            }

            if (Selection.RangedPath.Count > 0)
            {
                _frameCount = (ushort)(_frameCount + 1);
                if (_frameCount >= 5)
                {
                    _frameCount = 0;
                    _frame = (ushort)(_frame + 1);
                }

                if (_frame > 1)
                {
                    _frame = 0;
                }

                BattleEffectType effectType;
                if (Selection.TacticalBattleUnitHexSprite.SelectedAbility == 52)
                {
                    TacticalBattleUnitHexSprite tacticalBattleUnitHexSprite = Selection.TacticalBattleUnitHexSprite;
                    effectType = TacticalTables.SpellTypes[tacticalBattleUnitHexSprite.SelectedSpell];
                }
                else
                {
                    TacticalBattleUnitHexSprite tacticalBattleUnitHexSprite = Selection.TacticalBattleUnitHexSprite;
                    effectType = TacticalTables.AbilityTypes[tacticalBattleUnitHexSprite.SelectedAbility];
                }

                int x;
                int y;
                switch (effectType)
                {
                    case BattleEffectType.Breath:
                        int count = Selection.RangedPath.Count;
                        Point3D rangedPath = Selection.RangedPath[count - 1];
                        int fRangedPathCount = Selection.RangedPath.Count;
                        HexGeometry.PixelToHex(Selection.RangedPath[fRangedPathCount - 1].X, rangedPath.Y, out x, out y);
                        {
                            for (int i = 0; i <= 11; i++)
                            {
                                sbyte yhx = Selection.TacticalBattleUnitHexSprite.HexY;
                                int deltaHexToHexNumber2 = HexGeometry.DeltaHexToHexNumber(Selection.TacticalBattleUnitHexSprite.HexX, yhx, x, y);
                                sbyte hexY2 = Selection.TacticalBattleUnitHexSprite.HexY;
                                HexGeometry.CenterHexNumberToHex(Selection.TacticalBattleUnitHexSprite.HexX, hexY2, TacticalTables.BreathHexNumber[(deltaHexToHexNumber2 * 12) + i - 12], ref hexX, ref hexY);
                                HexGeometry.HexToPixel(hexX, hexY, out pixelX, out pixelY);
                                _targetImages[0].ShowClipped(pixelX - 8 - pixelXSurface, pixelY + 2 - pixelYSurface, lockedSurface);
                            }
                        }

                        break;
                    case BattleEffectType.None:
                    case BattleEffectType.Arrow:
                    case BattleEffectType.Rock:
                    case BattleEffectType.Comet:
                    case BattleEffectType.Touch:
                    case BattleEffectType.CometBall:
                    case BattleEffectType.Effect:
                    case BattleEffectType.FBExplosion:
                    case BattleEffectType.ScreenNESW:
                    case BattleEffectType.ScreenNS:
                    case BattleEffectType.Ray:
                    case BattleEffectType.Chain:
                    case BattleEffectType.Swarm:
                    case BattleEffectType.ScreenSwarm:
                    case BattleEffectType.SFOverlay:
                    case BattleEffectType.DamLightning:
                    case BattleEffectType.DamFire:
                    case BattleEffectType.DamPoison:
                    case BattleEffectType.DamMagic:
                    case BattleEffectType.DamHoly:
                    case BattleEffectType.DamDeath:
                    case BattleEffectType.DamCold:
                    case BattleEffectType.SacredWrath:
                    case BattleEffectType.SWComet:
                    case BattleEffectType.Dispel:
                    case BattleEffectType.WallSmoke:
                    case BattleEffectType.Blood:
                    case BattleEffectType.AltArrow:
                    case BattleEffectType.Tremors:
                    case BattleEffectType.Geyser:
                    case BattleEffectType.GeyserWater:
                    case BattleEffectType.HailImpact:
                    case BattleEffectType.Cloud:
                    case BattleEffectType.CloudPuff:
                    case BattleEffectType.HitGround:
                    case BattleEffectType.HitWater:
                    case BattleEffectType.HighPrayer:
                    case BattleEffectType.PrayerGlow:
                    case BattleEffectType.Resurrect:
                    case BattleEffectType.FrostBeam:
                    case BattleEffectType.Frost:
                    case BattleEffectType.MindDecay:
                    case BattleEffectType.DecayLeg:
                    case BattleEffectType.ShockWave:
                    case BattleEffectType.DecayComet:
                    case BattleEffectType.SacFlame:
                    case BattleEffectType.Winds:
                    case BattleEffectType.SacFire:
                    case BattleEffectType.BreathDam:
                    default:
                        switch (Selection.TacticalBattleUnitHexSprite.SelectedAbility)
                        {
                            case 52:
                                switch (Selection.TacticalBattleUnitHexSprite.SelectedSpell)
                                {
                                    case 104:
                                    case 106:
                                    case 116:
                                    case 117:
                                    case 120:
                                    case 129:
                                    case 130:
                                        {
                                            for (int i = 0; i <= 18; i++)
                                            {
                                                int count2 = Selection.RangedPath.Count;
                                                Point3D rangedPathY = Selection.RangedPath[count2 - 1];
                                                int count3 = Selection.RangedPath.Count;
                                                HexGeometry.PixelToHex(Selection.RangedPath[count3 - 1].X, rangedPathY.Y, out x, out y);
                                                HexGeometry.CenterHexNumberToHex(x, y, i, ref hexX, ref hexY);
                                                HexGeometry.HexToPixel(hexX, hexY, out pixelX, out pixelY);
                                                _targetImages[0].ShowClipped(pixelX - 8 - pixelXSurface, pixelY + 2 - pixelYSurface, lockedSurface);
                                            }
                                        }

                                        break;
                                    case 124:
                                    case 128:
                                        {
                                            for (int i = 0; i <= 6; i++)
                                            {
                                                int count2 = Selection.RangedPath.Count;
                                                Point3D rangedPathY = Selection.RangedPath[count2 - 1];
                                                int count3 = Selection.RangedPath.Count;
                                                HexGeometry.PixelToHex(Selection.RangedPath[count3 - 1].X, rangedPathY.Y, out x, out y);
                                                HexGeometry.CenterHexNumberToHex(x, y, i, ref hexX, ref hexY);
                                                HexGeometry.HexToPixel(hexX, hexY, out pixelX, out pixelY);
                                                _targetImages[0].ShowClipped(pixelX - 8 - pixelXSurface, pixelY + 2 - pixelYSurface, lockedSurface);
                                            }
                                        }

                                        break;
                                    default:
                                        int count4 = Selection.RangedPath.Count;
                                        Point3D rangedPath2 = Selection.RangedPath[count4 - 1];
                                        int count5 = Selection.RangedPath.Count;
                                        Point3D rangedPathX = Selection.RangedPath[count5 - 1];
                                        _miscellaneousImages[_frame + 3].ShowClipped(rangedPathX.X - 21 - pixelXSurface, rangedPath2.Y - 20 - pixelYSurface, lockedSurface);
                                        break;
                                }

                                break;
                            default:
                                int count6 = Selection.RangedPath.Count;
                                Point3D rangedPath3 = Selection.RangedPath[count6 - 1];
                                int count7 = Selection.RangedPath.Count;
                                Point3D rangedPath4 = Selection.RangedPath[count7 - 1];
                                _miscellaneousImages[_frame + 3].ShowClipped(rangedPath4.X - 21 - pixelXSurface, rangedPath3.Y - 20 - pixelYSurface, lockedSurface);
                                break;
                        }

                        break;
                }

                for (int i = 0, last = Selection.RangedPath.Count - 1; i <= last; i++)
                {
                    Point3D rangedPathY = Selection.RangedPath[i];
                    Point3D rangedPathX = Selection.RangedPath[i];
                    _miscellaneousImages[2].Show(rangedPathX.X - pixelXSurface, rangedPathY.Y - pixelYSurface, lockedSurface);
                }

                for (int i = 0, last = Selection.RangedPath.Count - 1; i <= last; i++)
                {
                    Point3D rangedPathY = Selection.RangedPath[i];
                    Point3D rangedPathZ = Selection.RangedPath[i];
                    Point3D rangedPathX = Selection.RangedPath[i];
                    _miscellaneousImages[0].Show(rangedPathX.X - pixelXSurface, rangedPathY.Y - rangedPathZ.Z - 8 - pixelYSurface, lockedSurface);
                }

                for (int i = 0, last = Selection.Obstacles.Count - 1; i <= last; i++)
                {
                    RangedObstacle obstacle = Selection.Obstacles[i];
                    Point3D rangedPath = Selection.RangedPath[obstacle.PathIndex];
                    RangedObstacle obstacle2 = Selection.Obstacles[i];
                    HexGeometry.PixelToHex(Selection.RangedPath[obstacle2.PathIndex].X, rangedPath.Y, out hexX, out hexY);
                    HexGeometry.HexToPixel(hexX, hexY, out pixelX, out pixelY);
                    _miscellaneousImages[1].Show(pixelX + 5 - pixelXSurface, pixelY + 5 - pixelYSurface, lockedSurface);
                    GetEngine().CombatFont.Color = 0;
                    RangedObstacle obstacle3 = Selection.Obstacles[i];
                    GetEngine().CombatFont.SetTextCentered(pixelX + 16 - pixelXSurface, pixelY + 12 - pixelYSurface, IntToStr(obstacle3.Chance), lockedSurface);
                }
            }
        }
    }

    // AoWTC.pas:4852
    public override void Activate()
    {
        base.Activate();
        _miscellaneousImages = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TCombat\\Misc.ILB");
        _targetImages = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TargHex.ILB");
    }

    // AoWTC.pas:4839
    public override void Deactivate()
    {
        GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TCombat\\Misc.ILB");
        GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TargHex.ILB");
        base.Deactivate();
    }
}
