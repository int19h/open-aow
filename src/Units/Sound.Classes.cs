using Aow.Rtl;

namespace Aow.Game.Audio;

public sealed class SoundSystemClass(string name, Func<TSoundSystem> create)
{
    public string Name { get; } = name;

    public TSoundSystem Create() => create();
}

public sealed class SampleClass(string name, Func<TSampleData, TSample> create)
{
    public string Name { get; } = name;

    public TSample Create(TSampleData data) => create(data);
}

public sealed class SampleDataClass(string name, Func<string, TSampleData> create, SampleDataClass.MemoryCreation createEx)
{
    public string Name { get; } = name;

    public delegate TSampleData MemoryCreation(in TWaveFormatEx format, object data, int size);

    public TSampleData Create(string name) => create(name);
    public TSampleData CreateEx(in TWaveFormatEx format, object data, int size) => createEx(format, data, size);
}
