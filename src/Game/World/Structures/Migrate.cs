
using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// Production id 13: the city's people move out and Race moves in.
public sealed partial record Migrate(int Race) : ProductionOrder
{
    public const int Kind = 13;

    public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, Race, 0);

    public override CityAction CityAction => CityAction.Migrate;

    public override string Name(ProductionControl control) => control.ProductionCity() != null
        ? RtlUnit.Format(AbilityRules.TranslateResourceString(s_migrateFormat), [AbilityRules.TranslateResourceString(GameTables.s_raceNamesPluralTexts[Race])])
        : base.Name(control);

    public override string NameProgressive(ProductionControl control) => control.ProductionCity() != null
        ? RtlUnit.Format(AbilityRules.TranslateResourceString(s_migratingFormat), [AbilityRules.TranslateResourceString(GameTables.s_raceNamesPluralTexts[Race])])
        : base.NameProgressive(control);

    public override bool CanExecute(ProductionControl control, ref string reason)
    {
        var city = control.ProductionCity();
        return city != null ? city.CanMigrate((sbyte)Race, ref reason) : base.CanExecute(control, ref reason);
    }

    public override void Executed(ProductionControl control)
    {
        var city = control.ProductionCity();
        if (city == null)
        {
            return;
        }

        control.Settings.TurnsTotal = (sbyte)city.MigrationTime((sbyte)Race);
        control.Settings.TurnsLeft = control.Settings.TurnsTotal;
        control.Settings.Player = control.GetPlayer();
        control.Status = ProductionControlStatus.Producing;
        ChangeRelation(control, city, -15);
    }

    public override void Completed(ProductionControl control)
    {
        var city = control.ProductionCity();
        if (city == null)
        {
            return;
        }

        control.BeginUpdate();
        var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.s_raceNamesPluralTexts[Race]);
        Log(control, city, RtlUnit.Format(AbilityRules.TranslateResourceString(s_cityMigratedToFormat), [city.GetName(), raceNamesPlural]));
        city.SetRace((sbyte)Race);
        ChangeRelation(control, city, 10);
        control.UnitProductions.Clear();
        city.SetUpgradeLevel(city.UpgradeLevel);
        control.ProductionDone();
        control.EndUpdate();
    }

    public override void Cancel(ProductionControl control)
    {
        var city = control.ProductionCity();
        if (city != null)
        {
            ChangeRelation(control, city, control.Settings.TurnsLeft == control.Settings.TurnsTotal ? 15 : 10);
        }
    }

    public override bool Loopable(ProductionControl control) => control.ProductionCity() == null;

    public override bool LocksQueue(ProductionControl control) => control.ProductionCity() != null;

    public override int ActivityImage(City city) => 44;

    public override int? CitySequence(City city, int row) => 39 + row;

    public override string RealmActivity(City city) => "4";

    private static readonly string s_cityMigratedToFormat = "%0:s migrated to %1:s";
    internal static readonly string s_migrateFormat = "Migrate to %s";
    private static readonly string s_migratingFormat = "Migrating to %s";
}
