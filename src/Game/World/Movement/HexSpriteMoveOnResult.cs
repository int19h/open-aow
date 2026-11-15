// Translated from HSEngine.pas.
namespace Aow.Game.World.Movement;

public struct HexSpriteMoveOnResult
{
    public int Code { get; internal set; }
    public int Data { get; init; }
    public bool Done { get; internal set; }
}
