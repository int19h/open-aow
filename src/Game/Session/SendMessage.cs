
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Session;

// Kind 3: the source writes to the target.
public sealed partial record SendMessage : DiplomaticProposal
{
    public const byte Kind = 3;

    public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

    public override ProposalView View => ProposalView.Message;

    public override string Name(DiplomaticAction Action) => AbilityRules.TranslateRStr(P5SendMessageRStr);

    public override string LogText(DiplomaticAction Action) => AbilityRules.TranslateRStr(GameText.P5MessageRStr);

    public override void Sent(DiplomaticAction Action) => Deliver(Action);

    internal static readonly string P5SendMessageRStr = "Send Message";
}
