// Translated from DisplayC.pas.

namespace Aow.Game.Draw;

// the asynchronous frame loop. calls TCustomDisplay.UpdateFrame through Synchronize at the
// frame rate. _display, _busy, _lastTime
public partial class UpdateFrameThread
{
    internal CustomDisplay _display;
    internal bool _busy;
    internal int _lastTime;

    // DisplayC.pas:1069
    internal void TriggerUpdateFrame()
    {
        /* TODO: Implement Classes.Terminated in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1078
    protected internal void Execute()
    {
        /* TODO: Implement Classes.Terminated in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1105
    public UpdateFrameThread(CustomDisplay display)
    {
        /* TODO: Implement Classes.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:358

}
