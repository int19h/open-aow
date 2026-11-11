// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.World.Map;

namespace Aow.Game.AI;

// references, location, group, moves, transporter, path strength
public partial class AIRecruitmentGroup : EngineObject
{
    // AIGroup.pas:750
    public AIRecruitmentGroup(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
    }

    public int FRefCount;
    public MapCoordinate FLocation;
    public AIGroup FGroup;
    public int FMovePoints;
    public int FTransporterID;
    public int FPathStrength;

    // AIGroup.pas:756
    public override void Dispose()
    {
        SetGroup(null);
        base.Dispose();
    }

    // AIGroup.pas:762
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // AIGroup.pas:768
    public override int Release()
    {
        FRefCount -= 1;
        int Result = FRefCount;
        if (FRefCount == 0)
        {
            Dispose();
        }

        return Result;
    }

    // AIGroup.pas:788
    public virtual bool Validate(AIGroupControl Control)
    {
        bool Result;
        MapCoordinate Location = default;
        if (FGroup.Count != 0)
        {
            Location.Packed = FGroup.GetLoc();
            Result = Location.X == FLocation.X && Location.Y == FLocation.Y && Location.L == FLocation.L && FGroup.FPlayer == Control.FGroup.FPlayer;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AIGroup.pas:775
    public void SetGroup(AIGroup Group)
    {
        AIGroup OldGroup;
        OldGroup = FGroup;
        if (OldGroup != Group)
        {
            if (OldGroup != null)
            {
                OldGroup.Release();
            }

            FGroup = Group;
            if (FGroup != null)
            {
                FGroup.AddRef();
            }
        }
    }

    // intf_P5_07.inc:159

}
