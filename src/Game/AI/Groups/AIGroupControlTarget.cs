#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// references. info. data. location. moves. transporter. status. path strength. data A. data B
public sealed class AIGroupControlTarget(CustomNode owner) : EngineObject(owner), IOwned<AIGroupControlTargetList>, IOwnedSink<AIGroupControlTargetList>
{
    // A reference list holds the target and owns it only while the list saves or loads it, and
    // nothing else owns it. AcceptsOwner admits only that list.
    public new AIGroupControlTargetList Owner => GetOwner<AIGroupControlTargetList>();

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is AIGroupControlTargetList;

    void IOwnedSink<AIGroupControlTargetList>.SetOwner(AIGroupControlTargetList owner) => SetOwner(owner);

    private int _refCount = 1;

    public AITargetInfo Info { get; init; } = new AITargetInfo();

    public AITargetData Data { get; internal set; }
    public MapCoordinate Location { get; internal init; }

    public int MovePoints { get; init; }
    public int TransporterId { get; set; } = -1;
    public AIGroupTargetFlags Status { get; set; }
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
    public bool Validate(AIGroupControl control)
    {
        int x = Location.X;
        return x != -1 && GetEngine().StrategicMap.MapContainer.GetField(x, Location.Y, Location.MapLevel).GetAITarget(control, Info, data: out _);
    }

    // AIGroup.pas:666
    public int TargetGold => Data.Cost;

    // AIGroup.pas:671
    public int TargetMana => 0;

    // AIGroup.pas:676
    public string GetDebugInfo()
    {
        string text = $"Target({((int)Location.X).ToString(CultureInfo.InvariantCulture)},{((int)Location.Y).ToString(CultureInfo.InvariantCulture)},{((int)Location.MapLevel).ToString(CultureInfo.InvariantCulture)}): Prio:{Data.Priority.ToString(CultureInfo.InvariantCulture)}";
        if (TransporterId != -1)
        {
            text = $"{text}/Transporter({TransporterId.ToString(CultureInfo.InvariantCulture)})";
        }
        return text;
    }
}
