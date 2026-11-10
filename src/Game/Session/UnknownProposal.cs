
namespace Aow.Game.Session;

// A kind no proposal registered. It does what the default arms did and keeps the three fields.
public sealed record UnknownProposal(byte Kind, int Gold, int Mana) : DiplomaticProposal
{
    public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);
}
