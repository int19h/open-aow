using Aow.Rtl;

namespace Aow.Game.Audio;

public sealed class SampleDataClass(string name, Func<string, SampleData> create, SampleDataClass.MemoryCreation createEx)
{
    public string Name { get; } = name;

    public delegate SampleData MemoryCreation(in WaveFormat format, object data, int size);

    public SampleData CreateInstance(string name) => create(name);
    public SampleData CreateInstance(in WaveFormat format, object data, int size) => createEx(format, data, size);
}
