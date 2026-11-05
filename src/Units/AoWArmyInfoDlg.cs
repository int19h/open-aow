// Translated from AoWArmyInfoDlg.pas.
using Aow.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using Aow.Units.AoWComp;
using Aow.Units.EventLog;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWArmyInfoDlg
{
    // unit-info window open. completion callback. army-info event
    public partial class TArmyInfoDlg : TDataModule
    {
        public TArmyInfoDlg(TComponent owner) : base(owner)
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
        public TAOWButton CloseBtn;
        public TAOWImage TitleIcon;
        public TAOWButton Close;
        public TAOWLabel Title;
        public TAoWFrame BCeltL;
        public TAOWWindow Dlg;
        public TAoWFrame UnitLine1;
        public TAoWFrame UnitLine2;
        public TAOWMemo Message;
        public TAOWImage FaceFrame;
        public TAOWLabel DetailsLbl;
        public TAOWLabel HeaderLbl;
        public TIvTranslator Translator;
        public bool FUnitInfoShown;
        public TExecuteDoneEvent FOnClose;
        public TArmyInfoEventLog FEvent;
        // AoWArmyInfoDlg.pas:63
        public void DrawUnit(int Index, in TRect R, ref TSurfaceDesc SD)
        {
            TRect SaveRect = default;
            TP2UnitShowInfo Info = default;
            ImageLib.TLibraryImage Img = default;
            TAlignment Alignment = default;
            int X = default;
            int Y = default;
            var call__1 = FEvent.FArmyView.GetCount();
            if (Index < call__1)
            {
                SaveRect = SD.ClipRect;
                SD.ClipRect = R;
                X = (R.Right + R.Left) / 2;
                Y = (R.Top + R.Bottom) / 2;
                var call__2 = FEvent.FArmyView.GetUnit(Index);
                var call__3 = call__2.GetAlignment();
                Alignment = call__3;
                if ((int)Alignment < aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs.Count)
                {
                    // { D3 masks the enum index after the original full-byte guard. }
                    var call__4 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[(int)Alignment];
                    Img = call__4;
                    Img.ShowClipped(X - Img.Width / 2, Y - Img.Height / 2, SD);
                }

                Info = default;
                Info.Direction = 3;
                Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoHitPoints, TUnitOverlayModesElement.uoRank);
                var call__5 = FEvent.FArmyView.GetUnit(Index);
                call__5.ShowEx(X - 33, Y - 35, SD, Info);
                if ((((int)(((uint)FEvent.FSelection) >> Index)) & 1) == 0)
                {
                    var call__6 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[7];
                    Img = call__6;
                    Img.ShowClipped(X - Img.Width / 2, Y - Img.Height / 2, SD);
                }

                SD.ClipRect = SaveRect;
            }
        }

        // AoWArmyInfoDlg.pas:92
        public void ClearEvent()
        {
            TArmyInfoEventLog Event = default;
            TExecuteDoneEvent Notify = default;
            if (FUnitInfoShown)
            {
                FUnitInfoShown = false;
                AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Priority = AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Priority;
                AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = false;
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

        // AoWArmyInfoDlg.pas:109
        public void CloseBtnClick()
        {
            General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWArmyInfoDlg.pas:115
        public void CloseClick()
        {
            General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWArmyInfoDlg.pas:121
        public void U1MouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            ;
            var call__7 = FEvent.FArmyView.GetCount();
            if (((TComponent)Sender).Tag < call__7)
            {
                var call__8 = FEvent.FArmyView.GetUnit(((TComponent)Sender).Tag);
                General.FSelectedUnit = call__8;
                if (Button == TMouseButton.mbLeft)
                {
                    FUnitInfoShown = true;
                    AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                    AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
                }
            }
        }

        // AoWArmyInfoDlg.pas:134
        public void U1PreDraw(TAoWComponent Sender, TSurfaceDesc SD)
        {
            TRect R = default;
            var image = (TAOWImage)Sender;
            R.Left = image.WinLeft + UnitPnl.WinLeft + 1;
            R.Right = image.WinWidth + R.Left;
            R.Top = image.WinTop + UnitPnl.WinTop;
            R.Bottom = image.WinHeight + R.Top;
            DrawUnit(Sender.Tag, R, ref SD);
        }

        // AoWArmyInfoDlg.pas:146
        public void FaceFramePreDraw(TAoWComponent Sender, TSurfaceDesc SD)
        {
            TRect R = default;
            int I = default;
            ;
            R.Left = this.FaceFrame.WinLeft + Dlg.WinLeft;
            R.Right = this.FaceFrame.WinWidth + R.Left;
            R.Top = this.FaceFrame.WinTop + Dlg.WinTop;
            R.Bottom = this.FaceFrame.WinHeight + R.Top;

            I = 0;
            while (true)
            {
                bool bool__11 = (FEvent.FSelection & (1 << I)) == 0;
                if (bool__11)
                {
                    var call__12 = FEvent.FArmyView.GetCount();
                    bool__11 = I != call__12;
                }

                if (!bool__11)
                    break;
                {
                    I += 1;
                }
            }

            var call__13 = FEvent.FArmyView.GetCount();
            if (I < call__13)
            {
                var call__14 = FEvent.FArmyView.GetUnit(I);
                call__14.ShowFace(R, SD);
            }
        }

        // AoWArmyInfoDlg.pas:161
        public void DlgHide()
        {
            ClearEvent();
        }

        // AoWArmyInfoDlg.pas:166
        public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            if (Key == 27 || Key == 32)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWArmyInfoDlg
    {
    }
}
