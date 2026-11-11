// Translated from GFXE.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// { The implementation list of image file classes (TList of TImageFileClass). }
public partial class ImageFileClassList : DelphiList<ImageFileClass>
{
    // GFXE.pas:941
    internal ImageFileClass GetClasses(int Index)
    {
        return this[Index];
    }

    // GFXE.pas:949
    public ImageFileClass FindClass(string Ext)
    {
        ImageFileClass Result;
        int I;
        I = 0;
        if (!IsEmpty(Ext) && Ext[0] == 46)
        {
            Ext = Copy(Ext, 2, Ext.Length - 1);
        }

        while (I != Count && CompareText(List[I].Extension, Ext) != 0)
        {
            I += 1;
        }

        if (I != Count)
        {
            Result = List[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    public sealed class ClassesAccessor(ImageFileClassList target)
    {
        public ImageFileClass this[int Index] { get => target.GetClasses(Index); }
    }

    public ClassesAccessor Classes => new ClassesAccessor(this);
}
