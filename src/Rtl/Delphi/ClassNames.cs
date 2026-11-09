namespace Aow.Rtl;

// Type names stored by the port. A later step replaces this with the persisted-name table.
public static class ClassNames
{
    public static string ClassNameOf(Type type) => type.Name.TrimStart('@');
}
