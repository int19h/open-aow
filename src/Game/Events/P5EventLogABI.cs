// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Events;

// { P5: AI, diplomacy, campaign and turn movement. }
// ABI views for the still-unmarked EventLog/P2 override declarations. Never instantiated.
// EventLog JoinEventLog
public partial class P5EventLogABI : EObject
{
    public P5EventLogABI(ECustomNode owner) : base(owner)
    { }

    // Combat.pas:951
    public virtual void Slot4C()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:952
    public virtual void Slot50()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:953
    public virtual void Slot54()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:954
    public virtual void Slot58()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:955
    public virtual void Slot5C()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:956
    public virtual void Slot60()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:957
    public virtual void Slot64()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:958
    public virtual void Slot68()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:959
    public virtual void Slot6C()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat.pas:960
    public virtual bool Execute(bool Immediate, bool Auto, object Data, object Done)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}
