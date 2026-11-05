// Translated from AoWHeroJoinDlg.pas.
using Aow.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using Aow.Units.AoWComp;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;

namespace Aow.Units.AoWHeroJoinDlg;

// done callback, join event, details flag, selection mask, unit list, face unit
public partial class THeroJoinDlg : TDataModule
{
    public THeroJoinDlg(TComponent owner) : base(owner)
    { }

    public TAOWPanel UnitPnl;
    public TAoWFrame U1;
    public TAoWFrame U2;
    public TAoWFrame U4;
    public TAoWFrame U5;
    public TAoWFrame U3;
    public TAoWFrame U6;
    public TAoWFrame U7;
    public TAoWFrame U8;
    public TAOWButton MiddleBtn;
    public TAoWFrame BCeltL;
    public TAOWButton RightBtn;
    public TAOWButton LeftBtn;
    public TAOWWindow Dlg;
    public TAOWLabel HeroName;
    public TAOWLabel HeroLevel;
    public TAOWImage FaceFrame;
    public TAOWLabel DetailsLbl;
    public TAOWMemo JoinMemo;
    public TIvTranslator Translator;
    public TExecuteDoneEvent FDone;
    public TJoinEventLog FJoinEvent;
    public bool FDetailsOpen;
    public byte FSelection;
    public TUnitUserList FUnits;
    public TAbstractUnit FFaceUnit;
    // AoWHeroJoinDlg.pas:68
    public bool ValidateJoin()
    {
        bool Result = FJoinEvent.Validate();
        if (!Result)
        {
            Dlg.Visible = false;
        }

        return Result;
    }

    // AoWHeroJoinDlg.pas:74
    public void FinishDialog()
    {
        TEventLog Event;
        TExecuteDoneEvent Done;
        FUnits.Clear();
        if (FDetailsOpen)
        {
            FDetailsOpen = false;
            AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Priority = AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Priority;
            AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = false;
        }

        Done = FDone;
        Event = FJoinEvent;
        FDone = null;
        FJoinEvent = null;
        if (Done != null)
        {
            Done(Event);
        }

        if (Event != null)
        {
            Event.Release();
        }
    }

    // AoWHeroJoinDlg.pas:94
    public void DrawUnit(int Index, in TRect R, ref TSurfaceDesc Surface)
    {
        TAlignment Alignment;
        int X;
        int Y;
        TRect OldClip;
        TP2UnitShowInfo Info;
        if (Index < FUnits.GetCount())
        {
            OldClip = Surface.ClipRect;
            Surface.ClipRect = R;
            X = (R.Right + R.Left) / 2;
            Y = (R.Top + R.Bottom) / 2;
            Alignment = FUnits.GetUnit(Index).GetAlignment();
            if ((int)Alignment < IntGfxMod.UnitBGs.Count)
            {
                var image = IntGfxMod.UnitBGs[(int)Alignment];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, Surface);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoHitPoints, TUnitOverlayModesElement.uoRank);
            FUnits.GetUnit(Index).ShowEx(X - 33, Y - 35, Surface, Info);
            if ((((int)(((uint)FSelection) >> Index)) & 1) == 0)
            {
                var image = IntGfxMod.UnitBGs[7];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, Surface);
            }

            Surface.ClipRect = OldClip;
        }
    }

    // AoWHeroJoinDlg.pas:122
    public void HeroJoinDlgCreate()
    {
        FUnits = new TUnitUserList(null);
    }

    // AoWHeroJoinDlg.pas:127
    public void HeroJoinDlgDestroy()
    {
        FUnits.Free();
    }

    // AoWHeroJoinDlg.pas:132
    public void LeftBtnClick()
    {
        string Reason = "";
        if (ValidateJoin())
        {
            General.PlayButtonSound(0);
            if (!FJoinEvent.Accept(ref Reason))
            {
                AoWHSMap.ShowWarning(Reason);
            }
            else
            {
                Dlg.Visible = false;
            }
        }
    }

    // AoWHeroJoinDlg.pas:144
    public void MiddleBtnClick()
    {
        string Reason = "";
        if (ValidateJoin())
        {
            General.PlayButtonSound(0);
            if (!FJoinEvent.Reject(ref Reason))
            {
                AoWHSMap.ShowWarning(Reason);
            }
            else
            {
                Dlg.Visible = false;
            }
        }
    }

    // AoWHeroJoinDlg.pas:156
    public void RightBtnClick()
    {
        if (ValidateJoin())
        {
            General.PlayButtonSound(0);
            Dlg.Visible = false;
        }
    }

    // AoWHeroJoinDlg.pas:165
    public void FaceFramePreDraw(TAoWComponent Sender, TSurfaceDesc Surface)
    {
        TRect R = default;
        if (ValidateJoin())
        {
            R.Left = FaceFrame.WinLeft + Dlg.WinLeft;
            R.Right = FaceFrame.WinWidth + R.Left;
            R.Top = FaceFrame.WinTop + Dlg.WinTop;
            R.Bottom = FaceFrame.WinHeight + R.Top;
            if (FFaceUnit.GetFace() != null)
            {
                FFaceUnit.GetFace().ShowStretched(R, Surface);
            }
        }
    }

    // AoWHeroJoinDlg.pas:179
    public void U1PreDraw(TAoWComponent Sender, TSurfaceDesc Surface)
    {
        TRect R = default;
        if (ValidateJoin())
        {
            R.Left = Sender.WinLeft + UnitPnl.WinLeft + 1;
            R.Right = Sender.WinWidth + R.Left;
            R.Top = Sender.WinTop + UnitPnl.WinTop;
            R.Bottom = Sender.WinHeight + R.Top;

            DrawUnit(Sender.Tag, R, ref Surface);
        }
    }

    // AoWHeroJoinDlg.pas:196
    public void U1MouseDown(TAoWComponent Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (ValidateJoin())
        {
            var count = FUnits.GetCount();
            if (Sender.Tag < count)
            {
                var unit = FUnits.GetUnit(Sender.Tag);
                General.FSelectedUnit = unit;
                if (Button == TMouseButton.mbLeft)
                {
                    FDetailsOpen = true;
                    AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                    AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
                }
            }
        }
    }

    // AoWHeroJoinDlg.pas:211
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWHeroJoinDlg.pas:216
    public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }
}

public static partial class AoWHeroJoinDlg
{
}
