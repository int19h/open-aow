// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat.Actions;

// FMode. FSourceID. FTargetID
public partial class SingleTargetCA : CombatAction
{

    public SingleTargetCA(ECustomNode owner) : base(owner)
    {
    }

    public Set<SingleTargetModeElement> FMode;
    public byte FSourceID;
    public byte FTargetID;

    // Combat2.pas:732
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FSourceID, 10);
        Stream.rwByte(ref FTargetID, 11);
        RwSet(ref FMode, 1, 12, Stream.rwData, 0);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Byte("SourceID", ref FSourceID);
        persister.Byte("TargetID", ref FTargetID);
        persister.Set("Mode", ref FMode);
    }

    // Combat2.pas:749
    public override void ShowFieldObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        if (!FMode.Contains(SingleTargetModeElement.Defensive))
        {
            if (Obj.FID == FSourceID)
            {
                AoWHSSet.Current.FCombatImages[2].Show(X + 15, Y + 18, Surface);
            }
        }
        else
        {
            if (Obj.FID == FTargetID)
            {
                AoWHSSet.Current.FCombatImages[2].Show(X + 15, Y + 18, Surface);
            }
        }

        ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        if (!FMode.Contains(SingleTargetModeElement.Defensive))
        {
            if (Obj.FID == FTargetID)
            {
                AoWHSSet.Current.FCombatImages[3].Show(X + 15, Y + 18, Surface);
            }
        }
        else
        {
            if (Obj.FID == FSourceID)
            {
                AoWHSSet.Current.FCombatImages[3].Show(X + 15, Y + 18, Surface);
            }
        }
    }

    // Combat2.pas:764
    public override void ShowFightingObjects(CombatViewer Viewer, int Frame, int X, int Y, in SurfaceDesc Surface)
    {
        CombatObject Source;
        CombatObject Target;
        Source = Viewer.FCombat.FCombatData.FindID(FSourceID);
        Target = Viewer.FCombat.FCombatData.FindID(FTargetID);
        if (!FMode.Contains(SingleTargetModeElement.Defensive))
        {
            Viewer.DefaultShowFightingObjects(Source, Target, Frame, X, Y, Surface);
        }
        else
        {
            Viewer.DefaultShowFightingObjects(Target, Source, Frame, X, Y, Surface);
        }
    }

    // Combat2.pas:740
    public bool GetDefensive() => FMode.Contains(SingleTargetModeElement.Defensive);

    // Combat2.pas:743
    public void SetDefensive(bool Defensive)
    {
        if (Defensive)
        {
            FMode = FMode + Set<SingleTargetModeElement>.Of(SingleTargetModeElement.Defensive);
        }
        else
        {
            FMode = FMode - Set<SingleTargetModeElement>.Of(SingleTargetModeElement.Defensive);
        }
    }
}
