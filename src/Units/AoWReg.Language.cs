using Aow.Rtl;

namespace Aow.Units.AoWReg;

public partial class TAoWRegistry
{
    // Automatic and English retain the shipped forcing behavior.
    public TAoWLanguage GetLanguage()
    {
        int value = 0;
        rwInteger("General", "Language", ref value, TReadWriteMode.rwRead);
        return value is >= 2 and <= 5 ? (TAoWLanguage)value : TAoWLanguage.alEnglish;
    }
}
