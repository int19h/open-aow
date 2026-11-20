#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.AI;

// registered classes
public class AIPlayerActionManager(CustomNode owner) : EngineObject(owner)
{

    private readonly DelphiList<AIPlayerActionClass> _registeredClasses = new();

    // AI.pas:42
    public override void Dispose()
    {
        _registeredClasses?.Dispose();
        base.Dispose();
    }

    // AI.pas:48
    public int GetCount() => _registeredClasses.Count;

    // AI.pas:53
    public AIPlayerActionClass GetRegisteredClasses(int index) => _registeredClasses.List[index];

    // AI.pas:63
    public void RegisterClasses(ReadOnlySpan<AIPlayerActionClass> classes)
    {
        for (int i = 0, last = classes.Length - 1; i <= last; i++)
        {
            if (_registeredClasses.IndexOf(classes[i]) != -1)
            {
                throw new InvalidOperationException("AIPlayerAction already Registered");
            }

            _ = _registeredClasses.Add(classes[i]);
        }

        _registeredClasses.Sort(CompareAIPlayerActionClasses);
    }

    // AI.pas:76
    public void UnregisterClasses(ReadOnlySpan<AIPlayerActionClass> classes)
    {
        for (int i = 0, last = classes.Length - 1; i <= last; i++)
        {
            _ = _registeredClasses.Remove(classes[i]);
        }
    }

    private static int CompareAIPlayerActionClasses(AIPlayerActionClass item1, AIPlayerActionClass item2) => item2.Priority - item1.Priority;
}
