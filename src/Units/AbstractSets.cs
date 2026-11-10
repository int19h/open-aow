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

internal static class AbstractSets
{
    internal static TAbstractAoWHSMap_UnFogExploreArea__TAssertText TAbstractAoWHSMap_UnFogExploreArea__AssertFile = new TAbstractAoWHSMap_UnFogExploreArea__TAssertText
    {
        RefCount = -1,
        Len = 31,
        Text = FixedArray_byte_128.From([68, 58, 92, 65, 111, 87, 68, 101, 118, 92, 65, 111, 87, 69, 92, 105, 110, 99, 92, 65, 111, 87, 72, 83, 77, 97, 112, 46, 80, 65, 83, 0])
    };

    internal static TAbstractAoWHSMap_UnFogExploreArea__TAssertText TAbstractAoWHSMap_UnFogExploreArea__AssertMessage = new TAbstractAoWHSMap_UnFogExploreArea__TAssertText
    {
        RefCount = -1,
        Len = 19,
        Text = FixedArray_byte_128.From([73, 110, 118, 97, 108, 105, 100, 32, 85, 110, 102, 111, 103, 32, 82, 97, 110, 103, 101, 0])
    };

    internal static TSet<TAbilitySelectionType> TAbstractUnit_GetAbilitySelectionTypes__SelectionTypes = TSet<TAbilitySelectionType>.FromMask(1UL);

    internal static TSet<TDamageType> TAbstractUnit_GetStrengthMax__AllDamageTypes = TSet<TDamageType>.FromMask(1023UL);

}
