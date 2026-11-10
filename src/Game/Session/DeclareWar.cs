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

    public override string Name(DiplomaticAction Action) => AbilityRules.TranslateRStr(GameText.P5DeclareWarRStr);

    public override string LogText(DiplomaticAction Action) => AbilityRules.TranslateRStr(GameTables.P5WarRStr);

    public override int MiddleIcon(DiplomaticAction Action) => 0x15;

    public override void DefaultMessage(DiplomaticMessageText Text, DiplomaticActionReaction Reaction, DiplomaticRelation Relation) =>
        Text.Proposing(Relation == DiplomaticRelation.Unknown ? 151 : 100);

    public override void Sent(DiplomaticAction Action)
    {
        Action.ApplyRelationModifiers();
        AoWHSMap.Current.FPlayers.GetPlayers(Action.FTarget).PlayerSeen((sbyte)(byte)Action.FSource);
        AoWHSMap.Current.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FSource, (sbyte)(byte)Action.FTarget, DiplomaticRelation.War);
        if (Action.FSource == AoWHSMap.Current.FSeatedPlayer)
        {
            var p5WarDeclared = AbilityRules.TranslateRStr(P5WarDeclaredRStr);
            AoWHSMap.Current.ShowMessage(p5WarDeclared);
        }

        Broadcast(Action, -1, Action.FTarget);
    }

    public override void AnswerAI(DiplomaticAction Action, sbyte Player, DiplomaticRelation Relation, ByteList IdealPeace, ByteList IdealWar) =>
        Action.UpdateAIDiplomaticActionsLog(Player);

    public override int RelationModifier(DiplomaticAction Action, sbyte Player, sbyte Race)
    {
        if (Player == Action.FSource && Relation(Action.FSource, Action.FTarget) != DiplomaticRelation.War)
        {
            int Modifier = Relation(Action.FSource, Action.FTarget) == DiplomaticRelation.Alliance ? -60 : -20;
            return RaceModifier(Action.FTarget, Race, Modifier);
        }

        if (Player == Action.FTarget && Relation(Action.FSource, Action.FTarget) != DiplomaticRelation.War && AoWHSMap.Current.FPlayers.GetPlayers(Action.FSource).FPlayerRace == Race)
        {
            return -20;
        }

        return 0;
    }

    private static readonly string P5WarDeclaredRStr = "War declared";
}
