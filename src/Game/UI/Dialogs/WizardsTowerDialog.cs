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
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// completion callback. tower-entry event
public partial class WizardsTowerDialog : DataModule
{
    public WizardsTowerDialog(Component owner) : base(owner)
    { }

    public Image TitleIcon;
    public Button Close;
    public Label Title;
    public Frame BCeltL;
    public Button CancelBtn;
    public Button BuyBtn;
    public Window Dlg;
    public Panel ResearchPnl;
    public Label SpellName;
    public Image SpellIcn;
    public Label SpellClass;
    public Label SpellCost;
    public Memo ResearchMemo;
    public Label PriceLbl;
    public Label OfferText;
    public Panel PicPnl;
    public IvTranslator Translator;
    private ExecuteDoneEvent _onClose;
    private EnterWizardsTowerEventLog _event;

    // AoWWizardsTowerDlg.pas:54
    public void ClearEvent()
    {
        EnterWizardsTowerEventLog eventValue;
        ExecuteDoneEvent notify;
        Dlg.Visible = false;
        notify = _onClose;
        eventValue = _event;
        _onClose = null;
        _event = null;
        if (notify != null)
        {
            notify(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
        }

        AoWHSMap.Current.TutorialControl.ShowMessage(69, true, true);
    }

    // AoWWizardsTowerDlg.pas:67
    public void CancelBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWWizardsTowerDlg.pas:73
    public void BuyBtnClick()
    {
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (!_event.Tower.Buy(AoWHSMap.Current.SeatedPlayer, ref reason))
        {
            AoWHSMap.Current.ShowWarning(reason);
        }
        else
        {
            Dlg.Visible = false;
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
            S = AoWHSSet.Current.SpellControl.GetSpell(wizardsTower.SpellId);
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
            Dlg.Visible = false;
        }
    }

    internal static void ShowTower(WizardsTowerDialog dialog, EnterWizardsTowerEventLog eventValue, ExecuteDoneEvent done)
    {
        WizardsTower tower;
        Spell spell;
        dialog._onClose = done;
        dialog._event = eventValue;
        dialog._event.AddRef();
        tower = dialog._event.Tower;
        if (tower.SpellId != 0)
        {
            var buyCost = dialog._event.Tower.BuyCost();
            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.ForGoldFormat), [IntToStr(buyCost)]);
            dialog.PriceLbl.Text = format + ".";
            spell = AoWHSSet.Current.SpellControl.GetSpell(tower.SpellId);
            dialog.SpellName.Text = spell.SpellName;
            var magicSphere = AbilityRules.TranslateResourceString(GameTables.s_magicSphereTexts[(int)spell.MagicSphere]);
            var level = AbilityRules.TranslateResourceString(GameText.LevelText);
            dialog.SpellClass.Text = magicSphere + ", " + level + " " + IntToStr(spell.SpellLevel);
            var castingCost = AbilityRules.TranslateResourceString(GameText.CastingCostText);
            var mana = AbilityRules.TranslateResourceString(GameText.ManaText);
            dialog.SpellCost.Text = castingCost + ": " + IntToStr(spell.BaseManaCost) + " " + mana;
            var text = spell.SpellText.Text;
            dialog.ResearchMemo.Items.Text = text;
        }
        else
        {
            dialog.PriceLbl.Text = "";
            var none = AbilityRules.TranslateResourceString(GameTables.NoneText);
            dialog.SpellName.Text = none;
            dialog.SpellClass.Text = "";
            dialog.SpellCost.Text = "";
            dialog.ResearchMemo.Items.Text = "";
        }

        dialog.Dlg.Visible = true;
    }

}
