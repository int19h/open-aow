using Aow.Rtl;

namespace Aow.Game.Audio;

internal class NullSample : Sample
{
    private protected NullSample(ConstructionContext context, SampleData data) : base(context, data)
    { }

    public static new NullSample Create(SampleData data) =>
            ConstructionContext.Create(data, static (context, value) => new NullSample(context, value));

    private int _volume;
    private bool _looped;
    private SamplePanning _panning;

    protected internal override bool GetPlaying() => false;
    protected internal override int GetVolume() => _volume;
    protected internal override void SetVolume(int value) => _volume = value;
    protected internal override bool GetLooped() => _looped;
    protected internal override void SetLooped(bool value) => _looped = value;
    protected internal override SamplePanning GetPanning() => _panning;
    protected internal override void SetPanning(SamplePanning value) => _panning = value;
}
