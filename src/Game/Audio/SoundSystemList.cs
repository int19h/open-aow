// Translated from Sound.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// the registered sound system classes
public partial class SoundSystemList : DelphiList
{
    // Sound.pas:1293
    public SoundSystemClass Find(string Name)
    {
        SoundSystemClass Result;
        int I;
        I = 0;
        while (I != Count && CompareText(Name, ((SoundSystemClass)List[I]).Name) != 0)
        {
            I += 1;
        }

        if (I != Count)
        {
            Result = (SoundSystemClass)List[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
