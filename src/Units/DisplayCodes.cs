using System.Runtime.InteropServices;
using Aow.Game.Engine.Buffers;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

internal static class DisplayCodes
{
    internal const int DCERR_NOTSUPPORTED = -2147414015;

    internal const int DCERR_FAILED = -2147414012;

    internal const int DCERR_NOTACTIVATED = -2147414010;

    internal const int DCERR_NOFLIP = -2147414007;

    internal const short WM_DISPLAYC = 27135;

}
