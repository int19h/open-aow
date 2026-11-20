#nullable disable
// Translated from Engine.pas.
using Aow.Engine.Nodes;
using Aow.Game;
using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;

namespace Aow.Engine.Storage;

// Visits every string of an object tree through TranslateString and TranslateStrings. Nothing
// is read or written. The visit is MainPersist, and the persister is not loading, so the visit
// clears nothing.
public partial class TranslateVisit(GameEngine engine) : IDisposable
{
    public delegate void TranslateErrorHandler(string message, ref bool handled);

    private TranslateErrorHandler _onError;

    public GameEngine Engine { get; } = engine;

    // Engine.pas:10831
    public virtual void Dispose()
    {
    }

    public void SetErrorHandler(TranslateErrorHandler handler) => _onError = handler;

    // Engine.pas:10865
    // A failure reports "Error reading object" and rethrows unless the handler continues.
    public virtual void Object(EngineObject objectValue, int id)
    {
        bool canContinue = false;
        try
        {
            objectValue.MainPersist(new TranslatePersister(this, node: objectValue));
        }
        catch
        {
            if (_onError is not null)
            {
                int classId = objectValue.EngineClass.ClassId;
                _onError($"Error reading object (cid:{IntToStr(classId)})", ref canContinue);
            }

            if (!canContinue)
            {
                throw;
            }
        }
    }

    // Engine.pas:10887
    public virtual void Children(CustomNode node, int id)
    {
        for (int i = 0, last = node.Count - 1; i <= last; i++)
        {
            EngineObject child = node.Children[i];
            if (child is not null && child.Owner == node && !child.GetControlStyle().Contains(EngineServices.Shared) && child.GetControlStyle().Contains(EngineServices.Stored))
            {
                Object(child, i);
            }
        }
    }

    // Engine.pas:10901
    public virtual void TranslateStrings(DelphiStringCollection lines)
    {
    }

    // Engine.pas:10905
    public virtual void TranslateString(ref string value)
    {
    }
}
