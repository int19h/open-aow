namespace Aow.Rtl;

/// <summary>
/// Marks a struct that reads or writes a format (a file record, a native interop record or a wire
/// message) and therefore has a fixed or explicit layout. Only such transient I/O types carry a
/// layout. Game data uses natural layout and is mapped to and from them at the I/O boundary. The
/// layout guard (LayoutRuleTests and the private layout census) refuses a struct with explicit
/// layout, field offsets, a pack or a size that lacks this marker, and a game type that holds a
/// marked type in a field.
/// </summary>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class StoredLayoutAttribute : Attribute;
