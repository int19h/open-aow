// Generated from AoWPowerDlg.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWHScroll;
using static Aow.Units.AoWHScroll.AoWHScroll;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;

namespace Aow.Units.AoWPowerDlg
{
    // { TPowerDlg: size $AC; all fields are form components, $9C power allocation slider. VMT 0042B9B0. }
    public partial class TPowerDlg : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +0044 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0048 }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +004C }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +0050 }
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        // { +0054 }
        public Aow.Units.AoWButton.TAOWButton SetBtn;
        // { +0058 }
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +005C }
        public Aow.Units.AoWLabel.TAOWLabel NetMana;
        // { +0060 }
        public Aow.Units.AoWLabel.TAOWLabel NetMamaLbl;
        // { +0064 }
        public Aow.Units.AoWPanel.TAOWPanel UpkeePnl;
        // { +0068 }
        public Aow.Units.AoWLabel.TAOWLabel ResearchLeft;
        // { +006C }
        public Aow.Units.AoWLabel.TAOWLabel SpellName;
        // { +0070 }
        public Aow.Units.AoWPanel.TAOWPanel ResearchPnl;
        // { +0074 }
        public Aow.Units.AoWPanel.TAOWPanel SliderPnl;
        // { +0078 }
        public Aow.Units.AoWLabel.TAOWLabel PowerLbl;
        // { +007C }
        public Aow.Units.AoWLabel.TAOWLabel Power;
        // { +0080 }
        public Aow.Units.AoWLabel.TAOWLabel Mana;
        // { +0084 }
        public Aow.Units.AoWLabel.TAOWLabel Research;
        // { +0088 }
        public Aow.Units.AoWPanel.TAOWPanel ManaPnl;
        // { +008C }
        public Aow.Units.AoWPanel.TAOWPanel ResPnl;
        // { +0090 }
        public Aow.Units.AoWLabel.TAOWLabel ManaLbl;
        // { +0094 }
        public Aow.Units.AoWLabel.TAOWLabel ResearchLbl;
        // { +0098 }
        public Aow.Units.AoWHScroll.TAOWHScrollBar PowerSlider;
        // { +009C }
        public Aow.Units.AoWImage.TAOWImage PAMana;
        // { +00A0 }
        public Aow.Units.AoWImage.TAOWImage PARes;
        // { +00A4 }
        public Aow.Rtl.TIvTranslator Translator;
        // AoWPowerDlg.pas:70
        // { +00A8 }
        public void Execute()
        {
            Aow.Units.AoWE.TPlayerMagicControl Control = default;
            Dlg.Visible = true;
            var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            Control = call__1.FMagicControl;
            PowerSlider.Position = Control.FResearchPercentage;
            var call__2 = Control.GetNetPower();
            Power.Text = Aow.Rtl.Rtl.IntToStr(call__2);
            UpdatePower();
        }

        // AoWPowerDlg.pas:80
        public void UpdatePower()
        {
            int Upkeep = default;
            int Turns = default;
            int SpellID = default;
            int PointsLeft = default;
            int ManaPower = default;
            int ResearchPower = default;
            var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__4 = call__3.FMagicControl.GetNetPower();
            ManaPower = call__4;
            var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            PointsLeft = call__5.FMagicControl.FResearchPointsLeft;
            var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__7 = call__6.FMagicControl.GetManaUpkeep();
            Upkeep = call__7;
            var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            SpellID = call__8.FMagicControl.FResearchSpellID;
            ResearchPower = Aow.Rtl.Rtl.Round(((double)(PowerSlider.Position * ManaPower)) / 100);
            ManaPower -= ResearchPower;
            if (SpellID != 0)
            {
                var call__9 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__10 = call__9.FMagicControl.ResearchTurns(ResearchPower, PointsLeft);
                Turns = call__10;
                var call__11 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(SpellID);
                SpellName.Text = call__11.FSpellName;
                var call__12 = Aow.Units.AoWE.AoWE.FmtTurnsStr(Turns);
                ResearchLeft.Text = call__12;
            }
            else
            {
                var call__13 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoneRStr);
                SpellName.Text = call__13;
                ResearchLeft.Text = AnsiString.FromBytes(45);
            }

            NetMana.Text = (((Aow.Rtl.Rtl.IntToStr(ManaPower) + AnsiString.FromBytes(45)) + Aow.Rtl.Rtl.IntToStr(Upkeep)) + AnsiString.FromBytes(61)) + Aow.Rtl.Rtl.IntToStr(ManaPower - Upkeep);
            Mana.Text = Aow.Rtl.Rtl.IntToStr(ManaPower);
            Research.Text = Aow.Rtl.Rtl.IntToStr(ResearchPower);
            Dlg.FDrawn = false;
        }

        // AoWPowerDlg.pas:61
        public void CancelBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWPowerDlg.pas:67
        public void PowerSliderChange()
        {
            UpdatePower();
        }

        // AoWPowerDlg.pas:108
        public void SetBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__14 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__15 = call__14.FMagicControl.ChangePowerSettings(PowerSlider.Position);
            if (call__15)
            {
                Dlg.Visible = false;
            }
        }

        // AoWPowerDlg.pas:115
        public void PAManaDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int X = default;
            int Y = default;
            int Intensity = default;
            {
                var w__16 = (Aow.Units.AoWImage.TAOWImage)Sender;
                X = w__16.AOWWindow.WinLeft;
                Y = w__16.AOWWindow.WinTop;
                Intensity = Aow.Rtl.Rtl.Round(((double)((100 - PowerSlider.Position) * 50)) / 100);
                var call__17 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt[7];
                call__17.ShowBlended(w__16.WinLeft + X, w__16.WinTop + Y, SD, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Intensity]);
            }
        }

        // AoWPowerDlg.pas:128
        public void PAResDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int X = default;
            int Y = default;
            int Intensity = default;
            {
                var w__18 = (Aow.Units.AoWImage.TAOWImage)Sender;
                X = w__18.AOWWindow.WinLeft;
                Y = w__18.AOWWindow.WinTop;
                Intensity = Aow.Rtl.Rtl.Round(((double)(PowerSlider.Position * 50)) / 100);
                var call__19 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt[8];
                call__19.ShowBlended(w__18.WinLeft + X, w__18.WinTop + Y, SD, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Intensity]);
            }
        }

        // AoWPowerDlg.pas:141
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWPowerDlg
    {
    }
}
