
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// Production id 12: build the city's next wall.
public sealed partial record Fortify : ProductionOrder
{
    public const int Kind = 12;

    public override (int Kind, int Parameter, int GroupId) ToFields() => (Kind, 0, 0);

    public override CityAction CityAction => CityAction.Fortify;

    public override string Name(ProductionControl control) =>
        control.ProductionCity() != null ? AbilityRules.TranslateResourceString(control.GetEngine(), GameText.FortifyText) : base.Name(control);

    public override string NameProgressive(ProductionControl control) =>
        control.ProductionCity() != null ? AbilityRules.TranslateResourceString(control.GetEngine(), s_fortifyingText) : base.NameProgressive(control);

    public override bool CanExecute(ProductionControl control, ref string reason)
    {
        var city = control.ProductionCity();
        if (city == null)
        {
            return base.CanExecute(control, ref reason);
        }

        if (StrategicMap.Current.Players.GetPlayers(control.GetPlayer()).Gems >= city.FortifyCost())
        {
            return true;
        }

        reason = AbilityRules.TranslateResourceString(control.GetEngine(), GameText.NotEnoughGoldText);
        return false;
    }

    public override void Executed(ProductionControl control)
    {
        var city = control.ProductionCity();
        if (city == null)
        {
            return;
        }

        control.Settings.TurnsTotal = (sbyte)city.FortifyTime();
        control.Settings.TurnsLeft = control.Settings.TurnsTotal;
        control.Settings.Player = control.GetPlayer();
        control.Settings.Gold = city.FortifyCost();
        var playerGems = StrategicMap.Current.Players.GetPlayers(control.GetPlayer());
        StrategicMap.Current.Players.GetPlayers(control.GetPlayer()).SetGems(playerGems.Gems - control.Settings.Gold);
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
        switch (city.WallType)
        {
            case 0:
                city.WallType = 1;
                break;
            case 1:
                city.WallType = 2;
                break;
        }

        Log(control, city, RtlUnit.Format(AbilityRules.TranslateResourceString(control.GetEngine(), s_cityFortifiedFormat), [city.GetName()]));
        city.UpdateImages();
        ChangeRelation(control, city, 5);
        control.ProductionDone();
        control.EndUpdate();
    }

    public override bool Loopable(ProductionControl control) => control.ProductionCity() == null;

    public override int ActivityImage(City city) => city.WallType == 0 ? 41 : 42;

    public override int? CitySequence(City city, int row) => city.WallType switch
    {
        0 => 30 + row,
        1 => 48 + row,
        _ => -1,
    };

    public override string RealmActivity(City city) => city.WallType == 1 ? "1" : "0";

    private static readonly string s_cityFortifiedFormat = "%s fortified";
    private static readonly string s_fortifyingText = "Fortifying";
}
