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
        FRefCount = 1;
        FTransporterID = -1;
    }

    public int FRefCount;
    public AITargetInfo FInfo;
    public AITargetData FData;
    public MapCoordinate FLocation;
    public int FMovePoints;
    public int FTransporterID;
    public Set<AIGroupTargetFlags> FStatus;
    public int FPathStrength;
    public int FDataA;
    public int FDataB;

    // AIGroup.pas:642
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // AIGroup.pas:648
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

    // AIGroup.pas:655
    public virtual bool Validate(AIGroupControl Control)
    {
        bool Result;
        AITargetData Data = default;
        sbyte X;
        X = FLocation.X;
        if (X != -1)
        {
            Result = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, FLocation.Y, FLocation.L)).GetAITarget(Control, FInfo, ref Data);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AIGroup.pas:666
    public int GetTargetGold() => FData.Cost;

    // AIGroup.pas:671
    public int GetTargetMana() => 0;

    // AIGroup.pas:676
    public void GetDebugInfo(ref string Text)
    {
        Text = "Target(" + IntToStr(FLocation.X) + "," + IntToStr(FLocation.Y) + "," + IntToStr(FLocation.L) + "): Prio:" + IntToStr(FData.Priority);
        if (FTransporterID != -1)
        {
            Text = Text + "/Transporter(" + IntToStr(FTransporterID) + ")";
        }
    }

    // intf_P5_07.inc:124

}
