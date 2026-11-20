#nullable disable
// Translated from Engine.pas.
using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Engine;

// a TList of class references
public partial class EngineObjectClassList : DelphiList<EngineClass>
{
    // Engine.pas:8296
    protected internal EngineClass GetEngineClass(int index) => this[index];

    // Engine.pas:8301
    public EngineClass FindClass(int classId)
    {
        int i = 0;
        bool found = false;
        while (!found && i < Count)
        {
            found = classId == this[i].ClassId;
            i += 1;
        }

        return found ? this[i - 1] : null;
    }

    // Engine.pas:8316
    public int FindClassIndex(int classId)
    {
        int i = 0;
        bool found = false;
        while (!found && i < Count)
        {
            found = classId == this[i].ClassId;
            i += 1;
        }

        return found ? i - 1 : -1;
    }

    // Engine.pas:8331
    public EngineClass GetClass(int classId)
    {
        EngineClass result = FindClass(classId);
        return result ?? throw new KeyNotFoundException($"Unregistered E-ClassID found ({IntToStr(classId)})");
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
        public EngineClass this[int index] => target.GetEngineClass(index);
    }

    public ClassesAccessor EngineClasses => new(this);
}
