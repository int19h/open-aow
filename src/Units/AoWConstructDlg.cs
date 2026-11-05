// Translated from AoWConstructDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWConstructDlg;

// activation request
public partial class TConstructDlg : TDataModule
{
    public TConstructDlg(TComponent owner) : base(owner)
    { }

    public TAOWImage TitleIcon;
    public TAOWButton Close;
    public TAOWLabel Title;
    public TAoWFrame BCeltL;
    public TAOWButton CancelBtn;
    public TAOWButton ConstructBtn;
    public TAOWWindow Dlg;
    public TAOWPanel ConstructPnl;
    public TAOWListBox ConstructionList;
    public TAOWButton SortConstructionsBtn;
    public TAOWButton SortConstructionCostBtn;
    public TAOWListBox ConstructionCostList;
    public TAOWListBox ConstructionTurnsList;
    public TAOWButton SortConstructionTurnsBtn;
    public TAOWLabel AOWLabel1;
    public TAOWVScrollBar ConSB;
    public TAOWImage CeltM;
    public TAOWButton AOWButton2;
    public TAOWVScrollBar T4VSB;
    public TIvTranslator Translator;
    public TActivateAbilityInfo FRequest;
    // AoWConstructDlg.pas:117
    public void Execute(TActivateAbilityInfo Request)
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
        TAbstractUnit U;
        string Name = "";
        TConstructAbility Ability;
        TConstructionControl Control;
        Dlg.FDrawn = false;
        U = AoWHSMap.FUnitControl.FindUnit(FRequest.FUnitID);
        ConstructionList.Items.Clear();
        ConstructionCostList.Items.Clear();
        ConstructionTurnsList.Items.Clear();
        Ability = (TConstructAbility)AoWHSSet.FAbilityControl.GetAbility(125);
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
        General.PlayButtonSound(0);
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
    public void ConstructBtnClick(TObject Sender)
    {
        string Reason = "";
        TConstructionControl Control;
        TAbstractUnit U;
        General.PlayButtonSound(0);
        bool condition = ConstructionList.Selected >= 0;
        if (condition)
        {
            var count = ConstructionList.Items.Count;
            condition = ConstructionList.Selected < count;
        }

        if (condition)
        {
            U = AoWHSMap.FUnitControl.FindUnit(FRequest.FUnitID);
            Control = (TConstructionControl)ConstructionList.Items.Objects[ConstructionList.Selected];
            if (Control.CanConstruct(U, ref Reason))
            {
                if (Control.Construct(U))
                {
                    Dlg.Visible = false;
                }
            }
            else
            {
                AoWHSMap.ShowWarning(Reason);
            }
        }
    }

    // AoWConstructDlg.pas:96
    public void SortConstructionsBtnClick()
    {
        General.PlayButtonSound(0);
    }

    // AoWConstructDlg.pas:99
    public void SortConstructionCostBtnClick()
    {
        General.PlayButtonSound(0);
    }

    // AoWConstructDlg.pas:102
    public void SortConstructionTurnsBtnClick()
    {
        General.PlayButtonSound(0);
    }

    // AoWConstructDlg.pas:105
    public void DlgHide()
    {
        Finish();
    }

    // AoWConstructDlg.pas:169
    public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }
}

public static partial class AoWConstructDlg
{
}
