#nullable disable
// Translated from AoWPowerDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// all fields are form components, power allocation slider
public partial class PowerDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Label NetManaLabel { get; private set; }
    public Label ResearchLeftLabel { get; private set; }
    public Label SpellNameLabel { get; private set; }
    public Label PowerLabel { get; private set; }
    public Label ManaLabel { get; private set; }
    public Label ResearchLabel { get; private set; }
    public HorizontalScrollBar PowerSlider { get; private set; }

    // AoWPowerDlg.pas:70
    public void Execute()
    {
        PlayerMagicControl control;
        Window.Visible = true;
        control = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl;
        PowerSlider.Position = control.ResearchPercentage;
        int netPower = control.GetNetPower();
        PowerLabel.Text = IntToStr(netPower);
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
        manaPower = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.GetNetPower();
        pointsLeft = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ResearchPointsLeft;
        upkeep = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.GetManaUpkeep();
        spellId = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ResearchSpellId;
        researchPower = Round((double)(PowerSlider.Position * manaPower) / 100);
        manaPower -= researchPower;
        if (spellId != 0)
        {
            turns = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ResearchTurns(researchPower, pointsLeft);
            Spell spell = GamePack.Current.SpellControl.GetSpell(spellId);
            SpellNameLabel.Text = spell.SpellName;
            string formatTurnsString = FormatTurnsString(UI.Engine, turns: turns);
            ResearchLeftLabel.Text = formatTurnsString;
        }
        else
        {
            string none = UI.Engine.TranslateResourceString(GameTables.NoneText);
            SpellNameLabel.Text = none;
            ResearchLeftLabel.Text = "-";
        }

        NetManaLabel.Text = IntToStr(manaPower) + "-" + IntToStr(upkeep) + "=" + IntToStr(manaPower - upkeep);
        ManaLabel.Text = IntToStr(manaPower);
        ResearchLabel.Text = IntToStr(researchPower);
        Window.Drawn = false;
    }

    // AoWPowerDlg.pas:61
    public void CancelBtnClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWPowerDlg.pas:67
    public void PowerSliderChange() => UpdatePower();

    // AoWPowerDlg.pas:108
    public void SetBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ChangePowerSettings(PowerSlider.Position))
        {
            Window.Visible = false;
        }
    }

    // AoWPowerDlg.pas:115
    public void PAManaDraw(Image sender, LockedSurface lockedSurface)
    {
        int x;
        int y;
        int intensity;
        x = sender.Window.AbsoluteLeft;
        y = sender.Window.AbsoluteTop;
        intensity = Round((double)((100 - PowerSlider.Position) * 50) / 100);
        UI.Graphics.MiscArt[7].ShowBlended(sender.WinLeft + x, sender.WinTop + y, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[intensity]);
    }

    // AoWPowerDlg.pas:128
    public void PAResDraw(Image sender, LockedSurface lockedSurface)
    {
        int x;
        int y;
        int intensity;
        x = sender.Window.AbsoluteLeft;
        y = sender.Window.AbsoluteTop;
        intensity = Round((double)(PowerSlider.Position * 50) / 100);
        UI.Graphics.MiscArt[8].ShowBlended(sender.WinLeft + x, sender.WinTop + y, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[intensity]);
    }

    // AoWPowerDlg.pas:141
    public void DlgKeyDown(in ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    private static readonly string s_turnFormat = "%s turn";

    private static string FormatTurnsString(GameEngine engine, int turns)
    {
        if (turns == 1)
        {
            string s = engine.TranslateResourceString(s_turnFormat);
            FormatArg value = (FormatArg)(byte)49;
            return AbilityRules.Format(s, new ReadOnlySpan<FormatArg>(ref value));
        }
        string s2 = engine.TranslateResourceString(GameText.TurnsFormat);
        FormatArg value2 = (FormatArg)IntToStr(turns);
        return AbilityRules.Format(s2, new ReadOnlySpan<FormatArg>(ref value2));
    }
}
