namespace Aow.Rtl;

public class TObject
{
    public TObject()
    {
        CaptureAddress.Assign(this);
    }

    public static string ClassNameOf(Type type) => type.Name.TrimStart('@');

    public virtual void Destroy() { }
}

public static class TObjectExtensions
{
    public static void Free(this TObject? value)
    {
        value?.Destroy();
    }
}
