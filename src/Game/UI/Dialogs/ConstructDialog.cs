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

    public Window Window;
    public ListBox<ConstructionControl> ConstructionList;
    public ListBox ConstructionCostList;
    public ListBox ConstructionTurnsList;
    private AbilityActivation _request;

    // AoWConstructDlg.pas:117
    public void Execute(AbilityActivation request)
    {
        if (_request != null)
        {
            _request.Release();
        }

        _request = request;
        _request.AddRef();
        UpdateConstructions();
        Window.Visible = true;
    }

    // AoWConstructDlg.pas:108
    public void Finish()
    {
        if (_request != null)
        {
            _request.Release();
            _request = null;
        }
    }

    // AoWConstructDlg.pas:142
    public void UpdateConstructions()
    {
        int days = default;
        int gold = default;
        Troop U;
        string name = "";
        ConstructAbility ability;
        ConstructionControl control;
        Window.Drawn = false;
        U = AoWHSMap.Current.UnitControl.FindUnit(_request.UnitId);
        ConstructionList.Items.Clear();
        ConstructionCostList.Items.Clear();
        ConstructionTurnsList.Items.Clear();
        ability = (ConstructAbility)AoWHSSet.Current.AbilityControl.GetAbility(125);
        for (int I = 0, last = ability.ConstructionControls.Count - 1; I <= last; I++)
        {
            control = ability.ConstructionControls.GetConstructionControl(I);
            control.GetInfo(U, ref name, ref gold, ref days);
            ConstructionList.Items.AddObject(name, control);
            if (gold != 0)
            {
                ConstructionCostList.Items.Add(IntToStr(gold));
            }
            else
            {
                ConstructionCostList.Items.Add("-");
            }

            if (days != 0)
            {
                ConstructionTurnsList.Items.Add(IntToStr(days));
            }
            else
            {
                ConstructionTurnsList.Items.Add("-");
            }
        }
    }

    // AoWConstructDlg.pas:126
    public void SelectConstruction(int index)
    {
        if (index >= 0 && index < ConstructionList.Items.Count)
        {
            ConstructionList.Selected = index;
            ConstructionCostList.Selected = index;
            ConstructionTurnsList.Selected = index;
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
        Window.Visible = false;
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
        string reason = "";
        ConstructionControl control;
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
            U = AoWHSMap.Current.UnitControl.FindUnit(_request.UnitId);
            control = ConstructionList.Items.Objects[ConstructionList.Selected];
            if (control.CanConstruct(U, ref reason))
            {
                if (control.Construct(U))
                {
                    Window.Visible = false;
                }
            }
            else
            {
                AoWHSMap.Current.ShowWarning(reason);
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
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }
}
