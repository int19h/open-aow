// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// MiscellaneousImages. TargetImages. Frame. FrameCount. Selection. Range
public partial class RangedPathHexSprite : FixedVisibleHexSprite
{
    public RangedPathHexSprite(ECustomNode owner) : base(owner)
    { }

    // AoWTC.pas:4846
    public new const int ClassId = 2228485;

    public CustomImageLibrary MiscellaneousImages;
    public CustomImageLibrary TargetImages;
    public ushort Frame;
    public ushort FrameCount;
    public CombatUnitSelectionControl Selection;
    public ushort Range;

    // AoWTC.pas:4849
    public override int GetShowPriority() => 1811939328;

    // AoWTC.pas:4859
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        int Count;
        int first;
        int Last;
        int hexX = default;
        int hexY = default;
        int pixelX = default;
        int pixelY = default;
        BattleEffectType effectType;
        if (Selection != null)
        {
            if (Range < 100)
            {
                Count = 6;
                first = 1;
                for (int I = 0, last = Range - 2; I <= last; I++)
                {
                    first += Count;
                    Count += 6;
                }

                Last = first + Count;
                for (int I = first; I <= Last - 1; I++)
                {
                    var yhx = Selection.GetTacticalBattleUnitHexSprite().GetHexY();
                    HexGeometry.CenterHexNumberToHex(Selection.GetTacticalBattleUnitHexSprite().GetHexX(), yhx, I, ref hexX, ref hexY);
                    HexGeometry.HexToPixel(hexX, hexY, ref pixelX, ref pixelY);
                    TargetImages[2].Show(pixelX - pixelXSurface - 8, pixelY - pixelYSurface + 2, lockedSurface);
                }
            }

            if (Selection.RangedPath.Count > 0)
            {
                FrameCount = (ushort)(FrameCount + 1);
                if (FrameCount >= 5)
                {
                    FrameCount = 0;
                    Frame = (ushort)(Frame + 1);
                }

                if (Frame > 1)
                {
                    Frame = 0;
                }

                if (Selection.GetTacticalBattleUnitHexSprite().SelectedAbility == 52)
                {
                    var tacticalBattleUnitHexSprite = Selection.GetTacticalBattleUnitHexSprite();
                    effectType = TacticalTables.SpellTypes[tacticalBattleUnitHexSprite.SelectedSpell];
                }
                else
                {
                    var tacticalBattleUnitHexSprite = Selection.GetTacticalBattleUnitHexSprite();
                    effectType = TacticalTables.s_abilityTypes[tacticalBattleUnitHexSprite.SelectedAbility];
                }

                switch (effectType)
                {
                    case BattleEffectType.Breath:
                        var count = Selection.RangedPath.Count;
                        var rangedPath = Selection.RangedPath[count - 1];
                        var fRangedPathCount = Selection.RangedPath.Count;
                        HexGeometry.PixelToHex(Selection.RangedPath[fRangedPathCount - 1].X, rangedPath.Y, ref X, ref Y);
                        {
                            for (int I = 0; I <= 11; I++)
                            {
                                var yhx = Selection.GetTacticalBattleUnitHexSprite().GetHexY();
                                var deltaHexToHexNumber2 = HexGeometry.DeltaHexToHexNumber(Selection.GetTacticalBattleUnitHexSprite().GetHexX(), yhx, X, Y);
                                var hexY2 = Selection.GetTacticalBattleUnitHexSprite().GetHexY();
                                HexGeometry.CenterHexNumberToHex(Selection.GetTacticalBattleUnitHexSprite().GetHexX(), hexY2, TacticalTables.s_breathHexNumber[deltaHexToHexNumber2 * 12 + I - 12], ref hexX, ref hexY);
                                HexGeometry.HexToPixel(hexX, hexY, ref pixelX, ref pixelY);
                                TargetImages[0].ShowClipped(pixelX - 8 - pixelXSurface, pixelY + 2 - pixelYSurface, lockedSurface);
                            }
                        }

                        break;
                    default:
                        switch (Selection.GetTacticalBattleUnitHexSprite().SelectedAbility)
                        {
                            case 52:
                                switch (Selection.GetTacticalBattleUnitHexSprite().SelectedSpell)
                                {
                                    case 104:
                                    case 106:
                                    case 116:
                                    case 117:
                                    case 120:
                                    case 129:
                                    case 130:
                                        {
                                            for (int I = 0; I <= 18; I++)
                                            {
                                                var count2 = Selection.RangedPath.Count;
                                                var rangedPathY = Selection.RangedPath[count2 - 1];
                                                var count3 = Selection.RangedPath.Count;
                                                HexGeometry.PixelToHex(Selection.RangedPath[count3 - 1].X, rangedPathY.Y, ref X, ref Y);
                                                HexGeometry.CenterHexNumberToHex(X, Y, I, ref hexX, ref hexY);
                                                HexGeometry.HexToPixel(hexX, hexY, ref pixelX, ref pixelY);
                                                TargetImages[0].ShowClipped(pixelX - 8 - pixelXSurface, pixelY + 2 - pixelYSurface, lockedSurface);
                                            }
                                        }

                                        break;
                                    case 124:
                                    case 128:
                                        {
                                            for (int I = 0; I <= 6; I++)
                                            {
                                                var count2 = Selection.RangedPath.Count;
                                                var rangedPathY = Selection.RangedPath[count2 - 1];
                                                var count3 = Selection.RangedPath.Count;
                                                HexGeometry.PixelToHex(Selection.RangedPath[count3 - 1].X, rangedPathY.Y, ref X, ref Y);
                                                HexGeometry.CenterHexNumberToHex(X, Y, I, ref hexX, ref hexY);
                                                HexGeometry.HexToPixel(hexX, hexY, ref pixelX, ref pixelY);
                                                TargetImages[0].ShowClipped(pixelX - 8 - pixelXSurface, pixelY + 2 - pixelYSurface, lockedSurface);
                                            }
                                        }

                                        break;
                                    default:
                                        var count4 = Selection.RangedPath.Count;
                                        var rangedPath2 = Selection.RangedPath[count4 - 1];
                                        var count5 = Selection.RangedPath.Count;
                                        var rangedPathX = Selection.RangedPath[count5 - 1];
                                        MiscellaneousImages[Frame + 3].ShowClipped(rangedPathX.X - 21 - pixelXSurface, rangedPath2.Y - 20 - pixelYSurface, lockedSurface);
                                        break;
                                }

                                break;
                            default:
                                var count6 = Selection.RangedPath.Count;
                                var rangedPath3 = Selection.RangedPath[count6 - 1];
                                var count7 = Selection.RangedPath.Count;
                                var rangedPath4 = Selection.RangedPath[count7 - 1];
                                MiscellaneousImages[Frame + 3].ShowClipped(rangedPath4.X - 21 - pixelXSurface, rangedPath3.Y - 20 - pixelYSurface, lockedSurface);
                                break;
                        }

                        break;
                }

                for (int I = 0, last = Selection.RangedPath.Count - 1; I <= last; I++)
                {
                    var rangedPathY = Selection.RangedPath[I];
                    var rangedPathX = Selection.RangedPath[I];
                    MiscellaneousImages[2].Show(rangedPathX.X - pixelXSurface, rangedPathY.Y - pixelYSurface, lockedSurface);
                }

                for (int I = 0, last = Selection.RangedPath.Count - 1; I <= last; I++)
                {
                    var rangedPathY = Selection.RangedPath[I];
                    var rangedPathZ = Selection.RangedPath[I];
                    var rangedPathX = Selection.RangedPath[I];
                    MiscellaneousImages[0].Show(rangedPathX.X - pixelXSurface, rangedPathY.Y - rangedPathZ.Z - 8 - pixelYSurface, lockedSurface);
                }

                for (int I = 0, last = Selection.Obstacles.Count - 1; I <= last; I++)
                {
                    var obstacle = Selection.Obstacles[I];
                    var rangedPath = Selection.RangedPath[((RangedObstacle)obstacle).PathIndex];
                    var obstacle2 = Selection.Obstacles[I];
                    HexGeometry.PixelToHex(Selection.RangedPath[((RangedObstacle)obstacle2).PathIndex].X, rangedPath.Y, ref hexX, ref hexY);
                    HexGeometry.HexToPixel(hexX, hexY, ref pixelX, ref pixelY);
                    MiscellaneousImages[1].Show(pixelX + 5 - pixelXSurface, pixelY + 5 - pixelYSurface, lockedSurface);
                    GameTables.GameEngine.CombatFont.Color = 0;
                    var obstacle3 = Selection.Obstacles[I];
                    GameTables.GameEngine.CombatFont.SetTextCentered(pixelX + 16 - pixelXSurface, pixelY + 12 - pixelYSurface, IntToStr(((RangedObstacle)obstacle3).Chance), lockedSurface);
                }
            }
        }
    }

    // AoWTC.pas:4852
    public override void Activate()
    {
        base.Activate();
        MiscellaneousImages = AoWHSSet.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TCombat\\Misc.ILB");
        TargetImages = AoWHSSet.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TargHex.ILB");
    }

    // AoWTC.pas:4839
    public override void Deactivate()
    {
        AoWHSSet.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TCombat\\Misc.ILB");
        AoWHSSet.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TargHex.ILB");
        base.Deactivate();
    }

    // Engine.pas:524

}
