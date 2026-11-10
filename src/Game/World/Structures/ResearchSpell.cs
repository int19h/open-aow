
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

    public override string Name(ProductionControl Control)
    {
        var production = Control.FUnitProductions.GetItem(Index);
        if (production == null)
        {
            return AbilityRules.TranslateRStr(GameTables.UnknownRStr);
        }

        var unitName = AoWHSSet.Current.FUnitResources.GetUnitResource(production.FUnitIndex).GetUnitName();
        return AbilityRules.Format(AbilityRules.TranslateRStr(GameText.InstallXRStr), [unitName]);
    }

    public override string NameProgressive(ProductionControl Control)
    {
        var production = Control.FUnitProductions.GetItem(Index);
        if (production == null)
        {
            return AbilityRules.TranslateRStr(GameTables.UnknownRStr);
        }

        var unitName = AoWHSSet.Current.FUnitResources.GetUnitResource(production.FUnitIndex).GetUnitName();
        return AbilityRules.Format(AbilityRules.TranslateRStr(InstallingXRStr), [unitName]);
    }

    public override bool CanExecute(ProductionControl Control, ref string Reason)
    {
        Waiting(Control, ref Reason);
        var players = AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer());
        if (Control.UnitResearchGold(Index) <= players.FGems)
        {
            return true;
        }

        Reason = AbilityRules.TranslateRStr(GameText.NotEnoughGoldRStr);
        return false;
    }

    public override void Execute(ProductionControl Control)
    {
        var settings = Control.FSettings;
        settings.Clear();
        settings.FUnitProductionIndex = Index;
        settings.FGold = Control.UnitResearchGold(Index);
        settings.FMana = 0;
        settings.FTurnsTotal = (sbyte)Control.UnitResearchTime(Index);
        settings.FPlayer = Control.GetPlayer();
        settings.FTurnsLeft = settings.FTurnsTotal;
        Pay(Control);
        Control.FStatus = ProductionControl.Status.Producing;
    }

    public override void Complete(ProductionControl Control, sbyte Player)
    {
        Control.UnitResearched();
        Control.FSettings.Clear();
        Control.ProductionDone();
    }

    public override bool Loopable(ProductionControl Control) => false;

    public override int ActivityImage(City City) => 5;

    public override int? CitySequence(City City, int Row) => -1;

    public override string RealmActivity(City City) => "5";

    public override void ActivityLines(ProductionControl Control, StringCollection Lines)
    {
        var unit = Control.CreateUnitProduction(Index);
        Lines.Add(AbilityRules.TranslateRStr(InstallingRStr));
        Lines.Add(unit.GetName());
        unit.Release();
    }

    private static readonly string InstallingRStr = "Installing";
    private static readonly string InstallingXRStr = "Installing %s";
}
