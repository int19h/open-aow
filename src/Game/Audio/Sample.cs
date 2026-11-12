// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// FDelay (Update ticks before it plays), FSampleData, FCallBack, FStatus. One playing sound
// effect, in SampleList
public partial class Sample : IDisposable
{
    public static SampleClass Descriptor { get; } = new("Sample", Create);

    private int _delay;
    private SampleData _sampleData;
    private SampleCallBack _callback;
    private Set<StatusElement> _status;

    // Sound.pas:746
    protected internal virtual bool GetPlaying() => _delay != 0;

    // Sound.pas:81
    protected internal virtual int GetVolume()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:82
    protected internal virtual void SetVolume(int value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:83
    protected internal virtual bool GetLooped()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:84
    protected internal virtual void SetLooped(bool value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:85
    protected internal virtual SamplePanning GetPanning()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:86
    protected internal virtual void SetPanning(SamplePanning value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:761
    // { Called every tick (ProcessSound): a delayed sample counts down and plays, a finished one that
    //   does not loop destroys itself. }
    public virtual void Update()
    {
        if (_status.Contains(StatusElement.Running) && !GetPlaying())
        {
            if (_delay != 0)
            {
                _delay -= 1;
                if (_delay == 0)
                {
                    Play();
                }
            }
            else
            {
                if (!GetLooped())
                {
                    Dispose();
                }
            }
        }
    }

    // Sound.pas:727
    protected Sample(ConstructionContext context, SampleData sampleData)
    {
        context.Capture(this);
        AudioHost.SampleList.Add(this);
        _sampleData = sampleData;
        _sampleData.AddRef();
    }

    // Sound.pas:751
    public virtual void Play()
    {
    }

    // Sound.pas:755
    public virtual void Stop()
    {
    }

    // Sound.pas:735
    public virtual void Dispose()
    {
        if (_callback != null)
        {
            _callback(this, SampleCallbackMode.Done);
        }

        if (_sampleData != null)
        {
            _sampleData.Release();
        }

        _sampleData = null;
        AudioHost.SampleList.Remove(this);
    }

    // Sound.pas:774
    public void Execute()
    {
        if (!_status.Contains(StatusElement.Running))
        {
            if (_delay == 0)
            {
                Play();
            }

            _status = _status + Set<StatusElement>.Of(StatusElement.Running);
        }
    }

    public SampleData SampleData { get => _sampleData; }
    public int Delay { get => _delay; set => _delay = value; }
    public SampleCallBack Callback { get => _callback; set => _callback = value; }
    public Set<StatusElement> Status { get => _status; }
    public bool Playing { get => GetPlaying(); }
    public int Volume { get => GetVolume(); set => SetVolume(value); }
    public bool Looped { get => GetLooped(); set => SetLooped(value); }
    public SamplePanning Panning { get => GetPanning(); set => SetPanning(value); }

    // Sound.pas:89
    public static Sample Create(SampleData data) =>
        ConstructionContext.Create(data, static (context, value) => new Sample(context, value));

    public enum StatusElement : byte
    {
        Running = 0
    }

}
