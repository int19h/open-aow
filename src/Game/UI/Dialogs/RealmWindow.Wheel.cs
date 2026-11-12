namespace Aow.Game.UI.Dialogs;

public partial class RealmWindow
{
    public override void Loaded()
    {
        base.Loaded();
        if (CityNameList != null) CityNameList.WheelScrollBar = CityScrollBar;
        if (CityRaceList != null) CityRaceList.WheelScrollBar = CityScrollBar;
        if (CityIncomeList != null) CityIncomeList.WheelScrollBar = CityScrollBar;
        if (CityLevelList != null) CityLevelList.WheelScrollBar = CityScrollBar;
        if (CityWallList != null) CityWallList.WheelScrollBar = CityScrollBar;
        if (CitySizeList != null) CitySizeList.WheelScrollBar = CityScrollBar;
        if (CityRelationList != null) CityRelationList.WheelScrollBar = CityScrollBar;
        if (CityMoraleList != null) CityMoraleList.WheelScrollBar = CityScrollBar;
        if (CityActionList != null) CityActionList.WheelScrollBar = CityScrollBar;
        if (Unit1List != null) Unit1List.WheelScrollBar = PartyScrollBar;
        if (Unit2List != null) Unit2List.WheelScrollBar = PartyScrollBar;
        if (Unit3List != null) Unit3List.WheelScrollBar = PartyScrollBar;
        if (Unit4List != null) Unit4List.WheelScrollBar = PartyScrollBar;
        if (Unit5List != null) Unit5List.WheelScrollBar = PartyScrollBar;
        if (Unit6List != null) Unit6List.WheelScrollBar = PartyScrollBar;
        if (Unit7List != null) Unit7List.WheelScrollBar = PartyScrollBar;
        if (PlaceNameList != null) PlaceNameList.WheelScrollBar = PlaceScrollBar;
        if (HeroHealthList != null) HeroHealthList.WheelScrollBar = HeroScrollBar;
        if (IncomeValueMemo != null) IncomeValueMemo.WheelScrollBar = IncomeScrollBar;
        if (UpkeepValueMemo != null) UpkeepValueMemo.WheelScrollBar = UpkeepScrollBar;
    }
}
