#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.Combat;

// Troop. Party. Disabled. WallCombatFeaturesValue. UnitGraphicsResourceIndex. Inherited: FID.
// FPosition. FStatus. FControlMode
public partial class CombatUnit : CombatObject
{
    // Combat.pas:1084
    public new const int ClassId = 131745;

    // Combat.pas:1071
    public CombatUnit(CustomNode owner) : base(owner)
    {
        ControlMode = Set<CombatObjectControlModeElement>.Of(CombatObjectControlModeElement.ConquerObject);
        _unitGraphicsResourceIndex = -1;
    }

    public Troop Troop { get; set; }

    private CombatParty _party;

    public bool Disabled { get; set; }

    public Set<WallCombatFeaturesElement> WallCombatFeaturesValue { get; private set; }

    private int _unitGraphicsResourceIndex;

    // Combat.pas:1078
    public override void Dispose()
    {
        SetUnit(null);
        base.Dispose();
    }

    // Combat.pas:1382
    public override void PersistFields(IPersister persister)
    {
        Troop u = default;
        base.PersistFields(persister);
        if (persister.Loading)
        {
            u = persister.Created("Troop", objectValue: u);
            SetUnit(u);
            _ = (u?.Release());
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
        Troop u;
        AbilityTypeList abilities;
        AbilityDamageValues values = default;
        int v;
        Ability ability;
        if (!Status.Contains(CombatObjectStatusElement.Destroyed))
        {
            u = Troop;
            combatValue = 0;
            damageValue = 0;
            abilities = GamePack.Current.AbilityControl.AbilitiesByType[(int)AbilityType.Combat];
            for (int i = abilities.Count - 1; i >= 0; i--)
            {
                ability = abilities.GetAbility(i);
                if (u.GetAbilityEnabled(ability.AbilityId))
                {
                    ability.TacticalBattleGetDamageValues(this, target, ref values);
                    if (values.SourceDamage > 0)
                    {
                        v = values.SourceDamage * values.SourceDamage / (values.TargetDamage + values.SourceDamage);
                        if (v <= 0)
                        {
                            v = 1;
                        }

                        if (v > combatValue)
                        {
                            combatValue = v;
                        }

                        if (values.SourceDamage > damageValue)
                        {
                            damageValue = values.SourceDamage;
                        }
                    }
                }
            }

            if (u.GetAbilityEnabled(52))
            {
                ability = GamePack.Current.AbilityControl.GetAbility(52);
                ability.TacticalBattleGetDamageValues(this, target, ref values);
                if (values.SourceDamage > 0)
                {
                    v = values.SourceDamage * values.SourceDamage / (values.TargetDamage + values.SourceDamage);
                    if (v <= 0)
                    {
                        v = 1;
                    }

                    if (v > combatValue)
                    {
                        combatValue = v;
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
        int result = target is CombatUnit targetUnit && !target.Status.Contains(CombatObjectStatusElement.Destroyed)
            ? Combat.CombatData.Players.GetPlayers(CombatPlayer).SideIndex == 1
                ? Troop.GetCombatValue(targetUnit.Troop, 1)
                : Troop.GetCombatValue(targetUnit.Troop, 2)
            : 0;
        return result;
    }

    // Combat.pas:1554
    public override int GetTargetPriority()
    {
        if (TargetPriorityInvalid)
        {
            _ = base.GetTargetPriority();
            int commandingTargetPriority = CommandingTargetPriority();
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
            _ = base.GetTargetStrength();
            int commandingTargetStrength = CommandingTargetStrength();
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
        sbyte result = Troop != null ? Troop.GetHits() : (sbyte)0;
        // { Direct field test retains the receiver. }
        // { No cast temporary. }

        return result;
    }

    // Combat.pas:1289
    public override sbyte GetHitPoints()
    {
        sbyte result = Troop != null ? Troop.GetHitPoints() : (sbyte)0;
        // { Direct field test retains the receiver. }
        // { No cast temporary. }

        return result;
    }

    // Combat.pas:1296
    public override void SetHitPoints(byte value) =>
        // { Direct nil guard. }
        // { No cast temporary. }
        Troop?.SetHitPoints((sbyte)value);

    // Combat.pas:1260
    public override Alignment GetAlignment() => Troop.GetAlignment();

    // Combat.pas:1257
    public override int GetMoves() => Troop.GetMoves();

    // Combat.pas:1477
    public override int GetExperience() => Troop.GetExperience();

    // Combat.pas:1480
    public override void SetExperience(int value) => Troop.SetExperience(value);

    // Combat.pas:1483
    public override byte GetLevel() => (byte)Troop.GetUnitLevel();

    // Combat.pas:1095
    public override int GetVisibilityRange()
    {
        int result;
        int level;
        Troop u;
        u = Troop;
        if (u != null)
        {
            level = u.GetAbilityLevel(64);
            result = level != 0 ? (level * 2) + 4 : 4;

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
        bool result = Troop != null && Troop.GetAbilityEnabled(abilityId);
        return result;
    }

    // Combat.pas:1271
    public override void SetAbilityEnabled(int abilityId, bool enabled) => Troop.SetAbilitySet(abilityId, (byte)(enabled ? 1 : 0));

    // Combat.pas:1274
    public override int GetAbilityLevel(int abilityId) => Troop.GetAbilityLevel(abilityId);

    // Combat.pas:1277
    public override int GetAbilityCount()
    {
        int result = Troop != null ? Troop.GetAbilityCount() : 0;
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
        Troop u;
        base.ObjectDestroyed(objectValue);
        if (Disabled && objectValue is CombatUnit combatUnit)
        {
            u = combatUnit.Troop;
            if (u != null && u.GetTransporter() && (int)(((uint)combatUnit.Position) >> 4) == (int)(((uint)Position) >> 4))
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
            _ = combat.CombatData.InvolvedLeaders.Add((byte)CombatPlayer);
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
                        sbyte player = GetPlayer();
                        if (StrategicMap.Current.Players.GetPlayers(player).GameOverStatus == GameOverStatus.Lost)
                        {
                            placed = Troop.Place(party.Location.X, party.Location.Y, party.Location.MapLevel, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.NeglectEmptyPlayerLocation));
                        }
                        else
                        {
                            sbyte player2 = GetPlayer();
                            placed = Troop.Place(party.Location.X, party.Location.Y, party.Location.MapLevel, player2, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.NeglectEmptyPlayerLocation));
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
                party = (Position & 15) <= 7 ? Combat.CombatData.Parties.GetParty((byte)(int)(((uint)Position) >> 4)) : null;

                if (party == null || party.Location.X != -1)
                {
                    sbyte player = GetPlayer();
                    if (StrategicMap.Current.Players.GetPlayers(player).GameOverStatus == GameOverStatus.Lost)
                    {
                        _ = Troop.Place(Combat.CombatData.Location.X, Combat.CombatData.Location.Y, Combat.CombatData.Location.MapLevel, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                    }
                    else
                    {
                        sbyte player2 = GetPlayer();
                        _ = Troop.Place(Combat.CombatData.Location.X, Combat.CombatData.Location.Y, Combat.CombatData.Location.MapLevel, player2, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                    }

                    Dispose();
                }
                else
                {
                    if (party != null && (sbyte)party.PlayerIndex != CombatPlayer)
                    {
                        sbyte player = GetPlayer();
                        _ = Troop.Place(Combat.CombatData.Location.X, Combat.CombatData.Location.Y, Combat.CombatData.Location.MapLevel, player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
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
        Troop u;
        int i;
        u = Troop;
        if (u != null && u.GetHitPoints() > 0)
        {
            result = false;
            i = Troop.GetAbilityCount();
            while (i != 0 && !result)
            {
                i -= 1;
                result = Troop.GetAbilityEnabled(i) && modes.Contains(GamePack.Current.AbilityControl.GetAbility(i).GetCombatMode(Troop));
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
                int unitGraphicsResourceIndex = Troop.GetUnitGraphicsResourceIndex();
                UnitGraphicsResource unitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex);
                _ = unitGraphicsResource.SfxLibraries[2].Play(0);
                break;
            case 1:
                int fUnitUnitGraphicsResourceIndex = Troop.GetUnitGraphicsResourceIndex();
                UnitGraphicsResource fUnitGraphicsResourcesUnitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(fUnitUnitGraphicsResourceIndex);
                _ = fUnitGraphicsResourcesUnitGraphicsResource.SfxLibraries[2].Play(1);
                break;
            case 2:
                int unitGraphicsResourceIndex2 = Troop.GetUnitGraphicsResourceIndex();
                UnitGraphicsResource unitGraphicsResource2 = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex2);
                _ = unitGraphicsResource2.SfxLibraries[3].Play(0);
                break;
            case 3:
                int unitGraphicsResourceIndex3 = Troop.GetUnitGraphicsResourceIndex();
                UnitGraphicsResource unitGraphicsResource3 = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex3);
                _ = unitGraphicsResource3.SfxLibraries[4].Play(0);
                break;
            default:
                break;
        }
    }

    // Combat.pas:1115
    public override void NewTurn(byte combatPlayer)
    {
        Troop u;
        base.NewTurn(combatPlayer);
        u = Troop;
        u?.TriggerNewCombatTurn(this, (sbyte)combatPlayer);
    }

    // Combat.pas:1124
    public override void NewRound() => base.NewRound();

    // Combat.pas:1127
    public override void CombatDone()
    {
        Troop u;
        base.CombatDone();
        u = Troop;
        u?.TriggerCombatDone(this);
    }

    // Combat.pas:1604
    public override void Show(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        Troop u;
        UnitShowInfo info = default;
        u = Troop;
        if (u != null)
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

            _ = u.Show(X, Y, surface, info);
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

        Status += Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed);
        ResetTargets();
        if (Troop is Leader)
        {
            _ = Combat.CombatData.DefeatedLeaders.Add((byte)CombatPlayer);
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
        Troop u;
        if (GetHitPoints() != 0)
        {
            SetHitPoints(0);
        }

        Status += Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed);
        u = Troop;
        if (u != null)
        {
            for (int i = u.GetAbilityCount() - 1; i >= 0; i--)
            {
                if (Troop.GetAbilityEnabled(i))
                {
                    GamePack.Current.AbilityControl.GetAbility(i).CombatObjectDestroyed(this);
                }
            }
        }

        if (Status.Contains(CombatObjectStatusElement.Destroyed))
        {
            ResetTargets();
            if (Troop is Leader)
            {
                _ = Combat.CombatData.DefeatedLeaders.Add((byte)CombatPlayer);
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
        string result = Combat.CombatData.Players.GetPlayers(CombatPlayer).SideIndex == 0 ? "A" : "D";
        if (Troop != null)
        {
            string name = Troop.GetName();
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
        Troop u;
        CombatParty party;
        u = Troop;
        if (u != troop)
        {
            if (u != null)
            {
                party = _party;
                if (party != null && Troop.Owner == party.Army)
                {
                    Troop.SetOwner(null);
                }

                _ = Troop.Release();
            }

            Troop = troop;
            u = Troop;
            if (u != null)
            {
                _ = u.AddRef();
                party = _party;
                if (party != null && !Status.Contains(CombatObjectStatusElement.Destroyed))
                {
                    _ = party.Army.AddUnit(Troop);
                }
            }
        }

        if (Status.Contains(CombatObjectStatusElement.Activated))
        {
            UpdateUnitGraphicsResource();
        }
    }

    // Combat.pas:1522
    public virtual void KillUnit() => Troop.Killed();

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
        Troop u;
        u = Troop;
        if (u != null && !Status.Contains(CombatObjectStatusElement.Destroyed))
        {
            WallCombatFeaturesValue = u.GetWallCombatFeatures();
            if (Troop.GetAbilityEnabled(52) && GamePack.Current.AbilityControl.GetAbility<SpellCastingAbility>(52).CanCastBreachWallSpell(Troop))
            {
                WallCombatFeaturesValue += Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching);
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
        CombatParty p;
        if (party != _party)
        {
            if (_party != null && Troop != null)
            {
                Troop.SetOwner(null);
            }

            _party = party;
            p = _party;
            if (p != null && Troop != null)
            {
                Troop.SetOwner(p.Army);
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
        Troop u;
        u = Troop;
        index = u != null ? u.GetUnitGraphicsResourceIndex() : -1;

        if (index != _unitGraphicsResourceIndex)
        {
            ReleaseUnitGraphicsResource();
            if (index != -1)
            {
                _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(index).AddRef();
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
            _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(index).Release();
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
        for (int i = 0; i <= 2; i++)
        {
            if (GetAbilityEnabled(CombatAbilityTables.CommandingPriorityAbilityIds[i]))
            {
                data = GamePack.Current.AbilityControl.GetAbility<CommandAbility>(CombatAbilityTables.CommandingPriorityAbilityIds[i]).GetData(Troop);
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
        for (int i = 0; i <= 2; i++)
        {
            if (GetAbilityEnabled(CombatAbilityTables.CommandingStrengthAbilityIds[i]))
            {
                data = GamePack.Current.AbilityControl.GetAbility<CommandAbility>(CombatAbilityTables.CommandingStrengthAbilityIds[i]).GetData(Troop);
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
