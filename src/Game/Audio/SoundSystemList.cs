#nullable disable
// Translated from Sound.pas.
using Aow.Rtl;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.Audio;

// the registered sound system classes
public class SoundSystemList : DelphiList<SoundSystemClass>
{
    // Sound.pas:1293
    public SoundSystemClass Find(string name)
    {
        int i = 0;
        while (i != Count && CompareText(name, List[i].Name) != 0)
        {
            i += 1;
        }

        return i != Count ? List[i] : null;
    }
}
