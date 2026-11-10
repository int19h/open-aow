using Aow.Rtl;

namespace Aow.Game.Audio;

internal class NullSample : Sample
{
    private protected NullSample(ConstructionContext context, SampleData data) : base(context, data)
    { }

    public static new NullSample Create(SampleData data) =>
            ConstructionContext.Create(data, static (context, value) => new NullSample(context, value));

    private int volume;
    private bool looped;
    private SamplePanning panning;

    protected internal override bool GetPlaying() => false;
    protected internal override int GetVolume() => volume;
    protected internal override void SetVolume(int value) => volume = value;
    protected internal override bool GetLooped() => looped;
    protected internal override void SetLooped(bool value) => looped = value;
    protected internal override SamplePanning GetPanning() => panning;
    protected internal override void SetPanning(SamplePanning value) => panning = value;
}
