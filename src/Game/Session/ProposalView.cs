
namespace Aow.Game.Session;

// A view key of the relations window. Instances are the keys, the name serves the reports.
public sealed class ProposalView(string name)
{
    public string Name => name;

    public override string ToString() => name;

    // The window has nothing to show.
    public static ProposalView None { get; } = new("None");
    // Sent at once and announced to every player who knows both sides (declare war, break alliance).
    public static ProposalView Notice { get; } = new("Notice");
    // Sent at once to the target alone (threaten to attack).
    public static ProposalView Threat { get; } = new("Threat");
    // The sender adds gold and mana, the target accepts or rejects.
    public static ProposalView Offer { get; } = new("Offer");
    // The sender writes the text.
    public static ProposalView Message { get; } = new("Message");
}
