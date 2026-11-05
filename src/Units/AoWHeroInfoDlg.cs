// Translated from AoWHeroInfoDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
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
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;

namespace Aow.Units.AoWHeroInfoDlg
{
    // done callback, army event, unit-details flag
    public partial class THeroInfoDlg : TDataModule
    {
        public THeroInfoDlg(TComponent owner) : base(owner)
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
        public TAoWFrame BCeltL;
        public TAOWWindow Dlg;
        public TAOWLabel HeroName;
        public TAOWLabel HeroLevel;
        public TAOWImage FaceFrame;
        public TAOWLabel DetailsLbl;
        public TAOWMemo Message;
        public TIvTranslator Translator;
        public TExecuteDoneEvent FDone;
        public TArmyInfoEventLog FArmyInfo;
        public bool FDetailsOpen;
        // AoWHeroInfoDlg.pas:99
        public void FinishDialog()
        {
            TExecuteDoneEvent Done = default;
            TEventLog Event = default;
            if (FDetailsOpen)
            {
                FDetailsOpen = false;
                AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Priority = AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Priority;
                AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = false;
            }

            Done = FDone;
            Event = FArmyInfo;
            FDone = null;
            FArmyInfo = null;
            if (Done != null)
            {
                Done(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }

        // AoWHeroInfoDlg.pas:118
        public void DrawUnit(int Index, in TRect R, ref TSurfaceDesc Surface)
        {
            TAlignment Alignment = default;
            int X = default;
            int Y = default;
            TRect OldClip = default;
            TP2UnitShowInfo Info = default;
            var call__1 = FArmyInfo.FArmyView.GetCount();
            if (Index < call__1)
            {
                OldClip = Surface.ClipRect;
                Surface.ClipRect = R;
                X = (R.Right + R.Left) / 2;
                Y = (R.Top + R.Bottom) / 2;
                var call__2 = FArmyInfo.FArmyView.GetUnit(Index);
                var call__3 = call__2.GetAlignment();
                Alignment = call__3;
                if ((int)Alignment < IntGfxMod.UnitBGs.Count)
                {
                    var image = IntGfxMod.UnitBGs[(int)Alignment];
                    image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, Surface);
                }

                Info = default;
                Info.Direction = 3;
                Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoHitPoints, TUnitOverlayModesElement.uoRank);
                var call__5 = FArmyInfo.FArmyView.GetUnit(Index);
                call__5.ShowEx(X - 33, Y - 35, Surface, Info);
                if ((((int)(((uint)FArmyInfo.FSelection) >> Index)) & 1) == 0)
                {
                    var image2 = IntGfxMod.UnitBGs[7];
                    image2.ShowClipped(X - image2.Width / 2, Y - image2.Height / 2, Surface);
                }

                Surface.ClipRect = OldClip;
            }
        }

        // AoWHeroInfoDlg.pas:146
        public void CloseBtnClick()
        {
            General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWHeroInfoDlg.pas:152
        public void FaceFramePreDraw(TObject Sender, TSurfaceDesc Surface)
        {
            TRect R = default;
            THero Hero = default;
            int I = default;
            R.Left = this.FaceFrame.WinLeft + Dlg.WinLeft;
            R.Right = this.FaceFrame.WinWidth + R.Left;
            R.Top = this.FaceFrame.WinTop + Dlg.WinTop;
            R.Bottom = this.FaceFrame.WinHeight + R.Top;

            var call__8 = FArmyInfo.FArmyView.Hero(FArmyInfo.FSelection, AllHeroTypes);
            Hero = call__8;
            if (Hero.GetFace() != null)
            {
                Hero.ShowFace(R, Surface);
            }
            else
            {
                I = 0;
                while (true)
                {
                    bool bool__9 = ((1 << I) & FArmyInfo.FSelection) == 0;
                    if (bool__9)
                    {
                        var call__10 = FArmyInfo.FArmyView.GetCount();
                        bool__9 = I != call__10;
                    }

                    if (!bool__9)
                        break;
                    {
                        I += 1;
                    }
                }

                var call__11 = FArmyInfo.FArmyView.GetCount();
                if (I < call__11)
                {
                    var call__12 = FArmyInfo.FArmyView.GetUnit(I);
                    call__12.ShowFace(R, Surface);
                }
            }
        }

        // AoWHeroInfoDlg.pas:175
        public void U1PreDraw(TAoWComponent Sender, TSurfaceDesc Surface)
        {
            TRect R = default;
            R.Left = Sender.WinLeft + UnitPnl.WinLeft + 1;
            R.Right = Sender.WinWidth + R.Left;
            R.Top = Sender.WinTop + UnitPnl.WinTop;
            R.Bottom = Sender.WinHeight + R.Top;

            DrawUnit(Sender.Tag, R, ref Surface);
        }

        // AoWHeroInfoDlg.pas:189
        public void U1MouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            ;
            var call__14 = FArmyInfo.FArmyView.GetCount();
            if (((TAoWComponent)Sender).Tag < call__14)
            {
                var call__15 = FArmyInfo.FArmyView.GetUnit(((TAoWComponent)Sender).Tag);
                General.FSelectedUnit = call__15;
                if (Button == TMouseButton.mbLeft)
                {
                    FDetailsOpen = true;
                    AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                    AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
                }
            }
        }

        // AoWHeroInfoDlg.pas:203
        public void DlgHide()
        {
            FinishDialog();
        }

        // AoWHeroInfoDlg.pas:208
        public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            if (Key == 27 || Key == 32)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWHeroInfoDlg
    {
        // AoWHeroInfoDlg.pas:61
        public static void ShowHeroInfo(THeroInfoDlg Dialog, TArmyInfoEventLog Event, TExecuteDoneEvent Done)
        {
            THero Hero = default;
            Assert(Dialog.FArmyInfo == null, (AnsiString)"", "AoWHeroInfoDlg.pas", 80);
            Dialog.FDone = Done;
            Dialog.FArmyInfo = Event;
            Dialog.FArmyInfo.AddRef();
            var call__17 = Event.FMessages.Text;
            Dialog.Message.Items.Text = call__17;
            var call__18 = Dialog.FArmyInfo.FArmyView.Hero(Dialog.FArmyInfo.FSelection, AllHeroTypes);
            Hero = call__18;
            if (Hero != null)
            {
                Dialog.HeroName.Text = Hero.FHeroName;
                var call__19 = Hero.GetLevel();
                var call__20 = TranslateRStr(LevelXRStr);
                var call__21 = AoWE.AoWE.Format(call__20, [IntToStr(call__19)]);
                Dialog.HeroLevel.Text = call__21;
            }
            else
            {
                Dialog.HeroName.Text = (AnsiString)"";
                Dialog.HeroLevel.Text = (AnsiString)"";
            }

            Dialog.Dlg.Visible = true;
            Dialog.FDetailsOpen = false;
        }
    }
}
