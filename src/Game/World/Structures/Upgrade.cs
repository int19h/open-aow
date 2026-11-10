
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

    public override string Name(ProductionControl Control) =>
        Control.ProductionCity() != null ? AbilityRules.TranslateRStr(GameText.UpgradeRStr) : base.Name(Control);

    public override string NameProgressive(ProductionControl Control) =>
        Control.ProductionCity() != null ? AbilityRules.TranslateRStr(UpgradingRStr) : base.NameProgressive(Control);

    public override bool CanExecute(ProductionControl Control, ref string Reason)
    {
        var city = Control.ProductionCity();
        if (city == null)
        {
            return base.CanExecute(Control, ref Reason);
        }

        if (AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer()).FGems >= city.UpgradeCost())
        {
            return true;
        }

        Reason = AbilityRules.TranslateRStr(GameText.NotEnoughGoldRStr);
        return false;
    }

    public override void Executed(ProductionControl Control)
    {
        var city = Control.ProductionCity();
        if (city == null)
        {
            return;
        }

        Control.FSettings.FTurnsTotal = (sbyte)city.UpgradeTime();
        Control.FSettings.FTurnsLeft = Control.FSettings.FTurnsTotal;
        Control.FSettings.FPlayer = Control.GetPlayer();
        Control.FSettings.FGold = city.UpgradeCost();
        var player = AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer());
        AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer()).SetGems(player.FGems - Control.FSettings.FGold);
        Control.FStatus = ProductionControl.Status.Producing;
    }

    public override void Completed(ProductionControl Control)
    {
        var city = Control.ProductionCity();
        if (city == null)
        {
            return;
        }

        Control.BeginUpdate();
        city.SetUpgradeLevel((byte)(city.FUpgradeLevel + 1));
        ChangeRelation(Control, city, 5);
        Control.ProductionDone();
        Log(Control, city, RtlUnit.Format(AbilityRules.TranslateRStr(XUpgradedRStr), [city.GetName()]));
        Control.EndUpdate();
    }

    public override bool Loopable(ProductionControl Control) => Control.ProductionCity() == null;

    public override int ActivityImage(City City) => 43;

    public override int? CitySequence(City City, int Row) => 33 + Row;

    public override string RealmActivity(City City) => "2";

    private static readonly string UpgradingRStr = "Upgrading";
    private static readonly string XUpgradedRStr = "%s upgraded";
}
