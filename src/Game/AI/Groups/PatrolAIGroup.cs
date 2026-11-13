// Translated from IndependentAG.pas.
using System.Runtime.CompilerServices;
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// from, to, previous heading, strength
public partial class PatrolAIGroup : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:295
    public new const int ClassId = 136274;

    public MapCoordinate PatrolFrom;
    public MapCoordinate PatrolTo;
    public sbyte PreviousHeading;
    public int PatrolStrength;

    // IndependentAG.pas:304
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref PatrolFrom.Packed, 40);
        stream.Int32(ref PatrolTo.Packed, 41);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref PreviousHeading), 42);
        stream.Int32(ref PatrolStrength, 43);
    }

    // IndependentAG.pas:285
    public PatrolAIGroup(ECustomNode owner) : base(owner)
    {
        ProcessPriority = 1;
        RecruitmentPriority = 2;
        PatrolFrom = MapCoordinate.None;
        PatrolTo = MapCoordinate.None;
        PreviousHeading = 0;
    }

    // IndependentAG.pas:301
    public override EngineClass GroupControlClass() => EngineClass.Of<PatrolAIGroupControl>();

    // IndependentAG.pas:298
    public static new GroupBehavior Behavior => GroupBehavior.Patrol;

}
