
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

    public override string Name(ProductionControl Control)
    {
        var production = Control.FUnitProductions.GetItem(Index);
        if (production == null)
        {
            return AbilityRules.TranslateRStr(GameTables.UnknownRStr);
        }

        var format = Control.UnitProductionGold(Index) != 0 ? GameText.ProduceXRStr : GameText.SummonXRStr;
        var unitName = AoWHSSet.Current.FUnitResources.GetUnitResource(production.FUnitIndex).GetUnitName();
        return AbilityRules.Format(AbilityRules.TranslateRStr(format), [unitName]);
    }

    public override string NameProgressive(ProductionControl Control)
    {
        var production = Control.FUnitProductions.GetItem(Index);
        if (production == null)
        {
            return AbilityRules.TranslateRStr(GameTables.UnknownRStr);
        }

        var format = Control.UnitProductionGold(Index) != 0 ? GameText.ProducingXRStr : GameText.SummoningXRStr;
        var unitName = AoWHSSet.Current.FUnitResources.GetUnitResource(production.FUnitIndex).GetUnitName();
        return AbilityRules.Format(AbilityRules.TranslateRStr(format), [unitName]);
    }

    public override bool CanExecute(ProductionControl Control, ref string Reason)
    {
        bool Result;
        Waiting(Control, ref Reason);
        var players = AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer());
        if (Control.UnitProductionGold(Index) <= players.FGems)
        {
            var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer());
            if (Control.UnitProductionMana(Index) <= fPlayersPlayers.FMagicControl.FMana)
            {
                Result = true;
            }
            else
            {
                Result = false;
                Reason = AbilityRules.TranslateRStr(GameText.NotEnoughManaRStr);
            }
        }
        else
        {
            Result = false;
            Reason = AbilityRules.TranslateRStr(GameText.NotEnoughGoldRStr);
        }

        return Result;
    }

    public override void Execute(ProductionControl Control)
    {
        var settings = Control.FSettings;
        settings.Clear();
        settings.FUnitProductionIndex = Index;
        settings.FUnit = Control.CreateUnitProduction(Index);
        settings.FGold = Control.UnitProductionGold(Index);
        settings.FMana = Control.UnitProductionMana(Index);
        settings.FGoldUpkeep = Control.UnitProductionGoldUpkeep(Index);
        settings.FManaUpkeep = Control.UnitProductionManaUpkeep(Index);
        settings.FGroupID = GroupId;
        settings.FTurnsTotal = (sbyte)Control.UnitProductionTime(Index);
        settings.FPlayer = Control.GetPlayer();
        settings.FTurnsLeft = settings.FTurnsTotal;
        Pay(Control);
        Control.FStatus = ProductionControl.Status.Producing;
        if (settings.FGroupID != 0)
        {
            Control.RegisterGroupProduction();
        }
    }

    public override void Complete(ProductionControl Control, sbyte Player)
    {
        if (Control.UnitProduced())
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human && Control.FSettings.FGroupID != 0)
            {
                Control.GroupProductionProduced(Control.FSettings.FUnit, Control.FSettings.FGroupID);
            }

            Control.FSettings.Clear();
            Control.ProductionDone();
        }
    }

    public override int ActivityImage(City City) => 4;

    public override string RealmActivity(City City) => "5";

    public override void ActivityLines(ProductionControl Control, StringCollection Lines)
    {
        var unit = Control.CreateUnitProduction(Index);
        Lines.Clear();
        Lines.Add(AbilityRules.TranslateRStr(GameText.ProducingRStr));
        Lines.Add(unit.GetName());
        unit.Release();
    }
}
