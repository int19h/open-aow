// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

public partial class ArmyCombineMoveTE : MoveArmyTE
{
    public ArmyCombineMoveTE(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135427;

    // MoveT.PAS:2471
    public override bool CanMoveNext()
    {
        ArmyHS ArmyHS;
        Army SourceArmy;
        Army TargetArmy;
        AddUnitsResult CombineResult;
        if (FMoveIndex == 1 && FPath[0].X == FTargetX && FPath[0].Y == FTargetY && FPath[0].L == FTargetL && FArmyHS.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer)
        {
            ArmyHS = GetMapF(FPath[0]).GetArmyHS();
            if (ArmyHS != null && ArmyHS.FArmy.FPlayer == FArmyHS.FArmy.FPlayer)
            {
                SourceArmy = FArmyHS.FArmy;
                TargetArmy = ArmyHS.FArmy;
                CombineResult = TargetArmy.CanAddUnitSelection(SourceArmy, 255);
                if (CombineResult != AddUnitsResult.OK)
                {
                    ShowCombineFailure(CombineResult);
                    FMode = FMode - Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation);
                }
            }
        }

        return base.CanMoveNext();
    }

    // MoveT.PAS:2462
    public void ShowCombineFailure(AddUnitsResult Reason)
    {
        switch (Reason)
        {
            case AddUnitsResult.InvalidMoveType:
                var p5CannotCombineArmies = AbilityRules.TranslateRStr(P5CannotCombineArmiesRStr);
                AoWHSMap.Current.ShowMessage(p5CannotCombineArmies);
                break;
            case AddUnitsResult.TooManyUnits:
                var aoWETranslateRStr = AbilityRules.TranslateRStr(P5CannotCombineArmiesRStr);
                AoWHSMap.Current.ShowMessage(aoWETranslateRStr);
                break;
            case AddUnitsResult.MultipleTransporters:
                var p5CannotCombineTransporters = AbilityRules.TranslateRStr(P5CannotCombineTransportersRStr);
                AoWHSMap.Current.ShowMessage(p5CannotCombineTransporters);
                break;
        }
    }

    private static readonly string P5CannotCombineArmiesRStr = "Cannot combine armies";
    private static readonly string P5CannotCombineTransportersRStr = "Cannot combine two transporters";

    // intf_P5_07.inc:816

    // intf_P5_07.inc:816

}
