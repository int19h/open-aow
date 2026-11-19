namespace Aow.Rtl;

/// <summary>
/// Declares padding bytes inside a <see cref="StoredLayoutAttribute"/> record image. The record
/// catalog ignores these bytes on read, writes 0 on write and never stores them in an own document.
/// </summary>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
public sealed class StoredPaddingAttribute(int offset, int length) : Attribute
{
    public int Offset { get; } = offset;
    public int Length { get; } = length;
}
