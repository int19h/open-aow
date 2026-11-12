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
        _refCount = 1;
    }

    private int _refCount;

    public MapCoordinate Location { get; set; }
    public AIGroup Group { get; private set; }
    public int MovePoints { get; set; }
    public int TransporterId { get; set; }
    public int PathStrength { get; set; }

    // AIGroup.pas:756
    public override void Dispose()
    {
        SetGroup(null);
        base.Dispose();
    }

    // AIGroup.pas:762
    public override int AddRef()
    {
        _refCount += 1;
        return _refCount;
    }

    // AIGroup.pas:768
    public override int Release()
    {
        _refCount -= 1;
        int result = _refCount;
        if (_refCount == 0)
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
