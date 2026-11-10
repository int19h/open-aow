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
public partial class DamageCA : SingleTargetCA
{
    // Combat2.pas:774
    public new const int ClassID = 131761;

    public DamageCA(ECustomNode owner) : base(owner)
    {
    }

    public byte FDamage;
    public P3PackedDamageTypes FDamageTypes;
    public P3PackedDamageTypes FEffects;
    public byte FDamageDone;

    // Combat2.pas:777
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FDamage, 15);
        Stream.rwData(Bytes.Of(ref FEffects, 2), 2, 16);
        Stream.rwData(Bytes.Of(ref FDamageTypes, 2), 2, 17);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Byte("Damage", ref FDamage);
        PersistSupport.Record(persister, "Effects", ref FEffects);
        PersistSupport.Record(persister, "DamageTypes", ref FDamageTypes);
    }

    // Combat2.pas:817
    public override void Execute(Battle Combat)
    {
        CombatObject Target;
        CombatObject Source;
        Source = Combat.FCombatData.FindID(FSourceID);
        Target = Combat.FCombatData.FindID(FTargetID);
        if (FDamage != 0)
        {
            FDamageDone = (byte)Source.DoDamage(Target, FDamage);
        }

        if (FEffects.Value != Set<DamageType>.Of())
        {
            Target.ExecuteDamageEffects(FEffects.Value);
        }
    }

    // Combat2.pas:826
    public override bool Play(CombatViewer Viewer)
    {
        int HitFrame;
        int Duration;
        int EffectFrames;
        CombatObject Target;
        if (Viewer.FDelay < 5)
        {
            HitFrame = Viewer.FDelay;
        }
        else
        {
            HitFrame = 5;
        }

        if (HitFrame == Viewer.FTime)
        {
            Execute(Viewer.FCombat);
            Target = Viewer.FCombat.FCombatData.FindID(FTargetID);
            if (FDamageDone != 0)
            {
                if (!Target.FStatus.Contains(CombatObjectStatusElement.Destroyed))
                {
                    Target.PlaySFX(2);
                }
                else
                {
                    Target.PlaySFX(3);
                }
            }
        }

        Duration = Viewer.FDelay * 2;
        if (FEffects.Value != Set<DamageType>.Of())
        {
            EffectFrames = DamageEffectsAnimationFrames(FEffects.Value) + Viewer.FDelay;
            HitFrame = Viewer.FDelay * 3;
            if (EffectFrames > HitFrame)
            {
                EffectFrames = HitFrame;
            }

            if (Duration < EffectFrames)
            {
                Duration = EffectFrames;
            }
        }

        return Duration > Viewer.FTime;
    }

    // Combat2.pas:848
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        int HitFrame;
        if (Obj.FID == FSourceID)
        {
            Obj.ShowAttackAnimation(Viewer.FTime, Direction, X, Y, Surface);
        }
        else
        {
            if (Viewer.FDelay < 5)
            {
                HitFrame = Viewer.FDelay;
            }
            else
            {
                HitFrame = 5;
            }

            if (Obj.FID == FTargetID && FDamageDone != 0 && HitFrame <= Viewer.FTime)
            {
                Obj.Show(Frame, Direction, X, Y, Surface);
                if (FEffects.Value != Set<DamageType>.Of())
                {
                    ShowDamageEffects(FEffects.Value, X, Y, Viewer.FTime - HitFrame, Surface);
                }

                var damageTypesToDamageType = DamageTypesToDamageType(FDamageTypes.Value);
                AbilityRules.ShowDamageIcon(X, Y, Surface, FDamageDone, damageTypesToDamageType);
            }
            else
            {
                Obj.Show(Frame, Direction, X, Y, Surface);
            }
        }
    }

    // Combat2.pas:785
    public virtual void Setup(byte SourceID, byte TargetID, byte Damage, Set<DamageType> Effects, Set<DamageType> DamageTypes, bool Defensive)
    {
        FSourceID = SourceID;
        FTargetID = TargetID;
        FDamage = Damage;
        FEffects.Value = Effects;
        FDamageTypes.Value = DamageTypes;
        if (Defensive)
        {
            FMode = FMode + Set<SingleTargetModeElement>.Of(SingleTargetModeElement.Defensive);
        }
    }

    // Combat2.pas:795
    public virtual void Generate(CombatObject Source, CombatObject Target, sbyte Attack, sbyte Damage, Set<DamageType> DamageTypes)
    {
        FSourceID = Source.FID;
        FTargetID = Target.FID;
        FDamage = (byte)Target.ExecuteDamageRole(Attack, Damage, DamageTypes);
        if (FDamage != 0)
        {
            var executeDamageEffectsRole = Target.ExecuteDamageEffectsRole(DamageTypes);
            FEffects.Value = executeDamageEffectsRole;
        }
        else
        {
            FEffects.Value = Set<DamageType>.Of();
        }

        var immunityTypes = Target.GetImmunityTypes();
        FDamageTypes.Value = DamageTypes - immunityTypes;
    }

    // Combat2.pas:806
    public virtual void GenerateEx(CombatObject Source, CombatObject Target, sbyte Attack, sbyte Damage, Set<DamageType> DamageTypes, byte DamageMode)
    {
        FSourceID = Source.FID;
        FTargetID = Target.FID;
        FDamage = (byte)Target.ExecuteDamageRoleEx(Attack, Damage, DamageTypes, DamageMode);
        if (FDamage != 0)
        {
            var executeDamageEffectsRole = Target.ExecuteDamageEffectsRole(DamageTypes);
            FEffects.Value = executeDamageEffectsRole;
        }
        else
        {
            FEffects.Value = Set<DamageType>.Of();
        }

        var immunityTypes = Target.GetImmunityTypes();
        FDamageTypes.Value = DamageTypes - immunityTypes;
    }

    private static bool ShowDamageEffects(Set<DamageType> DamageTypes, int X, int Y, int Frame, in SurfaceDesc Surface)
    {
        bool flag = false;
        if (DamageTypes.Contains(DamageType.Fire))
        {
            flag = flag || AoWHSSet.Current.FAbilityControl.GetAbility(127).FImageSequences.Show(0, X + 32, Y + 32, Frame, in Surface);
        }
        if (DamageTypes.Contains(DamageType.Poison))
        {
            flag = flag || AoWHSSet.Current.FAbilityControl.GetAbility(96).FImageSequences.Show(0, X + 32, Y + 32, Frame, in Surface);
        }
        if (DamageTypes.Contains(DamageType.Cold))
        {
            flag = flag || AoWHSSet.Current.FAbilityControl.GetAbility(95).FImageSequences.Show(1, X + 32, Y + 32, Frame, in Surface);
        }
        if (DamageTypes.Contains(DamageType.Death))
        {
            flag = flag || AoWHSSet.Current.FAbilityControl.GetAbility(93).FImageSequences.Show(0, X + 32, Y + 32, Frame, in Surface);
        }
        if (DamageTypes.Contains(DamageType.Holy))
        {
            flag = flag || AoWHSSet.Current.FAbilityControl.GetAbility(98).FImageSequences.Show(0, X + 32, Y + 32, Frame, in Surface);
        }
        return flag;
    }

    private static int DamageEffectsAnimationFrames(Set<DamageType> DamageTypes)
    {
        int num = 0;
        if (DamageTypes.Contains(DamageType.Fire))
        {
            int framesCount = AoWHSSet.Current.FAbilityControl.GetAbility(127).FImageSequences.GetFramesCount(0);
            if (framesCount > num)
            {
                num = framesCount;
            }
        }
        if (DamageTypes.Contains(DamageType.Poison))
        {
            int framesCount = AoWHSSet.Current.FAbilityControl.GetAbility(96).FImageSequences.GetFramesCount(0);
            if (framesCount > num)
            {
                num = framesCount;
            }
        }
        if (DamageTypes.Contains(DamageType.Cold))
        {
            int framesCount = AoWHSSet.Current.FAbilityControl.GetAbility(95).FImageSequences.GetFramesCount(1);
            if (framesCount > num)
            {
                num = framesCount;
            }
        }
        if (DamageTypes.Contains(DamageType.Death))
        {
            int framesCount = AoWHSSet.Current.FAbilityControl.GetAbility(93).FImageSequences.GetFramesCount(0);
            if (framesCount > num)
            {
                num = framesCount;
            }
        }
        if (DamageTypes.Contains(DamageType.Holy))
        {
            int framesCount = AoWHSSet.Current.FAbilityControl.GetAbility(98).FImageSequences.GetFramesCount(0);
            if (framesCount > num)
            {
                num = framesCount;
            }
        }
        return num;
    }

    private static DamageType DamageTypesToDamageType(Set<DamageType> DamageTypes)
    {
        if (DamageTypes != Set<DamageType>.Of(Array.Empty<DamageType>()))
        {
            if (DamageTypes.Contains(DamageType.Holy))
            {
                return DamageType.Holy;
            }
            if (DamageTypes.Contains(DamageType.Magic))
            {
                return DamageType.Magic;
            }
            if (DamageTypes.Contains(DamageType.Death))
            {
                return DamageType.Death;
            }
            if (DamageTypes.Contains(DamageType.Cold))
            {
                return DamageType.Cold;
            }
            if (DamageTypes.Contains(DamageType.Fire))
            {
                return DamageType.Fire;
            }
            if (DamageTypes.Contains(DamageType.Poison))
            {
                return DamageType.Poison;
            }
            if (DamageTypes.Contains(DamageType.Physical))
            {
                return DamageType.Physical;
            }
            if (DamageTypes.Contains(DamageType.Wall))
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
