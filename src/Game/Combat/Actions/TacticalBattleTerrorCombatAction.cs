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
    public new const int ClassID = 131785;

    public TacticalBattleTerrorCombatAction(ECustomNode owner) : base(owner)
    { }

    public byte FSourceID;
    public byte FTargetID;
    public int FManaCost;
    public bool FSucceeded;

    // CombatSpells.pas:1713
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FSourceID, 20);
        Stream.rwByte(ref FTargetID, 21);
        Stream.rwInteger(ref FManaCost, 22);
        Stream.rwBool(ref FSucceeded, 23);
    }

    // CombatSpells.pas:1723
    public override void Execute(Battle Combat)
    {
        CombatUnit Target;
        CombatUnit Source;
        if (FSucceeded)
        {
            Target = (CombatUnit)Combat.FCombatData.FindID(FTargetID);
            AoWHSSet.Current.FAbilityControl.GetAbility(108).Expand(Target.FUnit);
        }

        if (FManaCost != 0 && Combat.FStatus.Contains(CombatStatusElement.Executing))
        {
            Source = (CombatUnit)Combat.FCombatData.FindID(FSourceID);
            AoWHSSet.Current.FSpellControl.GetSpell(105).CastingDone(Source.FUnit, FManaCost);
        }
    }

    // CombatSpells.pas:1740
    public override bool Play(CombatViewer Viewer)
    {
        Execute(Viewer.FCombat);
        return false;
    }

    // CombatSpells.pas:1747
    public void Generate(CombatObject Source, CombatObject Target)
    {
        FSucceeded = false;
        FSourceID = Source.FID;
        FTargetID = Target.FID;
        if (!Target.GetAbilityEnabled(108) && !Target.GetAbilityEnabled(67) && Target is CombatUnit && !Target.GetImmunityTypes().Contains(DamageType.Death) && ((CombatUnit)Target).FUnit.GetUnitType() != UnitType.Machine)
        {
            FSucceeded = HitChance.HitRole(6 - Target.GetResistance());
        }
    }

}
