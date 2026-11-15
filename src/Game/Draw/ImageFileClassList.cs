#nullable disable
// Translated from GFXE.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// { The implementation list of image file classes (TList of TImageFileClass). }
public partial class ImageFileClassList : DelphiList<ImageFileClass>
{
    // GFXE.pas:941
    internal ImageFileClass GetClasses(int index)
    {
        return this[index];
    }

    // GFXE.pas:949
    public ImageFileClass FindClass(string ext)
    {
        ImageFileClass result;
        int i;
        i = 0;
        if (!IsEmpty(ext) && ext[0] == 46)
        {
            ext = Copy(ext, 2, ext.Length - 1);
        }

        while (i != Count && CompareText(List[i].Extension, ext) != 0)
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
