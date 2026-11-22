#nullable disable warnings
// Translated from AoWArmyEventDlg.pas.
using Aow.Engine.Collections;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.UI.Dialogs;

// completion callback. army event. frame. visible unit indices
public sealed partial class ArmyEventDialog(UIContext ui) : UIModule(ui)
{
    public Window ArmyEventWindow { get; private set; }
    public Panel UnitPanel { get; private set; }
    public Label EventTextLabel { get; private set; }
    public Image DamagePicture { get; private set; }

    private ExecuteDoneEvent? _close;
    private ArmyEventLog? _event;
    private int _frame;
    private ByteList _units;

    // AoWArmyEventDlg.pas:113
    public void DrawUnits(int X, int Y, in LockedSurface lockedSurface)
    {
        ByteList byteList = _units;
        int offset = X - (byteList.Count * 16);
        for (int i = 0, last = byteList.Count - 1; i <= last; i++)
        {
            DrawUnit(i, (i * 32) + offset, Y, lockedSurface);
        }
    }

    // AoWArmyEventDlg.pas:121
    public void DrawUnit(int index, int X, int Y, LockedSurface lockedSurface)
    {
        UnitShowInfo info;
        ArmyEventLog? eventValue = _event;
        byte unit = _units[index];
        if (eventValue.DamageValues[unit] != 0)
        {
            // { The original staggers animation by bits of the unit object's address. }
            byte unit2 = _units[index];
            Troop? fArmyViewUnit = eventValue.ArmyView.Units.GetUnit(unit2);
            int frame = _frame - FrameFixtureInputs.ArmyStagger(fArmyViewUnit);
            info = default;
            info.Frame = (frame >>> 1) - 1;
            info.Direction = 3;
            info.Animation = UnitAnimation.Hit;
            byte unit3 = _units[index];
            _ = eventValue.ArmyView.Units.GetUnit(unit3).Show(X, Y, lockedSurface, info);
            if (frame == 5)
            {
                byte unit4 = _units[index];
                if (eventValue.ArmyView.Units.GetUnit(unit4).GetHitPoints() != 0)
                {
                    byte unit5 = _units[index];
                    int unitGraphicsResourceIndex = eventValue.ArmyView.Units.GetUnit(unit5).UnitGraphicsResourceIndex;
                    UnitGraphicsResource? unitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex);
                    _ = unitGraphicsResource.SfxLibraries[3].Play(0);
                }
                else
                {
                    byte unit5 = _units[index];
                    int unitGraphicsResourceIndex = eventValue.ArmyView.Units.GetUnit(unit5).UnitGraphicsResourceIndex;
                    UnitGraphicsResource? unitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex);
                    _ = unitGraphicsResource.SfxLibraries[4].Play(0);
                }
            }

            if (frame > 5)
            {
                byte unit4 = _units[index];
                int damageValue = eventValue.DamageValues[unit4];
                byte unit5 = _units[index];
                int damageType = eventValue.DamageTypes[unit5];
                AbilityRules.ShowDamageIcon(UI.Engine, X, Y + 8, surface: lockedSurface, damage: damageValue, damageType: (DamageType)damageType);
                byte unit6 = _units[index];
                int hitPoints = eventValue.ArmyView.Units.GetUnit(unit6).GetHitPoints();
                byte unit7 = _units[index];
                int damageValue2 = eventValue.DamageValues[unit7];
                byte unit8 = _units[index];
                HitChance.ShowHitPercent(percent: HitChance.HitPointsToHitPercent((hitPoints - damageValue2).TruncateToSByte(), hits: eventValue.ArmyView.Units.GetUnit(unit8).GetHits()), x: X, y: Y, surface: lockedSurface);
            }
            else
            {
                byte unit4 = _units[index];
                int hits = eventValue.ArmyView.Units.GetUnit(unit4).GetHits();
                byte unit5 = _units[index];
                HitChance.ShowHitPercent(percent: HitChance.HitPointsToHitPercent(eventValue.ArmyView.Units.GetUnit(unit5).GetHitPoints(), hits), x: X, y: Y, surface: lockedSurface);
            }
        }
        else
        {
            byte unit2 = _units[index];
            eventValue.ArmyView.Units.GetUnit(unit2).Show(X, Y, lockedSurface);
        }
    }

    // AoWArmyEventDlg.pas:153
    public void ClearEvent()
    {
        UI.General.Manager.OverlayPriority = 0;
        for (int i = 0, last = _units.Count - 1; i <= last; i++)
        {
            byte unit = _units[i];
            int resource = _event.ArmyView.Units.GetUnit(unit).UnitGraphicsResourceIndex;
            if (resource != -1)
            {
                _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(resource).Release();
            }
        }

        ExecuteDoneEvent? notify = _close;
        ArmyEventLog? eventValue = _event;
        _close = null;
        _event = null;
        notify?.Invoke(eventValue);

        _ = (eventValue?.Release());
    }

    // AoWArmyEventDlg.pas:171
    public void ArmyEventDialogCreate() => _units = [];

    // AoWArmyEventDlg.pas:176
    public void ArmyEventDialogDestroy() => _units?.Dispose();

    // AoWArmyEventDlg.pas:181
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        ArmyEventWindow.Visible = false;
    }

    // AoWArmyEventDlg.pas:187
    public void UnitPnlDrawBack(LockedSurface lockedSurface)
    {
        if (_frame < 1_000)
        {
            _frame += 1;
        }

        DrawUnits(ArmyEventWindow.WinLeft + 203, ArmyEventWindow.WinTop + 100, lockedSurface);
    }

    // AoWArmyEventDlg.pas:193
    public void UnitPnlUpdate()
    {
        UnitPanel.IsDrawn = false;
        ArmyEventWindow.IsDrawn = false;
    }

    // AoWArmyEventDlg.pas:198
    public void ArmyEventWinHide() => ClearEvent();

    // AoWArmyEventDlg.pas:203
    public void ArmyEventWinKeyDown(in int key, ShiftStates shift)
    {
        if (key is 27 or 32)
        {
            ArmyEventWindow.Visible = false;
        }
    }

    internal static void ShowArmyEvent(ArmyEventDialog dialog, ArmyEventLog eventValue, ExecuteDoneEvent done)
    {
        Assert(dialog._event is null, message: "", file: "AoWArmyEventDlg.pas", line: 92);
        dialog._close = done;
        dialog._event = eventValue;
        _ = dialog._event.AddRef();
        ArmyEventLog log = dialog._event;
        dialog.DamagePicture.CurrentImage = log.ArmyIcon;
        string text = log.ArmyMessages.Text;
        dialog.EventTextLabel.Text = text;
        ArmyView army = log.ArmyView;
        dialog._units.Clear();
        for (int i = 0, last = army.Units.Count - 1; i <= last; i++)
        {
            if (!army.GetUnitConcealedForPlayer(i, player: StrategicMap.Current.SeatedPlayer) || log.DamageValues[i] != 0)
            {
                _ = dialog._units.Add(i.TruncateToByte());
                int image = log.ArmyView.Units.GetUnit(i).UnitGraphicsResourceIndex;
                if (image != -1)
                {
                    _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(image).AddRef();
                }
            }
        }

        dialog.ArmyEventWindow.Visible = true;
        dialog._frame = 0;
    }
}
