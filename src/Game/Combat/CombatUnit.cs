#nullable disable warnings
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Combat;

// Troop. Party. Disabled. WallCombatFeaturesValue. UnitGraphicsResourceIndex. Inherited: FID.
// FPosition. FStatus. FControlMode
public class CombatUnit : CombatObject
{
    // Combat.pas:1084
    public new const int ClassId = 131_745;

    // Combat.pas:1071
    public CombatUnit(CustomNode? owner) : base(owner)
    {
        ControlMode = CombatObjectControlModes.ConquerObject;
        _unitGraphicsResourceIndex = -1;
    }

    public Troop? Troop { get; set; }

    private CombatParty? _party;

    public bool Disabled { get; set; }

    public WallCombatFeatures WallCombatFeaturesValue { get; private set; }

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
        base.PersistFields(persister);
        if (persister.Loading)
        {
            Troop? u = default;
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
        if (!Status.Contains(CombatObjectStatuses.Destroyed))
        {
            Troop? u = Troop;
            combatValue = 0;
            damageValue = 0;
            AbilityTypeList abilities = GamePack.Current.AbilityControl.AbilitiesByType[(int)AbilityType.Combat];
            AbilityDamageValues values = default;
            int v;
            Ability? ability;
            for (int i = abilities.Count - 1; i >= 0; i--)
            {
                ability = abilities.GetAbility(i);
                if (u.GetAbilityEnabled(ability.AbilityId))
                {
                    ability.TacticalBattleGetDamageValues(this, defender: target, damageValues: ref values);
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
                ability = GamePack.Current.AbilityControl[52];
                ability.TacticalBattleGetDamageValues(this, defender: target, damageValues: ref values);
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
        return target is CombatUnit targetUnit && !target.Status.Contains(CombatObjectStatuses.Destroyed)
            ? Combat.CombatData.Players.GetPlayers(CombatPlayer).SideIndex == 1
                ? Troop.GetCombatValue(targetUnit.Troop, mode: 1)
                : Troop.GetCombatValue(targetUnit.Troop, mode: 2)
            : 0;
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
    public override bool GetEnabled() => Troop is not null && ((CombatObjectStatuses.Destroyed | CombatObjectStatuses.Fled) & Status) == CombatObjectStatuses.None && !Disabled;

    // Combat.pas:1142
    public override bool GetLocked() => Troop.GetAbilityEnabled(95) || Troop.GetAbilityEnabled(97) || Troop.GetAbilityEnabled(92) || Troop.GetAbilityEnabled(94) || Troop.GetAbilityEnabled(34);

    // Combat.pas:1459
    public override int GetAttack() => Troop.GetAttack();

    // Combat.pas:1462
    public override int GetDefense() => Troop.GetDefense();

    // Combat.pas:1465
    public override int GetResistance() => Troop.GetResistance();

    // Combat.pas:1468
    public override int GetDamage() => Troop.GetDamage();

    // Combat.pas:1471
    public override DamageTypes GetImmunityTypes() => Troop.GetImmunityTypes() | Combat.CombatData.ImmunityTypes;

    // Combat.pas:1474
    public override DamageTypes GetProtectionTypes() => Troop.GetProtectionTypes();

    // Combat.pas:1282
    public override int GetHits() => Troop is not null ? Troop.GetHits() : 0;// { Direct field test retains the receiver. }// { No cast temporary. }

    // Combat.pas:1289
    public override int HitPoints => Troop is not null ? Troop.GetHitPoints() : 0;// { Direct field test retains the receiver. }// { No cast temporary. }

    // Combat.pas:1296
    public override void SetHitPoints(int value)
        // { Direct nil guard. }
        // { No cast temporary. }
        => Troop?.SetHitPoints((sbyte)value);

    // Combat.pas:1260
    public override Alignment GetAlignment() => Troop.GetAlignment();

    // Combat.pas:1257
    public override int Moves => Troop.Moves;

    // Combat.pas:1477
    public override int Experience => Troop.GetExperience();

    // Combat.pas:1480
    public override void SetExperience(int value) => Troop.SetExperience(value);

    // Combat.pas:1483
    public override int Level => Troop.UnitLevel.TruncateToByte();

    // Combat.pas:1095
    public override int GetVisibilityRange()
    {
        Troop? u = Troop;
        int result;
        if (u is not null)
        {
            int level = u.GetAbilityLevel(64);
            result = level != 0 ? (level * 2) + 4 : 4;

            if (Combat.ExplorationSite == 1 && !Troop.GetAbilityEnabled(39))
            {
                return (result + 1) / 2;
            }
        }
        else
        {
            return 0;
        }

        return result;
    }

    // Combat.pas:1266
    public override bool GetAbilityEnabled(int abilityId) => Troop is not null && Troop.GetAbilityEnabled(abilityId);

    // Combat.pas:1271
    public override void SetAbilityEnabled(int abilityId, bool enabled) => Troop.SetAbilitySet(abilityId, value: enabled ? 1 : 0);

    // Combat.pas:1274
    public override int GetAbilityLevel(int abilityId) => Troop.GetAbilityLevel(abilityId);

    // Combat.pas:1277
    public override int GetAbilityCount() => Troop is not null ? Troop.GetAbilityCount() : 0;

    // Combat.pas:1263
    public override AbilityOwner? AbilityOwner => Troop;

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
        base.ObjectDestroyed(objectValue);
        if (Disabled && objectValue is CombatUnit combatUnit)
        {
            Troop? u = combatUnit.Troop;
            if (u is not null && u.GetTransporter() && combatUnit.Position >>> 4 == Position >>> 4)
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
            _ = combat.CombatData.InvolvedLeaders.Add(CombatPlayer.TruncateToByte());
        }
    }

    // Combat.pas:1200
    public override void Finalize(FinalizeMode mode)
    {
        if (!Status.Contains(CombatObjectStatuses.Destroyed) && !Troop.GetAbilityEnabled(130) && !Troop.GetAbilityEnabled(131))
        {
            CombatParty? party;
            if (mode != FinalizeMode.All)
            {
                if ((Position & 0xF) <= 7)
                {
                    party = Combat.CombatData.Parties.GetParty((byte)(Position >>> 4));
                    if (party is not null && (sbyte)party.PlayerIndex == CombatPlayer)
                    {
                        sbyte player = GetPlayer();
                        ArmyHexSprite? placed;
                        if (StrategicMap.Current.Players.GetPlayers(player).GameOverStatus == GameOverStatus.Lost)
                        {
                            placed = Troop.Place(party.Location.X, party.Location.Y, L: party.Location.MapLevel, player: 0, mode: UnitPlaceModes.NeglectEmptyPlayerLocation);
                        }
                        else
                        {
                            sbyte player2 = GetPlayer();
                            placed = Troop.Place(party.Location.X, party.Location.Y, L: party.Location.MapLevel, player: player2, mode: UnitPlaceModes.NeglectEmptyPlayerLocation);
                        }

                        if (placed is not null)
                        {
                            Dispose();
                        }
                    }
                }
            }
            else
            {
                party = (Position & 0xF) <= 7 ? Combat.CombatData.Parties.GetParty((byte)(Position >>> 4)) : null;

                if (party is null || party.Location.X != -1)
                {
                    sbyte player = GetPlayer();
                    if (StrategicMap.Current.Players.GetPlayers(player).GameOverStatus == GameOverStatus.Lost)
                    {
                        _ = Troop.Place(Combat.CombatData.Location.X, Combat.CombatData.Location.Y, L: Combat.CombatData.Location.MapLevel, player: 0, mode: UnitPlaceModes.ScanArea);
                    }
                    else
                    {
                        sbyte player2 = GetPlayer();
                        _ = Troop.Place(Combat.CombatData.Location.X, Combat.CombatData.Location.Y, L: Combat.CombatData.Location.MapLevel, player: player2, mode: UnitPlaceModes.ScanArea);
                    }

                    Dispose();
                }
                else
                {
                    if (party is not null && (sbyte)party.PlayerIndex != CombatPlayer)
                    {
                        sbyte player = GetPlayer();
                        _ = Troop.Place(Combat.CombatData.Location.X, Combat.CombatData.Location.Y, L: Combat.CombatData.Location.MapLevel, player, mode: UnitPlaceModes.ScanArea);
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
    public override bool SupportCombatMode(CombatModes modes)
    {
        Troop? u = Troop;
        bool result;
        if (u is not null && u.GetHitPoints() > 0)
        {
            result = false;
            int i = Troop.GetAbilityCount();
            while (i != 0 && !result)
            {
                i -= 1;
                result = Troop.GetAbilityEnabled(i) && modes.Contains(GamePack.Current.AbilityControl[i].GetCombatMode(Troop).ToFlag());
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Combat.pas:1151
    public override WallCombatFeatures WallCombatFeatures() => WallCombatFeaturesValue;

    // Combat.pas:1247
    public override void PlaySfx(int sound)
    {
        switch (sound)
        {
            case 0:
                int unitGraphicsResourceIndex = Troop.UnitGraphicsResourceIndex;
                UnitGraphicsResource? unitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex);
                _ = unitGraphicsResource.SfxLibraries[2].Play(0);
                break;
            case 1:
                int fUnitUnitGraphicsResourceIndex = Troop.UnitGraphicsResourceIndex;
                UnitGraphicsResource? fUnitGraphicsResourcesUnitGraphicsResource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(fUnitUnitGraphicsResourceIndex);
                _ = fUnitGraphicsResourcesUnitGraphicsResource.SfxLibraries[2].Play(1);
                break;
            case 2:
                int unitGraphicsResourceIndex2 = Troop.UnitGraphicsResourceIndex;
                UnitGraphicsResource? unitGraphicsResource2 = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex2);
                _ = unitGraphicsResource2.SfxLibraries[3].Play(0);
                break;
            case 3:
                int unitGraphicsResourceIndex3 = Troop.UnitGraphicsResourceIndex;
                UnitGraphicsResource? unitGraphicsResource3 = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex3);
                _ = unitGraphicsResource3.SfxLibraries[4].Play(0);
                break;
            default:
                break;
        }
    }

    // Combat.pas:1115
    public override void NewTurn(byte combatPlayer)
    {
        base.NewTurn(combatPlayer);
        Troop? u = Troop;
        u?.TriggerNewCombatTurn(this, player: (sbyte)combatPlayer);
    }

    // Combat.pas:1124
    public override void NewRound() => base.NewRound();

    // Combat.pas:1127
    public override void CombatDone()
    {
        base.CombatDone();
        Troop? u = Troop;
        u?.TriggerCombatDone(this);
    }

    // Combat.pas:1604
    public override void Show(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        UnitShowInfo info = default;
        Troop? u = Troop;
        if (u is not null)
        {
            info.Direction = (byte)direction;
            info.Overlays = UnitOverlayModes.HitPoints | UnitOverlayModes.Rank | UnitOverlayModes.Combat;
            info.Animation = UnitAnimation.StandStill;
            if (((CombatObjectStatuses.Destroyed | CombatObjectStatuses.Fled) & Status) != CombatObjectStatuses.None)
            {
                info.Frame = 0;
                info.Looped = false;
            }
            else
            {
                info.Frame = frame;
                info.Looped = true;
            }

            _ = u.Show(X, Y, lockedSurface: surface, info);
            if (Status.Contains(CombatObjectStatuses.Fled))
            {
                GamePack.Current.CombatImages[7].Show(X + 18, Y + 10, lockedSurface: surface);
            }
        }
    }

    // Combat.pas:1628
    public override int ShowAttackAnimation(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        UnitShowInfo info = default;
        // { Direct field receiver. }
        if (Troop is not null)
        {
            info.Direction = (byte)direction;
            info.Overlays = UnitOverlayModes.HitPoints | UnitOverlayModes.Rank | UnitOverlayModes.Combat;
            info.Animation = UnitAnimation.Attack;
            info.Frame = frame;
            info.Looped = false;
            return Troop.Show(X, Y, lockedSurface: surface, info);
        }
        else
        {
            return -1;
        }
    }

    // Combat.pas:1644
    public override int ShowHitAnimation(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        UnitShowInfo info = default;
        // { Direct field receiver. }
        if (Troop is not null)
        {
            info.Direction = (byte)direction;
            info.Overlays = UnitOverlayModes.HitPoints | UnitOverlayModes.Rank | UnitOverlayModes.Combat;
            info.Animation = UnitAnimation.Hit;
            info.Frame = frame;
            info.Looped = false;
            return Troop.Show(X, Y, lockedSurface: surface, info);
        }
        else
        {
            return -1;
        }
    }

    // Combat.pas:1159
    public override DamageTypes ExecuteDamageEffectsRole(DamageTypes damageTypes) => Troop.ExecuteDamageEffectsRole(damageTypes);

    // Combat.pas:1544
    public override void Surrender()
    {
        if (HitPoints != 0)
        {
            SetHitPoints(0);
        }

        Status |= CombatObjectStatuses.Destroyed;
        ResetTargets();
        if (Troop is Leader)
        {
            _ = Combat.CombatData.DefeatedLeaders.Add(CombatPlayer.TruncateToByte());
        }

        Combat.ObjectDestroyed(this);
        if (Troop is not null)
        {
            KillUnit();
        }
    }

    // Combat.pas:1525
    public override void DestroyObject()
    {
        if (HitPoints != 0)
        {
            SetHitPoints(0);
        }

        Status |= CombatObjectStatuses.Destroyed;
        Troop? u = Troop;
        if (u is not null)
        {
            for (int i = u.GetAbilityCount() - 1; i >= 0; i--)
            {
                if (Troop.GetAbilityEnabled(i))
                {
                    GamePack.Current.AbilityControl[i].CombatObjectDestroyed(this);
                }
            }
        }

        if (Status.Contains(CombatObjectStatuses.Destroyed))
        {
            ResetTargets();
            if (Troop is Leader)
            {
                _ = Combat.CombatData.DefeatedLeaders.Add(CombatPlayer.TruncateToByte());
            }

            Combat.ObjectDestroyed(this);
            if (Troop is not null)
            {
                KillUnit();
            }
        }
    }

    // Combat.pas:1154
    public override DamageTypes ExecuteDamageEffects(DamageTypes damageTypes) => Troop.ExecuteCombatDamageEffects(Combat, damageTypes);

    // Combat.pas:1486
    public override string IdString()
    {
        string result = Combat.CombatData.Players.GetPlayers(CombatPlayer).SideIndex == 0 ? "A" : "D";
        if (Troop is not null)
        {
            string name = Troop.GetName();
            return $"{result}({((int)Position).ToString(CultureInfo.InvariantCulture)})={name}";
        }
        else
        {
            return $"{result}({((int)Position).ToString(CultureInfo.InvariantCulture)})";
        }
    }

    // Combat.pas:1358
    public virtual void SetUnit(Troop? troop)
    {
        Troop? u = Troop;
        if (u != troop)
        {
            CombatParty? party;
            if (u is not null)
            {
                party = _party;
                if (party is not null && Troop.Owner == party.Army)
                {
                    Troop.SetOwner(null);
                }

                _ = Troop.Release();
            }

            Troop = troop;
            u = Troop;
            if (u is not null)
            {
                _ = u.AddRef();
                party = _party;
                if (party is not null && !Status.Contains(CombatObjectStatuses.Destroyed))
                {
                    _ = party.Army.AddUnit(Troop);
                }
            }
        }

        if (Status.Contains(CombatObjectStatuses.Activated))
        {
            UpdateUnitGraphicsResource();
        }
    }

    // Combat.pas:1522
    public virtual void KillUnit() => Troop.Killed();

    // Combat.pas:1087
    public void Setup(byte id, int side, Troop? troop, byte position, byte combatPlayer)
    {
        Id = id;
        Position = position;
        SetCombatPlayer(combatPlayer);
        SetUnit(troop);
    }

    // Combat.pas:1164
    public void UpdateWallCombatFeatures()
    {
        Troop? u = Troop;
        if (u is not null && !Status.Contains(CombatObjectStatuses.Destroyed))
        {
            WallCombatFeaturesValue = u.GetWallCombatFeatures();
            if (Troop.GetAbilityEnabled(52) && GamePack.Current.AbilityControl.GetAbility<SpellCastingAbility>(52).CanCastBreachWallSpell(Troop))
            {
                WallCombatFeaturesValue |= Game.Combat.WallCombatFeatures.WallBreaching;
            }
        }
        else
        {
            WallCombatFeaturesValue = Game.Combat.WallCombatFeatures.None;
        }
    }

    // Combat.pas:1303
    public void SetParty(CombatParty? party)
    {
        if (party != _party)
        {
            if (_party is not null && Troop is not null)
            {
                Troop.SetOwner(null);
            }

            _party = party;
            CombatParty? p = _party;
            if (p is not null && Troop is not null)
            {
                Troop.SetOwner(p.Army);
                _party.Army.UpdateFormation();
            }
        }
    }

    // Combat.pas:1319
    public void UpdateParty()
    {
        CombatParty? party;
        if (Status.Contains(CombatObjectStatuses.Activated))
        {
            party = Combat.CombatData.Parties.GetParty((byte)(Position >>> 4));
            if ((byte)CombatPlayer != party.PlayerIndex || Status.Contains(CombatObjectStatuses.Destroyed))
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
        Troop? u = Troop;
        int index = u is not null ? u.UnitGraphicsResourceIndex : -1;

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
        int index = _unitGraphicsResourceIndex;
        if (index != -1)
        {
            _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(index).Release();
            _unitGraphicsResourceIndex = -1;
        }
    }

    // Combat.pas:1512
    public void SetStatus(CombatObjectStatuses status)
    {
        if (status.Contains(CombatObjectStatuses.Fled) && !Status.Contains(CombatObjectStatuses.Fled))
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
        int result = 0;
        for (int i = 0; i <= 2; i++)
        {
            if (GetAbilityEnabled(CombatAbilityTables.CommandingPriorityAbilityIds[i]))
            {
                CommandAbilityData data = GamePack.Current.AbilityControl.GetAbility<CommandAbility>(CombatAbilityTables.CommandingPriorityAbilityIds[i]).GetData(Troop);
                if (data is not null)
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
        int result = 0;
        for (int i = 0; i <= 2; i++)
        {
            if (GetAbilityEnabled(CombatAbilityTables.CommandingStrengthAbilityIds[i]))
            {
                CommandAbilityData data = GamePack.Current.AbilityControl.GetAbility<CommandAbility>(CombatAbilityTables.CommandingStrengthAbilityIds[i]).GetData(Troop);
                if (data is not null)
                {
                    result += data.CommandingTargetStrength(Combat);
                }
            }
        }

        return result;
    }
}
