#nullable disable
using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class DisplayController
{
    private double _lastVerticalBlank;

    public override int WaitForVerticalBlank()
    {
        var clock = RtlUnit.Clock;
        double target = _lastVerticalBlank + _display.FrameTime;
        clock.WaitUntil(target);
        _lastVerticalBlank = clock.ElapsedMilliseconds;
        return 0;
    }

    public override int Handle() => 0;
    public override WinControl OutputWindow() => _display;
    public override void ShowError(string message) => RtlUnit.ShowMessage(message);
}
