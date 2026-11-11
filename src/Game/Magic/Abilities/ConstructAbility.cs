// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// (construction controls)
public partial class ConstructAbility : AdjustableAbility
{
    // AoWAb.PAS:5537
    public ConstructAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(P4ConstructRStr);
        FAbilityID = 125;
        FAbilityType = AbilityType.Enhancement;
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
        FConstructionControls = new ConstructionControlList(null);
        RegisterConstructionControl(new RebuildConstructionControl(null));
        RegisterConstructionControl(new BuildRoadsControl(null));
    }

    public ConstructionControlList FConstructionControls;

    // AoWAb.PAS:5548
    public override void Dispose()
    {
        FConstructionControls?.Dispose();
        base.Dispose();
    }

    // AoWAb.PAS:5553
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner Owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action);

    // AoWAb.PAS:5555
    public override bool CanActivate(AbilityOwner Owner, ref string Reason)
    {
        bool Result;
        if (Owner is Troop)
        {
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWAb.PAS:5560
    public void RegisterConstructionControl(ConstructionControl Control)
    {
        // { Pass the control ID directly without retaining a local. }
        var id = Control.ID();
        if (FConstructionControls.FindID(id) != null)
        {
            throw new InvalidOperationException("Construction Control ID already used");
        }

        Control.SetOwner(FConstructionControls);
    }

    private static readonly string P4ConstructRStr = "Construct";
}
