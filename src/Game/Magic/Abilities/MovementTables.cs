
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

internal static class MovementTables
{

    internal static MoveArmyTokenEvent MoveArmyMapCoordinate(sbyte player, ArmyHexSprite army, byte selection, sbyte X, sbyte Y, sbyte L, MoveAction action, bool force)
    {
        MoveArmyTokenEvent result;
        MoveControlSettings settings;
        settings = new MoveControlSettings(null);
        var hexLevel = army.GetHexLevel();
        var hexY = army.GetHexY();
        settings.SetSourcePosition(army.GetHexX(), hexY, hexLevel);
        settings.Player = army.Army.Player;
        settings.ArmyHexSprite = army;
        settings.Selection = selection;
        settings.SetDestinationPosition(X, Y, L);
        army.Army.CreateMovePointTable(selection, Bytes.Of(ref settings.MovePointTable));
        StrategicMap.Current.MoveControl.ProcessMove(settings);
        if (settings.Path.Count != 0)
        {
            if ((force || StrategicMap.Current.TokenManager.PlayerReady(player)) && settings.Path.Count > 1)
            {
                result = ArmyHexSprite.CreateArmyMoveTokenEvent(army, selection, settings.Path, action, Set<ArmyMoveModeElement>.Of());
                if (result != null)
                {
                    StrategicMap.Current.TokenManager.GetTokenControl(player).AddEvent(result);
                    // { Original returns the token-event pointer after releasing its reference. }
                    result.Release();
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                result = null;
            }
        }
        else
        {
            result = null;
        }

        settings.Release();
        return result;
    }

    // The overlay index of a move point table starts at -1, no overlay.
    internal const int FirstOverlay = -1;

    internal static MovePointTable[] MovePointTables { get; } = new MovePointTable[256];

    internal static int MoveTypesToInt32(Set<MoveType> types) => (byte)types.Image;

}
