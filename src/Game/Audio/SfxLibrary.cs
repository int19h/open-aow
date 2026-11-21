#nullable disable warnings
// Translated from Sound.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Audio;

// the sound effects (TSFXSequence children) by ID
public sealed class SfxLibrary(CustomNode? owner) : OwningNode(owner), IChildList<SfxSequence>
{
    // The sequence in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a SfxSequence, so the read never fails.
    public SfxSequence this[int index] => GetChild<SfxSequence>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is SfxSequence;

    int IChildSink<SfxSequence>.AddChild(SfxSequence child) => AddChild(child);

    public new const int ClassId = 2_162_691;

    // Sound.pas:1088
    public SfxSequence? GetSfxSequence(int index) => index >= 0 && index < Count ? this[index] : null;

    // Sound.pas:1096
    public Sample? Play(int id)
    {
        SfxSequence? s = Find(id);
        return s?.Play();
    }

    // Sound.pas:1107
    public Sample? Play(int id, int volume, bool looped, int delay, SamplePanning panning, Action<Sample, SampleCallbackMode>? callback)
    {
        SfxSequence? s = Find(id);
        return s?.Play(volume, looped, delay, panning, callback);
    }

    // Sound.pas:1119
    public SfxSequence? Find(int id)
    {
        int i = 0;
        SfxSequence? result = null;
        while (i != Count && result is null)
        {
            if (this[i].Id == id)
            {
                result = this[i];
            }

            i += 1;
        }

        return result;
    }

    // Sound.pas:1134
    // { The first ID that two sequences share, -1 when the IDs are unique. }
    public int DoubleId()
    {
        int i = 0;
        int result = -1;
        while (result == -1 && i != Count)
        {
            int id = this[i].Id;
            for (int j = i + 1, last = Count - 1; j <= last; j++)
            {
                if (id == this[j].Id)
                {
                    result = id;
                }
            }

            i += 1;
        }

        return result;
    }
}
