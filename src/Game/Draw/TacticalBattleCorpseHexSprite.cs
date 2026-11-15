#nullable disable
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
    public TacticalBattleCorpseHexSprite(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 2228488;

    public TacticalBattleUnit Unit { get; set; }
    public UnitGraphicsResource UnitGraphics { get; set; }
    public int Frame { get; set; }
    public int OffsetX { get; set; }
    public int OffsetY { get; set; }
    public int SequenceIndex { get; set; }

    private int _sequenceFrame;

    // AoWTC.pas:4007
    public override byte GetLevel() => 26;

    // AoWTC.pas:4012
    public override int GetShowPriority() => 1744830464 + (OffsetY << 13) + OffsetX;

    // AoWTC.pas:3985
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int frame;
        int x = default;
        int y = default;
        ImageSequence sequence;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out x, out y);
        pixelXSurface = x - pixelXSurface - 15;
        pixelYSurface = y - pixelYSurface - 25;
        if (SequenceIndex > -1)
        {
            sequence = GamePack.Current.ImageSequences.ImageSequences[SequenceIndex];
            while (sequence != null)
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
        int n;
        int x = default;
        int y = default;
        int deltaX;
        int deltaY;
        BattleEffectSprite effect;
        float z = default;
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
                    HexGeometry.HexToPixel(GetHexX(), hexY3, out x, out y);
                    for (int ring = 1; ring <= 3; ring++)
                    {
                        n = Random(5);
                        deltaX = Random(20) + (ring * 30);
                        deltaY = Random(20) + (ring * 30);
                        for (int i = 0; i <= 17; i++)
                        {
                            effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                            effect.AbilityIndex = 52;
                            effect.SpellIndex = message.Param[1].Int;
                            deltaX = -deltaX;
                            switch (i)
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
                            var sin = Math.Sin((double)(((i * 5) + n) * pi) / 180);
                            var mathPI = Math.PI;
                            var path = effect.Path;
                            EffectPaths.MakeLinePath(Round((double)Math.Cos((double)(((i * 5) + n) * mathPI) / 180) * deltaX) + x + 16, (Round((double)sin * deltaY) / 2) + y + 16, x + 16, y + 16, 100, ref path);
                            effect.Path = path;
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
                    HexGeometry.HexToPixel(GetHexX(), hexY2, out x, out y);
                    n = Random(6) + 25;
                    for (int i = 1; i <= n - 1; i++)
                    {
                        effect = CombatMap.Current.AddEffect(BattleEffectType.GeyserWater);
                        effect.AbilityIndex = 52;
                        effect.SpellIndex = message.Param[1].Int;
                        // { Original uses this smaller value for horizontal speed. }
                        speed = (float)((Random(10) + 10) * 0.05);
                        gravity = (float)0.5;
                        vz = (float)((Random(50) + 50) * 0.05);
                        z = 0;
                        var path2 = effect.Path;
                        EffectPaths.MakeArcPath(x + 16, y + 16, i * 360 / n, ref z, ref speed, ref vz, ref gravity, ref path2);
                        effect.Path = path2;
                    }

                    switch (message.Param[1].Int)
                    {
                        case 130:
                            RandSeed = StrategicMap.Current.Random(16777215);
                            animateAction = GamePack.Current.SpellControl.GetSpell(130).CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                            CombatMap.Current.Combat.ExecuteCombatAction(animateAction);
                            animateAction.Release();
                            break;
                        case 129:
                            RandSeed = StrategicMap.Current.Random(16777215);
                            raiseAction = GamePack.Current.SpellControl.GetSpell(129).CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                            CombatMap.Current.Combat.ExecuteCombatAction(raiseAction);
                            raiseAction.Release();
                            break;
                    }

                    if (Unit.GetEnabled())
                    {
                        unitHexSprite = new TacticalBattleUnitHexSprite(null) { Holder = CreationHolder() };
                        unitHexSprite.Initialize(Unit);
                        unitHexSprite.SetDirection((byte)(Random(6) + 1));
                        var hexY3 = GetHexY();
                        var hexX2 = GetHexX();
                        CombatMap.Current.PlaceHex(unitHexSprite, hexX2, hexY3, 0);
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

        UnitGraphics.AddRef();
        _sequenceFrame = -1;
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
        sequence = GamePack.Current.ImageSequences.ImageSequences[SequenceIndex];
        var totalSequenceFrames = sequence.GetTotalSequenceFrames();
        if (_sequenceFrame + 1 < totalSequenceFrames)
        {
            _sequenceFrame += 1;
        }
    }

    // Engine.pas:524

}
