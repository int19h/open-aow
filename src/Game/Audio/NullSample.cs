using Aow.Rtl;

namespace Aow.Game.Audio;

internal class NullSample : Sample
{
    private protected NullSample(ConstructionContext context, SampleData data) : base(context, data)
    { }

    public static new NullSample Create(SampleData data)
            => ConstructionContext.Create(data, static (context, value) => new NullSample(context, value));

    private int _volume;
    private bool _looped;
    private SamplePanning _panning;

    protected internal override bool GetPlaying() => false;

    public override int Volume
    {
        get => _volume;
        set => _volume = value;
    }

    public override bool Looped
    {
        get => _looped;
        set => _looped = value;
    }

    public override SamplePanning Panning
    {
        get => _panning;
        set => _panning = value;
    }
}
