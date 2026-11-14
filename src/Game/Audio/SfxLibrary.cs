// Translated from Sound.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Audio;

// the sound effects (TSFXSequence children) by ID
public partial class SfxLibrary : ENode, IChildList<SfxSequence>
{
    // The sequence in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a SfxSequence, so the cast never fails.
    public SfxSequence this[int index] => (SfxSequence)this.GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is SfxSequence;

    int IChildSink<SfxSequence>.AddChild(SfxSequence child) => AddChild(child);

    public new const int ClassId = 2162691;

    public SfxLibrary(ECustomNode owner) : base(owner)
    { }

    // Sound.pas:1088
    public SfxSequence GetSfxSequence(int index)
    {
        SfxSequence result;
        if (index >= 0 && index < this.Count)
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
        SfxSequence S;
        S = Find(id);
        if (S != null)
        {
            result = S.Play();
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
        SfxSequence S;
        S = Find(id);
        if (S != null)
        {
            result = S.Play(volume, looped, delay, panning, callback);
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
        int I;
        I = 0;
        SfxSequence result = null;
        while (I != this.Count && result == null)
        {
            if (this[I].Id == id)
            {
                result = this[I];
            }

            I += 1;
        }

        return result;
    }

    // Sound.pas:1134
    // { The first ID that two sequences share, -1 when the IDs are unique. }
    public int DoubleId()
    {
        int I;
        int id;
        I = 0;
        int result = -1;
        while (result == -1 && I != Count)
        {
            id = this[I].Id;
            for (int J = I + 1, last = Count - 1; J <= last; J++)
            {
                if (id == this[J].Id)
                {
                    result = id;
                }
            }

            I += 1;
        }

        return result;
    }

}
