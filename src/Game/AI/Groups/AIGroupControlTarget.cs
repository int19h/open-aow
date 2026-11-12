// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// references. info. data. location. moves. transporter. status. path strength. data A. data B
public partial class AIGroupControlTarget : EngineObject
{
    // AIGroup.pas:635
    public AIGroupControlTarget(ECustomNode owner) : base(owner)
    {
        RefCount = 1;
        TransporterId = -1;
    }

    public int RefCount;
    public AITargetInfo Info;
    public AITargetData Data;
    public MapCoordinate Location;
    public int MovePoints;
    public int TransporterId;
    public Set<AIGroupTargetFlags> Status;
    public int PathStrength;
    public int DataA;
    public int DataB;

    // AIGroup.pas:642
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // AIGroup.pas:648
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

    // AIGroup.pas:655
    public virtual bool Validate(AIGroupControl control)
    {
        bool result;
        AITargetData data = default;
        sbyte X;
        X = Location.X;
        if (X != -1)
        {
            result = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Location.Y, Location.MapLevel)).GetAITarget(control, Info, ref data);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AIGroup.pas:666
    public int GetTargetGold() => Data.Cost;

    // AIGroup.pas:671
    public int GetTargetMana() => 0;

    // AIGroup.pas:676
    public void GetDebugInfo(ref string text)
    {
        text = "Target(" + IntToStr(Location.X) + "," + IntToStr(Location.Y) + "," + IntToStr(Location.MapLevel) + "): Prio:" + IntToStr(Data.Priority);
        if (TransporterId != -1)
        {
            text = text + "/Transporter(" + IntToStr(TransporterId) + ")";
        }
    }

    // intf_P5_07.inc:124

}
