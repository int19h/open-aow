// Translated from AoWPowerDlg.pas.
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
    // all fields are form components, power allocation slider
    public partial class TPowerDlg : Aow.Rtl.TDataModule
    {
        public TPowerDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        public Aow.Units.AoWButton.TAOWButton Close;
        public Aow.Units.AoWLabel.TAOWLabel Title;
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        public Aow.Units.AoWButton.TAOWButton SetBtn;
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        public Aow.Units.AoWLabel.TAOWLabel NetMana;
        public Aow.Units.AoWLabel.TAOWLabel NetMamaLbl;
        public Aow.Units.AoWPanel.TAOWPanel UpkeePnl;
        public Aow.Units.AoWLabel.TAOWLabel ResearchLeft;
        public Aow.Units.AoWLabel.TAOWLabel SpellName;
        public Aow.Units.AoWPanel.TAOWPanel ResearchPnl;
        public Aow.Units.AoWPanel.TAOWPanel SliderPnl;
        public Aow.Units.AoWLabel.TAOWLabel PowerLbl;
        public Aow.Units.AoWLabel.TAOWLabel Power;
        public Aow.Units.AoWLabel.TAOWLabel Mana;
        public Aow.Units.AoWLabel.TAOWLabel Research;
        public Aow.Units.AoWPanel.TAOWPanel ManaPnl;
        public Aow.Units.AoWPanel.TAOWPanel ResPnl;
        public Aow.Units.AoWLabel.TAOWLabel ManaLbl;
        public Aow.Units.AoWLabel.TAOWLabel ResearchLbl;
        public Aow.Units.AoWHScroll.TAOWHScrollBar PowerSlider;
        public Aow.Units.AoWImage.TAOWImage PAMana;
        public Aow.Units.AoWImage.TAOWImage PARes;
        public Aow.Rtl.TIvTranslator Translator;
        // AoWPowerDlg.pas:70
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
            var image = (Aow.Units.AoWImage.TAOWImage)Sender;
            X = image.AOWWindow.WinLeft;
            Y = image.AOWWindow.WinTop;
            Intensity = Aow.Rtl.Rtl.Round(((double)((100 - PowerSlider.Position) * 50)) / 100);
            var call__17 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt[7];
            call__17.ShowBlended(image.WinLeft + X, image.WinTop + Y, SD, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Intensity]);
        }

        // AoWPowerDlg.pas:128
        public void PAResDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int X = default;
            int Y = default;
            int Intensity = default;
            var image = (Aow.Units.AoWImage.TAOWImage)Sender;
            X = image.AOWWindow.WinLeft;
            Y = image.AOWWindow.WinTop;
            Intensity = Aow.Rtl.Rtl.Round(((double)(PowerSlider.Position * 50)) / 100);
            var call__19 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt[8];
            call__19.ShowBlended(image.WinLeft + X, image.WinTop + Y, SD, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Intensity]);
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
