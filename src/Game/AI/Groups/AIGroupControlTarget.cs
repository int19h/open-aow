// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// references. info. data. location. moves. transporter. status. path strength. data A. data B
public partial class AIGroupControlTarget : EngineObject, IOwned<AIGroupControlTargetList>, IOwnedSink<AIGroupControlTargetList>
{
    // A reference list holds the target and owns it only while the list saves or loads it, and
    // nothing else owns it. AcceptsOwner admits only that list, so the cast never fails.
    public new AIGroupControlTargetList Owner { get => (AIGroupControlTargetList)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is AIGroupControlTargetList;

    void IOwnedSink<AIGroupControlTargetList>.SetOwner(AIGroupControlTargetList owner) => SetOwner(owner);

    // AIGroup.pas:635
    public AIGroupControlTarget(ECustomNode owner) : base(owner)
    {
        _refCount = 1;
        TransporterId = -1;
    }

    private int _refCount;

    public AITargetInfo Info { get; set; }

    public AITargetData Data;
    public MapCoordinate Location;

    public int MovePoints { get; set; }
    public int TransporterId { get; set; }
    public Set<AIGroupTargetFlags> Status { get; set; }
    public int PathStrength { get; set; }
    public int DataA { get; set; }
    public int DataB { get; set; }

    // AIGroup.pas:642
    public override int AddRef()
    {
        _refCount += 1;
        return _refCount;
    }

    // AIGroup.pas:648
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

    // AIGroup.pas:655
    public virtual bool Validate(AIGroupControl control)
    {
        bool result;
        AITargetData data = default;
        sbyte X;
        X = Location.X;
        if (X != -1)
        {
            result = StrategicMap.Current.MapContainer.GetField(X, Location.Y, Location.MapLevel).GetAITarget(control, Info, ref data);
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
