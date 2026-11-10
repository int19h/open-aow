#nullable disable
using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class DisplayCtrl
{
    private double lastVerticalBlank;

    public override int WaitForVerticalBlank()
    {
        var clock = RtlUnit.Clock;
        double target = lastVerticalBlank + FDisplay.FFrameTime;
        clock.WaitUntil(target);
        lastVerticalBlank = clock.ElapsedMilliseconds;
        FLastVBTime = (int)lastVerticalBlank;
        return 0;
    }

    public override int Handle() => 0;
    public override WinControl OutputWnd() => FDisplay;
    public override void ShowError(string message) => RtlUnit.ShowMessage(message);
}
