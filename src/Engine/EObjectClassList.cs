// Translated from Engine.pas.
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine;

// a TList of class references
public partial class EObjectClassList : DelphiList
{
    // Engine.pas:8296
    protected internal EClass GetEClass(int Index) => (EClass)this[Index];

    // Engine.pas:8301
    public EClass FindClass(int AClassID)
    {
        EClass Result;
        int I;
        bool Found;
        I = 0;
        Found = false;
        while (!Found && I < Count)
        {
            Found = AClassID == ((EClass)this[I]).ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = (EClass)this[I - 1];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:8316
    public int FindClassIndex(int AClassID)
    {
        int Result;
        int I;
        bool Found;
        I = 0;
        Found = false;
        while (!Found && I < Count)
        {
            Found = AClassID == ((EClass)this[I]).ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = I - 1;
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:8331
    public EClass GetClass(int AClassID)
    {
        EClass Result = FindClass(AClassID);
        if (Result == null)
        {
            throw new KeyNotFoundException("Unregistered E-ClassID found (" + IntToStr(AClassID) + ")");
        }

        return Result;
    }

    // Engine.pas:8338
    public void RegisterClass(EClass AClass)
    {
        if (FindClass(AClass.ClassID) == null && AClass.ClassID != 0)
        {
            Add(AClass);
        }
        else
        {
            throw new InvalidOperationException("EObject Class ID double defined!");
        }
    }

    // Engine.pas:8346
    public void UnregisterClass(EClass AClass)
    {
        int I;
        I = FindClassIndex(AClass.ClassID);
        if (I != -1)
        {
            Delete(I);
        }
    }

    public sealed class EClassesAccessor(EObjectClassList target)
    {
        public EClass this[int Index] { get => target.GetEClass(Index); }
    }

    public EClassesAccessor EClasses => new EClassesAccessor(this);
}
