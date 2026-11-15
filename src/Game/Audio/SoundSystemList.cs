// Translated from Sound.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// the registered sound system classes
public partial class SoundSystemList : DelphiList<SoundSystemClass>
{
    // Sound.pas:1293
    public SoundSystemClass Find(string name)
    {
        SoundSystemClass result;
        int i;
        i = 0;
        while (i != Count && CompareText(name, List[i].Name) != 0)
        {
            i += 1;
        }

        if (i != Count)
        {
            result = List[i];
        }
        else
        {
            result = null;
        }

        return result;
    }

}
