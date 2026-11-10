
using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// Production id 13: the city's people move out and Race moves in.
public sealed partial record Migrate(int Race) : ProductionOrder
{
    public const int Kind = 13;

    public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, Race, 0);

    public override CityAction CityAction => CityAction.Migrate;

    public override string Name(ProductionControl Control) => Control.ProductionCity() != null
        ? RtlUnit.Format(AbilityRules.TranslateRStr(MigrateToXRStr), [AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[Race])])
        : base.Name(Control);

    public override string NameProgressive(ProductionControl Control) => Control.ProductionCity() != null
        ? RtlUnit.Format(AbilityRules.TranslateRStr(MigratingToXRStr), [AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[Race])])
        : base.NameProgressive(Control);

    public override bool CanExecute(ProductionControl Control, ref string Reason)
    {
        var city = Control.ProductionCity();
        return city != null ? city.CanMigrate((sbyte)Race, ref Reason) : base.CanExecute(Control, ref Reason);
    }

    public override void Executed(ProductionControl Control)
    {
        var city = Control.ProductionCity();
        if (city == null)
        {
            return;
        }

        Control.FSettings.FTurnsTotal = (sbyte)city.MigrationTime((sbyte)Race);
        Control.FSettings.FTurnsLeft = Control.FSettings.FTurnsTotal;
        Control.FSettings.FPlayer = Control.GetPlayer();
        Control.FStatus = ProductionControl.Status.Producing;
        ChangeRelation(Control, city, -15);
    }

    public override void Completed(ProductionControl Control)
    {
        var city = Control.ProductionCity();
        if (city == null)
        {
            return;
        }

        Control.BeginUpdate();
        var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[Race]);
        Log(Control, city, RtlUnit.Format(AbilityRules.TranslateRStr(CityMigratedToRStr), [city.GetName(), raceNamesPlural]));
        city.SetRace((sbyte)Race);
        ChangeRelation(Control, city, 10);
        Control.FUnitProductions.Clear();
        city.SetUpgradeLevel(city.FUpgradeLevel);
        Control.ProductionDone();
        Control.EndUpdate();
    }

    public override void Cancel(ProductionControl Control)
    {
        var city = Control.ProductionCity();
        if (city != null)
        {
            ChangeRelation(Control, city, Control.FSettings.FTurnsLeft == Control.FSettings.FTurnsTotal ? 15 : 10);
        }
    }

    public override bool Loopable(ProductionControl Control) => Control.ProductionCity() == null;

    public override bool LocksQueue(ProductionControl Control) => Control.ProductionCity() != null;

    public override int ActivityImage(City City) => 44;

    public override int? CitySequence(City City, int Row) => 39 + Row;

    public override string RealmActivity(City City) => "4";

    private static readonly string CityMigratedToRStr = "%0:s migrated to %1:s";
    internal static readonly string MigrateToXRStr = "Migrate to %s";
    private static readonly string MigratingToXRStr = "Migrating to %s";
}
