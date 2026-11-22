#nullable disable warnings
// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Combat.Actions;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Combat;

// FCombat. FID. FTargets. FTargetedBy. outgoing CV/wall CV. incoming CV/wall CV. target counts.
// attacker counts. target priority/cache flag. strength/cache flag. FCombatPlayer. FPosition.
// Status. ControlMode
public class CombatObject(CustomNode? owner) : EngineObject(owner)
{

    public Battle Combat { get; set; }

    public byte Id { get; set; }

    public QuadItemList<CombatObject> Targets { get; } = new QuadItemList<CombatObject>();
    public QuadItemList<CombatObject> TargetedBy { get; } = new QuadItemList<CombatObject>();
    public int CombatValue { get; private set; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Construction snapshots record this field, and the Delphi field is only written.")]
    private int _wallCombatValue;

    public int IncomingCombatValue { get; private set; }
    public int IncomingWallCombatValue { get; private set; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Construction snapshots record this field, and the Delphi field is only written.")]
    private int _targetCount;
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Construction snapshots record this field, and the Delphi field is only written.")]
    private int _wallTargetCount;

    public int AttackerCount { get; private set; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Construction snapshots record this field, and the Delphi field is only written.")]
    private int _wallAttackerCount;

    public int TargetPriority { get; protected set; }
    public bool TargetPriorityInvalid { get; protected set; }
    public int TargetStrength { get; protected set; }
    public bool TargetStrengthInvalid { get; private set; }

    public sbyte CombatPlayer { get; private set; } = -1;
    public byte Position { get; set; }
    public CombatObjectStatuses Status { get; set; }

    public CombatObjectControlModes ControlMode { get; set; }

    // Combat.pas:1930
    public override void Dispose()
    {
        if (Status.Contains(CombatObjectStatuses.Activated))
        {
            Deactivate();
        }

        Targets?.Dispose();
        TargetedBy?.Dispose();
        base.Dispose();
    }

    // Combat.pas:5620
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Position = persister.Byte("Position", value: Position);
        CombatPlayer = (sbyte)persister.UnsignedByte(name: "CombatPlayer", value: CombatPlayer);
        Status = persister.Set("Status", value: Status);
        Id = persister.RawByte("Id", value: Id);
        // The translate stream's mode holds read, so the original ReadWrite clears the activated
        // flag there too.
        if (persister.Loading || persister.Translating())
        {
            Status &= ~CombatObjectStatuses.Activated;
        }
    }

    // Combat.pas:5524
    public virtual void GetTargetCombatValueDamageValue(CombatObject target, out int combatValue, out int damageValue)
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
        if (TargetPriorityInvalid)
        {
            TargetPriority = 0;
            int count = 0;
            int i = TargetedBy.Count - 1;
            for (; i >= 0; i -= 1)
            {
                if (TargetedBy.List[i].Second == 0)
                {
                    count += 1;
                }
            }

            i = Targets.Count - 1;
            for (; i >= 0; i -= 1)
            {
                int combatValue = Targets.List[i].Second;
                if (combatValue != 0)
                {
                    CombatObject target = Targets.List[i].First;
                    combatValue = Round((double)(combatValue * combatValue) / target.IncomingCombatValue);
                    TargetPriority += combatValue;
                }
            }

            TargetPriority = (count + 2) * TargetPriority;
            i = TargetedBy.Count;
            // { Original bug: the normalized priority is stored in FTargetStrength. }
            TargetStrength = i != 0 ? TargetPriority / i : 0;

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
            for (int i = Targets.Count - 1; i >= 0; i--)
            {
                TargetStrength += Targets.List[i].Fourth;
            }

            if (TargetStrength != 0)
            {
                TargetStrength /= Targets.Count;
            }

            TargetStrengthInvalid = false;
            return TargetStrength;
        }

        return TargetStrength;
    }

    // Combat.pas:5633
    public virtual bool GetEnabled() => ((CombatObjectStatuses.Destroyed | CombatObjectStatuses.Fled) & Status) == CombatObjectStatuses.None;

    // Combat.pas:5706
    public virtual bool GetLocked() => false;

    // Combat.pas:5709
    public virtual void SetCombatPlayer(byte player)
    {
        if (Status.Contains(CombatObjectStatuses.Activated))
        {
            bool condition = player != 255;
            if (condition)
            {
                CombatPlayer players = Combat.CombatData.Players.GetPlayers((sbyte)player);
                CombatSideIndex side = Side;
                condition = (CombatSideIndex)players.SideIndex != side;
            }

            bool changedSide = condition;
            if (CombatPlayer != -1)
            {
                Battle combat = Combat;
                CombatPlayer players = combat.CombatData.Players.GetPlayers(CombatPlayer);
                _ = combat.Sides[(int)(CombatSideIndex)players.SideIndex].RemoveChild(this);
            }

            CombatPlayer = (sbyte)player;
            if (CombatPlayer != -1)
            {
                Battle newCombat = Combat;
                CombatPlayer players = newCombat.CombatData.Players.GetPlayers(CombatPlayer);
                _ = newCombat.Sides[(int)(CombatSideIndex)players.SideIndex].AddChild(this);
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
    public virtual int GetAttack() => 0;

    // Combat.pas:5740
    public virtual int GetDefense() => 0;

    // Combat.pas:5743
    public virtual int GetResistance() => 0;

    // Combat.pas:5746
    public virtual int GetDamage() => 0;

    // Combat.pas:5749
    public virtual DamageTypes GetImmunityTypes() => DamageTypes.None;

    // Combat.pas:5752
    public virtual DamageTypes GetProtectionTypes() => DamageTypes.None;

    // Combat.pas:5758
    public virtual int GetHits() => 0;

    // Combat.pas:5764
    public virtual int HitPoints => 0;

    // Combat.pas:5761
    public virtual void SetHitPoints(int value)
    {
    }

    // Combat.pas:5779
    public virtual Alignment GetAlignment() => Alignment.Unaligned;

    // Combat.pas:5755
    public virtual int Moves => 0;

    // Combat.pas:5767
    public virtual int Experience => 0;

    // Combat.pas:5770
    public virtual void SetExperience(int value)
    {
    }

    // Combat.pas:5773
    public virtual int Level => 0;

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
    public virtual AbilityOwner? AbilityOwner => null;

    // Combat.pas:5672
    public virtual void Activate(Battle combat)
    {
        Combat = combat;
        Status |= CombatObjectStatuses.Activated;
        byte player = (byte)CombatPlayer;
        if (player != 255)
        {
            Battle combatLocal = Combat;
            // { This eliminated copy preserves the original saved registers. }
            CombatPlayer players = combatLocal.CombatData.Players.GetPlayers((sbyte)player);
            _ = combatLocal.Sides[(int)(CombatSideIndex)players.SideIndex].AddChild(this);
        }
    }

    // Combat.pas:5685
    public virtual void Deactivate()
    {
        Status &= ~CombatObjectStatuses.Activated;
        byte player = (byte)CombatPlayer;
        if (player != 255)
        {
            Battle combat = Combat;
            // { This eliminated copy preserves the original saved registers. }
            CombatPlayer players = combat.CombatData.Players.GetPlayers((sbyte)player);
            _ = combat.Sides[(int)(CombatSideIndex)players.SideIndex].RemoveChild(this);
        }
    }

    // Combat.pas:5813
    public virtual void ObjectDestroyed(CombatObject objectValue)
    {
        if (objectValue.Side != Side)
        {
            RemoveTarget(objectValue);
        }
    }

    // Combat.pas:5592
    public virtual void ObjectSideChanged(CombatObject objectValue)
    {
        if (objectValue.Side != Side)
        {
            GetTargetCombatValueDamageValue(objectValue, out int combatValue, out int damageValue);
            int wallCombatValue = Combat.CombatData.WallType != 0 ? GetTargetWallCombatValue(objectValue) : 0;

            AddTarget(objectValue, combatValue, wallCombatValue, damageValue);
        }
        else
        {
            RemoveTarget(objectValue);
        }
    }

    // Combat.pas:5697
    public virtual void Initialize(Battle combat) => Combat = combat;

    // Combat.pas:5700
    public virtual void Finalize(FinalizeMode mode) => Dispose();

    // Combat.pas:5614
    public virtual int FastBattleRoundDistance() => 0;

    // Combat.pas:5605
    public virtual bool FastBattleExecute(int moveCost) => false;

    // Combat.pas:5608
    public virtual bool FastBattleBehindWall() => false;

    // Combat.pas:5611
    public virtual bool FastBattleWallInBetween(int target) => false;

    // Combat.pas:5818
    public virtual bool SupportCombatMode(CombatModes modes) => false;

    // Combat.pas:5630
    public virtual WallCombatFeatures WallCombatFeatures() => Game.Combat.WallCombatFeatures.None;

    // Combat.pas:5703
    public virtual void PlaySfx(int sound)
    {
    }

    // Combat.pas:5654
    public virtual void NewTurn(byte combatPlayer)
    {
        if (combatPlayer == (byte)CombatPlayer)
        {
            Status &= ~CombatObjectStatuses.TurnDone;
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
    public virtual int DoDamage(CombatObject? target, int damage)
    {
        int result;
        if (damage != 0 && target.GetEnabled())
        {
            result = target.ExecuteDamage(damage);
            if (target.Status.Contains(CombatObjectStatuses.Destroyed) && !target.GetAbilityEnabled(129) && !target.GetAbilityEnabled(128))
            {
                damage = Experience;
                damage += target.Level;
                SetExperience(damage);
            }
        }
        else
        {
            return 0;
        }

        return result;
    }

    // Combat.pas:5821
    public virtual DamageType GetOptimalDamageType(DamageTypes types)
    {
        DamageType result = DamageType.NonSpecific;
        DamageTypes available = types & ~GetImmunityTypes();
        if (available != DamageTypes.None)
        {
            DamageTypes protection = GetProtectionTypes();
            if (protection == DamageTypes.None || (available & ~protection) == DamageTypes.None)
            {
                while (!available.Contains(result.ToFlag()))
                {
                    result = (DamageType)((int)result - 1);
                }
            }
            else
            {
                while (!available.Contains(result.ToFlag()) || protection.Contains(result.ToFlag()))
                {
                    result = (DamageType)((int)result - 1);
                }
            }
        }

        return result;
    }

    // Combat.pas:5836
    public virtual int ExecuteDamageRole(int attack, int damage, DamageTypes damageTypes)
    {
        int result = 0;
        DamageTypes types = damageTypes & ~GetImmunityTypes();
        if (types != DamageTypes.None)
        {
            int attackLocal = attack - GetDefense();
            result = AbilityRules.ExecuteDamageRole(damage, attackDefense: attackLocal);
            if ((types & ~GetProtectionTypes()) == DamageTypes.None)
            {
                return (result + 1) / 2;
            }
        }

        return result;
    }

    // Combat.pas:5849
    public virtual int ExecuteDamageRole(int attack, int damage, DamageTypes damageTypes, int damageMode)
    {
        int result = 0;
        DamageTypes types = damageTypes & ~GetImmunityTypes();
        if (types != DamageTypes.None)
        {
            int attackLocal = damageMode == 1 ? attack - GetResistance() : attack - GetDefense();

            result = AbilityRules.ExecuteDamageRole(damage, attackDefense: attackLocal);
            if ((types & ~GetProtectionTypes()) == DamageTypes.None)
            {
                return (result + 1) / 2;
            }
        }

        return result;
    }

    // Combat.pas:5865
    public virtual DamageTypes ExecuteDamageEffectsRole(DamageTypes damageTypes) => DamageTypes.None;

    // Combat.pas:5895
    public virtual void Surrender()
    {
        SetHitPoints(0);
        Status |= CombatObjectStatuses.Destroyed;
        ResetTargets();
        Combat.ObjectDestroyed(this);
    }

    // Combat.pas:5881
    public virtual void DestroyObject()
    {
        SetHitPoints(0);
        Status |= CombatObjectStatuses.Destroyed;
        for (int i = GetAbilityCount() - 1; i >= 0; i--)
        {
            if (GetAbilityEnabled(i))
            {
                GamePack.Current.AbilityControl[i].CombatObjectDestroyed(this);
            }
        }

        if (Status.Contains(CombatObjectStatuses.Destroyed))
        {
            ResetTargets();
            Combat.ObjectDestroyed(this);
        }
    }

    // Combat.pas:5903
    public virtual int ExecuteDamage(int damage)
    {
        Assert(damage is >= 0 and < 50, message: "Invalid Damage Value (TCombatUnit.ExecuteDamage)", file: "Combat.pas", line: 5982);
        int hits = HitPoints;
        int result = hits < damage ? hits : damage;

        SetHitPoints((hits - result).TruncateToByte());
        if (damage != 0)
        {
            Status |= CombatObjectStatuses.Damaged;
        }

        if (result != 0 && HitPoints == 0)
        {
            DestroyObject();
        }

        return result;
    }

    // Combat.pas:5862
    public virtual DamageTypes ExecuteDamageEffects(DamageTypes damageTypes) => DamageTypes.None;

    // Combat.pas:5663
    public virtual void TurnDone()
    {
        CombatObjectTurnDoneCombatAction a = new(null)
        {
            Holder = CreationHolder(),
            CombatObjectId = Id
        };
        Combat.ExecuteCombatAction(a);
        _ = a.Release();
    }

    // Combat.pas:5776
    public virtual string IdString() => "TCombatObject";

    // Combat.pas:1941
    public void AddTarget(CombatObject target, int combatValue, int wallCombatValue, int damageValue)
    {
        Assert(Targets.IndexOfItemA(target) == -1, message: "target already exists use update target", file: "Combat.pas", line: 5473);
        CombatValue += combatValue;
        _wallCombatValue += wallCombatValue;
        _ = Targets.AddABCD(target, combatValue, C: wallCombatValue, D: damageValue);
        _ = target.TargetedBy.AddABCD(this, combatValue, C: wallCombatValue, D: damageValue);
        target.IncomingCombatValue += combatValue;
        target.IncomingWallCombatValue += wallCombatValue;
        if (combatValue > 0)
        {
            _targetCount += 1;
            target.AttackerCount += 1;
        }

        if (wallCombatValue > 0)
        {
            _wallTargetCount += 1;
            target._wallAttackerCount += 1;
        }

        TargetsChanged();
        target.TargetsChanged();
    }

    // Combat.pas:5488
    public void DeleteTarget(int index)
    {
        if (index != -1)
        {
            CombatObject target = Targets.List[index].First;
            int combatValue = Targets.List[index].Second;
            int damageValue = Targets.List[index].Third;
            CombatValue -= combatValue;
            _wallCombatValue -= damageValue;
            Targets.Delete(index);
            if (combatValue > 0)
            {
                _targetCount -= 1;
            }

            if (damageValue > 0)
            {
                _wallTargetCount -= 1;
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
                    target._wallAttackerCount -= 1;
                }

                target.TargetsChanged();
            }
        }
    }

    // Combat.pas:5515
    public void RemoveTarget(CombatObject target) => DeleteTarget(Targets.IndexOfItemA(target));

    // Combat.pas:5518
    public void ResetTargets()
    {
        for (int i = Targets.Count - 1; i >= 0; i--)
        {
            DeleteTarget(i);
        }
    }

    // Combat.pas:5575
    public void SetupTargets()
    {
        CombatSideIndex opponentSide = OpponentSide;
        CombatSide side = Combat.Sides[(int)opponentSide];
        for (int i = side.Count - 1; i >= 0; i--)
        {
            CombatObject target = side.GetObjects(i);
            if (!target.Status.Contains(CombatObjectStatuses.Destroyed))
            {
                GetTargetCombatValueDamageValue(target, out int combatValue, out int damageValue);
                int wallCombatValue = Combat.CombatData.WallType != 0 ? GetTargetWallCombatValue(target) : 0;

                AddTarget(target, combatValue, wallCombatValue, damageValue);
            }
        }
    }

    // Combat.pas:5636
    public CombatSideIndex Side
    {
        get
        {
            sbyte player = CombatPlayer;
            return player != -1 ? (CombatSideIndex)Combat.CombatData.Players.GetPlayers(player).SideIndex : CombatSideIndex.Neutral;
        }
    }

    // Combat.pas:5645
    public CombatSideIndex OpponentSide
    {
        get
        {
            sbyte player = CombatPlayer;
            return player != -1 ? (CombatSideIndex)(Combat.CombatData.Players.GetPlayers(player).SideIndex ^ 1) : CombatSideIndex.Neutral;
        }
    }

    // Combat.pas:5797
    public sbyte GetPlayer()
    {
        byte player = (byte)CombatPlayer;
        return player != 255 ? Combat.CombatData.Players.GetPlayers((sbyte)player).Player : (sbyte)-1;
    }

    // Combat.pas:5805
    public sbyte PlayerIndependentRelation
    {
        get
        {
            byte player = (byte)CombatPlayer;
            return player != 255 ? (sbyte)Combat.CombatData.Players.GetPlayers((sbyte)player).IndependentRelation : (sbyte)-1;
        }
    }

    internal static DamageTypes MeleeDamageTypes(CombatObject? source)
    {
        return source is CombatUnit sourceUnit
            ? GamePack.Current.AbilityControl.GetAbility<StrikeAbility>(21).GetDamageTypes(sourceUnit.Troop)
            : DamageTypes.None;
    }

    internal static int StrikeDamageValue(CombatObject? source, CombatObject? target)
    {
        if (source.GetAbilityEnabled(21))
        {
            int attack = source.GetAttack();
            int damage = source.GetDamage();
            DamageTypes types = MeleeDamageTypes(source);
            bool condition = source.GetAbilityEnabled(146) || source.GetAbilityEnabled(160);
            if (condition)
            {
                Alignment alignment = target.GetAlignment();
                condition = alignment is Alignment.Evil or Alignment.PureEvil;
            }

            if (condition)
            {
                attack = (attack + 2).TruncateToSByte();
                damage = (damage + 2).TruncateToSByte();
            }

            bool test = source.GetAbilityEnabled(147) || source.GetAbilityEnabled(161);
            if (test)
            {
                Alignment alignment = target.GetAlignment();
                test = alignment is Alignment.PureGood or Alignment.Good;
            }

            if (test)
            {
                attack = (attack + 2).TruncateToSByte();
                damage = (damage + 2).TruncateToSByte();
            }

            if (source.GetAbilityEnabled(112) && target.GetAbilityEnabled(63))
            {
                attack = (attack + 3).TruncateToSByte();
                damage = (damage + 3).TruncateToSByte();
            }

            return AbilityRules.StatisticsToLimitedDamageValue(attack, damage, damageTypes: types, maximum: target.HitPoints, defense: target.GetDefense(), resistance: target.GetResistance(), immunityTypes: target.GetImmunityTypes(), protectionTypes: target.GetProtectionTypes());
        }
        else
        {
            return 0;
        }
    }

    internal static StrikeCombatAction? CreateStrikeCombatAction(CombatObject? source, CombatObject? target, bool defensive)
    {
        StrikeCombatAction result;
        if (source.GetAbilityEnabled(21))
        {
            int attack = source.GetAttack();
            int damage = source.GetDamage();
            DamageTypes types = MeleeDamageTypes(source);
            bool condition = source.GetAbilityEnabled(146) || source.GetAbilityEnabled(160);
            if (condition)
            {
                Alignment alignment = target.GetAlignment();
                condition = alignment is Alignment.Evil or Alignment.PureEvil;
            }

            if (condition)
            {
                attack = (attack + 2).TruncateToSByte();
                damage = (damage + 2).TruncateToSByte();
            }

            bool test = source.GetAbilityEnabled(147) || source.GetAbilityEnabled(161);
            if (test)
            {
                Alignment alignment = target.GetAlignment();
                test = alignment is Alignment.PureGood or Alignment.Good;
            }

            if (test)
            {
                attack = (attack + 2).TruncateToSByte();
                damage = (damage + 2).TruncateToSByte();
            }

            if (source.GetAbilityEnabled(112) && target.GetAbilityEnabled(63))
            {
                attack = (attack + 3).TruncateToSByte();
                damage = (damage + 3).TruncateToSByte();
            }

            result = new StrikeCombatAction(null) { Holder = source };
            result.Generate(source, target, attack, damage, damageTypes: types);
            result.SetDefensive(defensive);
        }
        else
        {
            return null;
        }

        return result;
    }
}
