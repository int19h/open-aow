#nullable disable
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
public sealed partial class CityProductionControl(CustomNode owner) : ProductionControl(owner)
{
    public City City { get; init; }

    // City.pas:645
    public override MapCoordinate Location => new(
        City.HexX,
        City.HexY,
        City.HexLevel);

    // City.pas:707
    public override sbyte GetPlayer() => City.Player;

    // City.pas:675
    public override bool UnitProduced()
    {
        City city = City;
        bool valid = false;
        int count = city.TerrainCount;
        MapLevel level = city.Owner.Owner;
        sbyte hexY = city.HexY;
        sbyte hexX = city.HexX;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, radius: 0);
        while (walk.HexNumber < count && !valid)
        {
            if (city.Resource.TerrainList.Terrain[walk.HexNumber] >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                sbyte hexLevel = city.HexLevel;
                valid = Settings.Unit.Place(walk.X, walk.Y, L: hexLevel, city.Player, mode: Set<UnitPlaceModeElement>.Of()) is not null;
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        if (!valid)
        {
            sbyte hexLevel = city.HexLevel;
            sbyte cityHexY = city.HexY;
            sbyte cityHexX = city.HexX;
            valid = Settings.Unit.Place(cityHexX, cityHexY, L: hexLevel, city.Player, mode: Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea)) is not null;
        }

        CityProductionEventLog log = new(null)
        {
            Holder = CreationHolder(),
            Player = city.Player
        };
        string name = Settings.Unit.GetName();
        string fCityName = City.GetName();
        log.Text = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.ProducedFormat), args: [name, fCityName]);
        log.SetLocation(level: city.HexLevel, y: city.HexY, x: city.HexX);
        StrategicMap.Current.Players.GetPlayers(log.Player).EventLogbook.AddEvent(log);
        _ = log.Release();
        return valid;
    }

    // City.pas:658
    public override void UnitResearched()
    {
        base.UnitResearched();
        City city = City;
        CityProductionEventLog log = new(null)
        {
            Holder = CreationHolder(),
            Player = city.Player
        };
        UnitProduction unitProduction = UnitProductions.GetItem(Settings.UnitProductionIndex);
        string unitName = GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex).GetUnitName();
        string name = City.GetName();
        log.Text = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.InstalledFormat), args: [unitName, name]);
        log.SetLocation(level: city.HexLevel, y: city.HexY, x: city.HexX);
        StrategicMap.Current.Players.GetPlayers(log.Player).EventLogbook.AddEvent(log);
        _ = log.Release();
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
        if (result is not null && City.Enchantments.Contains(CityEnchantmentId.Warmonger))
        {
            result.SetRank(Unit.Rank.Silver);
        }

        return result;
    }

    // City.pas:710
    public override int UnitProductionTime(int index)
    {
        int unitIndex = UnitProductions.GetItem(index).UnitIndex;
        UnitResource unitResource = GamePack.Current.UnitResources.GetUnitResource(unitIndex);
        int size = City.Size;
        int result = unitResource.Level - (size / 2) + 1;
        return result < 1 ? 1 : result;
    }
}
