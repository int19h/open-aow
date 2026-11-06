using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Aow.Formats.Tests
{
    internal static class UnitMappings
    {
        // The case mappings of TProductionItem and TDiplomaticAction are unit code
        // (own-formats.md 3.1). Loading Aow.Units runs ClassRegistry, which registers them.
        [System.Runtime.CompilerServices.ModuleInitializer]
        internal static void Load() =>
            System.Runtime.CompilerServices.RuntimeHelpers.RunModuleConstructor(typeof(Aow.Units.FormatMapping).Module.ModuleHandle);
    }
}
