// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class P1AbilityView : EObject
{
    public P1AbilityView(ECustomNode owner) : base(owner)
    { }

    // Combat.pas:43
    public virtual void Execute()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:44
    public virtual bool GetHidden()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:45
    public virtual AbilityType GetAbilityType()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:46
    public virtual string GetName(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:47
    public virtual sbyte GetAttack(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:48
    public virtual sbyte GetDefense(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:49
    public virtual sbyte GetResistance(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:50
    public virtual sbyte GetDamage(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:51
    public virtual Set<MoveType> GetMoveTypes(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:52
    public virtual sbyte GetLevel(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:53
    public virtual bool GetEnabled(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:54
    public virtual Set<DamageType> GetImmunityTypes(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:55
    public virtual Set<DamageType> GetProtectionTypes(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:56
    public virtual int GetSkillPoints(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:57
    public virtual CombatMode GetCombatMode(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:58
    public virtual Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:59
    public virtual string GetSourceName(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:60
    public virtual bool GetInherent(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:61
    public virtual int GetInherentLevel(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:62
    public virtual Set<AbilityControlTypeElement> GetControlType(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:63
    public virtual bool fcValidRoundDistance(CombatObject Source, CombatObject Target)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:64
    public virtual void fcPrefetchCombatCommands(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:65
    public virtual bool fcExecuteCombatCommand(AbilityOwner Owner, object Command)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:66
    public virtual void NewDay(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:67
    public virtual void NewTurn(AbilityOwner Owner, object Info)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:68
    public virtual bool CanActivate(AbilityOwner Owner, ref string Reason)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:69
    public virtual bool Activate(AbilityOwner Owner, ref string Reason)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:70
    public virtual bool CanActivateCombat(AbilityOwner Owner, ref string Reason)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:71
    public virtual bool ActivateCombat(AbilityOwner Owner, ref string Reason)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:72
    public virtual void ListSpells()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:73
    public virtual bool CanExpand(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:74
    public virtual int ExpandCost()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:75
    public virtual string ExpandName(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:76
    public virtual bool Expand(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:77
    public virtual bool Remove(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:78
    public virtual int GetDamageValue(CombatObject Source, CombatObject Target)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:79
    public virtual void GetDamageValueEx(CombatObject Source, CombatObject Target, bool Defensive, ref P1DamageValues Values)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:80
    public virtual void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:81
    public virtual void fcGetDamageValueEx(CombatObject Source, CombatObject Target, bool Defensive, ref P1DamageValues Values)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:82
    public virtual int fcGetDamageValue(CombatObject Source, CombatObject Target, bool Defensive)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:83
    public virtual void tcGetDamageValueEx(CombatObject Source, CombatObject Target, ref P1DamageValues Values)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:84
    public virtual int tcGetDamageValue(CombatObject Source, CombatObject Target)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:85
    public virtual void CombatObjectDestroyed(CombatObject Obj)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:86
    public virtual void NewCombatTurn(AbilityOwner Owner, object Info)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:87
    public virtual void CombatDone(AbilityOwner Owner)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:88
    public virtual void ListInfo(StringCollection Strings, byte Mode)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:89
    public virtual void GetCombatInfo(AbilityOwner Owner, ref P1CombatInfo Info)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:90
    public virtual Type AbilityDataClass()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    [StructLayout(LayoutKind.Explicit, Size = 20)]
    public struct P1DamageValues
    {
        [FieldOffset(0)]
        public int Damage;
        [FieldOffset(4)]
        public int CounterDamage;
        [FieldOffset(8)]
        public int Experience;
        [FieldOffset(12)]
        public int CounterExperience;
        [FieldOffset(16)]
        public int Reserved;
    }

    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public struct P1CombatInfo
    {
        [FieldOffset(0)]
        public byte Kind;
        [FieldOffset(1)]
        public sbyte Attack;
        [FieldOffset(2)]
        public sbyte Damage;
        [FieldOffset(3)]
        public ushort Types__image;

        public Set<DamageType> Types { get => Set<DamageType>.FromMask(Types__image); set => Types__image = (ushort)value.Image; }

        [FieldOffset(5)]
        public sbyte RepeatCount;
    }
}
