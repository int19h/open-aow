
namespace Aow.Game.World.Structures;

// An id no kind registered. It does what the default arms did and keeps the three fields.
public sealed record UnknownOrder(int Id, int Parameter, int GroupId) : ProductionOrder
{
    public override (int Kind, int Parameter, int GroupId) ToFields() => (Id, Parameter, GroupId);
}
