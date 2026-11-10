// Translated from CombatTE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

public partial class CAbTouchMoveTE : CombatMoveTE
{
    public CAbTouchMoveTE(ECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:624
    public new const int ClassID = 2236418;

    public TfxHS FEffect;
    public int FAbilityID;
    public PossessCA FPossessAction;
    public EntangleCA FEntangleAction;
    public WebCA FWebAction;
    public TurnUndeadCA FTurnUndeadAction;
    public HealingCA FHealingAction;
    public CommandCA FCommandAction;
    public InvokeDeathCA FDeathAction;
    public DispelMagicAbilityCA FDispelAction;
    public TfxType FEffectType;
    public bool FSuccessful;
    public int FDamage;
    public CityWall FWall;

    // CombatTE.pas:627
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FAbilityID, 30);
    }

    // CombatTE.pas:633
    public override MoveStatus LastMove()
    {
        XYZ Point;
        int I = default;
        int X = default;
        int Y = default;
        TacticalCombatUnitHS HS;
        MapFieldMsg Msg = default;
        MoveStatus Result = MoveStatus.Moving;
        try
        {
            switch (FStep)
            {
                case TokenStep.Start:
                    try
                    {
                        FEffectType = TacticalTables.AbilTypes[FAbilityID];
                        FUnit.FUnitHS.CancelMove();
                        FUnit.FUnitHS.Unselect();
                        FUnit.FUnitHS.FSelectedAbility = FAbilityID;
                        bool notExplorationEnabled = !AoWCombatMap.Current.FExplorationEnabled;
                        if (!notExplorationEnabled)
                        {
                            var yhx = FUnit.FUnitHS.Yhx;
                            var xhx = FUnit.FUnitHS.Xhx;
                            notExplorationEnabled = ((AoWMapField)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, 0)).GetExplored(AoWCombatMap.Current.FSeatedPlayer);
                        }

                        if (notExplorationEnabled)
                        {
                            var yhx = FUnit.FUnitHS.Yhx;
                            var xhx = FUnit.FUnitHS.Xhx;
                            AoWCombatMap.Current.MakeVisible(xhx, yhx, 0, 5);
                        }

                        var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                        var pathY = FPath[0];
                        var fUnitHSXhx = FUnit.FUnitHS.Xhx;
                        var pathX = FPath[0];
                        var meleeDirIndex = TacticalTables.GetMeleeDirIndex(FUnit.FUnitHS.Xhx, fUnitHSXhx - pathX.X, fUnitHSYhx - pathY.Y);
                        FUnit.FUnitHS.SetDirection((byte)meleeDirIndex);
                        if (FAbilityID == 65)
                        {
                            FStep = TokenStep.InitStrike;
                        }
                        else
                        {
                            if (FAbilityID == 114)
                            {
                                FStep = TokenStep.ExeStrike;
                                FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.RoundAttack);
                            }
                            else
                            {
                                FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.StepUp);
                                var path = FPath[0];
                                var path2 = FPath[0];
                                HS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(path2.X, path.Y).FindOwnedChild(2228484);
                                if (HS == null)
                                {
                                    FPreviousUnit = null;
                                }
                                else
                                {
                                    FPreviousUnit = HS.FUnit;
                                }

                                if (FPreviousUnit != null && FAbilityID != 117)
                                {
                                    FPreviousUnit.FUnitHS.SetDirection((byte)(FUnit.FUnitHS.FDirection + 3));
                                    if (FPreviousUnit.FUnitHS.FDirection > 6)
                                    {
                                        FPreviousUnit.FUnitHS.SetDirection((byte)(FPreviousUnit.FUnitHS.FDirection - 6));
                                    }
                                }
                                else
                                {
                                    if (FAbilityID == 117)
                                    {
                                        var path3 = FPath[0];
                                        var path4 = FPath[0];
                                        FWall = (CityWall)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(path4.X, path3.Y).FindOwnedChild(2229508);
                                        if (FWall == null)
                                        {
                                            var path5 = FPath[0];
                                            var path6 = FPath[0];
                                            FWall = AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(path6.X, path5.Y).FindOwnedChild(2229510) as CityWall;
                                        }

                                        if (FWall == null && FAbilityID == 117)
                                        {
                                            Result = MoveStatus.Done;
                                        }
                                    }
                                    else
                                    {
                                        Result = MoveStatus.Done;
                                    }
                                }

                                FStep = TokenStep.StepUp;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token Start", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                    }

                    break;
                case TokenStep.StepUp:
                    try
                    {
                        if (FUnit.FUnitHS.FActionDone)
                        {
                            FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.Ability);
                            FStep = TokenStep.InitStrike;
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token StepUp", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                    }

                    break;
                case TokenStep.InitStrike:
                    try
                    {
                        if (FPreviousUnit != null)
                        {
                            if (FUnit.FUnitHS.FActionDone)
                            {
                                FEffect = AoWCombatMap.Current.AddEffect(FEffectType);
                                FEffect.FAbilityIndex = FAbilityID;
                                Point = new XYZ();
                                var yhx = FPreviousUnit.FUnitHS.Yhx;
                                HexGeometry.HXtoHP(FPreviousUnit.FUnitHS.Xhx, yhx, ref Point.X, ref Point.Y);
                                Point.X += 16;
                                FEffect.FPath.Add(Point);
                                FStep = TokenStep.ExeStrike;
                            }
                        }
                        else
                        {
                            if (FWall != null || FAbilityID == 65)
                            {
                                switch (FAbilityID)
                                {
                                    case 117:
                                        FStep = TokenStep.End;
                                        FDamage = 0;
                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                        var defense = FWall.GetDefense();
                                        var executeDamageRole = AbilityRules.ExecuteDamageRole(TacticalTables.TCDamage[0], TacticalTables.TCAttack[0] - defense);
                                        FWall.SetHitPoints(FWall.FHitPoints - executeDamageRole);
                                        FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.StepBack);
                                        break;
                                    case 65:
                                        FEffect = AoWCombatMap.Current.AddEffect(FEffectType);
                                        FEffect.FAbilityIndex = FAbilityID;
                                        Point = new XYZ();
                                        var path = FPath[1];
                                        HexGeometry.HXtoHP(FPath[1].X, path.Y, ref Point.X, ref Point.Y);
                                        Point.X += 16;
                                        FEffect.FPath.Add(Point);
                                        FStep = TokenStep.ExeStrike;
                                        break;
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token InitStrike", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                    }

                    break;
                case TokenStep.ExeStrike:
                    try
                    {
                        if (FEffect == null)
                        {
                            if (!FUnit.GetEnabled())
                            {
                                Result = MoveStatus.Done;
                            }
                            else
                            {
                                switch (FAbilityID)
                                {
                                    case 114:
                                        if (FUnit.FUnitHS.FActionDone || FUnit.FUnitHS.FActionFrame >= 17)
                                        {
                                            Result = MoveStatus.Done;
                                        }
                                        else
                                        {
                                            if (TacticalTables.raDir[FUnit.FUnitHS.FActionFrame] > 0)
                                            {
                                                I = (TacticalTables.raDir[FUnit.FUnitHS.FActionFrame] + FUnit.FUnitHS.FDirection) % 6 + 1;
                                                var path = FPath[1];
                                                HexGeometry.CenterHNtoHX(FPath[1].X, path.Y, I, ref X, ref Y);
                                                HS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).FindOwnedChild(2228484);
                                                if (HS == null || HS == FUnit.FUnitHS)
                                                {
                                                    FPreviousUnit = null;
                                                }
                                                else
                                                {
                                                    if (HS.FUnit.GetAbilityEnabled(1))
                                                    {
                                                        FPreviousUnit = null;
                                                    }
                                                    else
                                                    {
                                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                                        FStrike = CombatObject.CreateStrikeCA(FUnit, HS.FUnit, true);
                                                        if (FStrike.FDamage > 0)
                                                        {
                                                            HS.SetAction(TacticalCombatUnitHS.SpriteAction.Hit);
                                                            FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.RoundAttackHit);
                                                            TacticalCombatUnit.MakeHitBlood(HS.FUnit, FStrike.FDamage);
                                                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(FStrike);
                                                        }
                                                        else
                                                        {
                                                            FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.RoundAttack);
                                                        }

                                                        FStrike.Release();
                                                        FStrike = null;
                                                    }
                                                }
                                            }
                                        }

                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (FEffect.FDone)
                            {
                                if (!FUnit.GetEnabled())
                                {
                                    Result = MoveStatus.Done;
                                }
                                else
                                {
                                    switch (FAbilityID)
                                    {
                                        case 40:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            FEntangleAction = ((EntangleAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID)).CreateEntangleCA(FUnit, FPreviousUnit);
                                            FSuccessful = FEntangleAction.FSuccessful;
                                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(FEntangleAction);
                                            FEntangleAction.Release();
                                            break;
                                        case 27:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            FWebAction = ((WebAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID)).CreateWebCA(FUnit, FPreviousUnit);
                                            FSuccessful = FWebAction.FSuccessful;
                                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(FWebAction);
                                            FWebAction.Release();
                                            break;
                                        case 28:
                                        case 29:
                                        case 148:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            FCommandAction = ((CommandAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID)).CreateCommandCA(FUnit, FPreviousUnit);
                                            FSuccessful = FCommandAction.FSuccessful;
                                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(FCommandAction);
                                            FCommandAction.Release();
                                            break;
                                        case 38:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            FTurnUndeadAction = ((TurnUndeadAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID)).CreateTurnUndeadCA(FUnit, FPreviousUnit);
                                            FSuccessful = FTurnUndeadAction.GetSuccessfull();
                                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(FTurnUndeadAction);
                                            FTurnUndeadAction.Release();
                                            break;
                                        case 35:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            FDeathAction = ((InvokeDeathAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID)).CreateCA(FUnit, FPreviousUnit);
                                            FSuccessful = FDeathAction.FSuccessful;
                                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(FDeathAction);
                                            FDeathAction.Release();
                                            break;
                                        case 47:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            FHealingAction = ((HealingAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID)).CreateHealingCA(FUnit, FPreviousUnit);
                                            FSuccessful = true;
                                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(FHealingAction);
                                            FHealingAction.Release();
                                            break;
                                        case 60:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            FDispelAction = ((DispelMagicAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID)).CreateCA(FUnit, FPreviousUnit);
                                            FSuccessful = FDispelAction.FEnchantmentIDs.Count > 0;
                                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(FDispelAction);
                                            FDispelAction.Release();
                                            break;
                                        case 106:
                                        case 107:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            FPossessAction = ((PossessAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID)).CreatePossessCA(FUnit, FPreviousUnit);
                                            FSuccessful = FPossessAction.FSuccessful;
                                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(FPossessAction);
                                            FPossessAction.Release();
                                            var unitGFXResourceIndex = FUnit.FUnit.GetUnitGFXResourceIndex();
                                            FUnit.FUnitHS.SetUnitGFXResourceIndex(unitGFXResourceIndex);
                                            if (!FPreviousUnit.GetEnabled())
                                            {
                                                FUnit.FUnitHS.MoveTo((MapField)FPreviousUnit.FUnitHS.Owner, 0);
                                                FUnit.FUnitHS.SetDirection(FPreviousUnit.FUnitHS.FDirection);
                                                FUnit.FUnitHS.FOffsetX = 0;
                                                FUnit.FUnitHS.FOffsetY = 0;
                                                FPreviousUnit.FUnitHS?.Dispose();
                                                FPreviousUnit.FUnitHS = null;
                                                Result = MoveStatus.Done;
                                            }

                                            break;
                                    }
                                }

                                if (!FUnit.GetEnabled() || ((FAbilityID == 106 || FAbilityID == 107) && FSuccessful))
                                {
                                    Result = MoveStatus.Done;
                                }
                                else
                                {
                                    if (!FSuccessful && FPreviousUnit != null && FPreviousUnit.GetEnabled() && !FPreviousUnit.GetLocked() && CombatObject.MeleeDamageTypes(FPreviousUnit) != Set<DamageType>.Of() && FPreviousUnit.GetSide() != FUnit.GetSide())
                                    {
                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                        FStrike = CombatObject.CreateStrikeCA(FPreviousUnit, FUnit, true);
                                        if (FStrike.FDamage == 0)
                                        {
                                            FPreviousUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.MeleeMiss);
                                        }
                                        else
                                        {
                                            FPreviousUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.MeleeHit);
                                        }

                                        FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.Busy);
                                        FStep = TokenStep.EndStrike;
                                    }
                                    else
                                    {
                                        FStep = TokenStep.End;
                                        FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.StepBack);
                                    }
                                }
                            }
                            else
                            {
                                if (FUnit.GetEnabled())
                                {
                                    switch (FAbilityID)
                                    {
                                        case 65:
                                            if (FEffect.FFrame == 5)
                                            {
                                                Msg.MsgID = 200704;
                                                Msg.Param[0] = FUnit;
                                                Msg.Result = -1;
                                                I = 0;
                                                for (; I <= 6; I += 1)
                                                {
                                                    var path = FPath[1];
                                                    HexGeometry.CenterHNtoHX(FPath[1].X, path.Y, I, ref X, ref Y);
                                                    AoWCombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                    EffectPaths.CreateFire(false, 50, X, Y);
                                                }

                                                FUnit.FDeathMethod = TacticalCombatUnit.DeathMethod.Explode;
                                            }

                                            break;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token ExeStrike", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                    }

                    break;
                case TokenStep.EndStrike:
                    try
                    {
                        if (FPreviousUnit.FUnitHS.FActionDone)
                        {
                            if (FStrike.FDamage > 0)
                            {
                                FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.Hit);
                                TacticalCombatUnit.MakeHitBlood(FUnit, FStrike.FDamage);
                            }

                            var yhx = FUnit.FUnitHS.Yhx;
                            HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx, ref I, ref X);
                            if (FStrike.FDamageTypes.Value - FStrike.FEffects.Value != Set<DamageType>.Of())
                            {
                                CombatEvents.MakeDEHSList(I + FUnit.FUnitHS.FOffsetX, X + FUnit.FUnitHS.FOffsetY, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                            }

                            if (FStrike.FEffects.Value != Set<DamageType>.Of())
                            {
                                CombatEvents.MakeDEHSList(I + FUnit.FUnitHS.FOffsetX, X + FUnit.FUnitHS.FOffsetY, FStrike.FEffects.Value, true);
                            }

                            FStep = TokenStep.ExeEndStrike;
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token StrikeBack", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                    }

                    break;
                case TokenStep.ExeEndStrike:
                    try
                    {
                        if (FUnit.FUnitHS.FActionDone || FStrike.FDamage == 0)
                        {
                            if (FStrike.FDamage > 0)
                            {
                                AoWCombatMap.Current.FCombat.ExecuteCombatAction(FStrike);
                            }

                            if (FUnit.GetEnabled())
                            {
                                FStep = TokenStep.End;
                                FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.StepBack);
                            }
                            else
                            {
                                Result = MoveStatus.Done;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token ExeStrikeBack", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                    }

                    break;
                case TokenStep.End:
                    if (FUnit.FUnitHS.FActionDone)
                    {
                        Result = MoveStatus.Done;
                    }

                    break;
            }

            if (Result == MoveStatus.Done)
            {
                if (FUnit.GetEnabled())
                {
                    FUnit.FUnitHS.SetAction(TacticalCombatUnitHS.SpriteAction.Move);
                    FUnit.FUnitHS.FSelectedAbility = -1;
                    FUnit.FMovePoints = 0;
                    FUnit.FUnitHS.Unselect();
                    if (FUnit.FCombatPlayer == AoWCombatMap.Current.FCurrentCombatPlayer)
                    {
                        var xhx = FUnit.FUnitHS.Xhx;
                        AoWCombatMap.Current.FPlayers.GetPlayers(AoWCombatMap.Current.FCurrentCombatPlayer).FLastX = xhx;
                        var yhx = FUnit.FUnitHS.Yhx;
                        AoWCombatMap.Current.FPlayers.GetPlayers(AoWCombatMap.Current.FCurrentCombatPlayer).FLastY = yhx;
                    }
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Token", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
        }

        return Result;
    }

    // intf_P5_07.inc:765

}
