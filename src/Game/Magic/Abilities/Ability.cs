// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Buffers;

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
        FDescription = new StringList();
        FSFXLibrary = new SfxLibrary(null);
        FImageSequences = new ImageSequenceList(AoWHSSet.Current.ImageLibraryManager, null);
    }

    public string FAbilityName = "";
    public int FAbilityID;
    public StringList FDescription;
    public int FSkillPointCost;
    public SfxLibrary FSFXLibrary;
    public ImageSequenceList FImageSequences;
    public Set<AbilitySelectionType> FSelectionTypes;

    // Ability.PAS:1245
    public override void Dispose()
    {
        FDescription?.Dispose();
        FSFXLibrary?.Dispose();
        FImageSequences?.Dispose();
        base.Dispose();
    }

    // Ability.PAS:1654
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FDescription, 5);
        Stream.rwInteger(ref FSkillPointCost, 6);
        Stream.rwEObject(FSFXLibrary, 7);
        Stream.rwStreamProc(8, FImageSequences.LoadFromStream, FImageSequences.SaveToStream);
        RwSet(ref FSelectionTypes, 2, 9, Stream.rwData, 0);
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
    public virtual string GetName(AbilityOwner Owner) => FAbilityName;

    // Ability.PAS:1453
    public virtual sbyte GetAttack(AbilityOwner Owner) => 0;

    // Ability.PAS:1457
    public virtual sbyte GetDefense(AbilityOwner Owner) => 0;

    // Ability.PAS:1462
    public virtual sbyte GetResistance(AbilityOwner Owner) => 0;

    // Ability.PAS:1467
    public virtual sbyte GetDamage(AbilityOwner Owner) => 0;

    // Ability.PAS:1472
    public virtual Set<MoveType> GetMoveTypes(AbilityOwner Owner) => P1EmptyMoveTypes;

    // Ability.PAS:1482
    public virtual int GetLevel(AbilityOwner Owner) => 0;

    // Ability.PAS:1550
    public virtual Set<DamageType> GetImmunityTypes(AbilityOwner Owner) => P1EmptyDamageTypes;

    // Ability.PAS:1555
    public virtual Set<DamageType> GetProtectionTypes(AbilityOwner Owner) => P1EmptyDamageTypes;

    // Ability.PAS:1268
    public virtual CombatMode GetCombatMode(AbilityOwner Owner) => 0;

    // Ability.PAS:1341
    public virtual Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner Owner) => P1EmptyWallFeatures;

    // Ability.PAS:1256
    public virtual string GetSourceName(AbilityOwner Owner) => Owner.GetOwnerName();

    // Ability.PAS:1420
    public virtual int GetInherentLevel(AbilityOwner Owner) => 0;

    // Ability.PAS:1264
    public virtual Set<AbilityControlTypeElement> GetControlType(AbilityOwner Owner) => P1EmptyControlType;

    // Ability.PAS:1298
    // { Original leaves Reserved untouched. }
    public virtual bool fcValidRoundDistance(CombatObject Source, CombatObject Target)
    {
        int Distance;
        bool Value;
        Distance = Source.FCombat.GetRoundDistance();
        if (Source.GetMoves() >= 32)
        {
            Distance += 2;
        }

        Value = Distance >= Target.fcRoundDistance();
        if (Value && Source is CombatUnit && ((CombatUnit)Source).FUnit is Hero)
        {
            Value = Value && ((((byte)Source.FStatus.Image) & 16) != 0 || Source.fcRoundDistance() <= Source.FCombat.GetRoundDistance() + 2);
        }

        return Value;
    }

    // Ability.PAS:1571
    public virtual void fcPrefetchCombatCommands(FastBattleCommandList Commands)
    {
    }

    // Ability.PAS:1574
    public virtual bool fcExecuteCombatCommand(CombatObject Source, FastBattleCommand Command) => false;

    // Ability.PAS:1582
    public virtual void NewDay(AbilityOwner Owner)
    {
    }

    // Ability.PAS:1585
    public virtual void NewTurn(AbilityOwner Owner, sbyte Player)
    {
    }

    // Ability.PAS:1594
    public virtual bool CanActivate(AbilityOwner Owner, ref string Reason)
    {
        Reason = "";
        return GetControlType(Owner).Contains(AbilityControlTypeElement.Action);
    }

    // Ability.PAS:1606
    public virtual bool Activate(AbilityOwner Owner, ref string Reason)
    {
        bool Result;
        AbilityActivation Info;
        // { The original reports successful activation after releasing the notification. }
        if (CanActivate(Owner, ref Reason))
        {
            Info = new AbilityActivation(null);
            Info.FAbilityID = FAbilityID;
            if (Owner is Troop)
            {
                Info.FUnitID = ((Troop)Owner).FUnitID;
            }
            else
            {
                Info.FUnitID = 0;
            }

            AoWHSMap.Current.FOnActivateAbilityStore.Raise(Info);
            Info.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Ability.PAS:1624
    public virtual bool CanActivateCombat(AbilityOwner Owner, ref string Reason)
    {
        Reason = "";
        return GetControlType(Owner).Contains(AbilityControlTypeElement.CombatAction);
    }

    // Ability.PAS:1637
    public virtual bool ActivateCombat(AbilityOwner Owner, ref string Reason)
    {
        bool Result;
        AbilityActivation Info;
        if (this.CanActivateCombat(Owner, ref Reason))
        {
            Info = new AbilityActivation(null);
            Info.FAbilityID = FAbilityID;
            if (Owner is Troop)
            {
                Info.FUnitID = ((Troop)Owner).FUnitID;
            }
            else
            {
                Info.FUnitID = 0;
            }

            AoWHSMap.Current.FCombatMap.FAbilityActivatedEventsStore.Raise(Info);
            Info.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Ability.PAS:1560
    public virtual void ListSpells()
    {
    }

    // Ability.PAS:1374
    public virtual string ExpandName(AbilityOwner Owner) => FAbilityName;

    // Ability.PAS:1382
    public virtual bool Expand(AbilityOwner Owner)
    {
        Owner.SetAbSet(FAbilityID, 1);
        return true;
    }

    // Ability.PAS:1272
    public virtual int GetDamageValue(Troop Attacker, Troop Defender) => 0;

    // Ability.PAS:1276
    public virtual void GetDamageValueEx(Troop Attacker, Troop Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        DamageValues.SourceDamage = 0;
        DamageValues.SourceCost = 0;
        DamageValues.TargetDamage = 0;
        DamageValues.TargetCost = 0;
    }

    // Ability.PAS:1487
    public virtual void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        Strength = 0;
        Types = P1EmptyDamageTypes;
    }

    // Ability.PAS:1287
    // { Original leaves the value at offset 16 untouched. }
    public virtual void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        DamageValues.SourceDamage = 0;
        DamageValues.SourceCost = 0;
        DamageValues.TargetDamage = 0;
        DamageValues.TargetCost = 0;
    }

    // Ability.PAS:1313
    public virtual int fcGetDamageValue(CombatObject Source, CombatObject Target, bool Defensive)
    {
        AbilityDamageValues Values = default;
        fcGetDamageValueEx(Source, Target, (WallSituation)(Defensive ? 1 : 0), ref Values);
        return Values.SourceDamage;
    }

    // Ability.PAS:1320
    public virtual void tcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, ref AbilityDamageValues DamageValues)
    {
        fcGetDamageValueEx(Attacker, Defender, WallSituation.None, ref DamageValues);
    }

    // Ability.PAS:1324
    public virtual int tcGetDamageValue(CombatObject Source, CombatObject Target)
    {
        AbilityDamageValues Values = default;
        this.tcGetDamageValueEx(Source, Target, ref Values);
        return Values.SourceDamage;
    }

    // Ability.PAS:1568
    public virtual void CombatObjectDestroyed(CombatObject CombatObject)
    {
    }

    // Ability.PAS:1588
    public virtual void NewCombatTurn(CombatObject CombatObject, sbyte Player)
    {
    }

    // Ability.PAS:1591
    public virtual void CombatDone(CombatObject CombatObject)
    {
    }

    // Ability.PAS:1492
    public virtual void ListInfo(StringCollection Text, Set<AbilityInfoModeElement> Mode)
    {
        AbilityCombatInfo Info = default;
        Set<DamageType> Types = default;
        int Strength = default;
        string DamageNames;
        DamageType DamageType_;
        Text.Add("Name: " + this.GetName(null));
        Text.Add("-------------------------------------------------");
        if (Mode.Contains(AbilityInfoModeElement.ID))
        {
            Text.Add("ID: " + IntToStr(FAbilityID));
        }

        var abilityType = this.GetAbilityType();
        Text.Add("Type: " + AbilityRules.TranslateRStr(GameTables.TAbilityTypeRStr[(int)abilityType]));
        if (this.GetAttack(null) != 0)
        {
            Text.Add("Attack modifier: " + IntToStr(this.GetAttack(null)));
        }

        if (this.GetDefense(null) != 0)
        {
            Text.Add("Defense modifier: " + IntToStr(this.GetDefense(null)));
        }

        if (this.GetDamage(null) != 0)
        {
            Text.Add("Damage modifier: " + IntToStr(this.GetDamage(null)));
        }

        if (this.GetResistance(null) != 0)
        {
            Text.Add("Resistance modifier: " + IntToStr(this.GetResistance(null)));
        }

        if (Mode.Contains(AbilityInfoModeElement.Description))
        {
            Text.Add("Description: ");
            Text.AddStrings(FDescription);
        }

        if (this.GetCombatMode(null) != CombatMode.None)
        {
            var combatMode = this.GetCombatMode(null);
            Text.Add("Combat mode: " + AbilityRules.TranslateRStr(TCombatModeRStr[(int)combatMode]));
        }

        if (Mode.Contains(AbilityInfoModeElement.Statistics))
        {
            this.GetCombatInfo(null, ref Info);
            if (Info.Attack != -1)
            {
                Text.Add("Attack: " + IntToStr(Info.Attack));
            }

            if (Info.Damage != -1)
            {
                Text.Add("Damage: " + IntToStr(Info.Damage));
            }

            if (Info.RepeatCount != -1)
            {
                Text.Add("Repeat: " + IntToStr(Info.RepeatCount));
            }

            if (Info.DamageTypes != Set<DamageType>.Of())
            {
                DamageNames = "";
                for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
                {
                    DamageType_ = (DamageType)damageTypeOrdinal;
                    if (Info.DamageTypes.Contains(DamageType_))
                    {
                        if (!IsEmpty(DamageNames))
                        {
                            DamageNames = DamageNames + ",";
                        }

                        DamageNames = DamageNames + AbilityRules.TranslateRStr(TDamageTypeRStr[(int)DamageType_]);
                    }
                }

                Text.Add("Damage Types: " + DamageNames);
            }
        }

        if (Mode.Contains(AbilityInfoModeElement.Strength))
        {
            this.GetOffensiveStrength(null, ref Strength, ref Types);
            if (Strength != 0)
            {
                Text.Add("Strength: " + IntToStr(Strength));
            }
        }
    }

    // Ability.PAS:1331
    public virtual void GetCombatInfo(AbilityOwner Owner, ref AbilityCombatInfo Info)
    {
        ref var abilityCombatInfo = ref Info;
        abilityCombatInfo.Attack = -1;
        abilityCombatInfo.Damage = -1;
        abilityCombatInfo.DamageTypes = Set<DamageType>.Of();
        abilityCombatInfo.RepeatCount = -1;
    }

    // Ability.PAS:1252
    // class receiver
    public void ShowInfo(AbilityOwner Owner, ImageLibraryFont Font, int X, int Y, in LockedSurface Surface)
    {
        Font.SetText(X, Y, this.GetName(Owner), Surface);
    }

    // Ability.PAS:1260
    public void SetAb(AbilityOwner Owner, int Index, byte Value)
    {
        Owner.SetAbSet(Index, Value);
    }

    // Ability.PAS:1445
    public virtual EngineClass AbilityDataClass() => default;

    public virtual bool CanExpand(AbilityOwner owner) => !owner.GetAbSet(FAbilityID) && (owner.GetAbilitySelectionTypes() & FSelectionTypes) != default(Set<AbilitySelectionType>);
    public virtual int ExpandCost(AbilityOwner owner) => FSkillPointCost;

    public virtual bool Remove(AbilityOwner owner)
    {
        if (!owner.GetAbSet(FAbilityID)) return false;
        owner.SetAbSet(FAbilityID, 0);
        return true;
    }

    public virtual bool GetInherent(AbilityOwner owner) => owner.GetAbSet(FAbilityID);
    public virtual int GetSkillPoints(AbilityOwner owner) => owner.GetAbSet(FAbilityID) ? FSkillPointCost : 0;
    public virtual bool GetEnabled(AbilityOwner owner) => owner.GetAbSet(FAbilityID);

    private static StringArray6 TCombatModeRStr = StringArray6.From(default(ReadOnlySpan<string>));
    private static StringArray10 TDamageTypeRStr = StringArray10.From(default(ReadOnlySpan<string>));
    private static Set<MoveType> P1EmptyMoveTypes = Set<MoveType>.FromMask(0uL);
    private static Set<DamageType> P1EmptyDamageTypes = Set<DamageType>.FromMask(0uL);
    private static Set<WallCombatFeaturesElement> P1EmptyWallFeatures = Set<WallCombatFeaturesElement>.FromMask(0uL);
    private static Set<AbilityControlTypeElement> P1EmptyControlType = Set<AbilityControlTypeElement>.FromMask(0uL);
}
