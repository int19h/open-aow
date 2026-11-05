using Aow.Rtl;
using Aow.Units.AoWCheckBox;

namespace Aow.Units.AoWCastUnitSpellDlg;

public partial class TCastUnitSpellDlg
{
    public void U1Change(TAOWCheckBox sender)
    {
        byte oldSelection = FCaster.FSelectionMask;
        int bit = 1 << sender.Tag;
        FCaster.SetSelection((byte)(sender.Checked
            ? FCaster.FAllowMultipleTargets ? FCaster.FSelectionMask | bit : bit
            : FCaster.FSelectionMask & ~bit));
        var checks = new[] { U1, U2, U3, U4, U5, U6, U7, U8 };
        for (int i = 0; i < checks.Length; i++) checks[i].Checked = (FCaster.FSelectionMask & (1 << i)) != 0;
        if (FCaster.FSelectionMask != oldSelection)
            AoWWinMan.AoWWinMan.General.PlayButtonSound((byte)(sender.Checked ? 1 : 2));
        if (FCaster.FSelectionMask == 0)
        {
            SelectionInfoLbl.Text = "";
            return;
        }
        AnsiString reason = default;
        SelectionInfoLbl.TextColor = FCaster.CanCast(ref reason) ? 0 : 149;
        // AoW.exe loads the two-byte separator
        SelectionInfoLbl.Text = AoWE.AoWE.TranslateRStr(AoWE.AoWE.RequiredManaRStr) +
            ": " + Rtl.Rtl.IntToStr(FCaster.GetRequiredMana());
    }
}
