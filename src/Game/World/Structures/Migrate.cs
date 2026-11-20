
using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// Production id 13: the city's people move out and Race moves in.
public sealed record Migrate(int Race) : ProductionOrder
{
    public const int Kind = 13;

    public override (int Kind, int Parameter, int GroupId) ToFields() => (Kind, Race, 0);

    public override CityAction CityAction => CityAction.Migrate;

    public override string Name(ProductionControl control) => control.ProductionCity() is not null
        ? DelphiFormat.Format(AbilityRules.TranslateResourceString(control.GetEngine(), S: MigrateFormat), args: [AbilityRules.TranslateResourceString(control.GetEngine(), S: GameTables.RaceNamesPluralTexts[Race])])
        : base.Name(control);

    public override string NameProgressive(ProductionControl control) => control.ProductionCity() is not null
        ? DelphiFormat.Format(AbilityRules.TranslateResourceString(control.GetEngine(), S: s_migratingFormat), args: [AbilityRules.TranslateResourceString(control.GetEngine(), S: GameTables.RaceNamesPluralTexts[Race])])
        : base.NameProgressive(control);

    public override bool CanExecute(ProductionControl control, ref string reason)
    {
        City city = control.ProductionCity();
        return city is not null ? city.CanMigrate((sbyte)Race, reason) : base.CanExecute(control, ref reason);
    }

    public override void Executed(ProductionControl control)
    {
        City city = control.ProductionCity();
        if (city is null)
        {
            return;
        }

        control.Settings.TurnsTotal = (sbyte)city.MigrationTime((sbyte)Race);
        control.Settings.TurnsLeft = control.Settings.TurnsTotal;
        control.Settings.Player = control.GetPlayer();
        control.Status = ProductionControlStatus.Producing;
        ChangeRelation(control, city, change: -15);
    }

    public override void Completed(ProductionControl control)
    {
        City city = control.ProductionCity();
        if (city is null)
        {
            return;
        }

        control.BeginUpdate();
        string raceNamesPlural = AbilityRules.TranslateResourceString(control.GetEngine(), S: GameTables.RaceNamesPluralTexts[Race]);
        Log(control, city, text: DelphiFormat.Format(AbilityRules.TranslateResourceString(control.GetEngine(), S: s_cityMigratedToFormat), args: [city.GetName(), raceNamesPlural]));
        city.SetRace((sbyte)Race);
        ChangeRelation(control, city, change: 10);
        control.UnitProductions.Clear();
        city.SetUpgradeLevel(city.UpgradeLevel);
        control.ProductionDone();
        control.EndUpdate();
    }

    public override void Cancel(ProductionControl control)
    {
        City city = control.ProductionCity();
        if (city is not null)
        {
            ChangeRelation(control, city, change: control.Settings.TurnsLeft == control.Settings.TurnsTotal ? 15 : 10);
        }
    }

    public override bool Loopable(ProductionControl control) => control.ProductionCity() is null;

    public override bool LocksQueue(ProductionControl control) => control.ProductionCity() is not null;

    public override int ActivityImage(City city) => 44;

    public override int? CitySequence(City city, int row) => 39 + row;

    public override string RealmActivity(City city) => "4";

    private static readonly string s_cityMigratedToFormat = "%0:s migrated to %1:s";

    internal static string MigrateFormat { get; } = "Migrate to %s";

    private static readonly string s_migratingFormat = "Migrating to %s";
}
