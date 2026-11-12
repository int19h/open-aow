
namespace Aow.Game.Session;

// A kind whose sender adds gold and mana and whose target accepts or rejects it.
public abstract record OfferProposal(int Gold, int Mana) : DiplomaticProposal
{
    public override DiplomaticProposal WithOffer(int gold, int mana) => this with { Gold = gold, Mana = mana };

    public override ProposalView View => ProposalView.Offer;
}
