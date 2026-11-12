// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// source ID, target ID, casting mana, success
public partial class TacticalBattleTerrorCombatAction : CombatAction
{
    // CombatSpells.pas:1707
    public new const int ClassId = 131785;

    public TacticalBattleTerrorCombatAction(ECustomNode owner) : base(owner)
    { }

    public byte SourceId;
    public byte TargetId;
    public int ManaCost;
    public bool Succeeded;

    // CombatSpells.pas:1713
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref SourceId, 20);
        stream.rwByte(ref TargetId, 21);
        stream.rwInteger(ref ManaCost, 22);
        stream.rwBool(ref Succeeded, 23);
    }

    // CombatSpells.pas:1723
    public override void Execute(Battle combat)
    {
        CombatUnit target;
        CombatUnit source;
        if (Succeeded)
        {
            target = (CombatUnit)combat.CombatData.FindId(TargetId);
            AoWHSSet.Current.AbilityControl.GetAbility(108).Expand(target.Troop);
        }

        if (ManaCost != 0 && combat.Status.Contains(CombatStatusElement.Executing))
        {
            source = (CombatUnit)combat.CombatData.FindId(SourceId);
            AoWHSSet.Current.SpellControl.GetSpell(105).CastingDone(source.Troop, ManaCost);
        }
    }

    // CombatSpells.pas:1740
    public override bool Play(CombatViewer viewer)
    {
        Execute(viewer.Combat);
        return false;
    }

    // CombatSpells.pas:1747
    public void Generate(CombatObject source, CombatObject target)
    {
        Succeeded = false;
        SourceId = source.Id;
        TargetId = target.Id;
        if (!target.GetAbilityEnabled(108) && !target.GetAbilityEnabled(67) && target is CombatUnit && !target.GetImmunityTypes().Contains(DamageType.Death) && ((CombatUnit)target).Troop.GetUnitType() != UnitType.Machine)
        {
            Succeeded = HitChance.HitRole(6 - target.GetResistance());
        }
    }

}
