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
        int I;
        I = 0;
        if (!IsEmpty(ext) && ext[0] == 46)
        {
            ext = Copy(ext, 2, ext.Length - 1);
        }

        while (I != Count && CompareText(List[I].Extension, ext) != 0)
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

    public sealed class ClassesAccessor(ImageFileClassList target)
    {
        public ImageFileClass this[int index] { get => target.GetClasses(index); }
    }

    public ClassesAccessor Classes => new ClassesAccessor(this);
}
