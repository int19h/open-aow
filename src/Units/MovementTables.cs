using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

internal static class MovementTables
{
    internal static TMoveControl_SetupMove__TOriginalAssertText TMoveControl_SetupMove__OriginalAssertFile = new TMoveControl_SetupMove__TOriginalAssertText
    {
        RefCount = -1,
        Len = 28,
        Text = FixedArray_byte_128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 77, 111, 118, 101, 84, 46, 80, 65, 83, 0])
    };

    internal static TMoveControl_SetupMove__TOriginalAssertText TMoveControl_SetupMove__OriginalAssertMessage = new TMoveControl_SetupMove__TOriginalAssertText
    {
        RefCount = -1,
        Len = 31,
        Text = FixedArray_byte_128.From([73, 110, 118, 97, 108, 105, 100, 32, 115, 101, 116, 116, 105, 110, 103, 115, 32, 40, 84, 77, 111, 118, 101, 67, 111, 110, 116, 114, 111, 108, 41, 0])
    };

    internal static TMoveControl_CalculatePathEx__TOriginalAssertText TMoveControl_CalculatePathEx__OriginalAssertFile = new TMoveControl_CalculatePathEx__TOriginalAssertText
    {
        RefCount = -1,
        Len = 28,
        Text = FixedArray_byte_128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 77, 111, 118, 101, 84, 46, 80, 65, 83, 0])
    };

    internal static TMoveControl_CalculatePathEx__TOriginalAssertText TMoveControl_CalculatePathEx__OriginalAssertMessage = new TMoveControl_CalculatePathEx__TOriginalAssertText
    {
        RefCount = -1,
        Len = 17,
        Text = FixedArray_byte_128.From([65, 115, 115, 101, 114, 116, 105, 111, 110, 32, 102, 97, 105, 108, 117, 114, 101, 0])
    };

    internal static TMoveControl_Activate__TOriginalAssertText TMoveControl_Activate__OriginalAssertFile = new TMoveControl_Activate__TOriginalAssertText
    {
        RefCount = -1,
        Len = 28,
        Text = FixedArray_byte_128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 77, 111, 118, 101, 84, 46, 80, 65, 83, 0])
    };

    internal static TMoveControl_Activate__TOriginalAssertText TMoveControl_Activate__OriginalAssertMessage = new TMoveControl_Activate__TOriginalAssertText
    {
        RefCount = -1,
        Len = 17,
        Text = FixedArray_byte_128.From([65, 115, 115, 101, 114, 116, 105, 111, 110, 32, 102, 97, 105, 108, 117, 114, 101, 0])
    };

    internal static TMoveArmyTE MoveArmyXYLEx(sbyte Player, TArmyHS Army, byte Selection, sbyte X, sbyte Y, sbyte L, TMoveAction Action, bool Force)
    {
        TMoveArmyTE Result;
        TMoveControlSettings Settings;
        Settings = new TMoveControlSettings(null);
        var lhx = Army.GetLhx();
        var yhx = Army.GetYhx();
        Settings.SetSourcePos(Army.GetXhx(), yhx, lhx);
        Settings.FPlayer = Army.FArmy.FPlayer;
        Settings.FArmyHS = Army;
        Settings.FSelection = Selection;
        Settings.SetDestinationPos(X, Y, L);
        Army.FArmy.CreateMovePointTable(Selection, Bytes.Of(ref Settings.FMovePointTable));
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FMoveControl.ProcessMove(Settings);
        if (Settings.FPath.Count != 0)
        {
            if ((Force || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.PlayerReady(Player)) && Settings.FPath.Count > 1)
            {
                Result = Aow.Game.Draw.TArmyHS.CreateArmyMoveTE(Army, Selection, Settings.FPath, Action, TSet<TArmyMoveModeElement>.Of());
                if (Result != null)
                {
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(Player).AddEvent(Result);
                    // { Original returns the token-event pointer after releasing its reference. }
                    Result.Release();
                }
                else
                {
                    Result = null;
                }
            }
            else
            {
                Result = null;
            }
        }
        else
        {
            Result = null;
        }

        Settings.Release();
        return Result;
    }

    internal static FixedArray_Aow_Units_FixedArray_Aow_Units_FixedArray_sbyte_16_16_256 MovePointTables;

    internal static int MoveTypesToInt(TSet<TMoveType> types) => (byte)types.Image;

}
