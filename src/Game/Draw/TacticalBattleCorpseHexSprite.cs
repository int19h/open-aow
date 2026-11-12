// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Draw.Images;

using Aow.Game.Units;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// Unit. UnitGraphics. Frame. OffsetX. OffsetY. SequenceIndex. SequenceFrame
public partial class TacticalBattleCorpseHexSprite : SingleHexSprite
{
    public TacticalBattleCorpseHexSprite(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 2228488;

    public TacticalBattleUnit Unit;
    public UnitGraphicsResource UnitGraphics;
    public int Frame;
    public int OffsetX;
    public int OffsetY;
    public int SequenceIndex;
    public int SequenceFrame;

    // AoWTC.pas:4007
    public override byte GetLevel() => 26;

    // AoWTC.pas:4012
    public override int GetShowPriority() => 1744830464 + (OffsetY << 13) + OffsetX;

    // AoWTC.pas:3985
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int frame;
        int X = default;
        int Y = default;
        ImageSequence sequence;
        HexGeometry.HexToPixel(((MapField)Owner).HexX, ((MapField)Owner).HexY, ref X, ref Y);
        pixelXSurface = X - pixelXSurface - 15;
        pixelYSurface = Y - pixelYSurface - 25;
        if (SequenceIndex > -1)
        {
            sequence = AoWHSSet.Current.ImageSequences.ImageSequences[SequenceIndex];
            while (sequence != null)
            {
                frame = sequence.GetUnloopedFrameIndex(SequenceFrame);
                if (frame != -1)
                {
                    sequence.ImageLibrary[frame].Show(pixelXSurface + OffsetX - 5, pixelYSurface + OffsetY + 10, lockedSurface);
                }

                sequence = sequence.Next;
            }
        }

        sequence = UnitGraphics.Sequences.ImageSequences[30];
        while (sequence != null)
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
        CombatAction animateAction;
        CombatAction raiseAction;
        int N;
        int X = default;
        int Y = default;
        int deltaX;
        int deltaY;
        BattleEffectSprite effect;
        float Z = default;
        float speed = default;
        float vz = default;
        float gravity = default;
        TacticalBattleUnitHexSprite unitHexSprite;
        base.MapFieldMessageProc(ref message);
        if (Unit != null)
        {
            if (message.MessageId == 200712 && message.HexNumber == 0)
            {
                var hexY = GetHexY();
                var hexX = GetHexX();
                if (CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].FindOwnedChild(2228484) == null)
                {
                    var hexY2 = GetHexY();
                    var hexX2 = GetHexX();
                    CombatMap.Current.MakeVisible(hexX2, hexY2, 0, 5);
                    var hexY3 = GetHexY();
                    HexGeometry.HexToPixel(GetHexX(), hexY3, ref X, ref Y);
                    for (int ring = 1; ring <= 3; ring++)
                    {
                        N = Random(5);
                        deltaX = Random(20) + ring * 30;
                        deltaY = Random(20) + ring * 30;
                        for (int I = 0; I <= 17; I++)
                        {
                            effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                            effect.AbilityIndex = 52;
                            effect.SpellIndex = message.Param[1].Int;
                            deltaX = -deltaX;
                            switch (I)
                            {
                                case 1:
                                case 3:
                                case 5:
                                case 7:
                                case 9:
                                case 11:
                                case 13:
                                case 15:
                                case 17:
                                    deltaY = -deltaY;
                                    break;
                            }

                            var pi = Math.PI;
                            var sin = Math.Sin((double)((double)(I * 5 + N) * pi) / 180);
                            var mathPI = Math.PI;
                            EffectPaths.MakeLinePath(Round((double)Math.Cos((double)((double)(I * 5 + N) * mathPI) / 180) * deltaX) + (X + 16), Round((double)sin * deltaY) / 2 + (Y + 16), X + 16, Y + 16, 100, ref effect.Path);
                        }
                    }

                    return;
                }
            }

            if (message.MessageId == 200713 && message.HexNumber == 0)
            {
                var hexY = GetHexY();
                var hexX = GetHexX();
                if (CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].FindOwnedChild(2228484) == null)
                {
                    var hexY2 = GetHexY();
                    HexGeometry.HexToPixel(GetHexX(), hexY2, ref X, ref Y);
                    N = Random(6) + 25;
                    for (int I = 1; I <= N - 1; I++)
                    {
                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                        effect.AbilityIndex = 52;
                        effect.SpellIndex = message.Param[1].Int;
                        // { Original uses this smaller value for horizontal speed. }
                        speed = (float)((double)(Random(10) + 10) * 0.05);
                        gravity = (float)0.5;
                        vz = (float)((double)(Random(50) + 50) * 0.05);
                        Z = 0;
                        EffectPaths.MakeArcPath(X + 16, Y + 16, I * 360 / N, ref Z, ref speed, ref vz, ref gravity, ref effect.Path);
                    }

                    switch (message.Param[1].Int)
                    {
                        case 130:
                            RandSeed = AoWHSMap.Current.Random(16777215);
                            animateAction = AoWHSSet.Current.SpellControl.GetSpell(130).CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                            CombatMap.Current.Combat.ExecuteCombatAction(animateAction);
                            animateAction.Release();
                            break;
                        case 129:
                            RandSeed = AoWHSMap.Current.Random(16777215);
                            raiseAction = AoWHSSet.Current.SpellControl.GetSpell(129).CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                            CombatMap.Current.Combat.ExecuteCombatAction(raiseAction);
                            raiseAction.Release();
                            break;
                    }

                    if (Unit.GetEnabled())
                    {
                        unitHexSprite = new TacticalBattleUnitHexSprite(null);
                        unitHexSprite.Initialize(Unit);
                        unitHexSprite.SetDirection((byte)(Random(6) + 1));
                        var hexY3 = GetHexY();
                        var hexX2 = GetHexX();
                        CombatMap.Current.PlaceHex(unitHexSprite, hexX2, hexY3, 0);
                    }

                    this.Dispose();
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

        UnitGraphics.AddRef();
        SequenceFrame = -1;
    }

    // AoWTC.pas:3966
    public override void Deactivate()
    {
        CombatMap.Current.NewFrameEvents -= NewFrame;
        UnitGraphics.Release();
        base.Deactivate();
    }

    // AoWTC.pas:3978
    public void NewFrame(object sender)
    {
        ImageSequence sequence;
        sequence = AoWHSSet.Current.ImageSequences.ImageSequences[SequenceIndex];
        var totalSequenceFrames = sequence.GetTotalSequenceFrames();
        if (SequenceFrame + 1 < totalSequenceFrames)
        {
            SequenceFrame += 1;
        }
    }

    // Engine.pas:524

}
