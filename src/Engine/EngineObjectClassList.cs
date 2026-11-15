#nullable disable
// Translated from Engine.pas.
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine;

// a TList of class references
public partial class EngineObjectClassList : DelphiList<EngineClass>
{
    // Engine.pas:8296
    protected internal EngineClass GetEngineClass(int index) => this[index];

    // Engine.pas:8301
    public EngineClass FindClass(int classId)
    {
        EngineClass result;
        int i;
        bool found;
        i = 0;
        found = false;
        while (!found && i < Count)
        {
            found = classId == this[i].ClassId;
            i += 1;
        }

        if (found)
        {
            result = this[i - 1];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Engine.pas:8316
    public int FindClassIndex(int classId)
    {
        int result;
        int i;
        bool found;
        i = 0;
        found = false;
        while (!found && i < Count)
        {
            found = classId == this[i].ClassId;
            i += 1;
        }

        if (found)
        {
            result = i - 1;
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Engine.pas:8331
    public EngineClass GetClass(int classId)
    {
        EngineClass result = FindClass(classId);
        if (result == null)
        {
            throw new KeyNotFoundException("Unregistered E-ClassID found (" + IntToStr(classId) + ")");
        }

        return result;
    }

    // Engine.pas:8338
    public void RegisterClass(EngineClass classValue)
    {
        if (FindClass(classValue.ClassId) == null && classValue.ClassId != 0)
        {
            Add(classValue);
        }
        else
        {
            throw new InvalidOperationException("EObject Class ID double defined!");
        }
    }

    // Engine.pas:8346
    public void UnregisterClass(EngineClass classValue)
    {
        int i;
        i = FindClassIndex(classValue.ClassId);
        if (i != -1)
        {
            Delete(i);
        }
    }

    public sealed class ClassesAccessor(EngineObjectClassList target)
    {
        public EngineClass this[int index] { get => target.GetEngineClass(index); }
    }

    public ClassesAccessor EngineClasses => new ClassesAccessor(this);
}
