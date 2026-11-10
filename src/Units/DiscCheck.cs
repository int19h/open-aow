using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game;

internal static class DiscCheck
{
    internal static bool ValidAoWCD(byte Drive)
    {
        /* TODO: Implement Windows.GetDriveType in the platform or editor service. */
        throw new NotImplementedException();
    }

    internal static bool AoWCDInserted() => true;

    internal static bool GuaranteeAoWCD(string FirstMessage, string RetryMessage)
    {
        /* TODO: Implement Dialogs.MessageDlg in the platform or editor service. */
        throw new NotImplementedException();
    }

    internal static readonly string AgeOfWondersMapStr = "Age Of Wonders Map";

}
