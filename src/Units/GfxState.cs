using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Game.Engine.Buffers;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public static class GfxState
{
    public static LookupBlendTableCollection100 AlphaBlendTable;

    public static LookupBlendTableCollection100 IntensityBlendTable;

    public static LookupBlendTableCollection100 BrightenBlendTable;

    public static LookupBlendTableCollection100 ShadowBlendTable;

    public static LookupBlendTableCollection100 LinearAlphaBlendTable;

    internal static void RegisterImageFileClasses(ReadOnlySpan<ImageFileClass> AClasses)
    {
        for (int I = 0, last = AClasses.Length - 1; I <= last; I++)
        {
            var extension = AClasses[I].Extension;
            if (ImageFileClassList.FindClass(extension) == null)
            {
                ImageFileClassList.Add(AClasses[I]);
            }
            else
            {
                throw new InvalidOperationException("ImageFile class with " + AClasses[I].Extension + " extension is already registered");
            }
        }
    }

    internal static int ForcedPixelFormatID;

    internal static ImageFileClassList ImageFileClassList;

    internal static void InitBlendTables(int Bits, int Count)
    {
        AlphaBlendTable.GenerateLookupTables(Bits, Aow.Game.Draw.BlendTable.GenerateAlphaBlendTable, Count);
        IntensityBlendTable.GenerateLookupTables(Bits, Aow.Game.Draw.BlendTable.GenerateIntensityBlendTable, Count);
        BrightenBlendTable.GenerateLookupTables(Bits, Aow.Game.Draw.BlendTable.GenerateBrightenBlendTable, Count);
        ShadowBlendTable.GenerateLookupTables(Bits, Aow.Game.Draw.BlendTable.GenerateShadowBlendTable, Count);
        LinearAlphaBlendTable.GenerateLookupTables(Bits, Aow.Game.Draw.BlendTable.GenerateLinearAlphaBlendTable, Count);
    }

    public static void Initialize()
    {
        AlphaBlendTable = new LookupBlendTableCollection100();
        IntensityBlendTable = new LookupBlendTableCollection100();
        BrightenBlendTable = new LookupBlendTableCollection100();
        ShadowBlendTable = new LookupBlendTableCollection100();
        LinearAlphaBlendTable = new LookupBlendTableCollection100();
        InitBlendTables(5, 31);
        ForcedPixelFormatID = 0;
        ImageFileClassList = new ImageFileClassList();
        RegisterImageFileClasses([ImageFileClass.Of<BMPImageFile>()]);
    }

    public static void Finalize()
    {
        AlphaBlendTable?.Dispose();
        IntensityBlendTable?.Dispose();
        BrightenBlendTable?.Dispose();
        ShadowBlendTable?.Dispose();
        LinearAlphaBlendTable?.Dispose();
        ImageFileClassList?.Dispose();
    }

}
