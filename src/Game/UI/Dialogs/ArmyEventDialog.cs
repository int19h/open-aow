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
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// completion callback. army event. frame. visible unit indices
public partial class ArmyEventDialog : DataModule
{
    public ArmyEventDialog(Component owner) : base(owner)
    { }

    public Window ArmyEventWin;
    public Frame BGoldSq;
    public Panel UnitPnl;
    public Label EventText;
    public Image DamagePic;
    public Button CloseBut;
    public Image TitleIcon;
    public Button Close;
    public Label Title;
    public Frame BCeltL;
    public Image DecoL;
    public Image DecoR;
    public IvTranslator Translator;
    public ExecuteDoneEvent OnClose;
    public ArmyEventLog Event;
    public int Frame;
    public ByteList Units;

    // AoWArmyEventDlg.pas:113
    public void DrawUnits(int X, int Y, in LockedSurface lockedSurface)
    {
        int offset;
        var byteList = Units;
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
        eventValue = Event;
        var unit = Units[index];
        if (eventValue.DamageValues[unit] != 0)
        {
            // { The original staggers animation by bits of the unit object's address. }
            var unit2 = Units[index];
            var fArmyViewUnit = eventValue.ArmyView.GetUnit(unit2);
            frame = Frame - FrameFixtureInputs.ArmyStagger(fArmyViewUnit);
            info = default;
            info.Frame = (int)(((uint)frame) >> 1) - 1;
            info.Direction = 3;
            info.Animation = UnitAnimation.Hit;
            var unit3 = Units[index];
            eventValue.ArmyView.GetUnit(unit3).Show(X, Y, lockedSurface, info);
            if (frame == 5)
            {
                var unit4 = Units[index];
                if (eventValue.ArmyView.GetUnit(unit4).GetHitPoints() != 0)
                {
                    var unit5 = Units[index];
                    var unitGraphicsResourceIndex = eventValue.ArmyView.GetUnit(unit5).GetUnitGraphicsResourceIndex();
                    var unitGraphicsResource = AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex);
                    unitGraphicsResource.SfxLibraries[3].Play(0);
                }
                else
                {
                    var unit5 = Units[index];
                    var unitGraphicsResourceIndex = eventValue.ArmyView.GetUnit(unit5).GetUnitGraphicsResourceIndex();
                    var unitGraphicsResource = AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex);
                    unitGraphicsResource.SfxLibraries[4].Play(0);
                }
            }

            if (frame > 5)
            {
                var unit4 = Units[index];
                var damageValue = eventValue.DamageValues[unit4];
                var unit5 = Units[index];
                var damageType = eventValue.DamageTypes[unit5];
                AbilityRules.ShowDamageIcon(X, Y + 8, lockedSurface, damageValue, (DamageType)damageType);
                var unit6 = Units[index];
                var hitPoints = eventValue.ArmyView.GetUnit(unit6).GetHitPoints();
                var unit7 = Units[index];
                var damageValue2 = eventValue.DamageValues[unit7];
                var unit8 = Units[index];
                var hitPointsToHitPercent = HitChance.HitPointsToHitPercent((sbyte)(byte)(hitPoints - damageValue2), eventValue.ArmyView.GetUnit(unit8).GetHits());
                HitChance.ShowHitPercent(X, Y, lockedSurface, hitPointsToHitPercent);
            }
            else
            {
                var unit4 = Units[index];
                var hits = eventValue.ArmyView.GetUnit(unit4).GetHits();
                var unit5 = Units[index];
                var hitPointsToHitPercent = HitChance.HitPointsToHitPercent(eventValue.ArmyView.GetUnit(unit5).GetHitPoints(), hits);
                HitChance.ShowHitPercent(X, Y, lockedSurface, hitPointsToHitPercent);
            }
        }
        else
        {
            var unit2 = Units[index];
            eventValue.ArmyView.GetUnit(unit2).Show(X, Y, lockedSurface);
        }
    }

    // AoWArmyEventDlg.pas:153
    public void ClearEvent()
    {
        ArmyEventLog eventValue;
        ExecuteDoneEvent notify;
        int resource = default;
        DialogHosts.General.Manager.OverlayPriority = 0;
        for (int I = 0, last = Units.Count - 1; I <= last; I++)
        {
            var unit = Units[I];
            resource = Event.ArmyView.GetUnit(unit).GetUnitGraphicsResourceIndex();
            if (resource != -1)
            {
                AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(resource).Release();
            }
        }

        notify = OnClose;
        eventValue = Event;
        OnClose = null;
        Event = null;
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
    public void ArmyEventDlgCreate()
    {
        Units = new ByteList();
    }

    // AoWArmyEventDlg.pas:176
    public void ArmyEventDlgDestroy()
    {
        Units?.Dispose();
    }

    // AoWArmyEventDlg.pas:181
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        ArmyEventWin.Visible = false;
    }

    // AoWArmyEventDlg.pas:187
    public void UnitPnlDrawBack(LockedSurface lockedSurface)
    {
        if (Frame < 1000)
        {
            Frame += 1;
        }

        DrawUnits(ArmyEventWin.WinLeft + 203, ArmyEventWin.WinTop + 100, lockedSurface);
    }

    // AoWArmyEventDlg.pas:193
    public void UnitPnlUpdate()
    {
        UnitPnl.Drawn = false;
        ArmyEventWin.Drawn = false;
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
            ArmyEventWin.Visible = false;
        }
    }

    internal static void ShowArmyEvent(ArmyEventDialog dialog, ArmyEventLog eventValue, ExecuteDoneEvent done)
    {
        ArmyEventLog log;
        ArmyView army;
        int image;
        Assert(dialog.Event == null, "", "AoWArmyEventDlg.pas", 92);
        dialog.OnClose = done;
        dialog.Event = eventValue;
        dialog.Event.AddRef();
        log = dialog.Event;
        dialog.DamagePic.CurrentImage = log.ArmyIcon;
        var text = log.ArmyMessages.Text;
        dialog.EventText.Text = text;
        army = log.ArmyView;
        dialog.Units.Clear();
        for (int I = 0, last = army.GetCount() - 1; I <= last; I++)
        {
            if (!army.GetUnitConcealedForPlayer(I, AoWHSMap.Current.SeatedPlayer) || log.DamageValues[I] != 0)
            {
                dialog.Units.Add((byte)I);
                image = log.ArmyView.GetUnit(I).GetUnitGraphicsResourceIndex();
                if (image != -1)
                {
                    AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(image).AddRef();
                }
            }
        }

        dialog.ArmyEventWin.Visible = true;
        dialog.Frame = 0;
    }

}
