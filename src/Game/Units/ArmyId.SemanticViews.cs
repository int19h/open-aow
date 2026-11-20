using Aow.Engine;
using Aow.Formats.Tree;
using Aow.Game.World.Map;

namespace Aow.Game.Units;

public sealed partial class ArmyId
{

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = persister.UnsignedByte(name: "Player", value: Player);
        int location = persister.Int32("Location", value: StoredCoordinateImage.From(Location).ToInt32());
        Location = StoredCoordinateImage.FromInt32(location).Location;
        persister.Int32List(name: "UnitIds", list: UnitIds);
    }
}
