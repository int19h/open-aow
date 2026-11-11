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
    public ExecuteDoneEvent FOnClose;
    public EnterWizardsTowerEventLog FEvent;

    // AoWWizardsTowerDlg.pas:54
    public void ClearEvent()
    {
        EnterWizardsTowerEventLog Event;
        ExecuteDoneEvent Notify;
        Dlg.Visible = false;
        Notify = FOnClose;
        Event = FEvent;
        FOnClose = null;
        FEvent = null;
        if (Notify != null)
        {
            Notify(Event);
        }

        if (Event != null)
        {
            Event.Release();
        }

        AoWHSMap.Current.FTutorialControl.ShowMessage(69, true, true);
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
        string Reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (!FEvent.FTower.Buy(AoWHSMap.Current.FSeatedPlayer, ref Reason))
        {
            AoWHSMap.Current.ShowWarning(Reason);
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
    public void SpellIcnDraw(Image Sender, LockedSurface SD)
    {
        Spell S;
        int X;
        int Y;
        var wizardsTower = FEvent.FTower;
        if (wizardsTower.FSpellID != 0)
        {
            S = AoWHSSet.Current.FSpellControl.GetSpell(wizardsTower.FSpellID);
            if (S != null)
            {
                if (S.FCastImages.GetImageSequence(10) != null)
                {
                    X = Sender.AOWWindow.WinLeft;
                    Y = Sender.AOWWindow.WinTop;
                    S.FCastImages.Show(10, Sender.WinLeft + X, Sender.WinTop + Y, 0, SD);
                }
            }
        }
    }

    // AoWWizardsTowerDlg.pas:101
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowTower(WizardsTowerDialog Dialog, EnterWizardsTowerEventLog Event, ExecuteDoneEvent Done)
    {
        WizardsTower Tower;
        Spell Spell;
        Dialog.FOnClose = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Tower = Dialog.FEvent.FTower;
        if (Tower.FSpellID != 0)
        {
            var buyCost = Dialog.FEvent.FTower.BuyCost();
            var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.ForXGoldRStr), [IntToStr(buyCost)]);
            Dialog.PriceLbl.Text = format + ".";
            Spell = AoWHSSet.Current.FSpellControl.GetSpell(Tower.FSpellID);
            Dialog.SpellName.Text = Spell.FSpellName;
            var magicSphere = AbilityRules.TranslateRStr(GameTables.TMagicSphereRStr[(int)Spell.FMagicSphere]);
            var level = AbilityRules.TranslateRStr(GameText.LevelRStr);
            Dialog.SpellClass.Text = magicSphere + ", " + level + " " + IntToStr(Spell.FSpellLevel);
            var castingCost = AbilityRules.TranslateRStr(GameText.CastingCostRStr);
            var mana = AbilityRules.TranslateRStr(GameText.ManaRStr);
            Dialog.SpellCost.Text = castingCost + ": " + IntToStr(Spell.FBaseManaCost) + " " + mana;
            var text = Spell.FSpellText.Text;
            Dialog.ResearchMemo.Items.Text = text;
        }
        else
        {
            Dialog.PriceLbl.Text = "";
            var none = AbilityRules.TranslateRStr(GameTables.NoneRStr);
            Dialog.SpellName.Text = none;
            Dialog.SpellClass.Text = "";
            Dialog.SpellCost.Text = "";
            Dialog.ResearchMemo.Items.Text = "";
        }

        Dialog.Dlg.Visible = true;
    }

}
