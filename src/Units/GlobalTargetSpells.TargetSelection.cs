using Aow.Rtl;
using Aow.Units.AoWE;

namespace Aow.Units.GlobalTargetSpells;

public partial class TGlobalTargetSpell
{
    // GlobalTargetSpells.pas:492. The recovered view aliases this control.
    public virtual void OnTargetSelectionChange(TObject sender)
    {
        var target = (TTargetSelectionControl)sender;
        var caster = AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FCasterID);
        if (caster is null || target.FChange == TTargetSelectionChange.tscDeactivated)
        {
            target.FOnChange.Remove(OnTargetSelectionChange);
            target.Deactivate();
        }
        else if (target.FChange == TTargetSelectionChange.tscSelected && ValidTargetSelection(target) &&
            TargetSelected(caster, target.FTargetX, target.FTargetY, target.FTargetL))
        {
            target.FOnChange.Remove(OnTargetSelectionChange);
            target.Deactivate();
        }
    }
}
