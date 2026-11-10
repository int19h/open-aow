// Translated from AoWWizardsTowerDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic.Spells;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWWizardsTowerDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Ui.Dialogs.AoWWinManUnit;

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

        AoWHSMap.FTutorialControl.ShowMessage(69, true, true);
    }

    // AoWWizardsTowerDlg.pas:67
    public void CancelBtnClick()
    {
        General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWWizardsTowerDlg.pas:73
    public void BuyBtnClick()
    {
        string Reason = "";
        General.PlayButtonSound(0);
        if (!FEvent.FTower.Buy(AoWHSMap.FSeatedPlayer, ref Reason))
        {
            AoWHSMap.ShowWarning(Reason);
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
            S = AoWHSSet.FSpellControl.GetSpell(wizardsTower.FSpellID);
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
}

public static partial class AoWWizardsTowerDlgUnit
{
}
