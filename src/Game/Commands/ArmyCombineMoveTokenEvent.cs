// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

public partial class ArmyCombineMoveTokenEvent : MoveArmyTokenEvent
{
    public ArmyCombineMoveTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135427;

    // MoveT.PAS:2471
    public override bool CanMoveNext()
    {
        ArmyHexSprite armyHexSprite;
        Army sourceArmy;
        Army targetArmy;
        AddUnitsResult combineResult;
        if (MoveIndex == 1 && Path[0].X == TargetX && Path[0].Y == TargetY && Path[0].MapLevel == TargetMapLevel && ArmyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
        {
            armyHexSprite = GetMapField(Path[0]).GetArmyHexSprite();
            if (armyHexSprite != null && armyHexSprite.Army.Player == ArmyHexSprite.Army.Player)
            {
                sourceArmy = ArmyHexSprite.Army;
                targetArmy = armyHexSprite.Army;
                combineResult = targetArmy.CanAddUnitSelection(sourceArmy, 255);
                if (combineResult != AddUnitsResult.Ok)
                {
                    ShowCombineFailure(combineResult);
                    Mode = Mode - Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation);
                }
            }
        }

        return base.CanMoveNext();
    }

    // MoveT.PAS:2462
    public void ShowCombineFailure(AddUnitsResult reason)
    {
        switch (reason)
        {
            case AddUnitsResult.InvalidMoveType:
                var cannotCombineArmies = AbilityRules.TranslateResourceString(s_cannotCombineArmiesText);
                StrategicMap.Current.ShowMessage(cannotCombineArmies);
                break;
            case AddUnitsResult.TooManyUnits:
                var translatedText = AbilityRules.TranslateResourceString(s_cannotCombineArmiesText);
                StrategicMap.Current.ShowMessage(translatedText);
                break;
            case AddUnitsResult.MultipleTransporters:
                var cannotCombineTransporters = AbilityRules.TranslateResourceString(s_cannotCombineTransportersText);
                StrategicMap.Current.ShowMessage(cannotCombineTransporters);
                break;
        }
    }

    private static readonly string s_cannotCombineArmiesText = "Cannot combine armies";
    private static readonly string s_cannotCombineTransportersText = "Cannot combine two transporters";

    // intf_P5_07.inc:816

    // intf_P5_07.inc:816

}
