namespace Aow.Game.Recording;

/// <summary>
/// Runs work on a thread of its own while the calling thread waits (issue 343). The runtime takes
/// an object's first identity hash from a sequence of the thread that asks for it, and the game
/// reads identity hashes as addresses (<c>FrameFixtureInputs.AnimationOffset</c> and
/// <c>IdentityHash</c>). Reflection hashes objects of its own the first time it sees a class. On
/// this thread, the recorder's reflection and tree writing add no hash to the game thread's
/// sequence. They can still fill a cache the game thread later reads, a reflection cache or a
/// record catalog entry, and then the game thread skips a first-sight hash it would take without
/// the recorder. The scenarios whose random traces the oracle compares with the recorder on and off
/// show no such case (tools/oracle/README.md in the private repository). The game thread waits,
/// so the work reads the game state while nothing changes it. A failed work item returns its error
/// text, built here, and never throws on the game thread.
/// </summary>
internal sealed class IsolatedThread : IDisposable
{
    private readonly Thread _thread;
    private readonly SemaphoreSlim _started = new(initialCount: 0);
    private readonly SemaphoreSlim _finished = new(initialCount: 0);
    private Action? _work;
    private bool _isStopping;
    private bool _isDisposed;

    public IsolatedThread(string name)
    {
        _thread = new Thread(Loop) { IsBackground = true, Name = name };
        _thread.Start();
    }

    /// <summary>The work's result, or its error as the exception type and message.</summary>
    public (T? Result, string? Error) Run<T>(Func<T> work)
    {
        if (_isDisposed)
        {
            return (default, "the isolated thread is stopped");
        }

        T? result = default;
        string? error = null;
        _work = () =>
        {
            try
            {
                result = work();
            }
            catch (Exception exception)
            {
                error = $"{exception.GetType().Name}: {exception.Message}";
            }
        };
        _ = _started.Release();
        _finished.Wait();
        _work = null;
        return (result, error);
    }

    private void Loop()
    {
        while (true)
        {
            _started.Wait();
            if (_isStopping)
            {
                return;
            }

            _work!();
            _ = _finished.Release();
        }
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        _isStopping = true;
        _ = _started.Release();
        _thread.Join();
        _started.Dispose();
        _finished.Dispose();
    }
}
