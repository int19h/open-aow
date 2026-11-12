// Translated from Arena.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;

using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

public partial class Arena : Structure
{
    public Arena(ECustomNode owner) : base(owner)
    { }

    // Arena.pas:95
    public new const int ClassId = 133276;

    // Arena.pas:212
    public override StructureTokenEvent CreateTokenEvent() => new ArenaTokenEvent(null);

    // Arena.pas:135
    public override ExecuteResult ExecuteTokenEvent(StructureTokenEvent tokenEvent, TokenExecutor executor)
    {
        Troop troop;
        string errorText = "";
        ExecuteResult result = base.ExecuteTokenEvent(tokenEvent, executor);
        if (tokenEvent.Action == 1)
        {
            if (CanTrainUnits((sbyte)tokenEvent.Param, ((ArenaTokenEvent)tokenEvent).UnitIds, ref errorText))
            {
                for (int I = 0, last = ((ArenaTokenEvent)tokenEvent).UnitIds.Count - 1; I <= last; I++)
                {
                    var unitId = ((ArenaTokenEvent)tokenEvent).UnitIds[I];
                    troop = AoWHSMap.Current.UnitControl.FindUnit(unitId);
                    if (troop != null)
                    {
                        var player = AoWHSMap.Current.Players.GetPlayers(tokenEvent.Param);
                        var unitTrainCost = UnitTrainCost(troop);
                        AoWHSMap.Current.Players.GetPlayers(tokenEvent.Param).SetGems(player.Gems - unitTrainCost);
                        ExecuteTrainUnit(troop);
                    }
                }

                if (AoWHSMap.Current.SeatedPlayer == tokenEvent.Param)
                {
                    var arenaTokenEvent = (ArenaTokenEvent)tokenEvent;
                    if (arenaTokenEvent.UnitIds.Count == 1)
                    {
                        var oneUnitTrained = AbilityRules.TranslateResourceString(s_oneUnitTrainedText);
                        AoWHSMap.Current.ShowMessage(oneUnitTrained);
                    }
                    else
                    {
                        var format = RtlUnit.Format(AbilityRules.TranslateResourceString(s_unitsTrainedFormat), [IntToStr(arenaTokenEvent.UnitIds.Count)]);
                        AoWHSMap.Current.ShowMessage(format);
                    }
                }
            }
        }

        return result;
    }

    // Arena.pas:98
    public override bool CanDoubleClick() => CanEnter(AoWHSMap.Current.SeatedPlayer);

    // Arena.pas:101
    public int UnitTrainCost(Troop troop)
    {
        int result;
        if (troop is Hero)
        {
            // { The original rejects heroes, although ExecuteTrainUnit supports them. }
            return -1;
        }

        if (troop is Unit)
        {
            var unitLocal = (Unit)troop;
            if (unitLocal.GetRank() != Unit.Rank.Gold)
            {
                result = (unitLocal.GetNextLevelExperience() - troop.GetExperience()) * 5;
            }
            else
            {
                result = -1;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Arena.pas:116
    public void ExecuteTrainUnit(Troop troop)
    {
        if (troop is Hero)
        {
            var hero = troop;
            hero.SetExperience(hero.GetNextLevelExperience());
            hero.Changed();

            return;
        }

        if (troop is Unit)
        {
            var troopLocal = troop;
            troopLocal.SetExperience(troopLocal.GetNextLevelExperience());
            troopLocal.Changed();
        }
    }

    // Arena.pas:159
    public bool CanTrainUnits(sbyte player, Int32List units, ref string errorText)
    {
        bool result;
        int total;
        int I;
        int cost;
        Troop troop;
        if (units.Count != 0)
        {
            total = 0;
            I = units.Count;
            result = true;
            while (result && I != 0)
            {
                I -= 1;
                var unit = units[I];
                troop = AoWHSMap.Current.UnitControl.FindUnit(unit);
                if (troop != null)
                {
                    cost = UnitTrainCost(troop);
                    if (cost != -1)
                    {
                        total += cost;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }

            if (result)
            {
                if (AoWHSMap.Current.Players.GetPlayers(player).Gems >= total)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    errorText = AbilityRules.TranslateResourceString(GameText.s_notEnoughGoldText);
                }
            }
        }
        else
        {
            result = false;
            errorText = AbilityRules.TranslateResourceString(GameText.s_noUnitSelectedText);
        }

        return result;
    }

    // Arena.pas:194
    public bool TrainUnits(sbyte player, Int32List units, ref string errorText)
    {
        bool result;
        ArenaTokenEvent tokenEvent;
        errorText = "";
        if (CanTrainUnits(player, units, ref errorText) && !AoWHSMap.Current.Players.GetPlayers(player).GetBusy())
        {
            tokenEvent = (ArenaTokenEvent)CreateTokenEvent();
            SetupTokenEvent(tokenEvent);
            tokenEvent.UnitIds.Assign(units);
            tokenEvent.Param = player;
            tokenEvent.Action = 1;
            AoWHSMap.Current.TokenManager.GetTokenControl(player).AddEvent(tokenEvent);
            tokenEvent.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Arena.pas:215
    public bool CanEnter(sbyte player)
    {
        bool result;
        ArmyHexSprite army;
        army = (ArmyHexSprite)_owner.FindOwnedChild(131607);
        if (army == null || army.Army.Player != player)
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Arena.pas:223
    public bool Enter()
    {
        bool result = default;
        EnterArenaEventLog log;
        ArmyHexSprite army;
        if (CanEnter(AoWHSMap.Current.SeatedPlayer))
        {
            log = new EnterArenaEventLog(null);
            log.Arena = this;
            army = (ArmyHexSprite)_owner.FindOwnedChild(131607);
            for (int I = 0, last = army.Army.Count - 1; I <= last; I++)
            {
                var troop = army.Army.GetUnit(I);
                log.UnitIds.Add(troop.UnitId);
                if (UnitTrainCost(army.Army.GetUnit(I)) != -1)
                {
                    log.Trainable = (byte)(log.Trainable | (1 << I));
                }
            }

            log.Execute(true, ExecuteTrigger.User, null);
            log.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_oneUnitTrainedText = "1 unit trained";
    private static readonly string s_unitsTrainedFormat = "%s units trained";

    public new partial class Resource : StructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            CastsShadow = true;
        }

        // Arena.pas:253
        public new const int ClassId = 133277;

        // Arena.pas:244

        // Arena.pas:256
        public override int ResourceUserClassId() => 133276;

        // Arena.pas:45

        // Arena.pas:250
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_arenaText);

        private static readonly string s_arenaText = "Arena";

    }

}
