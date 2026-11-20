// Translated from HSEngine.pas.

namespace Aow.Game.World;

// HSEngine.EMapObjectResourceMissing: TMapObject.ReadWrite raises it when the resource id read
// from the stream is not in the resource list
public sealed class MapObjectResourceMissingException(string message) : Exception(message)
{
}
