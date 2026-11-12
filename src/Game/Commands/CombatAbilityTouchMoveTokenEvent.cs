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

public partial class CombatAbilityTouchMoveTokenEvent : CombatMoveTokenEvent
{
    public CombatAbilityTouchMoveTokenEvent(ECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:624
    public new const int ClassId = 2236418;

    public BattleEffectSprite Effect;
    public int AbilityId;
    public PossessCombatAction PossessAction;
    public EntangleCombatAction EntangleAction;
    public WebCombatAction WebAction;
    public TurnUndeadCombatAction TurnUndeadAction;
    public HealingCombatAction HealingAction;
    public CommandCombatAction CommandAction;
    public InvokeDeathCombatAction DeathAction;
    public DispelMagicAbilityCombatAction DispelAction;
    public BattleEffectType EffectType;
    public bool Successful;
    public int Damage;
    public CityWall Wall;

    // CombatTE.pas:627
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref AbilityId, 30);
    }

    // CombatTE.pas:633
    public override MoveStatus LastMove()
    {
        Point3D point;
        int I = default;
        int X = default;
        int Y = default;
        TacticalBattleUnitHexSprite hexSprite;
        MapFieldMessage message = default;
        MoveStatus result = MoveStatus.Moving;
        try
        {
            switch (Step)
            {
                case TokenStep.Start:
                    try
                    {
                        EffectType = TacticalTables.s_abilityTypes[AbilityId];
                        Unit.UnitHexSprite.CancelMove();
                        Unit.UnitHexSprite.Unselect();
                        Unit.UnitHexSprite.SelectedAbility = AbilityId;
                        bool notExplorationEnabled = !CombatMap.Current.ExplorationEnabled;
                        if (!notExplorationEnabled)
                        {
                            var hexY = Unit.UnitHexSprite.HexY;
                            var hexX = Unit.UnitHexSprite.HexX;
                            notExplorationEnabled = ((AoWMapField)CombatMap.Current.MapContainer.GetField(hexX, hexY, 0)).GetExplored(CombatMap.Current.SeatedPlayer);
                        }

                        if (notExplorationEnabled)
                        {
                            var hexY = Unit.UnitHexSprite.HexY;
                            var hexX = Unit.UnitHexSprite.HexX;
                            CombatMap.Current.MakeVisible(hexX, hexY, 0, 5);
                        }

                        var fUnitHexSpriteHexY = Unit.UnitHexSprite.HexY;
                        var pathY = Path[0];
                        var fUnitHexSpriteHexX = Unit.UnitHexSprite.HexX;
                        var pathX = Path[0];
                        var meleeDirIndex = TacticalTables.GetMeleeDirectionIndex(Unit.UnitHexSprite.HexX, fUnitHexSpriteHexX - pathX.X, fUnitHexSpriteHexY - pathY.Y);
                        Unit.UnitHexSprite.SetDirection((byte)meleeDirIndex);
                        if (AbilityId == 65)
                        {
                            Step = TokenStep.InitializeStrike;
                        }
                        else
                        {
                            if (AbilityId == 114)
                            {
                                Step = TokenStep.ExecuteStrike;
                                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.RoundAttack);
                            }
                            else
                            {
                                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepUp);
                                var path = Path[0];
                                var path2 = Path[0];
                                hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(path2.X, path.Y).FindOwnedChild(2228484);
                                if (hexSprite == null)
                                {
                                    PreviousUnit = null;
                                }
                                else
                                {
                                    PreviousUnit = hexSprite.Unit;
                                }

                                if (PreviousUnit != null && AbilityId != 117)
                                {
                                    PreviousUnit.UnitHexSprite.SetDirection((byte)(Unit.UnitHexSprite.Direction + 3));
                                    if (PreviousUnit.UnitHexSprite.Direction > 6)
                                    {
                                        PreviousUnit.UnitHexSprite.SetDirection((byte)(PreviousUnit.UnitHexSprite.Direction - 6));
                                    }
                                }
                                else
                                {
                                    if (AbilityId == 117)
                                    {
                                        var path3 = Path[0];
                                        var path4 = Path[0];
                                        Wall = (CityWall)CombatMap.Current.MapContainer.MapLevels[0].GetField(path4.X, path3.Y).FindOwnedChild(2229508);
                                        if (Wall == null)
                                        {
                                            var path5 = Path[0];
                                            var path6 = Path[0];
                                            Wall = CombatMap.Current.MapContainer.MapLevels[0].GetField(path6.X, path5.Y).FindOwnedChild(2229510) as CityWall;
                                        }

                                        if (Wall == null && AbilityId == 117)
                                        {
                                            result = MoveStatus.Done;
                                        }
                                    }
                                    else
                                    {
                                        result = MoveStatus.Done;
                                    }
                                }

                                Step = TokenStep.StepUp;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token Start", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
                    }

                    break;
                case TokenStep.StepUp:
                    try
                    {
                        if (Unit.UnitHexSprite.ActionDone)
                        {
                            Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Ability);
                            Step = TokenStep.InitializeStrike;
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token StepUp", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
                    }

                    break;
                case TokenStep.InitializeStrike:
                    try
                    {
                        if (PreviousUnit != null)
                        {
                            if (Unit.UnitHexSprite.ActionDone)
                            {
                                Effect = CombatMap.Current.AddEffect(EffectType);
                                Effect.AbilityIndex = AbilityId;
                                point = new Point3D();
                                var hexY = PreviousUnit.UnitHexSprite.HexY;
                                HexGeometry.HexToPixel(PreviousUnit.UnitHexSprite.HexX, hexY, ref point.X, ref point.Y);
                                point.X += 16;
                                Effect.Path.Add(point);
                                Step = TokenStep.ExecuteStrike;
                            }
                        }
                        else
                        {
                            if (Wall != null || AbilityId == 65)
                            {
                                switch (AbilityId)
                                {
                                    case 117:
                                        Step = TokenStep.End;
                                        Damage = 0;
                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                        var defense = Wall.GetDefense();
                                        var executeDamageRole = AbilityRules.ExecuteDamageRole(TacticalTables.s_tacticalBattleDamage[0], TacticalTables.s_tacticalBattleAttack[0] - defense);
                                        Wall.SetHitPoints(Wall.HitPoints - executeDamageRole);
                                        Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepBack);
                                        break;
                                    case 65:
                                        Effect = CombatMap.Current.AddEffect(EffectType);
                                        Effect.AbilityIndex = AbilityId;
                                        point = new Point3D();
                                        var path = Path[1];
                                        HexGeometry.HexToPixel(Path[1].X, path.Y, ref point.X, ref point.Y);
                                        point.X += 16;
                                        Effect.Path.Add(point);
                                        Step = TokenStep.ExecuteStrike;
                                        break;
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token InitStrike", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
                    }

                    break;
                case TokenStep.ExecuteStrike:
                    try
                    {
                        if (Effect == null)
                        {
                            if (!Unit.GetEnabled())
                            {
                                result = MoveStatus.Done;
                            }
                            else
                            {
                                switch (AbilityId)
                                {
                                    case 114:
                                        if (Unit.UnitHexSprite.ActionDone || Unit.UnitHexSprite.ActionFrame >= 17)
                                        {
                                            result = MoveStatus.Done;
                                        }
                                        else
                                        {
                                            if (TacticalTables.s_rangedAttackDirection[Unit.UnitHexSprite.ActionFrame] > 0)
                                            {
                                                I = (TacticalTables.s_rangedAttackDirection[Unit.UnitHexSprite.ActionFrame] + Unit.UnitHexSprite.Direction) % 6 + 1;
                                                var path = Path[1];
                                                HexGeometry.CenterHexNumberToHex(Path[1].X, path.Y, I, ref X, ref Y);
                                                hexSprite = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).FindOwnedChild(2228484);
                                                if (hexSprite == null || hexSprite == Unit.UnitHexSprite)
                                                {
                                                    PreviousUnit = null;
                                                }
                                                else
                                                {
                                                    if (hexSprite.Unit.GetAbilityEnabled(1))
                                                    {
                                                        PreviousUnit = null;
                                                    }
                                                    else
                                                    {
                                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                                        Strike = CombatObject.CreateStrikeCombatAction(Unit, hexSprite.Unit, true);
                                                        if (Strike.Damage > 0)
                                                        {
                                                            hexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                                            Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.RoundAttackHit);
                                                            TacticalBattleUnit.MakeHitBlood(hexSprite.Unit, Strike.Damage);
                                                            CombatMap.Current.Combat.ExecuteCombatAction(Strike);
                                                        }
                                                        else
                                                        {
                                                            Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.RoundAttack);
                                                        }

                                                        Strike.Release();
                                                        Strike = null;
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
                            if (Effect.Done)
                            {
                                if (!Unit.GetEnabled())
                                {
                                    result = MoveStatus.Done;
                                }
                                else
                                {
                                    switch (AbilityId)
                                    {
                                        case 40:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            EntangleAction = ((EntangleAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityId)).CreateEntangleCombatAction(Unit, PreviousUnit);
                                            Successful = EntangleAction.Successful;
                                            CombatMap.Current.Combat.ExecuteCombatAction(EntangleAction);
                                            EntangleAction.Release();
                                            break;
                                        case 27:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            WebAction = ((WebAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityId)).CreateWebCombatAction(Unit, PreviousUnit);
                                            Successful = WebAction.Successful;
                                            CombatMap.Current.Combat.ExecuteCombatAction(WebAction);
                                            WebAction.Release();
                                            break;
                                        case 28:
                                        case 29:
                                        case 148:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            CommandAction = ((CommandAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityId)).CreateCommandCombatAction(Unit, PreviousUnit);
                                            Successful = CommandAction.Successful;
                                            CombatMap.Current.Combat.ExecuteCombatAction(CommandAction);
                                            CommandAction.Release();
                                            break;
                                        case 38:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            TurnUndeadAction = ((TurnUndeadAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityId)).CreateTurnUndeadCombatAction(Unit, PreviousUnit);
                                            Successful = TurnUndeadAction.GetSuccessful();
                                            CombatMap.Current.Combat.ExecuteCombatAction(TurnUndeadAction);
                                            TurnUndeadAction.Release();
                                            break;
                                        case 35:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            DeathAction = ((InvokeDeathAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityId)).CreateCombatAction(Unit, PreviousUnit);
                                            Successful = DeathAction.Successful;
                                            CombatMap.Current.Combat.ExecuteCombatAction(DeathAction);
                                            DeathAction.Release();
                                            break;
                                        case 47:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            HealingAction = ((HealingAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityId)).CreateHealingCombatAction(Unit, PreviousUnit);
                                            Successful = true;
                                            CombatMap.Current.Combat.ExecuteCombatAction(HealingAction);
                                            HealingAction.Release();
                                            break;
                                        case 60:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            DispelAction = ((DispelMagicAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityId)).CreateCombatAction(Unit, PreviousUnit);
                                            Successful = DispelAction.EnchantmentIds.Count > 0;
                                            CombatMap.Current.Combat.ExecuteCombatAction(DispelAction);
                                            DispelAction.Release();
                                            break;
                                        case 106:
                                        case 107:
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            PossessAction = ((PossessAbility)AoWHSSet.Current.AbilityControl.GetAbility(AbilityId)).CreatePossessCombatAction(Unit, PreviousUnit);
                                            Successful = PossessAction.Successful;
                                            CombatMap.Current.Combat.ExecuteCombatAction(PossessAction);
                                            PossessAction.Release();
                                            var unitGraphicsResourceIndex = Unit.Troop.GetUnitGraphicsResourceIndex();
                                            Unit.UnitHexSprite.SetUnitGraphicsResourceIndex(unitGraphicsResourceIndex);
                                            if (!PreviousUnit.GetEnabled())
                                            {
                                                Unit.UnitHexSprite.MoveTo((MapField)PreviousUnit.UnitHexSprite.Owner, 0);
                                                Unit.UnitHexSprite.SetDirection(PreviousUnit.UnitHexSprite.Direction);
                                                Unit.UnitHexSprite.OffsetX = 0;
                                                Unit.UnitHexSprite.OffsetY = 0;
                                                PreviousUnit.UnitHexSprite?.Dispose();
                                                PreviousUnit.UnitHexSprite = null;
                                                result = MoveStatus.Done;
                                            }

                                            break;
                                    }
                                }

                                if (!Unit.GetEnabled() || ((AbilityId == 106 || AbilityId == 107) && Successful))
                                {
                                    result = MoveStatus.Done;
                                }
                                else
                                {
                                    if (!Successful && PreviousUnit != null && PreviousUnit.GetEnabled() && !PreviousUnit.GetLocked() && CombatObject.MeleeDamageTypes(PreviousUnit) != Set<DamageType>.Of() && PreviousUnit.GetSide() != Unit.GetSide())
                                    {
                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                        Strike = CombatObject.CreateStrikeCombatAction(PreviousUnit, Unit, true);
                                        if (Strike.Damage == 0)
                                        {
                                            PreviousUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeMiss);
                                        }
                                        else
                                        {
                                            PreviousUnit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeHit);
                                        }

                                        Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Busy);
                                        Step = TokenStep.EndStrike;
                                    }
                                    else
                                    {
                                        Step = TokenStep.End;
                                        Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepBack);
                                    }
                                }
                            }
                            else
                            {
                                if (Unit.GetEnabled())
                                {
                                    switch (AbilityId)
                                    {
                                        case 65:
                                            if (Effect.Frame == 5)
                                            {
                                                message.MessageId = 200704;
                                                message.Param[0] = Unit;
                                                message.Result = -1;
                                                I = 0;
                                                for (; I <= 6; I += 1)
                                                {
                                                    var path = Path[1];
                                                    HexGeometry.CenterHexNumberToHex(Path[1].X, path.Y, I, ref X, ref Y);
                                                    CombatMap.Current.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMessage(ref message);
                                                    EffectPaths.CreateFire(false, 50, X, Y);
                                                }

                                                Unit.DeathMethod = TacticalBattleUnitDeathMethod.Explode;
                                            }

                                            break;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token ExeStrike", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
                    }

                    break;
                case TokenStep.EndStrike:
                    try
                    {
                        if (PreviousUnit.UnitHexSprite.ActionDone)
                        {
                            if (Strike.Damage > 0)
                            {
                                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                TacticalBattleUnit.MakeHitBlood(Unit, Strike.Damage);
                            }

                            var hexY = Unit.UnitHexSprite.HexY;
                            HexGeometry.HexToPixel(Unit.UnitHexSprite.HexX, hexY, ref I, ref X);
                            if (Strike.DamageTypes.Value - Strike.Effects.Value != Set<DamageType>.Of())
                            {
                                CombatEvents.MakeDamageEffectHexSpriteList(I + Unit.UnitHexSprite.OffsetX, X + Unit.UnitHexSprite.OffsetY, Strike.DamageTypes.Value - Strike.Effects.Value, false);
                            }

                            if (Strike.Effects.Value != Set<DamageType>.Of())
                            {
                                CombatEvents.MakeDamageEffectHexSpriteList(I + Unit.UnitHexSprite.OffsetX, X + Unit.UnitHexSprite.OffsetY, Strike.Effects.Value, true);
                            }

                            Step = TokenStep.ExecuteEndStrike;
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token StrikeBack", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
                    }

                    break;
                case TokenStep.ExecuteEndStrike:
                    try
                    {
                        if (Unit.UnitHexSprite.ActionDone || Strike.Damage == 0)
                        {
                            if (Strike.Damage > 0)
                            {
                                CombatMap.Current.Combat.ExecuteCombatAction(Strike);
                            }

                            if (Unit.GetEnabled())
                            {
                                Step = TokenStep.End;
                                Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepBack);
                            }
                            else
                            {
                                result = MoveStatus.Done;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token ExeStrikeBack", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
                    }

                    break;
                case TokenStep.End:
                    if (Unit.UnitHexSprite.ActionDone)
                    {
                        result = MoveStatus.Done;
                    }

                    break;
            }

            if (result == MoveStatus.Done)
            {
                if (Unit.GetEnabled())
                {
                    Unit.UnitHexSprite.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Move);
                    Unit.UnitHexSprite.SelectedAbility = -1;
                    Unit.MovePoints = 0;
                    Unit.UnitHexSprite.Unselect();
                    if (Unit.CombatPlayer == CombatMap.Current.CurrentCombatPlayer)
                    {
                        var hexX = Unit.UnitHexSprite.HexX;
                        CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastX = hexX;
                        var hexY = Unit.UnitHexSprite.HexY;
                        CombatMap.Current.Players.GetPlayers(CombatMap.Current.CurrentCombatPlayer).LastY = hexY;
                    }
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Token", "Error - TCTouchToken " + IntToStr(AbilityId), 0);
        }

        return result;
    }

    // intf_P5_07.inc:765

}
