// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

// references, leader, mode, original leader, items, done event
public partial class LeaderSetupControl : EngineObject
{

    // Player.PAS:1755
    // { Return the final search index. }
    public LeaderSetupControl(ECustomNode owner) : base(owner)
    {
        Items = new Int32List();
        OriginalLeader = new Leader(null);
        Leader = new Leader(null);
        RefCount = 1;
        Mode = Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Look, LeaderSetupModeElement.Settings, LeaderSetupModeElement.Spheres);
        OnDone = null;
    }

    public int RefCount;
    public Leader Leader;
    public Set<LeaderSetupModeElement> Mode;
    public Leader OriginalLeader;
    public Int32List Items;
    public LeaderSetupStatus Status;
    public NotifyEvent<LeaderSetupControl> OnDone;

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
        RefCount += 1;
        return RefCount;
    }

    // Player.PAS:1780
    public override int Release()
    {
        RefCount -= 1;
        int result = RefCount;
        if (result == 0)
        {
            Dispose();
        }

        return result;
    }

    // Player.PAS:1787
    public virtual bool Done()
    {
        if (OnDone != null)
        {
            OnDone(this);
        }

        return true;
    }

    public enum LeaderSetupStatus : byte
    {
        None = 0, Busy = 1, Done = 2, Canceled = 3
    }

    // intf_P1_03.inc:99

}
