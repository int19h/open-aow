using Aow.Game.Draw;
using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Structures;

// Production id 14: loot the city and raze it.
public sealed partial record Loot : ProductionOrder
{
    public const int Kind = 14;

    public override (int Id, int Parameter, int GroupId) ToFields() => (Kind, 0, 0);

    public override CityAction CityAction => CityAction.Loot;

    public override string Name(ProductionControl Control) =>
        Control.ProductionCity() != null ? AbilityRules.TranslateRStr(GameText.LootRStr) : base.Name(Control);

    public override string NameProgressive(ProductionControl Control) =>
        Control.ProductionCity() != null ? AbilityRules.TranslateRStr(LootingRStr) : base.NameProgressive(Control);

    public override bool CanExecute(ProductionControl Control, ref string Reason)
    {
        var city = Control.ProductionCity();
        return city != null ? city.CanLoot(ref Reason) : base.CanExecute(Control, ref Reason);
    }

    public override void Executed(ProductionControl Control)
    {
        var city = Control.ProductionCity();
        if (city == null)
        {
            return;
        }

        Control.FSettings.FTurnsTotal = (sbyte)city.LootTime();
        Control.FSettings.FTurnsLeft = Control.FSettings.FTurnsTotal;
        Control.FSettings.FPlayer = Control.GetPlayer();
        Control.FSettings.FGold = 0;
        ChangeRelation(Control, city, -30);
        var Log = new LocationTextEventLog(null);
        Log.FPlayer = Control.GetPlayer();
        Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(CityLootingRStr), [city.GetName()]);
        var lhx = city.GetLhx();
        var yhx = city.GetYhx();
        Log.SetLocation(city.GetXhx(), yhx, lhx);
        var cityYhx = city.GetYhx();
        var cityLhx = city.GetLhx();
        var xhx = city.GetXhx();
        EventLog.DistributeLocationEventLog(Log, Control.GetPlayer(), xhx, cityYhx, cityLhx, false);
        Log.Release();
        city.FOverlay.SetMode(CityOverlayHexSprite.OverlayMode.Looting);
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
        Log(Control, city, RtlUnit.Format(AbilityRules.TranslateRStr(CityLootedRStr), [city.GetName(), IntToStr(city.FIncome * 3)]));
        var playerGems = AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer());
        AoWHSMap.Current.FPlayers.GetPlayers(Control.GetPlayer()).SetGems(playerGems.FGems + city.FIncome * 3);
        city.FOverlay.SetMode(CityOverlayHexSprite.OverlayMode.Idle);
        city.SetRazed(true);
        Control.EndUpdate();
    }

    public override void Cancel(ProductionControl Control)
    {
        var city = Control.ProductionCity();
        if (city != null)
        {
            ChangeRelation(Control, city, Control.FSettings.FTurnsLeft == Control.FSettings.FTurnsTotal ? 30 : 20);
        }
    }

    public override bool Loopable(ProductionControl Control) => Control.ProductionCity() == null;

    public override bool LocksQueue(ProductionControl Control) => Control.ProductionCity() != null;

    public override int ActivityImage(City City) => 45;

    public override int? CitySequence(City City, int Row) => 45 + Row;

    public override string RealmActivity(City City) => "3";

    private static readonly string CityLootedRStr = "%0:s looted (%1:s gold gained)";
    private static readonly string CityLootingRStr = "%s is being looted";
    private static readonly string LootingRStr = "Looting";
}
