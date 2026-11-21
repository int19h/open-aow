// Translated from Engine.pas.

namespace Aow.Engine.Nodes;

[Flags]
public enum ControlStyles : byte
{
    None = 0,
    SaveLoad = 1 << 0,
    FixedChild = 1 << 1,
    VerifyFixedChildren = 1 << 2,
    ArrayMode = 1 << 3,
}
