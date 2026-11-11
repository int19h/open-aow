// Translated from Sound.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// the loaded TSampleData
public partial class SampleDataList : DelphiList<SampleData>
{
    // Sound.pas:659
    public SampleData Find(string Name)
    {
        SampleData Result;
        int I;
        I = 0;
        while (I != Count && CompareText(Name, List[I].FName) != 0)
        {
            I += 1;
        }

        if (I != Count)
        {
            Result = List[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
