#nullable disable warnings
// Translated from GFXE.pas.
using Aow.Rtl;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.Draw;

// { The implementation list of image file classes (TList of TImageFileClass). }
public sealed class ImageFileClassList : DelphiList<ImageFileClass>
{
    // GFXE.pas:941
    internal ImageFileClass? GetClasses(int index) => this[index];

    // GFXE.pas:949
    public ImageFileClass? FindClass(string ext)
    {
        int i = 0;
        if (!IsEmpty(ext) && ext[0] == 46)
        {
            ext = Copy(ext, index: 2, count: ext.Length - 1);
        }

        while (i != Count && CompareText(List[i].Extension, ext) != 0)
        {
            i += 1;
        }

        return i != Count ? List[i] : null;
    }
}
