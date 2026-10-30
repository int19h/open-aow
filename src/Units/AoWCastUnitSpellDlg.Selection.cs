using Aow.Rtl;

namespace Aow.Units.AoWCastUnitSpellDlg;

public partial class TCastUnitSpellDlg
{
    public void U1Change(Aow.Units.AoWCheckBox.TAOWCheckBox sender)
    {
        byte oldSelection = unchecked((byte)FCaster.FSelectionMask);
        int bit = 1 << sender.Tag;
        FCaster.SetSelection(unchecked((byte)(sender.Checked
            ? FCaster.FAllowMultipleTargets ? FCaster.FSelectionMask | bit : bit
            : FCaster.FSelectionMask & ~bit)));
        var checks = new[] { U1, U2, U3, U4, U5, U6, U7, U8 };
        for (int i = 0; i < checks.Length; i++) checks[i].Checked = (FCaster.FSelectionMask & (1 << i)) != 0;
        if (FCaster.FSelectionMask != oldSelection)
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound((byte)(sender.Checked ? 1 : 2));
        if (FCaster.FSelectionMask == 0)
        {
            SelectionInfoLbl.Text = "";
            return;
        }
        AnsiString reason = default;
        SelectionInfoLbl.TextColor = FCaster.CanCast(ref reason) ? 0 : 149;
        // AoW.exe loads the two-byte separator
        SelectionInfoLbl.Text = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RequiredManaRStr) +
            ": " + Aow.Rtl.Rtl.IntToStr(FCaster.GetRequiredMana());
    }
}
