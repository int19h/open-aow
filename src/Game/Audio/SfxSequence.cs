// Translated from Sound.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// FID. A sound effect: its children are TSFX, one of them plays (at random)
public partial class SfxSequence : ENode
{
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
    public Sfx GetSfx(int index) => (Sfx)this._children.List[index];

    // Sound.pas:1063
    public Sample Play()
    {
        Sample result;
        if (this._children.Count != 0)
        {
            var random = Random(this._children.Count);
            result = ((Sfx)this._children.List[random]).Play();
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
        if (this._children.Count != 0)
        {
            var random = Random(this._children.Count);
            result = ((Sfx)this._children.List[random]).Play(volume, looped, delay, panning, callback);
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
