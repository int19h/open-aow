using Aow.Formats.Tree;
using Aow.Game.Commands;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net;

internal static class NetworkSession
{
    internal const int AllPlayers = 32768;

    internal const short CID_PassTokenTE = 1364;

    internal static int NetworkEngineDID;

}
