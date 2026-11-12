
namespace Aow.Game.Audio;

public sealed class SoundSystemClass(string name, Func<SoundSystem> create)
{
    public string Name { get; } = name;

    public SoundSystem CreateInstance() => create();
}
