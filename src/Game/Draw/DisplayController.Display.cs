#nullable disable
using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class DisplayController
{
    private double _lastVerticalBlank;

    public virtual int WaitForVerticalBlank()
    {
        var clock = RtlUnit.Clock;
        double target = _lastVerticalBlank + _display.FrameTime;
        clock.WaitUntil(target);
        _lastVerticalBlank = clock.ElapsedMilliseconds;
        return 0;
    }

    public virtual void ShowError(string message) => RtlUnit.ShowMessage(message);
}
