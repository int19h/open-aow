// Translated from AoWPowerDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXEUnit;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWHScroll;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using Aow.Units.aowIntGfxMod;

namespace Aow.Units.AoWPowerDlg;

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
        TPlayerMagicControl Control;
        Dlg.Visible = true;
        Control = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl;
        PowerSlider.Position = Control.FResearchPercentage;
        var netPower = Control.GetNetPower();
        Power.Text = IntToStr(netPower);
        UpdatePower();
    }

    // AoWPowerDlg.pas:80
    public void UpdatePower()
    {
        int Upkeep;
        int Turns;
        int SpellID;
        int PointsLeft;
        int ManaPower;
        int ResearchPower;
        ManaPower = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.GetNetPower();
        PointsLeft = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.FResearchPointsLeft;
        Upkeep = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.GetManaUpkeep();
        SpellID = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.FResearchSpellID;
        ResearchPower = Round((double)(PowerSlider.Position * ManaPower) / 100);
        ManaPower -= ResearchPower;
        if (SpellID != 0)
        {
            Turns = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.ResearchTurns(ResearchPower, PointsLeft);
            var spell = AoWHSSet.FSpellControl.GetSpell(SpellID);
            SpellName.Text = spell.FSpellName;
            var fmtTurnsStr = FmtTurnsStr(Turns);
            ResearchLeft.Text = fmtTurnsStr;
        }
        else
        {
            var none = TranslateRStr(NoneRStr);
            SpellName.Text = none;
            ResearchLeft.Text = "-";
        }

        NetMana.Text = IntToStr(ManaPower) + "-" + IntToStr(Upkeep) + "=" + IntToStr(ManaPower - Upkeep);
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
        if (AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.ChangePowerSettings(PowerSlider.Position))
        {
            Dlg.Visible = false;
        }
    }

    // AoWPowerDlg.pas:115
    public void PAManaDraw(TObject Sender, TSurfaceDesc SD)
    {
        int X;
        int Y;
        int Intensity;
        var image = (TAOWImage)Sender;
        X = image.AOWWindow.WinLeft;
        Y = image.AOWWindow.WinTop;
        Intensity = Round((double)((100 - PowerSlider.Position) * 50) / 100);
        aowIntGfxModUnit.IntGfxMod.MiscArt[7].ShowBlended(image.WinLeft + X, image.WinTop + Y, SD, IntensityBlendTable.LookupTables[Intensity]);
    }

    // AoWPowerDlg.pas:128
    public void PAResDraw(TObject Sender, TSurfaceDesc SD)
    {
        int X;
        int Y;
        int Intensity;
        var image = (TAOWImage)Sender;
        X = image.AOWWindow.WinLeft;
        Y = image.AOWWindow.WinTop;
        Intensity = Round((double)(PowerSlider.Position * 50) / 100);
        aowIntGfxModUnit.IntGfxMod.MiscArt[8].ShowBlended(image.WinLeft + X, image.WinTop + Y, SD, IntensityBlendTable.LookupTables[Intensity]);
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

public static partial class AoWPowerDlgUnit
{
}
