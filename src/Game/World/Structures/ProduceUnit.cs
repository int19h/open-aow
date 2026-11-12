
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// Production id 1: produce unit production Index, for AI group GroupId when not 0.
public sealed record ProduceUnit(int Index, int GroupId) : ProductionOrder
{
    public const int Kind = 1;

    public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, Index, GroupId);

    public override CityAction CityAction => CityAction.Produce;

    public override int? UnitProductionIndex => Index;

    public override string Name(ProductionControl control)
    {
        var production = control.UnitProductions.GetItem(Index);
        if (production == null)
        {
            return AbilityRules.TranslateResourceString(GameTables.s_unknownText);
        }

        var format = control.UnitProductionGold(Index) != 0 ? GameText.s_produceFormat : GameText.s_summonFormat;
        var unitName = AoWHSSet.Current.UnitResources.GetUnitResource(production.UnitIndex).GetUnitName();
        return AbilityRules.Format(AbilityRules.TranslateResourceString(format), [unitName]);
    }

    public override string NameProgressive(ProductionControl control)
    {
        var production = control.UnitProductions.GetItem(Index);
        if (production == null)
        {
            return AbilityRules.TranslateResourceString(GameTables.s_unknownText);
        }

        var format = control.UnitProductionGold(Index) != 0 ? GameText.s_producingFormat : GameText.s_summoningFormat;
        var unitName = AoWHSSet.Current.UnitResources.GetUnitResource(production.UnitIndex).GetUnitName();
        return AbilityRules.Format(AbilityRules.TranslateResourceString(format), [unitName]);
    }

    public override bool CanExecute(ProductionControl control, ref string reason)
    {
        bool result;
        Waiting(control, ref reason);
        var players = AoWHSMap.Current.Players.GetPlayers(control.GetPlayer());
        if (control.UnitProductionGold(Index) <= players.Gems)
        {
            var fPlayersPlayers = AoWHSMap.Current.Players.GetPlayers(control.GetPlayer());
            if (control.UnitProductionMana(Index) <= fPlayersPlayers.MagicControl.Mana)
            {
                result = true;
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(GameText.s_notEnoughManaText);
            }
        }
        else
        {
            result = false;
            reason = AbilityRules.TranslateResourceString(GameText.s_notEnoughGoldText);
        }

        return result;
    }

    public override void Execute(ProductionControl control)
    {
        var settings = control.Settings;
        settings.Clear();
        settings.UnitProductionIndex = Index;
        settings.Unit = control.CreateUnitProduction(Index);
        settings.Gold = control.UnitProductionGold(Index);
        settings.Mana = control.UnitProductionMana(Index);
        settings.GoldUpkeep = control.UnitProductionGoldUpkeep(Index);
        settings.ManaUpkeep = control.UnitProductionManaUpkeep(Index);
        settings.GroupId = GroupId;
        settings.TurnsTotal = (sbyte)control.UnitProductionTime(Index);
        settings.Player = control.GetPlayer();
        settings.TurnsLeft = settings.TurnsTotal;
        Pay(control);
        control.Status = ProductionControlStatus.Producing;
        if (settings.GroupId != 0)
        {
            control.RegisterGroupProduction();
        }
    }

    public override void Complete(ProductionControl control, sbyte player)
    {
        if (control.UnitProduced())
        {
            if (AoWHSMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human && control.Settings.GroupId != 0)
            {
                control.GroupProductionProduced(control.Settings.Unit, control.Settings.GroupId);
            }

            control.Settings.Clear();
            control.ProductionDone();
        }
    }

    public override int ActivityImage(City city) => 4;

    public override string RealmActivity(City city) => "5";

    public override void ActivityLines(ProductionControl control, StringCollection lines)
    {
        var unit = control.CreateUnitProduction(Index);
        lines.Clear();
        lines.Add(AbilityRules.TranslateResourceString(GameText.s_producingText));
        lines.Add(unit.GetName());
        unit.Release();
    }
}
