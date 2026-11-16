#nullable disable
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
        int i;
        i = 0;
        while (i != Count && CompareText(name, List[i].Name) != 0)
        {
            i += 1;
        }

        result = i != Count ? List[i] : null;

        return result;
    }

}
