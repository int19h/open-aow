// Translated from AoWE.pas.
using System.Collections.Immutable;
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;
using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

// name, ID, description, cost, Sfx library, image sequences, selection types. Procedure
// return-ABI adapters preserve unmarked overrides in other packages
public partial class Ability : EngineObject
{

    // Ability.PAS:1238
    public Ability(ECustomNode owner) : base(owner)
    {
        Description = new StringList();
        SfxLibrary = new SfxLibrary(null);
        ImageSequences = new ImageSequenceList(GamePack.Current.ImageLibraryManager, null);
    }

    public string AbilityName { get; set; } = "";
    public int AbilityId { get; set; }
    public StringList Description { get; }

    public int SkillPointCost;

    public SfxLibrary SfxLibrary { get; }
    public ImageSequenceList ImageSequences { get; init; }

    public Set<AbilitySelectionType> SelectionTypes;

    // Ability.PAS:1245
    public override void Dispose()
    {
        Description?.Dispose();
        SfxLibrary?.Dispose();
        ImageSequences?.Dispose();
        base.Dispose();
    }

    // Ability.PAS:1654
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Lines(Description, 5);
        stream.Int32(ref SkillPointCost, 6);
        stream.Object(SfxLibrary, 7);
        stream.Payload(8, ImageSequences.LoadFromStream, ImageSequences.SaveToStream);
        TransferSet(ref SelectionTypes, 2, 9, stream.Bytes, 0);
    }

    // Ability.PAS:1579
    public virtual void Execute()
    {
    }

    // Ability.PAS:1477
    public virtual bool GetHidden() => false;

    // Ability.PAS:1563
    public virtual AbilityType GetAbilityType() => 0;

    // Ability.PAS:1449
    public virtual string GetName(AbilityOwner owner) => AbilityName;

    // Ability.PAS:1453
    public virtual sbyte GetAttack(AbilityOwner owner) => 0;

    // Ability.PAS:1457
    public virtual sbyte GetDefense(AbilityOwner owner) => 0;

    // Ability.PAS:1462
    public virtual sbyte GetResistance(AbilityOwner owner) => 0;

    // Ability.PAS:1467
    public virtual sbyte GetDamage(AbilityOwner owner) => 0;

    // Ability.PAS:1472
    public virtual Set<MoveType> GetMoveTypes(AbilityOwner owner) => s_emptyMoveTypes;

    // Ability.PAS:1482
    public virtual int GetLevel(AbilityOwner owner) => 0;

    // Ability.PAS:1550
    public virtual Set<DamageType> GetImmunityTypes(AbilityOwner owner) => s_emptyDamageTypes;

    // Ability.PAS:1555
    public virtual Set<DamageType> GetProtectionTypes(AbilityOwner owner) => s_emptyDamageTypes;

    // Ability.PAS:1268
    public virtual CombatMode GetCombatMode(AbilityOwner owner) => 0;

    // Ability.PAS:1341
    public virtual Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner owner) => s_emptyWallFeatures;

    // Ability.PAS:1256
    public virtual string GetSourceName(AbilityOwner owner) => owner.GetOwnerName();

    // Ability.PAS:1420
    public virtual int GetInherentLevel(AbilityOwner owner) => 0;

    // Ability.PAS:1264
    public virtual Set<AbilityControlTypeElement> GetControlType(AbilityOwner owner) => s_emptyControlType;

    // Ability.PAS:1298
    // { Original leaves Reserved untouched. }
    public virtual bool FastBattleValidRoundDistance(CombatObject source, CombatObject target)
    {
        int distance;
        bool value;
        distance = source.Combat.GetRoundDistance();
        if (source.GetMoves() >= 32)
        {
            distance += 2;
        }

        value = distance >= target.FastBattleRoundDistance();
        if (value && source is CombatUnit && ((CombatUnit)source).Troop is Hero)
        {
            value = value && ((((byte)source.Status.Image) & 16) != 0 || source.FastBattleRoundDistance() <= source.Combat.GetRoundDistance() + 2);
        }

        return value;
    }

    // Ability.PAS:1571
    public virtual void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
    }

    // Ability.PAS:1574
    public virtual bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command) => false;

    // Ability.PAS:1582
    public virtual void NewDay(AbilityOwner owner)
    {
    }

    // Ability.PAS:1585
    public virtual void NewTurn(AbilityOwner owner, sbyte player)
    {
    }

    // Ability.PAS:1594
    public virtual bool CanActivate(AbilityOwner owner, ref string reason)
    {
        reason = "";
        return GetControlType(owner).Contains(AbilityControlTypeElement.Action);
    }

    // Ability.PAS:1606
    public virtual bool Activate(AbilityOwner owner, ref string reason)
    {
        bool result;
        AbilityActivation info;
        // { The original reports successful activation after releasing the notification. }
        if (CanActivate(owner, ref reason))
        {
            info = new AbilityActivation(null);
            info.AbilityId = AbilityId;
            if (owner is Troop)
            {
                info.UnitId = ((Troop)owner).UnitId;
            }
            else
            {
                info.UnitId = 0;
            }

            StrategicMap.Current.OnActivateAbilityStore.Raise(info);
            info.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Ability.PAS:1624
    public virtual bool CanActivateCombat(AbilityOwner owner, out string reason)
    {
        reason = "";
        return GetControlType(owner).Contains(AbilityControlTypeElement.CombatAction);
    }

    // Ability.PAS:1637
    public virtual bool ActivateCombat(AbilityOwner owner, out string reason)
    {
        bool result;
        AbilityActivation info;
        if (this.CanActivateCombat(owner, out reason))
        {
            info = new AbilityActivation(null);
            info.AbilityId = AbilityId;
            if (owner is Troop)
            {
                info.UnitId = ((Troop)owner).UnitId;
            }
            else
            {
                info.UnitId = 0;
            }

            StrategicMap.Current.CombatMap.AbilityActivatedEventsStore.Raise(info);
            info.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Ability.PAS:1560
    public virtual void ListSpells()
    {
    }

    // Ability.PAS:1374
    public virtual string ExpandName(AbilityOwner owner) => AbilityName;

    // Ability.PAS:1382
    public virtual bool Expand(AbilityOwner owner)
    {
        owner.SetAbilitySet(AbilityId, 1);
        return true;
    }

    // Ability.PAS:1272
    public virtual int GetDamageValue(Troop attacker, Troop defender) => 0;

    // Ability.PAS:1276
    public virtual void GetDamageValues(Troop attacker, Troop defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        damageValues.SourceDamage = 0;
        damageValues.SourceCost = 0;
        damageValues.TargetDamage = 0;
        damageValues.TargetCost = 0;
    }

    // Ability.PAS:1487
    public virtual void GetOffensiveStrength(AbilityOwner owner, ref int strength, ref Set<DamageType> types)
    {
        strength = 0;
        types = s_emptyDamageTypes;
    }

    // Ability.PAS:1287
    // { Original leaves the value at offset 16 untouched. }
    public virtual void FastBattleGetDamageValues(CombatObject attacker, CombatObject defender, WallSituation wallSituation, ref AbilityDamageValues damageValues)
    {
        damageValues.SourceDamage = 0;
        damageValues.SourceCost = 0;
        damageValues.TargetDamage = 0;
        damageValues.TargetCost = 0;
    }

    // Ability.PAS:1313
    public virtual int FastBattleGetDamageValue(CombatObject source, CombatObject target, bool defensive)
    {
        AbilityDamageValues values = default;
        FastBattleGetDamageValues(source, target, (WallSituation)(defensive ? 1 : 0), ref values);
        return values.SourceDamage;
    }

    // Ability.PAS:1320
    public virtual void TacticalBattleGetDamageValues(CombatObject attacker, CombatObject defender, ref AbilityDamageValues damageValues)
    {
        FastBattleGetDamageValues(attacker, defender, WallSituation.None, ref damageValues);
    }

    // Ability.PAS:1324
    public virtual int TacticalBattleGetDamageValue(CombatObject source, CombatObject target)
    {
        AbilityDamageValues values = default;
        this.TacticalBattleGetDamageValues(source, target, ref values);
        return values.SourceDamage;
    }

    // Ability.PAS:1568
    public virtual void CombatObjectDestroyed(CombatObject combatObject)
    {
    }

    // Ability.PAS:1588
    public virtual void NewCombatTurn(CombatObject combatObject, sbyte player)
    {
    }

    // Ability.PAS:1591
    public virtual void CombatDone(CombatObject combatObject)
    {
    }

    // Ability.PAS:1492
    public virtual void ListInfo(StringCollection text, Set<AbilityInfoModeElement> mode)
    {
        AbilityCombatInfo info = default;
        Set<DamageType> types = default;
        int strength = default;
        string damageNames;
        DamageType damageType;
        text.Add("Name: " + this.GetName(null));
        text.Add("-------------------------------------------------");
        if (mode.Contains(AbilityInfoModeElement.Id))
        {
            text.Add("ID: " + IntToStr(AbilityId));
        }

        var abilityType = this.GetAbilityType();
        text.Add("Type: " + AbilityRules.TranslateResourceString(GameTables.AbilityTypeTexts[(int)abilityType]));
        if (this.GetAttack(null) != 0)
        {
            text.Add("Attack modifier: " + IntToStr(this.GetAttack(null)));
        }

        if (this.GetDefense(null) != 0)
        {
            text.Add("Defense modifier: " + IntToStr(this.GetDefense(null)));
        }

        if (this.GetDamage(null) != 0)
        {
            text.Add("Damage modifier: " + IntToStr(this.GetDamage(null)));
        }

        if (this.GetResistance(null) != 0)
        {
            text.Add("Resistance modifier: " + IntToStr(this.GetResistance(null)));
        }

        if (mode.Contains(AbilityInfoModeElement.Description))
        {
            text.Add("Description: ");
            text.AddStrings(Description);
        }

        if (this.GetCombatMode(null) != CombatMode.None)
        {
            var combatMode = this.GetCombatMode(null);
            text.Add("Combat mode: " + AbilityRules.TranslateResourceString(s_combatModeTexts[(int)combatMode]));
        }

        if (mode.Contains(AbilityInfoModeElement.Statistics))
        {
            this.GetCombatInfo(null, ref info);
            if (info.Attack != -1)
            {
                text.Add("Attack: " + IntToStr(info.Attack));
            }

            if (info.Damage != -1)
            {
                text.Add("Damage: " + IntToStr(info.Damage));
            }

            if (info.RepeatCount != -1)
            {
                text.Add("Repeat: " + IntToStr(info.RepeatCount));
            }

            if (info.DamageTypes != Set<DamageType>.Of())
            {
                damageNames = "";
                for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
                {
                    damageType = (DamageType)damageTypeOrdinal;
                    if (info.DamageTypes.Contains(damageType))
                    {
                        if (!IsEmpty(damageNames))
                        {
                            damageNames = damageNames + ",";
                        }

                        damageNames = damageNames + AbilityRules.TranslateResourceString(s_damageTypeTexts[(int)damageType]);
                    }
                }

                text.Add("Damage Types: " + damageNames);
            }
        }

        if (mode.Contains(AbilityInfoModeElement.Strength))
        {
            this.GetOffensiveStrength(null, ref strength, ref types);
            if (strength != 0)
            {
                text.Add("Strength: " + IntToStr(strength));
            }
        }
    }

    // Ability.PAS:1331
    public virtual void GetCombatInfo(AbilityOwner owner, ref AbilityCombatInfo info)
    {
        ref var abilityCombatInfo = ref info;
        abilityCombatInfo.Attack = -1;
        abilityCombatInfo.Damage = -1;
        abilityCombatInfo.DamageTypes = Set<DamageType>.Of();
        abilityCombatInfo.RepeatCount = -1;
    }

    // Ability.PAS:1252
    // class receiver
    public void ShowInfo(AbilityOwner owner, ImageLibraryFont font, int X, int Y, in LockedSurface surface)
    {
        font.SetText(X, Y, this.GetName(owner), surface);
    }

    // Ability.PAS:1260
    public void SetAbility(AbilityOwner owner, int index, byte value)
    {
        owner.SetAbilitySet(index, value);
    }

    // Ability.PAS:1445
    public virtual EngineClass AbilityDataClass() => default;

    public virtual bool CanExpand(AbilityOwner owner) => !owner.GetAbilitySetAt(AbilityId) && (owner.GetAbilitySelectionTypes() & SelectionTypes) != default(Set<AbilitySelectionType>);
    public virtual int ExpandCost(AbilityOwner owner) => SkillPointCost;

    public virtual bool Remove(AbilityOwner owner)
    {
        if (!owner.GetAbilitySetAt(AbilityId)) return false;
        owner.SetAbilitySet(AbilityId, 0);
        return true;
    }

    public virtual bool GetInherent(AbilityOwner owner) => owner.GetAbilitySetAt(AbilityId);
    public virtual int GetSkillPoints(AbilityOwner owner) => owner.GetAbilitySetAt(AbilityId) ? SkillPointCost : 0;
    public virtual bool GetEnabled(AbilityOwner owner) => owner.GetAbilitySetAt(AbilityId);

    private static readonly ImmutableArray<string> s_combatModeTexts = ["", "", "", "", "", ""];
    private static readonly ImmutableArray<string> s_damageTypeTexts = ["", "", "", "", "", "", "", "", "", ""];
    private static readonly Set<MoveType> s_emptyMoveTypes = Set<MoveType>.FromMask(0uL);
    private static readonly Set<DamageType> s_emptyDamageTypes = Set<DamageType>.FromMask(0uL);
    private static readonly Set<WallCombatFeaturesElement> s_emptyWallFeatures = Set<WallCombatFeaturesElement>.FromMask(0uL);
    private static readonly Set<AbilityControlTypeElement> s_emptyControlType = Set<AbilityControlTypeElement>.FromMask(0uL);
}
