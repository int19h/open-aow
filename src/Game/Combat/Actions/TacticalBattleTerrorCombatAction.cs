// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// source ID, target ID, casting mana, success
public class TacticalBattleTerrorCombatAction(CustomNode owner) : CombatAction(owner)
{
    // CombatSpells.pas:1707
    public new const int ClassId = 131785;
    private byte _sourceId;
    private byte _targetId;

    public int ManaCost { get; set; }

    private bool _succeeded;

    // CombatSpells.pas:1713
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _sourceId = persister.Byte("SourceId", value: _sourceId);
        _targetId = persister.Byte("TargetId", value: _targetId);
        ManaCost = persister.Int32("ManaCost", value: ManaCost);
        _succeeded = persister.Boolean("Succeeded", value: _succeeded);
    }

    // CombatSpells.pas:1723
    public override void Execute(Battle combat)
    {
        if (_succeeded)
        {
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(_targetId);
            _ = GamePack.Current.AbilityControl[108].Expand(target.Troop);
        }

        if (ManaCost != 0 && combat.Status.Contains(CombatStatusElement.Executing))
        {
            CombatUnit source = combat.CombatData.FindId<CombatUnit>(_sourceId);
            GamePack.Current.SpellControl[105].CastingDone(source.Troop, ManaCost);
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
        if (!target.GetAbilityEnabled(108) && !target.GetAbilityEnabled(67) && target is CombatUnit targetUnit && !target.GetImmunityTypes().Contains(DamageType.Death) && targetUnit.Troop.GetUnitType() != UnitType.Machine)
        {
            _succeeded = HitChance.HitRole(6 - target.GetResistance());
        }
    }
}
