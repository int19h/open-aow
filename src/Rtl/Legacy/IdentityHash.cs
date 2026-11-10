using System.Runtime.CompilerServices;

namespace Aow.Rtl;

// Masked so a modulo phase never goes negative. The runtime does not promise a non-negative hash.
internal static class IdentityHash
{
    internal static int Of(object? instance) => instance is null ? 0 : RuntimeHelpers.GetHashCode(instance) & int.MaxValue;
}
