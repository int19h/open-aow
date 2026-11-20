// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

public class ArmyCombineMoveTokenEvent(CustomNode owner) : MoveArmyTokenEvent(owner)
{
    public new const int ClassId = 135427;

    // MoveT.PAS:2471
    public override bool CanMoveNext()
    {
        if (MoveIndex == 1 && Path[0].Location.X == Target.X && Path[0].Location.Y == Target.Y && Path[0].Location.MapLevel == Target.MapLevel && ArmyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
        {
            ArmyHexSprite armyHexSprite = GetMapField(Path[0].Location).GetArmyHexSprite();
            if (armyHexSprite is not null && armyHexSprite.Army.Player == ArmyHexSprite.Army.Player)
            {
                Army sourceArmy = ArmyHexSprite.Army;
                Army targetArmy = armyHexSprite.Army;
                AddUnitsResult combineResult = targetArmy.CanAddUnitSelection(sourceArmy.Units, 255);
                if (combineResult != AddUnitsResult.Ok)
                {
                    ShowCombineFailure(combineResult);
                    Mode -= Set<ArmyMoveModeElement>.Of(ArmyMoveModeElement.SelectEndLocation);
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
                string cannotCombineArmies = AbilityRules.TranslateResourceString(GetEngine(), s_cannotCombineArmiesText);
                StrategicMap.Current.ShowMessage(cannotCombineArmies);
                break;
            case AddUnitsResult.TooManyUnits:
                string translatedText = AbilityRules.TranslateResourceString(GetEngine(), s_cannotCombineArmiesText);
                StrategicMap.Current.ShowMessage(translatedText);
                break;
            case AddUnitsResult.MultipleTransporters:
                string cannotCombineTransporters = AbilityRules.TranslateResourceString(GetEngine(), s_cannotCombineTransportersText);
                StrategicMap.Current.ShowMessage(cannotCombineTransporters);
                break;
            case AddUnitsResult.Ok:
            default:
                break;
        }
    }

    private static readonly string s_cannotCombineArmiesText = "Cannot combine armies";
    private static readonly string s_cannotCombineTransportersText = "Cannot combine two transporters";
}
