// Translated from AoWConstructDlg.pas.
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// activation request
public partial class ConstructDialog : DataModule
{
    public ConstructDialog(Component owner) : base(owner)
    { }

    public Image TitleIcon;
    public Button Close;
    public Label Title;
    public Frame BCeltL;
    public Button CancelBtn;
    public Button ConstructBtn;
    public Window Dlg;
    public Panel ConstructPnl;
    public ListBox ConstructionList;
    public Button SortConstructionsBtn;
    public Button SortConstructionCostBtn;
    public ListBox ConstructionCostList;
    public ListBox ConstructionTurnsList;
    public Button SortConstructionTurnsBtn;
    public Label AOWLabel1;
    public VerticalScrollBar ConSB;
    public Image CeltM;
    public Button AOWButton2;
    public VerticalScrollBar T4VSB;
    public IvTranslator Translator;
    public AbilityActivation FRequest;

    // AoWConstructDlg.pas:117
    public void Execute(AbilityActivation Request)
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
        Troop U;
        string Name = "";
        ConstructAbility Ability;
        ConstructionControl Control;
        Dlg.FDrawn = false;
        U = AoWHSMap.Current.FUnitControl.FindUnit(FRequest.FUnitID);
        ConstructionList.Items.Clear();
        ConstructionCostList.Items.Clear();
        ConstructionTurnsList.Items.Clear();
        Ability = (ConstructAbility)AoWHSSet.Current.FAbilityControl.GetAbility(125);
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
        DialogHosts.General.PlayButtonSound(0);
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
        Troop U;
        DialogHosts.General.PlayButtonSound(0);
        bool condition = ConstructionList.Selected >= 0;
        if (condition)
        {
            var count = ConstructionList.Items.Count;
            condition = ConstructionList.Selected < count;
        }

        if (condition)
        {
            U = AoWHSMap.Current.FUnitControl.FindUnit(FRequest.FUnitID);
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
                AoWHSMap.Current.ShowWarning(Reason);
            }
        }
    }

    // AoWConstructDlg.pas:96
    public void SortConstructionsBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWConstructDlg.pas:99
    public void SortConstructionCostBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWConstructDlg.pas:102
    public void SortConstructionTurnsBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
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
