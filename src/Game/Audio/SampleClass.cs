
namespace Aow.Game.Audio;

public sealed class SampleClass(string name, Func<SampleData, Sample> create)
{
    public string Name { get; } = name;

    public Sample CreateInstance(SampleData data) => create(data);
}
