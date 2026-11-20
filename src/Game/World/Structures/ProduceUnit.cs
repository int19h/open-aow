
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// Production id 1: produce unit production Index, for AI group GroupId when not 0.
public sealed record ProduceUnit(int Index, int GroupId) : ProductionOrder
{
    public const int Kind = 1;

    public override (int Kind, int Parameter, int GroupId) ToFields() => (Kind, Index, GroupId);

    public override CityAction CityAction => CityAction.Produce;

    public override int? UnitProductionIndex => Index;

    public override string Name(ProductionControl control)
    {
        UnitProduction production = control.UnitProductions.GetItem(Index);
        if (production is null)
        {
            return AbilityRules.TranslateResourceString(control.GetEngine(), GameTables.UnknownText);
        }

        string format = control.UnitProductionGold(Index) != 0 ? GameText.ProduceFormat : GameText.SummonFormat;
        string unitName = GamePack.Current.UnitResources.GetUnitResource(production.UnitIndex).GetUnitName();
        return AbilityRules.Format(AbilityRules.TranslateResourceString(control.GetEngine(), format), [unitName]);
    }

    public override string NameProgressive(ProductionControl control)
    {
        UnitProduction production = control.UnitProductions.GetItem(Index);
        if (production is null)
        {
            return AbilityRules.TranslateResourceString(control.GetEngine(), GameTables.UnknownText);
        }

        string format = control.UnitProductionGold(Index) != 0 ? GameText.ProducingFormat : GameText.SummoningFormat;
        string unitName = GamePack.Current.UnitResources.GetUnitResource(production.UnitIndex).GetUnitName();
        return AbilityRules.Format(AbilityRules.TranslateResourceString(control.GetEngine(), format), [unitName]);
    }

    public override bool CanExecute(ProductionControl control, ref string reason)
    {
        bool result;
        Waiting(control, ref reason);
        Player players = StrategicMap.Current.Players.GetPlayers(control.GetPlayer());
        if (control.UnitProductionGold(Index) <= players.Gems)
        {
            Player fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(control.GetPlayer());
            if (control.UnitProductionMana(Index) <= fPlayersPlayers.MagicControl.Mana)
            {
                result = true;
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(control.GetEngine(), GameText.NotEnoughManaText);
            }
        }
        else
        {
            result = false;
            reason = AbilityRules.TranslateResourceString(control.GetEngine(), GameText.NotEnoughGoldText);
        }

        return result;
    }

    public override void Execute(ProductionControl control)
    {
        ProductionSettings settings = control.Settings;
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
            if (StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human && control.Settings.GroupId != 0)
            {
                control.GroupProductionProduced(control.Settings.Unit, control.Settings.GroupId);
            }

            control.Settings.Clear();
            control.ProductionDone();
        }
    }

    public override int ActivityImage(City city) => 4;

    public override string RealmActivity(City city) => "5";

    public override void ActivityLines(ProductionControl control, DelphiStringCollection lines)
    {
        Unit unit = control.CreateUnitProduction(Index);
        lines.Clear();
        _ = lines.Add(AbilityRules.TranslateResourceString(control.GetEngine(), GameText.ProducingText));
        _ = lines.Add(unit.GetName());
        _ = unit.Release();
    }
}
