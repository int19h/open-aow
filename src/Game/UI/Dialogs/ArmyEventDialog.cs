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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// completion callback. army event. frame. visible unit indices
public partial class ArmyEventDialog : UIModule
{
    public ArmyEventDialog(UIContext ui) : base(ui)
    { }

    public Window ArmyEventWindow;
    public Panel UnitPanel;
    public Label EventTextLabel;
    public Image DamagePicture;
    private event ExecuteDoneEvent OnClose;
    private ArmyEventLog _event;
    private int _frame;
    private ByteList _units;

    // AoWArmyEventDlg.pas:113
    public void DrawUnits(int X, int Y, in LockedSurface lockedSurface)
    {
        int offset;
        var byteList = _units;
        offset = X - byteList.Count * 16;
        for (int I = 0, last = byteList.Count - 1; I <= last; I++)
        {
            DrawUnit(I, I * 32 + offset, Y, lockedSurface);
        }
    }

    // AoWArmyEventDlg.pas:121
    public void DrawUnit(int index, int X, int Y, LockedSurface lockedSurface)
    {
        ArmyEventLog eventValue;
        int frame;
        UnitShowInfo info;
        eventValue = _event;
        var unit = _units[index];
        if (eventValue.DamageValues[unit] != 0)
        {
            // { The original staggers animation by bits of the unit object's address. }
            var unit2 = _units[index];
            var fArmyViewUnit = eventValue.ArmyView.Units.GetUnit(unit2);
            frame = _frame - FrameFixtureInputs.ArmyStagger(fArmyViewUnit);
            info = default;
            info.Frame = (int)(((uint)frame) >> 1) - 1;
            info.Direction = 3;
            info.Animation = UnitAnimation.Hit;
            var unit3 = _units[index];
            eventValue.ArmyView.Units.GetUnit(unit3).Show(X, Y, lockedSurface, info);
            if (frame == 5)
            {
                var unit4 = _units[index];
                if (eventValue.ArmyView.Units.GetUnit(unit4).GetHitPoints() != 0)
                {
                    var unit5 = _units[index];
                    var unitGraphicsResourceIndex = eventValue.ArmyView.Units.GetUnit(unit5).GetUnitGraphicsResourceIndex();
                    var unitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex);
                    unitGraphicsResource.SfxLibraries[3].Play(0);
                }
                else
                {
                    var unit5 = _units[index];
                    var unitGraphicsResourceIndex = eventValue.ArmyView.Units.GetUnit(unit5).GetUnitGraphicsResourceIndex();
                    var unitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex);
                    unitGraphicsResource.SfxLibraries[4].Play(0);
                }
            }

            if (frame > 5)
            {
                var unit4 = _units[index];
                var damageValue = eventValue.DamageValues[unit4];
                var unit5 = _units[index];
                var damageType = eventValue.DamageTypes[unit5];
                AbilityRules.ShowDamageIcon(UI.Engine, X, Y + 8, lockedSurface, damageValue, (DamageType)damageType);
                var unit6 = _units[index];
                var hitPoints = eventValue.ArmyView.Units.GetUnit(unit6).GetHitPoints();
                var unit7 = _units[index];
                var damageValue2 = eventValue.DamageValues[unit7];
                var unit8 = _units[index];
                var hitPointsToHitPercent = HitChance.HitPointsToHitPercent((sbyte)(byte)(hitPoints - damageValue2), eventValue.ArmyView.Units.GetUnit(unit8).GetHits());
                HitChance.ShowHitPercent(X, Y, lockedSurface, hitPointsToHitPercent);
            }
            else
            {
                var unit4 = _units[index];
                var hits = eventValue.ArmyView.Units.GetUnit(unit4).GetHits();
                var unit5 = _units[index];
                var hitPointsToHitPercent = HitChance.HitPointsToHitPercent(eventValue.ArmyView.Units.GetUnit(unit5).GetHitPoints(), hits);
                HitChance.ShowHitPercent(X, Y, lockedSurface, hitPointsToHitPercent);
            }
        }
        else
        {
            var unit2 = _units[index];
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
        for (int I = 0, last = _units.Count - 1; I <= last; I++)
        {
            var unit = _units[I];
            resource = _event.ArmyView.Units.GetUnit(unit).GetUnitGraphicsResourceIndex();
            if (resource != -1)
            {
                GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(resource).Release();
            }
        }

        notify = OnClose;
        eventValue = _event;
        OnClose = null;
        _event = null;
        if (notify != null)
        {
            notify(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
        }
    }

    // AoWArmyEventDlg.pas:171
    public void ArmyEventDialogCreate()
    {
        _units = new ByteList();
    }

    // AoWArmyEventDlg.pas:176
    public void ArmyEventDialogDestroy()
    {
        _units?.Dispose();
    }

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
    public void ArmyEventWinHide()
    {
        ClearEvent();
    }

    // AoWArmyEventDlg.pas:203
    public void ArmyEventWinKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27 || key == 32)
        {
            ArmyEventWindow.Visible = false;
        }
    }

    internal static void ShowArmyEvent(ArmyEventDialog dialog, ArmyEventLog eventValue, ExecuteDoneEvent done)
    {
        ArmyEventLog log;
        ArmyView army;
        int image;
        Assert(dialog._event == null, "", "AoWArmyEventDlg.pas", 92);
        dialog.OnClose = done;
        dialog._event = eventValue;
        dialog._event.AddRef();
        log = dialog._event;
        dialog.DamagePicture.CurrentImage = log.ArmyIcon;
        var text = log.ArmyMessages.Text;
        dialog.EventTextLabel.Text = text;
        army = log.ArmyView;
        dialog._units.Clear();
        for (int I = 0, last = army.Units.Count - 1; I <= last; I++)
        {
            if (!army.GetUnitConcealedForPlayer(I, StrategicMap.Current.SeatedPlayer) || log.DamageValues[I] != 0)
            {
                dialog._units.Add((byte)I);
                image = log.ArmyView.Units.GetUnit(I).GetUnitGraphicsResourceIndex();
                if (image != -1)
                {
                    GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(image).AddRef();
                }
            }
        }

        dialog.ArmyEventWindow.Visible = true;
        dialog._frame = 0;
    }

}
