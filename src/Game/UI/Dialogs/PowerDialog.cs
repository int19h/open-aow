#nullable disable warnings
// Translated from AoWPowerDlg.pas.
using System.Globalization;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.UI.Dialogs;

// all fields are form components, power allocation slider
public sealed partial class PowerDialog(UIContext ui) : UIModule(ui)
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
        Window.Visible = true;
        PlayerMagicControl control = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl;
        PowerSlider.Position = control.ResearchPercentage;
        int netPower = control.GetNetPower();
        PowerLabel.Text = netPower.ToString(CultureInfo.InvariantCulture);
        UpdatePower();
    }

    // AoWPowerDlg.pas:80
    public void UpdatePower()
    {
        int manaPower = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.GetNetPower();
        int pointsLeft = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ResearchPointsLeft;
        int upkeep = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.GetManaUpkeep();
        int spellId = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ResearchSpellId;
        int researchPower = Round((double)(PowerSlider.Position * manaPower) / 100);
        manaPower -= researchPower;
        if (spellId != 0)
        {
            int turns = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.ResearchTurns(researchPower, points: pointsLeft);
            Spell? spell = GamePack.Current.SpellControl[spellId];
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

        NetManaLabel.Text = $"{manaPower.ToString(CultureInfo.InvariantCulture)}-{upkeep.ToString(CultureInfo.InvariantCulture)}={(manaPower - upkeep).ToString(CultureInfo.InvariantCulture)}";
        ManaLabel.Text = manaPower.ToString(CultureInfo.InvariantCulture);
        ResearchLabel.Text = researchPower.ToString(CultureInfo.InvariantCulture);
        Window.IsDrawn = false;
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
        int x = sender.Window.AbsoluteLeft;
        int y = sender.Window.AbsoluteTop;
        int intensity = Round((double)((100 - PowerSlider.Position) * 50) / 100);
        UI.Graphics.MiscArt[7].ShowBlended(sender.WinLeft + x, sender.WinTop + y, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[intensity]);
    }

    // AoWPowerDlg.pas:128
    public void PAResDraw(Image sender, LockedSurface lockedSurface)
    {
        int x = sender.Window.AbsoluteLeft;
        int y = sender.Window.AbsoluteTop;
        int intensity = Round((double)(PowerSlider.Position * 50) / 100);
        UI.Graphics.MiscArt[8].ShowBlended(sender.WinLeft + x, sender.WinTop + y, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[intensity]);
    }

    // AoWPowerDlg.pas:141
    public void DlgKeyDown(in ushort key, ShiftStates shift)
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
            return AbilityRules.Format(s, arguments: new ReadOnlySpan<FormatArg>(ref value));
        }
        string s2 = engine.TranslateResourceString(GameText.TurnsFormat);
        FormatArg value2 = (FormatArg)turns.ToString(CultureInfo.InvariantCulture);
        return AbilityRules.Format(s2, arguments: new ReadOnlySpan<FormatArg>(ref value2));
    }
}
