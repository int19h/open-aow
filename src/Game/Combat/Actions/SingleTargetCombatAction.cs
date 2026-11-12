// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat.Actions;

// Mode. SourceId. TargetId
public partial class SingleTargetCombatAction : CombatAction
{

    public SingleTargetCombatAction(ECustomNode owner) : base(owner)
    {
    }

    public Set<SingleTargetModeElement> Mode;
    public byte SourceId;
    public byte TargetId;

    // Combat2.pas:732
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref SourceId, 10);
        stream.rwByte(ref TargetId, 11);
        RwSet(ref Mode, 1, 12, stream.rwData, 0);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Byte("SourceID", ref SourceId);
        persister.Byte("TargetID", ref TargetId);
        persister.Set("Mode", ref Mode);
    }

    // Combat2.pas:749
    public override void ShowFieldObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        if (!Mode.Contains(SingleTargetModeElement.Defensive))
        {
            if (objectValue.Id == SourceId)
            {
                GamePack.Current.CombatImages[2].Show(X + 15, Y + 18, surface);
            }
        }
        else
        {
            if (objectValue.Id == TargetId)
            {
                GamePack.Current.CombatImages[2].Show(X + 15, Y + 18, surface);
            }
        }

        ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (!Mode.Contains(SingleTargetModeElement.Defensive))
        {
            if (objectValue.Id == TargetId)
            {
                GamePack.Current.CombatImages[3].Show(X + 15, Y + 18, surface);
            }
        }
        else
        {
            if (objectValue.Id == SourceId)
            {
                GamePack.Current.CombatImages[3].Show(X + 15, Y + 18, surface);
            }
        }
    }

    // Combat2.pas:764
    public override void ShowFightingObjects(CombatViewer viewer, int frame, int X, int Y, in LockedSurface surface)
    {
        CombatObject source;
        CombatObject target;
        source = viewer.Combat.CombatData.FindId(SourceId);
        target = viewer.Combat.CombatData.FindId(TargetId);
        if (!Mode.Contains(SingleTargetModeElement.Defensive))
        {
            viewer.DefaultShowFightingObjects(source, target, frame, X, Y, surface);
        }
        else
        {
            viewer.DefaultShowFightingObjects(target, source, frame, X, Y, surface);
        }
    }

    // Combat2.pas:740
    public bool GetDefensive() => Mode.Contains(SingleTargetModeElement.Defensive);

    // Combat2.pas:743
    public void SetDefensive(bool defensive)
    {
        if (defensive)
        {
            Mode = Mode + Set<SingleTargetModeElement>.Of(SingleTargetModeElement.Defensive);
        }
        else
        {
            Mode = Mode - Set<SingleTargetModeElement>.Of(SingleTargetModeElement.Defensive);
        }
    }
}
