#nullable enable
using Aow.Game.Host;

namespace Aow.Game.Commands;

public partial class MoveTokenEvent
{
    public virtual void InitializeMove()
    {
        MoveIndex = Path.Count - 1;
        HeadlessHooks.BeforeMove?.Invoke(this);
    }
}
