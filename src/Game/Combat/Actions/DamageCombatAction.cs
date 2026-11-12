// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// FDamage. FDamageTypes. FEffects. FDamageTaken. The two damage sets are deliberately unaligned
// packed fields
public partial class DamageCombatAction : SingleTargetCombatAction
{
    // Combat2.pas:774
    public new const int ClassId = 131761;

    public DamageCombatAction(ECustomNode owner) : base(owner)
    {
    }

    public byte Damage;
    public PackedDamageTypes DamageTypes;
    public PackedDamageTypes Effects;

    public byte DamageDone { get; protected set; }

    // Combat2.pas:777
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Damage, 15);
        stream.rwData(Bytes.Of(ref Effects, 2), 2, 16);
        stream.rwData(Bytes.Of(ref DamageTypes, 2), 2, 17);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Byte("Damage", ref Damage);
        PersistSupport.Record(persister, "Effects", ref Effects);
        PersistSupport.Record(persister, "DamageTypes", ref DamageTypes);
    }

    // Combat2.pas:817
    public override void Execute(Battle combat)
    {
        CombatObject target;
        CombatObject source;
        source = combat.CombatData.FindId(SourceId);
        target = combat.CombatData.FindId(TargetId);
        if (Damage != 0)
        {
            DamageDone = (byte)source.DoDamage(target, Damage);
        }

        if (Effects.Value != Set<DamageType>.Of())
        {
            target.ExecuteDamageEffects(Effects.Value);
        }
    }

    // Combat2.pas:826
    public override bool Play(CombatViewer viewer)
    {
        int hitFrame;
        int duration;
        int effectFrames;
        CombatObject target;
        if (viewer.Delay < 5)
        {
            hitFrame = viewer.Delay;
        }
        else
        {
            hitFrame = 5;
        }

        if (hitFrame == viewer.Time)
        {
            Execute(viewer.Combat);
            target = viewer.Combat.CombatData.FindId(TargetId);
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

        duration = viewer.Delay * 2;
        if (Effects.Value != Set<DamageType>.Of())
        {
            effectFrames = DamageEffectsAnimationFrames(Effects.Value) + viewer.Delay;
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
        int hitFrame;
        if (objectValue.Id == SourceId)
        {
            objectValue.ShowAttackAnimation(viewer.Time, direction, X, Y, surface);
        }
        else
        {
            if (viewer.Delay < 5)
            {
                hitFrame = viewer.Delay;
            }
            else
            {
                hitFrame = 5;
            }

            if (objectValue.Id == TargetId && DamageDone != 0 && hitFrame <= viewer.Time)
            {
                objectValue.Show(frame, direction, X, Y, surface);
                if (Effects.Value != Set<DamageType>.Of())
                {
                    ShowDamageEffects(Effects.Value, X, Y, viewer.Time - hitFrame, surface);
                }

                var damageTypesToDamageType = DamageTypesToDamageType(DamageTypes.Value);
                AbilityRules.ShowDamageIcon(X, Y, surface, DamageDone, damageTypesToDamageType);
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
        Effects.Value = effects;
        DamageTypes.Value = damageTypes;
        if (defensive)
        {
            Mode = Mode + Set<SingleTargetModeElement>.Of(SingleTargetModeElement.Defensive);
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
            var executeDamageEffectsRole = target.ExecuteDamageEffectsRole(damageTypes);
            Effects.Value = executeDamageEffectsRole;
        }
        else
        {
            Effects.Value = Set<DamageType>.Of();
        }

        var immunityTypes = target.GetImmunityTypes();
        DamageTypes.Value = damageTypes - immunityTypes;
    }

    // Combat2.pas:806
    public virtual void Generate(CombatObject source, CombatObject target, sbyte attack, sbyte damage, Set<DamageType> damageTypes, byte damageMode)
    {
        SourceId = source.Id;
        TargetId = target.Id;
        Damage = (byte)target.ExecuteDamageRole(attack, damage, damageTypes, damageMode);
        if (Damage != 0)
        {
            var executeDamageEffectsRole = target.ExecuteDamageEffectsRole(damageTypes);
            Effects.Value = executeDamageEffectsRole;
        }
        else
        {
            Effects.Value = Set<DamageType>.Of();
        }

        var immunityTypes = target.GetImmunityTypes();
        DamageTypes.Value = damageTypes - immunityTypes;
    }

    private static bool ShowDamageEffects(Set<DamageType> damageTypes, int X, int Y, int frame, in LockedSurface surface)
    {
        bool flag = false;
        if (damageTypes.Contains(DamageType.Fire))
        {
            flag = flag || AoWHSSet.Current.AbilityControl.GetAbility(127).ImageSequences.Show(0, X + 32, Y + 32, frame, in surface);
        }
        if (damageTypes.Contains(DamageType.Poison))
        {
            flag = flag || AoWHSSet.Current.AbilityControl.GetAbility(96).ImageSequences.Show(0, X + 32, Y + 32, frame, in surface);
        }
        if (damageTypes.Contains(DamageType.Cold))
        {
            flag = flag || AoWHSSet.Current.AbilityControl.GetAbility(95).ImageSequences.Show(1, X + 32, Y + 32, frame, in surface);
        }
        if (damageTypes.Contains(DamageType.Death))
        {
            flag = flag || AoWHSSet.Current.AbilityControl.GetAbility(93).ImageSequences.Show(0, X + 32, Y + 32, frame, in surface);
        }
        if (damageTypes.Contains(DamageType.Holy))
        {
            flag = flag || AoWHSSet.Current.AbilityControl.GetAbility(98).ImageSequences.Show(0, X + 32, Y + 32, frame, in surface);
        }
        return flag;
    }

    private static int DamageEffectsAnimationFrames(Set<DamageType> damageTypes)
    {
        int number = 0;
        if (damageTypes.Contains(DamageType.Fire))
        {
            int framesCount = AoWHSSet.Current.AbilityControl.GetAbility(127).ImageSequences.GetFramesCount(0);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        if (damageTypes.Contains(DamageType.Poison))
        {
            int framesCount = AoWHSSet.Current.AbilityControl.GetAbility(96).ImageSequences.GetFramesCount(0);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        if (damageTypes.Contains(DamageType.Cold))
        {
            int framesCount = AoWHSSet.Current.AbilityControl.GetAbility(95).ImageSequences.GetFramesCount(1);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        if (damageTypes.Contains(DamageType.Death))
        {
            int framesCount = AoWHSSet.Current.AbilityControl.GetAbility(93).ImageSequences.GetFramesCount(0);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        if (damageTypes.Contains(DamageType.Holy))
        {
            int framesCount = AoWHSSet.Current.AbilityControl.GetAbility(98).ImageSequences.GetFramesCount(0);
            if (framesCount > number)
            {
                number = framesCount;
            }
        }
        return number;
    }

    private static DamageType DamageTypesToDamageType(Set<DamageType> damageTypes)
    {
        if (damageTypes != Set<DamageType>.Of(Array.Empty<DamageType>()))
        {
            if (damageTypes.Contains(DamageType.Holy))
            {
                return DamageType.Holy;
            }
            if (damageTypes.Contains(DamageType.Magic))
            {
                return DamageType.Magic;
            }
            if (damageTypes.Contains(DamageType.Death))
            {
                return DamageType.Death;
            }
            if (damageTypes.Contains(DamageType.Cold))
            {
                return DamageType.Cold;
            }
            if (damageTypes.Contains(DamageType.Fire))
            {
                return DamageType.Fire;
            }
            if (damageTypes.Contains(DamageType.Poison))
            {
                return DamageType.Poison;
            }
            if (damageTypes.Contains(DamageType.Physical))
            {
                return DamageType.Physical;
            }
            if (damageTypes.Contains(DamageType.Wall))
            {
                return DamageType.Wall;
            }
            return DamageType.None;
        }
        return DamageType.None;
    }

    // intf_P3_02.inc:38

    // intf_P3_02.inc:38

}
