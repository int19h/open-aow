
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Session;

public partial class DiplomaticAction
{
    // Replaces the message with the default text of the kind for Reaction (Player.PAS:1188,
    // Diplomacy.pas:826, 950).
    public void RetrieveDefaultMessage(sbyte Source, sbyte Target, sbyte Other, DiplomaticActionReaction Reaction)
    {
        using var text = new StringList();
        RetrieveDefaultDiplomaticActionMessage(Source, Target, Other, Proposal, Reaction, text);
        Proposal = Proposal with { Message = DiplomaticProposal.Lines(text) };
    }

    // Moves every race's relation to source and target by the proposal's modifiers (Diplomacy.pas:826, 992).
    public void ApplyRelationModifiers()
    {
        AoWHSMap.Current.LockRelationsChanged();
        for (int I = 0, last = AoWHSMap.Current.FRaces.Count - 1; I <= last; I++)
        {
            var playerRelationValue = AoWHSMap.Current.FRaces.GetRace(I).GetPlayerRelationValue(FSource);
            var relationModifier = GetRelationModifier(FSource, (sbyte)I);
            AoWHSMap.Current.FRaces.GetRace(I).SetPlayerRelationValue(FSource, relationModifier + playerRelationValue);
            var playerRelationValue2 = AoWHSMap.Current.FRaces.GetRace(I).GetPlayerRelationValue(FTarget);
            var relationModifier2 = GetRelationModifier(FTarget, (sbyte)I);
            AoWHSMap.Current.FRaces.GetRace(I).SetPlayerRelationValue(FTarget, relationModifier2 + playerRelationValue2);
        }

        AoWHSMap.Current.UnlockRelationsChanged();
    }
}
