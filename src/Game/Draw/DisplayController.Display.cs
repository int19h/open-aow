#nullable disable
using Aow.Rtl;
using Aow.Rtl.Host;

namespace Aow.Game.Draw;

public partial class DisplayController
{
    private double _lastVerticalBlank;

    public virtual int WaitForVerticalBlank()
    {
        IClock clock = RtlUnit.Clock;
        clock.WaitUntil(_lastVerticalBlank + _display.FrameTime);
        _lastVerticalBlank = clock.ElapsedMilliseconds;
        return 0;
    }

    public virtual void ShowError(string message) => RtlUnit.ShowMessage(message);
}
