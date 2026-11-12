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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// completion callback. tower-entry event
public partial class WizardsTowerDialog : DataModule
{
    public WizardsTowerDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public Label SpellNameLabel;
    public Label SpellClassLabel;
    public Label SpellCostLabel;
    public Memo ResearchMemo;
    public Label PriceLabel;
    private event ExecuteDoneEvent OnClose;
    private EnterWizardsTowerEventLog _event;

    // AoWWizardsTowerDlg.pas:54
    public void ClearEvent()
    {
        EnterWizardsTowerEventLog eventValue;
        ExecuteDoneEvent notify;
        Window.Visible = false;
        notify = OnClose;
        eventValue = _event;
        OnClose = null;
        _event = null;
        if (notify != null)
        {
            notify(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
        }

        StrategicMap.Current.TutorialControl.ShowMessage(69, true, true);
    }

    // AoWWizardsTowerDlg.pas:67
    public void CancelBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWWizardsTowerDlg.pas:73
    public void BuyBtnClick()
    {
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (!_event.Tower.Buy(StrategicMap.Current.SeatedPlayer, ref reason))
        {
            StrategicMap.Current.ShowWarning(reason);
        }
        else
        {
            Window.Visible = false;
        }
    }

    // AoWWizardsTowerDlg.pas:82
    public void DlgHide()
    {
        ClearEvent();
    }

    // AoWWizardsTowerDlg.pas:87
    public void SpellIcnDraw(Image sender, LockedSurface lockedSurface)
    {
        Spell S;
        int X;
        int Y;
        var wizardsTower = _event.Tower;
        if (wizardsTower.SpellId != 0)
        {
            S = GamePack.Current.SpellControl.GetSpell(wizardsTower.SpellId);
            if (S != null)
            {
                if (S.CastImages.GetImageSequence(10) != null)
                {
                    X = sender.Window.WinLeft;
                    Y = sender.Window.WinTop;
                    S.CastImages.Show(10, sender.WinLeft + X, sender.WinTop + Y, 0, lockedSurface);
                }
            }
        }
    }

    // AoWWizardsTowerDlg.pas:101
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    internal static void ShowTower(WizardsTowerDialog dialog, EnterWizardsTowerEventLog eventValue, ExecuteDoneEvent done)
    {
        WizardsTower tower;
        Spell spell;
        dialog.OnClose = done;
        dialog._event = eventValue;
        dialog._event.AddRef();
        tower = dialog._event.Tower;
        if (tower.SpellId != 0)
        {
            var buyCost = dialog._event.Tower.BuyCost();
            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.ForGoldFormat), [IntToStr(buyCost)]);
            dialog.PriceLabel.Text = format + ".";
            spell = GamePack.Current.SpellControl.GetSpell(tower.SpellId);
            dialog.SpellNameLabel.Text = spell.SpellName;
            var magicSphere = AbilityRules.TranslateResourceString(GameTables.MagicSphereTexts[(int)spell.MagicSphere]);
            var level = AbilityRules.TranslateResourceString(GameText.LevelText);
            dialog.SpellClassLabel.Text = magicSphere + ", " + level + " " + IntToStr(spell.SpellLevel);
            var castingCost = AbilityRules.TranslateResourceString(GameText.CastingCostText);
            var mana = AbilityRules.TranslateResourceString(GameText.ManaText);
            dialog.SpellCostLabel.Text = castingCost + ": " + IntToStr(spell.BaseManaCost) + " " + mana;
            var text = spell.SpellText.Text;
            dialog.ResearchMemo.Items.Text = text;
        }
        else
        {
            dialog.PriceLabel.Text = "";
            var none = AbilityRules.TranslateResourceString(GameTables.NoneText);
            dialog.SpellNameLabel.Text = none;
            dialog.SpellClassLabel.Text = "";
            dialog.SpellCostLabel.Text = "";
            dialog.ResearchMemo.Items.Text = "";
        }

        dialog.Window.Visible = true;
    }

}
