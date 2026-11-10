// Translated from Sound.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Audio;

// the sound effects (TSFXSequence children) by ID
public partial class SFXLibrary : ENode
{
    public new const int ClassID = 2162691;

    public SFXLibrary(ECustomNode owner) : base(owner)
    { }

    // Sound.pas:1088
    public SFXSequence GetSFXSequence(int Index)
    {
        SFXSequence Result;
        if (Index >= 0 && Index < this.FChildren.Count)
        {
            Result = (SFXSequence)this.FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Sound.pas:1096
    public Sample Play(int ID)
    {
        Sample Result;
        SFXSequence S;
        S = Find(ID);
        if (S != null)
        {
            Result = S.Play();
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Sound.pas:1107
    public Sample PlayEx(int ID, int Volume, bool Looped, int Delay, SamplePanning Panning, SampleCallBack CallBack)
    {
        Sample Result = default;
        SFXSequence S;
        S = Find(ID);
        if (S != null)
        {
            Result = S.PlayEx(Volume, Looped, Delay, Panning, CallBack);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Sound.pas:1119
    public SFXSequence Find(int ID)
    {
        int I;
        I = 0;
        SFXSequence Result = null;
        while (I != this.FChildren.Count && Result == null)
        {
            if (((SFXSequence)this.FChildren.List[I]).FID == ID)
            {
                Result = (SFXSequence)this.FChildren.List[I];
            }

            I += 1;
        }

        return Result;
    }

    // Sound.pas:1134
    // { The first ID that two sequences share, -1 when the IDs are unique. }
    public int DoubleID()
    {
        int I;
        int ID;
        I = 0;
        int Result = -1;
        while (Result == -1 && I != Count)
        {
            ID = ((SFXSequence)this.FChildren.List[I]).FID;
            for (int J = I + 1, last = Count - 1; J <= last; J++)
            {
                if (ID == ((SFXSequence)this.FChildren.List[J]).FID)
                {
                    Result = ID;
                }
            }

            I += 1;
        }

        return Result;
    }

    public sealed class SFXSequencesAccessor(SFXLibrary target)
    {
        public SFXSequence this[int Index] { get => target.GetSFXSequence(Index); }
    }

    public SFXSequencesAccessor SFXSequences => new SFXSequencesAccessor(this);

}
