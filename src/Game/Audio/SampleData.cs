// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// FName, FRefCount. The sound data of one wave file, in SampleDataList while it is loaded.
// Create counts the first reference
public partial class SampleData : IDisposable
{
    public static SampleDataClass Descriptor { get; } = new("SampleData", Create, Create);

    private readonly string _name = "";
    private int _refCount;

    // Sound.pas:674
    protected SampleData(ConstructionContext context, string name)
    {
        context.Capture(this);
        _name = name;
        AudioHost.SampleDataList.Add(this);
        AddRef();
    }

    // Sound.pas:683
    // { Sound data from memory; it is not put in SampleDataList. }
    protected SampleData(ConstructionContext context, in WaveFormatEx format, object data, int size)
    {
        context.Capture(this);
        _name = "";
        AddRef();
    }

    // Sound.pas:696
    public virtual void Release()
    {
        _refCount -= 1;
        if (_refCount == 0)
        {
            Dispose();
        }
    }

    // Sound.pas:703
    public virtual void AddRef()
    {
        _refCount += 1;
    }

    // Sound.pas:690
    public virtual void Dispose()
    {
        AudioHost.SampleDataList.Remove(this);
    }

    public string Name { get => _name; }
    public int RefCount { get => _refCount; }

    // Sound.pas:60
    public static SampleData Create(string name) =>
        ConstructionContext.Create(name, static (context, value) => new SampleData(context, value));

    // Sound.pas:61
    public static SampleData Create(in WaveFormatEx format, object data, int size) =>
        ConstructionContext.Create((format, data, size), static (context, value) =>
            new SampleData(context, value.format, value.data, value.size));

    public static Sample CreateSample(SampleData sampleData)
    {
        Sample result;
        if (AudioHost.SoundSystem != null)
        {
            result = AudioHost.SoundSystem.SampleClass().CreateInstance(sampleData);
        }
        else
        {
            result = null;
        }

        return result;
    }

}
