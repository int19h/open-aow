// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.AI;

// registered classes
public partial class AIPlayerActionManager : EngineObject
{
    // AI.pas:36
    public AIPlayerActionManager(ECustomNode owner) : base(owner)
    {
        RegisteredClasses = new DelphiList<AIPlayerActionClass>();
    }

    public DelphiList<AIPlayerActionClass> RegisteredClasses;

    // AI.pas:42
    public override void Dispose()
    {
        RegisteredClasses?.Dispose();
        base.Dispose();
    }

    // AI.pas:48
    public int GetCount() => RegisteredClasses.Count;

    // AI.pas:53
    public AIPlayerActionClass GetRegisteredClasses(int index) => RegisteredClasses.List[index];

    // AI.pas:63
    public void RegisterClasses(ReadOnlySpan<AIPlayerActionClass> classes)
    {
        for (int I = 0, last = classes.Length - 1; I <= last; I++)
        {
            if (RegisteredClasses.IndexOf(classes[I]) != -1)
            {
                throw new InvalidOperationException("AIPlayerAction already Registered");
            }

            RegisteredClasses.Add(classes[I]);
        }

        RegisteredClasses.Sort(CompareAIPlayerActionClasses);
    }

    // AI.pas:76
    public void UnregisterClasses(ReadOnlySpan<AIPlayerActionClass> classes)
    {
        for (int I = 0, last = classes.Length - 1; I <= last; I++)
        {
            RegisteredClasses.Remove(classes[I]);
        }
    }

    private static int CompareAIPlayerActionClasses(AIPlayerActionClass item1, AIPlayerActionClass item2)
    {
        return item2.Priority - item1.Priority;
    }

    // intf_P5_06.inc:32

    // intf_P5_06.inc:32

}
