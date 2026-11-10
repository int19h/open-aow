// Translated from IndependentAG.pas.
using System.Runtime.CompilerServices;
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

namespace Aow.Game.Ai.Groups;

// from, to, previous heading, strength
public partial class PatrolAG : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:295
    public new const int ClassID = 136274;

    public XYL FPatrolFrom;
    public XYL FPatrolTo;
    public sbyte FPreviousHeading;
    public int FPatrolStrength;

    // IndependentAG.pas:304
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FPatrolFrom.Packed, 40);
        Stream.rwInteger(ref FPatrolTo.Packed, 41);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPreviousHeading), 42);
        Stream.rwInteger(ref FPatrolStrength, 43);
    }

    // IndependentAG.pas:285
    public PatrolAG(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 1;
        FRecruitmentPriority = 2;
        FPatrolFrom.Packed = 16777215;
        FPatrolTo.Packed = 16777215;
        FPreviousHeading = 0;
    }

    // IndependentAG.pas:301
    public override EClass GroupControlClass() => EClass.Of<PatrolAGC>();

    // IndependentAG.pas:298
    public static new GroupBehavior Behavior => GroupBehavior.Patrol;

}
