#nullable disable
// Translated from AoWWizardsTowerDlg.pas.
using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.UI.Dialogs;

// completion callback. tower-entry event
public partial class WizardsTowerDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Label SpellNameLabel { get; private set; }
    public Label SpellClassLabel { get; private set; }
    public Label SpellCostLabel { get; private set; }
    public Memo ResearchMemo { get; private set; }
    public Label PriceLabel { get; private set; }

    private event ExecuteDoneEvent OnClose;
    private EnterWizardsTowerEventLog _event;

    // AoWWizardsTowerDlg.pas:54
    public void ClearEvent()
    {
        Window.Visible = false;
        ExecuteDoneEvent notify = OnClose;
        EnterWizardsTowerEventLog eventValue = _event;
        OnClose = null;
        _event = null;
        notify?.Invoke(eventValue);

        _ = (eventValue?.Release());

        StrategicMap.Current.TutorialControl.ShowMessage(69, true, true);
    }

    // AoWWizardsTowerDlg.pas:67
    public void CancelBtnClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWWizardsTowerDlg.pas:73
    public void BuyBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (!_event.Tower.Buy(StrategicMap.Current.SeatedPlayer, out string reason))
        {
            StrategicMap.Current.ShowWarning(reason);
        }
        else
        {
            Window.Visible = false;
        }
    }

    // AoWWizardsTowerDlg.pas:82
    public void DlgHide() => ClearEvent();

    // AoWWizardsTowerDlg.pas:87
    public void SpellIcnDraw(Image sender, LockedSurface lockedSurface)
    {
        WizardsTower wizardsTower = _event.Tower;
        if (wizardsTower.SpellId != 0)
        {
            Spell s = GamePack.Current.SpellControl[wizardsTower.SpellId];
            if (s is not null)
            {
                if (s.CastImages.GetImageSequence(10) is not null)
                {
                    int x = sender.Window.AbsoluteLeft;
                    int y = sender.Window.AbsoluteTop;
                    _ = s.CastImages.Show(10, sender.WinLeft + x, sender.WinTop + y, 0, lockedSurface);
                }
            }
        }
    }

    // AoWWizardsTowerDlg.pas:101
    public void DlgKeyDown(in ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    internal static void ShowTower(WizardsTowerDialog dialog, EnterWizardsTowerEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.OnClose = done;
        dialog._event = eventValue;
        _ = dialog._event.AddRef();
        WizardsTower tower = dialog._event.Tower;
        if (tower.SpellId != 0)
        {
            int buyCost = dialog._event.Tower.BuyCost();
            string format = AbilityRules.Format(dialog.UI.Engine.TranslateResourceString(GameText.ForGoldFormat), [IntToStr(buyCost)]);
            dialog.PriceLabel.Text = $"{format}.";
            Spell spell = GamePack.Current.SpellControl[tower.SpellId];
            dialog.SpellNameLabel.Text = spell.SpellName;
            string magicSphere = dialog.UI.Engine.TranslateResourceString(GameTables.MagicSphereTexts[(int)spell.MagicSphere]);
            string level = dialog.UI.Engine.TranslateResourceString(GameText.LevelText);
            dialog.SpellClassLabel.Text = $"{magicSphere}, {level} {IntToStr(spell.SpellLevel)}";
            string castingCost = dialog.UI.Engine.TranslateResourceString(GameText.CastingCostText);
            string mana = dialog.UI.Engine.TranslateResourceString(GameText.ManaText);
            dialog.SpellCostLabel.Text = $"{castingCost}: {IntToStr(spell.BaseManaCost)} {mana}";
            string text = spell.SpellText.Text;
            dialog.ResearchMemo.Items.Text = text;
        }
        else
        {
            dialog.PriceLabel.Text = "";
            string none = dialog.UI.Engine.TranslateResourceString(GameTables.NoneText);
            dialog.SpellNameLabel.Text = none;
            dialog.SpellClassLabel.Text = "";
            dialog.SpellCostLabel.Text = "";
            dialog.ResearchMemo.Items.Text = "";
        }

        dialog.Window.Visible = true;
    }
}
