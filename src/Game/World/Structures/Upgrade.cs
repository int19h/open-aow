#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// Production id 11: raise the city's upgrade level.
public sealed record Upgrade : ProductionOrder
{
    public const int Kind = 11;

    public override (int Kind, int Parameter, int GroupId) ToFields() => (Kind, 0, 0);

    public override CityAction CityAction => CityAction.Upgrade;

    public override string Name(ProductionControl control)
        => control.ProductionCity() is not null ? AbilityRules.TranslateResourceString(control.GetEngine(), S: GameText.UpgradeText) : base.Name(control);

    public override string NameProgressive(ProductionControl control)
        => control.ProductionCity() is not null ? AbilityRules.TranslateResourceString(control.GetEngine(), S: s_upgradingText) : base.NameProgressive(control);

    public override bool CanExecute(ProductionControl control, ref string reason)
    {
        City city = control.ProductionCity();
        if (city is null)
        {
            return base.CanExecute(control, ref reason);
        }

        if (control.GetEngine().StrategicMap.Players.GetPlayers(control.GetPlayer()).Gems >= city.UpgradeCost())
        {
            return true;
        }

        reason = AbilityRules.TranslateResourceString(control.GetEngine(), S: GameText.NotEnoughGoldText);
        return false;
    }

    public override void Executed(ProductionControl control)
    {
        City city = control.ProductionCity();
        if (city is null)
        {
            return;
        }

        control.Settings.TurnsTotal = city.UpgradeTime().TruncateToSByte();
        control.Settings.TurnsLeft = control.Settings.TurnsTotal;
        control.Settings.Player = control.GetPlayer();
        control.Settings.Gold = city.UpgradeCost();
        StrategicMap strategicMap = control.GetEngine().StrategicMap;
        Player player = strategicMap.Players.GetPlayers(control.GetPlayer());
        strategicMap.Players.GetPlayers(control.GetPlayer()).SetGems(player.Gems - control.Settings.Gold);
        control.Status = ProductionControlStatus.Producing;
    }

    public override void Completed(ProductionControl control)
    {
        City city = control.ProductionCity();
        if (city is null)
        {
            return;
        }

        control.BeginUpdate();
        city.SetUpgradeLevel((city.UpgradeLevel + 1).TruncateToByte());
        ChangeRelation(control, city, change: 5);
        control.ProductionDone();
        Log(control, city, text: DelphiFormat.Format(AbilityRules.TranslateResourceString(control.GetEngine(), S: s_upgradedFormat), args: [city.GetName()]));
        control.EndUpdate();
    }

    public override bool Loopable(ProductionControl control) => control.ProductionCity() is null;

    public override int ActivityImage(City city) => 43;

    public override int? CitySequence(City city, int row) => 33 + row;

    public override string RealmActivity(City city) => "2";

    private static readonly string s_upgradingText = "Upgrading";
    private static readonly string s_upgradedFormat = "%s upgraded";
}
