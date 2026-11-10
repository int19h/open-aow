// Translated from AoWArmyEventDlg.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Rtl;
// Translated from AoWArmyEventDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// completion callback. army event. frame. visible unit indices
public partial class ArmyEventDlg : DataModule
{
    public ArmyEventDlg(Component owner) : base(owner)
    { }

    public AOWWindow ArmyEventWin;
    public AoWFrame BGoldSq;
    public AOWPanel UnitPnl;
    public AOWLabel EventText;
    public AOWImage DamagePic;
    public AOWButton CloseBut;
    public AOWImage TitleIcon;
    public AOWButton Close;
    public AOWLabel Title;
    public AoWFrame BCeltL;
    public AOWImage DecoL;
    public AOWImage DecoR;
    public IvTranslator Translator;
    public ExecuteDoneEvent FOnClose;
    public ArmyEventLog FEvent;
    public int FFrame;
    public ByteList FUnits;

    // AoWArmyEventDlg.pas:113
    public void DrawUnits(int X, int Y, in SurfaceDesc SD)
    {
        int Offset;
        var byteList = FUnits;
        Offset = X - byteList.Count * 16;
        for (int I = 0, last = byteList.Count - 1; I <= last; I++)
        {
            DrawUnit(I, I * 32 + Offset, Y, SD);
        }
    }

    // AoWArmyEventDlg.pas:121
    public void DrawUnit(int Index, int X, int Y, SurfaceDesc SD)
    {
        ArmyEventLog Event;
        int Frame;
        P2UnitShowInfo Info;
        Event = FEvent;
        var unit = FUnits[Index];
        if (Event.FDamageValues[unit] != 0)
        {
            // { The original staggers animation by bits of the unit object's address. }
            var unit2 = FUnits[Index];
            var fArmyViewUnit = Event.FArmyView.GetUnit(unit2);
            Frame = FFrame - FrameFixtureInputs.ArmyStagger(fArmyViewUnit);
            Info = default;
            Info.Frame = (int)(((uint)Frame) >> 1) - 1;
            Info.Direction = 3;
            Info.Animation = UnitAnimation.Hit;
            var unit3 = FUnits[Index];
            Event.FArmyView.GetUnit(unit3).ShowEx(X, Y, SD, Info);
            if (Frame == 5)
            {
                var unit4 = FUnits[Index];
                if (Event.FArmyView.GetUnit(unit4).GetHitPoints() != 0)
                {
                    var unit5 = FUnits[Index];
                    var unitGFXResourceIndex = Event.FArmyView.GetUnit(unit5).GetUnitGFXResourceIndex();
                    var unitGFXResource = AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(unitGFXResourceIndex);
                    unitGFXResource.FSFXLibraries[3].Play(0);
                }
                else
                {
                    var unit5 = FUnits[Index];
                    var unitGFXResourceIndex = Event.FArmyView.GetUnit(unit5).GetUnitGFXResourceIndex();
                    var unitGFXResource = AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(unitGFXResourceIndex);
                    unitGFXResource.FSFXLibraries[4].Play(0);
                }
            }

            if (Frame > 5)
            {
                var unit4 = FUnits[Index];
                var damageValue = Event.FDamageValues[unit4];
                var unit5 = FUnits[Index];
                var damageType = Event.FDamageTypes[unit5];
                AbilityRules.ShowDamageIcon(X, Y + 8, SD, damageValue, (DamageType)damageType);
                var unit6 = FUnits[Index];
                var hitPoints = Event.FArmyView.GetUnit(unit6).GetHitPoints();
                var unit7 = FUnits[Index];
                var damageValue2 = Event.FDamageValues[unit7];
                var unit8 = FUnits[Index];
                var hitPointsToHitPcnt = HitChance.HitPointsToHitPcnt((sbyte)(byte)(hitPoints - damageValue2), Event.FArmyView.GetUnit(unit8).GetHits());
                HitChance.ShowHitPcnt(X, Y, SD, hitPointsToHitPcnt);
            }
            else
            {
                var unit4 = FUnits[Index];
                var hits = Event.FArmyView.GetUnit(unit4).GetHits();
                var unit5 = FUnits[Index];
                var hitPointsToHitPcnt = HitChance.HitPointsToHitPcnt(Event.FArmyView.GetUnit(unit5).GetHitPoints(), hits);
                HitChance.ShowHitPcnt(X, Y, SD, hitPointsToHitPcnt);
            }
        }
        else
        {
            var unit2 = FUnits[Index];
            Event.FArmyView.GetUnit(unit2).Show(X, Y, SD);
        }
    }

    // AoWArmyEventDlg.pas:153
    public void ClearEvent()
    {
        ArmyEventLog Event;
        ExecuteDoneEvent Notify;
        int Resource = default;
        DialogHosts.General.Manager.OverlayPriority = 0;
        for (int I = 0, last = FUnits.Count - 1; I <= last; I++)
        {
            var unit = FUnits[I];
            Resource = FEvent.FArmyView.GetUnit(unit).GetUnitGFXResourceIndex();
            if (Resource != -1)
            {
                AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(Resource).Release();
            }
        }

        Notify = FOnClose;
        Event = FEvent;
        FOnClose = null;
        FEvent = null;
        if (Notify != null)
        {
            Notify(Event);
        }

        if (Event != null)
        {
            Event.Release();
        }
    }

    // AoWArmyEventDlg.pas:171
    public void ArmyEventDlgCreate()
    {
        FUnits = new ByteList();
    }

    // AoWArmyEventDlg.pas:176
    public void ArmyEventDlgDestroy()
    {
        FUnits?.Dispose();
    }

    // AoWArmyEventDlg.pas:181
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        ArmyEventWin.Visible = false;
    }

    // AoWArmyEventDlg.pas:187
    public void UnitPnlDrawBack(SurfaceDesc SD)
    {
        if (FFrame < 1000)
        {
            FFrame += 1;
        }

        DrawUnits(ArmyEventWin.WinLeft + 203, ArmyEventWin.WinTop + 100, SD);
    }

    // AoWArmyEventDlg.pas:193
    public void UnitPnlUpdate()
    {
        UnitPnl.FDrawn = false;
        ArmyEventWin.FDrawn = false;
    }

    // AoWArmyEventDlg.pas:198
    public void ArmyEventWinHide()
    {
        ClearEvent();
    }

    // AoWArmyEventDlg.pas:203
    public void ArmyEventWinKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            ArmyEventWin.Visible = false;
        }
    }

    internal static void ShowArmyEvent(ArmyEventDlg Dialog, ArmyEventLog Event, ExecuteDoneEvent Done)
    {
        ArmyEventLog Log;
        ArmyView Army;
        int Image;
        Assert(Dialog.FEvent == null, "", "AoWArmyEventDlg.pas", 92);
        Dialog.FOnClose = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Log = Dialog.FEvent;
        Dialog.DamagePic.CurrentImage = Log.FArmyIcon;
        var text = Log.FArmyMessages.Text;
        Dialog.EventText.Text = text;
        Army = Log.FArmyView;
        Dialog.FUnits.Clear();
        for (int I = 0, last = Army.GetCount() - 1; I <= last; I++)
        {
            if (!Army.GetUnitConcealedForPlayer(I, World.Map.AoWHSMap.Current.FSeatedPlayer) || Log.FDamageValues[I] != 0)
            {
                Dialog.FUnits.Add((byte)I);
                Image = Log.FArmyView.GetUnit(I).GetUnitGFXResourceIndex();
                if (Image != -1)
                {
                    AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(Image).AddRef();
                }
            }
        }

        Dialog.ArmyEventWin.Visible = true;
        Dialog.FFrame = 0;
    }

}
