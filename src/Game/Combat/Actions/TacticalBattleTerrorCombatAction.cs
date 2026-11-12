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

    private byte _sourceId;
    private byte _targetId;
    public int ManaCost;
    private bool _succeeded;

    // CombatSpells.pas:1713
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref _sourceId, 20);
        stream.rwByte(ref _targetId, 21);
        stream.rwInteger(ref ManaCost, 22);
        stream.rwBool(ref _succeeded, 23);
    }

    // CombatSpells.pas:1723
    public override void Execute(Battle combat)
    {
        CombatUnit target;
        CombatUnit source;
        if (_succeeded)
        {
            target = (CombatUnit)combat.CombatData.FindId(_targetId);
            AoWHSSet.Current.AbilityControl.GetAbility(108).Expand(target.Troop);
        }

        if (ManaCost != 0 && combat.Status.Contains(CombatStatusElement.Executing))
        {
            source = (CombatUnit)combat.CombatData.FindId(_sourceId);
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
        _succeeded = false;
        _sourceId = source.Id;
        _targetId = target.Id;
        if (!target.GetAbilityEnabled(108) && !target.GetAbilityEnabled(67) && target is CombatUnit && !target.GetImmunityTypes().Contains(DamageType.Death) && ((CombatUnit)target).Troop.GetUnitType() != UnitType.Machine)
        {
            _succeeded = HitChance.HitRole(6 - target.GetResistance());
        }
    }

}
