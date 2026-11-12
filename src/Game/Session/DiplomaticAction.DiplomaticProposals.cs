
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Session;

public partial class DiplomaticAction
{
    // Replaces the message with the default text of the kind for Reaction (Player.PAS:1188,
    // Diplomacy.pas:826, 950).
    public void RetrieveDefaultMessage(sbyte source, sbyte target, sbyte other, DiplomaticActionReaction reaction)
    {
        using var text = new StringList();
        RetrieveDefaultDiplomaticActionMessage(source, target, other, Proposal, reaction, text);
        Proposal = Proposal with { Message = DiplomaticProposal.Lines(text) };
    }

    // Moves every race's relation to source and target by the proposal's modifiers (Diplomacy.pas:826, 992).
    public void ApplyRelationModifiers()
    {
        AoWHSMap.Current.LockRelationsChanged();
        for (int I = 0, last = AoWHSMap.Current.Races.Count - 1; I <= last; I++)
        {
            var playerRelationValue = AoWHSMap.Current.Races.GetRace(I).GetPlayerRelationValue(Source);
            var relationModifier = GetRelationModifier(Source, (sbyte)I);
            AoWHSMap.Current.Races.GetRace(I).SetPlayerRelationValue(Source, relationModifier + playerRelationValue);
            var playerRelationValue2 = AoWHSMap.Current.Races.GetRace(I).GetPlayerRelationValue(Target);
            var relationModifier2 = GetRelationModifier(Target, (sbyte)I);
            AoWHSMap.Current.Races.GetRace(I).SetPlayerRelationValue(Target, relationModifier2 + playerRelationValue2);
        }

        AoWHSMap.Current.UnlockRelationsChanged();
    }
}
