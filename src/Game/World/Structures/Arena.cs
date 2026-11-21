#nullable disable warnings
// Translated from Arena.pas.
using System.Globalization;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiFormat;

namespace Aow.Game.World.Structures;

public partial class Arena(CustomNode owner) : Structure(owner)
{

    // Arena.pas:95
    public new const int ClassId = 133_276;

    // Arena.pas:212
    public override ArenaTokenEvent CreateTokenEvent() => new(null) { Holder = CreationHolder() };

    // Arena.pas:135
    public override ExecuteResult ExecuteTokenEvent(StructureTokenEvent tokenEvent, TokenExecutor executor)
    {
        ExecuteResult result = base.ExecuteTokenEvent(tokenEvent, executor);
        if (tokenEvent.Action == 1)
        {
            // Arena.CreateTokenEvent constructs ArenaTokenEvent for Arena actions.
            ArenaTokenEvent arenaTokenEvent = (ArenaTokenEvent)tokenEvent;
            string errorText = "";
            if (CanTrainUnits((sbyte)tokenEvent.Param, units: arenaTokenEvent.UnitIds, ref errorText))
            {
                for (int i = 0, last = arenaTokenEvent.UnitIds.Count - 1; i <= last; i++)
                {
                    int unitId = arenaTokenEvent.UnitIds[i];
                    Troop? troop = StrategicMap.Current.UnitControl.FindUnit(unitId);
                    if (troop is not null)
                    {
                        Player? player = StrategicMap.Current.Players.GetPlayers(tokenEvent.Param);
                        int unitTrainCost = UnitTrainCost(troop);
                        StrategicMap.Current.Players.GetPlayers(tokenEvent.Param).SetGems(player.Gems - unitTrainCost);
                        ExecuteTrainUnit(troop);
                    }
                }

                if (StrategicMap.Current.SeatedPlayer == tokenEvent.Param)
                {
                    if (arenaTokenEvent.UnitIds.Count == 1)
                    {
                        string oneUnitTrained = AbilityRules.TranslateResourceString(GetEngine(), S: s_oneUnitTrainedText);
                        StrategicMap.Current.ShowMessage(oneUnitTrained);
                    }
                    else
                    {
                        string format = Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_unitsTrainedFormat), args: [arenaTokenEvent.UnitIds.Count.ToString(CultureInfo.InvariantCulture)]);
                        StrategicMap.Current.ShowMessage(format);
                    }
                }
            }
        }

        return result;
    }

    // Arena.pas:98
    public override bool CanDoubleClick() => CanEnter(StrategicMap.Current.SeatedPlayer);

    // Arena.pas:101
    public int UnitTrainCost(Troop? troop)
    {
        if (troop is Hero)
        {
            // { The original rejects heroes, although ExecuteTrainUnit supports them. }
            return -1;
        }

        return troop is Unit unitLocal && unitLocal.GetRank() != Unit.Rank.Gold ? (unitLocal.GetNextLevelExperience() - troop.GetExperience()) * 5 : -1;
    }

    // Arena.pas:116
    public void ExecuteTrainUnit(Troop troop)
    {
        if (troop is Hero)
        {
            Troop hero = troop;
            hero.SetExperience(hero.GetNextLevelExperience());
            hero.Changed();

            return;
        }

        if (troop is Unit)
        {
            Troop troopLocal = troop;
            troopLocal.SetExperience(troopLocal.GetNextLevelExperience());
            troopLocal.Changed();
        }
    }

    // Arena.pas:159
    public bool CanTrainUnits(sbyte player, Int32List units, ref string errorText)
    {
        bool result;
        if (units.Count != 0)
        {
            int total = 0;
            int i = units.Count;
            result = true;
            while (result && i != 0)
            {
                i -= 1;
                int unit = units[i];
                Troop? troop = StrategicMap.Current.UnitControl.FindUnit(unit);
                if (troop is not null)
                {
                    int cost = UnitTrainCost(troop);
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
                if (StrategicMap.Current.Players.GetPlayers(player).Gems >= total)
                {
                    return true;
                }
                else
                {
                    result = false;
                    errorText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NotEnoughGoldText);
                }
            }
        }
        else
        {
            result = false;
            errorText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NoUnitSelectedText);
        }

        return result;
    }

    // Arena.pas:194
    public bool TrainUnits(sbyte player, Int32List units, out string errorText)
    {
        errorText = "";
        if (CanTrainUnits(player, units, ref errorText) && !StrategicMap.Current.Players.GetPlayers(player).GetBusy())
        {
            ArenaTokenEvent tokenEvent = CreateTokenEvent();
            SetupTokenEvent(tokenEvent);
            tokenEvent.UnitIds.Assign(units);
            tokenEvent.Param = player;
            tokenEvent.Action = 1;
            StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(tokenEvent);
            _ = tokenEvent.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Arena.pas:215
    public bool CanEnter(sbyte player)
    {
        ArmyHexSprite army = Owner.GetArmyHexSprite();
        return army is not null && army.Army.Player == player;
    }

    // Arena.pas:223
    public bool Enter()
    {
        if (CanEnter(StrategicMap.Current.SeatedPlayer))
        {
            EnterArenaEventLog log = new(null)
            {
                Holder = CreationHolder(),
                Arena = this
            };
            ArmyHexSprite army = Owner.GetArmyHexSprite();
            for (int i = 0, last = army.Army.Units.Count - 1; i <= last; i++)
            {
                Troop? troop = army.Army.Units.GetUnit(i);
                _ = log.UnitIds.Add(troop.UnitId);
                if (UnitTrainCost(army.Army.Units.GetUnit(i)) != -1)
                {
                    log.Trainable = (byte)(log.Trainable | (1 << i));
                }
            }

            _ = log.Execute(true, trigger: ExecuteTrigger.User, done: null);
            _ = log.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_oneUnitTrainedText = "1 unit trained";
    private static readonly string s_unitsTrainedFormat = "%s units trained";

    public new class Resource : StructureResource
    {
        public Resource(CustomNode owner) : base(owner) => CastsShadow = true;

        // Arena.pas:253
        public new const int ClassId = 133_277;

        // Arena.pas:256
        public override int ResourceUserClassId() => Arena.ClassId;

        // Arena.pas:250
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_arenaText);

        private static readonly string s_arenaText = "Arena";

    }
}
