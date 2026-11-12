// Translated from Sound.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// the loaded TSampleData
public partial class SampleDataList : DelphiList<SampleData>
{
    // Sound.pas:659
    public SampleData Find(string name)
    {
        SampleData result;
        int I;
        I = 0;
        while (I != Count && CompareText(name, List[I]._name) != 0)
        {
            I += 1;
        }

        if (I != Count)
        {
            result = List[I];
        }
        else
        {
            result = null;
        }

        return result;
    }

}
