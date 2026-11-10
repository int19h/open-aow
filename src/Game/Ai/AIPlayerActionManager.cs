// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Ai;

// registered classes
public partial class AIPlayerActionManager : EObject
{
    // AI.pas:36
    public AIPlayerActionManager(ECustomNode owner) : base(owner)
    {
        FRegisteredClasses = new DelphiList();
    }

    public DelphiList FRegisteredClasses;

    // AI.pas:42
    public override void Dispose()
    {
        FRegisteredClasses?.Dispose();
        base.Dispose();
    }

    // AI.pas:48
    public int GetCount() => FRegisteredClasses.Count;

    // AI.pas:53
    public AIPlayerActionClass GetRegisteredClasses(int Index) => (AIPlayerActionClass)FRegisteredClasses.List[Index];

    // AI.pas:63
    public void RegisterClasses(ReadOnlySpan<AIPlayerActionClass> Classes)
    {
        for (int I = 0, last = Classes.Length - 1; I <= last; I++)
        {
            if (FRegisteredClasses.IndexOf(Classes[I]) != -1)
            {
                throw new InvalidOperationException("AIPlayerAction already Registered");
            }

            FRegisteredClasses.Add(Classes[I]);
        }

        FRegisteredClasses.Sort(CompareAIPlayerActionClasses);
    }

    // AI.pas:76
    public void UnRegisterClasses(ReadOnlySpan<AIPlayerActionClass> Classes)
    {
        for (int I = 0, last = Classes.Length - 1; I <= last; I++)
        {
            FRegisteredClasses.Remove(Classes[I]);
        }
    }

    private static int CompareAIPlayerActionClasses(object Item1, object Item2)
    {
        return ((AIPlayerActionClass)Item2).Priority - ((AIPlayerActionClass)Item1).Priority;
    }

    // intf_P5_06.inc:32

    // intf_P5_06.inc:32

}
