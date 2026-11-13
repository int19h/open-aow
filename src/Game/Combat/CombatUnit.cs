// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// Troop. Party. Disabled. WallCombatFeaturesValue. UnitGraphicsResourceIndex. Inherited: FID.
// FPosition. FStatus. FControlMode
public partial class CombatUnit : CombatObject
{
    // Combat.pas:1084
    public new const int ClassId = 131745;

    // Combat.pas:1071
    public CombatUnit(ECustomNode owner) : base(owner)
    {
        ControlMode = Set<CombatObjectControlModeElement>.Of(CombatObjectControlModeElement.ConquerObject);
        _unitGraphicsResourceIndex = -1;
    }

    public Troop Troop;
    private CombatParty _party;
    public bool Disabled;

    public Set<WallCombatFeaturesElement> WallCombatFeaturesValue { get; private set; }

    private int _unitGraphicsResourceIndex;

    // Combat.pas:1078
    public override void Dispose()
    {
        SetUnit(null);
        base.Dispose();
    }

    // Combat.pas:1382
    public override void ReadWrite(StorageStream stream)
    {
        Troop U = default;
        base.ReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.Translate)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            stream.Created(ref U, null, 20);
            SetUnit(U);
            if (U != null)
            {
                U.Release();
            }
        }
        else
        {
            stream.Created(ref Troop, null, 20);
        }

        stream.Boolean(ref Disabled, 21);
    }

    public override void PersistFields(IPersister persister)
    {
        Troop U = default;
        base.PersistFields(persister);
        if (persister.Loading)
        {
            U = persister.Created("Troop", objectValue: U);
            SetUnit(U);
            if (U != null)
            {
                U.Release();
            }
        }
        else
        {
            Troop = persister.Created("Troop", objectValue: Troop);
        }

        Disabled = persister.Boolean("Disabled", value: Disabled);
    }

    // Combat.pas:1408
    public override void GetTargetCombatValueDamageValue(CombatObject target, out int combatValue, out int damageValue)
    {
        Troop U;
        AbilityTypeList abilities;
        AbilityDamageValues values = default;
        int V;
        Ability ability;
        if (!Status.Contains(CombatObjectStatusElement.Destroyed))
        {
            U = Troop;
            combatValue = 0;
            damageValue = 0;
            abilities = GamePack.Current.AbilityControl.AbilitiesByType[(int)AbilityType.Combat];
            for (int I = abilities.Count - 1; I >= 0; I--)
            {
                ability = abilities.GetAbility(I);
                if (U.GetAbilityEnabled(ability.AbilityId))
                {
                    ability.TacticalBattleGetDamageValues(this, target, ref values);
                    if (values.SourceDamage > 0)
                    {
                        V = values.SourceDamage * values.SourceDamage / (values.TargetDamage + values.SourceDamage);
                        if (V <= 0)
                        {
                            V = 1;
                        }

                        if (V > combatValue)
                        {
                            combatValue = V;
                        }

                        if (values.SourceDamage > damageValue)
                        {
                            damageValue = values.SourceDamage;
                        }
                    }
                }
            }

            if (U.GetAbilityEnabled(52))
            {
                ability = GamePack.Current.AbilityControl.GetAbility(52);
                ability.TacticalBattleGetDamageValues(this, target, ref values);
                if (values.SourceDamage > 0)
                {
                    V = values.SourceDamage * values.SourceDamage / (values.TargetDamage + values.SourceDamage);
                    if (V <= 0)
                    {
                        V = 1;
                    }

                    if (V > combatValue)
                    {
                        combatValue = V;
                    }

                    if (values.SourceDamage > damageValue)
                    {
                        damageValue = values.SourceDamage;
                    }
                }
            }
        }
        else
        {
            combatValue = 0;
            damageValue = 0;
        }
    }

    // Combat.pas:1450
    public override int GetTargetWallCombatValue(CombatObject target)
    {
        int result;
        if (target is CombatUnit && !target.Status.Contains(CombatObjectStatusElement.Destroyed))
        {
            if (Combat.CombatData.Players.GetPlayers(CombatPlayer).SideIndex == 1)
            {
                result = Troop.GetCombatValue(((CombatUnit)target).Troop, 1);
            }
            else
            {
                result = Troop.GetCombatValue(((CombatUnit)target).Troop, 2);
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Combat.pas:1554
    public override int GetTargetPriority()
    {
        if (TargetPriorityInvalid)
        {
            base.GetTargetPriority();
            var commandingTargetPriority = CommandingTargetPriority();
            TargetPriority += commandingTargetPriority;
            return TargetPriority;
        }

        return TargetPriority;
    }

    // Combat.pas:1566
    public override int GetTargetStrength()
    {
        if (TargetStrengthInvalid)
        {
            base.GetTargetStrength();
            var commandingTargetStrength = CommandingTargetStrength();
            TargetStrength += commandingTargetStrength;
            return TargetStrength;
        }

        return TargetStrength;
    }

    // Combat.pas:1136
    public override bool GetEnabled() => Troop != null && (Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed, CombatObjectStatusElement.Fled) & Status) == Set<CombatObjectStatusElement>.Of() && !Disabled;

    // Combat.pas:1142
    public override bool GetLocked() => Troop.GetAbilityEnabled(95) || Troop.GetAbilityEnabled(97) || Troop.GetAbilityEnabled(92) || Troop.GetAbilityEnabled(94) || Troop.GetAbilityEnabled(34);

    // Combat.pas:1459
    public override sbyte GetAttack() => Troop.GetAttack();

    // Combat.pas:1462
    public override sbyte GetDefense() => Troop.GetDefense();

    // Combat.pas:1465
    public override sbyte GetResistance() => Troop.GetResistance();

    // Combat.pas:1468
    public override sbyte GetDamage() => Troop.GetDamage();

    // Combat.pas:1471
    public override Set<DamageType> GetImmunityTypes() => Troop.GetImmunityTypes() + Combat.CombatData.ImmunityTypes;

    // Combat.pas:1474
    public override Set<DamageType> GetProtectionTypes() => Troop.GetProtectionTypes();

    // Combat.pas:1282
    public override sbyte GetHits()
    {
        sbyte result;
        // { Direct field test retains the receiver. }
        // { No cast temporary. }
        if (Troop != null)
        {
            result = Troop.GetHits();
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Combat.pas:1289
    public override sbyte GetHitPoints()
    {
        sbyte result;
        // { Direct field test retains the receiver. }
        // { No cast temporary. }
        if (Troop != null)
        {
            result = Troop.GetHitPoints();
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Combat.pas:1296
    public override void SetHitPoints(byte value)
    {
        // { Direct nil guard. }
        // { No cast temporary. }
        if (Troop != null)
        {
            Troop.SetHitPoints((sbyte)value);
        }
    }

    // Combat.pas:1260
    public override Alignment GetAlignment() => Troop.GetAlignment();

    // Combat.pas:1257
    public override int GetMoves() => Troop.GetMoves();

    // Combat.pas:1477
    public override int GetExperience() => Troop.GetExperience();

    // Combat.pas:1480
    public override void SetExperience(int value)
    {
        Troop.SetExperience(value);
    }

    // Combat.pas:1483
    public override byte GetLevel() => (byte)Troop.GetUnitLevel();

    // Combat.pas:1095
    public override int GetVisibilityRange()
    {
        int result;
        int level;
        Troop U;
        U = Troop;
        if (U != null)
        {
            level = U.GetAbilityLevel(64);
            if (level != 0)
            {
                result = level * 2 + 4;
            }
            else
            {
                result = 4;
            }

            if (Combat.ExplorationSite == 1 && !Troop.GetAbilityEnabled(39))
            {
                result = (result + 1) / 2;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Combat.pas:1266
    public override bool GetAbilityEnabled(int abilityId)
    {
        bool result;
        if (Troop != null)
        {
            result = Troop.GetAbilityEnabled(abilityId);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Combat.pas:1271
    public override void SetAbilityEnabled(int abilityId, bool enabled)
    {
        Troop.SetAbilitySet(abilityId, (byte)(enabled ? 1 : 0));
    }

    // Combat.pas:1274
    public override int GetAbilityLevel(int abilityId) => Troop.GetAbilityLevel(abilityId);

    // Combat.pas:1277
    public override int GetAbilityCount()
    {
        int result;
        if (Troop != null)
        {
            result = Troop.GetAbilityCount();
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Combat.pas:1263
    public override AbilityOwner GetAbilityOwner() => Troop;

    // Combat.pas:1178
    public override void Activate(Battle combat)
    {
        base.Activate(combat);
        UpdateParty();
        UpdateWallCombatFeatures();
        UpdateUnitGraphicsResource();
    }

    // Combat.pas:1186
    public override void Deactivate()
    {
        base.Deactivate();
        UpdateParty();
        ReleaseUnitGraphicsResource();
    }

    // Combat.pas:1396
    public override void ObjectDestroyed(CombatObject objectValue)
    {
        CombatUnit C;
        Troop U;
        base.ObjectDestroyed(objectValue);
        if (Disabled && objectValue is CombatUnit)
        {
            C = (CombatUnit)objectValue;
            U = C.Troop;
            if (U != null && U.GetTransporter() && (int)(((uint)C.Position) >> 4) == (int)(((uint)Position) >> 4))
            {
                DestroyObject();
            }
        }
    }

    // Combat.pas:1193
    public override void Initialize(Battle combat)
    {
        base.Initialize(combat);
        if (Troop is Leader)
        {
            combat.CombatData.InvolvedLeaders.Add((byte)CombatPlayer);
        }
    }

    // Combat.pas:1200
    public override void Finalize(FinalizeMode mode)
    {
        CombatParty party;
        ArmyHexSprite placed;
        if (!Status.Contains(CombatObjectStatusElement.Destroyed) && !Troop.GetAbilityEnabled(130) && !Troop.GetAbilityEnabled(131))
        {
            if (mode != FinalizeMode.All)
            {
                if ((Position & 15) <= 7)
                {
                    party = Combat.CombatData.Parties.GetParty((byte)(int)(((uint)Position) >> 4));
                    if (party != null && (sbyte)party.PlayerIndex == CombatPlayer)
                    {
                        var player = GetPlayer();
                        if (StrategicMap.Current.Players.GetPlayers(player).GameOverStatus == GameOverStatus.Lost)
                        {
                            placed = Troop.Place(party.X, party.Y, party.MapLevel, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.NeglectEmptyPlayerLocation));
                        }
                        else
                        {
                            var player2 = GetPlayer();
                            placed = Troop.Place(party.X, party.Y, party.MapLevel, player2, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.NeglectEmptyPlayerLocation));
                        }

                        if (placed != null)
                        {
                            Dispose();
                        }
                    }
                }
            }
            else
            {
                if ((Position & 15) <= 7)
                {
                    party = Combat.CombatData.Parties.GetParty((byte)(int)(((uint)Position) >> 4));
                }
                else
                {
                    party = null;
                }

                if (party == null || party.X != -1)
                {
                    var player = GetPlayer();
                    if (StrategicMap.Current.Players.GetPlayers(player).GameOverStatus == GameOverStatus.Lost)
                    {
                        Troop.Place(Combat.CombatData.X, Combat.CombatData.Y, Combat.CombatData.MapLevel, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                    }
                    else
                    {
                        var player2 = GetPlayer();
                        Troop.Place(Combat.CombatData.X, Combat.CombatData.Y, Combat.CombatData.MapLevel, player2, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                    }

                    Dispose();
                }
                else
                {
                    if (party != null && (sbyte)party.PlayerIndex != CombatPlayer)
                    {
                        var player = GetPlayer();
                        Troop.Place(Combat.CombatData.X, Combat.CombatData.Y, Combat.CombatData.MapLevel, player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                    }
                }
            }
        }
        else
        {
            Dispose();
        }
    }

    // Combat.pas:1494
    public override bool SupportCombatMode(Set<CombatMode> modes)
    {
        bool result;
        Troop U;
        int I;
        U = Troop;
        if (U != null && U.GetHitPoints() > 0)
        {
            result = false;
            I = Troop.GetAbilityCount();
            while (I != 0 && !result)
            {
                I -= 1;
                result = Troop.GetAbilityEnabled(I) && modes.Contains(GamePack.Current.AbilityControl.GetAbility(I).GetCombatMode(Troop));
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Combat.pas:1151
    public override Set<WallCombatFeaturesElement> WallCombatFeatures() => WallCombatFeaturesValue;

    // Combat.pas:1247
    public override void PlaySfx(byte sound)
    {
        switch (sound)
        {
            case 0:
                var unitGraphicsResourceIndex = Troop.GetUnitGraphicsResourceIndex();
                var unitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex);
                unitGraphicsResource.SfxLibraries[2].Play(0);
                break;
            case 1:
                var fUnitUnitGraphicsResourceIndex = Troop.GetUnitGraphicsResourceIndex();
                var fUnitGraphicsResourcesUnitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(fUnitUnitGraphicsResourceIndex);
                fUnitGraphicsResourcesUnitGraphicsResource.SfxLibraries[2].Play(1);
                break;
            case 2:
                var unitGraphicsResourceIndex2 = Troop.GetUnitGraphicsResourceIndex();
                var unitGraphicsResource2 = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex2);
                unitGraphicsResource2.SfxLibraries[3].Play(0);
                break;
            case 3:
                var unitGraphicsResourceIndex3 = Troop.GetUnitGraphicsResourceIndex();
                var unitGraphicsResource3 = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex3);
                unitGraphicsResource3.SfxLibraries[4].Play(0);
                break;
        }
    }

    // Combat.pas:1115
    public override void NewTurn(byte combatPlayer)
    {
        Troop U;
        base.NewTurn(combatPlayer);
        U = Troop;
        if (U != null)
        {
            U.TriggerNewCombatTurn(this, (sbyte)combatPlayer);
        }
    }

    // Combat.pas:1124
    public override void NewRound()
    {
        base.NewRound();
    }

    // Combat.pas:1127
    public override void CombatDone()
    {
        Troop U;
        base.CombatDone();
        U = Troop;
        if (U != null)
        {
            U.TriggerCombatDone(this);
        }
    }

    // Combat.pas:1604
    public override void Show(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        Troop U;
        UnitShowInfo info = default;
        U = Troop;
        if (U != null)
        {
            info.Direction = (byte)direction;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank, UnitOverlayModesElement.Combat);
            info.Animation = UnitAnimation.StandStill;
            if ((Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed, CombatObjectStatusElement.Fled) & Status) != Set<CombatObjectStatusElement>.Of())
            {
                info.Frame = 0;
                info.Looped = false;
            }
            else
            {
                info.Frame = frame;
                info.Looped = true;
            }

            U.Show(X, Y, surface, info);
            if (Status.Contains(CombatObjectStatusElement.Fled))
            {
                GamePack.Current.CombatImages[7].Show(X + 18, Y + 10, surface);
            }
        }
    }

    // Combat.pas:1628
    public override int ShowAttackAnimation(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        int result;
        UnitShowInfo info = default;
        // { Direct field receiver. }
        if (Troop != null)
        {
            info.Direction = (byte)direction;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank, UnitOverlayModesElement.Combat);
            info.Animation = UnitAnimation.Attack;
            info.Frame = frame;
            info.Looped = false;
            result = Troop.Show(X, Y, surface, info);
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Combat.pas:1644
    public override int ShowHitAnimation(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        int result;
        UnitShowInfo info = default;
        // { Direct field receiver. }
        if (Troop != null)
        {
            info.Direction = (byte)direction;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank, UnitOverlayModesElement.Combat);
            info.Animation = UnitAnimation.Hit;
            info.Frame = frame;
            info.Looped = false;
            result = Troop.Show(X, Y, surface, info);
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Combat.pas:1159
    public override Set<DamageType> ExecuteDamageEffectsRole(Set<DamageType> damageTypes) => Troop.ExecuteDamageEffectsRole(damageTypes);

    // Combat.pas:1544
    public override void Surrender()
    {
        if (GetHitPoints() != 0)
        {
            SetHitPoints(0);
        }

        Status = Status + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed);
        ResetTargets();
        if (Troop is Leader)
        {
            Combat.CombatData.DefeatedLeaders.Add((byte)CombatPlayer);
        }

        Combat.ObjectDestroyed(this);
        if (Troop != null)
        {
            KillUnit();
        }
    }

    // Combat.pas:1525
    public override void DestroyObject()
    {
        Troop U;
        if (GetHitPoints() != 0)
        {
            SetHitPoints(0);
        }

        Status = Status + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed);
        U = Troop;
        if (U != null)
        {
            for (int I = U.GetAbilityCount() - 1; I >= 0; I--)
            {
                if (Troop.GetAbilityEnabled(I))
                {
                    GamePack.Current.AbilityControl.GetAbility(I).CombatObjectDestroyed(this);
                }
            }
        }

        if (Status.Contains(CombatObjectStatusElement.Destroyed))
        {
            ResetTargets();
            if (Troop is Leader)
            {
                Combat.CombatData.DefeatedLeaders.Add((byte)CombatPlayer);
            }

            Combat.ObjectDestroyed(this);
            if (Troop != null)
            {
                KillUnit();
            }
        }
    }

    // Combat.pas:1154
    public override Set<DamageType> ExecuteDamageEffects(Set<DamageType> damageTypes) => Troop.ExecuteCombatDamageEffects(Combat, damageTypes);

    // Combat.pas:1486
    public override string IdString()
    {
        string result;
        if (Combat.CombatData.Players.GetPlayers(CombatPlayer).SideIndex == 0)
        {
            result = "A";
        }
        else
        {
            result = "D";
        }

        if (Troop != null)
        {
            var name = Troop.GetName();
            result = result + "(" + IntToStr(Position) + ")=" + name;
        }
        else
        {
            result = result + "(" + IntToStr(Position) + ")";
        }

        return result;
    }

    // Combat.pas:1358
    public virtual void SetUnit(Troop troop)
    {
        Troop U;
        CombatParty party;
        U = Troop;
        if (U != troop)
        {
            if (U != null)
            {
                party = _party;
                if (party != null && Troop.Owner == party.Army)
                {
                    Troop.SetOwner(null);
                }

                Troop.Release();
            }

            Troop = troop;
            U = Troop;
            if (U != null)
            {
                U.AddRef();
                party = _party;
                if (party != null && !Status.Contains(CombatObjectStatusElement.Destroyed))
                {
                    party.Army.AddUnit(Troop);
                }
            }
        }

        if (Status.Contains(CombatObjectStatusElement.Activated))
        {
            UpdateUnitGraphicsResource();
        }
    }

    // Combat.pas:1522
    public virtual void KillUnit()
    {
        Troop.Killed();
    }

    // Combat.pas:1087
    public void Setup(byte id, byte side, Troop troop, byte position, byte combatPlayer)
    {
        Id = id;
        Position = position;
        SetCombatPlayer(combatPlayer);
        SetUnit(troop);
    }

    // Combat.pas:1164
    public void UpdateWallCombatFeatures()
    {
        Troop U;
        U = Troop;
        if (U != null && !Status.Contains(CombatObjectStatusElement.Destroyed))
        {
            WallCombatFeaturesValue = U.GetWallCombatFeatures();
            if (Troop.GetAbilityEnabled(52) && ((SpellCastingAbility)GamePack.Current.AbilityControl.GetAbility(52)).CanCastBreachWallSpell(Troop))
            {
                WallCombatFeaturesValue = WallCombatFeaturesValue + Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching);
            }
        }
        else
        {
            WallCombatFeaturesValue = Set<WallCombatFeaturesElement>.Of();
        }
    }

    // Combat.pas:1303
    public void SetParty(CombatParty party)
    {
        CombatParty P;
        if (party != _party)
        {
            if (_party != null && Troop != null)
            {
                Troop.SetOwner(null);
            }

            _party = party;
            P = _party;
            if (P != null && Troop != null)
            {
                Troop.SetOwner(P.Army);
                _party.Army.UpdateFormation();
            }
        }
    }

    // Combat.pas:1319
    public void UpdateParty()
    {
        CombatParty party;
        if (Status.Contains(CombatObjectStatusElement.Activated))
        {
            party = Combat.CombatData.Parties.GetParty((byte)(int)(((uint)Position) >> 4));
            if ((byte)CombatPlayer != party.PlayerIndex || Status.Contains(CombatObjectStatusElement.Destroyed))
            {
                party = null;
            }
        }
        else
        {
            party = null;
        }

        SetParty(party);
    }

    // Combat.pas:1331
    public void UpdateUnitGraphicsResource()
    {
        int index;
        Troop U;
        U = Troop;
        if (U != null)
        {
            index = U.GetUnitGraphicsResourceIndex();
        }
        else
        {
            index = -1;
        }

        if (index != _unitGraphicsResourceIndex)
        {
            ReleaseUnitGraphicsResource();
            if (index != -1)
            {
                GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(index).AddRef();
                _unitGraphicsResourceIndex = index;
            }
        }
    }

    // Combat.pas:1347
    public void ReleaseUnitGraphicsResource()
    {
        int index;
        index = _unitGraphicsResourceIndex;
        if (index != -1)
        {
            GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(index).Release();
            _unitGraphicsResourceIndex = -1;
        }
    }

    // Combat.pas:1512
    public void SetStatus(Set<CombatObjectStatusElement> status)
    {
        if (status.Contains(CombatObjectStatusElement.Fled) && !Status.Contains(CombatObjectStatusElement.Fled))
        {
            Status = status;
            Combat.CheckTerminate();
        }
        else
        {
            Status = status;
        }
    }

    // Combat.pas:1578
    public int CommandingTargetPriority()
    {
        CommandAbilityData data;
        int result = 0;
        for (int I = 0; I <= 2; I++)
        {
            if (GetAbilityEnabled(CombatAbilityTables.CommandingPriorityAbilityIds[I]))
            {
                data = (CommandAbilityData)Troop.GetAbilityData(CombatAbilityTables.CommandingPriorityAbilityIds[I]);
                if (data != null)
                {
                    result += data.CommandingTargetPriority(Combat);
                }
            }
        }

        return result;
    }

    // Combat.pas:1591
    public int CommandingTargetStrength()
    {
        CommandAbilityData data;
        int result = 0;
        for (int I = 0; I <= 2; I++)
        {
            if (GetAbilityEnabled(CombatAbilityTables.CommandingStrengthAbilityIds[I]))
            {
                data = (CommandAbilityData)Troop.GetAbilityData(CombatAbilityTables.CommandingStrengthAbilityIds[I]);
                if (data != null)
                {
                    result += data.CommandingTargetStrength(Combat);
                }
            }
        }

        return result;
    }

    // intf_P3_01.inc:244

}
