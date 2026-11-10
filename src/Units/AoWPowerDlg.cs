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
        Control = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FMagicControl;
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
        ManaPower = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FMagicControl.GetNetPower();
        PointsLeft = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FMagicControl.FResearchPointsLeft;
        Upkeep = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FMagicControl.GetManaUpkeep();
        SpellID = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FMagicControl.FResearchSpellID;
        ResearchPower = Round((double)(PowerSlider.Position * ManaPower) / 100);
        ManaPower -= ResearchPower;
        if (SpellID != 0)
        {
            Turns = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FMagicControl.ResearchTurns(ResearchPower, PointsLeft);
            var spell = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(SpellID);
            SpellName.Text = spell.FSpellName;
            var fmtTurnsStr = Aow.Game.Ui.Dialogs.TPowerDlg.FmtTurnsStr(Turns);
            ResearchLeft.Text = fmtTurnsStr;
        }
        else
        {
            var none = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.NoneRStr);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FMagicControl.ChangePowerSettings(PowerSlider.Position))
        {
            Dlg.Visible = false;
        }
    }

    // AoWPowerDlg.pas:115
    public void PAManaDraw(TAOWImage Sender, TSurfaceDesc SD)
    {
        int X;
        int Y;
        int Intensity;
        X = Sender.AOWWindow.WinLeft;
        Y = Sender.AOWWindow.WinTop;
        Intensity = Round((double)((100 - PowerSlider.Position) * 50) / 100);
        Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.MiscArt[7].ShowBlended(Sender.WinLeft + X, Sender.WinTop + Y, SD, Aow.Game.Draw.GfxState.IntensityBlendTable.LookupTables[Intensity]);
    }

    // AoWPowerDlg.pas:128
    public void PAResDraw(TAOWImage Sender, TSurfaceDesc SD)
    {
        int X;
        int Y;
        int Intensity;
        X = Sender.AOWWindow.WinLeft;
        Y = Sender.AOWWindow.WinTop;
        Intensity = Round((double)(PowerSlider.Position * 50) / 100);
        Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.MiscArt[8].ShowBlended(Sender.WinLeft + X, Sender.WinTop + Y, SD, Aow.Game.Draw.GfxState.IntensityBlendTable.LookupTables[Intensity]);
    }

    // AoWPowerDlg.pas:141
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
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
            string s = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(XTurnRStr);
            FormatArg val = (FormatArg)((byte)49);
            return global::Aow.Game.Magic.Abilities.AbilityRules.Format(s, new global::System.ReadOnlySpan<FormatArg>(ref val));
        }
        string s2 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(global::Aow.Game.Magic.Abilities.GameText.XTurnsRStr);
        FormatArg val2 = (FormatArg)(RtlUnit.IntToStr(Turns));
        return global::Aow.Game.Magic.Abilities.AbilityRules.Format(s2, new global::System.ReadOnlySpan<FormatArg>(ref val2));
    }
}
