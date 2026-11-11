// Translated from AoWPowerDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// all fields are form components, power allocation slider
public partial class PowerDialog : DataModule
{
    public PowerDialog(Component owner) : base(owner)
    { }

    public Image TitleIcon;
    public Button Close;
    public Label Title;
    public Frame BCeltL;
    public Button CancelBtn;
    public Button SetBtn;
    public Window Dlg;
    public Label NetMana;
    public Label NetMamaLbl;
    public Panel UpkeePnl;
    public Label ResearchLeft;
    public Label SpellName;
    public Panel ResearchPnl;
    public Panel SliderPnl;
    public Label PowerLbl;
    public Label Power;
    public Label Mana;
    public Label Research;
    public Panel ManaPnl;
    public Panel ResPnl;
    public Label ManaLbl;
    public Label ResearchLbl;
    public HorizontalScrollBar PowerSlider;
    public Image PAMana;
    public Image PARes;
    public IvTranslator Translator;

    // AoWPowerDlg.pas:70
    public void Execute()
    {
        PlayerMagicControl Control;
        Dlg.Visible = true;
        Control = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl;
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
        ManaPower = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.GetNetPower();
        PointsLeft = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.FResearchPointsLeft;
        Upkeep = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.GetManaUpkeep();
        SpellID = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.FResearchSpellID;
        ResearchPower = Round((double)(PowerSlider.Position * ManaPower) / 100);
        ManaPower -= ResearchPower;
        if (SpellID != 0)
        {
            Turns = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.ResearchTurns(ResearchPower, PointsLeft);
            var spell = AoWHSSet.Current.FSpellControl.GetSpell(SpellID);
            SpellName.Text = spell.FSpellName;
            var fmtTurnsStr = FmtTurnsStr(Turns);
            ResearchLeft.Text = fmtTurnsStr;
        }
        else
        {
            var none = AbilityRules.TranslateRStr(GameTables.NoneRStr);
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
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.ChangePowerSettings(PowerSlider.Position))
        {
            Dlg.Visible = false;
        }
    }

    // AoWPowerDlg.pas:115
    public void PAManaDraw(Image Sender, LockedSurface SD)
    {
        int X;
        int Y;
        int Intensity;
        X = Sender.AOWWindow.WinLeft;
        Y = Sender.AOWWindow.WinTop;
        Intensity = Round((double)((100 - PowerSlider.Position) * 50) / 100);
        DialogHosts.IntGfxMod.MiscArt[7].ShowBlended(Sender.WinLeft + X, Sender.WinTop + Y, SD, GraphicsState.IntensityBlendTable.LookupTables[Intensity]);
    }

    // AoWPowerDlg.pas:128
    public void PAResDraw(Image Sender, LockedSurface SD)
    {
        int X;
        int Y;
        int Intensity;
        X = Sender.AOWWindow.WinLeft;
        Y = Sender.AOWWindow.WinTop;
        Intensity = Round((double)(PowerSlider.Position * 50) / 100);
        DialogHosts.IntGfxMod.MiscArt[8].ShowBlended(Sender.WinLeft + X, Sender.WinTop + Y, SD, GraphicsState.IntensityBlendTable.LookupTables[Intensity]);
    }

    // AoWPowerDlg.pas:141
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    private static readonly string XTurnRStr = "%s turn";

    private static string FmtTurnsStr(int Turns)
    {
        if (Turns == 1)
        {
            string s = AbilityRules.TranslateRStr(XTurnRStr);
            FormatArg val = (FormatArg)((byte)49);
            return AbilityRules.Format(s, new ReadOnlySpan<FormatArg>(ref val));
        }
        string s2 = AbilityRules.TranslateRStr(GameText.XTurnsRStr);
        FormatArg val2 = (FormatArg)(RtlUnit.IntToStr(Turns));
        return AbilityRules.Format(s2, new ReadOnlySpan<FormatArg>(ref val2));
    }
}
