// Translated from AoWHeroInfoDlg.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.aowIntGfxMod;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.E2CampaignSupport;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;

namespace Aow.Units.AoWHeroInfoDlg
{
    // { THeroInfoDlg: done callback $8C, army event $94, unit-details flag $98.
//     (instance size 156, VMT 00447CEC) }
    public partial class THeroInfoDlg : Aow.Rtl.TDataModule
    {
        public THeroInfoDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWPanel.TAOWPanel UnitPnl;
        // { +0044 }
        public Aow.Units.AoWFrame.TAoWFrame U1;
        // { +0048 }
        public Aow.Units.AoWFrame.TAoWFrame U2;
        // { +004C }
        public Aow.Units.AoWFrame.TAoWFrame U4;
        // { +0050 }
        public Aow.Units.AoWFrame.TAoWFrame U5;
        // { +0054 }
        public Aow.Units.AoWFrame.TAoWFrame U3;
        // { +0058 }
        public Aow.Units.AoWFrame.TAoWFrame U6;
        // { +005C }
        public Aow.Units.AoWFrame.TAoWFrame U7;
        // { +0060 }
        public Aow.Units.AoWFrame.TAoWFrame U8;
        // { +0064 }
        public Aow.Units.AoWButton.TAOWButton CloseBtn;
        // { +0068 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +006C }
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0070 }
        public Aow.Units.AoWLabel.TAOWLabel HeroName;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel HeroLevel;
        // { +0078 }
        public Aow.Units.AoWImage.TAOWImage FaceFrame;
        // { +007C }
        public Aow.Units.AoWLabel.TAOWLabel DetailsLbl;
        // { +0080 }
        public Aow.Units.AoWMemo.TAOWMemo Message;
        // { +0084 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0088 }
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        // { $8C }
        public Aow.Units.AoWE.TArmyInfoEventLog FArmyInfo;
        // { $94 }
        public bool FDetailsOpen;
        // { $98 }
        public Aow.Units.FixedArray_byte_3 F_HeroInfoDlg_99;
        // AoWHeroInfoDlg.pas:99
        public void FinishDialog()
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.EventLog.TEventLog Event = default;
            if (FDetailsOpen)
            {
                FDetailsOpen = false;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Priority = Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Priority;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = false;
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
        public void DrawUnit(int Index, in Aow.Rtl.TRect R, ref Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.AoWE.TAlignment Alignment = default;
            int X = default;
            int Y = default;
            Aow.Rtl.TRect OldClip = default;
            Aow.Units.AoWE.TP2UnitShowInfo Info = default;
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
                if (((int)Alignment) < Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs.Count)
                {
                    var image = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[unchecked((int)Alignment)];
                    image.ShowClipped(X - (image.Width / 2), Y - (image.Height / 2), Surface);
                }

                Info = default;
                Info.Direction = 3;
                Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoHitPoints, Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
                var call__5 = FArmyInfo.FArmyView.GetUnit(Index);
                call__5.ShowEx(X - 33, Y - 35, Surface, Info);
                if ((((int)(((uint)FArmyInfo.FSelection) >> Index)) & 1) == 0)
                {
                    var image2 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[7];
                    image2.ShowClipped(X - (image2.Width / 2), Y - (image2.Height / 2), Surface);
                }

                Surface.ClipRect = OldClip;
            }
        }

        // AoWHeroInfoDlg.pas:146
        public void CloseBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWHeroInfoDlg.pas:152
        // { draft }
        public void FaceFramePreDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect R = default;
            Aow.Units.AoWE.THero Hero = default;
            int I = default;
            R.Left = this.FaceFrame.WinLeft + Dlg.WinLeft;
            R.Right = this.FaceFrame.WinWidth + R.Left;
            R.Top = this.FaceFrame.WinTop + Dlg.WinTop;
            R.Bottom = this.FaceFrame.WinHeight + R.Top;

            var call__8 = FArmyInfo.FArmyView.Hero(unchecked((byte)FArmyInfo.FSelection), Aow.Units.E2CampaignSupport.E2CampaignSupport.AllHeroTypes);
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
                    bool bool__9 = (((1 << I) & FArmyInfo.FSelection) == 0);
                    if (bool__9)
                    {
                        var call__10 = FArmyInfo.FArmyView.GetCount();
                        bool__9 = (I != call__10);
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
        public void U1PreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect R = default;
            R.Left = (Sender.WinLeft + UnitPnl.WinLeft) + 1;
            R.Right = Sender.WinWidth + R.Left;
            R.Top = Sender.WinTop + UnitPnl.WinTop;
            R.Bottom = Sender.WinHeight + R.Top;

            DrawUnit(Sender.Tag, R, ref Surface);
        }

        // AoWHeroInfoDlg.pas:189
        public void U1MouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            ;
            var call__14 = FArmyInfo.FArmyView.GetCount();
            if (((Aow.Units.AoWComp.TAoWComponent)Sender).Tag < call__14)
            {
                var call__15 = FArmyInfo.FArmyView.GetUnit(((Aow.Units.AoWComp.TAoWComponent)Sender).Tag);
                Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = call__15;
                if (Button == Aow.Rtl.TMouseButton.mbLeft)
                {
                    FDetailsOpen = true;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
                }
            }
        }

        // AoWHeroInfoDlg.pas:203
        public void DlgHide()
        {
            FinishDialog();
        }

        // AoWHeroInfoDlg.pas:208
        // { draft }
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if ((Key == 27) || (Key == 32))
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWHeroInfoDlg
    {
        // AoWHeroInfoDlg.pas:61
        public static void ShowHeroInfo(Aow.Units.AoWHeroInfoDlg.THeroInfoDlg Dialog, Aow.Units.AoWE.TArmyInfoEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            Aow.Units.AoWE.THero Hero = default;
            Aow.Rtl.Rtl.Assert(Dialog.FArmyInfo == null, (AnsiString)"", "D:\\git\\aow\\src\\delphi3\\AoW\\AoWHeroInfoDlg.pas", 80);
            Dialog.FDone = Done;
            Dialog.FArmyInfo = Event;
            Dialog.FArmyInfo.AddRef();
            var call__17 = Event.FMessages.Text;
            Dialog.Message.Items.Text = call__17;
            var call__18 = Dialog.FArmyInfo.FArmyView.Hero(unchecked((byte)Dialog.FArmyInfo.FSelection), Aow.Units.E2CampaignSupport.E2CampaignSupport.AllHeroTypes);
            Hero = call__18;
            if (Hero != null)
            {
                Dialog.HeroName.Text = Hero.FHeroName;
                var call__19 = Hero.GetLevel();
                var call__20 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LevelXRStr);
                var call__21 = Aow.Units.AoWE.AoWE.Format(call__20, [Aow.Rtl.Rtl.IntToStr(call__19)]);
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
