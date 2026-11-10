// Translated from HSEngine.pas.

namespace Aow.Game.World;

// HSEngine.EMapObjectResourceMissing: TMapObject.ReadWrite raises it when the resource id read
// from the stream is not in the resource list
public partial class MapObjectResourceMissingException : Exception
{
    // SysUtils.pas:174
    public MapObjectResourceMissingException(string Msg) : base(Msg)
    {
    }
}
