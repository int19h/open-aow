using Aow.Engine.Collections;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

// Kind 0: the source declares war on the target.
public sealed record DeclareWar : DiplomaticProposal
{
    public const byte Kind = 0;

    public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

    public override ProposalView View => ProposalView.Notice;

    public override int Icon => 9;

    public override string Name(DiplomaticAction action) => AbilityRules.TranslateResourceString(action.GetEngine(), S: GameText.DeclareWarText);

    public override string LogText(DiplomaticAction action) => AbilityRules.TranslateResourceString(action.GetEngine(), S: GameTables.WarText);

    public override int MiddleIcon(DiplomaticAction action) => 0x15;

    public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation)
        => text.Proposing(relation == DiplomaticRelation.Unknown ? 151 : 100);

    public override void Sent(DiplomaticAction action)
    {
        action.ApplyRelationModifiers();
        StrategicMap.Current.Players.GetPlayers(action.Target).PlayerSeen((sbyte)(byte)action.Source);
        StrategicMap.Current.Players.SetDiplomaticRelation((sbyte)(byte)action.Source, otherPlayer: (sbyte)(byte)action.Target, relation: DiplomaticRelation.War);
        if (action.Source == StrategicMap.Current.SeatedPlayer)
        {
            string warDeclared = AbilityRules.TranslateResourceString(action.GetEngine(), S: s_warDeclaredText);
            StrategicMap.Current.ShowMessage(warDeclared);
        }

        Broadcast(action, exclude: -1, viewer: action.Target);
    }

    public override void AnswerAI(DiplomaticAction action, sbyte player, DiplomaticRelation relation, ByteList idealPeace, ByteList idealWar)
        => action.UpdateAIDiplomaticActionsLog(player);

    public override int RelationModifier(DiplomaticAction action, sbyte player, sbyte race)
    {
        return player == action.Source && Relation(action.Source, second: action.Target) != DiplomaticRelation.War
? RaceModifier(modifier: Relation(action.Source, second: action.Target) == DiplomaticRelation.Alliance
                ? -60
                : -20, other: action.Target, race: race)
            : player == action.Target && Relation(action.Source, second: action.Target) != DiplomaticRelation.War && StrategicMap.Current.Players.GetPlayers(action.Source).PlayerRace == race
            ? -20
            : 0;
    }

    private static readonly string s_warDeclaredText = "War declared";
}
