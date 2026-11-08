using Aow.Formats;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Aow.Formats.Tests;

internal static class UnitMappings
{
    // The case mappings of TProductionItem and TDiplomaticAction are unit code
    // (own-formats.md 3.1). Loading Aow.Units runs ClassRegistry, which registers them
    // and the unit enum catalog. The two sample enums belong to this assembly.
    [System.Runtime.CompilerServices.ModuleInitializer]
    internal static void Load()
    {
        System.Runtime.CompilerServices.RuntimeHelpers.RunModuleConstructor(typeof(Aow.Units.FormatMapping).Module.ModuleHandle);
        EnumCatalog.Register(new DictionaryEnumCatalog(typeof(TSampleRank), typeof(TOtherRank)));
    }
}

enum TSampleRank : byte
{
    srNone = 0,
    srGold = 1,
}

enum TOtherRank : byte
{
    orLow = 0,
}
