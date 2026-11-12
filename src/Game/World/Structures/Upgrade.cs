
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// Production id 11: raise the city's upgrade level.
public sealed partial record Upgrade : ProductionOrder
{
    public const int Kind = 11;

    public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, 0, 0);

    public override CityAction CityAction => CityAction.Upgrade;

    public override string Name(ProductionControl control) =>
        control.ProductionCity() != null ? AbilityRules.TranslateResourceString(GameText.UpgradeText) : base.Name(control);

    public override string NameProgressive(ProductionControl control) =>
        control.ProductionCity() != null ? AbilityRules.TranslateResourceString(s_upgradingText) : base.NameProgressive(control);

    public override bool CanExecute(ProductionControl control, ref string reason)
    {
        var city = control.ProductionCity();
        if (city == null)
        {
            return base.CanExecute(control, ref reason);
        }

        if (AoWHSMap.Current.Players.GetPlayers(control.GetPlayer()).Gems >= city.UpgradeCost())
        {
            return true;
        }

        reason = AbilityRules.TranslateResourceString(GameText.NotEnoughGoldText);
        return false;
    }

    public override void Executed(ProductionControl control)
    {
        var city = control.ProductionCity();
        if (city == null)
        {
            return;
        }

        control.Settings.TurnsTotal = (sbyte)city.UpgradeTime();
        control.Settings.TurnsLeft = control.Settings.TurnsTotal;
        control.Settings.Player = control.GetPlayer();
        control.Settings.Gold = city.UpgradeCost();
        var player = AoWHSMap.Current.Players.GetPlayers(control.GetPlayer());
        AoWHSMap.Current.Players.GetPlayers(control.GetPlayer()).SetGems(player.Gems - control.Settings.Gold);
        control.Status = ProductionControlStatus.Producing;
    }

    public override void Completed(ProductionControl control)
    {
        var city = control.ProductionCity();
        if (city == null)
        {
            return;
        }

        control.BeginUpdate();
        city.SetUpgradeLevel((byte)(city.UpgradeLevel + 1));
        ChangeRelation(control, city, 5);
        control.ProductionDone();
        Log(control, city, RtlUnit.Format(AbilityRules.TranslateResourceString(s_upgradedFormat), [city.GetName()]));
        control.EndUpdate();
    }

    public override bool Loopable(ProductionControl control) => control.ProductionCity() == null;

    public override int ActivityImage(City city) => 43;

    public override int? CitySequence(City city, int row) => 33 + row;

    public override string RealmActivity(City city) => "2";

    private static readonly string s_upgradingText = "Upgrading";
    private static readonly string s_upgradedFormat = "%s upgraded";
}
