// Translated from CombatFire.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Terrain;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public partial class CombatFire : SingleHexSprite
{
    public CombatFire(ECustomNode owner) : base(owner)
    {
    }

    // CombatFire.pas:80
    public new const int ClassId = 2229578;

    public int XShift;
    public int YShift;
    public int Player;
    public bool FirstTurn;

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
    public override void SetVisible(bool value)
    {
        base.SetVisible(value);
    }

    // CombatFire.pas:86
    public override int GetShowPriority() => 1811939328;

    // CombatFire.pas:146
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        int penalty;
        if (info.HexNumber == 0)
        {
            if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(7))
            {
                penalty = 0;
            }
            else
            {
                if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(71))
                {
                    penalty = 10;
                }
                else
                {
                    penalty = 20;
                }
            }

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
        XShift = Random(10) + 11;
        YShift = Random(10) + 11;
        Player = CombatMap.Current.CurrentCombatPlayer;
        FirstTurn = true;
    }

    // CombatFire.pas:55
    public override void Deactivate()
    {
        base.Deactivate();
    }

    // CombatFire.pas:75
    public override void MakeVisible(VisibilityList visibilityList)
    {
        base.MakeVisible(visibilityList);
    }

    // CombatFire.pas:98
    public void NewTurn()
    {
        int hexX = default;
        int hexY = default;
        MapFieldMessage message = default;
        if (FirstTurn)
        {
            FirstTurn = false;
        }
        else
        {
            if (CombatMap.Current.CurrentCombatPlayer == Player)
            {
                message.MessageId = 200711;
                message.Param[0] = this;
                message.Result = -1;
                var yhx = GetHexY();
                var xhx = GetHexX();
                CombatMap.Current.MapContainer.GetMapLevel(0).GetField(xhx, yhx).SendMapFieldMessage(ref message);
                for (int I = 1; I <= 6; I++)
                {
                    var hexY2 = GetHexY();
                    HexGeometry.CenterHexNumberToHex(GetHexX(), hexY2, I, ref hexX, ref hexY);
                    if (hexX > 0 && hexY > 0)
                    {
                        EffectPaths.CreateFire(true, 50, hexX, hexY);
                    }
                }

                this.Dispose();
            }
        }
    }

    // CombatFire.pas:26
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int imageIndex;
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(((MapField)Owner).HexX, ((MapField)Owner).HexY, ref pixelX, ref pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        sequence = AoWHSSet.Current.ImageSequences.GetImageSequence(60);
        if (sequence != null)
        {
            do
            {
                imageIndex = sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (imageIndex != -1)
                {
                    sequence.ImageLibrary.Get(imageIndex).Show(pixelXSurface + sequence.OffsetX + XShift, pixelYSurface + sequence.OffsetY + YShift, lockedSurface);
                }

                sequence = sequence.Next;
            }
            while (!(sequence == null));
        }
    }
}
