// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// inherited source ID, target ID, mana, success, spell ID
public partial class RecallSpiritCA : ExclusiveCombatSpellCA
{
    // CombatSpells.pas:1950
    public new const int ClassID = 131781;

    public RecallSpiritCA(ECustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:1953
    public override void Execute(Battle Combat)
    {
        CombatUnit Source;
        CombatUnit Target;
        Unit NewUnit;
        base.Execute(Combat);
        if (FSucceeded)
        {
            Source = (CombatUnit)Combat.FCombatData.FindID(FSourceID);
            Target = (CombatUnit)Combat.FCombatData.FindID(FTargetID);
            NewUnit = new Unit(null);
            NewUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(130));
            NewUnit.SetPlayer(Source.FUnit.FPlayer);
            NewUnit.ExpandAbility(130);
            NewUnit.Activate();
            Target.SetUnit(NewUnit);
            NewUnit.Release();
            Target.SetStatus(Target.FStatus - Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed));
            Target.SetCombatPlayer((byte)Source.FCombatPlayer);
        }
    }

}
