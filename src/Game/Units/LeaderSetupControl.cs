#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

// references, leader, mode, original leader, items, done event
public class LeaderSetupControl : EngineObject
{

    // Player.PAS:1755
    // { Return the final search index. }
    public LeaderSetupControl(CustomNode? owner) : base(owner)
    {
        Items = new Int32List();
        OriginalLeader = new Leader(null) { Holder = CreationHolder() };
        Leader = new Leader(null) { Holder = CreationHolder() };
        _refCount = 1;
        Mode = LeaderSetupModes.Look | LeaderSetupModes.Settings | LeaderSetupModes.Spheres;
        _done = null;
    }

    private int _refCount;

    public Leader Leader { get; }
    public LeaderSetupModes Mode { get; set; }
    public Leader OriginalLeader { get; init; }
    public Int32List Items { get; }
    public LeaderSetupStatus Status { get; set; }

    private NotifyEvent<LeaderSetupControl>? _done;

    internal void SetDoneHandler(NotifyEvent<LeaderSetupControl> handler) => _done = handler;

    // Player.PAS:1766
    public override void Dispose()
    {
        OriginalLeader?.Dispose();
        Leader?.Dispose();
        Items?.Dispose();
        base.Dispose();
    }

    // Player.PAS:1774
    public override int AddRef()
    {
        _refCount += 1;
        return _refCount;
    }

    // Player.PAS:1780
    public override int Release()
    {
        _refCount -= 1;
        int result = _refCount;
        if (result == 0)
        {
            Dispose();
        }

        return result;
    }

    // Player.PAS:1787
    public virtual bool Done()
    {
        _done?.Invoke(this);

        return true;
    }

    public enum LeaderSetupStatus : byte
    {
        None = 0, Busy = 1, Done = 2, Canceled = 3
    }
}
