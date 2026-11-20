#nullable disable
// Translated from Sound.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Audio;

// FID. A sound effect: its children are TSFX, one of them plays (at random)
public class SfxSequence(CustomNode owner) : OwningNode(owner), IChildList<Sfx>
{
    // The sound effect in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a Sfx, so the read never fails.
    public Sfx this[int index] => GetChild<Sfx>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is Sfx;

    int IChildSink<Sfx>.AddChild(Sfx child) => AddChild(child);

    public new const int ClassId = 2162690;

    // Sound.pas:1057
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Id = persister.Int32("Id", value: Id);
    }

    // Sound.pas:1052
    public Sfx GetSfx(int index) => this[index];

    // Sound.pas:1063
    public Sample Play() => Count != 0 ? this[Random(Count)].Play() : null;

    // Sound.pas:1071
    public Sample Play(int volume, bool looped, int delay, SamplePanning panning, SampleCallBack callback) => Count != 0 ? this[Random(Count)].Play(volume, looped, delay, panning, callback) : null;

    public int Id { get; set; }
}
