using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Saves;
using Aow.Game.World;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

internal static class HexText
{
    internal static readonly string BridgeStr = "Bridge";

    internal static readonly string InvalidPlaceMentStr = "Invalid Placement";

    internal static readonly string SelectedStr = "selected";

    internal static readonly string TransparentStr = "Transparent";

    internal static readonly string LoadingStr = "Loading";

    internal static readonly string ClosingStr = "Closing";

    internal static readonly string SaveChangesToXStr = "Save changes to %s";

    internal static readonly string CutStr = "Cu&t";

    internal static readonly string PasteStr = "&Paste";

    internal static readonly string DeleteStr = "&Delete";

    internal static readonly string CopyStr = "&Copy";

    internal static readonly string EditStr = "&Edit";

}
