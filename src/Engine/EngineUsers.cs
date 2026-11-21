// Translated from Engine.pas.

namespace Aow.Engine;

[Flags]
public enum EngineUsers : byte
{
    None = 0,
    Programmer = 1 << 0,
    Designer = 1 << 1,
    User = 1 << 2,
}
