// Translated from Sound.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// FID. A sound effect: its children are TSFX, one of them plays (at random)
public partial class SfxSequence : ENode, IChildList<Sfx>
{
    // The sound effect in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a Sfx, so the cast never fails.
    public Sfx this[int index] => (Sfx)this.GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is Sfx;

    int IChildSink<Sfx>.AddChild(Sfx child) => AddChild(child);

    public new const int ClassId = 2162690;

    public SfxSequence(ECustomNode owner) : base(owner)
    { }

    internal int _id;

    // Sound.pas:1057
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref _id, 5);
    }

    // Sound.pas:1052
    public Sfx GetSfx(int index) => this[index];

    // Sound.pas:1063
    public Sample Play()
    {
        Sample result;
        if (this.Count != 0)
        {
            var random = Random(this.Count);
            result = this[random].Play();
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Sound.pas:1071
    public Sample Play(int volume, bool looped, int delay, SamplePanning panning, SampleCallBack callback)
    {
        Sample result = default;
        if (this.Count != 0)
        {
            var random = Random(this.Count);
            result = this[random].Play(volume, looped, delay, panning, callback);
        }
        else
        {
            result = null;
        }

        return result;
    }

    public int Id { get => _id; set => _id = value; }

    public sealed class SfxAccessor(SfxSequence target)
    {
        public Sfx this[int index] { get => target.GetSfx(index); }
    }

    public SfxAccessor Sfx => new SfxAccessor(this);

}
