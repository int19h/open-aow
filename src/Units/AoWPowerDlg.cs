// Translated from AoWPowerDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWHScroll;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWPowerDlg
{
    // all fields are form components, power allocation slider
    public partial class TPowerDlg : TDataModule
    {
        public TPowerDlg(TComponent owner) : base(owner)
        { }

        public TAOWImage TitleIcon;
        public TAOWButton Close;
        public TAOWLabel Title;
        public TAoWFrame BCeltL;
        public TAOWButton CancelBtn;
        public TAOWButton SetBtn;
        public TAOWWindow Dlg;
        public TAOWLabel NetMana;
        public TAOWLabel NetMamaLbl;
        public TAOWPanel UpkeePnl;
        public TAOWLabel ResearchLeft;
        public TAOWLabel SpellName;
        public TAOWPanel ResearchPnl;
        public TAOWPanel SliderPnl;
        public TAOWLabel PowerLbl;
        public TAOWLabel Power;
        public TAOWLabel Mana;
        public TAOWLabel Research;
        public TAOWPanel ManaPnl;
        public TAOWPanel ResPnl;
        public TAOWLabel ManaLbl;
        public TAOWLabel ResearchLbl;
        public TAOWHScrollBar PowerSlider;
        public TAOWImage PAMana;
        public TAOWImage PARes;
        public TIvTranslator Translator;
        // AoWPowerDlg.pas:70
        public void Execute()
        {
            TPlayerMagicControl Control = default;
            Dlg.Visible = true;
            var call__1 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            Control = call__1.FMagicControl;
            PowerSlider.Position = Control.FResearchPercentage;
            var call__2 = Control.GetNetPower();
            Power.Text = IntToStr(call__2);
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
            var call__3 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__4 = call__3.FMagicControl.GetNetPower();
            ManaPower = call__4;
            var call__5 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            PointsLeft = call__5.FMagicControl.FResearchPointsLeft;
            var call__6 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__7 = call__6.FMagicControl.GetManaUpkeep();
            Upkeep = call__7;
            var call__8 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            SpellID = call__8.FMagicControl.FResearchSpellID;
            ResearchPower = Round((double)(PowerSlider.Position * ManaPower) / 100);
            ManaPower -= ResearchPower;
            if (SpellID != 0)
            {
                var call__9 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__10 = call__9.FMagicControl.ResearchTurns(ResearchPower, PointsLeft);
                Turns = call__10;
                var call__11 = AoWHSSet.FSpellControl.GetSpell(SpellID);
                SpellName.Text = call__11.FSpellName;
                var call__12 = FmtTurnsStr(Turns);
                ResearchLeft.Text = call__12;
            }
            else
            {
                var call__13 = TranslateRStr(NoneRStr);
                SpellName.Text = call__13;
                ResearchLeft.Text = AnsiString.FromBytes(45);
            }

            NetMana.Text = IntToStr(ManaPower) + AnsiString.FromBytes(45) + IntToStr(Upkeep) + AnsiString.FromBytes(61) + IntToStr(ManaPower - Upkeep);
            Mana.Text = IntToStr(ManaPower);
            Research.Text = IntToStr(ResearchPower);
            Dlg.FDrawn = false;
        }

        // AoWPowerDlg.pas:61
        public void CancelBtnClick()
        {
            General.PlayButtonSound(0);
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
            General.PlayButtonSound(0);
            var call__14 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__15 = call__14.FMagicControl.ChangePowerSettings(PowerSlider.Position);
            if (call__15)
            {
                Dlg.Visible = false;
            }
        }

        // AoWPowerDlg.pas:115
        public void PAManaDraw(TObject Sender, TSurfaceDesc SD)
        {
            int X = default;
            int Y = default;
            int Intensity = default;
            var image = (TAOWImage)Sender;
            X = image.AOWWindow.WinLeft;
            Y = image.AOWWindow.WinTop;
            Intensity = Round((double)((100 - PowerSlider.Position) * 50) / 100);
            var call__17 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt[7];
            call__17.ShowBlended(image.WinLeft + X, image.WinTop + Y, SD, IntensityBlendTable.LookupTables[Intensity]);
        }

        // AoWPowerDlg.pas:128
        public void PAResDraw(TObject Sender, TSurfaceDesc SD)
        {
            int X = default;
            int Y = default;
            int Intensity = default;
            var image = (TAOWImage)Sender;
            X = image.AOWWindow.WinLeft;
            Y = image.AOWWindow.WinTop;
            Intensity = Round((double)(PowerSlider.Position * 50) / 100);
            var call__19 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt[8];
            call__19.ShowBlended(image.WinLeft + X, image.WinTop + Y, SD, IntensityBlendTable.LookupTables[Intensity]);
        }

        // AoWPowerDlg.pas:141
        public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
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
