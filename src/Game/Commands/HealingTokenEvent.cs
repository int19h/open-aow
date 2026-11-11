// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// (unit IDs, players)
public partial class HealingTokenEvent : TokenEvent
{
    public HealingTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131826;

    public int FSourceID;
    public byte FSourcePlayer;
    public int FTargetID;
    public byte FTargetPlayer;

    // AoWAb.PAS:3853
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FSourceID, 25);
        Stream.rwByte(ref FSourcePlayer, 26);
        Stream.rwInteger(ref FTargetID, 27);
        Stream.rwByte(ref FTargetPlayer, 28);
    }

    // AoWAb.PAS:3861
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        ExecuteResult Result;
        Troop Source;
        Troop Target;
        HealingAbility Ability;
        ImageSequenceAnimation Anim;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.Execute(TE);
        Source = AoWHSMap.Current.FUnitControl.FindUnit(FSourceID);
        Target = AoWHSMap.Current.FUnitControl.FindUnit(FTargetID);
        if (Source != null && (byte)Source.FPlayer == FSourcePlayer && Target != null && (byte)Target.FPlayer == FTargetPlayer)
        {
            Ability = (HealingAbility)AoWHSSet.Current.FAbilityControl.GetAbility(47);
            if (Ability.GetEnabled(Source))
            {
                Ability.SetEnabled(Source, false);
                Ability.HealUnit(Target);
                Target.GetLocation(ref X, ref Y, ref L);
                AoWHSMap.Current.ValidateObservation((sbyte)FSourcePlayer, X, Y, L, 0);
                if (AoWHSMap.Current.Visible(X, Y, L, 0))
                {
                    Anim = new ImageSequenceAnimation(null);
                    Anim.SetImageSequence(Ability.FImageSequences.GetImageSequence(0));
                    AoWHSMap.Current.PlaceHX(Anim, X, Y, L);
                }

                if ((byte)AoWHSMap.Current.FSeatedPlayer == FSourcePlayer || AoWHSMap.Current.Watching(X, Y, L, 0))
                {
                    Ability.FSFXLibrary.Play(0);
                }
            }

            Result = ExecuteResult.Done;
        }
        else
        {
            Result = ExecuteResult.DoneHalt;
        }

        return Result;
    }

    // NetworkE.pas:230

}
