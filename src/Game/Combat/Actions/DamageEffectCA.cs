// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// FDamageType
public partial class DamageEffectCA : SingleTargetCA
{
    // Combat2.pas:876
    public new const int ClassID = 131762;

    public DamageEffectCA(ECustomNode owner) : base(owner)
    { }

    public DamageType FDamageType;

    // Combat2.pas:879
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref FDamageType, 1), 1, 15);
    }

    // Combat2.pas:890
    public override void Execute(Battle Combat)
    {
        CombatObject Target;
        CombatObject Source;
        // { Use the incoming combat. }
        Target = Combat.FCombatData.FindID(FTargetID);
        if (!Target.FStatus.Contains(CombatObjectStatusElement.Destroyed))
        {
            Source = Combat.FCombatData.FindID(FSourceID);
            Target.ExecuteDamageEffects(Set<DamageType>.Of(FDamageType));
            if (Target.FStatus.Contains(CombatObjectStatusElement.Destroyed))
            {
                Source.SetExperience(Source.GetExperience() + Target.GetLevel());
            }
        }
    }

    // Combat2.pas:926
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        if (Obj.FID == FTargetID)
        {
            switch (FDamageType)
            {
                case DamageType.Fire:
                    Obj.Show(Frame, Direction, X, Y, Surface);
                    AoWHSSet.Current.FAbilityControl.GetAbility(127).FImageSequences.Show(0, X + 32, Y + 32, Viewer.FTime, Surface);
                    break;
                case DamageType.Poison:
                    Obj.Show(Frame, Direction, X, Y, Surface);
                    AoWHSSet.Current.FAbilityControl.GetAbility(96).FImageSequences.Show(0, X + 32, Y + 32, Viewer.FTime, Surface);
                    break;
                case DamageType.Cold:
                    Obj.Show(Frame, Direction, X, Y, Surface);
                    AoWHSSet.Current.FAbilityControl.GetAbility(95).FImageSequences.Show(1, X + 32, Y + 32, Viewer.FTime, Surface);
                    break;
                case DamageType.Death:
                    Obj.Show(Frame, Direction, X, Y, Surface);
                    AoWHSSet.Current.FAbilityControl.GetAbility(93).FImageSequences.Show(0, X + 32, Y + 32, Viewer.FTime, Surface);
                    break;
                case DamageType.Holy:
                    Obj.Show(Frame, Direction, X, Y, Surface);
                    AoWHSSet.Current.FAbilityControl.GetAbility(98).FImageSequences.Show(0, X + 32, Y + 32, Viewer.FTime, Surface);
                    break;
                default:
                    Obj.Show(Frame, Direction, X, Y, Surface);
                    break;
            }
        }
        else
        {
            Obj.Show(Frame, Direction, X, Y, Surface);
        }
    }

    // Combat2.pas:882
    public void Initialize(byte SourceID, byte TargetID, byte DamageType, bool Defensive)
    {
        FSourceID = SourceID;
        FTargetID = TargetID;
        FDamageType = (DamageType)DamageType;
        if (Defensive)
        {
            FMode = FMode + Set<SingleTargetModeElement>.Of(SingleTargetModeElement.Defensive);
        }
    }

    public override bool Play(CombatViewer viewer)
    {
        if (viewer.FDelay * 2 == viewer.FTime) Execute(viewer.FCombat);
        return viewer.FDelay * 2 > viewer.FTime;
    }
}
