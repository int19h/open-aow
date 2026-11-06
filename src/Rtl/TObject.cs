namespace Aow.Rtl;

public class TObject
{
    private static int nextId;
    internal static int AllocateId() => Interlocked.Increment(ref nextId);
    public int InstanceId { get; } = AllocateId();
    public static string ClassNameOf(Type type) => type.Name.TrimStart('@');
    public virtual void Destroy() { }
    public virtual void FreeInstance() { }

}


public static class TObjectExtensions
{
    public static void Free(this TObject? value)
    {
        value?.Destroy();
    }
}
