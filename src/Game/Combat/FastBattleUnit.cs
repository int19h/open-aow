// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// round distance, state
public partial class FastBattleUnit : CombatUnit
{
    // AIGroup.pas:6778
    public new const int ClassId = 131791;

    // AIGroup.pas:6773
    public FastBattleUnit(ECustomNode owner) : base(owner)
    {
    }

    public int RoundDistance { get; set; }

    public Set<FastBattleUnitStateElement> State;

    // AIGroup.pas:6797
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        TransferSet(ref State, 1, 40, stream.Bytes, 0);
    }

    // AIGroup.pas:6862
    public override void GetTargetCombatValueDamageValue(CombatObject target, out int combatValue, out int damageValue)
    {
        Troop troop;
        AbilityTypeList abilities;
        AbilityDamageValues values = default;
        int value;
        Ability ability;
        if (!Status.Contains(CombatObjectStatusElement.Destroyed))
        {
            troop = Troop;
            combatValue = 0;
            damageValue = 0;
            abilities = GamePack.Current.AbilityControl.AbilitiesByType[(int)AbilityType.Combat];
            for (int I = abilities.Count - 1; I >= 0; I--)
            {
                ability = abilities.GetAbility(I);
                if (troop.GetAbilityEnabled(ability.AbilityId))
                {
                    ability.FastBattleGetDamageValues(this, target, WallSituation.None, ref values);
                    if (values.SourceDamage > 0)
                    {
                        value = values.SourceDamage * values.SourceDamage / (values.TargetDamage + values.SourceDamage);
                        if (value <= 0)
                        {
                            value = 1;
                        }

                        if (value > combatValue)
                        {
                            combatValue = value;
                        }

                        if (values.SourceDamage > damageValue)
                        {
                            damageValue = values.SourceDamage;
                        }
                    }
                }
            }

            if (troop.GetAbilityEnabled(52))
            {
                ability = GamePack.Current.AbilityControl.GetAbility(52);
                ability.TacticalBattleGetDamageValues(this, target, ref values);
                if (values.SourceDamage > 0)
                {
                    value = values.SourceDamage * values.SourceDamage / (values.TargetDamage + values.SourceDamage);
                    if (value <= 0)
                    {
                        value = 1;
                    }

                    if (value > combatValue)
                    {
                        combatValue = value;
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

    // AIGroup.pas:6913
    public override int GetTargetWallCombatValue(CombatObject target)
    {
        int best;
        WallSituation wall;
        Troop troop;
        AbilityTypeList abilities;
        int value;
        Ability ability;
        AbilityDamageValues values = default;
        troop = Troop;
        best = 0;
        if (Combat.CombatData.Players.GetPlayers(CombatPlayer).SideIndex == 1)
        {
            wall = WallSituation.Source;
        }
        else
        {
            wall = WallSituation.Target;
        }

        abilities = GamePack.Current.AbilityControl.AbilitiesByType[(int)AbilityType.Combat];
        for (int I = abilities.Count - 1; I >= 0; I--)
        {
            ability = abilities.GetAbility(I);
            if (troop.GetAbilityEnabled(ability.AbilityId))
            {
                ability.FastBattleGetDamageValues(this, target, wall, ref values);
                if (values.SourceDamage > 0)
                {
                    value = values.SourceDamage * values.SourceDamage / (values.TargetDamage + values.SourceDamage);
                    if (value <= 0)
                    {
                        value = 1;
                    }

                    if (value > best)
                    {
                        best = value;
                    }
                }
            }
        }

        return best;
    }

    // AIGroup.pas:6946
    public override int GetTargetPriority()
    {
        int I;
        int combatValue;
        CombatObject target;
        int count;
        if (TargetPriorityInvalid)
        {
            try
            {
                if (Combat.CombatData.WallType == 0 || Combat.CombatData.WallBreached)
                {
                    TargetPriority = 0;
                    count = 0;
                    I = TargetedBy.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        if (TargetedBy.List[I].Second == 0)
                        {
                            count += 1;
                        }
                    }

                    I = Targets.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        combatValue = Targets.List[I].Second;
                        if (combatValue != 0)
                        {
                            target = Targets.List[I].First;
                            combatValue = Round((double)(combatValue * combatValue) / target.IncomingCombatValue);
                            TargetPriority += combatValue;
                        }
                    }

                    TargetPriority = TargetPriority * (count + 2);
                    I = TargetedBy.Count;
                    // { The original stores normalized priority in the target-strength field. }
                    if (I != 0)
                    {
                        TargetStrength = TargetPriority / I;
                    }
                    else
                    {
                        TargetStrength = 0;
                    }
                }
                else
                {
                    TargetPriority = 0;
                    count = 0;
                    I = TargetedBy.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        if (TargetedBy.List[I].Third == 0)
                        {
                            count += 1;
                        }
                    }

                    I = Targets.Count - 1;
                    for (; I >= 0; I -= 1)
                    {
                        combatValue = Targets.List[I].Third;
                        if (combatValue != 0)
                        {
                            target = Targets.List[I].First;
                            combatValue = Round((double)(combatValue * combatValue) / target.IncomingCombatValue);
                            TargetPriority += combatValue;
                        }
                    }

                    TargetPriority = TargetPriority * (count + 2);
                    I = TargetedBy.Count;
                    if (I != 0)
                    {
                        TargetStrength = TargetPriority / I;
                    }
                    else
                    {
                        TargetStrength = 0;
                    }
                }

                TargetPriorityInvalid = false;
                var commandingTargetPriority = CommandingTargetPriority();
                TargetPriority += commandingTargetPriority * 2;
            }
            catch
            {
            }
        }

        return TargetPriority;
    }

    // AIGroup.pas:6850
    public override void SetCombatPlayer(byte player)
    {
        base.SetCombatPlayer(player);
        UpdateWallSiegeObjectRegistration();
    }

    // AIGroup.pas:7011
    public override void Activate(Battle combat)
    {
        base.Activate(combat);
        if (combat is FastBattle)
        {
            UpdateWallSiegeObjectRegistration();
        }
    }

    // AIGroup.pas:7017
    public override void Deactivate()
    {
        base.Deactivate();
        if (Combat is FastBattle)
        {
            UpdateWallSiegeObjectRegistration();
        }
    }

    // AIGroup.pas:7002
    public override void Initialize(Battle combat)
    {
        Troop troop;
        base.Initialize(combat);
        troop = Troop;
        if (troop is Leader || troop is Hero)
        {
            RoundDistance = 8;
        }
        else
        {
            RoundDistance = 4;
        }
    }

    // AIGroup.pas:6856
    public override int FastBattleRoundDistance()
    {
        int result = RoundDistance;
        if (State.Contains(FastBattleUnitStateElement.Vulnerable))
        {
            result -= 1;
        }

        return result;
    }

    // AIGroup.pas:7048
    public override bool FastBattleExecute(int moveCost)
    {
        FastBattle combat;
        FastBattleCommandList Commands;
        Troop troop;
        FleeCombatAction action;
        bool executed;
        combat = (FastBattle)Combat;
        var side = GetSide();
        Commands = new FastBattleCommandList(this, combat, combat.Sides[(int)side]);
        troop = Troop;
        for (int I = troop.GetAbilityCount() - 1; I >= 0; I--)
        {
            if (troop.GetAbilityEnabled(I))
            {
                var ability = GamePack.Current.AbilityControl.GetAbility(I);
                ability.FastBattlePrefetchCombatCommands(Commands);
            }
        }

        if (Commands.GetCount() == 0)
        {
            if (FastBattleBehindWall())
            {
                if (((FastBattle)Combat).AttackerCanCrushWall() || (double)((FastBattle)Combat).GetAttackerBallisticCombatValue() > (double)((FastBattle)Combat).GetDefenderBallisticCombatValue() * 1.5)
                {
                    State = State + Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.OutsideWall);
                    troop = Troop;
                    for (int I = troop.GetAbilityCount() - 1; I >= 0; I--)
                    {
                        if (troop.GetAbilityEnabled(I))
                        {
                            var ability = GamePack.Current.AbilityControl.GetAbility(I);
                            ability.FastBattlePrefetchCombatCommands(Commands);
                        }
                    }

                    State = State - Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.OutsideWall);
                }
            }
        }

        if (GetSide() == CombatSideIndex.Defender && AttackerCount == 0 && !Combat.CombatData.GetSideLeaderInvolved(1))
        {
            for (int I = Commands.GetCount() - 1; I >= 0; I--)
            {
                var commands = Commands.GetCommands(I);
                var hitPoints = GetHitPoints();
                if (commands.TargetDamageValue >= hitPoints * 75)
                {
                    Commands.Delete(I);
                }
            }
        }

        if (Commands.GetCount() != 0)
        {
            Commands.Sort();
            if (Commands.GetCommands(0).MoveCost <= moveCost)
            {
                var commands = Commands.GetCommands(0);
                var commandsCommands = Commands.GetCommands(0);
                executed = GamePack.Current.AbilityControl.GetAbility(commandsCommands.AbilityId).FastBattleExecuteCombatCommand(this, commands);
                if (Commands.GetCommands(0).AbilityId == 52)
                {
                    UpdateWallSiegeObjectRegistration();
                }

                TurnDone();
            }
            else
            {
                executed = false;
            }
        }
        else
        {
            if (moveCost == 2)
            {
                if (CombatValue == 0 && IncomingCombatValue != 0 && GetSide() == CombatSideIndex.Attacker && !GetLocked())
                {
                    action = new FleeCombatAction(null) { Holder = CreationHolder() };
                    action.ObjectId = Id;
                    Combat.ExecuteCombatAction(action);
                    action.Release();
                    executed = true;
                }
                else
                {
                    executed = false;
                }
            }
            else
            {
                executed = false;
            }
        }

        Commands?.Dispose();
        return executed;
    }

    // AIGroup.pas:7035
    public override bool FastBattleBehindWall()
    {
        bool result;
        byte position;
        // { Inner failure shares the validated-position return. }
        if (GetSide() == CombatSideIndex.Defender && !State.Contains(FastBattleUnitStateElement.OutsideWall))
        {
            position = Position;
            if ((position & 15) > 7 || (int)(((uint)position) >> 4) > 7 || Combat.CombatData.Parties.GetParty((byte)(int)(((uint)position) >> 4)).Wall == 0 || Combat.CombatData.Wall.Status.Contains(CombatObjectStatusElement.Destroyed))
            {
                result = false;
            }
            else
            {
                result = true;
            }

            return result;
        }

        return false;
    }

    // AIGroup.pas:7029
    public override bool FastBattleWallInBetween(byte target) => !State.Contains(FastBattleUnitStateElement.OutsideWall) && Combat.GetWallInBetween(Position, target);

    // AIGroup.pas:6830
    public override void Show(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        if (State.Contains(FastBattleUnitStateElement.OutsideWall))
        {
            GamePack.Current.CombatImages[21].Show(X + 10, Y, surface);
        }

        base.Show(frame, direction, X, Y, surface);
    }

    // AIGroup.pas:6836
    public override int ShowAttackAnimation(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        if (direction != 6 && State.Contains(FastBattleUnitStateElement.OutsideWall))
        {
            GamePack.Current.CombatImages[21].Show(X + 10, Y, surface);
        }

        return base.ShowAttackAnimation(frame, direction, X, Y, surface);
    }

    // AIGroup.pas:6843
    public override int ShowHitAnimation(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        if (State.Contains(FastBattleUnitStateElement.OutsideWall))
        {
            GamePack.Current.CombatImages[21].Show(X + 10, Y, surface);
        }

        // { The original uses the attack animation for this hit-animation override. }
        return base.ShowAttackAnimation(frame, direction, X, Y, surface);
    }

    // AIGroup.pas:7023
    public override void DestroyObject()
    {
        base.DestroyObject();
        if (Combat is FastBattle)
        {
            UpdateWallSiegeObjectRegistration();
        }
    }

    // AIGroup.pas:6792
    public override void SetUnit(Troop troop)
    {
        base.SetUnit(troop);
    }

    // AIGroup.pas:6783
    public override void KillUnit()
    {
        Troop troop;
        troop = Troop;
        if (troop is Hero && Combat.Status.Contains(CombatStatusElement.Executing))
        {
            var combatData = Combat.CombatData;
            ((Hero)troop).DropItemsOnMap(StrategicMap.Current, (sbyte)combatData.X, (sbyte)combatData.Y, (sbyte)combatData.MapLevel);
        }

        base.KillUnit();
    }

    // AIGroup.pas:6803
    public bool CanCastWallSiegeSpell() => Troop.GetAbilityEnabled(52) && ((SpellCastingAbility)GamePack.Current.AbilityControl.GetAbility(52)).CanCastBreachWallSpell(Troop);

    // AIGroup.pas:6809
    public void UpdateWallSiegeObjectRegistration()
    {
        bool registered;
        registered = Status.Contains(CombatObjectStatusElement.Activated) && GetSide() == CombatSideIndex.Attacker && !Status.Contains(CombatObjectStatusElement.Destroyed) && ((WallCombatFeaturesValue & Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.WallPassing)) != Set<WallCombatFeaturesElement>.Of() || CanCastWallSiegeSpell());
        if (registered)
        {
            if (!State.Contains(FastBattleUnitStateElement.WallSiegeObjectRegistered))
            {
                ((FastBattle)Combat).WallSiegeObjectCount += 1;
                State = State + Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.WallSiegeObjectRegistered);
            }
        }
        else
        {
            if (State.Contains(FastBattleUnitStateElement.WallSiegeObjectRegistered))
            {
                ((FastBattle)Combat).WallSiegeObjectCount -= 1;
                State = State - Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.WallSiegeObjectRegistered);
            }
        }
    }

    // intf_P5_07.inc:1031

}
