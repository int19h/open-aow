
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// Production id 2: research unit production Index.
public sealed partial record ResearchSpell(int Index) : ProductionOrder
{
    public const int Kind = 2;

    public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, Index, 0);

    public override CityAction CityAction => CityAction.Research;

    public override int? UnitProductionIndex => Index;

    public override string Name(ProductionControl control)
    {
        var production = control.UnitProductions.GetItem(Index);
        if (production == null)
        {
            return AbilityRules.TranslateResourceString(GameTables.UnknownText);
        }

        var unitName = GamePack.Current.UnitResources.GetUnitResource(production.UnitIndex).GetUnitName();
        return AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.InstallFormat), [unitName]);
    }

    public override string NameProgressive(ProductionControl control)
    {
        var production = control.UnitProductions.GetItem(Index);
        if (production == null)
        {
            return AbilityRules.TranslateResourceString(GameTables.UnknownText);
        }

        var unitName = GamePack.Current.UnitResources.GetUnitResource(production.UnitIndex).GetUnitName();
        return AbilityRules.Format(AbilityRules.TranslateResourceString(s_installingFormat), [unitName]);
    }

    public override bool CanExecute(ProductionControl control, ref string reason)
    {
        Waiting(control, ref reason);
        var players = StrategicMap.Current.Players.GetPlayers(control.GetPlayer());
        if (control.UnitResearchGold(Index) <= players.Gems)
        {
            return true;
        }

        reason = AbilityRules.TranslateResourceString(GameText.NotEnoughGoldText);
        return false;
    }

    public override void Execute(ProductionControl control)
    {
        var settings = control.Settings;
        settings.Clear();
        settings.UnitProductionIndex = Index;
        settings.Gold = control.UnitResearchGold(Index);
        settings.Mana = 0;
        settings.TurnsTotal = (sbyte)control.UnitResearchTime(Index);
        settings.Player = control.GetPlayer();
        settings.TurnsLeft = settings.TurnsTotal;
        Pay(control);
        control.Status = ProductionControlStatus.Producing;
    }

    public override void Complete(ProductionControl control, sbyte player)
    {
        control.UnitResearched();
        control.Settings.Clear();
        control.ProductionDone();
    }

    public override bool Loopable(ProductionControl control) => false;

    public override int ActivityImage(City city) => 5;

    public override int? CitySequence(City city, int row) => -1;

    public override string RealmActivity(City city) => "5";

    public override void ActivityLines(ProductionControl control, StringCollection lines)
    {
        var unit = control.CreateUnitProduction(Index);
        lines.Add(AbilityRules.TranslateResourceString(s_installingText));
        lines.Add(unit.GetName());
        unit.Release();
    }

    private static readonly string s_installingText = "Installing";
    private static readonly string s_installingFormat = "Installing %s";
}
