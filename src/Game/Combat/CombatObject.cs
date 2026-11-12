// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// FCombat. FID. FTargets. FTargetedBy. outgoing CV/wall CV. incoming CV/wall CV. target counts.
// attacker counts. target priority/cache flag. strength/cache flag. FCombatPlayer. FPosition.
// Status. ControlMode
public partial class CombatObject : EngineObject
{

    // Combat.pas:1922
    public CombatObject(ECustomNode owner) : base(owner)
    {
        Targets = new QuadItemList<CombatObject>();
        TargetedBy = new QuadItemList<CombatObject>();
        CombatPlayer = -1;
    }

    public Battle Combat;
    public byte Id;
    public QuadItemList<CombatObject> Targets;
    public QuadItemList<CombatObject> TargetedBy;
    public int CombatValue;
    public int WallCombatValue;
    public int IncomingCombatValue;
    public int IncomingWallCombatValue;
    public int TargetCount;
    public int WallTargetCount;
    public int AttackerCount;
    public int WallAttackerCount;
    public int TargetPriority;
    public bool TargetPriorityInvalid;
    public int TargetStrength;
    public bool TargetStrengthInvalid;
    public sbyte CombatPlayer;
    public byte Position;
    public Set<CombatObjectStatusElement> Status;
    public Set<CombatObjectControlModeElement> ControlMode;

    // Combat.pas:1930
    public override void Dispose()
    {
        if (Status.Contains(CombatObjectStatusElement.Activated))
        {
            Deactivate();
        }

        Targets?.Dispose();
        TargetedBy?.Dispose();
        base.Dispose();
    }

    // Combat.pas:5620
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Position, 10);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref CombatPlayer), 12);
        RwSet(ref Status, 1, 13, stream.rwData, 0);
        stream.rwData(Bytes.Of(ref Id, 1), 1, 14);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            Status = Status - Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Activated);
        }
    }

    // Combat.pas:5524
    public virtual void GetTargetCombatValueDamageValue(CombatObject target, ref int combatValue, ref int damageValue)
    {
        combatValue = 0;
        damageValue = 0;
    }

    // Combat.pas:5527
    public virtual int GetTargetWallCombatValue(CombatObject target) => 0;

    // Combat.pas:1938
    public virtual void TargetsChanged()
    {
        TargetPriorityInvalid = true;
        TargetStrengthInvalid = true;
    }

    // Combat.pas:5530
    public virtual int GetTargetPriority()
    {
        int I;
        int combatValue;
        CombatObject target;
        int count;
        if (TargetPriorityInvalid)
        {
            TargetPriority = 0;
            count = 0;
            I = TargetedBy.Count - 1;
            for (; I >= 0; I -= 1)
            {
                if (TargetedBy.List[I].Second == 0)
                {
                    count += 1;
                }
            }

            I = Targets.Count - 1;
            for (; I >= 0; I -= 1)
            {
                combatValue = Targets.List[I].Second;
                if (combatValue != 0)
                {
                    target = Targets.List[I].First;
                    combatValue = Round((double)(combatValue * combatValue) / target.IncomingCombatValue);
                    TargetPriority += combatValue;
                }
            }

            TargetPriority = (count + 2) * TargetPriority;
            I = TargetedBy.Count;
            // { Original bug: the normalized priority is stored in FTargetStrength. }
            if (I != 0)
            {
                TargetStrength = TargetPriority / I;
            }
            else
            {
                TargetStrength = 0;
            }

            TargetPriorityInvalid = false;
        }

        return TargetPriority;
    }

    // Combat.pas:5559
    public virtual int GetTargetStrength()
    {
        if (TargetStrengthInvalid)
        {
            TargetStrength = 0;
            for (int I = Targets.Count - 1; I >= 0; I--)
            {
                TargetStrength += Targets.List[I].Fourth;
            }

            if (TargetStrength != 0)
            {
                TargetStrength = TargetStrength / Targets.Count;
            }

            TargetStrengthInvalid = false;
            return TargetStrength;
        }

        return TargetStrength;
    }

    // Combat.pas:5633
    public virtual bool GetEnabled() => (Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed, CombatObjectStatusElement.Fled) & Status) == Set<CombatObjectStatusElement>.Of();

    // Combat.pas:5706
    public virtual bool GetLocked() => false;

    // Combat.pas:5709
    public virtual void SetCombatPlayer(byte player)
    {
        bool changedSide;
        Battle combat;
        Battle newCombat;
        if (Status.Contains(CombatObjectStatusElement.Activated))
        {
            bool condition = player != 255;
            if (condition)
            {
                var players = Combat.CombatData.Players.GetPlayers((sbyte)player);
                var side = GetSide();
                condition = (CombatSideIndex)players.SideIndex != side;
            }

            changedSide = condition;
            if (CombatPlayer != -1)
            {
                combat = Combat;
                var players = combat.CombatData.Players.GetPlayers(CombatPlayer);
                combat.Sides[(int)(CombatSideIndex)players.SideIndex].RemoveChild(this);
            }

            CombatPlayer = (sbyte)player;
            if (CombatPlayer != -1)
            {
                newCombat = Combat;
                var players = newCombat.CombatData.Players.GetPlayers(CombatPlayer);
                newCombat.Sides[(int)(CombatSideIndex)players.SideIndex].AddChild(this);
            }

            if (changedSide)
            {
                ResetTargets();
                SetupTargets();
                Combat.ObjectSideChanged(this);
            }
        }
        else
        {
            CombatPlayer = (sbyte)player;
        }
    }

    // Combat.pas:5737
    public virtual sbyte GetAttack() => 0;

    // Combat.pas:5740
    public virtual sbyte GetDefense() => 0;

    // Combat.pas:5743
    public virtual sbyte GetResistance() => 0;

    // Combat.pas:5746
    public virtual sbyte GetDamage() => 0;

    // Combat.pas:5749
    public virtual Set<DamageType> GetImmunityTypes() => Set<DamageType>.Of();

    // Combat.pas:5752
    public virtual Set<DamageType> GetProtectionTypes() => Set<DamageType>.Of();

    // Combat.pas:5758
    public virtual sbyte GetHits() => 0;

    // Combat.pas:5764
    public virtual sbyte GetHitPoints() => 0;

    // Combat.pas:5761
    public virtual void SetHitPoints(byte value)
    {
    }

    // Combat.pas:5779
    public virtual Alignment GetAlignment() => Alignment.None;

    // Combat.pas:5755
    public virtual int GetMoves() => 0;

    // Combat.pas:5767
    public virtual int GetExperience() => 0;

    // Combat.pas:5770
    public virtual void SetExperience(int value)
    {
    }

    // Combat.pas:5773
    public virtual byte GetLevel() => 0;

    // Combat.pas:5617
    public virtual int GetVisibilityRange() => 0;

    // Combat.pas:5785
    public virtual bool GetAbilityEnabled(int abilityId) => false;

    // Combat.pas:5788
    public virtual void SetAbilityEnabled(int abilityId, bool enabled)
    {
    }

    // Combat.pas:5791
    public virtual int GetAbilityLevel(int abilityId) => 0;

    // Combat.pas:5794
    public virtual int GetAbilityCount() => 0;

    // Combat.pas:5782
    public virtual AbilityOwner GetAbilityOwner() => null;

    // Combat.pas:5672
    public virtual void Activate(Battle combat)
    {
        byte player;
        Battle combatLocal;
        Combat = combat;
        Status = Status + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Activated);
        player = (byte)CombatPlayer;
        if (player != 255)
        {
            combatLocal = Combat;
            // { This eliminated copy preserves the original saved registers. }
            var players = combatLocal.CombatData.Players.GetPlayers((sbyte)player);
            combatLocal.Sides[(int)(CombatSideIndex)players.SideIndex].AddChild(this);
        }
    }

    // Combat.pas:5685
    public virtual void Deactivate()
    {
        byte player;
        Battle combat;
        Status = Status - Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Activated);
        player = (byte)CombatPlayer;
        if (player != 255)
        {
            combat = Combat;
            // { This eliminated copy preserves the original saved registers. }
            var players = combat.CombatData.Players.GetPlayers((sbyte)player);
            combat.Sides[(int)(CombatSideIndex)players.SideIndex].RemoveChild(this);
        }
    }

    // Combat.pas:5813
    public virtual void ObjectDestroyed(CombatObject objectValue)
    {
        if (objectValue.GetSide() != GetSide())
        {
            RemoveTarget(objectValue);
        }
    }

    // Combat.pas:5592
    public virtual void ObjectSideChanged(CombatObject objectValue)
    {
        int combatValue = default;
        int damageValue = default;
        int wallCombatValue;
        if (objectValue.GetSide() != GetSide())
        {
            GetTargetCombatValueDamageValue(objectValue, ref combatValue, ref damageValue);
            if (Combat.CombatData.WallType != 0)
            {
                wallCombatValue = GetTargetWallCombatValue(objectValue);
            }
            else
            {
                wallCombatValue = 0;
            }

            AddTarget(objectValue, combatValue, wallCombatValue, damageValue);
        }
        else
        {
            RemoveTarget(objectValue);
        }
    }

    // Combat.pas:5697
    public virtual void Initialize(Battle combat)
    {
        Combat = combat;
    }

    // Combat.pas:5700
    public virtual void Finalize(FinalizeMode mode)
    {
        Dispose();
    }

    // Combat.pas:5614
    public virtual int FastBattleRoundDistance() => 0;

    // Combat.pas:5605
    public virtual bool FastBattleExecute(int moveCost) => false;

    // Combat.pas:5608
    public virtual bool FastBattleBehindWall() => false;

    // Combat.pas:5611
    public virtual bool FastBattleWallInBetween(byte target) => false;

    // Combat.pas:5818
    public virtual bool SupportCombatMode(Set<CombatMode> modes) => false;

    // Combat.pas:5630
    public virtual Set<WallCombatFeaturesElement> WallCombatFeatures() => Set<WallCombatFeaturesElement>.Of();

    // Combat.pas:5703
    public virtual void PlaySfx(byte sound)
    {
    }

    // Combat.pas:5654
    public virtual void NewTurn(byte combatPlayer)
    {
        if (combatPlayer == (byte)CombatPlayer)
        {
            Status = Status - Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.TurnDone);
        }
    }

    // Combat.pas:5657
    public virtual void NewRound()
    {
    }

    // Combat.pas:5660
    public virtual void CombatDone()
    {
    }

    // Combat.pas:5990
    public virtual void Show(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
    }

    // Combat.pas:5993
    public virtual int ShowAttackAnimation(int frame, int direction, int X, int Y, in LockedSurface surface) => 0;

    // Combat.pas:5996
    public virtual int ShowHitAnimation(int frame, int direction, int X, int Y, in LockedSurface surface) => 0;

    // Combat.pas:5868
    public virtual int DoDamage(CombatObject target, int damage)
    {
        int result;
        if (damage != 0 && target.GetEnabled())
        {
            result = target.ExecuteDamage(damage);
            if (target.Status.Contains(CombatObjectStatusElement.Destroyed) && !target.GetAbilityEnabled(129) && !target.GetAbilityEnabled(128))
            {
                damage = GetExperience();
                damage += target.GetLevel();
                SetExperience(damage);
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Combat.pas:5821
    public virtual DamageType GetOptimalDamageType(Set<DamageType> types)
    {
        Set<DamageType> available;
        Set<DamageType> protection;
        DamageType result = DamageType.None;
        available = types - GetImmunityTypes();
        if (available != Set<DamageType>.Of())
        {
            protection = GetProtectionTypes();
            if (protection == Set<DamageType>.Of() || available - protection == Set<DamageType>.Of())
            {
                while (!available.Contains(result))
                {
                    result = (DamageType)((int)result - 1);
                }
            }
            else
            {
                while (!available.Contains(result) || protection.Contains(result))
                {
                    result = (DamageType)((int)result - 1);
                }
            }
        }

        return result;
    }

    // Combat.pas:5836
    public virtual int ExecuteDamageRole(int attack, int damage, Set<DamageType> damageTypes)
    {
        Set<DamageType> types;
        int attackLocal;
        int result = 0;
        types = damageTypes - GetImmunityTypes();
        if (types != Set<DamageType>.Of())
        {
            attackLocal = attack - GetDefense();
            result = AbilityRules.ExecuteDamageRole(damage, attackLocal);
            if (types - GetProtectionTypes() == Set<DamageType>.Of())
            {
                result = (result + 1) / 2;
            }
        }

        return result;
    }

    // Combat.pas:5849
    public virtual int ExecuteDamageRole(int attack, int damage, Set<DamageType> damageTypes, byte damageMode)
    {
        Set<DamageType> types;
        int attackLocal;
        int result = 0;
        types = damageTypes - GetImmunityTypes();
        if (types != Set<DamageType>.Of())
        {
            if (damageMode == 1)
            {
                attackLocal = attack - GetResistance();
            }
            else
            {
                attackLocal = attack - GetDefense();
            }

            result = AbilityRules.ExecuteDamageRole(damage, attackLocal);
            if (types - GetProtectionTypes() == Set<DamageType>.Of())
            {
                result = (result + 1) / 2;
            }
        }

        return result;
    }

    // Combat.pas:5865
    public virtual Set<DamageType> ExecuteDamageEffectsRole(Set<DamageType> damageTypes) => Set<DamageType>.Of();

    // Combat.pas:5895
    public virtual void Surrender()
    {
        SetHitPoints(0);
        Status = Status + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed);
        ResetTargets();
        Combat.ObjectDestroyed(this);
    }

    // Combat.pas:5881
    public virtual void DestroyObject()
    {
        SetHitPoints(0);
        Status = Status + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed);
        for (int I = GetAbilityCount() - 1; I >= 0; I--)
        {
            if (GetAbilityEnabled(I))
            {
                AoWHSSet.Current.AbilityControl.GetAbility(I).CombatObjectDestroyed(this);
            }
        }

        if (Status.Contains(CombatObjectStatusElement.Destroyed))
        {
            ResetTargets();
            Combat.ObjectDestroyed(this);
        }
    }

    // Combat.pas:5903
    public virtual int ExecuteDamage(int damage)
    {
        int result;
        int hits;
        Assert(damage >= 0 && damage < 50, "Invalid Damage Value (TCombatUnit.ExecuteDamage)", "Combat.pas", 5982);
        hits = GetHitPoints();
        if (hits < damage)
        {
            result = hits;
        }
        else
        {
            result = damage;
        }

        SetHitPoints((byte)(hits - result));
        if (damage != 0)
        {
            Status = Status + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Damaged);
        }

        if (result != 0 && GetHitPoints() == 0)
        {
            DestroyObject();
        }

        return result;
    }

    // Combat.pas:5862
    public virtual Set<DamageType> ExecuteDamageEffects(Set<DamageType> damageTypes) => Set<DamageType>.Of();

    // Combat.pas:5663
    public virtual void TurnDone()
    {
        CombatObjectTurnDoneCombatAction A;
        A = new CombatObjectTurnDoneCombatAction(null);
        A.CombatObjectId = Id;
        Combat.ExecuteCombatAction(A);
        A.Release();
    }

    // Combat.pas:5776
    public virtual string IdString() => "TCombatObject";

    // Combat.pas:1941
    public void AddTarget(CombatObject target, int combatValue, int wallCombatValue, int damageValue)
    {
        Assert(Targets.IndexOfItemA(target) == -1, "target already exists use update target", "Combat.pas", 5473);
        CombatValue += combatValue;
        WallCombatValue += wallCombatValue;
        Targets.AddABCD(target, combatValue, wallCombatValue, damageValue);
        target.TargetedBy.AddABCD(this, combatValue, wallCombatValue, damageValue);
        target.IncomingCombatValue += combatValue;
        target.IncomingWallCombatValue += wallCombatValue;
        if (combatValue > 0)
        {
            TargetCount += 1;
            target.AttackerCount += 1;
        }

        if (wallCombatValue > 0)
        {
            WallTargetCount += 1;
            target.WallAttackerCount += 1;
        }

        TargetsChanged();
        target.TargetsChanged();
    }

    // Combat.pas:5488
    public void DeleteTarget(int index)
    {
        CombatObject target;
        int combatValue;
        int damageValue;
        if (index != -1)
        {
            target = Targets.List[index].First;
            combatValue = Targets.List[index].Second;
            damageValue = Targets.List[index].Third;
            CombatValue -= combatValue;
            WallCombatValue -= damageValue;
            Targets.Delete(index);
            if (combatValue > 0)
            {
                TargetCount -= 1;
            }

            if (damageValue > 0)
            {
                WallTargetCount -= 1;
            }

            TargetsChanged();
            index = target.TargetedBy.IndexOfItemA(this);
            if (index != -1)
            {
                target.IncomingCombatValue -= target.TargetedBy.List[index].Second;
                // { Original bug: FIncomingWallCombatValue is not decremented. }
                target.TargetedBy.Delete(index);
                if (combatValue > 0)
                {
                    target.AttackerCount -= 1;
                }

                if (damageValue > 0)
                {
                    target.WallAttackerCount -= 1;
                }

                target.TargetsChanged();
            }
        }
    }

    // Combat.pas:5515
    public void RemoveTarget(CombatObject target)
    {
        DeleteTarget(Targets.IndexOfItemA(target));
    }

    // Combat.pas:5518
    public void ResetTargets()
    {
        for (int I = Targets.Count - 1; I >= 0; I--)
        {
            DeleteTarget(I);
        }
    }

    // Combat.pas:5575
    public void SetupTargets()
    {
        int combatValue = default;
        int damageValue = default;
        int wallCombatValue;
        CombatObject target;
        CombatSide side;
        var opponentSide = GetOpponentSide();
        side = Combat.Sides[(int)opponentSide];
        for (int I = side.Count - 1; I >= 0; I--)
        {
            target = side.GetObjects(I);
            if (!target.Status.Contains(CombatObjectStatusElement.Destroyed))
            {
                GetTargetCombatValueDamageValue(target, ref combatValue, ref damageValue);
                if (Combat.CombatData.WallType != 0)
                {
                    wallCombatValue = GetTargetWallCombatValue(target);
                }
                else
                {
                    wallCombatValue = 0;
                }

                AddTarget(target, combatValue, wallCombatValue, damageValue);
            }
        }
    }

    // Combat.pas:5636
    public CombatSideIndex GetSide()
    {
        CombatSideIndex result;
        sbyte player;
        player = CombatPlayer;
        if (player != -1)
        {
            result = (CombatSideIndex)Combat.CombatData.Players.GetPlayers(player).SideIndex;
        }
        else
        {
            result = CombatSideIndex.Neutral;
        }

        return result;
    }

    // Combat.pas:5645
    public CombatSideIndex GetOpponentSide()
    {
        CombatSideIndex result;
        sbyte player;
        player = CombatPlayer;
        if (player != -1)
        {
            result = (CombatSideIndex)(Combat.CombatData.Players.GetPlayers(player).SideIndex ^ 1);
        }
        else
        {
            result = CombatSideIndex.Neutral;
        }

        return result;
    }

    // Combat.pas:5797
    public sbyte GetPlayer()
    {
        sbyte result;
        byte player;
        player = (byte)CombatPlayer;
        if (player != 255)
        {
            result = Combat.CombatData.Players.GetPlayers((sbyte)player).Player;
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Combat.pas:5805
    public sbyte GetPlayerIndependentRelation()
    {
        sbyte result;
        byte player;
        player = (byte)CombatPlayer;
        if (player != 255)
        {
            result = (sbyte)Combat.CombatData.Players.GetPlayers((sbyte)player).IndependentRelation;
        }
        else
        {
            result = -1;
        }

        return result;
    }

    internal static Set<DamageType> MeleeDamageTypes(CombatObject source)
    {
        Set<DamageType> result;
        if (source is CombatUnit)
        {
            result = ((StrikeAbility)AoWHSSet.Current.AbilityControl.GetAbility(21)).GetDamageTypes(((CombatUnit)source).Troop);
        }
        else
        {
            result = Set<DamageType>.Of();
        }

        return result;
    }

    internal static int StrikeDamageValue(CombatObject source, CombatObject target)
    {
        int result;
        sbyte attack;
        sbyte damage;
        Set<DamageType> types;
        if (source.GetAbilityEnabled(21))
        {
            attack = source.GetAttack();
            damage = source.GetDamage();
            types = MeleeDamageTypes(source);
            bool condition = source.GetAbilityEnabled(146) || source.GetAbilityEnabled(160);
            if (condition)
            {
                var alignment = target.GetAlignment();
                condition = Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(alignment);
            }

            if (condition)
            {
                attack = (sbyte)(attack + 2);
                damage = (sbyte)(damage + 2);
            }

            bool test = source.GetAbilityEnabled(147) || source.GetAbilityEnabled(161);
            if (test)
            {
                var alignment = target.GetAlignment();
                test = Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(alignment);
            }

            if (test)
            {
                attack = (sbyte)(attack + 2);
                damage = (sbyte)(damage + 2);
            }

            if (source.GetAbilityEnabled(112) && target.GetAbilityEnabled(63))
            {
                attack = (sbyte)(attack + 3);
                damage = (sbyte)(damage + 3);
            }

            result = AbilityRules.StatisticsToLimitedDamageValue(attack, damage, types, target.GetHitPoints(), target.GetDefense(), target.GetResistance(), target.GetImmunityTypes(), target.GetProtectionTypes());
        }
        else
        {
            result = 0;
        }

        return result;
    }

    internal static StrikeCombatAction CreateStrikeCombatAction(CombatObject source, CombatObject target, bool defensive)
    {
        StrikeCombatAction result;
        sbyte attack;
        Set<DamageType> types;
        sbyte damage;
        if (source.GetAbilityEnabled(21))
        {
            attack = source.GetAttack();
            damage = source.GetDamage();
            types = MeleeDamageTypes(source);
            bool condition = source.GetAbilityEnabled(146) || source.GetAbilityEnabled(160);
            if (condition)
            {
                var alignment = target.GetAlignment();
                condition = Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(alignment);
            }

            if (condition)
            {
                attack = (sbyte)(attack + 2);
                damage = (sbyte)(damage + 2);
            }

            bool test = source.GetAbilityEnabled(147) || source.GetAbilityEnabled(161);
            if (test)
            {
                var alignment = target.GetAlignment();
                test = Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(alignment);
            }

            if (test)
            {
                attack = (sbyte)(attack + 2);
                damage = (sbyte)(damage + 2);
            }

            if (source.GetAbilityEnabled(112) && target.GetAbilityEnabled(63))
            {
                attack = (sbyte)(attack + 3);
                damage = (sbyte)(damage + 3);
            }

            result = new StrikeCombatAction(null);
            result.Generate(source, target, attack, damage, types);
            result.SetDefensive(defensive);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // intf_P3_01.inc:157

    // intf_P3_01.inc:157

}
