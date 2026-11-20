#nullable disable
// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// FName, FRefCount. The sound data of one wave file, in SampleDataList while it is loaded.
// Create counts the first reference
public class SampleData : IDisposable
{
    public static SampleDataClass Descriptor { get; } = new("SampleData", Create, createEx: Create);

    // Sound.pas:674
    protected SampleData(ConstructionContext context, string name)
    {
        context.Capture(this);
        Name = name;
        _ = AudioHost.SampleDataList.Add(this);
        AddRef();
    }

    // Sound.pas:683
    // { Sound data from memory; it is not put in SampleDataList. }
    protected SampleData(ConstructionContext context, in WaveFormat format, object data, int size)
    {
        context.Capture(this);
        Name = "";
        AddRef();
    }

    // Sound.pas:696
    public virtual void Release()
    {
        RefCount -= 1;
        if (RefCount == 0)
        {
            Dispose();
        }
    }

    // Sound.pas:703
    public virtual void AddRef() => RefCount += 1;

    // Sound.pas:690
    public virtual void Dispose() => AudioHost.SampleDataList.Remove(this);

    public string Name { get; } = "";
    public int RefCount { get; private set; }

    // Sound.pas:60
    public static SampleData Create(string name)
        => ConstructionContext.Create(name, static (context, value) => new SampleData(context, name: value));

    // Sound.pas:61
    public static SampleData Create(in WaveFormat format, object data, int size)
        => ConstructionContext.Create((format, data, size), static (context, value) =>
            new SampleData(context, value.format, value.data, value.size));

    public static Sample CreateSample(SampleData sampleData) => AudioHost.SoundSystem?.SampleClass().CreateInstance(sampleData);

}
