// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// FName, FRefCount. The sound data of one wave file, in SampleDataList while it is loaded.
// Create counts the first reference
public partial class SampleData : IDisposable
{
    public static SampleDataClass Descriptor { get; } = new("SampleData", Create, CreateEx);

    internal string FName = "";
    internal int FRefCount;

    // Sound.pas:674
    protected SampleData(ConstructionContext context, string Name)
    {
        context.Capture(this);
        FName = Name;
        AudioHost.SampleDataList.Add(this);
        AddRef();
    }

    // Sound.pas:683
    // { Sound data from memory; it is not put in SampleDataList. }
    protected SampleData(ConstructionContext context, in WaveFormatEx Format, object Data, int Size)
    {
        context.Capture(this);
        FName = "";
        AddRef();
    }

    // Sound.pas:696
    public virtual void Release()
    {
        FRefCount -= 1;
        if (FRefCount == 0)
        {
            Dispose();
        }
    }

    // Sound.pas:703
    public virtual void AddRef()
    {
        FRefCount += 1;
    }

    // Sound.pas:690
    public virtual void Dispose()
    {
        AudioHost.SampleDataList.Remove(this);
    }

    public string Name { get => FName; }
    public int RefCount { get => FRefCount; }

    // Sound.pas:60
    public static SampleData Create(string name) =>
        ConstructionContext.Create(name, static (context, value) => new SampleData(context, value));

    // Sound.pas:61
    public static SampleData CreateEx(in WaveFormatEx format, object data, int size) =>
        ConstructionContext.Create((format, data, size), static (context, value) =>
            new SampleData(context, value.format, value.data, value.size));

    public static Sample CreateSample(SampleData SampleData)
    {
        Sample Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = AudioHost.SoundSystem.SampleClass().Create(SampleData);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
