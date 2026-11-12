
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Session;

// Kind 3: the source writes to the target.
public sealed partial record SendMessage : DiplomaticProposal
{
    public const byte Kind = 3;

    public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

    public override ProposalView View => ProposalView.Message;

    public override string Name(DiplomaticAction action) => AbilityRules.TranslateResourceString(SendMessageText);

    public override string LogText(DiplomaticAction action) => AbilityRules.TranslateResourceString(GameText.DiplomaticMessageText);

    public override void Sent(DiplomaticAction action) => Deliver(action);

    internal static string SendMessageText { get; } = "Send Message";
}
