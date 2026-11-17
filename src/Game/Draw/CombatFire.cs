// Translated from CombatFire.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public partial class CombatFire(CustomNode owner) : SingleHexSprite(owner)
{

    // CombatFire.pas:80
    public new const int ClassId = 2229578;

    private int _xShift;
    private int _yShift;
    private int _player;
    private bool _firstTurn;

    // CombatFire.pas:60

    // CombatFire.pas:119
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537006081)
        {
            if (!CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
            {
                NewTurn();
            }
        }
    }

    // CombatFire.pas:83
    public override byte GetLevel() => 27;

    // CombatFire.pas:70
    public override void SetVisible(bool value) => base.SetVisible(value);

    // CombatFire.pas:86
    public override int GetShowPriority() => 1811939328;

    // CombatFire.pas:146
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        int penalty;
        if (info.HexNumber == 0)
        {
            penalty = ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(7)
                ? 0
                : ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(71) ? 10 : 20;

            if (moveResult.Code > 127 - penalty)
            {
                moveResult.Code = 127;
            }
            else
            {
                moveResult.Code += penalty;
            }
        }
    }

    // CombatFire.pas:89
    public override void Activate()
    {
        base.Activate();
        _xShift = Random(10) + 11;
        _yShift = Random(10) + 11;
        _player = CombatMap.Current.CurrentCombatPlayer;
        _firstTurn = true;
    }

    // CombatFire.pas:55
    public override void Deactivate() => base.Deactivate();

    // CombatFire.pas:75
    public override void MakeVisible(VisibilityList visibilityList) => base.MakeVisible(visibilityList);

    // CombatFire.pas:98
    public void NewTurn()
    {
        int hexX = default;
        int hexY = default;
        MapFieldMessage message = default;
        if (_firstTurn)
        {
            _firstTurn = false;
        }
        else
        {
            if (CombatMap.Current.CurrentCombatPlayer == _player)
            {
                message.MessageId = 200711;
                MapFieldMessageParameters param = message.Param;
                param[0] = this;
                message.Param = param;
                message.Result = -1;
                sbyte yhx = GetHexY();
                sbyte xhx = GetHexX();
                CombatMap.Current.MapContainer.GetMapLevel(0).GetField(xhx, yhx).SendMapFieldMessage(ref message);
                for (int i = 1; i <= 6; i++)
                {
                    sbyte hexY2 = GetHexY();
                    HexGeometry.CenterHexNumberToHex(GetHexX(), hexY2, i, ref hexX, ref hexY);
                    if (hexX > 0 && hexY > 0)
                    {
                        EffectPaths.CreateFire(true, 50, hexX, hexY, engine: GetEngine());
                    }
                }

                Dispose();
            }
        }
    }

    // CombatFire.pas:26
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int imageIndex;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        sequence = GamePack.Current.ImageSequences.GetImageSequence(60);
        if (sequence != null)
        {
            do
            {
                imageIndex = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (imageIndex != -1)
                {
                    sequence.ImageLibrary.Get(imageIndex).Show(pixelXSurface + sequence.OffsetX + _xShift, pixelYSurface + sequence.OffsetY + _yShift, lockedSurface);
                }

                sequence = sequence.Next;
            }
            while (!(sequence == null));
        }
    }
}
