#nullable disable warnings
// Translated from Engine.pas.
using System.Globalization;
using Aow.Rtl;

namespace Aow.Engine;

// a TList of class references
public class EngineObjectClassList : DelphiList<EngineClass>
{
    // Engine.pas:8296
    protected internal EngineClass? GetEngineClass(int index) => this[index];

    // Engine.pas:8301
    // ClassId is a field. The list enumerates this[i], which is the slot the loop reads.
    public EngineClass? FindClass(int classId) => this.FirstOrDefault(engineClass => classId == engineClass.ClassId);

    // Engine.pas:8316
    public int FindClassIndex(int classId) => this.FindIndex(engineClass => classId == engineClass.ClassId);

    // Engine.pas:8331
    public EngineClass GetClass(int classId)
    {
        EngineClass? result = FindClass(classId);
        return result ?? throw new KeyNotFoundException($"Unregistered E-ClassID found ({classId.ToString(CultureInfo.InvariantCulture)})");
    }

    // Engine.pas:8338
    public void RegisterClass(EngineClass classValue)
    {
        _ = FindClass(classValue.ClassId) is null && classValue.ClassId != 0
            ? Add(classValue)
            : throw new InvalidOperationException("EObject Class ID double defined!");
    }

    // Engine.pas:8346
    public void UnregisterClass(EngineClass classValue)
    {
        int i = FindClassIndex(classValue.ClassId);
        if (i != -1)
        {
            Delete(i);
        }
    }

    public sealed class ClassesAccessor(EngineObjectClassList target)
    {
        public EngineClass? this[int index] => target.GetEngineClass(index);
    }

    public ClassesAccessor EngineClasses => new(this);
}
