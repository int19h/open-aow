// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// FDamage. FDamageTypes. FEffects. FDamageTaken. The two damage sets are stored as unaligned
// packed records (PackedDamageTypes)
public partial class DamageCombatAction(CustomNode owner) : SingleTargetCombatAction(owner)
{
    // Combat2.pas:774
    public new const int ClassId = 131761;

    public byte Damage { get; set; }
    public Set<DamageType> DamageTypes { get; set; }
    public Set<DamageType> Effects { get; set; }

    public byte DamageDone { get; protected set; }

    // Combat2.pas:777
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Damage = persister.Byte("Damage", value: Damage);
        Effects = PersistSupport.Record<PackedDamageTypes, Set<DamageType>>(persister, name: "Effects", value: Effects);
        DamageTypes = PersistSupport.Record<PackedDamageTypes, Set<DamageType>>(persister, name: "DamageTypes", value: DamageTypes);
    }

    // Combat2.pas:817
    public override void Execute(Battle combat)
    {
        CombatObject source = combat.CombatData.FindId(SourceId);
        CombatObject target = combat.CombatData.FindId(TargetId);
        if (Damage != 0)
        {
            DamageDone = (byte)source.DoDamage(target, Damage);
        }

        if (Effects != Set<DamageType>.Of())
        {
            _ = target.ExecuteDamageEffects(Effects);
        }
    }

    // Combat2.pas:826
    public override bool Play(CombatViewer viewer)
    {
        int hitFrame = viewer.Delay < 5 ? viewer.Delay : 5;

        if (hitFrame == viewer.Time)
        {
            Execute(viewer.Combat);
            CombatObject target = viewer.Combat.CombatData.FindId(TargetId);
            if (DamageDone != 0)
            {
                if (!target.Status.Contains(CombatObjectStatusElement.Destroyed))
                {
                    target.PlaySfx(2);
                }
                else
                {
                    target.PlaySfx(3);
                }
            }
        }

        int duration = viewer.Delay * 2;
        if (Effects != Set<DamageType>.Of())
        {
            int effectFrames = DamageEffectsAnimationFrames(Effects) + viewer.Delay;
            hitFrame = viewer.Delay * 3;
            if (effectFrames > hitFrame)
            {
                effectFrames = hitFrame;
            }

            if (duration < effectFrames)
            {
                duration = effectFrames;
            }
        }

        return duration > viewer.Time;
    }

    // Combat2.pas:848
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        if (objectValue.Id == SourceId)
        {
            _ = objectValue.ShowAttackAnimation(viewer.Time, direction, X, Y, surface);
        }
        else
        {
            int hitFrame = viewer.Delay < 5 ? viewer.Delay : 5;

            if (objectValue.Id == TargetId && DamageDone != 0 && hitFrame <= viewer.Time)
            {
                objectValue.Show(frame, direction, X, Y, surface);
                if (Effects != Set<DamageType>.Of())
                {
                    _ = ShowDamageEffects(Effects, X, Y, viewer.Time - hitFrame, surface);
                }
                AbilityRules.ShowDamageIcon(damageType: DamageTypesToDamageType(DamageTypes), engine: GetEngine(), x: X, y: Y, surface: surface, damage: DamageDone);
            }
            else
            {
                objectValue.Show(frame, direction, X, Y, surface);
            }
        }
    }

    // Combat2.pas:785
    public virtual void Setup(byte sourceId, byte targetId, byte damage, Set<DamageType> effects, Set<DamageType> damageTypes, bool defensive)
    {
        SourceId = sourceId;
        TargetId = targetId;
        Damage = damage;
        Effects = effects;
        DamageTypes = damageTypes;
        if (defensive)
        {
            Mode += Set<SingleTargetModeElement>.Of(SingleTargetModeElement.Defensive);
        }
    }

    // Combat2.pas:795
    public virtual void Generate(CombatObject source, CombatObject target, sbyte attack, sbyte damage, Set<DamageType> damageTypes)
    {
        SourceId = source.Id;
        TargetId = target.Id;
        Damage = (byte)target.ExecuteDamageRole(attack, damage, damageTypes);
        if (Damage != 0)
        {
            Set<DamageType> executeDamageEffectsRole = target.ExecuteDamageEffectsRole(damageTypes);
            Effects = executeDamageEffectsRole;
        }
        else
        {
            Effects = Set<DamageType>.Of();
        }

        Set<DamageType> immunityTypes = target.GetImmunityTypes();
        DamageTypes = damageTypes - immunityTypes;
    }

    // Combat2.pas:806
    public virtual void Generate(CombatObject source, CombatObject target, sbyte attack, sbyte damage, Set<DamageType> damageTypes, byte damageMode)
    {
        SourceId = source.Id;
        TargetId = target.Id;
        Damage = (byte)target.ExecuteDamageRole(attack, damage, damageTypes, damageMode);
        if (Damage != 0)
        {
            Set<DamageType> executeDamageEffectsRole = target.ExecuteDamageEffectsRole(damageTypes);
            Effects = executeDamageEffectsRole;
        }
        else
        {
            Effects = Set<DamageType>.Of();
        }

        Set<DamageType> immunityTypes = target.GetImmunityTypes();
        DamageTypes = damageTypes - immunityTypes;
    }

    private static bool ShowDamageEffects(Set<DamageType> damageTypes, int X, int Y, int frame, in LockedSurface surface)
    {
        bool flag = false;
        if (damageTypes.Contains(DamageType.Fire))
        {
            flag = flag || GamePack.Current.AbilityControl.GetAbility(127).ImageSequences.Show(0, X + 32, Y + 32, frame, in surface);
        }
        if (damageTypes.Contains(DamageType.Poison))
        {
            flag = flag || GamePack.Current.AbilityControl.GetAbility(96).ImageSequences.Show(0, X + 32, Y + 32, frame, in surface);
        }
        if (damageTypes.Contains(DamageType.Cold))
        {
            flag = flag || GamePack.Current.AbilityControl.GetAbility(95).ImageSequences.Show(1, X + 32, Y + 32, frame, in surface);
        }
        if (damageTypes.Contains(DamageType.Death))
        {
            flag = flag || GamePack.Current.AbilityControl.GetAbility(93).ImageSequences.Show(0, X + 32, Y + 32, frame, in surface);
        }
        if (damageTypes.Contains(DamageType.Holy))
        {
            flag = flag || GamePack.Current.AbilityControl.GetAbility(98).ImageSequences.Show(0, X + 32, Y + 32, frame, in surface);
        }
        return flag;
    }

    private static int DamageEffectsAnimationFrames(Set<DamageType> damageTypes)
    {
        int number = 0;
        if (damageTypes.Contains(DamageType.Fire))
        {
            int framesCount = GamePack.Current.AbilityControl.GetAbility(127).ImageSequences.GetFramesCount(0);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        if (damageTypes.Contains(DamageType.Poison))
        {
            int framesCount = GamePack.Current.AbilityControl.GetAbility(96).ImageSequences.GetFramesCount(0);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        if (damageTypes.Contains(DamageType.Cold))
        {
            int framesCount = GamePack.Current.AbilityControl.GetAbility(95).ImageSequences.GetFramesCount(1);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        if (damageTypes.Contains(DamageType.Death))
        {
            int framesCount = GamePack.Current.AbilityControl.GetAbility(93).ImageSequences.GetFramesCount(0);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        if (damageTypes.Contains(DamageType.Holy))
        {
            int framesCount = GamePack.Current.AbilityControl.GetAbility(98).ImageSequences.GetFramesCount(0);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        return number;
    }

    private static DamageType DamageTypesToDamageType(Set<DamageType> damageTypes)
    {
        return damageTypes != Set<DamageType>.Of([])
            ? damageTypes.Contains(DamageType.Holy)
                ? DamageType.Holy
                : damageTypes.Contains(DamageType.Magic)
                ? DamageType.Magic
                : damageTypes.Contains(DamageType.Death)
                ? DamageType.Death
                : damageTypes.Contains(DamageType.Cold)
                ? DamageType.Cold
                : damageTypes.Contains(DamageType.Fire)
                ? DamageType.Fire
                : damageTypes.Contains(DamageType.Poison)
                ? DamageType.Poison
                : damageTypes.Contains(DamageType.Physical)
                ? DamageType.Physical
                : damageTypes.Contains(DamageType.Wall) ? DamageType.Wall : DamageType.None
            : DamageType.None;
    }

    // intf_P3_02.inc:38

    // intf_P3_02.inc:38

}
