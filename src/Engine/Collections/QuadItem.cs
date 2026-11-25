// Translated from Engine.pas.
namespace Aow.Engine.Collections;

public struct QuadItem<TA> where TA : class
{
    public TA First { get; init; }
    public int Second { get; init; }
    public int Third { get; init; }
    public int Fourth { get; set; }
}
