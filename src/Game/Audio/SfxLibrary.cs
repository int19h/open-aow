// Translated from Sound.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Audio;

// the sound effects (TSFXSequence children) by ID
public partial class SfxLibrary : ENode
{
    public new const int ClassId = 2162691;

    public SfxLibrary(ECustomNode owner) : base(owner)
    { }

    // Sound.pas:1088
    public SfxSequence GetSfxSequence(int index)
    {
        SfxSequence result;
        if (index >= 0 && index < this._children.Count)
        {
            result = (SfxSequence)this._children.List[index];
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
        while (I != this._children.Count && result == null)
        {
            if (((SfxSequence)this._children.List[I])._id == id)
            {
                result = (SfxSequence)this._children.List[I];
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
            id = ((SfxSequence)this._children.List[I])._id;
            for (int J = I + 1, last = Count - 1; J <= last; J++)
            {
                if (id == ((SfxSequence)this._children.List[J])._id)
                {
                    result = id;
                }
            }

            I += 1;
        }

        return result;
    }

    public sealed class SfxSequencesAccessor(SfxLibrary target)
    {
        public SfxSequence this[int index] { get => target.GetSfxSequence(index); }
    }

    public SfxSequencesAccessor SfxSequences => new SfxSequencesAccessor(this);

}
