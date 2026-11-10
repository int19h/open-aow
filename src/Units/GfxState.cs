using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Game.Engine.Buffers;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public static class GfxState
{
    public static TLookupBlendTableCollection100 AlphaBlendTable;

    public static TLookupBlendTableCollection100 IntensityBlendTable;

    public static TLookupBlendTableCollection100 BrightenBlendTable;

    public static TLookupBlendTableCollection100 ShadowBlendTable;

    public static TLookupBlendTableCollection100 LinearAlphaBlendTable;

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

    internal static TImageFileClassList ImageFileClassList;

    internal static void InitBlendTables(int Bits, int Count)
    {
        AlphaBlendTable.GenerateLookupTables(Bits, Aow.Game.Draw.TBlendTable.GenerateAlphaBlendTable, Count);
        IntensityBlendTable.GenerateLookupTables(Bits, Aow.Game.Draw.TBlendTable.GenerateIntensityBlendTable, Count);
        BrightenBlendTable.GenerateLookupTables(Bits, Aow.Game.Draw.TBlendTable.GenerateBrightenBlendTable, Count);
        ShadowBlendTable.GenerateLookupTables(Bits, Aow.Game.Draw.TBlendTable.GenerateShadowBlendTable, Count);
        LinearAlphaBlendTable.GenerateLookupTables(Bits, Aow.Game.Draw.TBlendTable.GenerateLinearAlphaBlendTable, Count);
    }

    public static void Initialize()
    {
        AlphaBlendTable = new TLookupBlendTableCollection100();
        IntensityBlendTable = new TLookupBlendTableCollection100();
        BrightenBlendTable = new TLookupBlendTableCollection100();
        ShadowBlendTable = new TLookupBlendTableCollection100();
        LinearAlphaBlendTable = new TLookupBlendTableCollection100();
        InitBlendTables(5, 31);
        ForcedPixelFormatID = 0;
        ImageFileClassList = new TImageFileClassList();
        RegisterImageFileClasses([ImageFileClass.Of<TBMPImageFile>()]);
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
