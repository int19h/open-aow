
namespace Aow.Game.Draw.Images;

public sealed class GrabInfoClass(string name, Func<GrabInfo> create)
{
    public string Name { get; } = name;

    public GrabInfo Create() => create();
}
