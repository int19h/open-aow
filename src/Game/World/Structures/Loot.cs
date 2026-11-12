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

    public override string Name(ProductionControl control) =>
        control.ProductionCity() != null ? AbilityRules.TranslateResourceString(GameText.LootText) : base.Name(control);

    public override string NameProgressive(ProductionControl control) =>
        control.ProductionCity() != null ? AbilityRules.TranslateResourceString(s_lootingText) : base.NameProgressive(control);

    public override bool CanExecute(ProductionControl control, ref string reason)
    {
        var city = control.ProductionCity();
        return city != null ? city.CanLoot(ref reason) : base.CanExecute(control, ref reason);
    }

    public override void Executed(ProductionControl control)
    {
        var city = control.ProductionCity();
        if (city == null)
        {
            return;
        }

        control.Settings.TurnsTotal = (sbyte)city.LootTime();
        control.Settings.TurnsLeft = control.Settings.TurnsTotal;
        control.Settings.Player = control.GetPlayer();
        control.Settings.Gold = 0;
        ChangeRelation(control, city, -30);
        var log = new LocationTextEventLog(null);
        log._player = control.GetPlayer();
        log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(s_cityLootingFormat), [city.GetName()]);
        var hexLevel = city.GetHexLevel();
        var hexY = city.GetHexY();
        log.SetLocation(city.GetHexX(), hexY, hexLevel);
        var cityHexY = city.GetHexY();
        var cityHexLevel = city.GetHexLevel();
        var hexX = city.GetHexX();
        EventLog.DistributeLocationEventLog(log, control.GetPlayer(), hexX, cityHexY, cityHexLevel, false);
        log.Release();
        city._overlay.SetMode(CityOverlayHexSprite.OverlayMode.Looting);
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
        Log(control, city, RtlUnit.Format(AbilityRules.TranslateResourceString(s_cityLootedFormat), [city.GetName(), IntToStr(city.Income * 3)]));
        var playerGems = StrategicMap.Current.Players.GetPlayers(control.GetPlayer());
        StrategicMap.Current.Players.GetPlayers(control.GetPlayer()).SetGems(playerGems.Gems + city.Income * 3);
        city._overlay.SetMode(CityOverlayHexSprite.OverlayMode.Idle);
        city.SetRazed(true);
        control.EndUpdate();
    }

    public override void Cancel(ProductionControl control)
    {
        var city = control.ProductionCity();
        if (city != null)
        {
            ChangeRelation(control, city, control.Settings.TurnsLeft == control.Settings.TurnsTotal ? 30 : 20);
        }
    }

    public override bool Loopable(ProductionControl control) => control.ProductionCity() == null;

    public override bool LocksQueue(ProductionControl control) => control.ProductionCity() != null;

    public override int ActivityImage(City city) => 45;

    public override int? CitySequence(City city, int row) => 45 + row;

    public override string RealmActivity(City city) => "3";

    private static readonly string s_cityLootedFormat = "%0:s looted (%1:s gold gained)";
    private static readonly string s_cityLootingFormat = "%s is being looted";
    private static readonly string s_lootingText = "Looting";
}
