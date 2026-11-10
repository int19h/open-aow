
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// Production id 12: build the city's next wall.
public sealed partial record Fortify : ProductionOrder
{
    public const int Kind = 12;

    public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, 0, 0);

    public override CityAction CityAction => CityAction.Fortify;

    public override string Name(ProductionControl Control) =>
        Control.ProductionCity() != null ? AbilityRules.TranslateRStr(GameText.FortifyRStr) : base.Name(Control);

    public override string NameProgressive(ProductionControl Control) =>
        Control.ProductionCity() != null ? AbilityRules.TranslateRStr(FortifyingRStr) : base.NameProgressive(Control);

    public override bool CanExecute(ProductionControl Control, ref string Reason)
    {
        var city = Control.ProductionCity();
        if (city == null)
        {
            return base.CanExecute(Control, ref Reason);
        }

        if (AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer()).FGems >= city.FortifyCost())
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

        Control.FSettings.FTurnsTotal = (sbyte)city.FortifyTime();
        Control.FSettings.FTurnsLeft = Control.FSettings.FTurnsTotal;
        Control.FSettings.FPlayer = Control.GetPlayer();
        Control.FSettings.FGold = city.FortifyCost();
        var playerGems = AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer());
        AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer()).SetGems(playerGems.FGems - Control.FSettings.FGold);
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
        switch (city.FWallType)
        {
            case 0:
                city.FWallType = 1;
                break;
            case 1:
                city.FWallType = 2;
                break;
        }

        Log(Control, city, RtlUnit.Format(AbilityRules.TranslateRStr(CityFortifiedRStr), [city.GetName()]));
        city.UpdateImages();
        ChangeRelation(Control, city, 5);
        Control.ProductionDone();
        Control.EndUpdate();
    }

    public override bool Loopable(ProductionControl Control) => Control.ProductionCity() == null;

    public override int ActivityImage(City City) => City.FWallType == 0 ? 41 : 42;

    public override int? CitySequence(City City, int Row) => City.FWallType switch
    {
        0 => 30 + Row,
        1 => 48 + Row,
        _ => -1,
    };

    public override string RealmActivity(City City) => City.FWallType == 1 ? "1" : "0";

    private static readonly string CityFortifiedRStr = "%s fortified";
    private static readonly string FortifyingRStr = "Fortifying";
}
