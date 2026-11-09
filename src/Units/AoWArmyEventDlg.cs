// Translated from AoWArmyEventDlg.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;

namespace Aow.Units.AoWArmyEventDlg;

// completion callback. army event. frame. visible unit indices
public partial class TArmyEventDlg : TDataModule
{
    public TArmyEventDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow ArmyEventWin;
    public TAoWFrame BGoldSq;
    public TAOWPanel UnitPnl;
    public TAOWLabel EventText;
    public TAOWImage DamagePic;
    public TAOWButton CloseBut;
    public TAOWImage TitleIcon;
    public TAOWButton Close;
    public TAOWLabel Title;
    public TAoWFrame BCeltL;
    public TAOWImage DecoL;
    public TAOWImage DecoR;
    public TIvTranslator Translator;
    public TExecuteDoneEvent FOnClose;
    public TArmyEventLog FEvent;
    public int FFrame;
    public TByteList FUnits;

    // AoWArmyEventDlg.pas:113
    public void DrawUnits(int X, int Y, in TSurfaceDesc SD)
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
    public void DrawUnit(int Index, int X, int Y, TSurfaceDesc SD)
    {
        TArmyEventLog Event;
        int Frame;
        TP2UnitShowInfo Info;
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
            Info.Animation = TUnitAnimation.uaHit;
            var unit3 = FUnits[Index];
            Event.FArmyView.GetUnit(unit3).ShowEx(X, Y, SD, Info);
            if (Frame == 5)
            {
                var unit4 = FUnits[Index];
                if (Event.FArmyView.GetUnit(unit4).GetHitPoints() != 0)
                {
                    var unit5 = FUnits[Index];
                    var unitGFXResourceIndex = Event.FArmyView.GetUnit(unit5).GetUnitGFXResourceIndex();
                    var unitGFXResource = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(unitGFXResourceIndex);
                    unitGFXResource.FSFXLibraries[3].Play(0);
                }
                else
                {
                    var unit5 = FUnits[Index];
                    var unitGFXResourceIndex = Event.FArmyView.GetUnit(unit5).GetUnitGFXResourceIndex();
                    var unitGFXResource = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(unitGFXResourceIndex);
                    unitGFXResource.FSFXLibraries[4].Play(0);
                }
            }

            if (Frame > 5)
            {
                var unit4 = FUnits[Index];
                var damageValue = Event.FDamageValues[unit4];
                var unit5 = FUnits[Index];
                var damageType = Event.FDamageTypes[unit5];
                ShowDamageIcon(X, Y + 8, SD, damageValue, (TDamageType)damageType);
                var unit6 = FUnits[Index];
                var hitPoints = Event.FArmyView.GetUnit(unit6).GetHitPoints();
                var unit7 = FUnits[Index];
                var damageValue2 = Event.FDamageValues[unit7];
                var unit8 = FUnits[Index];
                var hitPointsToHitPcnt = HitPointsToHitPcnt((sbyte)(byte)(hitPoints - damageValue2), Event.FArmyView.GetUnit(unit8).GetHits());
                ShowHitPcnt(X, Y, SD, hitPointsToHitPcnt);
            }
            else
            {
                var unit4 = FUnits[Index];
                var hits = Event.FArmyView.GetUnit(unit4).GetHits();
                var unit5 = FUnits[Index];
                var hitPointsToHitPcnt = HitPointsToHitPcnt(Event.FArmyView.GetUnit(unit5).GetHitPoints(), hits);
                ShowHitPcnt(X, Y, SD, hitPointsToHitPcnt);
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
        TArmyEventLog Event;
        TExecuteDoneEvent Notify;
        int Resource = default;
        General.Manager.OverlayPriority = 0;
        for (int I = 0, last = FUnits.Count - 1; I <= last; I++)
        {
            var unit = FUnits[I];
            Resource = FEvent.FArmyView.GetUnit(unit).GetUnitGFXResourceIndex();
            if (Resource != -1)
            {
                AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Resource).Release();
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
        FUnits = new TByteList();
    }

    // AoWArmyEventDlg.pas:176
    public void ArmyEventDlgDestroy()
    {
        FUnits?.Dispose();
    }

    // AoWArmyEventDlg.pas:181
    public void CloseClick()
    {
        General.PlayButtonSound(0);
        ArmyEventWin.Visible = false;
    }

    // AoWArmyEventDlg.pas:187
    public void UnitPnlDrawBack(TSurfaceDesc SD)
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
    public void ArmyEventWinKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            ArmyEventWin.Visible = false;
        }
    }
}

public static partial class AoWArmyEventDlgUnit
{
    // AoWArmyEventDlg.pas:84
    public static void ShowArmyEvent(TArmyEventDlg Dialog, TArmyEventLog Event, TExecuteDoneEvent Done)
    {
        TArmyEventLog Log;
        TArmyView Army;
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
            if (!Army.GetUnitConcealedForPlayer(I, AoWHSMap.FSeatedPlayer) || Log.FDamageValues[I] != 0)
            {
                Dialog.FUnits.Add((byte)I);
                Image = Log.FArmyView.GetUnit(I).GetUnitGFXResourceIndex();
                if (Image != -1)
                {
                    AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Image).AddRef();
                }
            }
        }

        Dialog.ArmyEventWin.Visible = true;
        Dialog.FFrame = 0;
    }
}
