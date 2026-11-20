#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Combat;

// round distance, state
public class FastBattleUnit(CustomNode owner) : CombatUnit(owner)
{
    // AIGroup.pas:6778
    public new const int ClassId = 131791;

    public int RoundDistance { get; set; }

    // Combat is assigned when the battle initializes the unit, after construction. A fast battle
    // creates only this unit, so the battle is a fast battle. Null stays null. Another battle fails.
    private FastBattle FastCombat => (FastBattle)Combat;

    public Set<FastBattleUnitStateElement> State { get; set; }

    // AIGroup.pas:6797
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        State = persister.Set("State", value: State);
    }

    // AIGroup.pas:6862
    public override void GetTargetCombatValueDamageValue(CombatObject target, out int combatValue, out int damageValue)
    {
        if (!Status.Contains(CombatObjectStatusElement.Destroyed))
        {
            Troop troop = Troop;
            combatValue = 0;
            damageValue = 0;
            AbilityTypeList abilities = GamePack.Current.AbilityControl.AbilitiesByType[(int)AbilityType.Combat];
            AbilityDamageValues values = default;
            int value;
            Ability ability;
            for (int i = abilities.Count - 1; i >= 0; i--)
            {
                ability = abilities.GetAbility(i);
                if (troop.GetAbilityEnabled(ability.AbilityId))
                {
                    ability.FastBattleGetDamageValues(this, defender: target, wallSituation: WallSituation.None, damageValues: ref values);
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
                ability = GamePack.Current.AbilityControl[52];
                ability.TacticalBattleGetDamageValues(this, defender: target, damageValues: ref values);
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
        AbilityDamageValues values = default;
        Troop troop = Troop;
        int best = 0;
        WallSituation wall = Combat.CombatData.Players.GetPlayers(CombatPlayer).SideIndex == 1 ? WallSituation.Source : WallSituation.Target;

        AbilityTypeList abilities = GamePack.Current.AbilityControl.AbilitiesByType[(int)AbilityType.Combat];
        for (int i = abilities.Count - 1; i >= 0; i--)
        {
            Ability ability = abilities.GetAbility(i);
            if (troop.GetAbilityEnabled(ability.AbilityId))
            {
                ability.FastBattleGetDamageValues(this, defender: target, wallSituation: wall, damageValues: ref values);
                if (values.SourceDamage > 0)
                {
                    int value = values.SourceDamage * values.SourceDamage / (values.TargetDamage + values.SourceDamage);
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
        if (TargetPriorityInvalid)
        {
            try
            {
                int i;
                int combatValue;
                CombatObject target;
                int count;
                if (Combat.CombatData.WallType == 0 || Combat.CombatData.WallBreached)
                {
                    TargetPriority = 0;
                    count = 0;
                    i = TargetedBy.Count - 1;
                    for (; i >= 0; i -= 1)
                    {
                        if (TargetedBy.List[i].Second == 0)
                        {
                            count += 1;
                        }
                    }

                    i = Targets.Count - 1;
                    for (; i >= 0; i -= 1)
                    {
                        combatValue = Targets.List[i].Second;
                        if (combatValue != 0)
                        {
                            target = Targets.List[i].First;
                            combatValue = Round((double)(combatValue * combatValue) / target.IncomingCombatValue);
                            TargetPriority += combatValue;
                        }
                    }

                    TargetPriority *= count + 2;
                    i = TargetedBy.Count;
                    // { The original stores normalized priority in the target-strength field. }
                    TargetStrength = i != 0 ? TargetPriority / i : 0;
                }
                else
                {
                    TargetPriority = 0;
                    count = 0;
                    i = TargetedBy.Count - 1;
                    for (; i >= 0; i -= 1)
                    {
                        if (TargetedBy.List[i].Third == 0)
                        {
                            count += 1;
                        }
                    }

                    i = Targets.Count - 1;
                    for (; i >= 0; i -= 1)
                    {
                        combatValue = Targets.List[i].Third;
                        if (combatValue != 0)
                        {
                            target = Targets.List[i].First;
                            combatValue = Round((double)(combatValue * combatValue) / target.IncomingCombatValue);
                            TargetPriority += combatValue;
                        }
                    }

                    TargetPriority *= count + 2;
                    i = TargetedBy.Count;
                    TargetStrength = i != 0 ? TargetPriority / i : 0;
                }

                TargetPriorityInvalid = false;
                int commandingTargetPriority = CommandingTargetPriority();
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
        base.Initialize(combat);
        Troop troop = Troop;
        RoundDistance = troop is Leader or Hero ? 8 : 4;
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
        FastBattle combat = FastCombat;
        CombatSideIndex side = Side;
        FastBattleCommandList fastBattleCommandList = new(this, combat, side: combat.Sides[(int)side]);
        Troop troop = Troop;
        for (int i = troop.GetAbilityCount() - 1; i >= 0; i--)
        {
            if (troop.GetAbilityEnabled(i))
            {
                Ability ability = GamePack.Current.AbilityControl[i];
                ability.FastBattlePrefetchCombatCommands(fastBattleCommandList);
            }
        }

        if (fastBattleCommandList.GetCount() == 0)
        {
            if (FastBattleBehindWall())
            {
                if (FastCombat.AttackerCanCrushWall() || FastCombat.GetAttackerBallisticCombatValue() > FastCombat.GetDefenderBallisticCombatValue() * 1.5)
                {
                    State += Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.OutsideWall);
                    troop = Troop;
                    for (int i = troop.GetAbilityCount() - 1; i >= 0; i--)
                    {
                        if (troop.GetAbilityEnabled(i))
                        {
                            Ability ability = GamePack.Current.AbilityControl[i];
                            ability.FastBattlePrefetchCombatCommands(fastBattleCommandList);
                        }
                    }

                    State -= Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.OutsideWall);
                }
            }
        }

        if (Side == CombatSideIndex.Defender && AttackerCount == 0 && !Combat.CombatData.GetSideLeaderInvolved(1))
        {
            for (int i = fastBattleCommandList.GetCount() - 1; i >= 0; i--)
            {
                FastBattleCommand commands = fastBattleCommandList[i];
                sbyte hitPoints = HitPoints;
                if (commands.TargetDamageValue >= hitPoints * 75)
                {
                    fastBattleCommandList.Delete(i);
                }
            }
        }

        bool executed;
        if (fastBattleCommandList.GetCount() != 0)
        {
            fastBattleCommandList.Sort();
            if (fastBattleCommandList[0].MoveCost <= moveCost)
            {
                FastBattleCommand commands = fastBattleCommandList[0];
                FastBattleCommand commandsCommands = fastBattleCommandList[0];
                executed = GamePack.Current.AbilityControl[commandsCommands.AbilityId].FastBattleExecuteCombatCommand(this, command: commands);
                if (fastBattleCommandList[0].AbilityId == 52)
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
                if (CombatValue == 0 && IncomingCombatValue != 0 && Side == CombatSideIndex.Attacker && !GetLocked())
                {
                    FleeCombatAction action = new(null)
                    {
                        Holder = CreationHolder(),
                        ObjectId = Id
                    };
                    Combat.ExecuteCombatAction(action);
                    _ = action.Release();
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

        fastBattleCommandList?.Dispose();
        return executed;
    }

    // AIGroup.pas:7035
    public override bool FastBattleBehindWall()
    {
        // { Inner failure shares the validated-position return. }
        if (Side == CombatSideIndex.Defender && !State.Contains(FastBattleUnitStateElement.OutsideWall))
        {
            byte position = Position;
            return (position & 15) <= 7 && position >>> 4 <= 7 && Combat.CombatData.Parties.GetParty((byte)(position >>> 4)).Wall != 0 && !Combat.CombatData.Wall.Status.Contains(CombatObjectStatusElement.Destroyed);
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
            GamePack.Current.CombatImages[21].Show(X + 10, Y, lockedSurface: surface);
        }

        base.Show(frame, direction, X, Y, surface);
    }

    // AIGroup.pas:6836
    public override int ShowAttackAnimation(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        if (direction != 6 && State.Contains(FastBattleUnitStateElement.OutsideWall))
        {
            GamePack.Current.CombatImages[21].Show(X + 10, Y, lockedSurface: surface);
        }

        return base.ShowAttackAnimation(frame, direction, X, Y, surface);
    }

    // AIGroup.pas:6843
    public override int ShowHitAnimation(int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        if (State.Contains(FastBattleUnitStateElement.OutsideWall))
        {
            GamePack.Current.CombatImages[21].Show(X + 10, Y, lockedSurface: surface);
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
    public override void SetUnit(Troop troop) => base.SetUnit(troop);

    // AIGroup.pas:6783
    public override void KillUnit()
    {
        Troop troop = Troop;
        if (troop is Hero hero && Combat.Status.Contains(CombatStatusElement.Executing))
        {
            CombatData combatData = Combat.CombatData;
            hero.DropItemsOnMap(StrategicMap.Current, combatData.Location.X, combatData.Location.Y, L: combatData.Location.MapLevel);
        }

        base.KillUnit();
    }

    // AIGroup.pas:6803
    public bool CanCastWallSiegeSpell() => Troop.GetAbilityEnabled(52) && GamePack.Current.AbilityControl.GetAbility<SpellCastingAbility>(52).CanCastBreachWallSpell(Troop);

    // AIGroup.pas:6809
    public void UpdateWallSiegeObjectRegistration()
    {
        bool registered = Status.Contains(CombatObjectStatusElement.Activated) && Side == CombatSideIndex.Attacker && !Status.Contains(CombatObjectStatusElement.Destroyed) && ((WallCombatFeaturesValue & Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.WallPassing)) != Set<WallCombatFeaturesElement>.Of() || CanCastWallSiegeSpell());
        if (registered)
        {
            if (!State.Contains(FastBattleUnitStateElement.WallSiegeObjectRegistered))
            {
                FastCombat.WallSiegeObjectCount += 1;
                State += Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.WallSiegeObjectRegistered);
            }
        }
        else
        {
            if (State.Contains(FastBattleUnitStateElement.WallSiegeObjectRegistered))
            {
                FastCombat.WallSiegeObjectCount -= 1;
                State -= Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.WallSiegeObjectRegistered);
            }
        }
    }
}
