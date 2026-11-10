// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Units;

// references, leader, mode, original leader, items, done event
public partial class LeaderSetupControl : EObject
{

    // Player.PAS:1755
    // { Return the final search index. }
    public LeaderSetupControl(ECustomNode owner) : base(owner)
    {
        FItems = new IntegerList();
        FOriginalLeader = new Leader(null);
        FLeader = new Leader(null);
        FRefCount = 1;
        FMode = Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Look, LeaderSetupModeElement.Settings, LeaderSetupModeElement.Spheres);
        FOnDone = null;
    }

    public int FRefCount;
    public Leader FLeader;
    public Set<LeaderSetupModeElement> FMode;
    public Leader FOriginalLeader;
    public IntegerList FItems;
    public LeaderSetupStatus FStatus;
    public NotifyEvent<LeaderSetupControl> FOnDone;

    // Player.PAS:1766
    public override void Dispose()
    {
        FOriginalLeader?.Dispose();
        FLeader?.Dispose();
        FItems?.Dispose();
        base.Dispose();
    }

    // Player.PAS:1774
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // Player.PAS:1780
    public override int Release()
    {
        FRefCount -= 1;
        int Result = FRefCount;
        if (Result == 0)
        {
            Dispose();
        }

        return Result;
    }

    // Player.PAS:1787
    public virtual bool Done()
    {
        if (FOnDone != null)
        {
            FOnDone(this);
        }

        return true;
    }

    public enum LeaderSetupStatus : byte
    {
        None = 0, Busy = 1, Done = 2, Canceled = 3
    }

    // intf_P1_03.inc:99

}
