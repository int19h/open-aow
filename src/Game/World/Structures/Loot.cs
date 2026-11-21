#nullable disable warnings
using System.Globalization;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiFormat;

namespace Aow.Game.World.Structures;

// Production id 14: loot the city and raze it.
public sealed record Loot : ProductionOrder
{
    public const int Kind = 14;

    public override (int Kind, int Parameter, int GroupId) ToFields() => (Kind, 0, 0);

    public override CityAction CityAction => CityAction.Loot;

    public override string Name(ProductionControl control)
        => control.ProductionCity() is not null ? AbilityRules.TranslateResourceString(control.GetEngine(), S: GameText.LootText) : base.Name(control);

    public override string NameProgressive(ProductionControl control)
        => control.ProductionCity() is not null ? AbilityRules.TranslateResourceString(control.GetEngine(), S: s_lootingText) : base.NameProgressive(control);

    public override bool CanExecute(ProductionControl control, ref string reason)
    {
        City? city = control.ProductionCity();
        return city is not null ? city.CanLoot(reason) : base.CanExecute(control, ref reason);
    }

    public override void Executed(ProductionControl control)
    {
        City? city = control.ProductionCity();
        if (city is null)
        {
            return;
        }

        control.Settings.TurnsTotal = (sbyte)city.LootTime();
        control.Settings.TurnsLeft = control.Settings.TurnsTotal;
        control.Settings.Player = control.GetPlayer();
        control.Settings.Gold = 0;
        ChangeRelation(control, city, change: -30);
        LocationTextEventLog log = new(null)
        {
            Holder = control,
            Player = control.GetPlayer(),
            Text = Format(AbilityRules.TranslateResourceString(control.GetEngine(), S: s_cityLootingFormat), args: [city.GetName()])
        };
        log.SetLocation(level: city.HexLevel, y: city.HexY, x: city.HexX);
        EventLog.DistributeLocationEventLog(y: city.HexY, level: city.HexLevel, x: city.HexX, log: log, player: control.GetPlayer(), execute: false);
        _ = log.Release();
        city.Overlay.SetMode(CityOverlayHexSprite.OverlayMode.Looting);
        control.Status = ProductionControlStatus.Producing;
    }

    public override void Completed(ProductionControl control)
    {
        City? city = control.ProductionCity();
        if (city is null)
        {
            return;
        }

        control.BeginUpdate();
        Log(control, city, text: Format(AbilityRules.TranslateResourceString(control.GetEngine(), S: s_cityLootedFormat), args: [city.GetName(), (city.Income * 3).ToString(CultureInfo.InvariantCulture)]));
        Player? playerGems = StrategicMap.Current.Players.GetPlayers(control.GetPlayer());
        StrategicMap.Current.Players.GetPlayers(control.GetPlayer()).SetGems(playerGems.Gems + (city.Income * 3));
        city.Overlay.SetMode(CityOverlayHexSprite.OverlayMode.Idle);
        city.SetRazed(true);
        control.EndUpdate();
    }

    public override void Cancel(ProductionControl control)
    {
        City? city = control.ProductionCity();
        if (city is not null)
        {
            ChangeRelation(control, city, change: control.Settings.TurnsLeft == control.Settings.TurnsTotal ? 30 : 20);
        }
    }

    public override bool Loopable(ProductionControl control) => control.ProductionCity() is null;

    public override bool LocksQueue(ProductionControl control) => control.ProductionCity() is not null;

    public override int ActivityImage(City city) => 45;

    public override int? CitySequence(City city, int row) => 45 + row;

    public override string RealmActivity(City city) => "3";

    private static readonly string s_cityLootedFormat = "%0:s looted (%1:s gold gained)";
    private static readonly string s_cityLootingFormat = "%s is being looted";
    private static readonly string s_lootingText = "Looting";
}
