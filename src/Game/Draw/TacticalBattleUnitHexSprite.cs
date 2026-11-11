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

// FUnit. FMoveIndicator. FIndicatorStep. FAction. FActionDone. FActionFrame. FOffsetX.
// FOffsetY. FSelectedAbility. FSelectedSpell. FCombatAbilityCount. FAbilities. FBusy
public partial class TacticalBattleUnitHexSprite : UnitHexSprite
{
    public TacticalBattleUnitHexSprite(ECustomNode owner) : base(owner)
    { }

    // AoWTC.pas:3348
    public new const int ClassID = 2228484;

    public TacticalBattleUnit FUnit;
    public short FMoveIndicator;
    public short FIndicatorStep;
    public SpriteAction FAction;
    public bool FActionDone;
    public short FActionFrame;
    public int FOffsetX;
    public int FOffsetY;
    public int FSelectedAbility;
    public int FSelectedSpell;
    public int FCombatAbilityCount;
    public List<CombatAbilityRef> FAbilities;
    public bool FBusy;

    // AoWTC.pas:3566
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID != 536936471 && (Msg.MsgID & 1073741824) == 0 && FUnit != null)
        {
            FUnit.MsgProc(ref Msg, Sender);
        }
    }

    // AoWTC.pas:3461
    public override void MapFieldMsgProc(ref MapFieldMessage Msg)
    {
        SelfDestructCombatAction SelfDestruct;
        DamageCombatAction SpellDamage;
        RangedAttackCombatAction RangedDamage;
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4364)
        {
            if (Msg.HN == 0 && (!CombatMap.Current.FFogEnabled || Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((AoWMapField)Owner).FFieldFlags).Hi).Lo != 0))
            {
                if (FUnit.GetPlayer() != 0)
                {
                    var player = FUnit.GetPlayer();
                    var playerPlayerRace = AoWHSMap.Current.FPlayers.GetPlayers(player);
                    Msg.Param[0] = Msg.Param[0].Int | (playerPlayerRace.FPlayerRace + 1);
                }
                else
                {
                    Msg.Param[0] = Msg.Param[0].Int | 13;
                }
            }
        }
        else
        {
            if (Msg.MsgID == 4352 && Msg.HN == 0)
            {
                var player = FUnit.GetPlayer();
                Msg.Result = player;
            }
            else
            {
                if (Msg.MsgID == 200704 && Msg.HN == 0)
                {
                    RandSeed = AoWHSMap.Current.Random(16777215);
                    SelfDestruct = ((SelfDestructAbility)AoWHSSet.Current.FAbilityControl.GetAbility(65)).CreateSelfDestructCA(Msg.Param[0].As<CombatObject>(), FUnit);
                    if (SelfDestruct.FDamage > 0)
                    {
                        SetAction(SpriteAction.Hit);
                    }

                    CombatMap.Current.FCombat.ExecuteCombatAction(SelfDestruct);
                    SelfDestruct.Release();
                }
                else
                {
                    if (Msg.MsgID == 200707 && Msg.HN == 0)
                    {
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        SpellDamage = (DamageCombatAction)AoWHSSet.Current.FSpellControl.GetSpell(117).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                        if (SpellDamage.FDamage > 0)
                        {
                            SetAction(SpriteAction.Hit);
                        }

                        CombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                        SpellDamage.Release();
                    }
                    else
                    {
                        if (Msg.MsgID == 200715 && Msg.HN == 0)
                        {
                            RandSeed = AoWHSMap.Current.Random(16777215);
                            SpellDamage = (DamageCombatAction)AoWHSSet.Current.FSpellControl.GetSpell(113).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                            if (SpellDamage.FDamage > 0)
                            {
                                SetAction(SpriteAction.Hit);
                            }

                            CombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                            SpellDamage.Release();
                        }
                        else
                        {
                            if (Msg.MsgID == 200714 && Msg.HN == 0)
                            {
                                RandSeed = AoWHSMap.Current.Random(16777215);
                                RangedDamage = ((RangedAttackAbility)AoWHSSet.Current.FAbilityControl.GetAbility(31)).CreateRangedAttackCA(Msg.Param[0].As<CombatObject>(), FUnit, 0);
                                if (RangedDamage.FDamage > 0)
                                {
                                    SetAction(SpriteAction.Hit);
                                }

                                CombatMap.Current.FCombat.ExecuteCombatAction(RangedDamage);
                                RangedDamage.Release();
                            }
                            else
                            {
                                if (Msg.MsgID == 200723 && Msg.HN == 0)
                                {
                                    RandSeed = AoWHSMap.Current.Random(16777215);
                                    SpellDamage = (DamageCombatAction)AoWHSSet.Current.FSpellControl.GetSpell(Msg.Param[1].Int).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                    if (SpellDamage.FDamage > 0)
                                    {
                                        SetAction(SpriteAction.Hit);
                                    }

                                    CombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                                    SpellDamage.Release();
                                }
                                else
                                {
                                    if (Msg.MsgID == 200722 && Msg.HN == 0)
                                    {
                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                        RangedDamage = ((RangedAttackAbility)AoWHSSet.Current.FAbilityControl.GetAbility(Msg.Param[1].Int)).CreateRangedAttackCA(Msg.Param[0].As<CombatObject>(), FUnit, 0);
                                        if (RangedDamage.FDamage > 0)
                                        {
                                            SetAction(SpriteAction.Hit);
                                        }

                                        CombatMap.Current.FCombat.ExecuteCombatAction(RangedDamage);
                                        RangedDamage.Release();
                                    }
                                    else
                                    {
                                        if (Msg.MsgID == 200709 && Msg.HN == 0)
                                        {
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            SpellDamage = (DamageCombatAction)AoWHSSet.Current.FSpellControl.GetSpell(104).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                            if (SpellDamage.FDamage > 0)
                                            {
                                                SetAction(SpriteAction.Hit);
                                            }

                                            CombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                                            SpellDamage.Release();
                                        }
                                        else
                                        {
                                            if (Msg.MsgID == 200716 && Msg.HN == 0)
                                            {
                                                RandSeed = AoWHSMap.Current.Random(16777215);
                                                SpellDamage = (DamageCombatAction)AoWHSSet.Current.FSpellControl.GetSpell(120).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                                if (SpellDamage.FDamage > 0)
                                                {
                                                    SetAction(SpriteAction.Hit);
                                                }

                                                CombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                                                SpellDamage.Release();
                                            }
                                            else
                                            {
                                                if (Msg.MsgID == 200710 && Msg.HN == 0)
                                                {
                                                    RandSeed = AoWHSMap.Current.Random(16777215);
                                                    SpellDamage = (DamageCombatAction)AoWHSSet.Current.FSpellControl.GetSpell(116).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                                    if (SpellDamage.FDamage > 0)
                                                    {
                                                        SetAction(SpriteAction.Hit);
                                                    }

                                                    CombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                                                    SpellDamage.Release();
                                                }
                                                else
                                                {
                                                    if (Msg.MsgID == 200717 && Msg.HN == 0)
                                                    {
                                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                                        SpellDamage = (DamageCombatAction)AoWHSSet.Current.FSpellControl.GetSpell(127).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                                        if (SpellDamage.FDamage > 0)
                                                        {
                                                            SetAction(SpriteAction.Hit);
                                                        }

                                                        CombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                                                        SpellDamage.Release();
                                                    }
                                                    else
                                                    {
                                                        if (Msg.MsgID == 200718 && Msg.HN == 0)
                                                        {
                                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                                            SpellDamage = (DamageCombatAction)AoWHSSet.Current.FSpellControl.GetSpell(128).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                                            if (SpellDamage.FDamage > 0)
                                                            {
                                                                SetAction(SpriteAction.Hit);
                                                            }

                                                            CombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                                                            SpellDamage.Release();
                                                        }
                                                        else
                                                        {
                                                            if (Msg.MsgID == 200719 && Msg.HN == 0)
                                                            {
                                                                SetDirection((byte)(FDirection + 1));
                                                                if (FDirection > 6)
                                                                {
                                                                    SetDirection(1);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (Msg.MsgID == 200720 && Msg.HN == 0)
                                                                {
                                                                    FOffsetX = Random(Msg.Param[0].Int * 2 + 1) - Msg.Param[0].Int;
                                                                    FOffsetY = Random(Msg.Param[0].Int * 2 + 1) - Msg.Param[0].Int;
                                                                }
                                                                else
                                                                {
                                                                    if (Msg.MsgID == 200721 && Msg.HN == 0)
                                                                    {
                                                                        FOffsetX = 0;
                                                                        FOffsetY = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (Msg.MsgID == 200708 && Msg.HN == 0)
                                                                        {
                                                                            if (!FUnit.FUnit.GetMoveTypes().Contains(MoveType.Flying) && ((MapField)Owner).TerrainType != 0 && ((MapField)Owner).TerrainType != 9)
                                                                            {
                                                                                RandSeed = AoWHSMap.Current.Random(16777215);
                                                                                SpellDamage = (DamageCombatAction)AoWHSSet.Current.FSpellControl.GetSpell(112).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                                                                if (SpellDamage.FDamage > 0)
                                                                                {
                                                                                    SetAction(SpriteAction.Hit);
                                                                                }

                                                                                CombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                                                                                SpellDamage.Release();
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
    public override void CanMoveOn(in HexSpriteMoveOnInfo Info, ref HexSpriteMoveOnResult MoveResult)
    {
        int X = default;
        int Y = default;
        MovementHexSprite Movement;
        AbilityCombatInfo AbilityInfo = default;
        AbilityCombatInfo EnemyInfo = default;
        if (Info.HN == 0)
        {
            MoveResult.Code = -1;
            MoveResult.Done = true;
            var yhx = GetYhx();
            var xhx = GetXhx();
            Movement = (MovementHexSprite)CombatMap.Current.MapContainer.MapLevels[0].Field[xhx, yhx].FindOwnedChild(2228490);
            if (Movement != null && Movement.FEnabled)
            {
                if (((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide() == FUnit.GetSide())
                {
                    MoveResult.Code = 0;
                    MoveResult.Done = false;
                    return;
                }
            }

            if (GetXhx() != ((MoveControl)Info.Sender).FDestinationX && ((MoveControl)Info.Sender).FDestinationX != -1)
            {
                return;
            }

            if (GetYhx() != ((MoveControl)Info.Sender).FDestinationY && ((MoveControl)Info.Sender).FDestinationY != -1)
            {
                return;
            }

            if (!((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Move) && !((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.MoveRanged) && CombatMap.Current.FUnitSelection.FMoveSettingsList.GetCount() >= 2)
            {
                return;
            }

            if (((CombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility <= -1 && (CombatObject.MeleeDamageTypes(((CombatMoveControl)Info.Sender).FUnitHS.FUnit) == Set<DamageType>.Of() || FUnit.GetAbilityEnabled(1) && !((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1)) && !((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.MoveRanged) && !((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Move))
            {
                return;
            }

            if (((CombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility == -1)
            {
                if (!((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Move) && !((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.MoveRanged))
                {
                    if (FUnit.GetSide() == ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide())
                    {
                        return;
                    }
                }

                MoveResult.Code = 0;
                return;
            }

            AoWHSSet.Current.FAbilityControl.GetAbility(((CombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility).GetCombatInfo(((CombatMoveControl)Info.Sender).FUnitHS.FUnit.FUnit, ref AbilityInfo);
            if (AbilityInfo.TargetTypes.Contains(CombatTargetTypeElement.Friend) && FUnit.GetSide() == ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide())
            {
                goto Touch;
            }

            AoWHSSet.Current.FAbilityControl.GetAbility(((CombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility).GetCombatInfo(((CombatMoveControl)Info.Sender).FUnitHS.FUnit.FUnit, ref EnemyInfo);
            if (!EnemyInfo.TargetTypes.Contains(CombatTargetTypeElement.Enemy))
            {
                return;
            }

            if (FUnit.GetSide() == ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide())
            {
                return;
            }

        Touch:
            if (((TouchAbility)AoWHSSet.Current.FAbilityControl.GetAbility(((CombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility)).CanTouch(((CombatMoveControl)Info.Sender).FUnitHS.FUnit, FUnit))
            {
                MoveResult.Code = 0;
            }
        }
        else
        {
            var yhx = GetYhx();
            HexGeometry.CenterHNtoHX(GetXhx(), yhx, Info.HN, ref X, ref Y);
            if (MoveResult.Code < 30)
            {
                MoveResult.Code = ((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][Info.MapField.OverlayType - -1];
            }

            if (MoveResult.Code <= -1)
            {
                return;
            }

            if (Info.HN >= 7)
            {
                return;
            }

            if (GetXhx() == ((MoveControl)Info.Sender).FDestinationX && GetYhx() == ((MoveControl)Info.Sender).FDestinationY)
            {
                return;
            }

            if (FUnit.GetSide() == ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide())
            {
                return;
            }

            if (CombatObject.MeleeDamageTypes(FUnit) == Set<DamageType>.Of())
            {
                return;
            }

            if (FUnit.GetLocked())
            {
                return;
            }

            if (FUnit.GetAbilityEnabled(1) != ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1))
            {
                return;
            }

            if (((MoveControl)Info.Sender).FDestinationX >= 0 && ((MoveControl)Info.Sender).FDestinationY >= 0 && HexGeometry.dHXtoRad(X, Y, ((MoveControl)Info.Sender).FDestinationX, ((MoveControl)Info.Sender).FDestinationY) <= 1)
            {
                if (CombatMap.Current.MapContainer.MapLevels[0].Field[((MoveControl)Info.Sender).FDestinationX, ((MoveControl)Info.Sender).FDestinationY].FindOwnedChild(2228484) != null)
                {
                    return;
                }
            }

            if (MoveResult.Code > 96)
            {
                MoveResult.Code = 127;
                return;
            }

            MoveResult.Code += 30;
        }
    }

    // AoWTC.pas:3447
    public override void Activate()
    {
        base.Activate();
        SetUnitGFXResourceIndex(FUnit.FUnit.GetUnitGFXResourceIndex());
        FAbilities = new List<CombatAbilityRef>();
        CreateTCAbList();
        CombatMap.Current.NewFrameEvents += NewFrame;
        if (CombatMap.Current.FExplorationEnabled)
        {
            var player = FUnit.GetPlayer();
            var visibilityRange = FUnit.GetVisibilityRange();
            var yhx = GetYhx();
            var xhx = GetXhx();
            CombatMap.Current.ExploreArea(xhx, yhx, 0, player, visibilityRange);
        }

        FActionFrame = -1;
        FSelectedAbility = -1;
        FSelectedSpell = -1;
    }

    // AoWTC.pas:3339
    public override void Deactivate()
    {
        if (CombatMap.Current != null)
        {
            CombatMap.Current.NewFrameEvents -= NewFrame;
        }

        for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
        {
            (FAbilities[I] as IDisposable)?.Dispose();
        }

        FAbilities?.Clear();
        base.Deactivate();
    }

    // AoWTC.pas:3439
    public override void UpdateMapField(in HexSpriteMapFieldInfo Info)
    {
        if (Info.HN == 0)
        {
            ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 16384 | 8192 | 33554432;
        }
        else
        {
            if (Info.HN <= 6)
            {
                ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 16384;
            }
        }
    }

    // AoWTC.pas:3784
    public override void Select()
    {
        base.Select();
        CreateTCAbList();
        FSelectedAbility = -1;
        if (CombatMap.Current.FUnitSelection.GetTCUnitHS() != this)
        {
            CombatMap.Current.FUnitSelection.SetTCUnitHS(this);
        }

        FUnitGFXResource.FSFXLibraries[0].Play(0);
        if (CombatMap.Current.FSeatedCombatPlayer == FUnit.FCombatPlayer)
        {
            CombatMap.Current.FPlayers.GetPlayers(CombatMap.Current.FSeatedCombatPlayer).FLastUnit = FUnit;
        }
    }

    // AoWTC.pas:3796
    public override void Unselect()
    {
        base.Unselect();
        if (CombatMap.Current != null)
        {
            if (CombatMap.Current.FUnitSelection.GetTCUnitHS() == this)
            {
                CombatMap.Current.FUnitSelection.Unselect();
            }
        }
    }

    // AoWTC.pas:3357
    public override int GetMoveSteps()
    {
        int Result;
        switch (GameTables.AoWEngine.FRegistry.GetMovementSpeed())
        {
            case GameSettings.MovementSpeed.Fast:
                Result = base.GetMoveSteps();
                if (Result > 8)
                {
                    Result = Result - 4;
                }
                else
                {
                    Result = 4;
                }

                break;
            case GameSettings.MovementSpeed.Turbo:
                Result = 2;
                break;
            default:
                Result = base.GetMoveSteps();
                break;
        }

        return Result;
    }

    // AoWTC.pas:3351
    public void Initialize(TacticalBattleUnit U)
    {
        FUnit = U;
        FUnit.FUnitHS = this;
    }

    // AoWTC.pas:3804
    public bool MeleeMoveTC(MovePath Path)
    {
        TacticalBattleMeleeMoveTokenEvent TE;
        bool Result = false;
        var player = FUnit.GetPlayer();
        if (AoWHSMap.Current.FTokenManager.PlayerReady(player) && Path.Count > 1)
        {
            TE = CreateTCMeleeMoveTE(FUnit, Path);
            if (TE != null)
            {
                var fUnitPlayer = FUnit.GetPlayer();
                AoWHSMap.Current.FTokenManager.GetTokenControl(fUnitPlayer).AddEvent(TE);
                TE.Release();
                Result = true;
            }
        }

        return Result;
    }

    // AoWTC.pas:3836
    public int SelectAbility(int AbilityID)
    {
        int Index;
        string Reason = "";
        CombatMap.Current.FUnitSelection.DeactivateRangedPath();
        FSelectedAbility = -1;
        int Result = -1;
        Index = -1;
        if (CombatMap.Current.FCurrentCombatPlayer == CombatMap.Current.FSeatedCombatPlayer)
        {
            for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
            {
                if ((FAbilities[I]).Ability == AbilityID)
                {
                    Index = I;
                }
            }

            if (!FUnit.GetLocked() && FUnit.FMovePoints > 0 && FUnit.FCombatPlayer == CombatMap.Current.FCurrentCombatPlayer && Index < FCombatAbilityCount && Index > -1)
            {
                if (AbilityID == 52)
                {
                    if (FUnit.FUnit.GetMoves() == FUnit.FMovePoints)
                    {
                        AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).ActivateCombat(FUnit.FUnit, ref Reason);
                        FSelectedSpell = -1;
                        FSelectedAbility = AbilityID;
                        Result = Index;
                    }
                }
                else
                {
                    var combatMode = AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetCombatMode(FUnit.FUnit);
                    if (Set<CombatMode>.Of(CombatMode.ShortRange, CombatMode.MediumRange, CombatMode.LongRange).Contains(combatMode))
                    {
                        if (FUnit.FUnit.GetMoves() == FUnit.FMovePoints)
                        {
                            if (AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetCombatMode(FUnit.FUnit) == CombatMode.ShortRange)
                            {
                                CombatMap.Current.FUnitSelection.FRangedPathHS.FRange = 4;
                            }
                            else
                            {
                                if (AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetCombatMode(FUnit.FUnit) == CombatMode.MediumRange)
                                {
                                    CombatMap.Current.FUnitSelection.FRangedPathHS.FRange = 8;
                                }
                                else
                                {
                                    CombatMap.Current.FUnitSelection.FRangedPathHS.FRange = 12;
                                }
                            }

                            CombatMap.Current.FUnitSelection.FRangedPathHS.FSelection = CombatMap.Current.FUnitSelection;
                            CombatMap.Current.DynamicFixedVisibleHS.AddChild(CombatMap.Current.FUnitSelection.FRangedPathHS);
                            FSelectedAbility = AbilityID;
                            Result = Index;
                        }
                    }
                    else
                    {
                        FSelectedAbility = AbilityID;
                        Result = Index;
                    }
                }
            }
        }

        for (int I = 0, last = CombatMap.Current.FUnitSelection.FMoveSettingsList.GetCount() - 1; I <= last; I++)
        {
            var moveSettings = CombatMap.Current.FUnitSelection.FMoveSettingsList.GetMoveSettings(I);
            moveSettings.FPath.Clear();
        }

        CombatMap.Current.FUnitSelection.FRangedPath.Clear();
        CombatMap.Current.FUnitSelection.UpdateMoveCursor();
        CombatMap.Current.FUnitChangedEventsStore.Raise(this);
        return Result;
    }

    // AoWTC.pas:3870
    public int SelectSpell(int SpellID)
    {
        SpellCombatInfo Info = default;
        CombatMap.Current.FUnitSelection.DeactivateRangedPath();
        FSelectedSpell = SpellID;
        AoWHSSet.Current.FSpellControl.GetSpell(FSelectedSpell).GetCombatInfo(FUnit.FUnit, ref Info);
        if (Info.Range != 0)
        {
            if (Info.Range == -1)
            {
                CombatMap.Current.FUnitSelection.FRangedPathHS.FRange = 100;
            }
            else
            {
                CombatMap.Current.FUnitSelection.FRangedPathHS.FRange = (ushort)Info.Range;
            }

            CombatMap.Current.FUnitSelection.FRangedPathHS.FSelection = CombatMap.Current.FUnitSelection;
            CombatMap.Current.DynamicFixedVisibleHS.AddChild(CombatMap.Current.FUnitSelection.FRangedPathHS);
        }

        int Result = SpellID;
        for (int I = 0, last = CombatMap.Current.FUnitSelection.FMoveSettingsList.GetCount() - 1; I <= last; I++)
        {
            var moveSettings = CombatMap.Current.FUnitSelection.FMoveSettingsList.GetMoveSettings(I);
            moveSettings.FPath.Clear();
        }

        CombatMap.Current.FUnitSelection.FRangedPath.Clear();
        switch (TacticalTables.SpellTypes[SpellID])
        {
            case BattleEffectType.ScreenNESW:
            case BattleEffectType.SacredWrath:
            case BattleEffectType.Tremors:
            case BattleEffectType.HighPrayer:
            case BattleEffectType.ShockWave:
                AbRangedTC(0, 0);
                Result = -1;
                Unselect();
                break;
        }

        CombatMap.Current.FUnitSelection.UpdateMoveCursor();
        CombatMap.Current.FUnitChangedEventsStore.Raise(this);
        return Result;
    }

    // AoWTC.pas:3895
    public void CreateTCAbList()
    {
        CombatAbilityRef Ability;
        for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
        {
            (FAbilities[I] as IDisposable)?.Dispose();
        }

        FAbilities.Clear();
        FCombatAbilityCount = 0;
        for (int I = 1; I <= 169; I++)
        {
            if (FUnit.GetAbilityEnabled(I))
            {
                Ability = new CombatAbilityRef();
                Ability.Ability = I;
                if (AoWHSSet.Current.FAbilityControl.GetAbility(I).GetControlType(FUnit.FUnit).Contains(AbilityControlTypeElement.CombatAction) && AoWHSSet.Current.FAbilityControl.GetAbility(I).GetCombatMode(FUnit.FUnit) != CombatMode.Melee)
                {
                    FAbilities.Insert(FCombatAbilityCount, Ability);
                    FCombatAbilityCount += 1;
                }
                else
                {
                    FAbilities.Add(Ability);
                }
            }
        }
    }

    // AoWTC.pas:3910
    public void ListAbilities(StringCollection Names, StringCollection Types, StringCollection Sources)
    {
        Names.Clear();
        if (Types != null)
        {
            Types.Clear();
        }

        if (Sources != null)
        {
            Sources.Clear();
        }

        for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
        {
            var ability = FAbilities[I];
            Names.AddObject(AoWHSSet.Current.FAbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).GetName(FUnit.FUnit), IntToObject((FAbilities[I]).Ability));
            if (Types != null)
            {
                var ability2 = FAbilities[I];
                var abilityType = AoWHSSet.Current.FAbilityControl.GetAbility(((CombatAbilityRef)ability2).Ability).GetAbilityType();
                Types.Add(AbilityRules.TranslateRStr(GameTables.TAbilityTypeRStr[(int)abilityType]));
            }

            if (Sources != null)
            {
                var ability2 = FAbilities[I];
                Sources.Add(AoWHSSet.Current.FAbilityControl.GetAbility(((CombatAbilityRef)ability2).Ability).GetSourceName(FUnit.FUnit));
            }
        }
    }

    // AoWTC.pas:3923
    public void SetAction(SpriteAction Action)
    {
        FActionDone = false;
        switch (Action)
        {
            case SpriteAction.StepUp:
            case SpriteAction.StepBack:
                FActionFrame = 0;
                break;
            case SpriteAction.RoundAttack:
            case SpriteAction.RoundAttackHit:
                if (FAction != SpriteAction.RoundAttack && FAction != SpriteAction.RoundAttackHit)
                {
                    FActionFrame = -1;
                }

                break;
            default:
                FActionFrame = -1;
                break;
        }

        FAction = Action;
    }

    // AoWTC.pas:3935
    public bool AbTouchMoveTC(MovePath Path)
    {
        TokenEvent TE;
        AbilityCombatInfo Buffer = default;
        bool Result = false;
        var player = FUnit.GetPlayer();
        if (AoWHSMap.Current.FTokenManager.PlayerReady(player) && Path.Count > 0)
        {
            if (Path.Count > 1)
            {
                TE = CreateTCAbTouchMoveTE(FUnit, Path, FSelectedAbility);
                if (TE != null)
                {
                    var fUnitPlayer = FUnit.GetPlayer();
                    AoWHSMap.Current.FTokenManager.GetTokenControl(fUnitPlayer).AddEvent(TE);
                    TE.Release();
                    Result = true;
                }
            }
            else
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(FSelectedAbility).GetCombatInfo(FUnit.FUnit, ref Buffer);
                if (Buffer.TargetTypes.Contains(CombatTargetTypeElement.Friend))
                {
                    var path = Path[0];
                    AbRangedTC((ushort)Path[0].X, (ushort)path.Y);
                }
            }
        }

        return Result;
    }

    // AoWTC.pas:3952
    public bool AbRangedTC(ushort X, ushort Y)
    {
        CombatAbilityRangedTokenEvent TE;
        bool Result = false;
        var player = FUnit.GetPlayer();
        if (AoWHSMap.Current.FTokenManager.PlayerReady(player))
        {
            TE = CreateTCAbRangedTE(FUnit, X, Y, FSelectedAbility, FSelectedSpell);
            if (TE != null)
            {
                var fUnitPlayer = FUnit.GetPlayer();
                AoWHSMap.Current.FTokenManager.GetTokenControl(fUnitPlayer).AddEvent(TE);
                TE.Release();
                Result = true;
            }
        }

        return Result;
    }

    private static Int32Array14 StatusAbils = Int32Array14.From([149, 150, 48, 92, 93, 94, 95, 96, 97, 98, 107, 163, 127, 34]);

    public static void OriginalUnitAssert(string Message, string Filename, int LineNumber)
    {
        RtlUnit.Assert(false, Message, Filename, LineNumber);
    }

    private static TacticalBattleMeleeMoveTokenEvent CreateTCMeleeMoveTE(TacticalBattleUnit UnitObject, MovePath Path)
    {
        TacticalBattleMeleeMoveTokenEvent tCMeleeMoveTE = new TacticalBattleMeleeMoveTokenEvent(null);
        tCMeleeMoveTE?.Setup(UnitObject, Path);
        return tCMeleeMoveTE;
    }

    private static CombatAbilityTouchMoveTokenEvent CreateTCAbTouchMoveTE(TacticalBattleUnit UnitObject, MovePath Path, int AbilityID)
    {
        CombatAbilityTouchMoveTokenEvent tCAbTouchMoveTE = new CombatAbilityTouchMoveTokenEvent(null);
        if (tCAbTouchMoveTE != null)
        {
            tCAbTouchMoveTE.Setup(UnitObject, Path);
            tCAbTouchMoveTE.FAbilityID = AbilityID;
        }
        return tCAbTouchMoveTE;
    }

    private static CombatAbilityRangedTokenEvent CreateTCAbRangedTE(TacticalBattleUnit UnitObject, ushort X, ushort Y, int AbilityID, int SpellID)
    {
        CombatAbilityRangedTokenEvent tCAbRangedTE = new CombatAbilityRangedTokenEvent(null);
        if (tCAbRangedTE != null)
        {
            tCAbRangedTE.FSourceX = (ushort)UnitObject.FUnitHS.Xhx;
            tCAbRangedTE.FSourceY = (ushort)UnitObject.FUnitHS.Yhx;
            tCAbRangedTE.FTargetX = X;
            tCAbRangedTE.FTargetY = Y;
            tCAbRangedTE.FAbilityID = AbilityID;
            tCAbRangedTE.FSpellID = SpellID;
        }
        return tCAbRangedTE;
    }

    public enum SpriteAction : byte
    {
        Move = 0, MeleeHit = 1, MeleeMiss = 2, Ability = 3, Hit = 4, Killed = 5, StepUp = 6, StepBack = 7, Busy = 8, RoundAttack = 9, RoundAttackHit = 10, FreeStruck = 11, BreathAttack = 12
    }

    // intf_P2_03.inc:74

    // intf_P2_03.inc:74

}
