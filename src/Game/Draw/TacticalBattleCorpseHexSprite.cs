#nullable disable
// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Draw.Images;

using Aow.Game.Units;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Draw;

// Unit. UnitGraphics. Frame. OffsetX. OffsetY. SequenceIndex. SequenceFrame
public class TacticalBattleCorpseHexSprite(CustomNode owner) : SingleHexSprite(owner)
{
    public new const int ClassId = 2228488;

    public TacticalBattleUnit Unit { get; set; }
    public UnitGraphicsResource UnitGraphics { get; init; }
    public int Frame { get; set; }
    public int OffsetX { get; set; }
    public int OffsetY { get; set; }
    public int SequenceIndex { get; set; }

    private int _sequenceFrame;

    // AoWTC.pas:4007
    public override byte Level => 26;

    // AoWTC.pas:4012
    public override int GetShowPriority() => 1744830464 + (OffsetY << 13) + OffsetX;

    // AoWTC.pas:3985
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int x, out int y);
        pixelXSurface = x - pixelXSurface - 15;
        pixelYSurface = y - pixelYSurface - 25;
        int frame;
        ImageSequence sequence;
        if (SequenceIndex > -1)
        {
            sequence = GamePack.Current.ImageSequences.ImageSequences[SequenceIndex];
            while (sequence is not null)
            {
                frame = sequence.GetUnloopedFrameIndex(_sequenceFrame);
                if (frame != -1)
                {
                    sequence.ImageLibrary[frame].Show(pixelXSurface + OffsetX - 5, pixelYSurface + OffsetY + 10, lockedSurface);
                }

                sequence = sequence.Next;
            }
        }

        sequence = UnitGraphics.Sequences.ImageSequences[30];
        while (sequence is not null)
        {
            frame = sequence.GetUnloopedFrameIndex(Frame);
            if (frame != -1)
            {
                sequence.ImageLibrary[frame].Show(pixelXSurface + OffsetX, pixelYSurface + OffsetY, lockedSurface);
            }

            sequence = sequence.Next;
        }
    }

    // AoWTC.pas:4025
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (Unit is not null)
        {
            int x;
            int y;
            int n;
            BattleEffectSprite effect;
            if (message.MessageId == 200712 && message.HexNumber == 0)
            {
                sbyte hexY = HexY;
                sbyte hexX = HexX;
                if (CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].FindOwnedChild(2228484) is null)
                {
                    sbyte hexY2 = HexY;
                    sbyte hexX2 = HexX;
                    CombatMap.Current.MakeVisible(hexX2, hexY2, 0, 5);
                    sbyte hexY3 = HexY;
                    HexGeometry.HexToPixel(HexX, hexY3, out x, out y);
                    for (int ring = 1; ring <= 3; ring++)
                    {
                        n = Random(5);
                        int deltaX = Random(20) + (ring * 30);
                        int deltaY = Random(20) + (ring * 30);
                        for (int i = 0; i <= 17; i++)
                        {
                            effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                            effect.AbilityIndex = 52;
                            effect.SpellIndex = message.Param[1].Int;
                            deltaX = -deltaX;
                            if (i is 1 or 3 or 5 or 7 or 9 or 11 or 13 or 15 or 17)
                            {
                                deltaY = -deltaY;
                            }

                            double pi = Math.PI;
                            double sin = Math.Sin((double)(((i * 5) + n) * pi) / 180);
                            double mathPI = Math.PI;
                            Point3DList path = effect.Path;
                            EffectPaths.MakeLinePath(Round((double)Math.Cos((double)(((i * 5) + n) * mathPI) / 180) * deltaX) + x + 16, (Round((double)sin * deltaY) / 2) + y + 16, x + 16, y + 16, 100, path);
                            effect.Path = path;
                        }
                    }

                    return;
                }
            }

            if (message.MessageId == 200713 && message.HexNumber == 0)
            {
                sbyte hexY = HexY;
                sbyte hexX = HexX;
                if (CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].FindOwnedChild(2228484) is null)
                {
                    sbyte hexY2 = HexY;
                    HexGeometry.HexToPixel(HexX, hexY2, out x, out y);
                    n = Random(6) + 25;
                    for (int i = 1; i <= n - 1; i++)
                    {
                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                        effect.AbilityIndex = 52;
                        effect.SpellIndex = message.Param[1].Int;
                        // { Original uses this smaller value for horizontal speed. }
                        float speed = (float)((Random(10) + 10) * 0.05);
                        float gravity = (float)0.5;
                        float vz = (float)((Random(50) + 50) * 0.05);
                        float z = 0;
                        Point3DList path2 = effect.Path;
                        EffectPaths.MakeArcPath(x + 16, y + 16, i * 360 / n, ref z, speed, ref vz, gravity, path2);
                        effect.Path = path2;
                    }

                    switch (message.Param[1].Int)
                    {
                        case 130:
                            RandSeed = StrategicMap.Current.Random(16777215);
                            CombatAction animateAction = GamePack.Current.SpellControl[130].CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                            CombatMap.Current.Combat.ExecuteCombatAction(animateAction);
                            _ = animateAction.Release();
                            break;
                        case 129:
                            RandSeed = StrategicMap.Current.Random(16777215);
                            CombatAction raiseAction = GamePack.Current.SpellControl[129].CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                            CombatMap.Current.Combat.ExecuteCombatAction(raiseAction);
                            _ = raiseAction.Release();
                            break;
                        default:
                            break;
                    }

                    if (Unit.GetEnabled())
                    {
                        TacticalBattleUnitHexSprite unitHexSprite = new(null) { Holder = CreationHolder() };
                        unitHexSprite.Initialize(Unit);
                        unitHexSprite.SetDirection((byte)(Random(6) + 1));
                        sbyte hexY3 = HexY;
                        sbyte hexX2 = HexX;
                        _ = CombatMap.Current.PlaceHex(unitHexSprite, hexX2, hexY3, 0);
                    }

                    Dispose();
                }
            }
        }
    }

    // AoWTC.pas:4017
    public override void Activate()
    {
        base.Activate();
        if (SequenceIndex > -1)
        {
            CombatMap.Current.NewFrameEvents += NewFrame;
        }

        _ = UnitGraphics.AddRef();
        _sequenceFrame = -1;
    }

    // AoWTC.pas:3966
    public override void Deactivate()
    {
        CombatMap.Current.NewFrameEvents -= NewFrame;
        _ = UnitGraphics.Release();
        base.Deactivate();
    }

    // AoWTC.pas:3978
    public void NewFrame(object sender)
    {
        ImageSequence sequence = GamePack.Current.ImageSequences.ImageSequences[SequenceIndex];
        int totalSequenceFrames = sequence.TotalSequenceFrames;
        if (_sequenceFrame + 1 < totalSequenceFrames)
        {
            _sequenceFrame += 1;
        }
    }
}
