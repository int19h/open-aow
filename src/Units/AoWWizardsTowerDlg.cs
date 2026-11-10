// Translated from AoWWizardsTowerDlg.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWWizardsTowerDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// completion callback. tower-entry event
public partial class TWizardsTowerDlg : TDataModule
{
    public TWizardsTowerDlg(TComponent owner) : base(owner)
    { }

    public TAOWImage TitleIcon;
    public TAOWButton Close;
    public TAOWLabel Title;
    public TAoWFrame BCeltL;
    public TAOWButton CancelBtn;
    public TAOWButton BuyBtn;
    public TAOWWindow Dlg;
    public TAOWPanel ResearchPnl;
    public TAOWLabel SpellName;
    public TAOWImage SpellIcn;
    public TAOWLabel SpellClass;
    public TAOWLabel SpellCost;
    public TAOWMemo ResearchMemo;
    public TAOWLabel PriceLbl;
    public TAOWLabel OfferText;
    public TAOWPanel PicPnl;
    public TIvTranslator Translator;
    public TExecuteDoneEvent FOnClose;
    public TEnterWizardsTowerEventLog FEvent;

    // AoWWizardsTowerDlg.pas:54
    public void ClearEvent()
    {
        TEnterWizardsTowerEventLog Event;
        TExecuteDoneEvent Notify;
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

        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTutorialControl.ShowMessage(69, true, true);
    }

    // AoWWizardsTowerDlg.pas:67
    public void CancelBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWWizardsTowerDlg.pas:73
    public void BuyBtnClick()
    {
        string Reason = "";
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (!FEvent.FTower.Buy(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer, ref Reason))
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
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
    public void SpellIcnDraw(TAOWImage Sender, TSurfaceDesc SD)
    {
        TSpell S;
        int X;
        int Y;
        var wizardsTower = FEvent.FTower;
        if (wizardsTower.FSpellID != 0)
        {
            S = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(wizardsTower.FSpellID);
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
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowTower(TWizardsTowerDlg Dialog, TEnterWizardsTowerEventLog Event, TExecuteDoneEvent Done)
    {
        TWizardsTower Tower;
        TSpell Spell;
        Dialog.FOnClose = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Tower = Dialog.FEvent.FTower;
        if (Tower.FSpellID != 0)
        {
            var buyCost = Dialog.FEvent.FTower.BuyCost();
            var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ForXGoldRStr), [IntToStr(buyCost)]);
            Dialog.PriceLbl.Text = format + ".";
            Spell = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(Tower.FSpellID);
            Dialog.SpellName.Text = Spell.FSpellName;
            var magicSphere = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TMagicSphereRStr[(int)Spell.FMagicSphere]);
            var level = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.LevelRStr);
            Dialog.SpellClass.Text = magicSphere + ", " + level + " " + IntToStr(Spell.FSpellLevel);
            var castingCost = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.CastingCostRStr);
            var mana = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ManaRStr);
            Dialog.SpellCost.Text = castingCost + ": " + IntToStr(Spell.FBaseManaCost) + " " + mana;
            var text = Spell.FSpellText.Text;
            Dialog.ResearchMemo.Items.Text = text;
        }
        else
        {
            Dialog.PriceLbl.Text = "";
            var none = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.NoneRStr);
            Dialog.SpellName.Text = none;
            Dialog.SpellClass.Text = "";
            Dialog.SpellCost.Text = "";
            Dialog.ResearchMemo.Items.Text = "";
        }

        Dialog.Dlg.Visible = true;
    }

}
