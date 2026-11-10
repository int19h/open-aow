// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class FastCombatHighPrayerCA : MultiTargetCombatSpellCA
{
    // CombatSpells.pas:937
    public new const int ClassID = 131783;

    public FastCombatHighPrayerCA(ECustomNode owner) : base(owner)
    { }

    // CombatSpells.pas:943
    public override void Execute(Battle Combat)
    {
        CombatObject Target;
        base.Execute(Combat);
        for (int I = FTargetIDs.Count - 1; I >= 0; I--)
        {
            var targetID = FTargetIDs[I];
            Target = Combat.FCombatData.FindID(targetID);
            Target.SetHitPoints((byte)(Target.GetHitPoints() + 5));
            AoWHSSet.Current.FAbilityControl.GetAbility(100).Expand(((CombatUnit)Target).FUnit);
        }
    }

    // CombatSpells.pas:958
    public void Generate(CombatObject Source)
    {
        CombatSide Side;
        CombatObject Target;
        FSpellID = 102;
        FSourceID = Source.FID;
        var combat = Source.FCombat;
        var side = Source.GetSide();
        Side = combat.FSides[(int)side];

        for (int I = Side.GetCount() - 1; I >= 0; I--)
        {
            Target = Side.GetObjects(I);
            if (Target is CombatUnit && !Target.FStatus.Contains(CombatObjectStatusElement.Destroyed))
            {
                FTargetIDs.Add(Target.FID);
            }
        }
    }

}
