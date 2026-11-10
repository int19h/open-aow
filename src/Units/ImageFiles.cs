using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

internal static class ImageFiles
{
    internal static string GetImageFilesFilter()
    {
        string Result = "";
        for (int I = 0, last = GfxState.ImageFileClassList.Count - 1; I <= last; I++)
        {
            if (!IsEmpty(Result))
            {
                Result = Result + "|";
            }

            Result = Result + GfxState.ImageFileClassList.Classes[I].Description + "|*." + GfxState.ImageFileClassList.Classes[I].Extension;
        }

        return Result;
    }

    internal static int LoadImageFile(string FileName, ref SurfaceDesc SD, int X, int Y)
    {
        int Result;
        ImageFileClass C;
        var extractFileExt = ExtractFileExt(FileName);
        C = GfxState.ImageFileClassList.FindClass(extractFileExt);
        if (C != null)
        {
            Result = C.LoadToSurfaceDesc(FileName, ref SD, X, Y);
        }
        else
        {
            Result = PixelFormats.GFXE_NOTSUPPORTED;
        }

        return Result;
    }

    internal static int GetImageFilePixelFormat(string FileName)
    {
        int Result;
        ImageFileClass C;
        var extractFileExt = ExtractFileExt(FileName);
        C = GfxState.ImageFileClassList.FindClass(extractFileExt);
        if (C != null)
        {
            Result = C.GetPixelFormatID(FileName);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

}
