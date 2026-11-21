// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// FDamage. FDamageTypes. FEffects. FDamageTaken. The two damage sets are stored as unaligned
// packed records (PackedDamageTypes)
public class DamageCombatAction(CustomNode owner) : SingleTargetCombatAction(owner)
{
    // Combat2.pas:774
    public new const int ClassId = 131_761;

    public byte Damage { get; set; }
    public DamageTypes DamageTypes { get; set; }
    public DamageTypes Effects { get; set; }

    public byte DamageDone { get; protected set; }

    // Combat2.pas:777
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Damage = persister.Byte("Damage", value: Damage);
        Effects = persister.Record<PackedDamageTypes, DamageTypes>(name: "Effects", value: Effects);
        DamageTypes = persister.Record<PackedDamageTypes, DamageTypes>(name: "DamageTypes", value: DamageTypes);
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

        if (Effects != DamageTypes.None)
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
                if (!target.Status.Contains(CombatObjectStatuses.Destroyed))
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
        if (Effects != DamageTypes.None)
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
                if (Effects != DamageTypes.None)
                {
                    _ = ShowDamageEffects(Effects, X, Y, frame: viewer.Time - hitFrame, surface);
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
    public virtual void Setup(byte sourceId, byte targetId, byte damage, DamageTypes effects, DamageTypes damageTypes, bool defensive)
    {
        SourceId = sourceId;
        TargetId = targetId;
        Damage = damage;
        Effects = effects;
        DamageTypes = damageTypes;
        if (defensive)
        {
            Mode |= SingleTargetModes.Defensive;
        }
    }

    // Combat2.pas:795
    public virtual void Generate(CombatObject source, CombatObject target, sbyte attack, sbyte damage, DamageTypes damageTypes)
    {
        SourceId = source.Id;
        TargetId = target.Id;
        Damage = (byte)target.ExecuteDamageRole(attack, damage, damageTypes);
        if (Damage != 0)
        {
            DamageTypes executeDamageEffectsRole = target.ExecuteDamageEffectsRole(damageTypes);
            Effects = executeDamageEffectsRole;
        }
        else
        {
            Effects = DamageTypes.None;
        }

        DamageTypes immunityTypes = target.GetImmunityTypes();
        DamageTypes = damageTypes & ~immunityTypes;
    }

    // Combat2.pas:806
    public virtual void Generate(CombatObject source, CombatObject target, sbyte attack, sbyte damage, DamageTypes damageTypes, byte damageMode)
    {
        SourceId = source.Id;
        TargetId = target.Id;
        Damage = (byte)target.ExecuteDamageRole(attack, damage, damageTypes, damageMode);
        if (Damage != 0)
        {
            DamageTypes executeDamageEffectsRole = target.ExecuteDamageEffectsRole(damageTypes);
            Effects = executeDamageEffectsRole;
        }
        else
        {
            Effects = DamageTypes.None;
        }

        DamageTypes immunityTypes = target.GetImmunityTypes();
        DamageTypes = damageTypes & ~immunityTypes;
    }

    private static bool ShowDamageEffects(DamageTypes damageTypes, int X, int Y, int frame, in LockedSurface surface)
    {
        bool flag = false;
        if (damageTypes.Contains(DamageTypes.Fire))
        {
            flag = flag || GamePack.Current.AbilityControl[127].ImageSequences.Show(0, X + 32, Y + 32, frame, lockedSurface: in surface);
        }
        if (damageTypes.Contains(DamageTypes.Poison))
        {
            flag = flag || GamePack.Current.AbilityControl[96].ImageSequences.Show(0, X + 32, Y + 32, frame, lockedSurface: in surface);
        }
        if (damageTypes.Contains(DamageTypes.Cold))
        {
            flag = flag || GamePack.Current.AbilityControl[95].ImageSequences.Show(1, X + 32, Y + 32, frame, lockedSurface: in surface);
        }
        if (damageTypes.Contains(DamageTypes.Death))
        {
            flag = flag || GamePack.Current.AbilityControl[93].ImageSequences.Show(0, X + 32, Y + 32, frame, lockedSurface: in surface);
        }
        if (damageTypes.Contains(DamageTypes.Holy))
        {
            flag = flag || GamePack.Current.AbilityControl[98].ImageSequences.Show(0, X + 32, Y + 32, frame, lockedSurface: in surface);
        }
        return flag;
    }

    private static int DamageEffectsAnimationFrames(DamageTypes damageTypes)
    {
        int number = 0;
        if (damageTypes.Contains(DamageTypes.Fire))
        {
            int framesCount = GamePack.Current.AbilityControl[127].ImageSequences.GetFramesCount(0);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        if (damageTypes.Contains(DamageTypes.Poison))
        {
            int framesCount = GamePack.Current.AbilityControl[96].ImageSequences.GetFramesCount(0);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        if (damageTypes.Contains(DamageTypes.Cold))
        {
            int framesCount = GamePack.Current.AbilityControl[95].ImageSequences.GetFramesCount(1);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        if (damageTypes.Contains(DamageTypes.Death))
        {
            int framesCount = GamePack.Current.AbilityControl[93].ImageSequences.GetFramesCount(0);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        if (damageTypes.Contains(DamageTypes.Holy))
        {
            int framesCount = GamePack.Current.AbilityControl[98].ImageSequences.GetFramesCount(0);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        return number;
    }

    private static DamageType DamageTypesToDamageType(DamageTypes damageTypes)
    {
        return damageTypes != DamageTypes.None
            ? damageTypes.Contains(DamageTypes.Holy)
                ? DamageType.Holy
                : damageTypes.Contains(DamageTypes.Magic)
                ? DamageType.Magic
                : damageTypes.Contains(DamageTypes.Death)
                ? DamageType.Death
                : damageTypes.Contains(DamageTypes.Cold)
                ? DamageType.Cold
                : damageTypes.Contains(DamageTypes.Fire)
                ? DamageType.Fire
                : damageTypes.Contains(DamageTypes.Poison)
                ? DamageType.Poison
                : damageTypes.Contains(DamageTypes.Physical)
                ? DamageType.Physical
                : damageTypes.Contains(DamageTypes.Wall) ? DamageType.Wall : DamageType.NonSpecific
            : DamageType.NonSpecific;
    }
}
