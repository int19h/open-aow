// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

// { ot=1 }
// City
public partial class CityProductionControl : ProductionControl
{
    public CityProductionControl(ECustomNode owner) : base(owner)
    { }

    public City City { get; set; }

    // City.pas:645
    public override void GetLocation(out sbyte X, out sbyte Y, out sbyte L)
    {
        X = City.GetHexX();
        Y = City.GetHexY();
        L = City.GetHexLevel();
    }

    // City.pas:707
    public override sbyte GetPlayer() => City.Player;

    // City.pas:675
    public override bool UnitProduced()
    {
        City city;
        bool valid;
        int count;
        MapLevel level;
        HexNumberWalkData walk = default;
        CityProductionEventLog log;
        city = City;
        valid = false;
        count = city.GetTerrainCount();
        level = city.Owner.Owner;
        var hexY = city.GetHexY();
        var hexX = city.GetHexX();
        HexNumberWalkData.InitializeHexNumberWalk(out walk, hexX, hexY, 0);
        while (walk.HexNumber < count && !valid)
        {
            if (city._resource.TerrainList.Terrain[walk.HexNumber] >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                var hexLevel = city.GetHexLevel();
                valid = Settings.Unit.Place(walk.X, walk.Y, hexLevel, city.Player, Set<UnitPlaceModeElement>.Of()) != null;
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        if (!valid)
        {
            var hexLevel = city.GetHexLevel();
            var cityHexY = city.GetHexY();
            var cityHexX = city.GetHexX();
            valid = Settings.Unit.Place(cityHexX, cityHexY, hexLevel, city.Player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea)) != null;
        }

        log = new CityProductionEventLog(null);
        log._player = city.Player;
        var name = Settings.Unit.GetName();
        var fCityName = City.GetName();
        log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.ProducedFormat), [name, fCityName]);
        var cityHexLevel = city.GetHexLevel();
        var hexY2 = city.GetHexY();
        log.SetLocation(city.GetHexX(), hexY2, cityHexLevel);
        StrategicMap.Current.Players.GetPlayers(log._player).EventLogbook.AddEvent(log);
        log.Release();
        return valid;
    }

    // City.pas:658
    public override void UnitResearched()
    {
        City city;
        CityProductionEventLog log;
        base.UnitResearched();
        city = City;
        log = new CityProductionEventLog(null);
        log._player = city.Player;
        var unitProduction = UnitProductions.GetItem(Settings.UnitProductionIndex);
        var unitName = GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex).GetUnitName();
        var name = City.GetName();
        log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.InstalledFormat), [unitName, name]);
        var hexLevel = city.GetHexLevel();
        var hexY = city.GetHexY();
        log.SetLocation(city.GetHexX(), hexY, hexLevel);
        StrategicMap.Current.Players.GetPlayers(log._player).EventLogbook.AddEvent(log);
        log.Release();
    }

    // City.pas:639
    public override void Update()
    {
        base.Update();
        if (UpdateCount == 0)
        {
            City.Update();
        }
    }

    // City.pas:652
    public override Unit CreateUnitProduction(int index)
    {
        Unit result = base.CreateUnitProduction(index);
        if (result != null && City.Enchantments.Contains(CityEnchantmentId.Warmonger))
        {
            result.SetRank(Unit.Rank.Silver);
        }

        return result;
    }

    // City.pas:710
    public override int UnitProductionTime(int index)
    {
        int unitIndex;
        unitIndex = UnitProductions.GetItem(index).UnitIndex;
        var unitResource = GamePack.Current.UnitResources.GetUnitResource(unitIndex);
        var size = City.GetSize();
        int result = unitResource.Level - size / 2 + 1;
        if (result < 1)
        {
            result = 1;
        }

        return result;
    }

    // intf_P1_02.inc:120

}
