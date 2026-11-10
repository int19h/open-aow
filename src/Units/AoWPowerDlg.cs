// Translated from AoWPowerDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWPowerDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// all fields are form components, power allocation slider
public partial class PowerDlg : DataModule
{
    public PowerDlg(Component owner) : base(owner)
    { }

    public AOWImage TitleIcon;
    public AOWButton Close;
    public AOWLabel Title;
    public AoWFrame BCeltL;
    public AOWButton CancelBtn;
    public AOWButton SetBtn;
    public AOWWindow Dlg;
    public AOWLabel NetMana;
    public AOWLabel NetMamaLbl;
    public AOWPanel UpkeePnl;
    public AOWLabel ResearchLeft;
    public AOWLabel SpellName;
    public AOWPanel ResearchPnl;
    public AOWPanel SliderPnl;
    public AOWLabel PowerLbl;
    public AOWLabel Power;
    public AOWLabel Mana;
    public AOWLabel Research;
    public AOWPanel ManaPnl;
    public AOWPanel ResPnl;
    public AOWLabel ManaLbl;
    public AOWLabel ResearchLbl;
    public AOWHScrollBar PowerSlider;
    public AOWImage PAMana;
    public AOWImage PARes;
    public IvTranslator Translator;

    // AoWPowerDlg.pas:70
    public void Execute()
    {
        PlayerMagicControl Control;
        Dlg.Visible = true;
        Control = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FMagicControl;
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
        ManaPower = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FMagicControl.GetNetPower();
        PointsLeft = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FMagicControl.FResearchPointsLeft;
        Upkeep = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FMagicControl.GetManaUpkeep();
        SpellID = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FMagicControl.FResearchSpellID;
        ResearchPower = Round((double)(PowerSlider.Position * ManaPower) / 100);
        ManaPower -= ResearchPower;
        if (SpellID != 0)
        {
            Turns = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FMagicControl.ResearchTurns(ResearchPower, PointsLeft);
            var spell = Engine.AoWHSSet.Current.FSpellControl.GetSpell(SpellID);
            SpellName.Text = spell.FSpellName;
            var fmtTurnsStr = FmtTurnsStr(Turns);
            ResearchLeft.Text = fmtTurnsStr;
        }
        else
        {
            var none = Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameTables.NoneRStr);
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
        if (World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FMagicControl.ChangePowerSettings(PowerSlider.Position))
        {
            Dlg.Visible = false;
        }
    }

    // AoWPowerDlg.pas:115
    public void PAManaDraw(AOWImage Sender, SurfaceDesc SD)
    {
        int X;
        int Y;
        int Intensity;
        X = Sender.AOWWindow.WinLeft;
        Y = Sender.AOWWindow.WinTop;
        Intensity = Round((double)((100 - PowerSlider.Position) * 50) / 100);
        DialogHosts.IntGfxMod.MiscArt[7].ShowBlended(Sender.WinLeft + X, Sender.WinTop + Y, SD, GfxState.IntensityBlendTable.LookupTables[Intensity]);
    }

    // AoWPowerDlg.pas:128
    public void PAResDraw(AOWImage Sender, SurfaceDesc SD)
    {
        int X;
        int Y;
        int Intensity;
        X = Sender.AOWWindow.WinLeft;
        Y = Sender.AOWWindow.WinTop;
        Intensity = Round((double)(PowerSlider.Position * 50) / 100);
        DialogHosts.IntGfxMod.MiscArt[8].ShowBlended(Sender.WinLeft + X, Sender.WinTop + Y, SD, GfxState.IntensityBlendTable.LookupTables[Intensity]);
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
            string s = Magic.Abilities.AbilityRules.TranslateRStr(XTurnRStr);
            FormatArg val = (FormatArg)((byte)49);
            return Magic.Abilities.AbilityRules.Format(s, new ReadOnlySpan<FormatArg>(ref val));
        }
        string s2 = Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameText.XTurnsRStr);
        FormatArg val2 = (FormatArg)(RtlUnit.IntToStr(Turns));
        return Magic.Abilities.AbilityRules.Format(s2, new ReadOnlySpan<FormatArg>(ref val2));
    }
}
