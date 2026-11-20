#nullable disable
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
public partial class ArmyEventDialog(UIContext ui) : UIModule(ui)
{
    public Window ArmyEventWindow { get; private set; }
    public Panel UnitPanel { get; private set; }
    public Label EventTextLabel { get; private set; }
    public Image DamagePicture { get; private set; }

    private event ExecuteDoneEvent OnClose;
    private ArmyEventLog _event;
    private int _frame;
    private ByteList _units;

    // AoWArmyEventDlg.pas:113
    public void DrawUnits(int X, int Y, in LockedSurface lockedSurface)
    {
        int offset;
        ByteList byteList = _units;
        offset = X - (byteList.Count * 16);
        for (int i = 0, last = byteList.Count - 1; i <= last; i++)
        {
            DrawUnit(i, (i * 32) + offset, Y, lockedSurface);
        }
    }

    // AoWArmyEventDlg.pas:121
    public void DrawUnit(int index, int X, int Y, LockedSurface lockedSurface)
    {
        ArmyEventLog eventValue;
        int frame;
        UnitShowInfo info;
        eventValue = _event;
        byte unit = _units[index];
        if (eventValue.DamageValues[unit] != 0)
        {
            // { The original staggers animation by bits of the unit object's address. }
            byte unit2 = _units[index];
            Troop fArmyViewUnit = eventValue.ArmyView.Units.GetUnit(unit2);
            frame = _frame - FrameFixtureInputs.ArmyStagger(fArmyViewUnit);
            info = default;
            info.Frame = (int)(((uint)frame) >> 1) - 1;
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
                    int unitGraphicsResourceIndex = eventValue.ArmyView.Units.GetUnit(unit5).GetUnitGraphicsResourceIndex();
                    UnitGraphicsResource unitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex);
                    _ = unitGraphicsResource.SfxLibraries[3].Play(0);
                }
                else
                {
                    byte unit5 = _units[index];
                    int unitGraphicsResourceIndex = eventValue.ArmyView.Units.GetUnit(unit5).GetUnitGraphicsResourceIndex();
                    UnitGraphicsResource unitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex);
                    _ = unitGraphicsResource.SfxLibraries[4].Play(0);
                }
            }

            if (frame > 5)
            {
                byte unit4 = _units[index];
                byte damageValue = eventValue.DamageValues[unit4];
                byte unit5 = _units[index];
                byte damageType = eventValue.DamageTypes[unit5];
                AbilityRules.ShowDamageIcon(UI.Engine, X, Y + 8, lockedSurface, damageValue, (DamageType)damageType);
                byte unit6 = _units[index];
                sbyte hitPoints = eventValue.ArmyView.Units.GetUnit(unit6).GetHitPoints();
                byte unit7 = _units[index];
                byte damageValue2 = eventValue.DamageValues[unit7];
                byte unit8 = _units[index];
                HitChance.ShowHitPercent(percent: HitChance.HitPointsToHitPercent((sbyte)(byte)(hitPoints - damageValue2), eventValue.ArmyView.Units.GetUnit(unit8).GetHits()), x: X, y: Y, surface: lockedSurface);
            }
            else
            {
                byte unit4 = _units[index];
                sbyte hits = eventValue.ArmyView.Units.GetUnit(unit4).GetHits();
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
        ArmyEventLog eventValue;
        ExecuteDoneEvent notify;
        int resource = default;
        UI.General.Manager.OverlayPriority = 0;
        for (int i = 0, last = _units.Count - 1; i <= last; i++)
        {
            byte unit = _units[i];
            resource = _event.ArmyView.Units.GetUnit(unit).GetUnitGraphicsResourceIndex();
            if (resource != -1)
            {
                _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(resource).Release();
            }
        }

        notify = OnClose;
        eventValue = _event;
        OnClose = null;
        _event = null;
        notify?.Invoke(eventValue);

        _ = (eventValue?.Release());
    }

    // AoWArmyEventDlg.pas:171
    public void ArmyEventDialogCreate() => _units = new ByteList();

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
        if (_frame < 1000)
        {
            _frame += 1;
        }

        DrawUnits(ArmyEventWindow.WinLeft + 203, ArmyEventWindow.WinTop + 100, lockedSurface);
    }

    // AoWArmyEventDlg.pas:193
    public void UnitPnlUpdate()
    {
        UnitPanel.Drawn = false;
        ArmyEventWindow.Drawn = false;
    }

    // AoWArmyEventDlg.pas:198
    public void ArmyEventWinHide() => ClearEvent();

    // AoWArmyEventDlg.pas:203
    public void ArmyEventWinKeyDown(in ushort key, Set<ShiftState> shift)
    {
        if (key is 27 or 32)
        {
            ArmyEventWindow.Visible = false;
        }
    }

    internal static void ShowArmyEvent(ArmyEventDialog dialog, ArmyEventLog eventValue, ExecuteDoneEvent done)
    {
        ArmyEventLog log;
        ArmyView army;
        int image;
        Assert(dialog._event is null, "", "AoWArmyEventDlg.pas", 92);
        dialog.OnClose = done;
        dialog._event = eventValue;
        _ = dialog._event.AddRef();
        log = dialog._event;
        dialog.DamagePicture.CurrentImage = log.ArmyIcon;
        string text = log.ArmyMessages.Text;
        dialog.EventTextLabel.Text = text;
        army = log.ArmyView;
        dialog._units.Clear();
        for (int i = 0, last = army.Units.Count - 1; i <= last; i++)
        {
            if (!army.GetUnitConcealedForPlayer(i, StrategicMap.Current.SeatedPlayer) || log.DamageValues[i] != 0)
            {
                _ = dialog._units.Add((byte)i);
                image = log.ArmyView.Units.GetUnit(i).GetUnitGraphicsResourceIndex();
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
