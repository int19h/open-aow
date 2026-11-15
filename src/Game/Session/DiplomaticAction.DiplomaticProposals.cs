
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Session;

public partial class DiplomaticAction
{
    // Replaces the message with the default text of the kind for Reaction (Player.PAS:1188,
    // Diplomacy.pas:826, 950).
    public void RetrieveDefaultMessage(sbyte source, sbyte target, sbyte other, DiplomaticActionReaction reaction)
    {
        using StringList text = new();
        RetrieveDefaultDiplomaticActionMessage(source, target, other, Proposal, reaction, text, engine: GetEngine());
        Proposal = Proposal with { Message = DiplomaticProposal.Lines(text) };
    }

    // Moves every race's relation to source and target by the proposal's modifiers (Diplomacy.pas:826, 992).
    public void ApplyRelationModifiers()
    {
        StrategicMap.Current.LockRelationsChanged();
        for (int i = 0, last = StrategicMap.Current.Races.Count - 1; i <= last; i++)
        {
            int playerRelationValue = StrategicMap.Current.Races.GetRace(i).GetPlayerRelationValue(Source);
            int relationModifier = GetRelationModifier(Source, (sbyte)i);
            StrategicMap.Current.Races.GetRace(i).SetPlayerRelationValue(Source, relationModifier + playerRelationValue);
            int playerRelationValue2 = StrategicMap.Current.Races.GetRace(i).GetPlayerRelationValue(Target);
            int relationModifier2 = GetRelationModifier(Target, (sbyte)i);
            StrategicMap.Current.Races.GetRace(i).SetPlayerRelationValue(Target, relationModifier2 + playerRelationValue2);
        }

        StrategicMap.Current.UnlockRelationsChanged();
    }
}
