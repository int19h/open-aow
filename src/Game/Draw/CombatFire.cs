#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from CombatFire.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Draw;

public sealed class CombatFire(CustomNode owner) : SingleHexSprite(owner)
{

    // CombatFire.pas:80
    public new const int ClassId = 2_229_578;

    private int _xShift;
    private int _yShift;
    private int _player;
    private bool _firstTurn;

    // CombatFire.pas:119
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537006081)
        {
            if (!CombatMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
            {
                NewTurn();
            }
        }
    }

    // CombatFire.pas:83
    public override int Level => 27;

    // CombatFire.pas:70
    public override bool Visible
    {
        set => base.Visible = value;
    }

    // CombatFire.pas:86
    public override int GetShowPriority() => 1_811_939_328;

    // CombatFire.pas:146
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (info.HexNumber == 0)
        {
            int penalty = ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(7)
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
                int yhx = HexY;
                int xhx = HexX;
                CombatMap.Current.MapContainer.GetMapLevel(0).GetField(xhx, yhx).SendMapFieldMessage(ref message);
                int hexX = default;
                int hexY = default;
                for (int i = 1; i <= 6; i++)
                {
                    int hexY2 = HexY;
                    HexGeometry.CenterHexNumberToHex(HexX, hexY2, hexNumber: i, ref hexX, ref hexY);
                    if (hexX > 0 && hexY > 0)
                    {
                        EffectPaths.CreateFire(50, hexX, hexY, engine: GetEngine());
                    }
                }

                Dispose();
            }
        }
    }

    // CombatFire.pas:26
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        ImageSequence sequence = GamePack.Current.ImageSequences.GetImageSequence(60);
        if (sequence is not null)
        {
            do
            {
                int imageIndex = sequence.GetFrameIndex(StrategicMap.Current.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
                if (imageIndex != -1)
                {
                    sequence.ImageLibrary.Get(imageIndex).Show(pixelXSurface + sequence.OffsetX + _xShift, pixelYSurface + sequence.OffsetY + _yShift, lockedSurface);
                }

                sequence = sequence.Next;
            }
            while (sequence is not null);
        }
    }
}
