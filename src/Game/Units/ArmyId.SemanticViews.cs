using Aow.Engine;
using Aow.Formats.Tree;
using Aow.Game.World.Map;

namespace Aow.Game.Units;

public partial class ArmyId
{

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        int location = persister.Int32("Location", value: StoredCoordinateImage.From(Location).ToInt32());
        Location = StoredCoordinateImage.FromInt32(location).Location;
        PersistSupport.Int32List(persister, name: "UnitIds", list: UnitIds);
    }
}
