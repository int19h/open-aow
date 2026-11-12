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
        RefCount = 1;
    }

    public int RefCount;
    public MapCoordinate Location;
    public AIGroup Group;
    public int MovePoints;
    public int TransporterId;
    public int PathStrength;

    // AIGroup.pas:756
    public override void Dispose()
    {
        SetGroup(null);
        base.Dispose();
    }

    // AIGroup.pas:762
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // AIGroup.pas:768
    public override int Release()
    {
        RefCount -= 1;
        int result = RefCount;
        if (RefCount == 0)
        {
            Dispose();
        }

        return result;
    }

    // AIGroup.pas:788
    public virtual bool Validate(AIGroupControl control)
    {
        bool result;
        MapCoordinate location = default;
        if (Group.Count != 0)
        {
            location.Packed = Group.GetLocation();
            result = location.X == Location.X && location.Y == Location.Y && location.MapLevel == Location.MapLevel && Group.Player == control.Group.Player;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AIGroup.pas:775
    public void SetGroup(AIGroup group)
    {
        AIGroup oldGroup;
        oldGroup = Group;
        if (oldGroup != group)
        {
            if (oldGroup != null)
            {
                oldGroup.Release();
            }

            Group = group;
            if (Group != null)
            {
                Group.AddRef();
            }
        }
    }

    // intf_P5_07.inc:159

}
