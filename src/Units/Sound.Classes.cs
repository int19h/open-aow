using System;
using Aow.Rtl;

namespace Aow.Units.Sound;

public interface ISoundSystemClass
{
    static abstract AnsiString Name { get; }
}

public sealed class SoundSystemClass
{
    private readonly Func<TSoundSystem> create;
    public Type Type { get; }
    public AnsiString Name { get; }

    private SoundSystemClass(Type type, AnsiString name, Func<TSoundSystem> create)
    {
        Type = type;
        Name = name;
        this.create = create;
    }

    public TSoundSystem Create() => create();
    public SoundSystemClass Alias(AnsiString name) => new(Type, name, create);
    public static SoundSystemClass Of<T>() where T : TSoundSystem, ISoundSystemClass, new() => Holder<T>.Value;

    private static class Holder<T> where T : TSoundSystem, ISoundSystemClass, new()
    {
        internal static readonly SoundSystemClass Value = new(typeof(T), T.Name, static () => new T());
    }
}

public sealed class SampleClass(Func<TSampleData, TSample> create)
{
    public TSample Create(TSampleData data) => create(data);
}

public sealed class SampleDataClass(Func<AnsiString, TSampleData> create, SampleDataClass.MemoryCreation createEx)
{
    public delegate TSampleData MemoryCreation(in TWaveFormatEx format, object data, int size);
    public TSampleData Create(AnsiString name) => create(name);
    public TSampleData CreateEx(in TWaveFormatEx format, object data, int size) => createEx(format, data, size);
}
