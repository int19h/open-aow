// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// FDelay (Update ticks before it plays), FSampleData, FCallBack, FStatus. One playing sound
// effect, in SampleList
public partial class Sample : IDisposable
{
    public static SampleClass Descriptor { get; } = new("Sample", Create);

    internal int FDelay;
    internal SampleData FSampleData;
    internal SampleCallBack FCallBack;
    internal Set<StatusElement> FStatus;

    // Sound.pas:746
    protected internal virtual bool GetPlaying() => FDelay != 0;

    // Sound.pas:81
    protected internal virtual int GetVolume()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:82
    protected internal virtual void SetVolume(int Value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:83
    protected internal virtual bool GetLooped()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:84
    protected internal virtual void SetLooped(bool Value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:85
    protected internal virtual SamplePanning GetPanning()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:86
    protected internal virtual void SetPanning(SamplePanning Value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:761
    // { Called every tick (ProcessSound): a delayed sample counts down and plays, a finished one that
    //   does not loop destroys itself. }
    public virtual void Update()
    {
        if (FStatus.Contains(StatusElement.Running) && !GetPlaying())
        {
            if (FDelay != 0)
            {
                FDelay -= 1;
                if (FDelay == 0)
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
    protected Sample(ConstructionContext context, SampleData ASampleData)
    {
        context.Capture(this);
        AudioHost.SampleList.Add(this);
        FSampleData = ASampleData;
        FSampleData.AddRef();
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
        if (FCallBack != null)
        {
            FCallBack(this, SampleCallbackMode.Done);
        }

        if (FSampleData != null)
        {
            FSampleData.Release();
        }

        FSampleData = null;
        AudioHost.SampleList.Remove(this);
    }

    // Sound.pas:774
    public void Execute()
    {
        if (!FStatus.Contains(StatusElement.Running))
        {
            if (FDelay == 0)
            {
                Play();
            }

            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.Running);
        }
    }

    public SampleData SampleData { get => FSampleData; }
    public int Delay { get => FDelay; set => FDelay = value; }
    public SampleCallBack CallBack { get => FCallBack; set => FCallBack = value; }
    public Set<StatusElement> Status { get => FStatus; }
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
