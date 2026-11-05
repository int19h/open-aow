namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static T ReadLegacyRecord<T>(TObject value) => Ptr<T>.Unbox(value).Value;
}
