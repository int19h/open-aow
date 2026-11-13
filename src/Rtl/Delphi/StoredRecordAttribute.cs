namespace Aow.Rtl;

/// <summary>
/// Binds a <see cref="StoredLayoutAttribute"/> image to the record name that schemas and own
/// documents store. The record catalog describes the image under this name, so the stored name
/// stays fixed while the game type the image maps is free to change.
/// </summary>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class StoredRecordAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
