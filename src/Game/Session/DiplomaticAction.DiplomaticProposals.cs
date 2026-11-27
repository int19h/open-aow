#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Rtl;

namespace Aow.Game.Session;

public partial class DiplomaticAction
{
    // Replaces the message with the default text of the kind for Reaction (Player.PAS:1188,
    // Diplomacy.pas:826, 950).
    public void RetrieveDefaultMessage(PlayerId? source, int target, PlayerId? other, DiplomaticActionReaction reaction)
    {
        using DelphiStringList text = new();
        RetrieveDefaultDiplomaticActionMessage(source, target: PlayerId.FromOriginal(target), other: PlayerIdExtensions.ToOriginal(other), Proposal, reaction, text, engine: GetEngine());
        Proposal = Proposal with { Message = DiplomaticProposal.Lines(text) };
    }

    // Moves every race's relation to source and target by the proposal's modifiers (Diplomacy.pas:826, 992).
    public void ApplyRelationModifiers()
    {
        GetEngine().StrategicMap.LockRelationsChanged();
        for (int i = 0, last = GetEngine().StrategicMap.Races.Count - 1; i <= last; i++)
        {
            int playerRelationValue = GetEngine().StrategicMap.Races.GetRace(i).GetPlayerRelationValue(Source);
            int relationModifier = GetRelationModifier(Source, race: i.TruncateToSByte());
            GetEngine().StrategicMap.Races.GetRace(i).SetPlayerRelationValue(Source, value: relationModifier + playerRelationValue);
            int playerRelationValue2 = GetEngine().StrategicMap.Races.GetRace(i).GetPlayerRelationValue(Target);
            int relationModifier2 = GetRelationModifier(Target, race: i.TruncateToSByte());
            GetEngine().StrategicMap.Races.GetRace(i).SetPlayerRelationValue(Target, value: relationModifier2 + playerRelationValue2);
        }

        GetEngine().StrategicMap.UnlockRelationsChanged();
    }
}
