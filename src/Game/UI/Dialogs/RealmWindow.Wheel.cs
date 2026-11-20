namespace Aow.Game.UI.Dialogs;

public sealed partial class RealmWindow
{
    public override void Loaded()
    {
        base.Loaded();
        CityNameList?.WheelScrollBar = CityScrollBar;

        CityRaceList?.WheelScrollBar = CityScrollBar;

        CityIncomeList?.WheelScrollBar = CityScrollBar;

        CityLevelList?.WheelScrollBar = CityScrollBar;

        CityWallList?.WheelScrollBar = CityScrollBar;

        CitySizeList?.WheelScrollBar = CityScrollBar;

        CityRelationList?.WheelScrollBar = CityScrollBar;

        CityMoraleList?.WheelScrollBar = CityScrollBar;

        CityActionList?.WheelScrollBar = CityScrollBar;

        Unit1List?.WheelScrollBar = PartyScrollBar;

        Unit2List?.WheelScrollBar = PartyScrollBar;

        Unit3List?.WheelScrollBar = PartyScrollBar;

        Unit4List?.WheelScrollBar = PartyScrollBar;

        Unit5List?.WheelScrollBar = PartyScrollBar;

        Unit6List?.WheelScrollBar = PartyScrollBar;

        Unit7List?.WheelScrollBar = PartyScrollBar;

        PlaceNameList?.WheelScrollBar = PlaceScrollBar;

        HeroHealthList?.WheelScrollBar = HeroScrollBar;

        IncomeValueMemo?.WheelScrollBar = IncomeScrollBar;

        UpkeepValueMemo?.WheelScrollBar = UpkeepScrollBar;
    }
}
