using System.Runtime.CompilerServices;
using Aow.Engine.Nodes;
using Aow.Formats;

namespace Aow.Convert;

/// <summary>
/// Registers the unit enums for the converter. Formats does not reference the units assembly,
/// so the converter loads that catalog beside the schema it already uses.
/// </summary>
public static class UnitCatalog
{
    [ModuleInitializer]
    public static void Register()
    {
        EnumCatalog.RegisterReflection(typeof(EngineObject).Assembly);
    }
}
