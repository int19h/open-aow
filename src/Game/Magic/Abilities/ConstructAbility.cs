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
    public ConstructAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_constructText);
        AbilityId = 125;
        AbilityType = AbilityType.Enhancement;
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
        ConstructionControls = new ConstructionControlList(null) { Holder = CreationHolder() };
        RegisterConstructionControl(new RebuildConstructionControl(null) { Holder = CreationHolder() });
        RegisterConstructionControl(new BuildRoadsControl(null) { Holder = CreationHolder() });
    }

    public ConstructionControlList ConstructionControls { get; }

    // AoWAb.PAS:5548
    public override void Dispose()
    {
        ConstructionControls?.Dispose();
        base.Dispose();
    }

    // AoWAb.PAS:5553
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action);

    // AoWAb.PAS:5555
    public override bool CanActivate(AbilityOwner owner, ref string reason)
    {
        bool result;
        if (owner is Troop)
        {
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWAb.PAS:5560
    public void RegisterConstructionControl(ConstructionControl control)
    {
        // { Pass the control ID directly without retaining a local. }
        var id = control.Id();
        if (ConstructionControls.FindId(id) != null)
        {
            throw new InvalidOperationException("Construction Control ID already used");
        }

        control.SetOwner(ConstructionControls);
    }

    private static readonly string s_constructText = "Construct";
}
