// Translated from AoWPartyJoin.pas.
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

namespace Aow.Units.AoWPartyJoin;

// done callback, join event, details flag, selection mask, unit list, face unit
public partial class TPartyJoinDlg : TDataModule
{
    public TPartyJoinDlg(TComponent owner) : base(owner)
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
    public TAOWImage TitleIcon;
    public TAOWLabel Title;
    public TAoWFrame BCeltL;
    public TAOWButton RightBtn;
    public TAOWButton LeftBtn;
    public TAOWWindow Dlg;
    public TAoWFrame UnitLine1;
    public TAoWFrame UnitLine2;
    public TAOWMemo JoinMemo;
    public TAOWLabel Header;
    public TAOWImage FaceFrame;
    public TAOWLabel DetailsLbl;
    public TAOWButton Close;
    public TIvTranslator Translator;
    public TExecuteDoneEvent FDone;
    public TJoinEventLog FJoinEvent;
    public bool FDetailsOpen;
    public byte FSelection;
    public TUnitUserList FUnits;
    public TAbstractUnit FFaceUnit;
    // AoWPartyJoin.pas:73
    public bool ValidateJoin()
    {
        bool Result = FJoinEvent.Validate();
        if (!Result)
        {
            Dlg.Visible = false;
        }

        return Result;
    }

    // AoWPartyJoin.pas:79
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

    // AoWPartyJoin.pas:99
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

    // AoWPartyJoin.pas:127
    public void PartyJoinDlgCreate()
    {
        FUnits = new TUnitUserList(null);
    }

    // AoWPartyJoin.pas:132
    public void PartyJoinDlgDestroy()
    {
        FUnits.Free();
    }

    // AoWPartyJoin.pas:137
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

    // AoWPartyJoin.pas:149
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

    // AoWPartyJoin.pas:161
    public void RightBtnClick()
    {
        if (ValidateJoin())
        {
            General.PlayButtonSound(0);
            Dlg.Visible = false;
        }
    }

    // AoWPartyJoin.pas:170
    public void CloseClick()
    {
        if (ValidateJoin())
        {
            General.PlayButtonSound(0);
            Dlg.Visible = false;
        }
    }

    // AoWPartyJoin.pas:179
    public void FaceFramePreDraw(TAoWComponent Sender, TSurfaceDesc Surface)
    {
        TRect R = default;
        if (ValidateJoin())
        {
            R.Left = FaceFrame.WinLeft + Dlg.WinLeft;
            R.Right = FaceFrame.WinWidth + R.Left;
            R.Top = FaceFrame.WinTop + Dlg.WinTop;
            R.Bottom = FaceFrame.WinHeight + R.Top;
            if (FFaceUnit != null)
            {
                if (FFaceUnit.GetFace() != null)
                {
                    FFaceUnit.GetFace().ShowStretched(R, Surface);
                }
            }
        }
    }

    // AoWPartyJoin.pas:194
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

    // AoWPartyJoin.pas:211
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

    // AoWPartyJoin.pas:226
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWPartyJoin.pas:231
    public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }
}

public static partial class AoWPartyJoin
{
}
