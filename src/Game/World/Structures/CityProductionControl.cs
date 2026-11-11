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
// FCity
public partial class CityProductionControl : ProductionControl
{
    public CityProductionControl(ECustomNode owner) : base(owner)
    { }

    public City FCity;

    // City.pas:645
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        X = FCity.GetXhx();
        Y = FCity.GetYhx();
        L = FCity.GetLhx();
    }

    // City.pas:707
    public override sbyte GetPlayer() => FCity.FPlayer;

    // City.pas:675
    public override bool UnitProduced()
    {
        City City;
        bool Valid;
        int Count;
        MapLevel Level;
        HexNumberWalkData Walk = default;
        CityProductionEventLog Log;
        City = FCity;
        Valid = false;
        Count = City.GetTerrainCount();
        Level = (MapLevel)City.Owner.Owner;
        var yhx = City.GetYhx();
        var xhx = City.GetXhx();
        HexNumberWalkData.InitHNWalk(ref Walk, xhx, yhx, 0);
        while (Walk.HN < Count && !Valid)
        {
            if (City.FResource.TerrainList.Terrain[Walk.HN] >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                var lhx = City.GetLhx();
                Valid = FSettings.FUnit.Place(Walk.X, Walk.Y, lhx, City.FPlayer, Set<UnitPlaceModeElement>.Of()) != null;
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        if (!Valid)
        {
            var lhx = City.GetLhx();
            var cityYhx = City.GetYhx();
            var cityXhx = City.GetXhx();
            Valid = FSettings.FUnit.Place(cityXhx, cityYhx, lhx, City.FPlayer, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea)) != null;
        }

        Log = new CityProductionEventLog(null);
        Log.FPlayer = City.FPlayer;
        var name = FSettings.FUnit.GetName();
        var fCityName = FCity.GetName();
        Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XProducedAtYRStr), [name, fCityName]);
        var cityLhx = City.GetLhx();
        var yhx2 = City.GetYhx();
        Log.SetLocation(City.GetXhx(), yhx2, cityLhx);
        AoWHSMap.Current.FPlayers.GetPlayers(Log.FPlayer).FEventLogbook.AddEvent(Log);
        Log.Release();
        return Valid;
    }

    // City.pas:658
    public override void UnitResearched()
    {
        City City;
        CityProductionEventLog Log;
        base.UnitResearched();
        City = FCity;
        Log = new CityProductionEventLog(null);
        Log.FPlayer = City.FPlayer;
        var unitProduction = FUnitProductions.GetItem(FSettings.FUnitProductionIndex);
        var unitName = AoWHSSet.Current.FUnitResources.GetUnitResource(unitProduction.FUnitIndex).GetUnitName();
        var name = FCity.GetName();
        Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XInstalledAtYRStr), [unitName, name]);
        var lhx = City.GetLhx();
        var yhx = City.GetYhx();
        Log.SetLocation(City.GetXhx(), yhx, lhx);
        AoWHSMap.Current.FPlayers.GetPlayers(Log.FPlayer).FEventLogbook.AddEvent(Log);
        Log.Release();
    }

    // City.pas:639
    public override void Update()
    {
        base.Update();
        if (FUpdateCount == 0)
        {
            FCity.Update();
        }
    }

    // City.pas:652
    public override Unit CreateUnitProduction(int Index)
    {
        Unit Result = base.CreateUnitProduction(Index);
        if (Result != null && FCity.FEnchantments.Contains(CityEnchantmentId.Warmonger))
        {
            Result.SetRank(Unit.Rank.Silver);
        }

        return Result;
    }

    // City.pas:710
    public override int UnitProductionTime(int Index)
    {
        int UnitIndex;
        UnitIndex = FUnitProductions.GetItem(Index).FUnitIndex;
        var unitResource = AoWHSSet.Current.FUnitResources.GetUnitResource(UnitIndex);
        var size = FCity.GetSize();
        int Result = unitResource.FLevel - size / 2 + 1;
        if (Result < 1)
        {
            Result = 1;
        }

        return Result;
    }

    // intf_P1_02.inc:120

}
