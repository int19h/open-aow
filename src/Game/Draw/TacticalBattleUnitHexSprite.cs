#nullable disable warnings
// Translated from AoWTC.pas.

using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.Draw;

// Unit. MoveIndicator. IndicatorStep. Action. ActionDone. ActionFrame. OffsetX.
// OffsetY. SelectedAbility. SelectedSpell. CombatAbilityCount. Abilities. Busy
public partial class TacticalBattleUnitHexSprite(CustomNode? owner) : UnitHexSprite(owner)
{

    // AoWTC.pas:3348
    public new const int ClassId = 2_228_484;

    public TacticalBattleUnit Unit { get; set; }

    private short _moveIndicator;
    private short _indicatorStep;

    public SpriteAction Action { get; private set; }
    public bool ActionDone { get; private set; }
    public short ActionFrame { get; private set; }

    public int OffsetX { get; internal set; }
    public int OffsetY { get; internal set; }

    public int SelectedAbility { get; set; }
    public int SelectedSpell { get; set; }
    public int CombatAbilityCount { get; private set; }
    public List<CombatAbilityRef> Abilities { get; private set; }
    public bool Busy { get; set; }

    // AoWTC.pas:3566
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId != 536936471 && (message.MessageId & 0b0100_0000_0000_0000_0000_0000_0000_0000) == 0 && Unit is not null)
        {
            Unit.MessageProc(ref message, sender);
        }
    }

    // AoWTC.pas:3461
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 4364)
        {
            if (message.HexNumber == 0 && (!CombatMap.Current.FogEnabled || Owner.FogCount != 0))
            {
                if (Unit.GetPlayer() != 0)
                {
                    sbyte player = Unit.GetPlayer();
                    Player? playerPlayerRace = StrategicMap.Current.Players.GetPlayers(player);
                    MapFieldMessageParameters param = message.Param;
                    param[0] = param[0].Int | (playerPlayerRace.PlayerRace + 1);
                    message.Param = param;
                }
                else
                {
                    MapFieldMessageParameters param2 = message.Param;
                    param2[0] = param2[0].Int | 0b1101;
                    message.Param = param2;
                }
            }
        }
        else
        {
            if (message.MessageId == 4352 && message.HexNumber == 0)
            {
                sbyte player = Unit.GetPlayer();
                message.Result = player;
            }
            else
            {
                if (message.MessageId == 200704 && message.HexNumber == 0)
                {
                    RandSeed = StrategicMap.Current.Random(16777215);
                    SelfDestructCombatAction selfDestruct = GamePack.Current.AbilityControl.GetAbility<SelfDestructAbility>(65).CreateSelfDestructCombatAction(message.Param[0].As<CombatObject>(), target: Unit);
                    if (selfDestruct.Damage > 0)
                    {
                        SetAction(SpriteAction.Hit);
                    }

                    CombatMap.Current.Combat.ExecuteCombatAction(selfDestruct);
                    _ = selfDestruct.Release();
                }
                else
                {
                    // The spell factory returns the base action type by design. Each spell id in this cascade builds a damage action.
                    DamageCombatAction spellDamage;
                    if (message.MessageId == 200707 && message.HexNumber == 0)
                    {
                        RandSeed = StrategicMap.Current.Random(16777215);
                        spellDamage = GamePack.Current.SpellControl[117].CreateCombatAction<DamageCombatAction>(message.Param[0].As<CombatObject>(), target: Unit, chargeMana: false);
                        if (spellDamage.Damage > 0)
                        {
                            SetAction(SpriteAction.Hit);
                        }

                        CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                        _ = spellDamage.Release();
                    }
                    else
                    {
                        if (message.MessageId == 200715 && message.HexNumber == 0)
                        {
                            RandSeed = StrategicMap.Current.Random(16777215);
                            spellDamage = GamePack.Current.SpellControl[113].CreateCombatAction<DamageCombatAction>(message.Param[0].As<CombatObject>(), target: Unit, chargeMana: false);
                            if (spellDamage.Damage > 0)
                            {
                                SetAction(SpriteAction.Hit);
                            }

                            CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                            _ = spellDamage.Release();
                        }
                        else
                        {
                            RangedAttackCombatAction rangedDamage;
                            if (message.MessageId == 200714 && message.HexNumber == 0)
                            {
                                RandSeed = StrategicMap.Current.Random(16777215);
                                rangedDamage = GamePack.Current.AbilityControl.GetAbility<RangedAttackAbility>(31).CreateRangedAttackCombatAction(message.Param[0].As<CombatObject>(), target: Unit, attackPenalty: 0);
                                if (rangedDamage.Damage > 0)
                                {
                                    SetAction(SpriteAction.Hit);
                                }

                                CombatMap.Current.Combat.ExecuteCombatAction(rangedDamage);
                                _ = rangedDamage.Release();
                            }
                            else
                            {
                                if (message.MessageId == 200723 && message.HexNumber == 0)
                                {
                                    RandSeed = StrategicMap.Current.Random(16777215);
                                    spellDamage = GamePack.Current.SpellControl[message.Param[1].Int].CreateCombatAction<DamageCombatAction>(message.Param[0].As<CombatObject>(), target: Unit, chargeMana: false);
                                    if (spellDamage.Damage > 0)
                                    {
                                        SetAction(SpriteAction.Hit);
                                    }

                                    CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                    _ = spellDamage.Release();
                                }
                                else
                                {
                                    if (message.MessageId == 200722 && message.HexNumber == 0)
                                    {
                                        RandSeed = StrategicMap.Current.Random(16777215);
                                        rangedDamage = GamePack.Current.AbilityControl.GetAbility<RangedAttackAbility>(message.Param[1].Int).CreateRangedAttackCombatAction(message.Param[0].As<CombatObject>(), target: Unit, attackPenalty: 0);
                                        if (rangedDamage.Damage > 0)
                                        {
                                            SetAction(SpriteAction.Hit);
                                        }

                                        CombatMap.Current.Combat.ExecuteCombatAction(rangedDamage);
                                        _ = rangedDamage.Release();
                                    }
                                    else
                                    {
                                        if (message.MessageId == 200709 && message.HexNumber == 0)
                                        {
                                            RandSeed = StrategicMap.Current.Random(16777215);
                                            spellDamage = GamePack.Current.SpellControl[104].CreateCombatAction<DamageCombatAction>(message.Param[0].As<CombatObject>(), target: Unit, chargeMana: false);
                                            if (spellDamage.Damage > 0)
                                            {
                                                SetAction(SpriteAction.Hit);
                                            }

                                            CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                            _ = spellDamage.Release();
                                        }
                                        else
                                        {
                                            if (message.MessageId == 200716 && message.HexNumber == 0)
                                            {
                                                RandSeed = StrategicMap.Current.Random(16777215);
                                                spellDamage = GamePack.Current.SpellControl[120].CreateCombatAction<DamageCombatAction>(message.Param[0].As<CombatObject>(), target: Unit, chargeMana: false);
                                                if (spellDamage.Damage > 0)
                                                {
                                                    SetAction(SpriteAction.Hit);
                                                }

                                                CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                                _ = spellDamage.Release();
                                            }
                                            else
                                            {
                                                if (message.MessageId == 200710 && message.HexNumber == 0)
                                                {
                                                    RandSeed = StrategicMap.Current.Random(16777215);
                                                    spellDamage = GamePack.Current.SpellControl[116].CreateCombatAction<DamageCombatAction>(message.Param[0].As<CombatObject>(), target: Unit, chargeMana: false);
                                                    if (spellDamage.Damage > 0)
                                                    {
                                                        SetAction(SpriteAction.Hit);
                                                    }

                                                    CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                                    _ = spellDamage.Release();
                                                }
                                                else
                                                {
                                                    if (message.MessageId == 200717 && message.HexNumber == 0)
                                                    {
                                                        RandSeed = StrategicMap.Current.Random(16777215);
                                                        spellDamage = GamePack.Current.SpellControl[127].CreateCombatAction<DamageCombatAction>(message.Param[0].As<CombatObject>(), target: Unit, chargeMana: false);
                                                        if (spellDamage.Damage > 0)
                                                        {
                                                            SetAction(SpriteAction.Hit);
                                                        }

                                                        CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                                        _ = spellDamage.Release();
                                                    }
                                                    else
                                                    {
                                                        if (message.MessageId == 200718 && message.HexNumber == 0)
                                                        {
                                                            RandSeed = StrategicMap.Current.Random(16777215);
                                                            spellDamage = GamePack.Current.SpellControl[128].CreateCombatAction<DamageCombatAction>(message.Param[0].As<CombatObject>(), target: Unit, chargeMana: false);
                                                            if (spellDamage.Damage > 0)
                                                            {
                                                                SetAction(SpriteAction.Hit);
                                                            }

                                                            CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                                            _ = spellDamage.Release();
                                                        }
                                                        else
                                                        {
                                                            if (message.MessageId == 200719 && message.HexNumber == 0)
                                                            {
                                                                SetDirection((byte)(Direction + 1));
                                                                if (Direction > 6)
                                                                {
                                                                    SetDirection(1);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (message.MessageId == 200720 && message.HexNumber == 0)
                                                                {
                                                                    OffsetX = Random((message.Param[0].Int * 2) + 1) - message.Param[0].Int;
                                                                    OffsetY = Random((message.Param[0].Int * 2) + 1) - message.Param[0].Int;
                                                                }
                                                                else
                                                                {
                                                                    if (message.MessageId == 200721 && message.HexNumber == 0)
                                                                    {
                                                                        OffsetX = 0;
                                                                        OffsetY = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (message.MessageId == 200708 && message.HexNumber == 0)
                                                                        {
                                                                            if (!Unit.Troop.GetMoveTypes().Contains(MoveTypes.Flying) && Owner.TerrainType != 0 && Owner.TerrainType != 9)
                                                                            {
                                                                                RandSeed = StrategicMap.Current.Random(16777215);
                                                                                spellDamage = GamePack.Current.SpellControl[112].CreateCombatAction<DamageCombatAction>(message.Param[0].As<CombatObject>(), target: Unit, chargeMana: false);
                                                                                if (spellDamage.Damage > 0)
                                                                                {
                                                                                    SetAction(SpriteAction.Hit);
                                                                                }

                                                                                CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                                                                _ = spellDamage.Release();
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // AoWTC.pas:3370
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        int x = default;
        int y = default;
        MovementHexSprite movement;
        AbilityCombatInfo abilityInfo = default;
        AbilityCombatInfo enemyInfo = default;
        if (info.HexNumber == 0)
        {
            moveResult.Code = -1;
            moveResult.Done = true;
            sbyte hexY = HexY;
            sbyte hexX = HexX;
            movement = CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].FindOwnedChild<MovementHexSprite>(MovementHexSprite.ClassId);
            if (movement is not null && movement.Enabled)
            {
                if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.Side == Unit.Side)
                {
                    moveResult.Code = 0;
                    moveResult.Done = false;
                    return;
                }
            }

            if (HexX != ((MoveControl)info.Sender).Destination.X && ((MoveControl)info.Sender).Destination.X != -1)
            {
                return;
            }

            if (HexY != ((MoveControl)info.Sender).Destination.Y && ((MoveControl)info.Sender).Destination.Y != -1)
            {
                return;
            }

            if (!((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.Move) && !((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.MoveRanged) && CombatMap.Current.UnitSelection.MoveSettingsList.GetCount() >= 2)
            {
                return;
            }

            if (((CombatMoveControl)info.Sender).UnitHexSprite.SelectedAbility <= -1 && (CombatObject.MeleeDamageTypes(((CombatMoveControl)info.Sender).UnitHexSprite.Unit) == DamageTypes.None || (Unit.GetAbilityEnabled(1) && !((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(1))) && !((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.MoveRanged) && !((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.Move))
            {
                return;
            }

            if (((CombatMoveControl)info.Sender).UnitHexSprite.SelectedAbility == -1)
            {
                if (!((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.Move) && !((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModes.MoveRanged))
                {
                    if (Unit.Side == ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.Side)
                    {
                        return;
                    }
                }

                moveResult.Code = 0;
                return;
            }

            GamePack.Current.AbilityControl[((CombatMoveControl)info.Sender).UnitHexSprite.SelectedAbility].GetCombatInfo(((CombatMoveControl)info.Sender).UnitHexSprite.Unit.Troop, info: ref abilityInfo);
            if (abilityInfo.TargetTypes.Contains(CombatTargetTypes.Friend) && Unit.Side == ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.Side)
            {
                goto Touch;
            }

            GamePack.Current.AbilityControl[((CombatMoveControl)info.Sender).UnitHexSprite.SelectedAbility].GetCombatInfo(((CombatMoveControl)info.Sender).UnitHexSprite.Unit.Troop, info: ref enemyInfo);
            if (!enemyInfo.TargetTypes.Contains(CombatTargetTypes.Enemy))
            {
                return;
            }

            if (Unit.Side == ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.Side)
            {
                return;
            }

        Touch:
            if (GamePack.Current.AbilityControl.GetAbility<TouchAbility>(((CombatMoveControl)info.Sender).UnitHexSprite.SelectedAbility).CanTouch(((CombatMoveControl)info.Sender).UnitHexSprite.Unit, target: Unit))
            {
                moveResult.Code = 0;
            }
        }
        else
        {
            sbyte hexY = HexY;
            HexGeometry.CenterHexNumberToHex(HexX, hexY, info.HexNumber, ref x, ref y);
            if (moveResult.Code < 30)
            {
                moveResult.Code = ((MoveControl)info.Sender).MovePointTable[info.MapField.TerrainType][info.MapField.OverlayType - MovementTables.FirstOverlay];
            }

            if (moveResult.Code <= -1)
            {
                return;
            }

            if (info.HexNumber >= 7)
            {
                return;
            }

            if (HexX == ((MoveControl)info.Sender).Destination.X && HexY == ((MoveControl)info.Sender).Destination.Y)
            {
                return;
            }

            if (Unit.Side == ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.Side)
            {
                return;
            }

            if (CombatObject.MeleeDamageTypes(Unit) == DamageTypes.None)
            {
                return;
            }

            if (Unit.GetLocked())
            {
                return;
            }

            if (Unit.GetAbilityEnabled(1) != ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(1))
            {
                return;
            }

            if (((MoveControl)info.Sender).Destination.X >= 0 && ((MoveControl)info.Sender).Destination.Y >= 0 && HexGeometry.DeltaHexToRadius(x, y, ((MoveControl)info.Sender).Destination.X, ((MoveControl)info.Sender).Destination.Y) <= 1)
            {
                if (CombatMap.Current.MapContainer.MapLevels[0].Field[((MoveControl)info.Sender).Destination.X, ((MoveControl)info.Sender).Destination.Y].FindOwnedChild(ClassId) is not null)
                {
                    return;
                }
            }

            if (moveResult.Code > 96)
            {
                moveResult.Code = 127;
                return;
            }

            moveResult.Code += 30;
        }
    }

    // AoWTC.pas:3447
    public override void Activate()
    {
        base.Activate();
        SetUnitGraphicsResourceIndex(Unit.Troop.UnitGraphicsResourceIndex);
        Abilities = [];
        CreateTacticalBattleAbilityList();
        CombatMap.Current.FrameStarted += NewFrame;
        if (CombatMap.Current.ExplorationEnabled)
        {
            sbyte player = Unit.GetPlayer();
            int visibilityRange = Unit.GetVisibilityRange();
            sbyte hexY = HexY;
            sbyte hexX = HexX;
            _ = CombatMap.Current.ExploreArea(hexX, hexY, 0, player, range: visibilityRange);
        }

        ActionFrame = -1;
        SelectedAbility = -1;
        SelectedSpell = -1;
    }

    // AoWTC.pas:3339
    public override void Deactivate()
    {
        CombatMap.Current?.FrameStarted -= NewFrame;

        for (int i = 0, last = Abilities.Count - 1; i <= last; i++)
        {
            (Abilities[i] as IDisposable)?.Dispose();
        }

        Abilities?.Clear();
        base.Deactivate();
    }

    // AoWTC.pas:3439
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber == 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOnFlag | MapField.MoveOverFlag | MapField.OwnerFlag;
        }
        else
        {
            if (info.HexNumber <= 6)
            {
                info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOnFlag;
            }
        }
    }

    // AoWTC.pas:3784
    public override void Select()
    {
        base.Select();
        CreateTacticalBattleAbilityList();
        SelectedAbility = -1;
        if (CombatMap.Current.UnitSelection.TacticalBattleUnitHexSprite != this)
        {
            CombatMap.Current.UnitSelection.SetTacticalBattleUnitHexSprite(this);
        }

        _ = UnitGraphicsResource.SfxLibraries[0].Play(0);
        if (CombatMap.Current.SeatedCombatPlayer == Unit.CombatPlayer)
        {
            CombatMap.Current.Players.GetPlayers(CombatMap.Current.SeatedCombatPlayer).LastUnit = Unit;
        }
    }

    // AoWTC.pas:3796
    public override void Unselect()
    {
        base.Unselect();
        if (CombatMap.Current is not null)
        {
            if (CombatMap.Current.UnitSelection.TacticalBattleUnitHexSprite == this)
            {
                CombatMap.Current.UnitSelection.Unselect();
            }
        }
    }

    // AoWTC.pas:3357
    public override int GetMoveSteps()
    {
        int result;
        switch (GetEngine().Registry.GetMovementSpeed())
        {
            case GameSettings.MovementSpeed.Fast:
                result = base.GetMoveSteps();
                return result > 8 ? result - 4 : 4;
            case GameSettings.MovementSpeed.Turbo:
                return 2;
            case GameSettings.MovementSpeed.Normal:
            default:
                return base.GetMoveSteps();
        }
    }

    // AoWTC.pas:3351
    public void Initialize(TacticalBattleUnit U)
    {
        Unit = U;
        Unit.UnitHexSprite = this;
    }

    // AoWTC.pas:3804
    public bool MeleeMoveTacticalBattle(MovePath path)
    {
        sbyte player = Unit.GetPlayer();
        if (StrategicMap.Current.TokenManager.PlayerReady(player) && path.Count > 1)
        {
            TacticalBattleMeleeMoveTokenEvent? tokenEvent = CreateTacticalBattleMeleeMoveTokenEvent(Unit, path);
            if (tokenEvent is not null)
            {
                sbyte fUnitPlayer = Unit.GetPlayer();
                StrategicMap.Current.TokenManager.GetTokenControl(fUnitPlayer).AddEvent(tokenEvent);
                _ = tokenEvent.Release();
                return true;
            }
        }

        return false;
    }

    // AoWTC.pas:3836
    public int SelectAbility(int abilityId)
    {
        CombatMap.Current.UnitSelection.DeactivateRangedPath();
        SelectedAbility = -1;
        int result = -1;
        if (CombatMap.Current.CurrentCombatPlayer == CombatMap.Current.SeatedCombatPlayer)
        {
            int index = -1;
            for (int i = 0, last = Abilities.Count - 1; i <= last; i++)
            {
                if (Abilities[i].Ability == abilityId)
                {
                    index = i;
                }
            }

            if (!Unit.GetLocked() && Unit.MovePoints > 0 && Unit.CombatPlayer == CombatMap.Current.CurrentCombatPlayer && index < CombatAbilityCount && index > -1)
            {
                if (abilityId == 52)
                {
                    if (Unit.Troop.Moves == Unit.MovePoints)
                    {
                        _ = GamePack.Current.AbilityControl[abilityId].ActivateCombat(Unit.Troop, reason: out _);
                        SelectedSpell = -1;
                        SelectedAbility = abilityId;
                        result = index;
                    }
                }
                else
                {
                    CombatMode combatMode = GamePack.Current.AbilityControl[abilityId].GetCombatMode(Unit.Troop);
                    if (combatMode is >= CombatMode.ShortRange and <= CombatMode.LongRange)
                    {
                        if (Unit.Troop.Moves == Unit.MovePoints)
                        {
                            CombatMap.Current.UnitSelection.RangedPathHexSprite.Range = GamePack.Current.AbilityControl[abilityId].GetCombatMode(Unit.Troop) == CombatMode.ShortRange
                                ? (ushort)4
: GamePack.Current.AbilityControl[abilityId].GetCombatMode(Unit.Troop) == CombatMode.MediumRange
                                    ? (ushort)8
                                    : (ushort)12;

                            CombatMap.Current.UnitSelection.RangedPathHexSprite.Selection = CombatMap.Current.UnitSelection;
                            _ = CombatMap.Current.DynamicFixedVisibleHexSprite.AddChild(CombatMap.Current.UnitSelection.RangedPathHexSprite);
                            SelectedAbility = abilityId;
                            result = index;
                        }
                    }
                    else
                    {
                        SelectedAbility = abilityId;
                        result = index;
                    }
                }
            }
        }

        for (int i = 0, last = CombatMap.Current.UnitSelection.MoveSettingsList.GetCount() - 1; i <= last; i++)
        {
            MoveSettings moveSettings = CombatMap.Current.UnitSelection.MoveSettingsList.GetMoveSettings(i);
            moveSettings.Path.Clear();
        }

        CombatMap.Current.UnitSelection.RangedPath.Clear();
        CombatMap.Current.UnitSelection.UpdateMoveCursor();
        CombatMap.Current.UnitChangedEventsStore.Raise(this);
        return result;
    }

    // AoWTC.pas:3870
    public int SelectSpell(int spellId)
    {
        SpellCombatInfo info = default;
        CombatMap.Current.UnitSelection.DeactivateRangedPath();
        SelectedSpell = spellId;
        GamePack.Current.SpellControl[SelectedSpell].GetCombatInfo(Unit.Troop, ref info);
        if (info.Range != 0)
        {
            CombatMap.Current.UnitSelection.RangedPathHexSprite.Range = info.Range == -1 ? (ushort)100 : (ushort)info.Range;

            CombatMap.Current.UnitSelection.RangedPathHexSprite.Selection = CombatMap.Current.UnitSelection;
            _ = CombatMap.Current.DynamicFixedVisibleHexSprite.AddChild(CombatMap.Current.UnitSelection.RangedPathHexSprite);
        }

        int result = spellId;
        for (int i = 0, last = CombatMap.Current.UnitSelection.MoveSettingsList.GetCount() - 1; i <= last; i++)
        {
            MoveSettings moveSettings = CombatMap.Current.UnitSelection.MoveSettingsList.GetMoveSettings(i);
            moveSettings.Path.Clear();
        }

        CombatMap.Current.UnitSelection.RangedPath.Clear();
        switch (TacticalTables.SpellTypes[spellId])
        {
            case BattleEffectType.ScreenNESW:
            case BattleEffectType.SacredWrath:
            case BattleEffectType.Tremors:
            case BattleEffectType.HighPrayer:
            case BattleEffectType.ShockWave:
                _ = AbilityRangedTacticalBattle(0, 0);
                result = -1;
                Unselect();
                break;
            case BattleEffectType.None:
            case BattleEffectType.Arrow:
            case BattleEffectType.Rock:
            case BattleEffectType.Comet:
            case BattleEffectType.Touch:
            case BattleEffectType.Breath:
            case BattleEffectType.CometBall:
            case BattleEffectType.Effect:
            case BattleEffectType.FBExplosion:
            case BattleEffectType.ScreenNS:
            case BattleEffectType.Ray:
            case BattleEffectType.Chain:
            case BattleEffectType.Swarm:
            case BattleEffectType.ScreenSwarm:
            case BattleEffectType.SFOverlay:
            case BattleEffectType.DamLightning:
            case BattleEffectType.DamFire:
            case BattleEffectType.DamPoison:
            case BattleEffectType.DamMagic:
            case BattleEffectType.DamHoly:
            case BattleEffectType.DamDeath:
            case BattleEffectType.DamCold:
            case BattleEffectType.SWComet:
            case BattleEffectType.Dispel:
            case BattleEffectType.WallSmoke:
            case BattleEffectType.Blood:
            case BattleEffectType.AltArrow:
            case BattleEffectType.Geyser:
            case BattleEffectType.GeyserWater:
            case BattleEffectType.HailImpact:
            case BattleEffectType.Cloud:
            case BattleEffectType.CloudPuff:
            case BattleEffectType.HitGround:
            case BattleEffectType.HitWater:
            case BattleEffectType.PrayerGlow:
            case BattleEffectType.Resurrect:
            case BattleEffectType.FrostBeam:
            case BattleEffectType.Frost:
            case BattleEffectType.MindDecay:
            case BattleEffectType.DecayLeg:
            case BattleEffectType.DecayComet:
            case BattleEffectType.SacFlame:
            case BattleEffectType.Winds:
            case BattleEffectType.SacFire:
            case BattleEffectType.BreathDam:
            default:
                break;
        }

        CombatMap.Current.UnitSelection.UpdateMoveCursor();
        CombatMap.Current.UnitChangedEventsStore.Raise(this);
        return result;
    }

    // AoWTC.pas:3895
    public void CreateTacticalBattleAbilityList()
    {
        for (int i = 0, last = Abilities.Count - 1; i <= last; i++)
        {
            (Abilities[i] as IDisposable)?.Dispose();
        }

        Abilities.Clear();
        CombatAbilityCount = 0;
        for (int i = 1; i <= 169; i++)
        {
            if (Unit.GetAbilityEnabled(i))
            {
                CombatAbilityRef ability = new()
                {
                    Ability = i
                };
                if (GamePack.Current.AbilityControl[i].GetControlType(Unit.Troop).Contains(AbilityControlTypes.CombatAction) && GamePack.Current.AbilityControl[i].GetCombatMode(Unit.Troop) != CombatMode.Melee)
                {
                    Abilities.Insert(CombatAbilityCount, item: ability);
                    CombatAbilityCount += 1;
                }
                else
                {
                    Abilities.Add(ability);
                }
            }
        }
    }

    // AoWTC.pas:3910
    public void ListAbilities(DelphiStringCollection names, DelphiStringCollection? types, DelphiStringCollection? sources)
    {
        names.Clear();
        types?.Clear();

        sources?.Clear();

        for (int i = 0, last = Abilities.Count - 1; i <= last; i++)
        {
            CombatAbilityRef ability = Abilities[i];
            _ = names.AddObject(GamePack.Current.AbilityControl[ability.Ability].GetName(Unit.Troop), item: IntToObject(Abilities[i].Ability));
            if (types is not null)
            {
                CombatAbilityRef ability2 = Abilities[i];
                AbilityType abilityType = GamePack.Current.AbilityControl[ability2.Ability].GetAbilityType();
                _ = types.Add(AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.AbilityTypeTexts[(int)abilityType]));
            }

            if (sources is not null)
            {
                CombatAbilityRef ability2 = Abilities[i];
                _ = sources.Add(GamePack.Current.AbilityControl[ability2.Ability].GetSourceName(Unit.Troop));
            }
        }
    }

    // AoWTC.pas:3923
    public void SetAction(SpriteAction action)
    {
        ActionDone = false;
        switch (action)
        {
            case SpriteAction.StepUp:
            case SpriteAction.StepBack:
                ActionFrame = 0;
                break;
            case SpriteAction.RoundAttack:
            case SpriteAction.RoundAttackHit:
                if (Action is not SpriteAction.RoundAttack and not SpriteAction.RoundAttackHit)
                {
                    ActionFrame = -1;
                }

                break;
            case SpriteAction.Move:
            case SpriteAction.MeleeHit:
            case SpriteAction.MeleeMiss:
            case SpriteAction.Ability:
            case SpriteAction.Hit:
            case SpriteAction.Killed:
            case SpriteAction.Busy:
            case SpriteAction.FreeStruck:
            case SpriteAction.BreathAttack:
            default:
                ActionFrame = -1;
                break;
        }

        Action = action;
    }

    // AoWTC.pas:3935
    public bool AbilityTouchMoveTacticalBattle(MovePath path)
    {
        sbyte player = Unit.GetPlayer();
        if (StrategicMap.Current.TokenManager.PlayerReady(player) && path.Count > 0)
        {
            if (path.Count > 1)
            {
                TokenEvent? tokenEvent = CreateTacticalBattleAbilityTouchMoveTokenEvent(Unit, path, abilityId: SelectedAbility);
                if (tokenEvent is not null)
                {
                    sbyte fUnitPlayer = Unit.GetPlayer();
                    StrategicMap.Current.TokenManager.GetTokenControl(fUnitPlayer).AddEvent(tokenEvent);
                    _ = tokenEvent.Release();
                    return true;
                }
            }
            else
            {
                AbilityCombatInfo buffer = default;
                GamePack.Current.AbilityControl[SelectedAbility].GetCombatInfo(Unit.Troop, info: ref buffer);
                if (buffer.TargetTypes.Contains(CombatTargetTypes.Friend))
                {
                    PathStep pathLocal = path[0];
                    _ = AbilityRangedTacticalBattle((ushort)path[0].Location.X, (ushort)pathLocal.Location.Y);
                }
            }
        }

        return false;
    }

    // AoWTC.pas:3952
    public bool AbilityRangedTacticalBattle(ushort X, ushort Y)
    {
        sbyte player = Unit.GetPlayer();
        if (StrategicMap.Current.TokenManager.PlayerReady(player))
        {
            CombatAbilityRangedTokenEvent? tokenEvent = CreateTacticalBattleAbilityRangedTokenEvent(Unit, X, Y, abilityId: SelectedAbility, spellId: SelectedSpell);
            if (tokenEvent is not null)
            {
                sbyte fUnitPlayer = Unit.GetPlayer();
                StrategicMap.Current.TokenManager.GetTokenControl(fUnitPlayer).AddEvent(tokenEvent);
                _ = tokenEvent.Release();
                return true;
            }
        }

        return false;
    }

    private static ReadOnlySpan<int> StatusAbilities => [149, 150, 48, 92, 93, 94, 95, 96, 97, 98, 107, 163, 127, 34];

    public static void OriginalUnitAssert(string message, string fileName, int lineNumber) => Assert(false, message, file: fileName, line: lineNumber);

    private static TacticalBattleMeleeMoveTokenEvent? CreateTacticalBattleMeleeMoveTokenEvent(TacticalBattleUnit unitObject, MovePath path)
    {
        TacticalBattleMeleeMoveTokenEvent tacticalBattleMeleeMoveTokenEvent = new(null) { Holder = unitObject };
        _ = (tacticalBattleMeleeMoveTokenEvent?.Setup(unitObject, path));
        return tacticalBattleMeleeMoveTokenEvent;
    }

    private static CombatAbilityTouchMoveTokenEvent? CreateTacticalBattleAbilityTouchMoveTokenEvent(TacticalBattleUnit unitObject, MovePath path, int abilityId)
    {
        CombatAbilityTouchMoveTokenEvent tacticalBattleAbilityTouchMoveTokenEvent = new(null) { Holder = unitObject };
        if (tacticalBattleAbilityTouchMoveTokenEvent is not null)
        {
            _ = tacticalBattleAbilityTouchMoveTokenEvent.Setup(unitObject, path);
            tacticalBattleAbilityTouchMoveTokenEvent.AbilityId = abilityId;
        }
        return tacticalBattleAbilityTouchMoveTokenEvent;
    }

    private static CombatAbilityRangedTokenEvent? CreateTacticalBattleAbilityRangedTokenEvent(TacticalBattleUnit unitObject, ushort X, ushort Y, int abilityId, int spellId)
    {
        CombatAbilityRangedTokenEvent tacticalBattleAbilityRangedTokenEvent = new(null) { Holder = unitObject };
        if (tacticalBattleAbilityRangedTokenEvent is not null)
        {
            tacticalBattleAbilityRangedTokenEvent.SourceX = (ushort)unitObject.UnitHexSprite.HexX;
            tacticalBattleAbilityRangedTokenEvent.SourceY = (ushort)unitObject.UnitHexSprite.HexY;
            tacticalBattleAbilityRangedTokenEvent.TargetX = X;
            tacticalBattleAbilityRangedTokenEvent.TargetY = Y;
            tacticalBattleAbilityRangedTokenEvent.AbilityId = abilityId;
            tacticalBattleAbilityRangedTokenEvent.SpellId = spellId;
        }
        return tacticalBattleAbilityRangedTokenEvent;
    }

    public enum SpriteAction : byte
    {
        Move = 0, MeleeHit = 1, MeleeMiss = 2, Ability = 3, Hit = 4, Killed = 5, StepUp = 6, StepBack = 7, Busy = 8, RoundAttack = 9, RoundAttackHit = 10, FreeStruck = 11, BreathAttack = 12
    }
}
