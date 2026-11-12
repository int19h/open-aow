
using Aow.Engine.Buffers;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

internal static class MovementTables
{
    internal static MoveControl_SetupMove__TOriginalAssertText TMoveControl_SetupMove__OriginalAssertFile { get; } = new MoveControl_SetupMove__TOriginalAssertText
    {
        RefCount = -1,
        Len = 28,
        Text = ByteArray128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 77, 111, 118, 101, 84, 46, 80, 65, 83, 0])
    };

    internal static MoveControl_SetupMove__TOriginalAssertText TMoveControl_SetupMove__OriginalAssertMessage { get; } = new MoveControl_SetupMove__TOriginalAssertText
    {
        RefCount = -1,
        Len = 31,
        Text = ByteArray128.From([73, 110, 118, 97, 108, 105, 100, 32, 115, 101, 116, 116, 105, 110, 103, 115, 32, 40, 84, 77, 111, 118, 101, 67, 111, 110, 116, 114, 111, 108, 41, 0])
    };

    internal static MoveControl_CalculatePathEx__TOriginalAssertText TMoveControl_CalculatePathEx__OriginalAssertFile { get; } = new MoveControl_CalculatePathEx__TOriginalAssertText
    {
        RefCount = -1,
        Len = 28,
        Text = ByteArray128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 77, 111, 118, 101, 84, 46, 80, 65, 83, 0])
    };

    internal static MoveControl_CalculatePathEx__TOriginalAssertText TMoveControl_CalculatePathEx__OriginalAssertMessage { get; } = new MoveControl_CalculatePathEx__TOriginalAssertText
    {
        RefCount = -1,
        Len = 17,
        Text = ByteArray128.From([65, 115, 115, 101, 114, 116, 105, 111, 110, 32, 102, 97, 105, 108, 117, 114, 101, 0])
    };

    internal static MoveControl_Activate__TOriginalAssertText TMoveControl_Activate__OriginalAssertFile { get; } = new MoveControl_Activate__TOriginalAssertText
    {
        RefCount = -1,
        Len = 28,
        Text = ByteArray128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 77, 111, 118, 101, 84, 46, 80, 65, 83, 0])
    };

    internal static MoveControl_Activate__TOriginalAssertText TMoveControl_Activate__OriginalAssertMessage { get; } = new MoveControl_Activate__TOriginalAssertText
    {
        RefCount = -1,
        Len = 17,
        Text = ByteArray128.From([65, 115, 115, 101, 114, 116, 105, 111, 110, 32, 102, 97, 105, 108, 117, 114, 101, 0])
    };

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
        AoWHSMap.Current.MoveControl.ProcessMove(settings);
        if (settings.Path.Count != 0)
        {
            if ((force || AoWHSMap.Current.TokenManager.PlayerReady(player)) && settings.Path.Count > 1)
            {
                result = ArmyHexSprite.CreateArmyMoveTokenEvent(army, selection, settings.Path, action, Set<ArmyMoveModeElement>.Of());
                if (result != null)
                {
                    AoWHSMap.Current.TokenManager.GetTokenControl(player).AddEvent(result);
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

    internal static SByteArray16By16By256 MovePointTables;

    internal static int MoveTypesToInt32(Set<MoveType> types) => (byte)types.Image;

}
