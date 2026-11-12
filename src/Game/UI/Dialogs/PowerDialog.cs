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

    public Window Dlg;
    public Label NetMana;
    public Label ResearchLeft;
    public Label SpellName;
    public Label Power;
    public Label Mana;
    public Label Research;
    public HorizontalScrollBar PowerSlider;

    // AoWPowerDlg.pas:70
    public void Execute()
    {
        PlayerMagicControl control;
        Dlg.Visible = true;
        control = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl;
        PowerSlider.Position = control.ResearchPercentage;
        var netPower = control.GetNetPower();
        Power.Text = IntToStr(netPower);
        UpdatePower();
    }

    // AoWPowerDlg.pas:80
    public void UpdatePower()
    {
        int upkeep;
        int turns;
        int spellId;
        int pointsLeft;
        int manaPower;
        int researchPower;
        manaPower = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.GetNetPower();
        pointsLeft = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.ResearchPointsLeft;
        upkeep = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.GetManaUpkeep();
        spellId = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.ResearchSpellId;
        researchPower = Round((double)(PowerSlider.Position * manaPower) / 100);
        manaPower -= researchPower;
        if (spellId != 0)
        {
            turns = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.ResearchTurns(researchPower, pointsLeft);
            var spell = AoWHSSet.Current.SpellControl.GetSpell(spellId);
            SpellName.Text = spell.SpellName;
            var formatTurnsString = FormatTurnsString(turns);
            ResearchLeft.Text = formatTurnsString;
        }
        else
        {
            var none = AbilityRules.TranslateResourceString(GameTables.NoneText);
            SpellName.Text = none;
            ResearchLeft.Text = "-";
        }

        NetMana.Text = IntToStr(manaPower) + "-" + IntToStr(upkeep) + "=" + IntToStr(manaPower - upkeep);
        Mana.Text = IntToStr(manaPower);
        Research.Text = IntToStr(researchPower);
        Dlg.Drawn = false;
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
        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.ChangePowerSettings(PowerSlider.Position))
        {
            Dlg.Visible = false;
        }
    }

    // AoWPowerDlg.pas:115
    public void PAManaDraw(Image sender, LockedSurface lockedSurface)
    {
        int X;
        int Y;
        int intensity;
        X = sender.Window.WinLeft;
        Y = sender.Window.WinTop;
        intensity = Round((double)((100 - PowerSlider.Position) * 50) / 100);
        DialogHosts.UIGraphics.MiscArt[7].ShowBlended(sender.WinLeft + X, sender.WinTop + Y, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[intensity]);
    }

    // AoWPowerDlg.pas:128
    public void PAResDraw(Image sender, LockedSurface lockedSurface)
    {
        int X;
        int Y;
        int intensity;
        X = sender.Window.WinLeft;
        Y = sender.Window.WinTop;
        intensity = Round((double)(PowerSlider.Position * 50) / 100);
        DialogHosts.UIGraphics.MiscArt[8].ShowBlended(sender.WinLeft + X, sender.WinTop + Y, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[intensity]);
    }

    // AoWPowerDlg.pas:141
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Dlg.Visible = false;
        }
    }

    private static readonly string s_turnFormat = "%s turn";

    private static string FormatTurnsString(int turns)
    {
        if (turns == 1)
        {
            string s = AbilityRules.TranslateResourceString(s_turnFormat);
            FormatArg value = (FormatArg)((byte)49);
            return AbilityRules.Format(s, new ReadOnlySpan<FormatArg>(ref value));
        }
        string s2 = AbilityRules.TranslateResourceString(GameText.TurnsFormat);
        FormatArg value2 = (FormatArg)(RtlUnit.IntToStr(turns));
        return AbilityRules.Format(s2, new ReadOnlySpan<FormatArg>(ref value2));
    }
}
