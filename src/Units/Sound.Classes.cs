using System;
using Aow.Rtl;

namespace Aow.Units.Sound;

public sealed class SoundSystemClass(AnsiString name, Func<TSoundSystem> create)
{
    public AnsiString Name { get; } = name;
    public TSoundSystem Create() => create();
}

public sealed class SampleClass(AnsiString name, Func<TSampleData, TSample> create)
{
    public AnsiString Name { get; } = name;
    public TSample Create(TSampleData data) => create(data);
}

public sealed class SampleDataClass(AnsiString name, Func<AnsiString, TSampleData> create, SampleDataClass.MemoryCreation createEx)
{
    public AnsiString Name { get; } = name;
    public delegate TSampleData MemoryCreation(in TWaveFormatEx format, object data, int size);
    public TSampleData Create(AnsiString name) => create(name);
    public TSampleData CreateEx(in TWaveFormatEx format, object data, int size) => createEx(format, data, size);
}
