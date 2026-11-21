#nullable disable
// Translated from AoWConstructDlg.pas.
using System.Globalization;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// activation request
public sealed partial class ConstructDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public ListBox<ConstructionControl> ConstructionList { get; private set; }
    public ListBox ConstructionCostList { get; private set; }
    public ListBox ConstructionTurnsList { get; private set; }

    private AbilityActivation _request;

    // AoWConstructDlg.pas:117
    public void Execute(AbilityActivation request)
    {
        _ = (_request?.Release());

        _request = request;
        _ = _request.AddRef();
        UpdateConstructions();
        Window.Visible = true;
    }

    // AoWConstructDlg.pas:108
    public void Finish()
    {
        _ = (_request?.Release());
        _request = null;
    }

    // AoWConstructDlg.pas:142
    public void UpdateConstructions()
    {
        Window.Drawn = false;
        Troop u = StrategicMap.Current.UnitControl.FindUnit(_request.UnitId);
        ConstructionList.Items.Clear();
        ConstructionCostList.Items.Clear();
        ConstructionTurnsList.Items.Clear();
        ConstructionControlList controls = GamePack.Current.AbilityControl.GetAbility<ConstructAbility>(125).ConstructionControls;
        for (int i = 0, last = controls.Count - 1; i <= last; i++)
        {
            ConstructionControl control = controls.GetConstructionControl(i);
            control.GetInfo(u, text: out string name, out int gold, out int days);
            _ = ConstructionList.Items.AddObject(name, item: control);
            _ = gold != 0 ? ConstructionCostList.Items.Add(gold.ToString(CultureInfo.InvariantCulture)) : ConstructionCostList.Items.Add("-");

            _ = days != 0 ? ConstructionTurnsList.Items.Add(days.ToString(CultureInfo.InvariantCulture)) : ConstructionTurnsList.Items.Add("-");
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
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWConstructDlg.pas:68
    public void ConstructionListChange() => SelectConstruction(ConstructionList.Selected);

    // AoWConstructDlg.pas:71
    public void ConstructionCostListChange() => SelectConstruction(ConstructionCostList.Selected);

    // AoWConstructDlg.pas:74
    public void ConstructionTurnsListChange() => SelectConstruction(ConstructionTurnsList.Selected);

    // AoWConstructDlg.pas:77
    public void ConstructBtnClick()
    {
        UI.General.PlayButtonSound(0);
        bool condition = ConstructionList.Selected >= 0;
        if (condition)
        {
            int count = ConstructionList.Items.Count;
            condition = ConstructionList.Selected < count;
        }

        if (condition)
        {
            Troop u = StrategicMap.Current.UnitControl.FindUnit(_request.UnitId);
            ConstructionControl control = ConstructionList.Items.Objects[ConstructionList.Selected];
            string reason = "";
            if (control.CanConstruct(u, ref reason))
            {
                if (control.Construct(u))
                {
                    Window.Visible = false;
                }
            }
            else
            {
                StrategicMap.Current.ShowWarning(reason);
            }
        }
    }

    // AoWConstructDlg.pas:96
    public void SortConstructionsBtnClick() => UI.General.PlayButtonSound(0);

    // AoWConstructDlg.pas:99
    public void SortConstructionCostBtnClick() => UI.General.PlayButtonSound(0);

    // AoWConstructDlg.pas:102
    public void SortConstructionTurnsBtnClick() => UI.General.PlayButtonSound(0);

    // AoWConstructDlg.pas:105
    public void DlgHide() => Finish();

    // AoWConstructDlg.pas:169
    public void DlgKeyDown(in ushort key, ShiftStates shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }
}
