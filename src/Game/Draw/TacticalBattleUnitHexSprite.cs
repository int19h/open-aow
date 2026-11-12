// Translated from AoWTC.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// Unit. MoveIndicator. IndicatorStep. Action. ActionDone. ActionFrame. OffsetX.
// OffsetY. SelectedAbility. SelectedSpell. CombatAbilityCount. Abilities. Busy
public partial class TacticalBattleUnitHexSprite : UnitHexSprite
{
    public TacticalBattleUnitHexSprite(ECustomNode owner) : base(owner)
    { }

    // AoWTC.pas:3348
    public new const int ClassId = 2228484;

    public TacticalBattleUnit Unit { get; set; }

    private short _moveIndicator;
    private short _indicatorStep;

    public SpriteAction Action { get; private set; }
    public bool ActionDone { get; private set; }
    public short ActionFrame { get; private set; }

    public int OffsetX;
    public int OffsetY;

    public int SelectedAbility { get; set; }
    public int SelectedSpell { get; set; }
    public int CombatAbilityCount { get; private set; }
    public List<CombatAbilityRef> Abilities { get; private set; }
    public bool Busy { get; set; }

    // AoWTC.pas:3566
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId != 536936471 && (message.MessageId & 1073741824) == 0 && Unit != null)
        {
            Unit.MessageProc(ref message, sender);
        }
    }

    // AoWTC.pas:3461
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        SelfDestructCombatAction selfDestruct;
        DamageCombatAction spellDamage;
        RangedAttackCombatAction rangedDamage;
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 4364)
        {
            if (message.HexNumber == 0 && (!CombatMap.Current.FogEnabled || Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((AoWMapField)Owner).FieldFlags).Hi).Lo != 0))
            {
                if (Unit.GetPlayer() != 0)
                {
                    var player = Unit.GetPlayer();
                    var playerPlayerRace = AoWHSMap.Current.Players.GetPlayers(player);
                    message.Param[0] = message.Param[0].Int | (playerPlayerRace.PlayerRace + 1);
                }
                else
                {
                    message.Param[0] = message.Param[0].Int | 13;
                }
            }
        }
        else
        {
            if (message.MessageId == 4352 && message.HexNumber == 0)
            {
                var player = Unit.GetPlayer();
                message.Result = player;
            }
            else
            {
                if (message.MessageId == 200704 && message.HexNumber == 0)
                {
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    selfDestruct = ((SelfDestructAbility)AoWHSSet.Current.AbilityControl.GetAbility(65)).CreateSelfDestructCombatAction(message.Param[0].As<CombatObject>(), Unit);
                    if (selfDestruct.Damage > 0)
                    {
                        SetAction(SpriteAction.Hit);
                    }

                    CombatMap.Current.Combat.ExecuteCombatAction(selfDestruct);
                    selfDestruct.Release();
                }
                else
                {
                    if (message.MessageId == 200707 && message.HexNumber == 0)
                    {
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        spellDamage = (DamageCombatAction)AoWHSSet.Current.SpellControl.GetSpell(117).CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                        if (spellDamage.Damage > 0)
                        {
                            SetAction(SpriteAction.Hit);
                        }

                        CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                        spellDamage.Release();
                    }
                    else
                    {
                        if (message.MessageId == 200715 && message.HexNumber == 0)
                        {
                            RandSeed = AoWHSMap.Current.Random(16777215);
                            spellDamage = (DamageCombatAction)AoWHSSet.Current.SpellControl.GetSpell(113).CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                            if (spellDamage.Damage > 0)
                            {
                                SetAction(SpriteAction.Hit);
                            }

                            CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                            spellDamage.Release();
                        }
                        else
                        {
                            if (message.MessageId == 200714 && message.HexNumber == 0)
                            {
                                RandSeed = AoWHSMap.Current.Random(16777215);
                                rangedDamage = ((RangedAttackAbility)AoWHSSet.Current.AbilityControl.GetAbility(31)).CreateRangedAttackCombatAction(message.Param[0].As<CombatObject>(), Unit, 0);
                                if (rangedDamage.Damage > 0)
                                {
                                    SetAction(SpriteAction.Hit);
                                }

                                CombatMap.Current.Combat.ExecuteCombatAction(rangedDamage);
                                rangedDamage.Release();
                            }
                            else
                            {
                                if (message.MessageId == 200723 && message.HexNumber == 0)
                                {
                                    RandSeed = AoWHSMap.Current.Random(16777215);
                                    spellDamage = (DamageCombatAction)AoWHSSet.Current.SpellControl.GetSpell(message.Param[1].Int).CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                                    if (spellDamage.Damage > 0)
                                    {
                                        SetAction(SpriteAction.Hit);
                                    }

                                    CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                    spellDamage.Release();
                                }
                                else
                                {
                                    if (message.MessageId == 200722 && message.HexNumber == 0)
                                    {
                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                        rangedDamage = ((RangedAttackAbility)AoWHSSet.Current.AbilityControl.GetAbility(message.Param[1].Int)).CreateRangedAttackCombatAction(message.Param[0].As<CombatObject>(), Unit, 0);
                                        if (rangedDamage.Damage > 0)
                                        {
                                            SetAction(SpriteAction.Hit);
                                        }

                                        CombatMap.Current.Combat.ExecuteCombatAction(rangedDamage);
                                        rangedDamage.Release();
                                    }
                                    else
                                    {
                                        if (message.MessageId == 200709 && message.HexNumber == 0)
                                        {
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            spellDamage = (DamageCombatAction)AoWHSSet.Current.SpellControl.GetSpell(104).CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                                            if (spellDamage.Damage > 0)
                                            {
                                                SetAction(SpriteAction.Hit);
                                            }

                                            CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                            spellDamage.Release();
                                        }
                                        else
                                        {
                                            if (message.MessageId == 200716 && message.HexNumber == 0)
                                            {
                                                RandSeed = AoWHSMap.Current.Random(16777215);
                                                spellDamage = (DamageCombatAction)AoWHSSet.Current.SpellControl.GetSpell(120).CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                                                if (spellDamage.Damage > 0)
                                                {
                                                    SetAction(SpriteAction.Hit);
                                                }

                                                CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                                spellDamage.Release();
                                            }
                                            else
                                            {
                                                if (message.MessageId == 200710 && message.HexNumber == 0)
                                                {
                                                    RandSeed = AoWHSMap.Current.Random(16777215);
                                                    spellDamage = (DamageCombatAction)AoWHSSet.Current.SpellControl.GetSpell(116).CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                                                    if (spellDamage.Damage > 0)
                                                    {
                                                        SetAction(SpriteAction.Hit);
                                                    }

                                                    CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                                    spellDamage.Release();
                                                }
                                                else
                                                {
                                                    if (message.MessageId == 200717 && message.HexNumber == 0)
                                                    {
                                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                                        spellDamage = (DamageCombatAction)AoWHSSet.Current.SpellControl.GetSpell(127).CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                                                        if (spellDamage.Damage > 0)
                                                        {
                                                            SetAction(SpriteAction.Hit);
                                                        }

                                                        CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                                        spellDamage.Release();
                                                    }
                                                    else
                                                    {
                                                        if (message.MessageId == 200718 && message.HexNumber == 0)
                                                        {
                                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                                            spellDamage = (DamageCombatAction)AoWHSSet.Current.SpellControl.GetSpell(128).CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                                                            if (spellDamage.Damage > 0)
                                                            {
                                                                SetAction(SpriteAction.Hit);
                                                            }

                                                            CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                                            spellDamage.Release();
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
                                                                    OffsetX = Random(message.Param[0].Int * 2 + 1) - message.Param[0].Int;
                                                                    OffsetY = Random(message.Param[0].Int * 2 + 1) - message.Param[0].Int;
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
                                                                            if (!Unit.Troop.GetMoveTypes().Contains(MoveType.Flying) && Owner.TerrainType != 0 && Owner.TerrainType != 9)
                                                                            {
                                                                                RandSeed = AoWHSMap.Current.Random(16777215);
                                                                                spellDamage = (DamageCombatAction)AoWHSSet.Current.SpellControl.GetSpell(112).CreateCombatAction(message.Param[0].As<CombatObject>(), Unit, false);
                                                                                if (spellDamage.Damage > 0)
                                                                                {
                                                                                    SetAction(SpriteAction.Hit);
                                                                                }

                                                                                CombatMap.Current.Combat.ExecuteCombatAction(spellDamage);
                                                                                spellDamage.Release();
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
        int X = default;
        int Y = default;
        MovementHexSprite movement;
        AbilityCombatInfo abilityInfo = default;
        AbilityCombatInfo enemyInfo = default;
        if (info.HexNumber == 0)
        {
            moveResult.Code = -1;
            moveResult.Done = true;
            var hexY = GetHexY();
            var hexX = GetHexX();
            movement = (MovementHexSprite)CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].FindOwnedChild(2228490);
            if (movement != null && movement.Enabled)
            {
                if (((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetSide() == Unit.GetSide())
                {
                    moveResult.Code = 0;
                    moveResult.Done = false;
                    return;
                }
            }

            if (GetHexX() != ((MoveControl)info.Sender).DestinationX && ((MoveControl)info.Sender).DestinationX != -1)
            {
                return;
            }

            if (GetHexY() != ((MoveControl)info.Sender).DestinationY && ((MoveControl)info.Sender).DestinationY != -1)
            {
                return;
            }

            if (!((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.Move) && !((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.MoveRanged) && CombatMap.Current.UnitSelection.MoveSettingsList.GetCount() >= 2)
            {
                return;
            }

            if (((CombatMoveControl)info.Sender).UnitHexSprite.SelectedAbility <= -1 && (CombatObject.MeleeDamageTypes(((CombatMoveControl)info.Sender).UnitHexSprite.Unit) == Set<DamageType>.Of() || Unit.GetAbilityEnabled(1) && !((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetAbilityEnabled(1)) && !((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.MoveRanged) && !((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.Move))
            {
                return;
            }

            if (((CombatMoveControl)info.Sender).UnitHexSprite.SelectedAbility == -1)
            {
                if (!((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.Move) && !((CombatMoveControl)info.Sender).MoveFlags.Contains(CombatMoveModeElement.MoveRanged))
                {
                    if (Unit.GetSide() == ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetSide())
                    {
                        return;
                    }
                }

                moveResult.Code = 0;
                return;
            }

            AoWHSSet.Current.AbilityControl.GetAbility(((CombatMoveControl)info.Sender).UnitHexSprite.SelectedAbility).GetCombatInfo(((CombatMoveControl)info.Sender).UnitHexSprite.Unit.Troop, ref abilityInfo);
            if (abilityInfo.TargetTypes.Contains(CombatTargetTypeElement.Friend) && Unit.GetSide() == ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetSide())
            {
                goto Touch;
            }

            AoWHSSet.Current.AbilityControl.GetAbility(((CombatMoveControl)info.Sender).UnitHexSprite.SelectedAbility).GetCombatInfo(((CombatMoveControl)info.Sender).UnitHexSprite.Unit.Troop, ref enemyInfo);
            if (!enemyInfo.TargetTypes.Contains(CombatTargetTypeElement.Enemy))
            {
                return;
            }

            if (Unit.GetSide() == ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetSide())
            {
                return;
            }

        Touch:
            if (((TouchAbility)AoWHSSet.Current.AbilityControl.GetAbility(((CombatMoveControl)info.Sender).UnitHexSprite.SelectedAbility)).CanTouch(((CombatMoveControl)info.Sender).UnitHexSprite.Unit, Unit))
            {
                moveResult.Code = 0;
            }
        }
        else
        {
            var hexY = GetHexY();
            HexGeometry.CenterHexNumberToHex(GetHexX(), hexY, info.HexNumber, ref X, ref Y);
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

            if (GetHexX() == ((MoveControl)info.Sender).DestinationX && GetHexY() == ((MoveControl)info.Sender).DestinationY)
            {
                return;
            }

            if (Unit.GetSide() == ((CombatMoveControl)info.Sender).UnitHexSprite.Unit.GetSide())
            {
                return;
            }

            if (CombatObject.MeleeDamageTypes(Unit) == Set<DamageType>.Of())
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

            if (((MoveControl)info.Sender).DestinationX >= 0 && ((MoveControl)info.Sender).DestinationY >= 0 && HexGeometry.DeltaHexToRadius(X, Y, ((MoveControl)info.Sender).DestinationX, ((MoveControl)info.Sender).DestinationY) <= 1)
            {
                if (CombatMap.Current.MapContainer.MapLevels[0].Field[((MoveControl)info.Sender).DestinationX, ((MoveControl)info.Sender).DestinationY].FindOwnedChild(2228484) != null)
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
        SetUnitGraphicsResourceIndex(Unit.Troop.GetUnitGraphicsResourceIndex());
        Abilities = new List<CombatAbilityRef>();
        CreateTacticalBattleAbilityList();
        CombatMap.Current.NewFrameEvents += NewFrame;
        if (CombatMap.Current.ExplorationEnabled)
        {
            var player = Unit.GetPlayer();
            var visibilityRange = Unit.GetVisibilityRange();
            var hexY = GetHexY();
            var hexX = GetHexX();
            CombatMap.Current.ExploreArea(hexX, hexY, 0, player, visibilityRange);
        }

        ActionFrame = -1;
        SelectedAbility = -1;
        SelectedSpell = -1;
    }

    // AoWTC.pas:3339
    public override void Deactivate()
    {
        if (CombatMap.Current != null)
        {
            CombatMap.Current.NewFrameEvents -= NewFrame;
        }

        for (int I = 0, last = Abilities.Count - 1; I <= last; I++)
        {
            (Abilities[I] as IDisposable)?.Dispose();
        }

        Abilities?.Clear();
        base.Deactivate();
    }

    // AoWTC.pas:3439
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber == 0)
        {
            ((AoWMapField)info.MapField).FieldFlags = ((AoWMapField)info.MapField).FieldFlags | 16384 | 8192 | 33554432;
        }
        else
        {
            if (info.HexNumber <= 6)
            {
                ((AoWMapField)info.MapField).FieldFlags = ((AoWMapField)info.MapField).FieldFlags | 16384;
            }
        }
    }

    // AoWTC.pas:3784
    public override void Select()
    {
        base.Select();
        CreateTacticalBattleAbilityList();
        SelectedAbility = -1;
        if (CombatMap.Current.UnitSelection.GetTacticalBattleUnitHexSprite() != this)
        {
            CombatMap.Current.UnitSelection.SetTacticalBattleUnitHexSprite(this);
        }

        UnitGraphicsResource.SfxLibraries[0].Play(0);
        if (CombatMap.Current.SeatedCombatPlayer == Unit.CombatPlayer)
        {
            CombatMap.Current.Players.GetPlayers(CombatMap.Current.SeatedCombatPlayer).LastUnit = Unit;
        }
    }

    // AoWTC.pas:3796
    public override void Unselect()
    {
        base.Unselect();
        if (CombatMap.Current != null)
        {
            if (CombatMap.Current.UnitSelection.GetTacticalBattleUnitHexSprite() == this)
            {
                CombatMap.Current.UnitSelection.Unselect();
            }
        }
    }

    // AoWTC.pas:3357
    public override int GetMoveSteps()
    {
        int result;
        switch (GameTables.GameEngine.Registry.GetMovementSpeed())
        {
            case GameSettings.MovementSpeed.Fast:
                result = base.GetMoveSteps();
                if (result > 8)
                {
                    result = result - 4;
                }
                else
                {
                    result = 4;
                }

                break;
            case GameSettings.MovementSpeed.Turbo:
                result = 2;
                break;
            default:
                result = base.GetMoveSteps();
                break;
        }

        return result;
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
        TacticalBattleMeleeMoveTokenEvent tokenEvent;
        bool result = false;
        var player = Unit.GetPlayer();
        if (AoWHSMap.Current.TokenManager.PlayerReady(player) && path.Count > 1)
        {
            tokenEvent = CreateTacticalBattleMeleeMoveTokenEvent(Unit, path);
            if (tokenEvent != null)
            {
                var fUnitPlayer = Unit.GetPlayer();
                AoWHSMap.Current.TokenManager.GetTokenControl(fUnitPlayer).AddEvent(tokenEvent);
                tokenEvent.Release();
                result = true;
            }
        }

        return result;
    }

    // AoWTC.pas:3836
    public int SelectAbility(int abilityId)
    {
        int index;
        string reason = "";
        CombatMap.Current.UnitSelection.DeactivateRangedPath();
        SelectedAbility = -1;
        int result = -1;
        index = -1;
        if (CombatMap.Current.CurrentCombatPlayer == CombatMap.Current.SeatedCombatPlayer)
        {
            for (int I = 0, last = Abilities.Count - 1; I <= last; I++)
            {
                if ((Abilities[I]).Ability == abilityId)
                {
                    index = I;
                }
            }

            if (!Unit.GetLocked() && Unit.MovePoints > 0 && Unit.CombatPlayer == CombatMap.Current.CurrentCombatPlayer && index < CombatAbilityCount && index > -1)
            {
                if (abilityId == 52)
                {
                    if (Unit.Troop.GetMoves() == Unit.MovePoints)
                    {
                        AoWHSSet.Current.AbilityControl.GetAbility(abilityId).ActivateCombat(Unit.Troop, ref reason);
                        SelectedSpell = -1;
                        SelectedAbility = abilityId;
                        result = index;
                    }
                }
                else
                {
                    var combatMode = AoWHSSet.Current.AbilityControl.GetAbility(abilityId).GetCombatMode(Unit.Troop);
                    if (Set<CombatMode>.Of(CombatMode.ShortRange, CombatMode.MediumRange, CombatMode.LongRange).Contains(combatMode))
                    {
                        if (Unit.Troop.GetMoves() == Unit.MovePoints)
                        {
                            if (AoWHSSet.Current.AbilityControl.GetAbility(abilityId).GetCombatMode(Unit.Troop) == CombatMode.ShortRange)
                            {
                                CombatMap.Current.UnitSelection.RangedPathHexSprite.Range = 4;
                            }
                            else
                            {
                                if (AoWHSSet.Current.AbilityControl.GetAbility(abilityId).GetCombatMode(Unit.Troop) == CombatMode.MediumRange)
                                {
                                    CombatMap.Current.UnitSelection.RangedPathHexSprite.Range = 8;
                                }
                                else
                                {
                                    CombatMap.Current.UnitSelection.RangedPathHexSprite.Range = 12;
                                }
                            }

                            CombatMap.Current.UnitSelection.RangedPathHexSprite.Selection = CombatMap.Current.UnitSelection;
                            CombatMap.Current.DynamicFixedVisibleHexSprite.AddChild(CombatMap.Current.UnitSelection.RangedPathHexSprite);
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

        for (int I = 0, last = CombatMap.Current.UnitSelection.MoveSettingsList.GetCount() - 1; I <= last; I++)
        {
            var moveSettings = CombatMap.Current.UnitSelection.MoveSettingsList.GetMoveSettings(I);
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
        AoWHSSet.Current.SpellControl.GetSpell(SelectedSpell).GetCombatInfo(Unit.Troop, ref info);
        if (info.Range != 0)
        {
            if (info.Range == -1)
            {
                CombatMap.Current.UnitSelection.RangedPathHexSprite.Range = 100;
            }
            else
            {
                CombatMap.Current.UnitSelection.RangedPathHexSprite.Range = (ushort)info.Range;
            }

            CombatMap.Current.UnitSelection.RangedPathHexSprite.Selection = CombatMap.Current.UnitSelection;
            CombatMap.Current.DynamicFixedVisibleHexSprite.AddChild(CombatMap.Current.UnitSelection.RangedPathHexSprite);
        }

        int result = spellId;
        for (int I = 0, last = CombatMap.Current.UnitSelection.MoveSettingsList.GetCount() - 1; I <= last; I++)
        {
            var moveSettings = CombatMap.Current.UnitSelection.MoveSettingsList.GetMoveSettings(I);
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
                AbilityRangedTacticalBattle(0, 0);
                result = -1;
                Unselect();
                break;
        }

        CombatMap.Current.UnitSelection.UpdateMoveCursor();
        CombatMap.Current.UnitChangedEventsStore.Raise(this);
        return result;
    }

    // AoWTC.pas:3895
    public void CreateTacticalBattleAbilityList()
    {
        CombatAbilityRef ability;
        for (int I = 0, last = Abilities.Count - 1; I <= last; I++)
        {
            (Abilities[I] as IDisposable)?.Dispose();
        }

        Abilities.Clear();
        CombatAbilityCount = 0;
        for (int I = 1; I <= 169; I++)
        {
            if (Unit.GetAbilityEnabled(I))
            {
                ability = new CombatAbilityRef();
                ability.Ability = I;
                if (AoWHSSet.Current.AbilityControl.GetAbility(I).GetControlType(Unit.Troop).Contains(AbilityControlTypeElement.CombatAction) && AoWHSSet.Current.AbilityControl.GetAbility(I).GetCombatMode(Unit.Troop) != CombatMode.Melee)
                {
                    Abilities.Insert(CombatAbilityCount, ability);
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
    public void ListAbilities(StringCollection names, StringCollection types, StringCollection sources)
    {
        names.Clear();
        if (types != null)
        {
            types.Clear();
        }

        if (sources != null)
        {
            sources.Clear();
        }

        for (int I = 0, last = Abilities.Count - 1; I <= last; I++)
        {
            var ability = Abilities[I];
            names.AddObject(AoWHSSet.Current.AbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).GetName(Unit.Troop), IntToObject((Abilities[I]).Ability));
            if (types != null)
            {
                var ability2 = Abilities[I];
                var abilityType = AoWHSSet.Current.AbilityControl.GetAbility(((CombatAbilityRef)ability2).Ability).GetAbilityType();
                types.Add(AbilityRules.TranslateResourceString(GameTables.AbilityTypeTexts[(int)abilityType]));
            }

            if (sources != null)
            {
                var ability2 = Abilities[I];
                sources.Add(AoWHSSet.Current.AbilityControl.GetAbility(((CombatAbilityRef)ability2).Ability).GetSourceName(Unit.Troop));
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
                if (Action != SpriteAction.RoundAttack && Action != SpriteAction.RoundAttackHit)
                {
                    ActionFrame = -1;
                }

                break;
            default:
                ActionFrame = -1;
                break;
        }

        Action = action;
    }

    // AoWTC.pas:3935
    public bool AbilityTouchMoveTacticalBattle(MovePath path)
    {
        TokenEvent tokenEvent;
        AbilityCombatInfo buffer = default;
        bool result = false;
        var player = Unit.GetPlayer();
        if (AoWHSMap.Current.TokenManager.PlayerReady(player) && path.Count > 0)
        {
            if (path.Count > 1)
            {
                tokenEvent = CreateTacticalBattleAbilityTouchMoveTokenEvent(Unit, path, SelectedAbility);
                if (tokenEvent != null)
                {
                    var fUnitPlayer = Unit.GetPlayer();
                    AoWHSMap.Current.TokenManager.GetTokenControl(fUnitPlayer).AddEvent(tokenEvent);
                    tokenEvent.Release();
                    result = true;
                }
            }
            else
            {
                AoWHSSet.Current.AbilityControl.GetAbility(SelectedAbility).GetCombatInfo(Unit.Troop, ref buffer);
                if (buffer.TargetTypes.Contains(CombatTargetTypeElement.Friend))
                {
                    var pathLocal = path[0];
                    AbilityRangedTacticalBattle((ushort)path[0].X, (ushort)pathLocal.Y);
                }
            }
        }

        return result;
    }

    // AoWTC.pas:3952
    public bool AbilityRangedTacticalBattle(ushort X, ushort Y)
    {
        CombatAbilityRangedTokenEvent tokenEvent;
        bool result = false;
        var player = Unit.GetPlayer();
        if (AoWHSMap.Current.TokenManager.PlayerReady(player))
        {
            tokenEvent = CreateTacticalBattleAbilityRangedTokenEvent(Unit, X, Y, SelectedAbility, SelectedSpell);
            if (tokenEvent != null)
            {
                var fUnitPlayer = Unit.GetPlayer();
                AoWHSMap.Current.TokenManager.GetTokenControl(fUnitPlayer).AddEvent(tokenEvent);
                tokenEvent.Release();
                result = true;
            }
        }

        return result;
    }

    private static ReadOnlySpan<int> StatusAbilities => [149, 150, 48, 92, 93, 94, 95, 96, 97, 98, 107, 163, 127, 34];

    public static void OriginalUnitAssert(string message, string fileName, int lineNumber)
    {
        RtlUnit.Assert(false, message, fileName, lineNumber);
    }

    private static TacticalBattleMeleeMoveTokenEvent CreateTacticalBattleMeleeMoveTokenEvent(TacticalBattleUnit unitObject, MovePath path)
    {
        TacticalBattleMeleeMoveTokenEvent tacticalBattleMeleeMoveTokenEvent = new TacticalBattleMeleeMoveTokenEvent(null);
        tacticalBattleMeleeMoveTokenEvent?.Setup(unitObject, path);
        return tacticalBattleMeleeMoveTokenEvent;
    }

    private static CombatAbilityTouchMoveTokenEvent CreateTacticalBattleAbilityTouchMoveTokenEvent(TacticalBattleUnit unitObject, MovePath path, int abilityId)
    {
        CombatAbilityTouchMoveTokenEvent tacticalBattleAbilityTouchMoveTokenEvent = new CombatAbilityTouchMoveTokenEvent(null);
        if (tacticalBattleAbilityTouchMoveTokenEvent != null)
        {
            tacticalBattleAbilityTouchMoveTokenEvent.Setup(unitObject, path);
            tacticalBattleAbilityTouchMoveTokenEvent.AbilityId = abilityId;
        }
        return tacticalBattleAbilityTouchMoveTokenEvent;
    }

    private static CombatAbilityRangedTokenEvent CreateTacticalBattleAbilityRangedTokenEvent(TacticalBattleUnit unitObject, ushort X, ushort Y, int abilityId, int spellId)
    {
        CombatAbilityRangedTokenEvent tacticalBattleAbilityRangedTokenEvent = new CombatAbilityRangedTokenEvent(null);
        if (tacticalBattleAbilityRangedTokenEvent != null)
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

    // intf_P2_03.inc:74

    // intf_P2_03.inc:74

}
