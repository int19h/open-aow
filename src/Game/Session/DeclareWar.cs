using Aow.Engine.Collections;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

// Kind 0: the source declares war on the target.
public sealed partial record DeclareWar : DiplomaticProposal
{
    public const byte Kind = 0;

    public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

    public override ProposalView View => ProposalView.Notice;

    public override int Icon => 9;

    public override string Name(DiplomaticAction action) => AbilityRules.TranslateResourceString(action.GetEngine(), GameText.DeclareWarText);

    public override string LogText(DiplomaticAction action) => AbilityRules.TranslateResourceString(action.GetEngine(), GameTables.WarText);

    public override int MiddleIcon(DiplomaticAction action) => 0x15;

    public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation) =>
        text.Proposing(relation == DiplomaticRelation.Unknown ? 151 : 100);

    public override void Sent(DiplomaticAction action)
    {
        action.ApplyRelationModifiers();
        StrategicMap.Current.Players.GetPlayers(action.Target).PlayerSeen((sbyte)(byte)action.Source);
        StrategicMap.Current.Players.SetDiplomaticRelation((sbyte)(byte)action.Source, (sbyte)(byte)action.Target, DiplomaticRelation.War);
        if (action.Source == StrategicMap.Current.SeatedPlayer)
        {
            var warDeclared = AbilityRules.TranslateResourceString(action.GetEngine(), s_warDeclaredText);
            StrategicMap.Current.ShowMessage(warDeclared);
        }

        Broadcast(action, -1, action.Target);
    }

    public override void AnswerAI(DiplomaticAction action, sbyte player, DiplomaticRelation relation, ByteList idealPeace, ByteList idealWar) =>
        action.UpdateAIDiplomaticActionsLog(player);

    public override int RelationModifier(DiplomaticAction action, sbyte player, sbyte race)
    {
        if (player == action.Source && Relation(action.Source, action.Target) != DiplomaticRelation.War)
        {
            int modifier = Relation(action.Source, action.Target) == DiplomaticRelation.Alliance ? -60 : -20;
            return RaceModifier(action.Target, race, modifier);
        }

        if (player == action.Target && Relation(action.Source, action.Target) != DiplomaticRelation.War && StrategicMap.Current.Players.GetPlayers(action.Source).PlayerRace == race)
        {
            return -20;
        }

        return 0;
    }

    private static readonly string s_warDeclaredText = "War declared";
}
