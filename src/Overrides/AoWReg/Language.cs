using Aow.Rtl;

namespace Aow.Units.AoWReg;

public partial class TAoWRegistry
{
    // AoWReg.pas reads the setting, then the shipped patch forces English.
    public TAoWLanguage GetLanguage()
    {
        int value = 0;
        rwInteger((AnsiString)"General", (AnsiString)"Language", ref value, TReadWriteMode.rwRead);
        return TAoWLanguage.alEnglish;
    }
}
