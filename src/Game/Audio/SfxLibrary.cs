#nullable disable
// Translated from Sound.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Audio;

// the sound effects (TSFXSequence children) by ID
public partial class SfxLibrary : OwningNode, IChildList<SfxSequence>
{
    // The sequence in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a SfxSequence, so the cast never fails.
    public SfxSequence this[int index] => (SfxSequence)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is SfxSequence;

    int IChildSink<SfxSequence>.AddChild(SfxSequence child) => AddChild(child);

    public new const int ClassId = 2162691;

    public SfxLibrary(CustomNode owner) : base(owner)
    { }

    // Sound.pas:1088
    public SfxSequence GetSfxSequence(int index)
    {
        SfxSequence result;
        if (index >= 0 && index < Count)
        {
            result = this[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Sound.pas:1096
    public Sample Play(int id)
    {
        Sample result;
        SfxSequence s;
        s = Find(id);
        if (s != null)
        {
            result = s.Play();
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Sound.pas:1107
    public Sample Play(int id, int volume, bool looped, int delay, SamplePanning panning, SampleCallBack callback)
    {
        Sample result = default;
        SfxSequence s;
        s = Find(id);
        if (s != null)
        {
            result = s.Play(volume, looped, delay, panning, callback);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Sound.pas:1119
    public SfxSequence Find(int id)
    {
        int i;
        i = 0;
        SfxSequence result = null;
        while (i != Count && result == null)
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
        int i;
        int id;
        i = 0;
        int result = -1;
        while (result == -1 && i != Count)
        {
            id = this[i].Id;
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
