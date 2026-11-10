// Translated from AoWConstructDlg.pas.
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWConstructDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// activation request
public partial class ConstructDlg : DataModule
{
    public ConstructDlg(Component owner) : base(owner)
    { }

    public AOWImage TitleIcon;
    public AOWButton Close;
    public AOWLabel Title;
    public AoWFrame BCeltL;
    public AOWButton CancelBtn;
    public AOWButton ConstructBtn;
    public AOWWindow Dlg;
    public AOWPanel ConstructPnl;
    public AOWListBox ConstructionList;
    public AOWButton SortConstructionsBtn;
    public AOWButton SortConstructionCostBtn;
    public AOWListBox ConstructionCostList;
    public AOWListBox ConstructionTurnsList;
    public AOWButton SortConstructionTurnsBtn;
    public AOWLabel AOWLabel1;
    public AOWVScrollBar ConSB;
    public AOWImage CeltM;
    public AOWButton AOWButton2;
    public AOWVScrollBar T4VSB;
    public IvTranslator Translator;
    public ActivateAbilityInfo FRequest;

    // AoWConstructDlg.pas:117
    public void Execute(ActivateAbilityInfo Request)
    {
        if (FRequest != null)
        {
            FRequest.Release();
        }

        FRequest = Request;
        FRequest.AddRef();
        UpdateConstructions();
        Dlg.Visible = true;
    }

    // AoWConstructDlg.pas:108
    public void Finish()
    {
        if (FRequest != null)
        {
            FRequest.Release();
            FRequest = null;
        }
    }

    // AoWConstructDlg.pas:142
    public void UpdateConstructions()
    {
        int Days = default;
        int Gold = default;
        AbstractUnit U;
        string Name = "";
        ConstructAbility Ability;
        ConstructionControl Control;
        Dlg.FDrawn = false;
        U = Aow.Game.World.Map.AoWHSMap.Current.FUnitControl.FindUnit(FRequest.FUnitID);
        ConstructionList.Items.Clear();
        ConstructionCostList.Items.Clear();
        ConstructionTurnsList.Items.Clear();
        Ability = (ConstructAbility)Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(125);
        for (int I = 0, last = Ability.FConstructionControls.Count - 1; I <= last; I++)
        {
            Control = Ability.FConstructionControls.GetConstructionControl(I);
            Control.GetInfo(U, ref Name, ref Gold, ref Days);
            ConstructionList.Items.AddObject(Name, Control);
            if (Gold != 0)
            {
                ConstructionCostList.Items.Add(IntToStr(Gold));
            }
            else
            {
                ConstructionCostList.Items.Add("-");
            }

            if (Days != 0)
            {
                ConstructionTurnsList.Items.Add(IntToStr(Days));
            }
            else
            {
                ConstructionTurnsList.Items.Add("-");
            }
        }
    }

    // AoWConstructDlg.pas:126
    public void SelectConstruction(int Index)
    {
        if (Index >= 0 && Index < ConstructionList.Items.Count)
        {
            ConstructionList.Selected = Index;
            ConstructionCostList.Selected = Index;
            ConstructionTurnsList.Selected = Index;
        }
        else
        {
            ConstructionList.Selected = -1;
            ConstructionCostList.Selected = -1;
            ConstructionTurnsList.Selected = -1;
        }
    }

    // AoWConstructDlg.pas:62
    public void CancelBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWConstructDlg.pas:68
    public void ConstructionListChange()
    {
        SelectConstruction(ConstructionList.Selected);
    }

    // AoWConstructDlg.pas:71
    public void ConstructionCostListChange()
    {
        SelectConstruction(ConstructionCostList.Selected);
    }

    // AoWConstructDlg.pas:74
    public void ConstructionTurnsListChange()
    {
        SelectConstruction(ConstructionTurnsList.Selected);
    }

    // AoWConstructDlg.pas:77
    public void ConstructBtnClick()
    {
        string Reason = "";
        ConstructionControl Control;
        AbstractUnit U;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        bool condition = ConstructionList.Selected >= 0;
        if (condition)
        {
            var count = ConstructionList.Items.Count;
            condition = ConstructionList.Selected < count;
        }

        if (condition)
        {
            U = Aow.Game.World.Map.AoWHSMap.Current.FUnitControl.FindUnit(FRequest.FUnitID);
            Control = (ConstructionControl)ConstructionList.Items.Objects[ConstructionList.Selected];
            if (Control.CanConstruct(U, ref Reason))
            {
                if (Control.Construct(U))
                {
                    Dlg.Visible = false;
                }
            }
            else
            {
                Aow.Game.World.Map.AoWHSMap.Current.ShowWarning(Reason);
            }
        }
    }

    // AoWConstructDlg.pas:96
    public void SortConstructionsBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
    }

    // AoWConstructDlg.pas:99
    public void SortConstructionCostBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
    }

    // AoWConstructDlg.pas:102
    public void SortConstructionTurnsBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
    }

    // AoWConstructDlg.pas:105
    public void DlgHide()
    {
        Finish();
    }

    // AoWConstructDlg.pas:169
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }
}
