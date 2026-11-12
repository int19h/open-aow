// Translated from Sound.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// the registered sound system classes
public partial class SoundSystemList : DelphiList
{
    // Sound.pas:1293
    public SoundSystemClass Find(string name)
    {
        SoundSystemClass result;
        int I;
        I = 0;
        while (I != Count && CompareText(name, ((SoundSystemClass)List[I]).Name) != 0)
        {
            I += 1;
        }

        if (I != Count)
        {
            result = (SoundSystemClass)List[I];
        }
        else
        {
            result = null;
        }

        return result;
    }

}
