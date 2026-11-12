
namespace Aow.Game.Draw;

public static class GraphicsState
{
    public static LookupBlendTableCollection100 AlphaBlendTable;

    public static LookupBlendTableCollection100 IntensityBlendTable;

    public static LookupBlendTableCollection100 BrightenBlendTable;

    public static LookupBlendTableCollection100 ShadowBlendTable;

    public static LookupBlendTableCollection100 LinearAlphaBlendTable;

    internal static void RegisterImageFileClasses(ReadOnlySpan<ImageFileClass> classes)
    {
        for (int I = 0, last = classes.Length - 1; I <= last; I++)
        {
            var extension = classes[I].Extension;
            if (ImageFileClassList.FindClass(extension) == null)
            {
                ImageFileClassList.Add(classes[I]);
            }
            else
            {
                throw new InvalidOperationException("ImageFile class with " + classes[I].Extension + " extension is already registered");
            }
        }
    }

    internal static int s_forcedPixelFormatId;

    internal static ImageFileClassList ImageFileClassList;

    internal static void InitializeBlendTables(int bits, int count)
    {
        AlphaBlendTable.GenerateLookupTables(bits, BlendTable.GenerateAlphaBlendTable, count);
        IntensityBlendTable.GenerateLookupTables(bits, BlendTable.GenerateIntensityBlendTable, count);
        BrightenBlendTable.GenerateLookupTables(bits, BlendTable.GenerateBrightenBlendTable, count);
        ShadowBlendTable.GenerateLookupTables(bits, BlendTable.GenerateShadowBlendTable, count);
        LinearAlphaBlendTable.GenerateLookupTables(bits, BlendTable.GenerateLinearAlphaBlendTable, count);
    }

    public static void Initialize()
    {
        AlphaBlendTable = new LookupBlendTableCollection100();
        IntensityBlendTable = new LookupBlendTableCollection100();
        BrightenBlendTable = new LookupBlendTableCollection100();
        ShadowBlendTable = new LookupBlendTableCollection100();
        LinearAlphaBlendTable = new LookupBlendTableCollection100();
        InitializeBlendTables(5, 31);
        s_forcedPixelFormatId = 0;
        ImageFileClassList = new ImageFileClassList();
        RegisterImageFileClasses([ImageFileClass.Of<BmpImageFile>()]);
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
