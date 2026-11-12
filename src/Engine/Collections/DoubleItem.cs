// Translated from Engine.pas.

namespace Aow.Engine.Collections;

public struct DoubleItem<TA> where TA : class
{
    public TA First { get; init; }
    public int Second { get; init; }
}
