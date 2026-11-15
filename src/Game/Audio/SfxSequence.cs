#nullable disable
// Translated from Sound.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// FID. A sound effect: its children are TSFX, one of them plays (at random)
public partial class SfxSequence : OwningNode, IChildList<Sfx>
{
    // The sound effect in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a Sfx, so the cast never fails.
    public Sfx this[int index] => (Sfx)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is Sfx;

    int IChildSink<Sfx>.AddChild(Sfx child) => AddChild(child);

    public new const int ClassId = 2162690;

    public SfxSequence(CustomNode owner) : base(owner)
    { }

    private int _id;

    // Sound.pas:1057
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _id = persister.Int32("Id", value: _id);
    }

    // Sound.pas:1052
    public Sfx GetSfx(int index) => this[index];

    // Sound.pas:1063
    public Sample Play()
    {
        Sample result;
        if (Count != 0)
        {
            result = this[Random(Count)].Play();
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
        if (Count != 0)
        {
            result = this[Random(Count)].Play(volume, looped, delay, panning, callback);
        }
        else
        {
            result = null;
        }

        return result;
    }

    public int Id { get => _id; set => _id = value; }

}
