namespace Aow.Game.Ui.Dialogs;

public partial class RealmWin
{
    public override void Loaded()
    {
        base.Loaded();
        if (T2NameLB != null) T2NameLB.WheelScrollBar = CitySB;
        if (T2RaceLB != null) T2RaceLB.WheelScrollBar = CitySB;
        if (T2IncomeLB != null) T2IncomeLB.WheelScrollBar = CitySB;
        if (T2LevelLB != null) T2LevelLB.WheelScrollBar = CitySB;
        if (T2WallLB != null) T2WallLB.WheelScrollBar = CitySB;
        if (T2SizeLB != null) T2SizeLB.WheelScrollBar = CitySB;
        if (T2RelLB != null) T2RelLB.WheelScrollBar = CitySB;
        if (T2MorLB != null) T2MorLB.WheelScrollBar = CitySB;
        if (T2ActLB != null) T2ActLB.WheelScrollBar = CitySB;
        if (Unit1LB != null) Unit1LB.WheelScrollBar = PartySb;
        if (Unit2LB != null) Unit2LB.WheelScrollBar = PartySb;
        if (Unit3LB != null) Unit3LB.WheelScrollBar = PartySb;
        if (Unit4LB != null) Unit4LB.WheelScrollBar = PartySb;
        if (Unit5LB != null) Unit5LB.WheelScrollBar = PartySb;
        if (Unit6LB != null) Unit6LB.WheelScrollBar = PartySb;
        if (Unit7LB != null) Unit7LB.WheelScrollBar = PartySb;
        if (T3Name != null) T3Name.WheelScrollBar = PlaceSB;
        if (T5Health != null) T5Health.WheelScrollBar = HeroSB;
        if (IncomeValue != null) IncomeValue.WheelScrollBar = IncomeSB;
        if (UpkeepValue != null) UpkeepValue.WheelScrollBar = UpkeepSB;
    }
}
