#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World;

using Aow.Game.World.Structures;

namespace Aow.Game.Magic.Abilities;

// (construction controls)
public class ConstructAbility : AdjustableAbility
{
    // AoWAb.PAS:5537
    public ConstructAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_constructText);
        AbilityId = 125;
        AbilityType = AbilityType.Enhancement;
        SelectionTypes = AbilitySelectionTypes.Unit;
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
    public override AbilityControlTypes GetControlType(AbilityOwner owner) => AbilityControlTypes.Action;

    // AoWAb.PAS:5555
    public override bool CanActivate(AbilityOwner owner, ref string reason) => owner is Troop;

    // AoWAb.PAS:5560
    public void RegisterConstructionControl(ConstructionControl control)
    {
        // { Pass the control ID directly without retaining a local. }
        int id = control.Id();
        if (ConstructionControls.FindId(id) is not null)
        {
            throw new InvalidOperationException("Construction Control ID already used");
        }

        control.SetOwner(ConstructionControls);
    }

    private static readonly string s_constructText = "Construct";
}
